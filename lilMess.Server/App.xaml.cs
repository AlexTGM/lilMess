namespace lilMess.Server
{
    using System.Windows;
    using System.Windows.Threading;

    public partial class App : Application
    {
        public App()
        {
            this.Dispatcher.UnhandledException += this.OnDispatcherUnhandledException;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMessage = string.Format("An unhandled exception occurred: {0}", e.Exception.Message);
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}