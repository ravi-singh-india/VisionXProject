using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using VisionX.BusinessEntities.Repositories;

namespace VisionX.BusinessAccessLayer.Repositories
{
    public class Bal
    {
        #region Initialize

        DBConfigPara DBConfigPara = new DBConfigPara();
        string ConnectionString = "";
        Int32 strIsrequiredToMaintainLog = 0;
        string strSecurityCode = "";
        private byte[] lbtVector = { 240, 3, 45, 29, 0, 76, 173, 59 };
        private string lscryptoKey = "Password";

        #endregion

        #region "Function"

        public string Serialize<T>(T ObjectToSerialize)
        {
            var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(ObjectToSerialize.GetType());
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, ObjectToSerialize, emptyNamespaces);
                return stream.ToString();
            }
        }
        public T Deserialize<T>(string input) where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }
     
        public Int32 ConvertMinutesToMilliseconds(double minutes)
        {
            return Convert.ToInt32(TimeSpan.FromMinutes(minutes).TotalMilliseconds);
        }
        public string ReadConfigPara()
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(AppDomain.CurrentDomain.BaseDirectory + "Config.xml"))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "DBServer":
                                    DBConfigPara.strDBIP = reader.ReadString();
                                    break;
                                case "DBName":
                                    DBConfigPara.strDBName = reader.ReadString();
                                    break;
                                case "DBUname":
                                    DBConfigPara.strUserID = reader.ReadString();
                                    break;
                                case "DBPword":
                                    DBConfigPara.strPassword = Pass_Decrypt(reader.ReadString());
                                    break;
                            }
                        }
                    }
                    ConnectionString = "Data Source = " + DBConfigPara.strDBIP + "; Initial Catalog=" + DBConfigPara.strDBName + "; User ID=" + DBConfigPara.strUserID + "; pwd=" + DBConfigPara.strPassword + ";Encrypt=True;TrustServerCertificate=True;";
                }
                return ConnectionString;
            }
            catch
            {
                throw;
            }
        }
        public string Pass_Decrypt(string sQueryString)
        {
            byte[] buffer;
            TripleDESCryptoServiceProvider loCryptoClass = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider loCryptoProvider = new MD5CryptoServiceProvider();
            try
            {
                buffer = Convert.FromBase64String(sQueryString);
                loCryptoClass.Key = loCryptoProvider.ComputeHash(ASCIIEncoding.ASCII.GetBytes(lscryptoKey));
                loCryptoClass.IV = lbtVector;
                return Encoding.ASCII.GetString(loCryptoClass.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                loCryptoClass.Clear();
                loCryptoProvider.Clear();
                loCryptoClass = null/* TODO Change to default(_) if this is not a reference type */;
                loCryptoProvider = null/* TODO Change to default(_) if this is not a reference type */;
            }
        }
        public string Pass_Encrypt(string sInputVal)
        {
            TripleDESCryptoServiceProvider loCryptoClass = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider loCryptoProvider = new MD5CryptoServiceProvider();
            byte[] lbtBuffer;
            try
            {
                lbtBuffer = System.Text.Encoding.ASCII.GetBytes(sInputVal);
                loCryptoClass.Key = loCryptoProvider.ComputeHash(ASCIIEncoding.ASCII.GetBytes(lscryptoKey));
                loCryptoClass.IV = lbtVector;
                sInputVal = Convert.ToBase64String(loCryptoClass.CreateEncryptor().TransformFinalBlock(lbtBuffer, 0, lbtBuffer.Length));
                return sInputVal;
            }
            catch (CryptographicException ex)
            {
                throw ex;
            }
            catch (FormatException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                loCryptoClass.Clear();
                loCryptoProvider.Clear();
                loCryptoClass = null/* TODO Change to default(_) if this is not a reference type */;
                loCryptoProvider = null/* TODO Change to default(_) if this is not a reference type */;
            }
        }
      
        public bool IsApplicationAlreadyRunning()
        {
            string proc = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(proc);
            if (processes.Length > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int IsrequiredToMaintainLog()
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(AppDomain.CurrentDomain.BaseDirectory + "Config.xml"))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "MaintanLog":
                                    strIsrequiredToMaintainLog = Convert.ToInt32(reader.ReadString());
                                    break;
                            }
                        }
                    }
                }
                return strIsrequiredToMaintainLog;
            }
            catch
            {
                throw;
            }
        }
        public string GetSecurityCode()
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(AppDomain.CurrentDomain.BaseDirectory + "Config.xml"))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "SecurityCode":
                                    strSecurityCode = Pass_Decrypt(reader.ReadString());
                                    break;
                            }
                        }
                    }
                }
                return strSecurityCode;
            }
            catch
            {
                throw;
            }
        }
        public string GetPlazaSecurityCode()
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(AppDomain.CurrentDomain.BaseDirectory + "Config.xml"))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "PlazaSecurityCode":
                                    strSecurityCode = Pass_Decrypt(reader.ReadString());
                                    break;
                            }
                        }
                    }
                }
                return strSecurityCode;
            }
            catch
            {
                throw;
            }
        }
      
        #endregion
    }
}
