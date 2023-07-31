using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WifiGeddan.ViewModels
{
    public class ReactiveViewModel : ReactiveObject
    {
        // This event is implemented by "INotifyPropertyChanged" and is all we need to inform
        // our view about changes.
        public ReactiveViewModel()
        {
            // We can listen to any property changes with "WhenAnyValue" and do whatever we want in "Subscribe".
            this.WhenAnyValue(x => x.ClientCount)
                 .ToProperty(this, x => x.CountedClients, out countedClients);
            this.WhenAnyValue(x => x.IfaceMode)
                 .ToProperty(this, x => x.CurrentMode, out currentMode);
            //this.WhenAnyValue(d => d.)

        }

        #region Client_Properties
        private readonly ObservableAsPropertyHelper<string> countedClients;        //firstName
        public string CountedClients => this.countedClients.Value;                 //FirstName
        private static string CHolder = $"Clients: {_cCount}";
        private string clientCount = $"Clients: 0"; // This is our backing field for Name    //name
        public static int _cCount = -1;
        public int? cCount = -1;
        public string ClientCount                                                 //Name
        {
            get => CHolder;// + _cCount;
            set
            {
                //CHolder = CHolder + _cCount++;
                this.RaiseAndSetIfChanged(ref clientCount, value, ClientCount);
            }//=> 
        }
        #endregion

        #region Admin_Properties
        private readonly ObservableAsPropertyHelper<string> currentMode;         //firstName
        public string CurrentMode => this.currentMode.Value;                   //FirstName
        private string ifaceMode = ""; // This is our backing field for Name     //name
        private static string AHolder = $"";

        public string IfaceMode                                                  //Name
        {
            get => AHolder;
            set
            {
                this.RaiseAndSetIfChanged(ref ifaceMode, value, IfaceMode);
            }//=> 
        }
        #endregion
    }
}
