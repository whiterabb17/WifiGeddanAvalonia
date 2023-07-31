/*
ExifGlass - Standalone Exif tool for ImageGlass
Copyright (C) 2023 DUONG DIEU PHAP
Project homepage: https://github.com/d2phap/ExifGlass

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using ReactiveUI;
using Splat.ModeDetection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WifiGeddan.Models;
using WifiGeddan.ViewModels;

namespace WifiGeddan;

public partial class MainWindow : StyledWindow
{
    public MainWindow()
    {
        this.Width = 800;
        this.Height = 760;
        InitializeComponent();
        AvaloniaXamlLoader.Load(this);
        ViewHolder._mainWindow = this;
        //var clientGrid = this.FindControl<DataGrid>("InterfaceGrid");
        //clientGrid.SelectionChanged += GridSelection;
        var ifaceCbx = this.FindControl<ComboBox>("Interfaces");
        ifaceCbx.SelectionChanged += GridSelection;
        //SetWorkingSystem();
        //LogList = new ObservableCollection<Logs>(GenerateInitialLogTable());
        //RouterTable = new ObservableCollection<AiroDumpRouters>(GenerateAiroRouterTable());
        //DeviceTable = new ObservableCollection<AiroDumpDevices>(GenerateAiroDeviceTable());
        //AvailableInterfaces = new ObservableCollection<string>(GenerateInterfaceChoices());
        //EnableMonMode = ReactiveCommand.Create(EnableMonitorMode);
        //DisableMonMode = ReactiveCommand.Create(DisableMonitorMode);
        //EnableInterface = ReactiveCommand.Create(EnableAdapter);
        //DisableInterface = ReactiveCommand.Create(DisableAdapter);
        //GetIFaceMode = ReactiveCommand.Create(GetAdapterMode);
        //StartAiroDumpScan = ReactiveCommand.Create(AiroDumpThread);
        //InterfaceList = new ObservableCollection<NetInterfaces>(GenerateInterfaceInfoTable());
        ////         if (WorkingOS == "Windows")
        ////	InterfaceList = new ObservableCollection<NetInterfaces>(GenerateInterfaceInfoTable());
        ////else
        ////	InterfaceList = new ObservableCollection<NetInterfaces>(GenerateInterfaceCollection());
        //depCheck();
        //var Sender = this.FindControl<Button>("SendBtn");
        //Sender.Click += StartAiroScan;
    }
    internal void InsertInterface(NetInterfaces iface, string mode)
    {
        MainWindowViewModel._Main.InterfaceList.Clear();
        MainWindowViewModel._Main.InterfaceList.Add(
            new NetInterfaces()
            {
                Enabled = iface.Enabled,
                State = iface.State,
                Name = iface.Name,
                Mode = mode.ToString().Replace("\r\n", "")
            });
    }
    public async void InsertLog(Logs log)
    {
        MainWindowViewModel._Main.LogList.Add(
                new Logs()
                {
                    LogTime = log.LogTime,
                    LogData = log.LogData
                });
    }
    public async void ClearIfaces()
    {
        MainWindowViewModel._Main.LogList.Clear();
    }
    internal void StartAiroScan(string mode)
    {
        var Sender = this.FindControl<Button>("AiroButton");
        Sender.Content = mode + " Airodump";
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
        if (ifaceCbx.SelectedItem != null)
        {
            var iface = ifaceCbx.SelectedItem.ToString();
            if (iface != null)
                MainWindowViewModel.selectedInterface = iface; //ClientGrid.CurrentColumn.ToString();
            selectedIface.Text = iface;
        }
    }

    #region MainWindowViewModel
        
    #endregion MainWindowViewModel
}