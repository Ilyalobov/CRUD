using System.Windows;

namespace Store.UI
{
    using Autofac;
    using Store.UI.Startup;
    using Store.UI.View;

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var bootStraper = new Bootstraper();
            var container = bootStraper.BootStrap();
            var mainWIndow = container.Resolve<MainWindow>();
            mainWIndow.Show();
        }
    }
}
