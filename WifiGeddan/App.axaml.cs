using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using System.Globalization;
using System.Threading.Tasks;
using System;
using WifiGeddan.ViewModels;

namespace WifiGeddan;

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
            // load user configs
            

            if (Current != null)
            {
                // load Theme mode
                var themeVariant = ThemeVariant.Default;
            //    if (Config.ThemeMode == ThemeMode.Dark)
            //    {
                    themeVariant = ThemeVariant.Dark;
            //    }
            //    else if (Config.ThemeMode == ThemeMode.Light)
            //    {
            //        themeVariant = ThemeVariant.Light;
            //    }

                Current.RequestedThemeVariant = themeVariant;
            }


            desktop.MainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(),
            };
            desktop.ShutdownRequested += Desktop_ShutdownRequested;
        }

        base.OnFrameworkInitializationCompleted();
    }


    private void Desktop_ShutdownRequested(object? sender, ShutdownRequestedEventArgs e)
    {
       
    }
}
