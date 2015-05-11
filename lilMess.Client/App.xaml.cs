namespace lilMess.Client
{
    using System.Windows;
    using System.Windows.Threading;

    using lilMess.Tools;

    public partial class App
    {
        public static readonly KeyboardListener KeyboardListener = new KeyboardListener();

        public App()
        {
            Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var errorMessage = string.Format("An unhandled exception occurred: {0}", e.Exception.Message);

            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            e.Handled = true;
        }

        private void ApplicationExit(object sender, ExitEventArgs e)
        {
            KeyboardListener.Dispose();
        }
    }
}