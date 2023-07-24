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
            var policyCheck = this.FindControl<CheckBox>("PolicyCheck");
            policyCheck.Click += EncCheck;
            var authCheck = this.FindControl<CheckBox>("AuthCheck");
            authCheck.Click += CheckAuth;
            var serverChk = this.FindControl<CheckBox>("ServerChoice");
            serverChk.Click += CheckServer;
            var clientChk = this.FindControl<CheckBox>("ClientChoice");
            clientChk.Click += CheckClient;
            var buildBtn = this.FindControl<Button>("BuildButton");
            buildBtn.Click += Build;
        }
        private void CheckServer(object? sender, RoutedEventArgs e)
        {
            //if (ServerChoice.IsChecked == true)
            //{ 
            //    //LinServer = true;
            //    ServerChoice.Content = "Lin";
            //}
            //else
            //{ 
            //    //LinServer = false;
            //    ServerChoice.Content = "Win";
            //}
        }
        private void CheckClient(object? sender, RoutedEventArgs e)
        {
            //if (ClientChoice.IsChecked == true)
            //{
            //    //LinClient = true;
            //    ClientChoice.Content = "Lin";
            //}
            //else
            //{
            //    //LinClient = false;
            //    ClientChoice.Content = "Win";
            //}
        }
        private void EncCheck(object? sender, RoutedEventArgs e)
        {
            //if (PolicyCheck.IsChecked == true)
            //    UsingEnc = true;
            //else
            //    UsingEnc = false;
        }
        private void CheckAuth(object? sender, RoutedEventArgs e)
        {
            //if (AuthCheck.IsChecked == true)
            //{ 
            //   // UsingAuth = true;
            //    PName.IsEnabled = true;
            //}
            //else
            //{ 
            //  //  UsingAuth = false;
            //    PName.IsEnabled = false;
            //}
        }
        private void Build(object? sender, RoutedEventArgs e)
        {
            //string _enc = UsingEnc ? "wss" : "ws";
            //string _auth = UsingAuth ? "true" : "false";
            //string C = LinClient ? "LinClient" : "WinClient.exe";
            //string S = LinServer ? "LinServer" : "WinServer.exe";
            //string _c = Path.GetTempFileName();
            //string _s = Path.GetTempFileName();
            //byte[] c = null;
            //byte[] s = null;
            //bool cBuild = false;
            //bool sBuild = false;
            //Ancestory ancestory = new Ancestory();
            //if (LinClient)
            //    c = ancestory.cfl;
            //else
            //    c = ancestory.cfw;
            //if (LinServer)
            //    s = ancestory.sfl;
            //else
            //    s = ancestory.sfw;
            //ModuleDefMD? clientDef = null;
            //try
            //{
            //    File.WriteAllBytes(_c, c);
            //    Task.Delay(1000);
            //    using (clientDef = ModuleDefMD.Load(_c))
            //    {
            //        if (string.IsNullOrEmpty(PName.Text))
            //            WriteSettingsC(clientDef, C, ServerPort.Text, APIExt.Text, UName.Text, ServerURL.Text);
            //        else
            //            WriteSettingsC(clientDef, C, ServerPort.Text, APIExt.Text, UName.Text, ServerURL.Text, PName.Text);
            //        clientDef.Write(C);
            //        clientDef.Dispose();
            //    }
            //    cBuild = true;
            //}
            //catch (Exception ex)
            //{
            //    clientDef?.Dispose();
            //    cBuild = false;
            //}
            //Task.Delay(1000);
            //File.Delete(_c);

            //ModuleDefMD? serverDef = null;
            //try
            //{
            //    File.WriteAllBytes(_s, s);
            //    Task.Delay(1000);
            //    using (serverDef = ModuleDefMD.Load(_s))
            //    {
            //        if (string.IsNullOrEmpty(PName.Text))
            //            WriteSettingsS(serverDef, S, ServerPort.Text, APIExt.Text, UName.Text);
            //        else
            //            WriteSettingsS(serverDef, S, ServerPort.Text, APIExt.Text, UName.Text, PName.Text);
            //        serverDef.Write(S);
            //        serverDef.Dispose();
            //        sBuild = true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    serverDef?.Dispose();
            //    sBuild = false;
            //}
            //Task.Delay(1000);
            //File.Delete(_s);
            MessageBox.Show(this, "This window is an example of a MessageBox in Avalonia", "Build Results", MessageBox.MessageBoxButtons.Ok);
            this.Close();
        }
        //private async Task WriteSettingsC(ModuleDefMD asmDef, string AsmName, string Port, string Ext, string Name, string URL, string Pass = "")
        //{
        //    try
        //    {
        //        foreach (TypeDef type in asmDef.Types)
        //        {
        //            asmDef.Assembly.Name = Path.GetFileNameWithoutExtension(AsmName);
        //            asmDef.Name = Path.GetFileName(AsmName);
        //            if (type.Name == "Lineage")
        //                foreach (MethodDef method in type.Methods)
        //                {
        //                    if (method.Body == null) continue;
        //                    for (int i = 0; i < method.Body.Instructions.Count(); i++)
        //                    {
        //                        if (method.Body.Instructions[i].OpCode == OpCodes.Ldstr)
        //                        {
        //                            if (method.Body.Instructions[i].Operand.ToString() == "%PORT%")
        //                                method.Body.Instructions[i].Operand = Port;

        //                            if (method.Body.Instructions[i].Operand.ToString() == "%SERVERURL%")
        //                                method.Body.Instructions[i].Operand = URL;

        //                            if (method.Body.Instructions[i].Operand.ToString() == "%AUTH%")
        //                                method.Body.Instructions[i].Operand = Convert.ToString(UsingAuth);

        //                            if (method.Body.Instructions[i].Operand.ToString() == "%EXT%")
        //                                method.Body.Instructions[i].Operand = Ext;

        //                            if (method.Body.Instructions[i].Operand.ToString() == "%NAME%")
        //                                method.Body.Instructions[i].Operand = Name;

        //                            if (method.Body.Instructions[i].Operand.ToString() == "%PASS%")
        //                                method.Body.Instructions[i].Operand = Pass;

        //                            if (method.Body.Instructions[i].Operand.ToString() == "%ENC%")
        //                                method.Body.Instructions[i].Operand = UsingEnc ? "wss" : "ws";

        //                        }
        //                    }
        //                }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ArgumentException("WriteSettings: " + ex.Message);
        //    }
        //}
       //private async Task WriteSettingsS(ModuleDefMD asmDef, string AsmName, string Port, string Ext, string Name, string Pass = "")
       // {
       //     try
       //     {
       //         foreach (TypeDef type in asmDef.Types)
       //         {
       //             asmDef.Assembly.Name = Path.GetFileNameWithoutExtension(AsmName);
       //             asmDef.Name = Path.GetFileName(AsmName);
       //             if (type.Name == "Lineage")
       //                 foreach (MethodDef method in type.Methods)
       //                 {
       //                     if (method.Body == null) continue;
       //                     for (int i = 0; i < method.Body.Instructions.Count(); i++)
       //                     {
       //                         if (method.Body.Instructions[i].OpCode == OpCodes.Ldstr)
       //                         {
       //                             if (method.Body.Instructions[i].Operand.ToString() == "%PORT%")
       //                                 method.Body.Instructions[i].Operand = Port;

       //                             if (method.Body.Instructions[i].Operand.ToString() == "%AUTH%")
       //                                 method.Body.Instructions[i].Operand = Convert.ToString(UsingAuth);

       //                             if (method.Body.Instructions[i].Operand.ToString() == "%EXT%")
       //                                 method.Body.Instructions[i].Operand = Ext;

       //                             if (method.Body.Instructions[i].Operand.ToString() == "%UNAME%")
       //                                 method.Body.Instructions[i].Operand = Name;

       //                             if (method.Body.Instructions[i].Operand.ToString() == "%PNAME%")
       //                                 method.Body.Instructions[i].Operand = Pass;

       //                             if (method.Body.Instructions[i].Operand.ToString() == "%ENC%")
       //                                 method.Body.Instructions[i].Operand = Convert.ToString(UsingEnc);

       //                         }
       //                     }
       //                 }
       //         }
       //     }
       //     catch (Exception ex)
       //     {
       //         throw new ArgumentException("WriteSettings: " + ex.Message);
       //     }
       // }
    }
}
