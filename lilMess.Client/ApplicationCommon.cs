namespace lilMess.Client
{
    using lilMess.Client.Services;

    public class ApplicationCommon
    {
        private readonly IUserService _userService;

        private readonly ITranslationService _translationService;

        private readonly IThemeService _themeService;

        public ApplicationCommon(IUserService userService, ITranslationService translationService, IThemeService themeService)
        {
            _userService = userService;
            _translationService = translationService;
            _themeService = themeService;
        }

        public IUserService UserService
        {
            get { return _userService; }
        }

        public IThemeService ThemeService
        {
            get { return _themeService; }
        }

        public ITranslationService TranslationService
        {
            get { return _translationService; }
        }
    }
}