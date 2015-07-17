using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace Wttech.DataSubmitted.Services
{
    [RunInstaller(true)]
    public partial class DataInstaller : System.Configuration.Install.Installer
    {
        private ServiceInstaller serviceInstaller;
        private ServiceProcessInstaller processInstaller;
        public DataInstaller()
        {
            InitializeComponent();
            processInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            serviceInstaller.ServiceName = "DataSubmittedService";
            serviceInstaller.Description = "";//描述信息
            serviceInstaller.DisplayName = "数据报送系统数据生成服务SJSC1.02.00.141229";// +Assembly.GetExecutingAssembly().GetName().Version.ToString();

            Installers.Add(serviceInstaller);
            Installers.Add(processInstaller);
        }
    }
}
