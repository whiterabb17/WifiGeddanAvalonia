using Avalonia.Interactivity;
using Avalonia.Threading;
using WifiAvalonia.Models;
using WifiAvalonia.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Avalonia.Controls;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Http;
using System.Runtime.InteropServices;

namespace WifiAvalonia.ViewModels
{
	public class MainWindowViewModel : ViewModelBase
	{
		public ReactiveViewModel ReactiveView { get; } = new ReactiveViewModel();
		public MainWindowViewModel()
		{
			_Main = this;
			SetWorkingSystem();
			LogList = new ObservableCollection<Logs>(GenerateInitialLogTable());
			AvailableInterfaces = new ObservableCollection<string>(GenerateInterfaceChoices());
			EnableMonMode = ReactiveCommand.Create(EnableMonitorMode);
			DisableMonMode = ReactiveCommand.Create(DisableMonitorMode);
			EnableInterface = ReactiveCommand.Create(EnableAdapter);
			DisableInterface = ReactiveCommand.Create(DisableAdapter);
			GetIFaceMode = ReactiveCommand.Create(GetAdapterMode);
			StartAiroDumpScan = ReactiveCommand.Create(AiroDumpThread);
		//	IFaceMode = GetInterfaceMode();
			if (WorkingOS == "Windows")
				InterfaceList = new ObservableCollection<NetInterfaces>(GenerateInterfaceInfoTable());
			else 
				InterfaceList = new ObservableCollection<NetInterfaces>(GenerateInterfaceCollection());
			//  StartUp();
			//  GenerateCounts();
			//if (ViewHolder._mainWindow != null)
			//	checkForDeps(ViewHolder._mainWindow);
		}

		#region ReactiveCommands
		public ReactiveCommand<Unit, Unit> EnableMonMode { get; }
		public ReactiveCommand<Unit, Unit> DisableMonMode { get; }
		public ReactiveCommand<Unit, Unit> EnableInterface { get; }
		public ReactiveCommand<Unit, Unit> DisableInterface { get; }
		public ReactiveCommand<Unit, Unit> GetIFaceMode { get; }
		public ReactiveCommand<Unit, Unit> StartAiroDumpScan { get; }
		#endregion ReactiveCommands

		#region Variables
		public static string? IFaceMode { get;set; }
		public static MainWindowViewModel? _Main { get; set; }
		//private static Random random = new Random();
		private static Thread? AiroThread
		{
			get; set;
		}
		private static Thread? LogCollectionThread
		{
			get; set;
		}
		private void StartAiroDump()
		{
			AiroThread = new Thread(AiroDumpThread);
			AiroThread.Start();
			AiroThread.IsBackground = true;
		}
		public ObservableCollection<NetInterfaces> InterfaceList { get; }
		public ObservableCollection<Logs> LogList { get; }
		public ObservableCollection<string> AvailableInterfaces { get; }
		internal static string selectedInterface = "";
		private static string? WorkingOS
		{
			get; set;
		}
		#endregion Variables
		#region Initializers
		private void SetWorkingSystem()
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				WorkingOS = "Windows";
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
				WorkingOS = "Linux";
			Console.WriteLine($"\n\n\n{WorkingOS}\n\n\n");
			//ViewHolder._mainWindow.ClientCount.Text = "Clients: 0";
		}
		

		private async void ShowError(Window window)
		{
			await MessageBox.Show(window, "Npcap Driver is required but is not installed\nWill Attempt to install now.", "Build Results", MessageBox.MessageBoxButtons.Ok);
			HttpClient http = new HttpClient();
			var npcap = await http.GetByteArrayAsync("https://npcap.com/dist/npcap-1.75.exe");
			File.WriteAllBytes("npcapInstaller.exe", npcap);
			Process.Start("npcapInstaller.exe").WaitForExit();
			File.Delete("npcapInstaller.exe");
			//await Dispatcher.UIThread.InvokeAsync(() => ShowIFaceMode(_mode), DispatcherPriority.Background);
		}

