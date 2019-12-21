namespace Store.UI.Startup
{
    using Autofac;
    using Prism.Events;
    using Store.DataAccess;
    using Store.UI.DateProvider;
    using Store.UI.Dialogs;
    using Store.UI.View;
    using Store.UI.ViewModel;

    public class Bootstraper
    {
        public IContainer BootStrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<HumanEditViewModel>().As<IHumanEditViewModel>();
            builder.RegisterType<NavigationViewModel>().As<InavigationViewModel>();
            builder.RegisterType<HumanDataProvider>().As<IHumanDataProvider>();
            builder.RegisterType<NavigationDataProvider>().As<INavigationDataProvider>();
            builder.RegisterType<FileDataService>().As<IDataService>();
            return builder.Build();
        }
    }
}