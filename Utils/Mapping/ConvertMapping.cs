using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Sails.Utils
{
    public delegate object ConvertMethodHandler(string value);
    public delegate object ConvertToObjectMethodHandler(string value);
    public delegate string ConvertToStringMethodHandler(object value);
    public delegate T ConvertMethodHandlerT<T>(string value);
    public delegate T ConvertToObjectMethodHandlerT<T>(string value);
    public delegate string ConvertToStringMethodHandler<T>(T value);

    public class ConvertMapping
    {
        #region 变量

        static Dictionary<object, object> ToObjectMap = new Dictionary<object, object>();
        static Dictionary<object, object> ToStringMap = new Dictionary<object, object>();

        #endregion

        static ConvertMapping()
        {
            #region ToObject

            SetMappingToObject(SystemTypes.Boolean, new ConvertMethodHandlerT<bool>(Oct.ToBoolean));
            SetMappingToObject(SystemTypes.BooleanArray, new ConvertMethodHandlerT<bool[]>(Oct.ToBooleanArray));
            SetMappingToObject(SystemTypes.Byte, new ConvertMethodHandlerT<byte>(Oct.ToByte));
            SetMappingToObject(SystemTypes.ByteArray, new ConvertMethodHandlerT<byte[]>(Oct.ToByteArray));
            SetMappingToObject(SystemTypes.Char, new ConvertMethodHandlerT<char>(Oct.ToChar));
            SetMappingToObject(SystemTypes.CharArray, new ConvertMethodHandlerT<char[]>(Oct.ToCharArray));
#if !SILVERLIGHT
            SetMappingToObject(SystemTypes.Color, new ConvertMethodHandlerT<Color>(Oct.ToColor));
            SetMappingToObject(SystemTypes.ColorArray, new ConvertMethodHandlerT<Color[]>(Oct.ToColorArray));
#endif
            SetMappingToObject(SystemTypes.DateTime, new ConvertMethodHandlerT<DateTime>(Oct.ToDateTime));
            SetMappingToObject(SystemTypes.DateTimeArray, new ConvertMethodHandlerT<DateTime[]>(Oct.ToDateTimeArray));
            SetMappingToObject(SystemTypes.Decimal, new ConvertMethodHandlerT<Decimal>(Oct.ToDecimal));
            SetMappingToObject(SystemTypes.DecimalArray, new ConvertMethodHandlerT<Decimal[]>(Oct.ToDecimalArray));
            SetMappingToObject(SystemTypes.Double, new ConvertMethodHandlerT<Double>(Oct.ToDouble));
            SetMappingToObject(SystemTypes.DoubleArray, new ConvertMethodHandlerT<Double[]>(Oct.ToDoubleArray));
            SetMappingToObject(SystemTypes.Int16, new ConvertMethodHandlerT<Int16>(Oct.ToInt16));
            SetMappingToObject(SystemTypes.Int16Array, new ConvertMethodHandlerT<Int16[]>(Oct.ToInt16Array));
            SetMappingToObject(SystemTypes.Int32, new ConvertMethodHandlerT<Int32>(Oct.ToInt32));
            SetMappingToObject(SystemTypes.Int32Array, new ConvertMethodHandlerT<Int32[]>(Oct.ToInt32Array));
            SetMappingToObject(SystemTypes.Int64, new ConvertMethodHandlerT<Int64>(Oct.ToInt64));
            SetMappingToObject(SystemTypes.Int64Array, new ConvertMethodHandlerT<Int64[]>(Oct.ToInt64Array));
#if !SILVERLIGHT
            SetMappingToObject(SystemTypes.Point, new ConvertMethodHandlerT<Point>(Oct.ToPoint));
            SetMappingToObject(SystemTypes.PointArray, new ConvertMethodHandlerT<Point[]>(Oct.ToPointArray));
            SetMappingToObject(SystemTypes.PointF, new ConvertMethodHandlerT<PointF>(Oct.ToPointF));
            SetMappingToObject(SystemTypes.PointFArray, new ConvertMethodHandlerT<PointF[]>(Oct.ToPointFArray));
            SetMappingToObject(SystemTypes.Rectangle, new ConvertMethodHandlerT<Rectangle>(Oct.ToRectangle));
            SetMappingToObject(SystemTypes.RectangleArray, new ConvertMethodHandlerT<Rectangle[]>(Oct.ToRectangleArray));
            SetMappingToObject(SystemTypes.RectangleF, new ConvertMethodHandlerT<RectangleF>(Oct.ToRectangleF));
            SetMappingToObject(SystemTypes.RectangleFArray, new ConvertMethodHandlerT<RectangleF[]>(Oct.ToRectangleFArray));
            SetMappingToObject(SystemTypes.Size, new ConvertMethodHandlerT<Size>(Oct.ToSize));
            SetMappingToObject(SystemTypes.SizeArray, new ConvertMethodHandlerT<Size[]>(Oct.ToSizeArray));
            SetMappingToObject(SystemTypes.SizeF, new ConvertMethodHandlerT<SizeF>(Oct.ToSizeF));
            SetMappingToObject(SystemTypes.SizeFArray, new ConvertMethodHandlerT<SizeF[]>(Oct.ToSizeFArray));
#endif
            SetMappingToObject(SystemTypes.SByte, new ConvertMethodHandlerT<SByte>(Oct.ToSByte));
            SetMappingToObject(SystemTypes.SByteArray, new ConvertMethodHandlerT<SByte[]>(Oct.ToSByteArray));
            SetMappingToObject(SystemTypes.Single, new ConvertMethodHandlerT<Single>(Oct.ToSingle));
            SetMappingToObject(SystemTypes.SingleArray, new ConvertMethodHandlerT<Single[]>(Oct.ToSingleArray));
            SetMappingToObject(SystemTypes.String, new ConvertMethodHandlerT<String>(Oct.ToString));
            SetMappingToObject(SystemTypes.StringArray, new ConvertMethodHandlerT<String[]>(Oct.ToStringArray));
            SetMappingToObject(SystemTypes.TimeSpan, new ConvertMethodHandlerT<TimeSpan>(Oct.ToTimeSpan));
            SetMappingToObject(SystemTypes.TimeSpanArray, new ConvertMethodHandlerT<TimeSpan[]>(Oct.ToTimeSpaneArray));
            SetMappingToObject(SystemTypes.UInt16, new ConvertMethodHandlerT<UInt16>(Oct.ToUInt16));
            SetMappingToObject(SystemTypes.UInt16Array, new ConvertMethodHandlerT<UInt16[]>(Oct.ToUInt16Array));
            SetMappingToObject(SystemTypes.UInt32, new ConvertMethodHandlerT<UInt32>(Oct.ToUInt32));
            SetMappingToObject(SystemTypes.UInt32Array, new ConvertMethodHandlerT<UInt32[]>(Oct.ToUInt32Array));
            SetMappingToObject(SystemTypes.UInt64, new ConvertMethodHandlerT<UInt64>(Oct.ToUInt64));
            SetMappingToObject(SystemTypes.UInt64Array, new ConvertMethodHandlerT<UInt64[]>(Oct.ToUInt64Array));

#if !SILVERLIGHT
            SetMappingToObject(typeof(IPEndPoint), new ConvertMethodHandlerT<IPEndPoint>(Oct.ToEndPoint));
            SetMappingToObject(typeof(IPEndPoint[]), new ConvertMethodHandlerT<IPEndPoint[]>(Oct.ToEndPointArray));
#endif

            ConvertMapping.SetMappingToObject(typeof(ProtocolType[]), new ConvertMethodHandler(Oct.ToProtocol));
            #endregion

            #region ToString

            SetMappingToString(SystemTypes.Boolean, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.BooleanArray, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.Byte, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.ByteArray, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.Char, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.CharArray, new ConvertToStringMethodHandler(Oct.ToString));
#if !SILVERLIGHT
            SetMappingToString(SystemTypes.Color, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.ColorArray, new ConvertToStringMethodHandler(Oct.ToString));
#endif
            SetMappingToString(SystemTypes.DateTime, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.DateTimeArray, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.Decimal, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.DecimalArray, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.Double, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.DoubleArray, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.Int16, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.Int16Array, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.Int32, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.Int32Array, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.Int64, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.Int64Array, new ConvertToStringMethodHandler(Oct.ToString));
#if !SILVERLIGHT
            SetMappingToString(SystemTypes.Point, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.PointArray, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.PointF, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.PointFArray, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.Rectangle, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.RectangleArray, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.RectangleF, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.RectangleFArray, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.Size, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.SizeArray, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.SizeF, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.SizeFArray, new ConvertToStringMethodHandler(Oct.ToString));
#endif
            SetMappingToString(SystemTypes.SByte, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.SByteArray, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.Single, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.SingleArray, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.String, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.StringArray, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.TimeSpan, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.TimeSpanArray, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.UInt16, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.UInt16Array, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.UInt32, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.UInt32Array, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.UInt64, new ConvertToStringMethodHandler(Oct.ToString));
            SetMappingToString(SystemTypes.UInt64Array, new ConvertToStringMethodHandler(Oct.ToString));

            #endregion
        }

        /// <summary>
        /// 转换给定的值
        /// </summary>
        /// <param name="type">值的类型</param>
        /// <param name="value">给定的值，如果为null则返回默认设定项</param>
        /// <returns></returns>
        public static object ConvertToObject(Type type, string value)
        {
            if (Oct.IsNull(value))
            {
                if (type == ST.String) return string.Empty;
                return type.Assembly.CreateInstance(type.FullName);
            }
            if (ToObjectMap.ContainsKey(type))
            {
                Delegate method = (Delegate)ToObjectMap[type];
                return method.DynamicInvoke(new object[] { value });
            }
            else
            {
                if (type.BaseType == SystemTypes.Enum)
                {
                    Enum enumValue = Oct.ToEnum(type, value);
                    return enumValue;
                }
                return null;
            }
        }

        public static string ConvertToString(Type type, object value)
        {
            if (Oct.IsNull(value))
            {
                return null;
            }
            if (ToStringMap.ContainsKey(type))
            {
                Delegate method = (Delegate)ToStringMap[type];
                return (string)method.DynamicInvoke(new object[] { value });
            }
            else
            {
                if (type.BaseType == SystemTypes.Enum)
                {
                    return Oct.ToString(value);
                }
                return null;
            }
        }

        /// <summary>
        /// 为给定类型对象提供字符串到对象的转换方法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method">ConvertMethodHandler委托，这是一个输入string值，返回object对象的方法</param>
        /// <param name="overWrite"></param>
        public static void SetMappingToObject(Type type, Delegate method, bool overWrite)
        {
            if (ToObjectMap.ContainsKey(type))
            {
                if (overWrite)
                    ToObjectMap[type] = method;
            }
            else
            {
                ToObjectMap.Add(type, method);
            }
        }

        /// <summary>
        /// 为给定类型对象提供字符串到对象的转换方法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method">ConvertMethodHandler委托，这是一个输入string值，返回object对象的方法</param>
        public static void SetMappingToObject(Type type, Delegate method)
        {
            SetMappingToObject(type, method, true);
        }

        public static void SetMappingToString(Type type, Delegate method, bool overWrite)
        {
            if (ToStringMap.ContainsKey(type))
            {
                if (overWrite)
                    ToStringMap[type] = method;
            }
            else
            {
                ToStringMap.Add(type, method);
            }
        }

        public static void SetMappingToString(Type type, Delegate method)
        {
            SetMappingToString(type, method, true);
        }

        public static bool ContainsToObjectType(Type type)
        {
            bool result = ToObjectMap.ContainsKey(type);
            if (!result)
            {
                if (type.BaseType == SystemTypes.Enum)
                    result = true;
            }
            return result;
        }

        public static bool ContainsToStringType(Type type)
        {
            bool result = ToStringMap.ContainsKey(type);
            if (!result)
            {
                if (type.BaseType == SystemTypes.Enum)
                    result = true;
            }
            return result;
        }
    }
}