		private async void checkForDeps(Window window)
		{
			if (WorkingOS == "Windows")
			{
				if (!File.Exists("C:\\Windows\\System32\\Npcap\\wlanhelper.exe"))
					ShowError(window);
			}
			else
			{
				Console.WriteLine("\n\n\nLINUX SYSTEM\n\n\n");
			//	var root = runAndReturn("whoami", "", "");
			//	if (root.ToString() != "root")
			//		await MessageBox.Show(window, "Must be run as root", "Linux Permission Error", MessageBox.MessageBoxButtons.Ok);
			}
		}

		#region Interface Class Filler
		private IEnumerable<NetInterfaces> GenerateInterfaceCollection()
		{
			var interfaceList = new List<NetInterfaces>();
			foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
			{
				string mon = "";
				if (ni.Name.Contains("mon"))
				{
					mon = "monitor";
				}
				var iface = new NetInterfaces()
				{
					Enabled = "Enabled",
					State = "Online",
					Name = ni.Name,
					Mode = mon
				};
				interfaceList.Add(iface);
			}
			return interfaceList;
		}

		private List<NetInterfaces> GetNewInterfaceCollection()
		{
			var interfaceList = new List<NetInterfaces>();
			foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
			{
				string mon = "";
				if (ni.Name.Contains("mon"))
					mon = "monitor";
				var iface = new NetInterfaces()
				{
					Enabled = "Enabled",
					State = "Online",
					Name = ni.Name,
					Mode = mon
				};
				interfaceList.Add(iface);
			}
			return interfaceList;
		}
		#endregion Interface Class Filler
		private void EnableMonitorMode()
		{
			var mode = "managed";
			if (WorkingOS == "Windows")
			{
				runAndReturn("C:\\Windows\\System32\\Npcap\\wlanhelper.exe", "C:\\Windows\\System32\\Npcap", $"\"{selectedInterface}\" mode managed");
				mode = runAndReturn("C:\\Windows\\System32\\Npcap\\wlanhelper.exe", "C:\\Windows\\System32\\Npcap", $"\"{selectedInterface}\" mode");
			}
			else if (WorkingOS == "Linux")
			{
				runAndReturn("airmon-ng", "", $"start \"{selectedInterface}\"");
				mode = "monitor";
			}
			var _ilist = interfaceList();
			var newname = "";
			foreach (var _i in _ilist)
			{
				if (_i.Name == selectedInterface || _i.Name != null && _i.Name.Contains("mon"))
				{
					newname = _i.Name;
					var selIface = new NetInterfaces()
					{
						Enabled = _i.Enabled,
						Name = _i.Name,
						State = _i.State,
						Mode = mode.ToString().Replace("\r\n", "")
					};
					runAndReturn("sudo ifconfig", "", $"{_i.Name} up");
					InsertIface(selIface, mode);
				}
			}
			var log = new Logs
			{
				LogTime = DateTime.Now.ToLongTimeString(),
				LogData = "Monitor mode has been enabled on " + newname
			};
			InsertLog(log);
		}

