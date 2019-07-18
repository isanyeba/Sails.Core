using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sails.Utils
{
    public static class RegexHelper
    {
        #region 判别字符串
        public const string QQNumberMailPattern = @"\d+\@[q|Q]{2}\.[cC][oO][mM]";

        public const string QQNumberMailPatternP = @"^\d+\@[q|Q]{2}\.[cC][oO][mM]$";

        /// <summary>
        /// 邮件地址判别字符串（强制匹配）
        /// </summary>
        public const string EMailPatternP = @"^[a-z|A-Z|{0-9}|_|-]+(\.?[a-z|A-Z|{0-9}|_|-])+\@([[a-z|A-Z|{0-9}|_|-]+\.)+[a-z|A-Z|{0-9}|_|-]+$";
        /// <summary>
        /// 邮件地址判别字符串
        /// </summary>
        public const string EMailPattern = @"[a-z|A-Z|{0-9}|_|-]+\@([[a-z|A-Z|{0-9}|_|-]+\.)+[a-z|A-Z|{0-9}|_|-]+";

        /// <summary>
        /// 用于匹配数字的正则表达式
        /// </summary>
        public const string NumberPattern = @"\s*[-|+]?\d+\.?\d*\s*";

        /// <summary>
        /// 用于匹配字母和数字的正则表达式
        /// </summary>
        public const string AZ09 = @"^[\d|A-Z|a-z]*$";

        /// <summary>
        /// 用于匹配字母和数字的正则表达式
        /// </summary>
        public const string AZ09_ = @"^[\d|A-Z|a-z|_]*$";


        /// <summary>
        /// 用于匹配字母的正则表达式
        /// </summary>
        public const string AZ = @"^[A-Z|a-z]*$";

        /// <summary>
        /// 用于匹配字母的正则表达式
        /// </summary>
        public const string AZ_Lower = @"^[A-Z]*$";

        /// <summary>
        /// 用于匹配字母的正则表达式
        /// </summary>
        public const string AZ_Upper = @"^[a-z]*$";

        /// <summary>
        /// 用于强制匹配数字的正则表达式
        /// </summary>
        public const string NumberPatternP = @"^[-|+]?\d+\.?\d*$";

        /// <summary>
        /// 用于匹配16进制数字的正则表达式
        /// </summary>
        public const string NumberPattern16 = @"^\s*[-|+]?[\d|A-F|a-f]+\.?[\d|A-F|a-f]*\s*$";

        /// <summary>
        /// 用于强制匹配16进制数字的正则表达式
        /// </summary>
        public const string NumberPattern16P = @"^[-|+]?[\d|A-F|a-f]+\.?[\d|A-F|a-f]*$";

        /// <summary>
        /// 用于匹配Size的正则表达式
        /// </summary>
        public const string SizePattern = @"(\s*[-|+]?\d+\.?\d*\s*)[,|，](\s*[-|+]?\d+\.?\d*\s*)";

        /// <summary>
        /// 用于匹配Size的正则表达式
        /// </summary>
        public const string SizePatternP = @"^[-|+]?\d+\.?\d*\s*)[,|，](\s*[-|+]?\d+\.?\d*$";

        /// <summary>
        /// 用于匹配数字数组(该数组以逗号或者)
        /// </summary>
        public const string NumberArrayPattern = @"(\s*[-|+]?\d+\.?\d*\s*[,|，|;|；]?\s*)+";

        /// <summary>
        /// 用于匹配汉字的正则表达式
        /// </summary>
        public const string CharZhCNPattern = @"[\u4e00-\u9fa5]";

        /// <summary>
        /// 用于匹配汉字的正则表达式
        /// </summary>
        public const string ZhCNPattern = @"[\u4e00-\u9fa5]+";

        /// <summary>
        /// 不带符号验证器
        /// </summary>
        public const string NoSymbolPattern = @"^[A-Z|a-z|\u4e00-\u9fa5]*$";

        /// <summary>
        /// 匹配空格的正则表达式 \s*
        /// </summary>
        public const string SpacePattern = @"\s+";

        /// <summary>
        /// 用于匹配Size的正则表达式
        /// </summary>
        public const string RectanglePattern = @"^(\s*[-|+]?\d+\.?\d*\s*)(([,|，]\s*)(\s*[-|+]?\d+\.?\d*\s*)){3}$";

        /// <summary>
        /// 用于匹配Size的正则表达式
        /// </summary>
        public const string Point3Pattern = @"(\s*[-|+]?\d+\.?\d*\s*)(([,|，]\s*)(\s*[-|+]?\d+\.?\d*\s*)){2}";

        /// <summary>
        /// 用于匹配TimeSpan的正则表达式
        /// </summary>
        public const string TimeSpanPattern = @"(\s*[-|+]?\d+\.?\d*\s*)(([:|：]\s*)(\s*[-|+]?\d+\.?\d*\s*)){0,4}";

        /// <summary>
        /// 强制远程地址判别串
        /// </summary>
        public const string EndPointPattern = @".{0,255}\:{1}\d{1,5}";

        /// <summary>
        /// 匹配IP地址格式字符串，非严格匹配
        /// </summary>
        public const string IPAddressPattern = @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}";

        /// <summary>
        /// 匹配IP地址格式字符串，非严格匹配
        /// </summary>
        public const string IPAddressPatternP = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$";

        /// <summary>
        /// 强制远程地址判别串
        /// </summary>
        public const string EndPointPatternP = @"^.{0,255}\:{1}\d{1,5}$";

        #endregion

        #region 数字判断
        /// <summary>
        /// 返回传入的字符串是否为数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNum(this string value)
        {
            return IsMatch(value, NumberPatternP);
        }

        /// <summary>
        /// 返回传入的字符串是否为数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNum(this string value, int intBase)
        {
            if (intBase <= 10)
            {
                return IsMatch(value, NumberPattern);
            }
            else
            {
                return IsMatch(value, NumberPattern16);
            }
        }

        /// <summary>
        /// 返回传入的字符串是否为数字数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumArray(this string value)
        {
            return IsMatch(value, NumberArrayPattern);
        }

        #endregion

        #region 点类型判断
        public static bool IsPoint3(this string valueString)
        {
            return IsMatch(valueString, Point3Pattern);
        }

        /// <summary>
        /// 返回传入的字符串是否为一个Point字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPoint(this string value)
        {
            return IsSize(value);
        }
        #endregion

        #region 时间判定
        public const string Time_CST = @"([A-Z]+[a-z][a-z]\s){2}[0-3][0-9]\s[0-2][0-9]\:[0-5][0-9]\:[0-5][0-9]\sCST\s\d{4}";

        /// <summary>
        /// true＝给定的字符是CST时间
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsTime_CST(this string str)
        {
            return IsMatch(str, Time_CST);
        }

        public const string Time_GMT = @"[A-Z][a-z]{2},\s\d{2}[\s|-][A-Z][a-z]{2}[\s|-]\d{2,4}\s\d{2}:\d{2}:\d{2}\sGMT";

        /// <summary>
        /// true＝给定的字符是CST时间
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsTime_GMT(this string str)
        {
            return IsMatch(str, Time_GMT);
        }
        #endregion

        /// <summary>
        /// 判断给定的字符串是否为数字和字符组成的
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsAZ09(this string value)
        {
            return IsMatch(value, AZ09);
        }

        /// <summary>
        /// 判断给定的字符串是否允许作为对象名称
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNameable(this string value)
        {
            if (value.StartWithNum()) return false;
            return IsMatch(value, AZ09_);
        }

        /// <summary>
        /// 判定是否为qq号码邮箱
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsQQNumMail(this string value)
        {
            return Regex.IsMatch(value, QQNumberMailPatternP);
        }

        /// <summary>
        /// 判断给定的字符串是否为数字和字符组成的
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsAZ(this string value)
        {
            return IsMatch(value, AZ);
        }

        /// <summary>
        /// 返回给定的字符是否为汉字 true=是汉字
        /// </summary>
        /// <returns></returns>
        public static bool IsZhCNCharacter(this char ch)
        {
            return Regex.IsMatch(ch.ToString(), CharZhCNPattern);
        }

        /// <summary>
        /// 判定是否为中文字符
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsZhCn(this string value)
        {
            return IsMatch(value, ZhCNPattern);
        }

        /// <summary>
        /// 不带符号的字符
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNoSymbol(this string value)
        {
            return IsMatch(value, NoSymbolPattern);
        }

        /// <summary>
        /// 返回该字符串是否是以Size定义的形式，例如  800,600
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsSize(this string value)
        {
            return IsMatch(value, SizePattern);
        }

        /// <summary>
        /// 返回传入的字符串是否为一个矩形字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsRectangle(this string value)
        {
            return IsMatch(value, RectanglePattern);
        }

        /// <summary>
        /// 判别是否为TimeSpan类型的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsTimeSpan(this string value)
        {
            return IsMatch(value, TimeSpanPattern);
        }

        /// <summary>
        /// 判断一个字符串是否为ARGB格式的颜色字符(是矩形形式，或者三维点形式)
        /// </summary>
        public static bool IsColor(this string valueString)
        {
            return IsRectangle(valueString) || IsPoint3(valueString);
        }

        /// <summary>
        /// 判断一个字符串是否以数字开头
        /// </summary>
        /// <param name="valueStr"></param>
        /// <returns></returns>
        public static bool StartWithNum(this string valueStr)
        {
            if (valueStr == null || valueStr == string.Empty) return false;
            string valueString = valueStr[0].ToString();
            return IsNum(valueString);
        }

        /// <summary>
        /// 计算给定的字符串（source）中包含的目标字符串(target)的个数
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int GetMatchCount(this string source, string target)
        {
            //string str=new string('\\',target.Length*2);
            char[] str = new char[target.Length * 2];
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = '\\';
            }
            for (int i = 0; i < target.Length; i++)
            {
                str[i * 2 + 1] = target[i];
            }
            return Regex.Matches(source, new string(str)).Count;
        }

        public static bool IsMatch(this string value, string pattern)
        {
            if (value == null || value == string.Empty) return false;
            return Regex.IsMatch(value, pattern);
        }

        public static bool IsMatch(this string value, string pattern, RegexOptions option)
        {
            return Regex.IsMatch(value, pattern, option);
        }

        /// <summary>
        /// 返回ture表示字符串为邮件地址
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmail(this string value)
        {
            return IsMatch(value, EMailPatternP);
        }

        public static string Replace(this string value, string pattern, string replacement, RegexOptions option)
        {
            return Regex.Replace(value, pattern, replacement, option);
        }

        /// <summary>
        /// 判断给定的字符串是否为一个EndPoint
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEndPoint(this string value)
        {
            return IsMatch(value, EndPointPatternP);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsIPAddress(this string value)
        {
            return IsMatch(value, IPAddressPattern);
        }

        public static string GetValue(this string value, string pattern)
        {
            Match match = Regex.Match(value, pattern);
            if (match == null) return null;
            return match.Value;
        }
    }
}
