using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Sails.Utils
{
    /// <summary>
	/// 提供一个应用程序配置访问器
	/// </summary>
	public class AppConfigProvider : ParameterProvider
    {
        XmlNode appSettings;
        string xmlFile;

        public string XmlFile
        {
            get { return xmlFile; }
        }

        protected override void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                xmlFile = null;
                appSettings = null;
                base.Dispose(disposing);
            }
        }

        public AppConfigProvider(string xmlFile)
        {
            Initialize(xmlFile);
        }

        public AppConfigProvider()
        {
            var assembly = Assembly.GetEntryAssembly();
            if (assembly != null)
                Initialize(assembly.Location + ".config");
        }

        private void Initialize(string xmlFile)
        {
            this.xmlFile = xmlFile.ToLower();
            if (xmlFile == null || xmlFile == string.Empty || !File.Exists(xmlFile)) return;
            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(xmlFile);
                appSettings = XmlHelper.GetChild(document, "appSettings");
                Debug.WriteLine("成功加载配置文件“" + xmlFile + "”!");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("在加载配置文件过程中发生错误，所有配置将使用默认值。详细信息:" + ex.ToString());
                return;
            }

            if (appSettings == null)
            {
                Debug.WriteLine("无法加载参数，因为在配置文件中找不到“appSettings”节点");
                return;
            }
        }

        protected override object GetParameterA(string name, Type parameterType, int index = 0)
        {
            object result = null;
            if (CanAccess)
            {
                var nodes = appSettings.SelectNodes("add[@key='" + name + "']");
                if (nodes != null && nodes.Count > 0)
                {
                    if (index < nodes.Count)
                    {
                        var node = nodes[index];
                        XmlAttribute att = node.Attributes["value"];
                        if (att != null)
                        {
                            result = att.Value;
                        }
                        else
                            return string.Empty;
                    }
                }
            }
            //else
            //{

            //}
            //if(result == null && index == 0)
            //{
            //    var array = System.Configuration.ConfigurationManager.AppSettings.GetValues(name);
            //    if(array != null && array.Length > index)
            //        result = array[index];
            //}
            return result;
        }

        public override void SaveToSource()
        {
            if (appSettings != null)
            {
                if (xmlFile == null)
                {
                    var ass = Assembly.GetEntryAssembly();
                    if (ass == null) ass = Assembly.GetCallingAssembly();
                    if (ass != null)
                        xmlFile = ass.Location + ".config";
                }
                var dir = Path.GetDirectoryName(xmlFile);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                appSettings.OwnerDocument.Save(xmlFile);
            }
            //{
            //    XmlDocument document = new XmlDocument();
            //    var root = XmlHelper.CreateRoot("configuration", document);
            //    XmlHelper.CreateNode("appSettings", root, true);

            //}
        }

        /// <summary>
        /// 为给定名称的参数赋值
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        public override bool SetParameter(string name, object value)
        {
            if (!CanAccess)
            {
                XmlDocument document = new XmlDocument();
                document.AppendChild(document.CreateNode(XmlNodeType.XmlDeclaration, null, null));
                XmlNode configuration = document.CreateNode(XmlNodeType.Element, "configuration", null);
                document.AppendChild(configuration);
                appSettings = document.CreateNode(XmlNodeType.Element, "appSettings", null);
                configuration.AppendChild(appSettings);
            }


            XmlNode node = appSettings.SelectSingleNode("add[@key='" + name + "']");
            if (node == null)
            {
                node = appSettings.OwnerDocument.CreateNode(XmlNodeType.Element, "add", null);
                appSettings.AppendChild(node);
                XmlAttribute key = appSettings.OwnerDocument.CreateAttribute("key");
                key.Value = name;
                node.Attributes.Append(key);
            }
            XmlAttribute att = node.Attributes["value"];
            if (att == null)
            {
                att = appSettings.OwnerDocument.CreateAttribute("value");
                node.Attributes.Append(att);
            }
            att.Value = ObjectToString(value);
            return true;
        }

        public static string ObjectToString(object value)
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

        public static bool IsNull(object target)
        {
            return target == null || target == DBNull.Value;
        }


        private class XmlDocumentCache : IDisposable
        {
            // Fields
            public ArrayList documents = new ArrayList();
            public ArrayList keys = new ArrayList();

            // Methods
            public void Add(string key, XmlDocument value)
            {
                this[key] = value;
            }

            public bool ContainsKey(string key)
            {
                return this.keys.Contains(key);
            }

            public void Dispose()
            {
                this.keys.Clear();
                this.keys = null;
                this.documents.Clear();
                this.documents = null;
            }

            internal void Remove(string key)
            {
                int index = this.keys.IndexOf(key);
                if (index > -1)
                {
                    this.documents.RemoveAt(index);
                    this.keys.Remove(key);
                }
            }

            // Properties
            public XmlDocument this[string key]
            {
                get
                {
                    int index = this.keys.IndexOf(key);
                    if (index == -1)
                    {
                        return null;
                    }
                    return (XmlDocument)this.documents[index];
                }
                set
                {
                    int index = this.keys.IndexOf(key);
                    if (index == -1)
                    {
                        this.keys.Add(key);
                        this.documents.Add(value);
                    }
                    else
                    {
                        this.documents[index] = value;
                    }
                }
            }
        }

        #region 属性
        /// <summary>
        /// 获取对应的配置文件是否访问
        /// </summary>
        public bool CanAccess
        {
            get
            {
                return appSettings != null;
            }
        }
        #endregion
    }
}
