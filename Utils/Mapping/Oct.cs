using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Sails.Utils
{
    public static class Oct
    {
        public static ProtocolType[] ToProtocol(string valuestr)
        {
            if (valuestr == null) return null;
            List<ProtocolType> lst = new List<ProtocolType>();
            string[] strArray = valuestr.Split(',');
            for (int i = 0; i < strArray.Length; i++)
            {
                lst.Add((ProtocolType)Oct.ToEnum(typeof(ProtocolType), strArray[i]));
            }
            return lst.ToArray();
        }

        #region 常用类型转换
#if !SILVERLIGHT

        static WebColorConverter colorConverter;

        #region POINT
        public static Point ToPoint(object value)
        {
            if (IsNull(value)) return Point.Empty;
            if (value.GetType() == SystemTypes.Point) return (Point)value;
            return ToPoint(value.ToString());
        }

        /// <summary>
        /// 从Point格式的字符串中生成Point对象
        /// </summary>
        /// <param name="valueString"></param>
        /// <returns></returns>
        public static Point ToPoint(string valueString)
        {
            if (valueString == string.Empty) return Point.Empty;
            if (valueString.IndexOf('{') > -1)
            {
                valueString = Regex.Replace(valueString, @"[\{|\}|{a-z}|{A-Z}|\=|\s]*", "");
            }
            int[] nums = ToInt32Array(valueString);
            if (nums.Length < 2)
                return Point.Empty;
            return new Point(nums[0], nums[1]);
        }

        public static Point[] ToPointArray(object value)
        {
            if (IsNull(value)) return null;
            Type t = value.GetType();
            if (t.IsArray)
            {
                Array array = (Array)value;
                Point[] result = new Point[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = ToPoint(array.GetValue(i));
                }
                return result;
            }
            else
            {
                return ToPointArray(value.ToString());
            }
        }

        public static Point[] ToPointArray(string valueString)
        {
            if (valueString == string.Empty) return null;
            valueString = valueString.Replace('；', ';').Trim().Trim(';');
            string[] stringS = null;
            if (valueString.IndexOf(';') > -1)
            {
                stringS = valueString.Split(';');
            }
            else
            {
                stringS = new string[] { valueString };
            }
            Point[] result = new Point[stringS.Length];
            for (int i = 0; i < stringS.Length; i++)
            {
                result[i] = ToPoint(stringS[i]);
            }
            return result;
        }
        #endregion

        #region PointF
        public static PointF ToPointF(object value)
        {
            if (IsNull(value)) return PointF.Empty;
            if (value.GetType() == SystemTypes.PointF) return (PointF)value;
            return ToPointF(value.ToString());
        }

        /// <summary>
        /// 从PointF格式的字符串中生成PointF对象
        /// </summary>
        /// <param name="valueString"></param>
        /// <returns></returns>
        public static PointF ToPointF(string valueString)
        {
            if (valueString == string.Empty) return PointF.Empty;
            if (valueString.IndexOf('{') > -1)
            {
                valueString = Regex.Replace(valueString, @"[\{|\}|{a-z}|{A-Z}|\=|\s]*", "");
            }
            float[] nums = ToSingleArray(valueString);
            if (nums.Length < 2)
                return PointF.Empty;
            return new PointF(nums[0], nums[1]);
        }

        public static PointF[] ToPointFArray(object value)
        {
            if (IsNull(value)) return null;
            Type t = value.GetType();
            if (t.IsArray)
            {
                Array array = (Array)value;
                PointF[] result = new PointF[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = ToPointF(array.GetValue(i));
                }
                return result;
            }
            else
            {
                return ToPointFArray(value.ToString());
            }
        }

        public static PointF[] ToPointFArray(string valueString)
        {
            if (valueString == string.Empty) return null;
            valueString = valueString.Replace('；', ';').Trim().Trim(';');
            string[] stringS = null;
            if (valueString.IndexOf(';') > -1)
            {
                stringS = valueString.Split(';');
            }
            else
            {
                stringS = new string[] { valueString };
            }
            PointF[] result = new PointF[stringS.Length];
            for (int i = 0; i < stringS.Length; i++)
            {
                result[i] = ToPointF(stringS[i]);
            }
            return result;
        }
        #endregion

        #region Size
        public static Size ToSize(object value)
        {
            if (IsNull(value)) return Size.Empty;
            if (value.GetType() == SystemTypes.Size) return (Size)value;
            return ToSize(value.ToString());
        }

        /// <summary>
        /// 从Size格式的字符串中生成Size对象
        /// </summary>
        /// <param name="valueString"></param>
        /// <returns></returns>
        public static Size ToSize(string valueString)
        {
            if (valueString == string.Empty) return Size.Empty;
            if (valueString.IndexOf('{') > -1)
            {
                valueString = Regex.Replace(valueString, @"[\{|\}|{a-z}|{A-Z}|\=|\s]*", "");
            }
            int[] nums = ToInt32Array(valueString);
            if (nums.Length < 2)
                return Size.Empty;
            return new Size(nums[0], nums[1]);
        }

        public static Size[] ToSizeArray(object value)
        {
            if (IsNull(value)) return null;
            Type t = value.GetType();
            if (t.IsArray)
            {
                Array array = (Array)value;
                Size[] result = new Size[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = ToSize(array.GetValue(i));
                }
                return result;
            }
            else
            {
                return ToSizeArray(value.ToString());
            }
        }

        public static Size[] ToSizeArray(string valueString)
        {
            if (valueString == string.Empty) return null;
            valueString = valueString.Replace('；', ';').Trim().Trim(';');
            string[] stringS = null;
            if (valueString.IndexOf(';') > -1)
            {
                stringS = valueString.Split(';');
            }
            else
            {
                stringS = new string[] { valueString };
            }
            Size[] result = new Size[stringS.Length];
            for (int i = 0; i < stringS.Length; i++)
            {
                result[i] = ToSize(stringS[i]);
            }
            return result;
        }
        #endregion

        #region SizeF
        public static SizeF ToSizeF(object value)
        {
            if (IsNull(value)) return SizeF.Empty;
            if (value.GetType() == SystemTypes.SizeF) return (SizeF)value;
            return ToSizeF(value.ToString());
        }

        /// <summary>
        /// 从SizeF格式的字符串中生成SizeF对象
        /// </summary>
        /// <param name="valueString"></param>
        /// <returns></returns>
        public static SizeF ToSizeF(string valueString)
        {
            if (valueString == string.Empty) return SizeF.Empty;
            if (valueString.IndexOf('{') > -1)
            {
                valueString = Regex.Replace(valueString, @"[\{|\}|{a-z}|{A-Z}|\=|\s]*", "");
            }
            float[] nums = ToSingleArray(valueString);
            if (nums.Length < 2)
                return SizeF.Empty;
            return new SizeF(nums[0], nums[1]);
        }

        public static SizeF[] ToSizeFArray(object value)
        {
            if (IsNull(value)) return null;
            Type t = value.GetType();
            if (t.IsArray)
            {
                Array array = (Array)value;
                SizeF[] result = new SizeF[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = ToSizeF(array.GetValue(i));
                }
                return result;
            }
            else
            {
                return ToSizeFArray(value.ToString());
            }
        }

        public static SizeF[] ToSizeFArray(string valueString)
        {
            if (valueString == string.Empty) return null;
            valueString = valueString.Replace('；', ';').Trim().Trim(';');
            string[] stringS = null;
            if (valueString.IndexOf(';') > -1)
            {
                stringS = valueString.Split(';');
            }
            else
            {
                stringS = new string[] { valueString };
            }
            SizeF[] result = new SizeF[stringS.Length];
            for (int i = 0; i < stringS.Length; i++)
            {
                result[i] = ToSizeF(stringS[i]);
            }
            return result;
        }
        #endregion

        #region RectangleF
        public static RectangleF ToRectangleF(object value)
        {
            if (IsNull(value)) return RectangleF.Empty;
            if (value.GetType() == SystemTypes.RectangleF) return (RectangleF)value;
            return ToRectangleF(value.ToString());
        }

        /// <summary>
        /// 从RectangleF格式的字符串中生成RectangleF对象
        /// </summary>
        /// <param name="valueString"></param>
        /// <returns></returns>
        public static RectangleF ToRectangleF(string valueString)
        {
            if (valueString == string.Empty) return RectangleF.Empty;
            if (valueString.IndexOf('{') > -1)
            {
                valueString = Regex.Replace(valueString, @"[\{|\}|{a-z}|{A-Z}|\=|\s]*", "");
            }
            float[] nums = ToSingleArray(valueString);
            if (nums.Length < 4)
                return RectangleF.Empty;
            return new RectangleF(nums[0], nums[1], nums[2], nums[3]);
        }

        public static RectangleF[] ToRectangleFArray(object value)
        {
            if (IsNull(value)) return null;
            Type t = value.GetType();
            if (t.IsArray)
            {
                Array array = (Array)value;
                RectangleF[] result = new RectangleF[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = ToRectangleF(array.GetValue(i));
                }
                return result;
            }
            else
            {
                return ToRectangleFArray(value.ToString());
            }
        }

        public static RectangleF[] ToRectangleFArray(string valueString)
        {
            if (valueString == string.Empty) return null;
            valueString = valueString.Replace('；', ';').Trim().Trim(';');
            string[] stringS = null;
            if (valueString.IndexOf(';') > -1)
            {
                stringS = valueString.Split(';');
            }
            else
            {
                stringS = new string[] { valueString };
            }
            RectangleF[] result = new RectangleF[stringS.Length];
            for (int i = 0; i < stringS.Length; i++)
            {
                result[i] = ToRectangleF(stringS[i]);
            }
            return result;
        }
        #endregion

        #region Rectangle
        public static Rectangle ToRectangle(object value)
        {
            if (IsNull(value)) return Rectangle.Empty;
            if (value.GetType() == SystemTypes.Rectangle) return (Rectangle)value;
            return ToRectangle(value.ToString());
        }

        /// <summary>
        /// 从Rectangle格式的字符串中生成Rectangle对象
        /// </summary>
        /// <param name="valueString"></param>
        /// <returns></returns>
        public static Rectangle ToRectangle(string valueString)
        {
            if (valueString == string.Empty) return Rectangle.Empty;
            if (valueString.IndexOf('{') > -1)
            {
                valueString = Regex.Replace(valueString, @"[\{|\}|{a-z}|{A-Z}|\=|\s]*", "");
            }
            int[] nums = ToInt32Array(valueString);
            if (nums.Length < 4)
                return Rectangle.Empty;
            return new Rectangle(nums[0], nums[1], nums[2], nums[3]);
        }

        public static Rectangle[] ToRectangleArray(object value)
        {
            if (IsNull(value)) return null;
            Type t = value.GetType();
            if (t.IsArray)
            {
                Array array = (Array)value;
                Rectangle[] result = new Rectangle[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = ToRectangle(array.GetValue(i));
                }
                return result;
            }
            else
            {
                return ToRectangleArray(value.ToString());
            }
        }

        public static Rectangle[] ToRectangleArray(string valueString)
        {
            if (valueString == string.Empty) return null;
            valueString = valueString.Replace('；', ';').Trim().Trim(';');
            string[] stringS = null;
            if (valueString.IndexOf(';') > -1)
            {
                stringS = valueString.Split(';');
            }
            else
            {
                stringS = new string[] { valueString };
            }
            Rectangle[] result = new Rectangle[stringS.Length];
            for (int i = 0; i < stringS.Length; i++)
            {
                result[i] = ToRectangle(stringS[i]);
            }
            return result;
        }
        #endregion


        #region COLOR


        public static Color ToColor(object value)
        {
            if (IsNull(value)) return Color.Transparent;
            if (value.GetType() == SystemTypes.Color) return (Color)value;
            return ToColor(value.ToString());
        }

        /// <summary>
        /// 将一个颜色字符串类型转换为颜色
        /// </summary>
        /// <param name="valueString"></param>
        /// <returns></returns>
        public static Color ToColor(string valueString)
        {
            if (!RegexHelper.IsColor(valueString))
            {
                valueString = valueString.Trim('#');
                if (((valueString.Length == 8 || valueString.Length == 6) && RegexHelper.IsNum(valueString, 16)))
                {
                    valueString = "#" + valueString;
                }
                else
                    return Color.Transparent;
            }

            valueString = valueString.Replace("，", ",");
            int stringType = valueString.IndexOf(',') > 0 ? 2 : valueString.IndexOf('#') > 0 ? 1 : 3;
            int a = 255;
            int r = 0;
            int g = 0;
            int b = 0;
            switch (stringType)
            {
                case 1:
                valueString = valueString.Trim('#');
                int stringLen = valueString.Length;
                int startIdx = 0;
                string astr = valueString.Substring(startIdx, 2);
                if (RegexHelper.IsNum(astr, 16))
                {
                    if (stringLen >= 8)
                    {
                        a = Convert.ToInt32(valueString.Substring(startIdx, 2), 16);
                        startIdx += 2;
                    }
                    r = Convert.ToInt32(valueString.Substring(startIdx, 2), 16);
                    g = Convert.ToInt32(valueString.Substring(startIdx + 2, 2), 16);
                    b = Convert.ToInt32(valueString.Substring(startIdx + 4, 2), 16);
                }
                else
                {
                    if (colorConverter == null) colorConverter = new WebColorConverter();
                    return (Color)colorConverter.ConvertFromString(valueString);
                }
                break;
                case 2:
                valueString = valueString.Trim().Replace('，', ',').Trim(',');
                string[] array = valueString.Split(',');
                int index = 0;
                if (array.Length > 3)
                {
                    a = (int)Convert.ToSingle(array[index]);
                    index += 1;
                }
                r = (int)Convert.ToSingle(array[index]);
                g = (int)Convert.ToSingle(array[index + 1]);
                b = (int)Convert.ToSingle(array[index + 2]);

                break;
                case 3:
                {
                    if (colorConverter == null) colorConverter = new WebColorConverter();
                    valueString = valueString.Trim(',');
                    return (Color)colorConverter.ConvertFromString(valueString);
                }
            }
            if (a > 255) a = 255;
            if (r > 255) r = 255;
            if (g > 255) g = 255;
            if (b > 255) b = 255;
            return Color.FromArgb(a, r, g, b);
        }

        public static Color[] ToColorArray(object value)
        {
            if (IsNull(value)) return null;
            Type t = value.GetType();
            if (t.IsArray)
            {
                Array array = (Array)value;
                Color[] result = new Color[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = ToColor(array.GetValue(i));
                }
                return result;
            }
            else
            {
                return ToColorArray(value.ToString());
            }
        }

        public static Color[] ToColorArray(string valueString)
        {
            if (valueString == string.Empty) return null;
            valueString = valueString.Replace('；', ';').Trim().Trim(';');
            string[] colors = null;
            if (valueString.IndexOf(';') > -1)
            {
                colors = valueString.Split(';');
            }
            else
            {
                valueString = valueString.Replace('，', ',').Trim().Trim(',');
                colors = valueString.Split(',');
            }
            Color[] colorArray = new Color[colors.Length];
            for (int i = 0; i < colors.Length; i++)
            {
                colorArray[i] = ToColor(colors[i]);
            }
            return colorArray;
        }

        #endregion


#endif


        #region TIMESPAN
        public static TimeSpan ToTimeSpan(object value)
        {
            if (IsNull(value)) return TimeSpan.MinValue;
            if (value.GetType() == SystemTypes.TimeSpan) return (TimeSpan)value;
            return ToTimeSpan(value.ToString());
        }

        /// <summary>
        /// 从TimeSpan格式的字符串中生成TimeSpan对象
        /// </summary>
        /// <param name="valueString"></param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpan(string valueString)
        {
            string[] nums = valueString.Replace('：', ':').Split(':');
            int days = 0, hours = 0, minutes = 0, seconds = 0, mills = 0;
            int index = 0;
            if (index < nums.Length)
            {
                days = Convert.ToInt32(nums[index]);
                index++;
            }
            if (index < nums.Length)
            {
                hours = Convert.ToInt32(nums[index]);
                index++;
            }
            if (index < nums.Length)
            {
                minutes = Convert.ToInt32(nums[index]);
                index++;
            }
            if (index < nums.Length)
            {
                seconds = Convert.ToInt32(nums[index]);
                index++;
            }
            if (index < nums.Length)
            {
                mills = Convert.ToInt32(nums[index]);
                index++;
            }
            return new TimeSpan(days, hours, minutes, seconds, mills);
        }

        public static TimeSpan[] ToTimeSpaneArray(object value)
        {
            if (IsNull(value)) return null;
            Type t = value.GetType();
            if (t.IsArray)
            {
                Array array = (Array)value;
                TimeSpan[] result = new TimeSpan[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = ToTimeSpan(array.GetValue(i));
                }
                return result;
            }
            else
            {
                return ToTimeSpaneArray(value.ToString());
            }
        }

        public static TimeSpan[] ToTimeSpaneArray(string valueString)
        {
            if (valueString == string.Empty) return null;
            valueString = valueString.Replace('；', ';').Trim().Trim(';');
            string[] stringS = null;
            if (valueString.IndexOf(';') > -1)
            {
                stringS = valueString.Split(';');
            }
            else
            {
                valueString = valueString.Replace('，', ',').Trim().Trim(',');
                stringS = valueString.Split(',');
            }
            TimeSpan[] result = new TimeSpan[stringS.Length];
            for (int i = 0; i < stringS.Length; i++)
            {
                result[i] = ToTimeSpan(stringS[i]);
            }
            return result;
        }

        #endregion

        #region ENUM
        public static Enum ToEnum(Type enumType, object value, bool ignoreCase)
        {
            if (IsNull(value)) throw new ArgumentException();
            Type t = value.GetType();
            if (t.BaseType == SystemTypes.Enum)
            {
                UInt64 num = Convert.ToUInt64(value);
                return (Enum)Enum.Parse(enumType, num.ToString(), ignoreCase);
            }
            else
                return (Enum)Enum.Parse(enumType, value.ToString(), ignoreCase);
        }

        public static Enum ToEnum(Type enumType, object value)
        {
            return ToEnum(enumType, value, true);
        }
        #endregion

        #region STRING
        /// <summary>
        /// 将给定的对象转换为参数形式的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(object value)
        {
            string result = null;
            if (!IsNull(value))
            {
                result = "";
                Type t = value.GetType();
                switch (t.Name)
                {
                    #region 基本类型的转换
                    case "String[]":
                    string[] stringArray = (string[])value;
                    foreach (string item in stringArray)
                    {
                        if (result != string.Empty)
                            result += ",";
                        result += item;
                    }
                    break;
                    case "Single":
                    case "Boolean":
                    case "Int32":
                    case "Char":
                    case "Byte":
                    case "Int64":
                    case "Int16":
                    case "Decimal":
                    case "UInt64":
                    case "Double":
                    case "UInt32":
                    case "SByte":
                    case "String":
                    result = value.ToString();
                    break;
                    case "Int32[]":
                    int[] intArray = (int[])value;
                    foreach (int item in intArray)
                    {
                        if (result != string.Empty)
                            result += ",";
                        result += item.ToString();
                    }
                    break;
                    case "Single[]":
                    Single[] floatArray = (Single[])value;
                    foreach (Single item in floatArray)
                    {
                        if (result != string.Empty)
                            result += ",";
                        result += item.ToString();
                    }
                    break;
                    case "Double[]":
                    Double[] doubleArray = (Double[])value;
                    foreach (Double item in doubleArray)
                    {
                        if (result != string.Empty)
                            result += ",";
                        result += item.ToString();
                    }
                    break;
                    case "Decimal[]":
                    Decimal[] decimalArray = (Decimal[])value;
                    foreach (Decimal item in decimalArray)
                    {
                        if (result != string.Empty)
                            result += ",";
                        result += item.ToString();
                    }
                    break;
                    case "UInt32[]":
                    UInt32[] uintArray = (UInt32[])value;
                    foreach (UInt32 item in uintArray)
                    {
                        if (result != string.Empty)
                            result += ",";
                        result += item.ToString();
                    }
                    break;
                    case "UInt64[]":
                    UInt64[] uint64Array = (UInt64[])value;
                    foreach (UInt64 item in uint64Array)
                    {
                        if (result != string.Empty)
                            result += ",";
                        result += item.ToString();
                    }
                    break;
                    case "Int64[]":
                    Int64[] int64Array = (Int64[])value;
                    foreach (Int64 item in int64Array)
                    {
                        if (result != string.Empty)
                            result += ",";
                        result += item.ToString();
                    }
                    break;
                    case "Int16[]":
                    Int16[] int16Array = (Int16[])value;
                    foreach (Int16 item in int16Array)
                    {
                        if (result != string.Empty)
                            result += ",";
                        result += item.ToString();
                    }
                    break;
                    case "Char[]":
                    Char[] charArray = (Char[])value;
                    result = new string(charArray);
                    break;

                    case "Byte[]":
                    Byte[] byteArray = (Byte[])value;
                    foreach (Byte item in byteArray)
                    {
                        if (result != string.Empty)
                            result += ",";
                        result += item.ToString();
                    }
                    break;

                    case "SByte[]":
                    SByte[] sbyteArray = (SByte[])value;
                    foreach (SByte item in sbyteArray)
                    {
                        if (result != string.Empty)
                            result += ",";
                        result += item.ToString();
                    }
                    break;
                    case "DateTime":
                    DateTime time = (DateTime)value;
                    result = time.ToString("yyyy-MM-dd HH:mm:ss");
                    break;
                    //case "Size":
                    //    Size size = (Size)value;
                    //    result = size.Width.ToString() + "," + size.Height.ToString();
                    //    break;
                    //case "Size[]":
                    //    Size[] sizeArray = (Size[])value;
                    //    foreach (Size item in sizeArray)
                    //    {
                    //        if (result != string.Empty)
                    //            result += ";";
                    //        result += item.Width.ToString() + "," + item.Height.ToString();
                    //    }
                    //    break;
                    //case "SizeF":
                    //    SizeF sizef = (SizeF)value;
                    //    result = sizef.Width.ToString() + "," + sizef.Height.ToString();
                    //    break;
                    //case "SizeF[]":
                    //    SizeF[] sizefArray = (SizeF[])value;
                    //    foreach (SizeF item in sizefArray)
                    //    {
                    //        if (result != string.Empty)
                    //            result += ";";
                    //        result += item.Width.ToString() + "," + item.Height.ToString();
                    //    }
                    //    break;
                    //case "Point":
                    //    Point point = (Point)value;
                    //    result = point.X.ToString() + "," + point.Y.ToString();
                    //    break;
                    //case "Point[]":
                    //    Point[] pointArray = (Point[])value;
                    //    foreach (Point item in pointArray)
                    //    {
                    //        if (result != string.Empty)
                    //            result += ";";
                    //        result += item.X.ToString() + "," + item.Y.ToString();
                    //    }
                    //    break;
                    //case "PointF":
                    //    PointF pointf = (PointF)value;
                    //    result = pointf.X.ToString() + "," + pointf.Y.ToString();
                    //    break;
                    //case "PointF[]":
                    //    PointF[] pointfArray = (PointF[])value;
                    //    foreach (PointF item in pointfArray)
                    //    {
                    //        if (result != string.Empty)
                    //            result += ";";
                    //        result += item.X.ToString() + "," + item.Y.ToString();
                    //    }
                    //    break;
                    //case "RectangleF":
                    //    RectangleF rectanglef = (RectangleF)value;
                    //    result = rectanglef.X.ToString() + "," + rectanglef.Y.ToString() + "," + rectanglef.Width.ToString() + "," + rectanglef.Height.ToString();
                    //    break;
                    //case "RectangleF[]":
                    //    RectangleF[] rectangleFArray = (RectangleF[])value;
                    //    foreach (RectangleF item in rectangleFArray)
                    //    {
                    //        if (result != string.Empty)
                    //            result += ";";
                    //        result += item.X.ToString() + "," + item.Y.ToString() + "," + item.Width.ToString() + "," + item.Height.ToString();
                    //    }
                    //    break;
                    //case "Rectangle":
                    //    Rectangle rectangle = (Rectangle)value;
                    //    result = rectangle.X.ToString() + "," + rectangle.Y.ToString() + "," + rectangle.Width.ToString() + "," + rectangle.Height.ToString();
                    //    break;
                    //case "Rectangle[]":
                    //    Rectangle[] rectangleArray = (Rectangle[])value;
                    //    foreach (Rectangle item in rectangleArray)
                    //    {
                    //        if (result != string.Empty)
                    //            result += ";";
                    //        result += item.X.ToString() + "," + item.Y.ToString() + "," + item.Width.ToString() + "," + item.Height.ToString();
                    //    }
                    //    break;
                    case "TimeSpan":
                    TimeSpan ts = (TimeSpan)value;
                    result = ts.Days.ToString() + ":" + ts.Hours.ToString() + ":" + ts.Minutes.ToString() + ":" + ts.Seconds.ToString() + ":" + ts.Milliseconds.ToString();
                    break;
                    //case "Color":
                    //    Color color = (Color)value;
                    //    result = "#" + Convert.ToString(color.A, 16).PadLeft(2, '0') + Convert.ToString(color.R, 16).PadLeft(2, '0') + Convert.ToString(color.G, 16).PadLeft(2, '0') + Convert.ToString(color.B, 16).PadLeft(2, '0');
                    //    break;
                    //case "Color[]":
                    //    Color[] colorArray = (Color[])value;
                    //    foreach (Color item in colorArray)
                    //    {
                    //        if (result != string.Empty)
                    //            result += ",";
                    //        result += "#" + Convert.ToString(item.A, 16).PadLeft(2, '0') + Convert.ToString(item.R, 16).PadLeft(2, '0') + Convert.ToString(item.G, 16).PadLeft(2, '0') + Convert.ToString(item.B, 16).PadLeft(2, '0');
                    //    }
                    //    break;

                    #endregion
                    default:
                    result = value.ToString();
                    break;
                }
            }
            return result;
        }

        public static string ToString(string valueString)
        {
            return valueString;
        }

        public static String[] ToStringArray(object value)
        {
            if (IsNull(value)) return null;
            Type t = value.GetType();
            if (t.IsArray)
            {
                Array array = (Array)value;
                String[] result = new String[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = ToString(array.GetValue(i));
                }
                return result;
            }
            else
            {
                return ToStringArray(value.ToString());
            }
        }

        /// <summary>
        /// 转换为字符串数组，每个数组之间用分号格开，如果需要表示分号，用\;代替
        /// </summary>
        /// <param name="valueString"></param>
        /// <returns></returns>
        public static String[] ToStringArray(string valueString)
        {
            if (valueString == string.Empty) return new string[] { string.Empty };
            valueString.Replace("\\,", "\b");
            string[] result = valueString.Split(',');
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = result[i].Replace("\b", ",");
            }
            return result;
        }
        #endregion

        #region DOUBLE

        public static Double ToDouble(object value)
        {
            if (IsNull(value)) return 0;
            if (value.GetType() == SystemTypes.Double) return (double)value;
            return ToDouble(value.ToString());
        }

        public static Double ToDouble(string valueString)
        {
            if (!RegexHelper.IsNum(valueString)) return 0;
            return Convert.ToDouble(valueString);
        }

        public static Double[] ToDoubleArray(object value)
        {
            if (IsNull(value)) return null;
            Type t = value.GetType();
            if (t.IsArray)
            {
                Array array = (Array)value;
                Double[] result = new Double[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = ToDouble(array.GetValue(i));
                }
                return result;
            }
            else
            {
                return ToDoubleArray(value.ToString());
            }
        }

        public static Double[] ToDoubleArray(string valueString)
        {
            if (!RegexHelper.IsNumArray(valueString)) return null;
            MatchCollection matches = Regex.Matches(valueString, RegexHelper.NumberPattern);
            Double[] result = new Double[matches.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Convert.ToDouble(matches[i].Value);
            }
            return result;
        }

        #endregion

        #region UInt64

        public static UInt64 ToUInt64(object value)
        {
            if (IsNull(value)) return 0;
            if (value.GetType() == SystemTypes.UInt64) return (UInt64)value;
            return ToUInt64(value.ToString());
        }

        public static UInt64 ToUInt64(string valueString)
        {
            if (!RegexHelper.IsNum(valueString)) return 0;
            return Convert.ToUInt64(valueString);
        }

        public static UInt64[] ToUInt64Array(object value)
        {
            if (IsNull(value)) return null;
            Type t = value.GetType();
            if (t.IsArray)
            {
                Array array = (Array)value;
                UInt64[] result = new UInt64[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = ToUInt64(array.GetValue(i));
                }
                return result;
            }
            else
            {
                return ToUInt64Array(value.ToString());
            }
        }

        public static UInt64[] ToUInt64Array(string valueString)
        {
            if (!RegexHelper.IsNumArray(valueString)) return null;
            MatchCollection matches = Regex.Matches(valueString, RegexHelper.NumberPattern);
            UInt64[] result = new UInt64[matches.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Convert.ToUInt64(matches[i].Value);
            }
            return result;
        }

        #endregion

        #region UInt16

        public static UInt16 ToUInt16(object value)
        {
            if (IsNull(value)) return 0;
            if (value.GetType() == SystemTypes.UInt16) return (UInt16)value;
            return ToUInt16(value.ToString());
        }

        public static UInt16 ToUInt16(string valueString)
        {
            if (!RegexHelper.IsNum(valueString)) return 0;
            return Convert.ToUInt16(valueString);
        }

        public static UInt16[] ToUInt16Array(object value)
        {
            if (IsNull(value)) return null;
            Type t = value.GetType();
            if (t.IsArray)
            {
                Array array = (Array)value;
                UInt16[] result = new UInt16[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = ToUInt16(array.GetValue(i));
                }
                return result;
            }
            else
            {
                return ToUInt16Array(value.ToString());
            }
        }

        public static UInt16[] ToUInt16Array(string valueString)
        {
            if (!RegexHelper.IsNumArray(valueString)) return null;
            MatchCollection matches = Regex.Matches(valueString, RegexHelper.NumberPattern);
            UInt16[] result = new UInt16[matches.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Convert.ToUInt16(matches[i].Value);
            }
            return result;
        }

        #endregion

        #region SByte

        public static SByte ToSByte(object value)
        {
            if (IsNull(value)) return 0;
            if (value.GetType() == SystemTypes.SByte) return (SByte)value;
            return ToSByte(value.ToString());
        }

        public static SByte ToSByte(string valueString)
        {
            if (!RegexHelper.IsNum(valueString)) return 0;
            return Convert.ToSByte(valueString);
        }

        public static SByte[] ToSByteArray(object value)
        {
            if (IsNull(value)) return null;
            Type t = value.GetType();
            if (t.IsArray)
            {
                Array array = (Array)value;
                SByte[] result = new SByte[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = ToSByte(array.GetValue(i));
                }
                return result;
            }
            else
            {
                return ToSByteArray(value.ToString());
            }
        }

        public static SByte[] ToSByteArray(string valueString)
        {
            if (!RegexHelper.IsNumArray(valueString)) return null;
            MatchCollection matches = Regex.Matches(valueString, RegexHelper.NumberPattern);
            SByte[] result = new SByte[matches.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Convert.ToSByte(matches[i].Value);
            }
            return result;
        }

        #endregion

        #region Int64

        public static Int64 ToInt64(object value)
        {
            if (IsNull(value)) return 0;
            if (value.GetType() == SystemTypes.Int64) return (Int64)value;
            return ToInt64(value.ToString());
        }

        public static Int64 ToInt64(string valueString)
        {
            if (!RegexHelper.IsNum(valueString)) return 0;
            return Convert.ToInt64(valueString);
        }

        public static Int64 ToInt64Hex(object value)
        {
            if (IsNull(value)) return 0;
            if (value.GetType() == SystemTypes.Int64) return (Int64)value;
            return ToInt64Hex(value.ToString());
        }

        public static Int64 ToInt64Hex(string valueString)
        {
            if (!RegexHelper.IsNum(valueString, 16)) return 0;
            return Convert.ToInt64(valueString, 16);
        }

        public static Int64[] ToInt64Array(object value)
        {
            if (IsNull(value)) return null;
            Type t = value.GetType();
            if (t.IsArray)
            {
                Array array = (Array)value;
                Int64[] result = new Int64[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = ToInt64(array.GetValue(i));
                }
                return result;
            }
            else
            {
                return ToInt64Array(value.ToString());
            }
        }

        public static Int64[] ToInt64Array(string valueString)
        {
            if (!RegexHelper.IsNumArray(valueString)) return null;
            MatchCollection matches = Regex.Matches(valueString, RegexHelper.NumberPattern);
            Int64[] result = new Int64[matches.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Convert.ToInt64(matches[i].Value);
            }
            return result;
        }

        #endregion

        #region UInt32

        public static UInt32 ToUInt32(object value)
        {
            if (IsNull(value)) return 0;
            if (value.GetType() == SystemTypes.UInt32) return (UInt32)value;
            return ToUInt32(value.ToString());
        }

        public static UInt32 ToUInt32(string valueString)
        {
            if (!RegexHelper.IsNum(valueString)) return 0;
            return Convert.ToUInt32(valueString);
        }

        public static UInt32[] ToUInt32Array(object value)
        {
            if (IsNull(value)) return null;
            Type t = value.GetType();
            if (t.IsArray)
            {
                Array array = (Array)value;
                UInt32[] result = new UInt32[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = ToUInt32(array.GetValue(i));
                }
                return result;
            }
            else
            {
                return ToUInt32Array(value.ToString());
            }
        }

        public static UInt32[] ToUInt32Array(string valueString)
        {
            if (!RegexHelper.IsNumArray(valueString)) return null;
            MatchCollection matches = Regex.Matches(valueString, RegexHelper.NumberPattern);
            UInt32[] result = new UInt32[matches.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Convert.ToUInt32(matches[i].Value);
            }
            return result;
        }

        #endregion

        #region Int32

        public static Int32 ToInt32(object value)
        {
            if (IsNull(value)) return 0;
            if (value.GetType() == SystemTypes.Int32) return (Int32)value;
            return ToInt32(value.ToString());
        }

        public static Int32 ToInt32(string valueString)
        {
            //if (!RegexHelper.IsNum(valueString)) return 0;
            int result = 0;
            int.TryParse(valueString, out result);
            return result;
        }

        public static Int32[] ToInt32Array(object value)
        {
            if (IsNull(value)) return null;
            Type t = value.GetType();
            if (t.IsArray)
            {
                Array array = (Array)value;
                Int32[] result = new Int32[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = ToInt32(array.GetValue(i));
                }
                return result;
            }
            else
            {
                return ToInt32Array(value.ToString());
            }
        }

        public static Int32[] ToInt32Array(string valueString)
        {
            if (!RegexHelper.IsNumArray(valueString)) return null;
            MatchCollection matches = Regex.Matches(valueString, RegexHelper.NumberPattern);
            Int32[] result = new Int32[matches.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Convert.ToInt32(matches[i].Value);
            }
            return result;
        }

        #endregion

        #region Int16

        public static Int16 ToInt16(object value)
        {
            if (IsNull(value)) return 0;
            if (value.GetType() == SystemTypes.Int16) return (Int16)value;
            return ToInt16(value.ToString());
        }

        public static Int16 ToInt16(string valueString)
        {
            if (!RegexHelper.IsNum(valueString)) return 0;
            return Convert.ToInt16(valueString);
        }

        public static Int16[] ToInt16Array(object value)
        {
            if (IsNull(value)) return null;
            Type t = value.GetType();
            if (t.IsArray)
            {
                Array array = (Array)value;
                Int16[] result = new Int16[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = ToInt16(array.GetValue(i));
                }
                return result;
            }
            else
            {
                return ToInt16Array(value.ToString());
            }
        }

        public static Int16[] ToInt16Array(string valueString)
        {
            if (!RegexHelper.IsNumArray(valueString)) return null;
            MatchCollection matches = Regex.Matches(valueString, RegexHelper.NumberPattern);
            Int16[] result = new Int16[matches.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Convert.ToInt16(matches[i].Value);
            }
            return result;
        }

        #endregion

        #region DECIMAL

        public static Decimal ToDecimal(object value)
        {
            if (IsNull(value)) return 0;
            if (value.GetType() == SystemTypes.Decimal) return (Decimal)value;
            return ToDecimal(value.ToString());
        }

        public static decimal ToDecimal(string valueString)
        {
            if (!RegexHelper.IsNum(valueString)) return 0;
            return Convert.ToDecimal(valueString);
        }

        public static Decimal[] ToDecimalArray(object value)
        {
            if (IsNull(value)) return null;
            Type t = value.GetType();
            if (t.IsArray)
            {
                Array array = (Array)value;
                Decimal[] result = new Decimal[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = ToDecimal(array.GetValue(i));
                }
                return result;
            }
            else
            {
                return ToDecimalArray(value.ToString());
            }
        }

        public static Decimal[] ToDecimalArray(string valueString)
        {
            if (!RegexHelper.IsNumArray(valueString)) return null;
            MatchCollection matches = Regex.Matches(valueString, RegexHelper.NumberPattern);
            Decimal[] result = new Decimal[matches.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Convert.ToDecimal(matches[i].Value);
            }
            return result;
        }

        #endregion

        #region BYTE

        public static Byte ToByte(object value)
        {
            if (IsNull(value)) return 0;
            if (value.GetType() == SystemTypes.Byte) return (Byte)value;
            return ToByte(value.ToString());
        }

        public static Byte ToByte(object value, int ibase)
        {
            if (IsNull(value)) return 0;
            if (value.GetType() == SystemTypes.Byte) return (Byte)value;
            return ToByte(value.ToString(), ibase);
        }

        public static Byte[] ToByteArray(object value)
        {
            if (IsNull(value)) return null;
            Type t = value.GetType();
            if (t.IsArray)
            {
                Array array = (Array)value;
                Byte[] result = new Byte[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = ToByte(array.GetValue(i));
                }
                return result;
            }
            else
            {
                return ToByteArray(value.ToString());
            }
        }

        public static byte ToByte(string valueString, int ibase)
        {
            if (!RegexHelper.IsNum(valueString, ibase)) return byte.MinValue;
            return Convert.ToByte(valueString, ibase);
        }

        public static byte ToByte(string valueString)
        {
            if (!RegexHelper.IsNum(valueString)) return byte.MinValue;
            return Convert.ToByte(valueString);
        }

        public static byte[] ToByteArray(string valueString)
        {
            if (!RegexHelper.IsNumArray(valueString)) return null;
            MatchCollection matches = Regex.Matches(valueString, RegexHelper.NumberPattern);
            Byte[] result = new Byte[matches.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Convert.ToByte(matches[i].Value);
            }
            return result;
        }


        #endregion

        #region Single

        public static Single ToSingle(object value)
        {
            if (IsNull(value)) return 0;
            if (value.GetType() == SystemTypes.Single) return (Single)value;
            return ToSingle(value.ToString());
        }

        public static Single ToSingle(string valueString)
        {
            if (string.IsNullOrEmpty(valueString)) return 0;
            valueString = valueString.Trim();
            if (!RegexHelper.IsNum(valueString)) return 0;
            return Convert.ToSingle(valueString);
        }

        public static Single[] ToSingleArray(object value)
        {
            if (IsNull(value)) return null;
            Type t = value.GetType();
            if (t.IsArray)
            {
                Array array = (Array)value;
                Single[] result = new Single[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = ToSingle(array.GetValue(i));
                }
                return result;
            }
            else
            {
                return ToSingleArray(value.ToString());
            }
        }

        public static Single[] ToSingleArray(string valueString)
        {
            if (!RegexHelper.IsNumArray(valueString)) return null;
            MatchCollection matches = Regex.Matches(valueString, RegexHelper.NumberPattern);
            Single[] result = new Single[matches.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Convert.ToSingle(matches[i].Value);
            }
            return result;
        }

        #endregion

        #region BOOL

        public static Boolean ToBoolean(object value)
        {
            if (IsNull(value)) return false;
            if (value.GetType() == SystemTypes.Boolean) return (Boolean)value;
            return ToBoolean(value.ToString().ToLower());
        }

        /// <summary>
        /// 将字符串转换为布尔值
        /// </summary>
        /// <param name="valueString"></param>
        /// <returns></returns>
        public static bool ToBoolean(string valueString)
        {
            if (valueString == string.Empty) return false;
            if (valueString == "true" || valueString[0] == 't' || valueString[0] == 'T' || (RegexHelper.IsNum(valueString) && valueString[0] != '0'))
                return true;
            return false;
        }

        public static Boolean[] ToBooleanArray(object value)
        {
            if (IsNull(value)) return null;
            Type t = value.GetType();
            if (t.IsArray)
            {
                Array array = (Array)value;
                bool[] result = new bool[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = ToBoolean(array.GetValue(i));
                }
                return result;
            }
            else
            {
                return ToBooleanArray(value.ToString());
            }
        }

        public static bool[] ToBooleanArray(string valueString)
        {
            if (valueString == string.Empty) return null;
            if (RegexHelper.IsNumArray(valueString))
            {
                MatchCollection matches = Regex.Matches(valueString, RegexHelper.NumberPattern);
                Boolean[] result = new Boolean[matches.Count];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = Convert.ToBoolean(matches[i].Value);
                }
                return result;
            }
            else
            {
                string[] array = null;
                if (valueString.IndexOf(";") > -1)
                {
                    array = valueString.Split(';');
                }
                else if (valueString.IndexOf(",") > -1)
                {
                    array = valueString.Split(',');
                }
                if (array == null) return null;
                bool[] result = new Boolean[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = ToBoolean(array[i]);
                }
                return result;
            }
        }

        #endregion

        #region CHAR
        public static Char ToChar(object value)
        {
            if (IsNull(value)) return Char.MinValue;
            if (value.GetType() == SystemTypes.Char) return (Char)value;
            return ToChar(value.ToString());
        }

        public static Char ToChar(string valueString)
        {
            if (valueString == String.Empty) return Char.MinValue;
            return valueString[0];
        }

        public static Char[] ToCharArray(object value)
        {
            if (IsNull(value)) return null;
            Type t = value.GetType();
            if (t.IsArray)
            {
                Array array = (Array)value;
                Char[] result = new Char[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = ToChar(array.GetValue(i));
                }
                return result;
            }
            else
            {
                return ToCharArray(value.ToString());
            }

        }

        public static Char[] ToCharArray(string valueString)
        {
            return valueString.ToCharArray();
        }
        #endregion


        #region DateTime

        static DateTime timeStampbase = new DateTime(1970, 1, 1);
        /// <summary>
        /// 返回js脚本中调用getTime时返回的数字
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime ToTimestampTime(this long time)
        {
            var l = time * 10000 + (timeStampbase - DateTime.MinValue).Ticks;
            return new DateTime(l);
        }

        /// <summary>
        /// 返回js脚本中调用getTime时返回的数字
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime ToTimestampTime(this int time)
        {
            var l = time * 10 + (timeStampbase - DateTime.MinValue).Ticks;
            return new DateTime(l);
        }

        public static DateTime ToDateTime(object value)
        {
            if (IsNull(value)) return DateTime.MinValue;
            else if (value.GetType() == SystemTypes.DateTime) return (DateTime)value;
            else if (value.GetType() == typeof(TimeSpan))
            {
                var ts = (TimeSpan)value;
                var now = DateTime.Now;
                var time = new DateTime(now.Year, now.Month, now.Day);
                time.AddSeconds(ts.TotalSeconds);
                return time;
            }
            else if (value.GetType() == typeof(long))
                return ((long)value).ToTimestampTime();
            else if (value.GetType() == typeof(int))
                return (1000L * (int)value).ToTimestampTime();
            return ToDateTime(value.ToString());
        }

        public static DateTime ToDateTime(string valueString)
        {
            if (string.IsNullOrEmpty(valueString) || string.IsNullOrWhiteSpace(valueString)) return DateTime.MinValue;
            if (RegexHelper.IsTime_CST(valueString))
                return DateTime.ParseExact(valueString, "ddd MMM dd HH:mm:ss CST yyyy", Culis.en_us);
            else
            {
                var regGmt = new Regex(@"([A-Z][a-z]{2}),\s(\d{2})[\s|-]([A-Z][a-z]{2})[\s|-](\d{2,4})\s(\d{2}):(\d{2}):(\d{2})\sGMT");
                var gmtMatch = regGmt.Match(valueString);
                if (gmtMatch.Success)
                {
                    //"Fri, 02-Jan-2020 00:00:00 GMT";

                    int offset = 1;
                    var xqstr = gmtMatch.Groups[offset++].Value;
                    var dayStr = gmtMatch.Groups[offset++].Value;
                    var monStr = gmtMatch.Groups[offset++].Value;
                    var yearStr = gmtMatch.Groups[offset++].Value;
                    var hourStr = gmtMatch.Groups[offset++].Value;
                    var minStr = gmtMatch.Groups[offset++].Value;
                    var secStr = gmtMatch.Groups[offset++].Value;
                    if (yearStr.Length < 4) yearStr = "20" + yearStr;

                    switch (monStr.Length)
                    {
                        case 3: return DateTime.ParseExact(yearStr + monStr + dayStr + hourStr + minStr + secStr, "yyyyMMMddHHmmss", Culis.en_us);
                        case 2: return DateTime.ParseExact(yearStr + monStr + dayStr + hourStr + minStr + secStr, "yyyyMMddHHmmss", Culis.en_us);
                    }
                }

                var regExcel = new Regex(@"(\d{1,2})-(\d{1,2})月-(\d{1,4})");
                var excelMath = regExcel.Match(valueString);
                if (excelMath.Success)
                {
                    //"12-3月-2020";

                    int offset = 1;
                    var dayStr = excelMath.Groups[offset++].Value.PadLeft(2, '0');
                    var monStr = excelMath.Groups[offset++].Value.PadLeft(2, '0');
                    var yearStr = excelMath.Groups[offset++].Value;
                    return DateTime.ParseExact(yearStr + monStr + dayStr, "yyyyMMdd", Culis.en_us);
                }

                if (valueString.IsNum() && valueString.Length == 14)
                {
                    return DateTime.ParseExact(valueString, "yyyyMMddHHmmss", Culis.en_us);
                }

                if (valueString.IsNum() && valueString.Length == 12)
                {
                    return DateTime.ParseExact(valueString, "yyyyMMddHHmm", Culis.en_us);
                }

                if (valueString.IsNum() && valueString.Length == 10)
                {
                    return DateTime.ParseExact(valueString, "yyyyMMddHH", Culis.en_us);
                }

                if (valueString.IsNum() && valueString.Length == 8)
                {
                    return DateTime.ParseExact(valueString, "yyyyMMdd", Culis.en_us);
                }

                if (valueString.IsNum() && valueString.Length == 6)
                {
                    return DateTime.ParseExact(valueString, "yyyyMM", Culis.en_us);
                }



                DateTime time;
                if (DateTime.TryParse(valueString, out time))
                    return time;
                return DateTime.MinValue;
            }
        }

        public static DateTime[] ToDateTimeArray(object value)
        {
            if (IsNull(value)) return null;
            Type t = value.GetType();
            if (t.IsArray)
            {
                Array array = (Array)value;
                DateTime[] result = new DateTime[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = ToDateTime(array.GetValue(i));
                }
                return result;
            }
            else
            {
                return ToDateTimeArray(value.ToString());
            }
        }

        public static DateTime[] ToDateTimeArray(string valueString)
        {
            if (valueString == string.Empty) return null;
            valueString = valueString.Replace('；', ';').Trim().Trim(';');
            string[] dateTimes = null;
            if (valueString.IndexOf(';') > -1)
            {
                dateTimes = valueString.Split(';');
            }
            else
            {
                valueString = valueString.Replace('，', ',').Trim().Trim(',');
                dateTimes = valueString.Split(',');
            }
            DateTime[] result = new DateTime[dateTimes.Length];
            for (int i = 0; i < dateTimes.Length; i++)
            {
                result[i] = ToDateTime(dateTimes[i]);
            }
            return result;
        }
        #endregion

        #endregion

        #region System.Net
