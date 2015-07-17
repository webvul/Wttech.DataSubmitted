/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：中国常用农历日期时间类文件
 * 创建标识：ta0395侯兴鼎20141209
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wttech.DataSubmitted.Common
{
    /// <summary>
    /// 中国常用农历日期时间类
    /// zj53hao@qq.com   http://hi.csdn.net/zj53hao
    /// </summary>
   public  class ChinaDateTime
    {
        private int year, month, dayOfMonth;
        private bool isLeap;
        public DateTime time;

        /// <summary>
        /// 获取当前日期的农历年
        /// </summary>
        public int Year
        {
            get { return year; }
        }

        /// <summary>
        /// 获取当前日期的农历月份
        /// </summary>
        public int Month
        {
            get { return month; }
        }

        /// <summary>
        /// 获取当前日期的农历月中天数
        /// </summary>
        public int DayOfMonth
        {
            get { return dayOfMonth; }
        }

        /// <summary>
        /// 获取当前日期是否为闰月中的日期
        /// </summary>
        public bool IsLeap
        {
            get { return isLeap; }
        }

        System.Globalization.ChineseLunisolarCalendar cc;
        /// <summary>
        /// 返回指定公历日期的阴历时间
        /// </summary>
        /// <param name="time"></param>
        public ChinaDateTime(DateTime time)
        {
            cc = new System.Globalization.ChineseLunisolarCalendar();

            if (time > cc.MaxSupportedDateTime || time < cc.MinSupportedDateTime)
                throw new Exception("参数日期时间不在支持的范围内,支持范围：" + cc.MinSupportedDateTime.ToShortDateString() + "到" + cc.MaxSupportedDateTime.ToShortDateString());
            year = cc.GetYear(time);
            month = cc.GetMonth(time);
            dayOfMonth = cc.GetDayOfMonth(time);
            isLeap = cc.IsLeapMonth(year, month);
            if (isLeap) month -= 1;
            this.time = time;

        }
        /// <summary>
        /// 返回当前日前的农历日期。
        /// </summary>
        public static ChinaDateTime Now
        {
            get { return new ChinaDateTime(DateTime.Now); }
        }

        /// <summary>
        /// 返回指定农历年，月，日，是否为闰月的农历日期时间
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="Month"></param>
        /// <param name="DayOfMonth"></param>
        /// <param name="IsLeap"></param>
        public ChinaDateTime(int Year, int Month, int DayOfMonth, bool IsLeap)
        {
            if (Year >= cc.MaxSupportedDateTime.Year || Year <= cc.MinSupportedDateTime.Year)
                throw new Exception("参数年份时间不在支持的范围内,支持范围：" + cc.MinSupportedDateTime.ToShortDateString() + "到" + cc.MaxSupportedDateTime.ToShortDateString());

            if (Month < 1 || Month > 12)
                throw new Exception("月份必须在1~12范围");
            cc = new System.Globalization.ChineseLunisolarCalendar();

            if (cc.GetLeapMonth(Year) != Month && IsLeap)
                throw new Exception("指定的月份不是当年的闰月");
            if (cc.GetDaysInMonth(Year, IsLeap ? Month + 1 : Month) < DayOfMonth || DayOfMonth < 1)
                throw new Exception("指定的月中的天数不在当前月天数有效范围");
            year = Year;
            month = Month;
            dayOfMonth = DayOfMonth;
            isLeap = IsLeap;
            time = DateTime.Now;
        }

        /// <summary>
        /// 获取当前农历日期的公历时间
        /// </summary>
        public DateTime ToDateTime()
        {
            return cc.ToDateTime(year, isLeap ? month + 1 : month, dayOfMonth, time.Hour, time.Minute, time.Second, time.Millisecond);
        }

        /// <summary>
        /// 获取指定农历时间对应的公历时间
        /// </summary>
        /// <param name="CnTime"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(ChinaDateTime CnTime)
        {
            return CnTime.ToDateTime();
        }

        /// <summary>
        /// 获取指定公历时间转换为农历时间
        /// </summary>
        /// <param name="Time"></param>
        /// <returns></returns>
        public static ChinaDateTime ToChinaDateTime(DateTime Time)
        {
            return new ChinaDateTime(Time);
        }
    }
}
