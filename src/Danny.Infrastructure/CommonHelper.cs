﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Danny.Infrastructure
{
    public partial class CommonHelper
    {
        #region 提取html中所有的文本

        public static string ExtractTextFromHtml(string htmlstring)
        {
            if (string.IsNullOrEmpty(htmlstring)) return "";
            //删除脚本  
            htmlstring = Regex.Replace(htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML  
            htmlstring = Regex.Replace(htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(amp|emsp);", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            htmlstring.Replace("<", "");
            htmlstring.Replace(">", "");
            htmlstring.Replace("\r\n", "");

            return htmlstring;
        }
        #endregion

        #region 相对路径映射到磁盘的物理路径

        /// <summary>
        /// 映射一个路径到磁盘的物理路径
        /// </summary>
        /// <param name="path">相对路径 例如："~/bin"</param>
        /// <returns>物理路径.  例如："c:\inetpub\wwwroot\bin"</returns>
        public static string MapPath(string path)
        {
            if (HostingEnvironment.IsHosted)
            {
                return HostingEnvironment.MapPath(path);
            }

            //不在宿主环境中运行，运行在单元测试中
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(baseDirectory, path);
        }

        #endregion

        #region 返回中文格式的时间差

        /// <summary>
        /// 获取当前时间到某个时间的中文格式的时间差
        /// 例如 :1小时前
        /// </summary>
        /// <param name="targetTime">目标时间</param>
        /// <returns>中文格式的时间差</returns>
        public static string DateDiff(DateTime targetTime)
        {
            string dateDiff = null;

            var timeSpan = DateTime.Now - targetTime;
            if (timeSpan.Days >= 1)
            {
                dateDiff = targetTime.Month + "月" + targetTime.Day + "日";
            }
            else
            {
                if (timeSpan.Hours > 1)
                {
                    dateDiff = timeSpan.Hours + "小时前";
                }
                else
                {
                    dateDiff = timeSpan.Minutes + "分钟前";
                }
            }
            
            return dateDiff;
        }

        #endregion

        #region 生成字符串类型的随机数字

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="length">数字的长度</param>
        /// <returns>随机数字字符串</returns>
        public static string GenerateRandomDigitCode(int length)
        {
            var random = new Random();
            var str = string.Empty;
            for (int i = 0; i < length; i++)
                str = string.Concat(str, random.Next(10).ToString());
            return str;
        }

        #endregion

        #region 验证字符串是否是正确的Email格式

        /// <summary>
        /// 验证字符串是否是正确的Email格式
        /// </summary>
        public static bool IsValidEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
                return false;

            email = email.Trim();
            var result = Regex.IsMatch(email,
                "^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$",
                RegexOptions.IgnoreCase);
            return result;
        }

        #endregion

        #region 校验字符串是否是正确的IP地址格式

        /// <summary>
        /// 校验字符串是否是正确的IP地址格式
        /// </summary>
        public static bool IsValidIpAddress(string ipAddress)
        {
            IPAddress ip;
            return IPAddress.TryParse(ipAddress, out ip);
        }

        #endregion
    }
}
