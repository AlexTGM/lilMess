using System;
using System.Threading;
using System.Windows;

namespace lilMessServer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Network.Initialise(DisplayMessage);
            Network.StartServer();
        }

        private void DisplayMessage(string message)
        {
            Dispatcher.BeginInvoke(new ThreadStart(
                () => Output.AppendText(":" + DateTime.Now + ":\n" + message + Environment.NewLine)));            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Network.Shutdown();
        }
    }
}