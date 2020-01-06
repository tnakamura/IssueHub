using System;
using System.Reactive.Disposables;

namespace IssueHub.ViewModels
{
    public abstract class PageViewModel : ViewModelBase, IDisposable
    {
        protected PageViewModel()
            : base()
        {
        }

        string pageTitle;

        public string PageTitle
        {
            get => pageTitle;
            set => SetProperty(ref pageTitle, value);
        }

        public event EventHandler RequestPopModal;

        protected void RaiseRequestPopModal()
        {
            RequestPopModal?.Invoke(this, EventArgs.Empty);
        }

        readonly CompositeDisposable disposables = new CompositeDisposable();

        protected void AddDisposable(IDisposable disposable) =>
            disposables.Add(disposable);

        public void Dispose()
        {
            disposables.Dispose();
        }
    }
}
