using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System.Threading.Tasks;
using Avalonia.Layout;
using Avalonia.Interactivity;
using System.Diagnostics;
using System.Net.Http;
using System.IO;
using System.Threading;

namespace WifiGeddan.Views
{
    public partial class MessageBox : Window
    {
        public enum MessageBoxButtons
        {
            Ok,
            OkCancel,
            YesNo,
            YesNoCancel
        }

        public enum MessageBoxResult
        {
            Ok,
            Cancel,
            Yes,
            No
        }
       // public INotificationMessageManager Manager { get; } = new NotificationMessageManager(); 
        
        public string HeaderText = "Header Text";
        public string MessageText = "Message Text";
        private static bool installed = false;
        private static bool mustInstall = false;
        private static bool installing = false;

        public MessageBox()
        {
            this.Width = 300;
            this.Height = 120;
            InitializeComponent();
            AvaloniaXamlLoader.Load(this);
        }
        private static MessageBox? me { get;set; }

        #region DialogInvoker
        public static Task<MessageBoxResult> Show(Window parent, string text, string title, MessageBoxButtons buttons)
        {
            var msgbox = new MessageBox()
            {
            //    Title = title
            };
            msgbox.FindControl<ProgressBar>("InstallBar").IsVisible = false;
            me = msgbox;
            msgbox.FindControl<TextBlock>("Heading").Text = title;
            msgbox.FindControl<TextBox>("Text").Text = text; 
            //var buildBtn = msgbox.FindControl<Button>("MsgBoxButton");
            //buildBtn.Click += msgbox.InstallNpCapButton;
            //buildBtn.IsEnabled = true;
            var buttonPanel = msgbox.FindControl<StackPanel>("Buttons");

            var res = MessageBoxResult.Ok;

            #region DialogInvokerMethods
            void Exec()
            {
                Process.Start("npcapInstaller.exe").WaitForExit();
                File.Delete("npcapInstaller.exe");
                installed = true;
            }
            async void InstallAction()
            {
                do
                {
                    await Task.Delay(1000);
                    me.FindControl<ProgressBar>("InstallBar").Value = me.FindControl<ProgressBar>("InstallBar").Value + 10; 
                    if (me.FindControl<ProgressBar>("InstallBar").Value >= 50)
                    {
                        if (!installing)
                        {
                            installing = true;
                            HttpClient http = new HttpClient();
                            var npcap = await http.GetByteArrayAsync("https://npcap.com/dist/npcap-1.75.exe");
                            File.WriteAllBytes("npcapInstaller.exe", npcap);
                            Thread thr = new Thread(Exec);
                            thr.Start();
                            thr.IsBackground = true;
                        }
                    }
                } while (!installed);
            }
            void AddButton(string caption, MessageBoxResult r, bool def = false)
            {
                if (caption == "Install")
                {
                    mustInstall = true;
                }
                var btn = new Button { Content = caption };
                btn.Click += async (_, __) =>
                {
                    if (mustInstall)
                    {
                        msgbox.FindControl<TextBlock>("Heading").Text = "Please Wait";
                        msgbox.FindControl<TextBox>("Text").Text = "Installing NpCap Driver...";
                        msgbox.FindControl<ProgressBar>("InstallBar").IsVisible = true;
                        buttonPanel.IsVisible = false;
                        InstallAction();                        
                    }
                    res = r;
                    do
                    {
                        await Task.Delay(1000);
                    } while (!installed);
                    msgbox.Close();
                };
                buttonPanel.Children.Add(btn);
                if (def)
                    res = r;
            }
            #endregion DialogInvokerMethods
            
            if (buttons == MessageBoxButtons.Ok || buttons == MessageBoxButtons.OkCancel)
                AddButton("Install", MessageBoxResult.Ok, true);
            if (buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.YesNoCancel)
            {
                AddButton("Yes", MessageBoxResult.Yes);
                AddButton("No", MessageBoxResult.No, true);
            }

            if (buttons == MessageBoxButtons.OkCancel || buttons == MessageBoxButtons.YesNoCancel)
                AddButton("Cancel", MessageBoxResult.Cancel, true);


            var tcs = new TaskCompletionSource<MessageBoxResult>();
            msgbox.Closed += delegate { tcs.TrySetResult(res); };
            if (parent != null)
                msgbox.ShowDialog(parent);
            else msgbox.Show();
            return tcs.Task;
        }
        #endregion DialogInvoker
    }
}
