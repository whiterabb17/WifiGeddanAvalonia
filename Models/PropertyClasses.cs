using WifiGeddan.Views;
using System.Collections.Generic;
using CsvHelper.Configuration.Attributes;

namespace WifiGeddan.Models
{
    public class ViewHolder
    {
        public static MainWindow? _mainWindow;
    }
   
    public class AiroDumpRouters
    {
        [Index(13)]
        public string? ESSID
        {
            get; set;
        }
        [Index(0)]
        public string? BSSID
        {
            get; set;
        }
        [Index(1)]
        public string? FirstSeen
        {
            get; set;
        }
        [Index(2)]
        public string? LastSeen
        {
            get; set;
        }
        [Index(3)]
        public string? Channel
        {
            get; set;
        }
        [Index(4)]
        public string? Speed
        {
            get; set;
        }
        [Index(5)]
        public string? Privacy
        {
            get; set;
        }
        [Index(6)]
        public string? Cipher
        {
            get; set;
        }
        [Index(7)]
        public string? Authentication
        {
            get; set;
        }
        [Index(8)]
        public string? Power
        {
            get; set;
        }
        [Index(9)]
        public string? Beacons
        {
            get; set;
        }
        [Index(10)]
        public string? IV
        {
            get; set;
        }
        [Index(11)]
        public string? LANIP
        {
            get; set;
        }
        [Index(12)]
        public string? ID_Length
        {
            get; set;
        }
        [Index(14)]
        public string? Key
        {
            get; set;
        }
    }
    public class AiroDumpDevices
    {
        [Index(0)]
        public string? StationMAC
        {
            get; set;
        }
        [Index(1)]
        public string? FirstSeen
        {
            get; set;
        }
        [Index(2)]
        public string? LastSeen
        {
            get; set;
        }
        [Index(3)]
        public string? Power
        {
            get; set;
        }
        [Index(4)]
        public string? Packets
        {
            get; set;
        }
        [Index(5)]
        public string? BSSID
        {
            get; set;
        }
        [Index(6)]
        public string? Probed_ESSID
        {
            get; set;
        }
    }
    public class Logs
    { 
        public string? LogTime { get; set; }
        public string? LogData { get; set; }
    }
    public class AdapterInfo
    { 
        public string? AdapterName { get; set; }
        public string? AdapterId { get; set; }
        public string? AdapterType { get; set; }
        public string? AdapterMode { get; set; }
        public string? OperationalStatus { get; set; }
        public string? Description { get; set; }
        public string? GatewayAddress { get; set; }
        public string? MulticastAddress { get; set; }
        public string? DnsAddress { get; set; }
        public bool? DnsEnabled { get; set; }
        public long? BytesRecieved { get; set; }
        public long? BytesSent { get; set; }
    }
}
