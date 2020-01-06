using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using IssueHub.Actions;
using IssueHub.States;
using IssueHub.Utils;
using Reactive.Bindings;
using ReduxSharp;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace IssueHub.ViewModels
{
    public sealed class LabelsPageViewModel : PageViewModel
    {
        readonly IStore<AppState> store;

        public LabelsPageViewModel(IStore<AppState> store)
            : base()
        {
            this.store = store;

            Labels = new ObservableCollection<LabelViewModel>();

            IsLabelsLoaded = new ReactivePropertySlim<bool>(
                store.State.Labels.Initialized);

            InitializeCommand = new AsyncReactiveCommand()
                .WithSubscribe(InitializeAsync, AddDisposable);
            RefreshCommand = new AsyncReactiveCommand()
                .WithSubscribe(RefreshAsync, AddDisposable);
            ToggleSelectCommand = new Command<LabelViewModel>(ToggleSelect);

            AddDisposable(
                store.DistinctUntilChanged(x => (x.Labels.Labels, x.IssueForm.Labels))
                    .Subscribe(x =>
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            IsLabelsLoaded.Value = x.Labels.Initialized;
                            Labels.Merge(
                                ToViewModels(x),
                                LabelViewModel.EqualityComparer.Default);
                        });
                    }));
        }

        static IEnumerable<LabelViewModel> ToViewModels(AppState state)
        {
            foreach (var label in state.Labels.Labels)
            {
                yield return new LabelViewModel(label)
                {
                    IsSelected = state.IssueForm.Labels.Any(x => x.Name == label.Name),
                };
            }
        }

        public IReactiveProperty<bool> IsRefreshing { get; } = new ReactivePropertySlim<bool>(false);

        public IReactiveProperty<bool> IsLoading { get; } = new ReactivePropertySlim<bool>(false);

        public IReactiveProperty<bool> IsLabelsLoaded { get; }

        public ObservableCollection<LabelViewModel> Labels { get; }

        public AsyncReactiveCommand InitializeCommand { get; }

        public AsyncReactiveCommand RefreshCommand { get; }

        public Command<LabelViewModel> ToggleSelectCommand { get; }

        void ToggleSelect(LabelViewModel label)
        {
            if (label.IsSelected)
            {
                store.Dispatch(new DeselectLabel(label));
            }
            else
            {
                store.Dispatch(new SelectLabel(label));
            }
        }

        Task InitializeAsync() => LoadCoreAsync(IsLoading);

        Task RefreshAsync() => LoadCoreAsync(IsRefreshing);

        async Task LoadCoreAsync(IReactiveProperty<bool> isBusy)
        {
            try
            {
                isBusy.Value = true;
                await store.DispatchAsync(
                    IssueFormActions.LoadLabels(
                        store.State.Issues.Repository.Owner.Login,
                        store.State.Issues.Repository.Name));
            }
            finally
            {
                isBusy.Value = false;
            }
        }
    }

    public sealed class LabelViewModel : Octokit.Label, INotifyPropertyChanged
    {
        public LabelViewModel(Octokit.Label label)
            : base()
        {
            Id = label.Id;
            Name = label.Name;
        }

        bool isSelected;

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsSelected)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal sealed class EqualityComparer : IEqualityComparer<LabelViewModel>
        {
            public static readonly EqualityComparer Default = new EqualityComparer();
            EqualityComparer() { }

            public bool Equals(LabelViewModel x, LabelViewModel y)
            {
                return x.Id == y.Id &&
                    x.Name == y.Name &&
                    x.IsSelected == y.IsSelected;
            }

            public int GetHashCode(LabelViewModel obj)
            {
                return obj.Id.GetHashCode() ^ obj.Name.GetHashCode();
            }
        }
    }
}