		private void DisableMonitorMode()
		{
			var mode = "monitor";
			if (WorkingOS == "Windows")
			{
				runAndReturn("C:\\Windows\\System32\\Npcap\\wlanhelper.exe", "C:\\Windows\\System32\\Npcap", $"\"{selectedInterface}\" mode managed");
				mode = runAndReturn("C:\\Windows\\System32\\Npcap\\wlanhelper.exe", "C:\\Windows\\System32\\Npcap", $"\"{selectedInterface}\" mode");
			}
			else if (WorkingOS == "Linux")
			{
				runAndReturn("sudo airmon-ng", "", $"stop \"{selectedInterface}\"");
				mode = "managed";
			}
			var _ilist = interfaceList();
			foreach (var _i in _ilist)
			{
				if (_i.Name == selectedInterface)
				{
					var selIface = new NetInterfaces()
					{
						Enabled = _i.Enabled,
						Name = _i.Name,
						State = _i.State,
						Mode = mode.ToString().Replace("\r\n", "")
					};
					InsertIface(selIface, mode);
				}
			}
			var log = new Logs
			{
				LogTime = DateTime.Now.ToLongTimeString(),
				LogData = "Monitor mode has been disabled on " + selectedInterface
			};
			InsertLog(log);
		}
		private void AiroDumpThread()
		{
			if (WorkingOS == "Linux")
			{
				//ensureFile("airodump.buf", "", true);    "top", "", "")); 
				AiroThread = new Thread(() => runAndRead("airodump-ng", "", $"--enc wpa {selectedInterface} -w airodump --output-format csv --write-interval 10"));
				AiroThread.Start();
				AiroThread.IsBackground = true;
				var log = new Logs
				{
					LogTime = DateTime.Now.ToLongTimeString(),
					LogData = "Starting Airodump on " + selectedInterface
				};
				InsertLog(log);
			}
			else
			{
				var log = new Logs
				{
					LogTime = DateTime.Now.ToLongTimeString(),
					LogData = "Not supported on Windows OS yet"
				};
				InsertLog(log);
			}
			var count = 0;
			while (count <= 120)
			{
				Thread.Sleep(1000);
				count++;
				if (count == 120)
				{
					foreach (Process ps in Process.GetProcesses())
					{
						if (ps.ProcessName.Contains("airodump"))
							ps.Kill();
						mustBreakLoop = true;
					}
				}
			}
		}
		private async void LogThread(StreamReader stream)
		{
			var count = 0;
			if (LogCollectionThread != null)
			{
				while (LogCollectionThread.IsAlive)
				{
					count++;
					var text = "";
					await Task.Delay(1000);
					// TODO: Implement a stream reader to read the file as it fills with data
					//var swriter = StreamReader()
					//logstream += stream.ReadLineAsync();
					if (count == 10)
					{
						var files = Directory.GetFiles(Directory.GetCurrentDirectory());
						foreach (var file in files)
						{
							if (file.Contains("airodump"))
							{
								text = File.ReadAllText(file);
								if (ViewHolder._mainWindow != null)
									await Dispatcher.UIThread.InvokeAsync(() => ViewHolder._mainWindow.insertData(text), DispatcherPriority.Background);
							}
						}
						
					}
				}
			}
		}
		private void StartLogThread(StreamReader file)
		{
			LogCollectionThread = new Thread(() => LogThread(file));
			LogCollectionThread.Start();
			LogCollectionThread.IsBackground = true;
		}
		private async Task pauseThread()
		{
				var text = File.ReadAllText("log.file");
				if (ViewHolder._mainWindow != null && text != null)
						await Dispatcher.UIThread.InvokeAsync(() => ViewHolder._mainWindow.insertData(text), DispatcherPriority.Background);
				File.Delete("log.file");
		}
		private void EnableAdapter()
		{
			ProcessStartInfo psi = new ProcessStartInfo("netsh", "interface set interface \"" + selectedInterface + "\" enable");
			Process p = new Process();
			p.StartInfo = psi;
			p.Start();
			var log = new Logs
			{
				LogTime = DateTime.Now.ToLongTimeString(),
				LogData = selectedInterface + " has been enabled"
			};
			InsertLog(log);
			var temp = new List<NetInterfaces>();
			foreach (NetInterfaces net in InterfaceList)
			{
				if (net.Name == selectedInterface)
				{
					var rei = new NetInterfaces
					{
						Enabled = "Enabled",
						State = "Online",
						Name = net.Name,
						Mode = net.Mode
					};
					temp.Add(rei);
				}
			}
			InterfaceList.Clear();
			foreach (NetInterfaces net2 in temp)
			{
				InterfaceList.Add(net2);
			}
		}

