using ModernVPN.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ModernVPN.MVVM.ViewModel
{
    internal class ProtectionViewModel : ObservableObject
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

        private string _connectVPN = "Connect";
        public string ConnectButton
        {
            get { return _connectVPN; }
            set
            { 
                _connectVPN = value;
                OnPropertyChanged();
            }

        }
        public RelayCommand ConnectCommand { get; set; }
        public ProtectionViewModel()
        {
           

            ConnectCommand = new RelayCommand(o => { 
                if(ConnectionStatus == "Connected!") 
                {
                    var disprocess = new Process();
                    disprocess.StartInfo.FileName = "cmd.exe";
                    disprocess.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
                    disprocess.StartInfo.ArgumentList.Add(@"/c rasdial /d");
                    
                    disprocess.Start();
                    disprocess.WaitForExit();
                    ConnectButton = "Connect";
                    ConnectionStatus = "";
                    return;
                }
                ConnectionStatus = "Connecting";
                var process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
                process.StartInfo.ArgumentList.Add(@"/c rasdial MyServer0 vpnbook 3ev7r8m /phonebook:./VPN/MyServer0.pbk");

                process.Start();
                process.WaitForExit();

                switch(process.ExitCode)
                {
                    case 0:
                       ConnectionStatus =  "Connected!";
                        ConnectButton = "Disconnect";
                        break;
                    case 691:
                        ConnectionStatus = "Wrong Credentials";
                        break;
                    default:
                        ConnectionStatus = $"Error: {process.ExitCode}";
                        break;
                }


            });

           

        }

    }
}

