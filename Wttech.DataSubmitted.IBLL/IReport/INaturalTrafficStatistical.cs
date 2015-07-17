using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wttech.DataSubmitted.DAL;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.Common.ViewModels;
using Wttech.DataSubmitted.IBLL.Common;

namespace Wttech.DataSubmitted.IBLL.IReport
{
    /// <summary>
    /// 每日报送数据报表接口
    /// </summary>
    public interface INaturalTrafficStatistical : IReportQuery, IGenerateSheet, IUpdate<UpdateNaturalInfoViewModel>, IExport, ICheck
    {

    }
}
