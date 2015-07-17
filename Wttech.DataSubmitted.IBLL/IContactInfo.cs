using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wttech.DataSubmitted.DAL;
using Wttech.DataSubmitted.Common;

namespace Wttech.DataSubmitted.IBLL
{
    /// <summary>
    /// 导出文件记录接口
    /// </summary>
    public interface IContactInfo : IQuery<List<OT_ExportHis>>, ICreate<OT_ExportHis>
    {
    }
}