#if !SILVERLIGHT
        public static IPEndPoint ToEndPoint(object value)
        {
            if (IsNull(value)) return null;
            if (value.GetType() == typeof(IPEndPoint)) return (IPEndPoint)value;
            return ToEndPoint(value.ToString());
        }

        static IPEndPoint ToEndPoint(string value)
        {
            if (value == string.Empty) return null;
            //if (!RegexHelper.IsEndPoint(value)) return null;
            if (value.IndexOf(':') == -1) value = value + ":80";
            string[] strArray = value.Split(':');
            IPAddress address = GetAddress(strArray[0]);
            if (address != null)
            {
                return new IPEndPoint(address, Oct.ToInt32(strArray[1]));
            }
            return null;
        }

        public static IPAddress GetAddress(string addressString)
        {
            IPAddress address = null;
            if (Regex.IsMatch(addressString, "\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}"))
            {
                string ipstr = addressString;
                if (addressString.Contains(".0") || addressString.StartsWith("0"))
                {
                    string[] split = addressString.Split('.');
                    ipstr = string.Empty;

                    for (int i = 0; i < split.Length; i++)
                    {
                        if (i != 0)
                            ipstr += ".";
                        ipstr += Convert.ToInt32(split[i]).ToString();
                    }
                }
                //address = IPAddress.Parse(ipstr);
                var suc = IPAddress.TryParse(ipstr, out address);
            }
            else
            {
                try
                {
                    
                    IPHostEntry ent = Dns.GetHostEntry(addressString);
                    address = ent.AddressList[0];
                }
                catch
                {

                }
            }
            return address;
        }

        public static IPEndPoint[] ToEndPointArray(object value)
        {
            if (IsNull(value)) return null;
            if (value.GetType() == typeof(IPEndPoint[])) return (IPEndPoint[])value;
            return ToEndPointArray(value.ToString());
        }

        static IPEndPoint[] ToEndPointArray(string value)
        {
            string[] strArray = value.Split(';');
            List<IPEndPoint> lst = new List<IPEndPoint>();
            for (int i = 0; i < strArray.Length; i++)
            {
                IPEndPoint ep = ToEndPoint(strArray[i]);
                if (ep != null)
                    lst.Add(ep);
            }
            return lst.ToArray();
        }

