using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace SkillBank.Site.Services.Utility
{
    public class XMLHelper
    {

        #region Deserialize

        /// <summary> 
        /// 反序列化 
        /// </summary> 
        /// <param name="xml">XML字符串</param> 
        /// <returns></returns> 
        public static T Deserialize<T>(string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(typeof(T));
                    T t = (T)xmldes.Deserialize(sr);
                    return t;
                }
            }
            catch (Exception e)
            {
                var d = e.Message;
                return default(T);
            }
        }

        /// <summary> 
        /// 反序列化 
        /// </summary> 
        public static T Deserialize<T>(Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(typeof(T));
            T t = (T)xmldes.Deserialize(stream);
            return t;
        }

        #endregion
        

        #region

        /// <summary>
        /// 序列化XML文件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <returns></returns>

        public static string Serializer<T>(object obj)
        {

            MemoryStream Stream = new MemoryStream();
            //创建序列化对象 
            XmlSerializer xml = new XmlSerializer(typeof(T));


            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Encoding = new UTF8Encoding(false);
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.OmitXmlDeclaration = true;
            
            try
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(Stream, xmlWriterSettings))
                {
                    //序列化对象
                    xml.Serialize(xmlWriter, obj, ns);
                }
            }
            catch (InvalidOperationException)
            {
                throw;
            }

            return Encoding.UTF8.GetString(Stream.ToArray()).Trim();
            //xml.Serialize(Stream, obj, ns);  

            //序列化对象 
            //xml.Serialize(Stream, obj);

            //Stream.Position = 0;

            //StreamReader sr = new StreamReader(Stream);

            //string str = sr.ReadToEnd();

            //return str;
        }

        #endregion

    }
}
