using System.Collections.Generic;
using System.IO;
using System.Text;
using CodeScales.Http.Entity.Mime;

namespace SkillBank.Site.Services.Net.Mail
{
    public class UTF8FileBody : Body
    {
            private string m_name;
            private string m_fileName;
            private byte[] m_content;
            private string m_mimeType;

            public UTF8FileBody(string name, string fileName, FileInfo fileInfo, string mimeType)
                : this(name, fileName, fileInfo)
            {
                this.m_mimeType = mimeType;
            }

            public UTF8FileBody(string name, string fileName, FileInfo fileInfo)
            {
                this.m_name = name;
                this.m_fileName = fileName;
                this.m_content = null;

                if (fileInfo == null)
                {
                    this.m_content = new byte[0];
                }
                else
                {
                    using (BinaryReader reader = new BinaryReader(fileInfo.OpenRead()))
                    {
                        this.m_content = reader.ReadBytes((int)reader.BaseStream.Length);
                    }
                }
            }

            public byte[] GetContent(string boundry)
            {
                var bytes = new List<byte>();

                string paramBoundry = "--" + boundry + "\r\n";
                string stringParam = "Content-Disposition: form-data; name=\"" + m_name + "\"; filename=\"" + m_fileName + "\"\r\n";
                string paramEnd = null;
                if (m_mimeType != null)
                    paramEnd = "Content-Type: " + m_mimeType + "\r\n\r\n";
                else
                    paramEnd = "Content-Type: application/octet-stream\r\n\r\n";

                string foo = paramBoundry + stringParam + paramEnd;
                bytes.AddRange(Encoding.UTF8.GetBytes(paramBoundry + stringParam + paramEnd));
                bytes.AddRange(m_content);
                bytes.AddRange(Encoding.UTF8.GetBytes("\r\n"));
                return bytes.ToArray();
            }
    }
}