		private void DisableAdapter()
		{
			ProcessStartInfo psi = new ProcessStartInfo("netsh", "interface set interface \"" + selectedInterface + "\" disable");
			Process p = new Process();
			p.StartInfo = psi;
			p.Start();
			var log = new Logs
			{
				LogTime = DateTime.Now.ToLongTimeString(),
				LogData = selectedInterface + " has been disabled"
			};
			InsertLog(log);
			var temp = new List<NetInterfaces>();
			foreach (NetInterfaces net in InterfaceList)
			{
				if (net.Name == selectedInterface)
				{
					var rei = new NetInterfaces
					{
						Enabled = "Enabled",
						State = "Offline",
						Name = net.Name,
						Mode = net.Mode
					};
					temp.Add(rei);
				}
			}
			InterfaceList.Clear();
			foreach (NetInterfaces net2 in temp)
			{
				InterfaceList.Add(net2);
			}
		}

		private async void GetAdapterMode()
		{
			var log = new Logs
			{
				LogTime = DateTime.Now.ToLongTimeString(),
				LogData = selectedInterface + " selected"
			};
			InsertLog(log);
			var mode = "";
			if (WorkingOS == "Windows")
			{
				mode = runAndReturn("C:\\Windows\\System32\\Npcap\\wlanhelper.exe", "C:\\Windows\\System32\\Npcap", $"\"{selectedInterface}\" mode");
				Console.WriteLine(mode);
				SetModeLabel(mode.ToString().Replace("\r\n", ""));
				var _ilist = interfaceList();
				foreach (var _i in _ilist)
				{
					if (_i.Name == selectedInterface)
					{
						string _stat = "";
						if (_i.State == "Connected")
							_stat = "Online";
						else if (_i.State == "Disconnected")
							_stat = "Offline";
						var selIface = new NetInterfaces()
						{
							Enabled = _i.Enabled,
							Name = _i.Name,
							State = _stat,
							Mode = mode.ToString().Replace("\r\n", "")
						};
						InsertIface(selIface, mode);
						if (ViewHolder._mainWindow != null)
							await Dispatcher.UIThread.InvokeAsync(() => ViewHolder._mainWindow.setIFaceMode(mode.ToString().Replace("\r\n", "")), DispatcherPriority.Background);
					}
				}
			}
			else if (WorkingOS == "Linux")
			{
				InterfaceList.Clear();
				// NetInterfaces[] ifaces = new NetInterfaces[0];
				var _ilist = GetNewInterfaceCollection();
				foreach (NetInterfaces _i in _ilist)
				{
					//await MessageBox.Show(ViewHolder._mainWindow, _i.Name, "Build Results", MessageBox.MessageBoxButtons.Ok);
					if (_i.Name != null && _i.Name.Contains("mon"))
					{
						var selIface = new NetInterfaces()
						{
							Enabled = _i.Enabled,
							Name = _i.Name,
							State = "Online",
							Mode = "monitor"
						};
						if (ViewHolder._mainWindow != null)
							await Dispatcher.UIThread.InvokeAsync(() => ViewHolder._mainWindow.setIFaceMode("monitor"), DispatcherPriority.Background);
						// await Dispatcher.UIThread.InvokeAsync(() => InterfaceList.Clear(), DispatcherPriority.Background);
						await Dispatcher.UIThread.InvokeAsync(() => InterfaceList.Add(selIface), DispatcherPriority.Background);
						InterfaceList.Add(selIface);
						//   InsertIface(selIface, mode);
					}
					else if (_i.Name != null && _i.Name.Contains(selectedInterface))
					{
						var selIface = new NetInterfaces()
						{
							Enabled = _i.Enabled,
							Name = _i.Name,
							State = "Online",
							Mode = "managed"
						};
						if (ViewHolder._mainWindow != null)
							await Dispatcher.UIThread.InvokeAsync(() => ViewHolder._mainWindow.setIFaceMode("managed"), DispatcherPriority.Background);
						//  await Dispatcher.UIThread.InvokeAsync(() => InterfaceList.Clear(), DispatcherPriority.Background);
						await Dispatcher.UIThread.InvokeAsync(() => InterfaceList.Add(selIface), DispatcherPriority.Background);
						InterfaceList.Add(selIface);
					}
				}
			}
		}
		private string GetInterfaceMode()
		{
			if (!string.IsNullOrEmpty(selectedInterface) && WorkingOS == "Windows")
			{
				var mode = runAndReturn("C:\\Windows\\System32\\Npcap\\wlanhelper.exe", "C:\\Windows\\System32\\Npcap", $"\"{selectedInterface}\" mode");
				Console.WriteLine(mode);
				SetModeLabel(mode.ToString().Replace("\r\n", ""));
				return mode;
			}
			else
			{
				return "";
			}
		}
		private static IEnumerable<string> GenerateInterfaceChoices()
		{
			var defaultList = new List<string>();
			foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
			{
				defaultList.Add($"{ni.Name}");
			}
			return defaultList;
		}
		private IEnumerable<Logs> GenerateInitialLogTable()
		{
			var logList = new List<Logs>();
			var iniLog = new Logs
			{
				LogTime = DateTime.Now.ToLongTimeString(),
				LogData = "ExampleLog Data"
			};
			logList.Add(iniLog);
			return logList;
		}
		private IEnumerable<NetInterfaces> GenerateInterfaceInfoTable()
		{
			var interList = new List<NetInterfaces>();
			if (File.Exists("interface.log"))
				File.Delete("interface.log");
			if (WorkingOS == "Windows")
			{
                var returnInfo = runAndReturn("netsh.exe", "", "interface show interface");
                Console.WriteLine(returnInfo);
                File.WriteAllText("interface.log", returnInfo);
				File.SetAttributes("interface.log", FileAttributes.Hidden);
				string[] interfaces = File.ReadAllLines("interface.log");
				File.Delete("interface.log");
				int linecount = 0;
				foreach (string iface in interfaces)
				{
					linecount++;
					if (linecount > 3)
					{
						if (iface.StartsWith("Enabled") || iface.StartsWith("Disabled"))
						{
							string[] values = iface.Split(' ');
							//int inner = 0;
							var _iface = new NetInterfaces()
							{
								Enabled = values[0]
							};
							switch (values[8])
							{
								case "Connected":
									int _ = 0;
									string _name = "";
									_iface.State = "Online";
									foreach (string val in values)
									{
										_++;
										if (_ > 19)
											_name += val + " ";
									}
									_iface.Name = _name.TrimEnd().TrimStart();
									var mode = "";
									if (WorkingOS == "Windows")
									{
										mode = runAndReturn("C:\\Windows\\System32\\Npcap\\wlanhelper.exe", "C:\\Windows\\System32\\Npcap", $"\"{_iface.Name}\" mode");
									}
									_iface.Mode = mode.ToString().Replace("\r\n", "");
									SetModeLabel(mode.ToString().Replace("\r\n", ""));
									interList.Add(_iface);
									break;
								case "Disconnected":
									_iface.State = "Offline";
									int __ = 0;
									string __name = "";
									foreach (string val in values)
									{
										__++;
										if (__ > 19)
										{
											// MessageBox.Show(val);
											__name += val + " ";
										}
									}
									_iface.Name = __name.TrimEnd().TrimStart();
									var _mode = "";
									if (WorkingOS == "Windows")
									{
										_mode = runAndReturn("C:\\Windows\\System32\\Npcap\\wlanhelper.exe", "C:\\Windows\\System32\\Npcap", $"\"{_iface.Name}\" mode");
									}
									_iface.Mode = _mode.ToString().Replace("\r\n", "");
									SetModeLabel(_mode.ToString().Replace("\r\n", ""));
									interList.Add(_iface);
									break;
							}
						}
					}
				}
			}
			else
			{
				File.WriteAllText("interface.log", runAndReturn("iwconfig"));
			}
			
			if (File.Exists("interface.log"))
				File.Delete("interface.log");
			return interList;
		}
		#endregion Initializers

