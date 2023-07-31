using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;
using System.Collections.Generic;

namespace WifiGeddan.ViewModels;

public class Interfaces
{
    public string? Name
    {
        get; set;
    }
    public string? Type
    {
        get; set;
    }
    public string? Status
    {
        get; set;
    }
    public long BytesRecieved
    {
        get; set;
    }
    public long BytesSent
    {
        get; set;
    }
    //    public ICollection<SampleOrderDetail> Details { get; set; }
}
public class NetInterfaces
{
    public string? Enabled
    {
        get; set;
    }
    public string? State
    {
        get; set;
    }
    public string? Name
    {
        get; set;
    }
    public string? Mode
    {
        get; set;
    }
    //    public ICollection<SampleOrderDetail> Details { get; set; }
}
public class ViewLocator : IDataTemplate
{
    public Control Build(object data)
    {
        var name = data.GetType().FullName!.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }
        else
        {
            return new TextBlock { Text = "Not Found: " + name };
        }
    }

    public bool Match(object data)
    {
        return data is ViewModelBase;
    }
}
