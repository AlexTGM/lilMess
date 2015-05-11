namespace lilMess.Client.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Windows;

    public class TranslationService : ITranslationService
    {
        private const string ResFolder = "Resources/Translations";

        public TranslationService()
        {
            LanguageChanged += AppLanguageChanged;

            AvaiableCultures = new List<CultureInfo> { new CultureInfo("en-US"), new CultureInfo("ru-RU") };

            Language = Properties.Settings.Default.DefaultLanguage;
        }

        public event EventHandler LanguageChanged;

        public List<CultureInfo> AvaiableCultures { get; private set; }

        public CultureInfo Language
        {
            get
            {
                return Thread.CurrentThread.CurrentUICulture;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (Equals(value, Thread.CurrentThread.CurrentUICulture))
                {
                    return;
                }

                Thread.CurrentThread.CurrentUICulture = value;

                var dictionary = AvaiableCultures.Contains(value)
                                     ? new ResourceDictionary { Source = new Uri(string.Format("{0}/lang.{1}.xaml", ResFolder, value.Name), UriKind.Relative) }
                                     : new ResourceDictionary { Source = new Uri(string.Format("{0}/lang.{1}.xaml", ResFolder, "en-US"), UriKind.Relative) };

                var merged = Application.Current.Resources.MergedDictionaries;

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

                if (LanguageChanged != null)
                {
                    LanguageChanged(Application.Current, new EventArgs());
                }
            }
        }

        private void AppLanguageChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.DefaultLanguage = Language;
            Properties.Settings.Default.Save();
        }
    }
}