		#region Functions
		//private static string RandomString(int length)
		//{
		//	const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
		//	return new string(Enumerable.Repeat(chars, length)
		//		.Select(s => s[random.Next(s.Length)]).ToArray());
		//}
		public async void AddLogs(List<NetInterfaces> interfaceList)
		{
			foreach (var iface in interfaceList)
			{
				await Dispatcher.UIThread.InvokeAsync(() => InsertIface(iface), DispatcherPriority.Background);
			}
		}
		public async void SetModeLabel(string imode)
		{
			await Dispatcher.UIThread.InvokeAsync(() => IFaceMode = imode, DispatcherPriority.Background);
			if (ViewHolder._mainWindow != null)
				await Dispatcher.UIThread.InvokeAsync(() => ViewHolder._mainWindow.setIFaceMode(imode), DispatcherPriority.Background);
		}
		public void InsertIface(NetInterfaces iface)
		{
			var mode = "";
			if (WorkingOS == "Windows")
			{            
				mode = runAndReturn("C:\\Windows\\System32\\Npcap\\wlanhelper.exe", "C:\\Windows\\System32\\Npcap", $"\"{iface.Name}\" mode");
			}
			InterfaceList.Add(
				new NetInterfaces()
				{
					Enabled = iface.Enabled,
					State = iface.State,
					Name = iface.Name,
					Mode = mode.ToString().Replace("\r\n", "")
				});
			SetModeLabel(mode.ToString().Replace("\r\n", ""));
		}
		public void InsertLog(Logs log)
		{
			LogList.Add(
				new Logs()
				{
					LogTime = log.LogTime,
					LogData = log.LogData
				});
		}
		public void InsertIface(NetInterfaces iface, string mode)
		{
			InterfaceList.Clear();
				InterfaceList.Add(
					new NetInterfaces()
					{
						Enabled = iface.Enabled,
						State = iface.State,
						Name = iface.Name,
						Mode = mode
					});

		}
		public void InsertIface(NetInterfaces[] iface, string mode)
		{
			InterfaceList.Clear();
			foreach (NetInterfaces net2 in iface)
			{            
				InterfaceList.Add(
					new NetInterfaces()
					{
						Enabled = net2.Enabled,
						State = net2.State,
						Name = net2.Name,
						Mode = mode
					});
			}
		}
		void ClearLogs(bool _true)
		{
			if (_true)
				LogList.Clear();
		}
	   
