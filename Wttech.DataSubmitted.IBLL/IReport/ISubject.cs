using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wttech.DataSubmitted.IBLL.IReport
{
    /// <summary>
    /// 被观察者接口
    /// </summary>
    public interface ISubject
    {
        /// <summary>
        /// 通知事件
        /// </summary>
        /// <param name="prame">参数类</param>
        void Notify(object prame);
        /// <summary>
        /// 通知者状态
        /// </summary>
        int State { set; get; }
    }
}
