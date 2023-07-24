using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Material.Colors;
using Material.Styles.Themes;
using WifiGeddan.ViewModels;
using WifiGeddan.Views;
using Material.Styles.Themes;
using Material.Styles.Themes.Base;

namespace WifiGeddan
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }
            base.OnFrameworkInitializationCompleted();
        //    var primary = PrimaryColor.DeepPurple;
        //    var primaryColor = SwatchHelper.Lookup[(MaterialColor)primary];

        //    var secondary = SecondaryColor.Orange;
        //    var secondaryColor = SwatchHelper.Lookup[(MaterialColor)secondary];

        //    var theme = Theme.Create(Theme.Dark, primaryColor, secondaryColor);
        //    var themeBootstrap = this.LocateMaterialTheme<MaterialThemeBase>();
        //    themeBootstrap.CurrentTheme = theme;
        }
    }
}