		#endregion Functions

		#region DataHandler

		private void ensureFile(string file, string content, bool recreate = false)
		{
			if (recreate)
			{
				if (File.Exists(file))
					File.Delete(file);
			}
			if (!File.Exists(file))
				File.WriteAllText(file, content);
		}
		private async void runAndRead(string process, string directory = "", string command = "")
		{
			try
			{
				var proc = new Process
				{
						StartInfo = new ProcessStartInfo
						{
							FileName = process,
							Arguments = command,
							WorkingDirectory = directory,
							UseShellExecute = false,
							RedirectStandardOutput = true,
							CreateNoWindow = true
						}
				};
				char[] bb = new char[1024];
				var str = "";
				proc.Start();
				while (!proc.StandardOutput.EndOfStream)
				{
					#region OldCollection
					//	var chars = proc.StandardOutput.ReadLine();
					// str += chars.ToString() + "\n";
					//for (var i = 0; i < await chars.ConfigureAwait(false); i++)
					//{
					//var _str = Convert.ToChar(bb[i]);                    
					//    str += _str;
					//Console.WriteLine(str);
					// ScanData += str;
					//GetTextAsync();
					#endregion OldCollection
					StartLogThread(proc.StandardOutput);
					//var chars = proc.StandardOutput.ReadLine();

					//if (ViewHolder._mainWindow != null)
					//	Dispatcher.UIThread.InvokeAsync(() => ViewHolder._mainWindow.insertData(chars + "\r\n"), DispatcherPriority.Background);
	//                collection(str);
					//for (var i = 0; i < await chars.ConfigureAwait(false); i++)
					//{
					//var _str = Convert.ToChar(bb[i]);                    
					//    str += _str;
					
					//await Dispatcher.UIThread.InvokeAsync(() => ViewHolder._mainWindow.addLogs(str), DispatcherPriority.Background);
					//	}
					//Console.WriteLine(logstream);
				}
				try { if (LogCollectionThread != null) { LogCollectionThread.Abort(); } } catch (Exception ex) { Console.WriteLine(ex.Message); }
			}
			catch (Exception ex)
			{ 
				ensureFile("error.log", "WifiGeddan Error Log\n\n");
				File.AppendAllText("error.log", ex.Message);
			}
		}
		private async void collection(string str)
		{
			Console.WriteLine(str);
			ScanData += str;
			ensureFile("log.file", "", true);
			File.AppendAllText("log.file", ScanData);
			await pauseThread();
			str = "";
		}
		private static bool mustBreakLoop = false;
		private string runAndReturn(string process, string directory = "", string command = "")
		{
			var _info = "";
			try
			{
				var proc = new Process
				{
					StartInfo = new ProcessStartInfo
					{
						FileName = process,
						Arguments = command,
						WorkingDirectory = directory,
						UseShellExecute = false,
						RedirectStandardOutput = true,
						CreateNoWindow = true
					}
				};
				proc.Start();
				while (!proc.StandardOutput.EndOfStream)
				{
					_info = proc.StandardOutput.ReadToEnd();
				}
				return _info;
			}
			catch (Exception ex)
			{
				ensureFile("error.log", "WifiGeddan Error Log\n\n");
				File.AppendAllText("error.log", ex.Message);
				return ex.Message;
			}
		}
		private List<NetInterfaces> interfaceList()
		{
			var interList = new List<NetInterfaces>();
			if (File.Exists("interface.log"))
				File.Delete("interface.log");
			if (WorkingOS == "Windows")
			{
				File.WriteAllText("interface.log", runAndReturn("netsh.exe", "", "interface show interface"));
			}
			else
			{
				File.WriteAllText("interface.log", runAndReturn("iwconfig"));
			}
			File.SetAttributes("interface.log", FileAttributes.Hidden);
			string[] interfaces = File.ReadAllLines("interface.log");
			File.Delete("interface.log");
			int linecount = 0;
			foreach (var iface in interfaces)
			{
				linecount++;
				if (linecount > 3)
				{
					if (iface.StartsWith("Enabled") || iface.StartsWith("Disabled"))
					{
						var values = iface.Split(' ');
						//int inner = 0;
						var _iface = new NetInterfaces()
						{
							Enabled = values[0],
							State = values[8]
						};
						if (values[8] == "Connected")
						{
							int _ = 0;
							string _name = "";
							foreach (string val in values)
							{
								_++;
								if (_ > 19)
									_name += val + " ";
							}
							_iface.Name = _name.TrimEnd().TrimStart();
							var mode = "";
							if (WorkingOS == "Windows")
							{
								mode = runAndReturn("C:\\Windows\\System32\\Npcap\\wlanhelper.exe", "C:\\Windows\\System32\\Npcap", $"\"{_iface.Name}\" mode");
							}
							_iface.Mode = mode.ToString().Replace("\r\n", "");
							SetModeLabel(mode.ToString().Replace("\r\n", ""));
							interList.Add(_iface);
						}
						else if (values[8] == "Disconnected")
						{
							int _ = 0;
							var _name = "";
							foreach (string val in values)
							{
								_++;
								if (_ > 19)
								{
									// MessageBox.Show(val);
									_name += val + " ";
								}
							}
							_iface.Name = _name.TrimEnd().TrimStart();
							var mode = "";
							if (WorkingOS == "Windows")
							{
								mode = runAndReturn("C:\\Windows\\System32\\Npcap\\wlanhelper.exe", "C:\\Windows\\System32\\Npcap", $"\"{_iface.Name}\" mode");
							}
							_iface.Mode = mode.ToString().Replace("\r\n", "");
							SetModeLabel(mode.ToString().Replace("\r\n", ""));
							interList.Add(_iface);
						}
					}
				}
			}
			if (File.Exists("interface.log"))
				File.Delete("interface.log");
			return interList;
		}
		#endregion DataHandler
		#region AsyncTask_Binding
		public Task<string> MyAsyncText => GetTextAsync();
		private static string ScanData = "";
		private async Task<string> GetTextAsync()
		{
			await Task.Delay(100);
			return ScanData;
		}
		#endregion
	}
}
