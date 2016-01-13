using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace SkillBank.Site.Services.Tencent
{
    public static class WeChatMessageHelper
    {

        public static WeChatMessage GetWXMessage(Stream stream)
        {
            WeChatMessage wx = new WeChatMessage();
            StreamReader str = new StreamReader(stream, System.Text.Encoding.UTF8);
            XmlDocument xml = new XmlDocument();
            xml.Load(str);
            wx.ToUserName = xml.SelectSingleNode("xml").SelectSingleNode("ToUserName").InnerText;
            wx.FromUserName = xml.SelectSingleNode("xml").SelectSingleNode("FromUserName").InnerText;
            wx.MsgType = xml.SelectSingleNode("xml").SelectSingleNode("MsgType").InnerText;
            if (wx.MsgType.Trim() == "text")
            {
                wx.Content = xml.SelectSingleNode("xml").SelectSingleNode("Content").InnerText;
            }
            if (wx.MsgType.Trim() == "event")
            {
                wx.EventName = xml.SelectSingleNode("xml").SelectSingleNode("Event").InnerText;
            }
            
            return wx;
        }
    }
}