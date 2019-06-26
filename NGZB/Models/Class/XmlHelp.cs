using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace NGZB.Models.Class
{
    public class XmlHelp
    {
        private XElement xmlDoc = null;
        private string xmlFile = null;
        public bool FileExists = false;

        /// <summary>
        /// xlm文件操作
        /// </summary>
        /// <param name="xmlFileName">文件路径格式如：@"Content\init.xml"</param>
        public XmlHelp(string xmlFileName)
        {
            xmlFile = AppDomain.CurrentDomain.BaseDirectory + xmlFileName;
            FileInfo fileio = new FileInfo(xmlFile);
            if (fileio.Exists)
            {
                xmlDoc = XElement.Load(xmlFile);
                FileExists = true;
            }
        }

        /// <summary>
        /// 删除xml节点
        /// </summary>
        /// <param name="node">要删除的节点</param>
        /// <param name="whereItem">条件子节点</param>
        /// <param name="whereValue">条件值</param>
        /// <returns></returns>
        public bool Del(string node, string whereItem, string whereValue)
        {
            if (xmlDoc != null)
            {
                try
                {
                    var deleteinfo = from items in xmlDoc.Descendants(node) where items.Element(whereItem).Value == whereValue select items;
                    deleteinfo.Remove();
                    xmlDoc.Save(xmlFile);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public int Count(string node, string whereItem = null, string whereValue = null)
        {
            if (xmlDoc != null)
            {
                try
                {
                    IEnumerable<XElement> count = null;
                    if (whereItem != null && whereValue != null)
                    {
                        count = from items in xmlDoc.Descendants(node) where items.Element(whereItem).Value == whereValue select items;
                    }
                    else
                    {
                        count = from items in xmlDoc.Descendants(node) select items;
                    }
                    if (count != null)
                    {
                        return count.Count();
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 编辑xml节点
        /// </summary>
        /// <param name="node">节点名称</param>
        /// <param name="whereItem">子节点条件</param>
        /// <param name="whereValue">条件值</param>
        /// <param name="editItem">要编辑的子节点</param>
        /// <param name="newValue">新值</param>
        /// <returns></returns>
        public bool Edit(string node, string whereItem, string whereValue, string editItem, string newValue)
        {
            if (xmlDoc != null)
            {
                try
                {
                    var editinfo = from items in xmlDoc.Descendants(node) where items.Element(whereItem).Value == whereValue select items;
                    foreach (var i in editinfo)
                    {
                        i.Element(editItem).Value = newValue;
                    }
                    xmlDoc.Save(xmlFile);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 查询xml，返回Json
        /// </summary>
        /// <param name="node">要查询的节点</param>
        /// <param name="whereItem">条件子节点元素</param>
        /// <param name="whereValue">查询条件</param>
        /// <param name="orderByItem">排序子节点元素</param>
        /// <param name="orderByDescAsc">排序方向</param>
        /// <returns></returns>
        public string Select(string node, string whereItem = null, string whereValue = null, string orderByItem = null, string orderByDescAsc = "DESC")
        {
            var xdoc = XElement.Load(xmlFile);
            IEnumerable<XElement> targetNodes = null;
            if (whereItem == null || whereValue == null)
            {
                if (orderByItem != null)
                {
                    if (orderByDescAsc.ToUpper() == "DESC")
                    {
                        targetNodes = from target in xdoc.Descendants(node) orderby target.Element(orderByItem).Value descending select target;
                    }
                    else
                    {
                        targetNodes = from target in xdoc.Descendants(node) orderby target.Element(orderByItem).Value ascending select target;
                    }
                }
                else
                {
                    targetNodes = from target in xdoc.Descendants(node) select target;
                }
            }
            else
            {
                if (orderByItem != null)
                {
                    if (orderByDescAsc.ToUpper() == "DESC")
                    {
                        targetNodes = from target in xdoc.Descendants(node) where target.Element(whereItem).Value.Equals(whereValue) orderby target.Element(orderByItem).Value descending select target;
                    }
                    else
                    {
                        targetNodes = from target in xdoc.Descendants(node) where target.Element(whereItem).Value.Equals(whereValue) orderby target.Element(orderByItem).Value ascending select target;
                    }
                }
                else
                {
                    targetNodes = from target in xdoc.Descendants(node) where target.Element(whereItem).Value.Equals(whereValue) select target;
                }
            }
            if (targetNodes != null)
            {
                return JsonConvert.SerializeObject(targetNodes);
            }
            return null;
        }

        /// <summary>
        /// 删除指定的xml文件，执行该方法后，该对象的其它所有方法失效
        /// </summary>
        /// <returns></returns>
        public bool DelXmlFile()
        {
            FileInfo fileio = new FileInfo(xmlFile);
            if (fileio.Exists)
            {
                try
                {
                    fileio.Delete();
                    xmlDoc = null;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                xmlDoc = null;
                return true;
            }
        }
    }
}