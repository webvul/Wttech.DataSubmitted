using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.ServiceProcess;

namespace Wttech.DataSubmitted.Services
{
    public partial class DataService : ServiceBase
    {

        #region 5 Constructors
        public DataService()
        {
            InitializeComponent();
        }
        #endregion

        #region 11 Private Methods

        #endregion

        #region 12 Protected Methods
        protected override void OnStart(string[] args)
        {
            Bootstrapper.Run();
            DataServicesCore.GetInstance().Start(this);
        }

        protected override void OnStop()
        {
            DataServicesCore.GetInstance().Stop();
        }
        #endregion
    }
}
