using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace lilMessClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Network.Initialise(DisplayMessage);
        }

        private void DisplayMessage(string message)
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                ChatBox.AppendText(":" + DateTime.Now + ":\n" +
                    message + Environment.NewLine);
            }));
        }

        private void Message_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Network.Send(Message.Text);
                Message.Text = string.Empty;
            }
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            Network.Connect("127.0.0.1", 9997);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Network.Shutdown();
        }
    }
}