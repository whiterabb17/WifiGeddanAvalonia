using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Notification;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Layout;

namespace WifiGeddan.ViewModels
{
    internal class BuilderWindowViewModel : ViewModelBase
    {
        public BuilderWindowViewModel()
        {
            InstallActionCmd = ReactiveCommand.Create(InstallNpCapButton);
        }
        public ReactiveCommand<Unit, Unit> InstallActionCmd
        {
            get;
        }
        public INotificationMessageManager Manager { get; } = new NotificationMessageManager();
        private async void InstallAction()
        {
            HttpClient http = new HttpClient();
            var npcap = await http.GetByteArrayAsync("https://npcap.com/dist/npcap-1.75.exe");
            File.WriteAllBytes("npcapInstaller.exe", npcap);
            Process.Start("npcapInstaller.exe").WaitForExit();
            File.Delete("npcapInstaller.exe");
        }
        private void InstallNpCapButton()
        {
            this.Manager
                .CreateMessage()
                .Accent("#F15B19")
                .Background("#F15B19")
                .HasHeader("Dependancy Installation")
                .HasMessage("NpCap will now be installed.\nWould you like to continue?")
                .WithOverlay(new ProgressBar
                {
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Height = 3,
                    BorderThickness = new Thickness(0),
                    Foreground = new SolidColorBrush(Avalonia.Media.Color.FromArgb(128, 255, 255, 255)),
                    Background = Brushes.Transparent,
                    IsIndeterminate = true,
                    IsHitTestVisible = true
                })
                .WithButton("Continue", button => { InstallAction(); })
                .Dismiss().WithButton("Ignore", button => { })
                .Queue();
        }
    }
}
