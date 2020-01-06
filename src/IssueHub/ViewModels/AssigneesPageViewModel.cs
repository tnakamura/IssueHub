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
using Octokit;
using Reactive.Bindings;
using ReduxSharp;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace IssueHub.ViewModels
{
    public class AssigneesPageViewModel : PageViewModel
    {
        readonly IStore<AppState> store;

        public AssigneesPageViewModel(IStore<AppState> store)
            : base()
        {
            this.store = store;

            Assignees = new ObservableCollection<AssigneeViewModel>();

            IsAssigneesLoaded = new ReactivePropertySlim<bool>(
                store.State.Assignees.Initialized);

            InitializeCommand = new AsyncReactiveCommand()
                .WithSubscribe(InitializeAsync, AddDisposable);
            RefreshCommand = new AsyncReactiveCommand()
                .WithSubscribe(RefreshAsync, AddDisposable);
            ToggleSelectCommand = new Command<AssigneeViewModel>(ToggleSelect);

            AddDisposable(
                store.DistinctUntilChanged(x => (x.Assignees.Assignees, x.IssueForm.Assignees))
                    .Subscribe(x =>
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            IsAssigneesLoaded.Value = x.Assignees.Initialized;
                            Assignees.Merge(
                                ToViewModels(x),
                                AssigneeViewModel.EqualityComparer.Default);
                        });
                    }));
        }

        static IEnumerable<AssigneeViewModel> ToViewModels(AppState state)
        {
            foreach (var assignee in state.Assignees.Assignees)
            {
                yield return new AssigneeViewModel(assignee)
                {
                    IsSelected = state.IssueForm.Assignees.Any(x => x.Login == assignee.Login),
                };
            }
        }

        public IReactiveProperty<bool> IsLoading { get; } = new ReactivePropertySlim<bool>(false);

        public IReactiveProperty<bool> IsRefreshing { get; } = new ReactivePropertySlim<bool>(false);

        public IReactiveProperty<bool> IsAssigneesLoaded { get; }

        public AsyncReactiveCommand InitializeCommand { get; }

        public AsyncReactiveCommand RefreshCommand { get; }

        public ObservableCollection<AssigneeViewModel> Assignees { get; }

        public Command<AssigneeViewModel> ToggleSelectCommand { get; }

        void ToggleSelect(AssigneeViewModel assignee)
        {
            if (assignee.IsSelected)
            {
                store.Dispatch(new DeselectAssignee(assignee));
            }
            else
            {
                store.Dispatch(new SelectAssignee(assignee));
            }
        }

        Task InitializeAsync() =>
            LoadCoreAsync(IsLoading);

        Task RefreshAsync() =>
            LoadCoreAsync(IsRefreshing);

        async Task LoadCoreAsync(IReactiveProperty<bool> isBusy)
        {
            try
            {
                isBusy.Value = true;
                await store.DispatchAsync(
                    IssueFormActions.LoadAssignees(
                        store.State.Issues.Repository.Owner.Login,
                        store.State.Issues.Repository.Name));
            }
            finally
            {
                isBusy.Value = false;
            }
        }
    }

    public sealed class AssigneeViewModel : User, INotifyPropertyChanged
    {
        public AssigneeViewModel(User assignee)
            : base()
        {
            Id = assignee.Id;
            Login = assignee.Login;
            Name = assignee.Name;
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

        internal sealed class EqualityComparer : IEqualityComparer<AssigneeViewModel>
        {
            public static readonly EqualityComparer Default = new EqualityComparer();

            EqualityComparer() { }

            public bool Equals(AssigneeViewModel x, AssigneeViewModel y)
            {
                return x.Id == y.Id &&
                    x.Login == y.Login &&
                    x.IsSelected == y.IsSelected;
            }

            public int GetHashCode(AssigneeViewModel obj)
            {
                return obj.Login.GetHashCode();
            }
        }
    }
}
