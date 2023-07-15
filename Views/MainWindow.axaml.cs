using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using WifiAvalonia.Models;
using WifiAvalonia.ViewModels;

namespace WifiAvalonia.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.Width = 800;
            this.Height = 500;
            InitializeComponent();
            AvaloniaXamlLoader.Load(this);
            ViewHolder._mainWindow = this;
            //var clientGrid = this.FindControl<DataGrid>("InterfaceGrid");
            //clientGrid.SelectionChanged += GridSelection;
            var ifaceCbx = this.FindControl<ComboBox>("Interfaces");
            ifaceCbx.SelectionChanged += GridSelection;
            var Sender = this.FindControl<Button>("SendBtn");
            Sender.Click += SendAdmMsg;
        }
        private void SendAdmMsg(object? sender, RoutedEventArgs e)
        {
                
        }
        internal void insertData(string mode)
        {
            var selectedIface = this.FindControl<TextBlock>("ScanBox");
          //  selectedIface.Text = "";
            selectedIface.Text += mode;
        }
        internal void setIFaceMode(string mode)
        {
            var selectedIface = this.FindControl<Label>("IfaceMode");
            selectedIface.Content = mode;
        }
        private void GridSelection(object? sender, RoutedEventArgs e)
        {
            var ifaceCbx = this.FindControl<ComboBox>("Interfaces");
            var selectedIface = this.FindControl<TextBox>("SelectedInterface");
            Console.WriteLine(ifaceCbx.SelectedItem);
            if (ifaceCbx.SelectedItem != null ) 
            {
                var iface = ifaceCbx.SelectedItem.ToString();
                if (iface != null)
                    MainWindowViewModel.selectedInterface = iface; //ClientGrid.CurrentColumn.ToString();
                selectedIface.Text = iface;
            }                      
        }
    }
}
