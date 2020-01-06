using System;
using System.IO;
using IssueHub.Middlewares;
using IssueHub.Models;
using IssueHub.Pages;
using IssueHub.States;
using ReduxSharp;
using SQLite;
using ValueTaskSupplement;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace IssueHub
{
    public partial class App : Application
    {
        public IStore<AppState> Store { get; }

        public App()
        {
            InitializeComponent();

            ModalPopped += HandleModalPopped;

            SQLitePCL.Batteries_V2.Init();

            var lazyConnection = ValueTaskEx.Lazy(async () =>
            {
                var dbPath = Path.Combine(
                    FileSystem.AppDataDirectory,
                    "IssueHub.sqlite");
                var connection = new SQLiteAsyncConnection(dbPath);
                await connection.CreateTableAsync<Favorite>();
                return connection;
            });

            Store = new Store<AppState>(
                new AppState.AppReducer(),
                new AppState(
                    new FavoritesRepository(lazyConnection)),
                new AutoLoginMiddleware());

            MainPage = new MainPage(Store)
                .WrapNavigationPage();
        }

        void HandleModalPopped(object sender, ModalPoppedEventArgs e)
        {
            (e.Modal.BindingContext as IDisposable)?.Dispose();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
