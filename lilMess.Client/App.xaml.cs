namespace lilMess.Client
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Threading;

    using lilMess.Tools;

    public partial class App
    {
        private const string ResFolder = "Resources/Translations";

        public static event EventHandler LanguageChanged;

        KeyboardListener KListener = new KeyboardListener();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            KListener.KeyDown += new RawKeyEventHandler(KListener_KeyDown);
        }

        void KListener_KeyDown(object sender, RawKeyEventArgs args)
        {
            Console.WriteLine(args.Key.ToString());
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            KListener.Dispose();
        }

        public App()
        {
            Dispatcher.UnhandledException += OnDispatcherUnhandledException;

            LanguageChanged += App_LanguageChanged;

            AvaiableCultures = new List<CultureInfo>();

            Language = Thread.CurrentThread.CurrentUICulture;

            AvaiableCultures = new List<CultureInfo> { new CultureInfo("en-US"), new CultureInfo("ru-RU") };
        }

        public static List<CultureInfo> AvaiableCultures { get; private set; }

        public static CultureInfo Language
        {
            get { return Thread.CurrentThread.CurrentUICulture; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (Equals(value, Thread.CurrentThread.CurrentUICulture)) return;

                Thread.CurrentThread.CurrentUICulture = value;

                var dictionary = AvaiableCultures.Contains(value)
                                     ? new ResourceDictionary { Source = new Uri(string.Format("{0}/lang.{1}.xaml", ResFolder, value.Name), UriKind.Relative) }
                                     : new ResourceDictionary { Source = new Uri(string.Format("{0}/lang.{1}.xaml", ResFolder, "en-US"), UriKind.Relative) };

                var merged = Current.Resources.MergedDictionaries;

                var oldDictionary = merged.FirstOrDefault(dict => dict.Source != null && dict.Source.OriginalString.StartsWith(ResFolder + "lang."));

                if (oldDictionary != null)
                {
                    var ind = merged.IndexOf(oldDictionary);
                    merged.Remove(oldDictionary);
                    merged.Insert(ind, dictionary);
                }
                else
                {
                    merged.Add(dictionary);
                }

                LanguageChanged(Current, new EventArgs());
            }
        }

        private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var errorMessage = string.Format("An unhandled exception occurred: {0}", e.Exception.Message);

            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            e.Handled = true;
        }

        private void ApplicationLoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Language = Client.Properties.Settings.Default.DefaultLanguage;
        }

        private void App_LanguageChanged(object sender, EventArgs e)
        {
            Client.Properties.Settings.Default.DefaultLanguage = Language;
            Client.Properties.Settings.Default.Save();
        }
    }
}