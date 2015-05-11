namespace lilMess.Client.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public interface ITranslationService
    {
        event EventHandler LanguageChanged;

        CultureInfo Language { get; set; }

        List<CultureInfo> AvaiableCultures { get; }
    }
}