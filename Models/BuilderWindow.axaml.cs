using Avalonia.Markup.Xaml;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace WifiGeddan.Views
{
    public partial class BuilderWindow : Window
    {
        //private static bool UsingAuth = false;
        //private static bool UsingEnc = false;
        //private static bool LinServer = false;
        //private static bool LinClient = false;
        public BuilderWindow()
        {
            this.Width = 500;
            this.Height = 270;
            InitializeComponent();
            AvaloniaXamlLoader.Load(this);
            
        }
    }
}
