using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sails.Utils
{
    public class SystemTypes
    {
        static List<Type> allTypes;
        static SystemTypes()
        {
            allTypes = new List<Type>();
            FieldInfo[] fis = typeof(SystemTypes).GetFields();
            for (int i = 0; i < fis.Length; i++)
            {
                if (fis[i].FieldType != typeof(Type)) continue;
                allTypes.Add((Type)fis[i].GetValue(null));
            }
        }

        public static string GetTypeString(Type type, bool fullName)
        {
            if (type == null || type.FullName == "System.Void") return "void";
            if (fullName)
                return GetTypeName(type);
            else
            {
                if (!allTypes.Contains(type))
                {
                    return GetTypeName(type);
                }

                if (!type.IsArray)
                {
                    SystemTypesEnum en = (SystemTypesEnum)System.Enum.Parse(typeof(SystemTypesEnum), type.Name, true);
                    switch (en)
                    {
                        case SystemTypesEnum.TimeSpanArray:
                        return "TimeSpan[]";
                        case SystemTypesEnum.UInt16Array:
                        return "ushort[]";
                        case SystemTypesEnum.UInt32Array:
                        return "uint[]";
                        case SystemTypesEnum.UInt64Array:
                        return "ulong[]";
                        case SystemTypesEnum.Int32:
                        return "int";
                        case SystemTypesEnum.Int64:
                        return "long";
                        case SystemTypesEnum.SByte:
                        return "sbyte";
                        case SystemTypesEnum.Byte:
                        return "byte";
                        case SystemTypesEnum.Boolean:
                        return "bool";
                        case SystemTypesEnum.Char:
                        return "char";
                        case SystemTypesEnum.UInt16:
                        return "ushort";
                        case SystemTypesEnum.UInt64:
                        return "ulong";
                        case SystemTypesEnum.UInt32:
                        return "uint";
                        case SystemTypesEnum.Decimal:
                        return "decimal";
                        case SystemTypesEnum.Double:
                        return "double";
                        case SystemTypesEnum.Single:
                        return "float";
                        case SystemTypesEnum.String:
                        return "string";
                        case SystemTypesEnum.Int16:
                        return "short";
                        case SystemTypesEnum.Object:
                        return "object";
                        case SystemTypesEnum.Void:
                        return "void";
                    }
                }
                return type.FullName.Replace('+', '.');
            }
        }

        public static string GetTS(Delegate function)
        {
            var method = function.Method;
            string parmListStr = string.Empty;
            string contract = string.Empty;
            var parms = method.GetParameters();
            if (parms.Length != 0)
            {
                foreach (var parm in parms)
                {
                    if (parmListStr != string.Empty)
                        parmListStr += "&";
                    if (contract != string.Empty)
                        contract += ", ";
                    contract += ST.GetTS(parm.ParameterType);
                    parmListStr += parm.Name + "=";
                }
            }
            contract = $"{method.DeclaringType.FullName}.{method.Name}({contract})";
            return contract;
        }

        public static string GetTypeString<T>()
        {
            return GetTypeString(typeof(T));
        }

        public static string GetTypeString(Type type)
        {
            return GetTypeString(type, false);
        }

        public static string GetTS<T>()
        {
            return GetTypeString(typeof(T));
        }

        public static string GetTS(Type type)
        {
            return GetTypeString(type);
        }

        private static string GetTypeName(Type type)
        {
            if (!type.IsGenericType)
                return type.FullName.Replace('+', '.');
            else
            {
                Type genericType = type.GetGenericTypeDefinition();
                string fullName = genericType.FullName;
                var typeNameArray = fullName.Split('+');
                string ret = string.Empty;
                Type[] generics = type.GetGenericArguments();
                int arrayIndex = 0;
                for (int j = 0; j < typeNameArray.Length; j++)
                {
                    if (j != 0) ret += ".";
                    var item = typeNameArray[j];
                    int index = item.IndexOf('`');
                    if (index != -1)
                    {
                        int count = Convert.ToInt32(item.Substring(index + 1));
                        string genStr = "<";
                        for (int i = 0; i < count; i++)
                        {
                            if (i != 0) genStr += ",";
                            genStr += GetTypeString(generics[arrayIndex++]);
                        }
                        genStr += ">";
                        ret += item.Substring(0, index) + genStr;
                    }
                    else
                        ret += item;
                }
                return ret;
            }
        }

        public static SystemTypesEnum GetTypeEnum(Type type)
        {
            string enumName = type.Name.Replace("[]", "Array");
            SystemTypesEnum ret;
            if (System.Enum.TryParse<SystemTypesEnum>(enumName, true, out ret))
                return ret;
            if (type.IsEnum) return SystemTypesEnum.Enum;
            return SystemTypesEnum.Unknown;
        }

        /// <summary>
        /// 返回指定类型的数据长度，如果返回-1 则表示该数据类型没有一个固定长度
        /// </summary>
        /// <param name="type"></param>
        /// <param name="errorThrow">true=无法测算大小时抛出异常</param>
        /// <returns></returns>
        public static int GetSize(Type type, bool errorThrow)
        {
            var typeEnum = ST.GetTypeEnum(type);
            var ret = GetSize(typeEnum);
            if (ret == -1 && errorThrow) throw new ArgumentException("无法测算大小!");
            return ret;
        }

        public static int GetSize(Type type)
        {
            return GetSize(type, true);
        }

        /// <summary>
        /// 返回指定类型的数据长度，如果返回-1 则表示该数据类型没有一个固定长度
        /// </summary>
        /// <param name="typeEnum"></param>
        /// <returns></returns>
        public static int GetSize(SystemTypesEnum typeEnum)
        {
            var size = -1;
            switch (typeEnum)
            {
                case SystemTypesEnum.Int32:
                case SystemTypesEnum.Single:
                case SystemTypesEnum.UInt32:
                size = 4;
                break;
                case SystemTypesEnum.TimeSpan:
                case SystemTypesEnum.Double:
                case SystemTypesEnum.Int64:
                case SystemTypesEnum.DateTime:
                case SystemTypesEnum.UInt64:
                size = 8;
                break;
                case SystemTypesEnum.Byte:
                case SystemTypesEnum.SByte:
                case SystemTypesEnum.Boolean:
                size = 1;
                break;
                case SystemTypesEnum.UInt16:
                case SystemTypesEnum.Char:
                case SystemTypesEnum.Int16:
                size = 2;
                break;
                case SystemTypesEnum.Decimal:
                size = 16;
                break;
            }
            return size;
        }

        #region 属性
        public static Type Object = typeof(Object);

        public static Type ObjectArray = typeof(Object[]);

        public static Type Int64Array = typeof(Int64[]);

        public static Type TimeSpanArray = typeof(TimeSpan[]);

#if !SILVERLIGHT
        public static Type Color = typeof(Color);
        public static Type PointF = typeof(PointF);

        public static Type Point = typeof(Point);

        public static Type RectangleF = typeof(RectangleF);

        public static Type Rectangle = typeof(Rectangle);

        public static Type SizeF = typeof(SizeF);

        public static Type Size = typeof(Size);

        public static Type ColorArray = typeof(Color[]);

        public static Type PointFArray = typeof(PointF[]);

        public static Type PointArray = typeof(Point[]);

        public static Type RectangleFArray = typeof(RectangleF[]);

        public static Type RectangleArray = typeof(Rectangle[]);

        public static Type SizeFArray = typeof(SizeF[]);

        public static Type Image = typeof(Image);

        public static Type SizeArray = typeof(Size[]);
#endif

        public static Type DateTimeArray = typeof(DateTime[]);

        public static Type SByteArray = typeof(sbyte[]);

        public static Type Int32 = typeof(Int32);

        public static Type Int32Array = typeof(Int32[]);

        public static Type ByteArray = typeof(byte[]);

        public static Type BooleanArray = typeof(bool[]);

        public static Type CharArray = typeof(char[]);

        public static Type UInt16Array = typeof(UInt16[]);

        public static Type UInt64Array = typeof(UInt64[]);

        public static Type UInt32Array = typeof(UInt32[]);

        public static Type DecimalArray = typeof(Decimal[]);

        public static Type DoubleArray = typeof(double[]);

        public static Type SingleArray = typeof(Single[]);

        public static Type StringArray = typeof(string[]);

        public static Type Int16Array = typeof(Int16[]);

        public static Type Int64 = typeof(Int64);

        public static Type TimeSpan = typeof(TimeSpan);

        public static Type DateTime = typeof(DateTime);

        public static Type SByte = typeof(sbyte);

        public static Type Byte = typeof(byte);

        public static Type Boolean = typeof(bool);

        public static Type Char = typeof(char);

        public static Type UInt16 = typeof(UInt16);

        public static Type UInt64 = typeof(UInt64);

        public static Type UInt32 = typeof(UInt32);

        public static Type Decimal = typeof(decimal);

        public static Type Double = typeof(double);

        public static Type Single = typeof(Single);

        public static Type String = typeof(string);

        public static Type Int16 = typeof(Int16);

        public static Type Enum = typeof(Enum);

        public static Type Void = Type.GetType("System.Void");

        //==
        #endregion
    }

    /// <summary>
    /// SystemTypes的缩写
    /// </summary>
    public class ST : SystemTypes { }

    public enum SystemTypesEnum
    {
        Int64Array,

        TimeSpanArray,

        DateTimeArray,

        SByteArray,

        Int32,

        Int32Array,

        ByteArray,

        BooleanArray,

        CharArray,

        UInt16Array,

        UInt64Array,

        UInt32Array,

        DecimalArray,

        DoubleArray,

        SingleArray,

        StringArray,

        Int16Array,

        Int64,

        TimeSpan,

        DateTime,

        SByte,

        Byte,

        Boolean,

        Char,

        UInt16,

        UInt64,

        UInt32,

        Decimal,

        Double,

        Single,

        String,

        Int16,

        Void,

        Enum,

#if !SILVERLIGHT
        PointFArray,

        PointArray,

        RectangleFArray,

        RectangleArray,

        SizeFArray,

        SizeArray,

        ColorArray,

        PointF,

        Point,

        RectangleF,

        Rectangle,

        SizeF,

        Size,

        Color,

        Image,
#endif

        Object,

        ObjectArray,

        Unknown = int.MaxValue,
    }
}
