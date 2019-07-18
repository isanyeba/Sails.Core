using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Sails.Utils
{
    public static class XmlHelper
    {
        /// <summary>
        /// 读取指定名称的属性值,如果没有该属性则返回Null
        /// </summary>
        /// <param name="attName"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string GetAttribValue(this XmlNode node, string attName)
        {
            string result = null;
            if (node != null && node.Attributes != null)
            {
                XmlAttribute attribute = node.Attributes[attName];
                if (attribute != null)
                {
                    result = attribute.Value;
                }
            }
            return result;
        }
        /// <summary>
        /// 在指定XML节点上新增一个节点
        /// </summary>
        /// <param name="name"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static XmlNode CreateChild(this XmlNode node, string name, bool createNew = true)
        {
            XmlNode child;
            if (!createNew)
            {
                child = GetChild(node, name);
                if (child != null) return child;
                //for (int i = 0; i < node.ChildNodes.Count; i++)
                //{
                //    XmlNode child = node.ChildNodes.Item(i);
                //    if (child.Name == name)
                //        return child;
                //}
            }
            child = node.OwnerDocument.CreateNode(XmlNodeType.Element, name, "");
            node.AppendChild(child);
            return child;
        }

        public static XmlNode GetChileNode(this XmlNode node, string name, string attributeName, string attributeValue)
        {
            if (node == null || node.ChildNodes.Count == 0) return null;
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                XmlNode child = node.ChildNodes.Item(i);
                if (child.Name == name)
                {
                    if (attributeName != null && attributeName != string.Empty)
                    {
                        XmlAttribute attribute = child.Attributes[attributeName];
                        if (attribute != null)
                        {
                            if (attribute.Value == attributeValue)
                                return child;
                        }
                    }
                    else
                        return child;
                }
            }
            return null;
        }

        public static XmlNode GetChileNode(this XmlNode node, string name)
        {
            return GetChileNode(node, name, null, null);
        }
        /// <summary>
        /// 创建XML根节点，如果已存在同名根节点直接返回，否则会抛出异常
        /// </summary>
        /// <returns></returns>
        public static XmlNode CreateRoot(this XmlDocument document, string name)
        {
            XmlNode node = document.SelectSingleNode(name);
            if (node == null)
            {
                node = document.CreateNode(XmlNodeType.Element, name, null);
                document.AppendChild(node);
            }

            return node;
        }
        /// <summary>
        /// 在指定的XML节点上新增一个属性
        /// </summary>
        /// <returns></returns>
        public static XmlAttribute CreateAttribute(this XmlNode node, string name, object value, string namespaceURI = null)
        {
            XmlAttribute attribute = node.Attributes[name];
            if (attribute == null)
            {
                var index = name.IndexOf(":");
                if (index != -1)
                {
                    var prefix = name.Substring(0, index).ToLower();
                    if (prefix == "xmlns")
                    {
                        namespaceURI = string.Format("http://www.w3.org/2000/{0}/", prefix);
                        attribute = node.OwnerDocument.CreateAttribute(prefix, name.Substring(index + 1), namespaceURI);
                    }
                    else
                    {
                        attribute = node.OwnerDocument.CreateAttribute(prefix, name.Substring(index + 1), namespaceURI);
                    }
                }
                else
                    attribute = node.OwnerDocument.CreateAttribute(name);
                node.Attributes.Append(attribute);
            }
            attribute.Value = value != null ? value.ToString() : "";
            return attribute;
        }

        public static XmlNode GetChild(this XmlNode node, string name)
        {
            return GetChild(node, name, null, null);
        }

        public static XmlNode GetChild(this XmlNode node, string nodeName, string attributeName, string attributeValue)
        {
            if (node == null) return null;
            XmlNode result = null;
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                XmlNode child = node.ChildNodes.Item(i);

                if (child.Name == nodeName)
                {
                    if (attributeName != null && attributeName != string.Empty)
                    {
                        XmlAttribute attribute = child.Attributes[attributeName];
                        if (attribute != null)
                        {
                            if (attribute.Value == attributeValue)
                            {
                                result = child;
                                break;
                            }
                        }
                    }
                    else
                    {
                        result = child;
                        break;
                    }
                }
                else
                {
                    result = GetChild(child, nodeName, attributeName, attributeValue);
                    if (result != null)
                        break;
                }
            }
            return result;
        }

        public static XmlNode FindNode(this XmlNode root, string name)
        {
            foreach (XmlNode node in root)
            {
                if (node.Name == name)
                    return node;
            }
            return null;
        }
    }
}