#else

		private static DnsEndPoint ToEndPoint(string value)
		{
			string[] array = value.Split(':');
			if(array.Length < 2 || !RegexHelper.IsNum(array[1]))
				throw new ArgumentException("无法将“" + value + "”解析为有效的服务器IP地址！");
			return new DnsEndPoint(array[0], Convert.ToInt32(array[1]));
		}

		public static DnsEndPoint ToEndPoint(object value)
		{
			if(IsNull(value)) return null;
			if(value.GetType() == typeof(IPEndPoint)) return (DnsEndPoint)value;
			return ToEndPoint(value.ToString());
		}

#endif
        #endregion

        public static bool IsNull(object target)
        {
            return target == null || target == DBNull.Value;
        }

        /// <summary>
        /// 将typeSrc类型的数字转换成dst类型的数字，srcValue是需要转换的值
        /// </summary>
        /// <param name="typeSrc"></param>
        /// <param name="srcValue"></param>
        /// <param name="dst"></param>
        /// <returns></returns>
        public static object ConvertNumber(Type typeSrc, object srcValue, Type dst)
        {
            switch (typeSrc.Name)
            {
                case "Int32":
                switch (dst.Name)
                {
                    case "Int32":
                    return srcValue;
                    case "UInt32":
                    return (UInt32)(Int32)srcValue;
                    case "Int64":
                    return (Int64)(Int32)srcValue;
                    case "UInt64":
                    return (UInt64)(Int32)srcValue;
                    case "Int16":
                    return (Int16)(Int32)srcValue;
                    case "UInt16":
                    return (UInt16)(Int32)srcValue;
                    case "Byte":
                    return (Byte)(Int32)srcValue;
                    case "SByte":
                    return (SByte)(Int32)srcValue;
                }
                break;
                case "UInt32":
                switch (dst.Name)
                {
                    case "Int32":
                    return (SByte)(UInt32)srcValue;
                    case "UInt32":
                    return (UInt32)(UInt32)srcValue;
                    case "Int64":
                    return (Int64)(UInt32)srcValue;
                    case "UInt64":
                    return (UInt64)(UInt32)srcValue;
                    case "Int16":
                    return (Int16)(UInt32)srcValue;
                    case "UInt16":
                    return (UInt16)(UInt32)srcValue;
                    case "Byte":
                    return (Byte)(UInt32)srcValue;
                    case "SByte":
                    return (SByte)(UInt32)srcValue;
                }
                break;
                case "Int64":
                switch (dst.Name)
                {
                    case "Int32":
                    return (Int32)(Int64)srcValue;
                    case "UInt32":
                    return (UInt32)(Int64)srcValue;
                    case "Int64":
                    return (Int64)(Int64)srcValue;
                    case "UInt64":
                    return (UInt64)(Int64)srcValue;
                    case "Int16":
                    return (Int16)(Int64)srcValue;
                    case "UInt16":
                    return (UInt16)(Int64)srcValue;
                    case "Byte":
                    return (Byte)(Int64)srcValue;
                    case "SByte":
                    return (SByte)(Int64)srcValue;
                }
                break;
                case "UInt64":
                switch (dst.Name)
                {
                    case "Int32":
                    return (Int32)(UInt64)srcValue;
                    case "UInt32":
                    return (UInt32)(UInt64)srcValue;
                    case "Int64":
                    return (Int64)(UInt64)srcValue;
                    case "UInt64":
                    return (UInt64)(UInt64)srcValue;
                    case "Int16":
                    return (Int16)(UInt64)srcValue;
                    case "UInt16":
                    return (UInt16)(UInt64)srcValue;
                    case "Byte":
                    return (Byte)(UInt64)srcValue;
                    case "SByte":
                    return (SByte)(UInt64)srcValue;
                }
                break;
                case "Int16":
                switch (dst.Name)
                {
                    case "Int32":
                    return (Int32)(Int16)srcValue;
                    case "UInt32":
                    return (UInt32)(Int16)srcValue;
                    case "Int64":
                    return (Int64)(Int16)srcValue;
                    case "UInt64":
                    return (UInt64)(Int16)srcValue;
                    case "Int16":
                    return (Int16)(Int16)srcValue;
                    case "UInt16":
                    return (UInt16)(Int16)srcValue;
                    case "Byte":
                    return (Byte)(Int16)srcValue;
                    case "SByte":
                    return (SByte)(Int16)srcValue;
                }
                break;
                case "UInt16":
                switch (dst.Name)
                {
                    case "Int32":
                    return (Int32)(UInt16)srcValue;
                    case "UInt32":
                    return (UInt32)(UInt16)srcValue;
                    case "Int64":
                    return (Int64)(UInt16)srcValue;
                    case "UInt64":
                    return (UInt64)(UInt16)srcValue;
                    case "Int16":
                    return (Int16)(UInt16)srcValue;
                    case "UInt16":
                    return (UInt16)(UInt16)srcValue;
                    case "Byte":
                    return (Byte)(UInt16)srcValue;
                    case "SByte":
                    return (SByte)(UInt16)srcValue;
                }
                break;
                case "Byte":
                switch (dst.Name)
                {
                    case "Int32":
                    return (Int32)(Byte)srcValue;
                    case "UInt32":
                    return (UInt32)(Byte)srcValue;
                    case "Int64":
                    return (Int64)(Byte)srcValue;
                    case "UInt64":
                    return (UInt64)(Byte)srcValue;
                    case "Int16":
                    return (Int16)(Byte)srcValue;
                    case "UInt16":
                    return (UInt16)(Byte)srcValue;
                    case "Byte":
                    return (Byte)(Byte)srcValue;
                    case "SByte":
                    return (SByte)(Byte)srcValue;
                }
                break;
                case "SByte":
                switch (dst.Name)
                {
                    case "Int32":
                    return (Int32)(SByte)srcValue;
                    case "UInt32":
                    return (UInt32)(SByte)srcValue;
                    case "Int64":
                    return (Int64)(SByte)srcValue;
                    case "UInt64":
                    return (UInt64)(SByte)srcValue;
                    case "Int16":
                    return (Int16)(SByte)srcValue;
                    case "UInt16":
                    return (UInt16)(SByte)srcValue;
                    case "Byte":
                    return (Byte)(SByte)srcValue;
                    case "SByte":
                    return (SByte)(SByte)srcValue;
                }
                break;

            }

            return srcValue;

        }
    }

    public class Culis
    {
        public static readonly CultureInfo en_us = new CultureInfo("en-us");
    }
}
