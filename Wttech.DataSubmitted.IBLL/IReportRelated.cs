using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.Common.ViewModels;

namespace Wttech.DataSubmitted.IBLL
{
    /// <summary>
    /// 无数据收费站列表接口
    /// </summary>
    public interface IReportRelated
    {
        /// <summary>
        /// 获取无数据收费站列表
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        List<ReportRelatedViewModels> GetNoDataList(QueryParameters para);
    }
}
