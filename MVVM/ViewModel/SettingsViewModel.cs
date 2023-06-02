using Microsoft.Win32;
using ModernVPN.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ModernVPN.MVVM.ViewModel
{
    internal class SettingsViewModel : ObservableObject
    {
        
        private string _connectionStatus;
        public string ConnectionStatus
        {
            get { return _connectionStatus; }
            set
            {
                _connectionStatus = value;
                OnPropertyChanged();
            }
        }

        private string _connectProxy = "Connect";
        public string ConnectProxy
        {
            get { return _connectProxy; }
            set
            {
                _connectProxy = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand Connect2Command { get; set; }

        RegistryKey RegKey;
        string Proxy = string.Empty;
        public SettingsViewModel()
        {
           
            Connect2Command = new RelayCommand(async o =>
            {
                if (ConnectionStatus == "Connected!")
                {
                    RegKey.SetValue("ProxyEnable", 0);
                    ConnectionStatus = string.Empty;
                    ConnectProxy = "Connect";
                    return;
                }


                ConnectProxy = "Connect";
                ConnectionStatus = "Connecting";
                RegKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
                Proxy = "186.121.235.66:8080";
                RegKey.SetValue("ProxyEnable", 1);
                RegKey.SetValue("ProxyServer", Proxy);
                await Task.Delay(3000);
                try
                {
                    using (TcpClient client = new TcpClient())
                    {
                        client.Connect("186.121.235.66", 8080);
                        ConnectionStatus = "Connected!";
                        ConnectProxy = "Disconnect";
                    }
                }
                catch
                {
                    ConnectionStatus = "Error";
                }
               
            });
        }
    }
}

