using System.Net.Mail;
using System.Text;
using SmtpClient = System.Net.Mail.SmtpClient;
using LabourCommissioner.Abstraction.ViewDataModels;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Data;
using System.Reflection;
using System.Security.Cryptography;
using Microsoft.Reporting.NETCore;
using System.Text.RegularExpressions;
using System.Web;
using LabourCommissioner.Abstraction;
using Warning = Microsoft.Reporting.NETCore.Warning;
using Microsoft.Extensions.DependencyInjection;
using ClosedXML.Excel;
using System.Net.Http.Json;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using Serilog;
using System.Collections.Specialized;

namespace LabourCommissioner.Common.Utility
{
    public class CommonUtils
    {
        private readonly IConfiguration appConfig;
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _couchDbUrl;
        private readonly string _couchDbName;
        private readonly string _couchDbUser;

        private readonly string _sMTPConfigSenderAddress;
        private readonly string _sMTPConfigSenderDisplayName;
        private readonly string _sMTPConfigUserName;
        private readonly string _sMTPConfigPassword;
        private readonly string _sMTPConfigHost;
        private readonly int _sMTPConfigPort;
        private readonly bool _sMTPConfigEnableSSL;
        private readonly string _sMTPConfigUseDefaultCredentials;
        private readonly string _sMTPConfigIsBodyHTML;
        private static string _otherKeyValueEncKey;
        private readonly string _isSMSRequired;
        private readonly string _messageUrl;
        private readonly string _messageUserName;
        private readonly string _messagePassword;
        private readonly string _messageSenderId;
        private readonly string _secureKey;
        private readonly string _emailforexception;
        private readonly string _bocwRegistrationAPI;
        private readonly string _bocwAuthHeader;

        private readonly string _sftpDestinationUploadPath;
        private readonly string _sftpDestinationReadPath;
        private readonly string _sftpHost;
        private readonly int _sftpPort;
        private readonly string _sftpUserName;
        private readonly string _sftpPassword;
        private readonly string _BOCWResponsePath;
        private readonly string _BOCWCorporateId;

        private readonly string _GLWBResponsePath;
        private readonly string _GLWBCorporateId;


        //IConfiguration config;
        public CommonUtils(IConfiguration config)
        {
            appConfig = config ?? throw new ArgumentNullException(nameof(config));
            _couchDbUrl = appConfig["CouchDB:URL"];
            _couchDbName = appConfig["CouchDB:DbName"];
            _couchDbUser = appConfig["CouchDB:User"];

            _sMTPConfigSenderAddress = appConfig["SMTPConfig:_SenderAddress"];
            _sMTPConfigSenderDisplayName = appConfig["SMTPConfig:_SenderDisplayName"];
            _sMTPConfigUserName = appConfig["SMTPConfig:_UserName"];
            _sMTPConfigPassword = appConfig["SMTPConfig:_Password"];
            _sMTPConfigHost = appConfig["SMTPConfig:_Host"];
            _sMTPConfigPort = Convert.ToInt32(appConfig["SMTPConfig:_Port"]);
            _sMTPConfigEnableSSL = Convert.ToBoolean(appConfig["SMTPConfig:_EnableSSL"]);
            _sMTPConfigUseDefaultCredentials = appConfig["SMTPConfig:_UseDefaultCredentials"];
            _sMTPConfigIsBodyHTML = appConfig["SMTPConfig:_IsBodyHTML"];
            _otherKeyValueEncKey = appConfig["OtherKeyValue:EncKey"];
            _isSMSRequired = appConfig["SMSConfig:IsSMSRequired"];
            _messageUrl = appConfig["SMSConfig:MessageUrl"];
            _messageUserName = appConfig["SMSConfig:MessageUserName"];
            _messagePassword = appConfig["SMSConfig:MessagePassword"];
            _messageSenderId = appConfig["SMSConfig:MessageSenderId"];
            _secureKey = appConfig["SMSConfig:SecureKey"];

            _emailforexception = appConfig["SMTPConfig:_EmailForException"];
            _bocwRegistrationAPI = appConfig["RegistrationAPI:BOCW"];
            _bocwAuthHeader = appConfig["RegistrationAPI:BOCWAuthHeader"];

            _sftpDestinationUploadPath = appConfig["SBISFTP:DestinationUploadPath"];
            _sftpDestinationReadPath = appConfig["SBISFTP:DestinationReadPath"];
            _sftpHost = appConfig["SBISFTP:Host"];
            _sftpPort = Convert.ToInt32(appConfig["SBISFTP:Port"]);
            _sftpUserName = appConfig["SBISFTP:UserName"];
            _sftpPassword = appConfig["SBISFTP:Password"];
            _BOCWResponsePath = appConfig["SBISFTP:BOCWResponsePath"];
            _BOCWCorporateId = appConfig["SBISFTP:BOCWCorporateId"];
            _GLWBResponsePath = appConfig["SBISFTP:GLWBResponsePath"];
            _GLWBCorporateId = appConfig["SBISFTP:GLWBCorporateId"];

        }
        public CommonUtils(IConfiguration config, IHttpClientFactory clientFactory)
        {
            appConfig = config ?? throw new ArgumentNullException(nameof(config));
            _clientFactory = clientFactory;
            _couchDbUrl = appConfig["CouchDB:URL"];
            _couchDbName = appConfig["CouchDB:DbName"];
            _couchDbUser = appConfig["CouchDB:User"];

        }

        #region Sent Email
        public bool SendPasswordMail(string DisplayName, string emailAddress, string OTP_Code, string UserName, string Password, int UserType, string ImagePath, string URL, string beneficiarytype)
        {
            string strMailBody = "";

            //string path = UserType == 1
            //        ? Path.Combine(ImagePath + "\\MailTemplate\\MailVerification.html")
            //        : Path.Combine(ImagePath + "\\MailTemplate\\PasswordRecovery.html");

            string path = string.Empty;

            if (UserType == 1)
            {
                if (OperatingSystem.IsWindows())
                    path = Path.Combine(ImagePath + "\\MailTemplate\\MailVerification.html");
                else if (OperatingSystem.IsLinux())
                    path = Path.Combine(ImagePath + "//MailTemplate//MailVerification.html");
                else
                {
                    path = "";
                }

            }
            else
            {
                if (OperatingSystem.IsWindows())
                    path = Path.Combine(ImagePath + "\\MailTemplate\\PasswordRecovery.html");
                else if (OperatingSystem.IsLinux())
                    path = Path.Combine(ImagePath + "//MailTemplate//PasswordRecovery.html");
                else
                {
                    path = "";
                }
            }


            StreamReader IDDSbody = new StreamReader(path);
            string beneficiaryname = "";
            if (beneficiarytype == "1")
            {
                beneficiaryname = "ગુજરાત મકાન અને અન્‍ય બાંધકામ શ્રમયોગી કલ્‍યાણ બોર્ડ";
            }
            else
            {
                beneficiaryname = "ગુજરાત શ્રમયોગી કલ્યાણ બોર્ડ";
            }




            strMailBody = IDDSbody.ReadToEnd();
            strMailBody = strMailBody.Replace("{Name}", DisplayName);
            strMailBody = strMailBody.Replace("{OTPCode}", OTP_Code);
            strMailBody = strMailBody.Replace("{BeneficiaryType}", beneficiaryname);
            strMailBody = strMailBody.Replace("{URL}", URL);
            strMailBody = strMailBody.Replace("{username}", UserName);
            strMailBody = strMailBody.Replace("{password}", Password);
            strMailBody = strMailBody.Replace("{ImagePath}", ImagePath);

            bool Chk = SendMail(_sMTPConfigSenderAddress, _sMTPConfigSenderDisplayName, emailAddress, _sMTPConfigSenderAddress, strMailBody, "Password Recovery Mail", "", ""
                , true, "", _sMTPConfigUserName, _sMTPConfigPassword, _sMTPConfigHost, _sMTPConfigPort, _sMTPConfigEnableSSL);

            return Chk;
        }

        public bool SendCitizenRegisteredMail(string DisplayName, string emailAddress, string UserId, string Password, string emailLogo, string RootPath, string TemplateName, int? beneficiarytype)
        {
            string strMailBody = "";

            //string path = Path.Combine(RootPath + "\\MailTemplate\\" + TemplateName);

            string path = string.Empty;
            if (OperatingSystem.IsWindows())
                path = Path.Combine(RootPath + "\\MailTemplate\\" + TemplateName);
            else if (OperatingSystem.IsLinux())
                path = Path.Combine(RootPath + "//MailTemplate//" + TemplateName);
            else
            {
                path = "";
            }


            StreamReader IDDSbody = new StreamReader(path);
            string beneficiaryname = "";
            if (beneficiarytype == 1)
            {
                beneficiaryname = "ગુજરાત મકાન અને અન્‍ય બાંધકામ શ્રમયોગી કલ્‍યાણ બોર્ડ";
            }
            else
            {
                beneficiaryname = "ગુજરાત શ્રમયોગી કલ્યાણ બોર્ડ";
            }


            strMailBody = IDDSbody.ReadToEnd();
            strMailBody = strMailBody.Replace("{LogoImage}", emailLogo);
            strMailBody = strMailBody.Replace("{BeneficiaryType}", beneficiaryname);
            strMailBody = strMailBody.Replace("{Name}", DisplayName);
            strMailBody = strMailBody.Replace("{UserID}", UserId);
            strMailBody = strMailBody.Replace("{Password}", Password);

            bool Chk = SendMail(_sMTPConfigSenderAddress, _sMTPConfigSenderDisplayName, emailAddress, _sMTPConfigSenderAddress, strMailBody, "Citizen Registered", "", ""
                , true, "", _sMTPConfigUserName, _sMTPConfigPassword, _sMTPConfigHost, _sMTPConfigPort, _sMTPConfigEnableSSL);

            return Chk;
        }
        public bool SendCCRegisteredMail(string DisplayName, string emailAddress, string UserId, string Password, string emailLogo, string RootPath, string TemplateName, int? beneficiarytype)
        {
            string strMailBody = "";

            //string path = Path.Combine(RootPath + "\\MailTemplate\\" + TemplateName);

            string path = string.Empty;
            if (OperatingSystem.IsWindows())
                path = Path.Combine(RootPath + "\\MailTemplate\\" + TemplateName);
            else if (OperatingSystem.IsLinux())
                path = Path.Combine(RootPath + "//MailTemplate//" + TemplateName);
            else
            {
                path = "";
            }


            StreamReader IDDSbody = new StreamReader(path);
            string beneficiaryname = "";
            if (beneficiarytype == 1)
            {
                beneficiaryname = "ગુજરાત મકાન અને અન્‍ય બાંધકામ શ્રમયોગી કલ્‍યાણ બોર્ડ";
            }
            else
            {
                beneficiaryname = "ગુજરાત શ્રમયોગી કલ્યાણ બોર્ડ";
            }


            strMailBody = IDDSbody.ReadToEnd();
            strMailBody = strMailBody.Replace("{LogoImage}", emailLogo);
            strMailBody = strMailBody.Replace("{BeneficiaryType}", beneficiaryname);
            strMailBody = strMailBody.Replace("{Name}", DisplayName);
            strMailBody = strMailBody.Replace("{UserID}", UserId);
            strMailBody = strMailBody.Replace("{Password}", Password);

            bool Chk = SendMail(_sMTPConfigSenderAddress, _sMTPConfigSenderDisplayName, emailAddress, _sMTPConfigSenderAddress, strMailBody, "CESS Collector Registration Details", "", ""
                , true, "", _sMTPConfigUserName, _sMTPConfigPassword, _sMTPConfigHost, _sMTPConfigPort, _sMTPConfigEnableSSL);

            return Chk;
        }
        public bool SendApplicationRegisteredMail(string DisplayName, string emailAddress, string Message, string RootPath, string TemplateName, string beneficiarytype)
        {
            string strMailBody = "";

            //string path = Path.Combine(RootPath + "\\MailTemplate\\" + TemplateName);

            string path = string.Empty;
            if (OperatingSystem.IsWindows())
                path = Path.Combine(RootPath + "\\MailTemplate\\" + TemplateName);
            else if (OperatingSystem.IsLinux())
                path = Path.Combine(RootPath + "//MailTemplate//" + TemplateName);
            else
            {
                path = "";
            }
            string beneficiaryname = "";

            StreamReader IDDSbody = new StreamReader(path);

            if (beneficiarytype == "1")
            {
                beneficiaryname = "ગુજરાત મકાન અને અન્‍ય બાંધકામ શ્રમયોગી કલ્‍યાણ બોર્ડ";
            }
            else
            {
                beneficiaryname = "ગુજરાત શ્રમયોગી કલ્યાણ બોર્ડ";
            }




            strMailBody = IDDSbody.ReadToEnd();
            strMailBody = strMailBody.Replace("{Name}", DisplayName);
            strMailBody = strMailBody.Replace("{BeneficiaryType}", beneficiaryname);
            strMailBody = strMailBody.Replace("{Message}", Message);

            bool Chk = SendMail(_sMTPConfigSenderAddress, _sMTPConfigSenderDisplayName, emailAddress, _sMTPConfigSenderAddress, strMailBody, "Application Registered", "", ""
                , true, "", _sMTPConfigUserName, _sMTPConfigPassword, _sMTPConfigHost, _sMTPConfigPort, _sMTPConfigEnableSSL);

            return Chk;
        }

        public bool SendARSApplicationMail(string DisplayName, string emailAddress, string Message, string RootPath, string TemplateName, string emailLogo)
        {
            string strMailBody = "";

            //string path = Path.Combine(RootPath + "\\MailTemplate\\" + TemplateName);
            string path = string.Empty;
            if (OperatingSystem.IsWindows())
                path = Path.Combine(RootPath + "\\MailTemplate\\" + TemplateName);
            else if (OperatingSystem.IsLinux())
                path = Path.Combine(RootPath + "//MailTemplate//" + TemplateName);
            else
            {
                path = "";
            }

            StreamReader IDDSbody = new StreamReader(path);



            strMailBody = IDDSbody.ReadToEnd();
            strMailBody = strMailBody.Replace("{LogoImage}", emailLogo);
            strMailBody = strMailBody.Replace("{Name}", DisplayName);
            strMailBody = strMailBody.Replace("{Message}", Message);

            bool Chk = SendMail(_sMTPConfigSenderAddress, _sMTPConfigSenderDisplayName, emailAddress, _sMTPConfigSenderAddress, strMailBody, "Application Status", "", ""
                , true, "", _sMTPConfigUserName, _sMTPConfigPassword, _sMTPConfigHost, _sMTPConfigPort, _sMTPConfigEnableSSL);

            return Chk;
        }

        public static bool SendMail(string FromEmail, string FromName, string ToEmails, string ReplyEmail, string MailBody, string MailSubject, string CcEmails, string BccEmails
            , bool IsHtml, string attPath, string UserName, string Password, string Host, int Port, bool EnableSSL)
        {
            SmtpClient smtpClient = new SmtpClient(FromEmail);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential(UserName, Password);
            smtpClient.Port = Port;
            smtpClient.EnableSsl = EnableSSL;
            smtpClient.Host = Host;

            MailMessage objMail = new MailMessage();

            if (!string.IsNullOrEmpty(FromName))
            {
                objMail.From = new MailAddress(FromEmail, FromName);
            }
            else
            {
                objMail.From = new MailAddress(FromEmail);
            }
            if (!string.IsNullOrEmpty(ReplyEmail))
            {
                //objMail.ReplyToList.Add(ReplyEmail);
                objMail.ReplyTo = new MailAddress(ReplyEmail);

            }
            if (!string.IsNullOrEmpty(ToEmails))
            {
                objMail.To.Add(ToEmails);
            }
            if (!string.IsNullOrEmpty(CcEmails))
            {
                objMail.CC.Add(CcEmails);
            }
            if (!string.IsNullOrEmpty(BccEmails))
            {
                objMail.Bcc.Add(BccEmails);
            }
            else
            {
                //Comment for temporary purpose
                //BccEmails = System.Web.Configuration.WebConfigurationManager.AppSettings["ErrorBCCEmail"];
            }
            if (!string.IsNullOrEmpty(attPath))
            {
                Attachment path = new Attachment(attPath);

                objMail.Attachments.Add(path);
            }
            else
            {
                //Comment for temporary purpose
                //BccEmails = System.Web.Configuration.WebConfigurationManager.AppSettings["ErrorBCCEmail"];
            }
            objMail.Subject = MailSubject;
            objMail.Body = MailBody;
            objMail.IsBodyHtml = IsHtml;
            try
            {
                smtpClient.Send(objMail);
                return true;
            }
            catch (Exception e)
            {
                Exception ex = e;
                return false;
            }

        }

        public bool SendChangePasswordMail(string DisplayName, string emailAddress, string UserName, string Password, string RootPath, string beneficiarytype)
        {
            string strMailBody = "";
            string path = string.Empty;

            if (OperatingSystem.IsWindows())
                path = Path.Combine(RootPath + "\\MailTemplate\\ChangePassword.htm");
            else if (OperatingSystem.IsLinux())
                path = Path.Combine(RootPath + "//MailTemplate//ChangePassword.htm");
            else
            {
                path = "";
            }

            StreamReader IDDSbody = new StreamReader(path);
            string beneficiaryname = "";
            if (beneficiarytype == "1")
            {
                beneficiaryname = "ગુજરાત મકાન અને અન્‍ય બાંધકામ શ્રમયોગી કલ્‍યાણ બોર્ડ";
            }
            else
            {
                beneficiaryname = "ગુજરાત શ્રમયોગી કલ્યાણ બોર્ડ";
            }




            strMailBody = IDDSbody.ReadToEnd();
            strMailBody = strMailBody.Replace("{Name}", DisplayName);
            strMailBody = strMailBody.Replace("{BeneficiaryType}", beneficiaryname);
            strMailBody = strMailBody.Replace("{username}", UserName);
            strMailBody = strMailBody.Replace("{password}", Password);


            bool Chk = SendMail(_sMTPConfigSenderAddress, _sMTPConfigSenderDisplayName, emailAddress, _sMTPConfigSenderAddress, strMailBody, "Change Password Mail", "", ""
                , true, "", _sMTPConfigUserName, _sMTPConfigPassword, _sMTPConfigHost, _sMTPConfigPort, _sMTPConfigEnableSSL);

            return Chk;
        }

        public bool SendExceptionMail(ExceptionLogger exceptionLogger, string RootPath, string TemplateName)
        {
            string strMailBody = "";

            //string path = Path.Combine(RootPath + "\\MailTemplate\\" + TemplateName);
            string path = string.Empty;
            if (OperatingSystem.IsWindows())
                path = Path.Combine(RootPath + "\\MailTemplate\\" + TemplateName);
            else if (OperatingSystem.IsLinux())
                path = Path.Combine(RootPath + "//MailTemplate//" + TemplateName);
            else
            {
                path = "";
            }


            StreamReader IDDSbody = new StreamReader(path);



            strMailBody = IDDSbody.ReadToEnd();
            strMailBody = strMailBody.Replace("{ExceptionDate}", DateTime.Now.ToString("dd/MM/yyyyy hh:mm:ss"));
            strMailBody = strMailBody.Replace("{UserID}", Convert.ToString(exceptionLogger.UserId));
            strMailBody = strMailBody.Replace("{ActionName}", Convert.ToString(exceptionLogger.ActionName));
            strMailBody = strMailBody.Replace("{ControllerName}", Convert.ToString(exceptionLogger.ControllerName));
            strMailBody = strMailBody.Replace("{ExceptionMessage}", Convert.ToString(exceptionLogger.ExceptionMessage));
            strMailBody = strMailBody.Replace("{ExceptionStackTrace}", Convert.ToString(exceptionLogger.ExceptionStackTrace));
            strMailBody = strMailBody.Replace("{RequestURI}", Convert.ToString(exceptionLogger.RequestURI));
            strMailBody = strMailBody.Replace("{LineNo}", Convert.ToString(exceptionLogger.LineNumber));
            strMailBody = strMailBody.Replace("{LogFrom}", Convert.ToString(nameof(EnumLookup.exceptionlogfrom.WEB)));
            strMailBody = strMailBody.Replace("{IpAddress}", Convert.ToString(exceptionLogger.IpAddress));
            strMailBody = strMailBody.Replace("{HostName}", Convert.ToString(exceptionLogger.HostName));

            bool Chk = SendMail(_sMTPConfigSenderAddress, _sMTPConfigSenderDisplayName, _emailforexception, _sMTPConfigSenderAddress, strMailBody, "Shramsetu - Exception Occured", "", ""
                , true, "", _sMTPConfigUserName, _sMTPConfigPassword, _sMTPConfigHost, _sMTPConfigPort, _sMTPConfigEnableSSL);

            return Chk;
        }
        #endregion

        #region SendSMS
        public bool SendServiceSMSWithDBLogINGujarati(string SmsContent, string MobileNo, string TemplateId)
        {
            if (_isSMSRequired == "1")
            {
                string response = "";
                string MessageUrl = _messageUrl;
                string UserName = _messageUserName;
                string Password = _messagePassword;
                string SenderId = _messageSenderId;
                string SecureKey = _secureKey;

                if (!string.IsNullOrEmpty(MessageUrl) && !string.IsNullOrEmpty(UserName) &&
                    !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(SenderId) && !string.IsNullOrEmpty(MobileNo)
                    && !string.IsNullOrEmpty(SecureKey))
                {
                    response = SendUnicodeSMS(MessageUrl, UserName, Password, SenderId, MobileNo, SmsContent, SecureKey, TemplateId);
                    return true;
                    //UserRegistrationmgmt.AddSMSLogs(mobile, UserCookies.ServiceId, msg, UserCookies.UserId);
                }
                else
                {
                    return false;
                    throw new ArgumentException("Invalid SMS Arguments Provided");
                }
            }
            else
            {
                return true;
            }
        }

        public static String SendUnicodeSMS(string URL, String username, String password, String senderid, String mobileNos, String Unicodemessage, String secureKey, string TemplateId)
        {
            Stream dataStream;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //forcing .Net framework to use TLSv1.2

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.ProtocolVersion = HttpVersion.Version10;
            request.KeepAlive = false;
            request.ServicePoint.ConnectionLimit = 1;

            ((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";

            request.Method = "POST";

            //ServicePointManager.CertificatePolicy = new MyPolicy();
            String U_Convertedmessage = "";

            foreach (char c in Unicodemessage)
            {
                int j = (int)c;
                String sss = "&#" + j + ";";
                U_Convertedmessage = U_Convertedmessage + sss;
            }
            String encryptedPassword = encryptedPasswod(password);
            String NewsecureKey = hashGenerator(username.Trim(), senderid.Trim(), U_Convertedmessage.Trim(), secureKey.Trim());


            String smsservicetype = "unicodemsg"; // for unicode msg
            String query = "username=" + HttpUtility.UrlEncode(username.Trim()) +
                "&password=" + HttpUtility.UrlEncode(encryptedPassword) +
                "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
                "&content=" + HttpUtility.UrlEncode(U_Convertedmessage.Trim()) +
                "&bulkmobno=" + HttpUtility.UrlEncode(mobileNos) +
                "&senderid=" + HttpUtility.UrlEncode(senderid.Trim()) +
                "&key=" + HttpUtility.UrlEncode(NewsecureKey.Trim()) +
                "&templateid=" + HttpUtility.UrlEncode(TemplateId.Trim());


            byte[] byteArray = Encoding.ASCII.GetBytes(query);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            String Status = ((HttpWebResponse)response).StatusDescription;
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            String responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }
        #endregion
        protected static String hashGenerator(String Username, String sender_id, String message, String secure_key)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Username).Append(sender_id).Append(message).Append(secure_key);
            byte[] genkey = Encoding.UTF8.GetBytes(sb.ToString());
            //static byte[] pwd = new byte[encPwd.Length];
            HashAlgorithm sha1 = HashAlgorithm.Create("SHA512");
            byte[] sec_key = sha1.ComputeHash(genkey);

            StringBuilder sb1 = new StringBuilder();
            for (int i = 0; i < sec_key.Length; i++)
            {
                sb1.Append(sec_key[i].ToString("x2"));
            }
            return sb1.ToString();
        }
        protected static String encryptedPasswod(String password)
        {
            byte[] encPwd = Encoding.UTF8.GetBytes(password);
            //static byte[] pwd = new byte[encPwd.Length];
            HashAlgorithm sha1 = HashAlgorithm.Create("SHA1");
            byte[] pp = sha1.ComputeHash(encPwd);
            // static string result = System.Text.Encoding.UTF8.GetString(pp);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in pp)
            {

                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
        public static string ConcatString(string strtext1, string strtext2, string seperator)
        {
            return string.Concat(strtext1, seperator, strtext2);

        }

        #region Uplod To CouchDB
        public async Task<CouchDBResponse> UplodToCouchDB(CouchDBRequest attachInfo)
        {
            var dbClient = DbHttpClient();
            var jsonData = JsonConvert.SerializeObject(attachInfo);
            var httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var postResult = await dbClient.PostAsync(_couchDbName, httpContent).ConfigureAwait(true);

            var result = await postResult.Content.ReadAsStringAsync();

            var savedInfo = JsonConvert.DeserializeObject<CouchDBResponse>(result);
            var requestContent = new ByteArrayContent(attachInfo.AttachmentData);
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");

            var putResult = await dbClient.PutAsync(_couchDbName + "/" + savedInfo.Id + "/" + attachInfo.FileName + "?rev=" + savedInfo.Rev, requestContent);
            CouchDBResponse couchDBResponse = new CouchDBResponse();
            if (putResult.IsSuccessStatusCode)
            {

                couchDBResponse.IsSuccess = true;
                couchDBResponse.Id = savedInfo.Id;
                couchDBResponse.Rev = savedInfo.Rev;
                couchDBResponse.Result = savedInfo.Result;
                //return new { IsSuccess = true, Result = await putResult.Content.ReadAsStringAsync() };
                return couchDBResponse;
            }

            //return new { IsSuccess = false, Result = putResult.ReasonPhrase };
            couchDBResponse.IsSuccess = false;
            couchDBResponse.Result = putResult.ReasonPhrase;
            return couchDBResponse;
        }

        private HttpClient DbHttpClient()
        {
            var httpClient = _clientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Clear();

            httpClient.BaseAddress = new Uri(_couchDbUrl);
            var dbUserByteArray = Encoding.ASCII.GetBytes(_couchDbUser);
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(dbUserByteArray));
            return httpClient;
        }

        public async Task<CouchDBResponse> GetAttachmentByteArray(string DocId, string AttName)
        {
            var dbClient = DbHttpClient();
            var dbResult = await dbClient.GetAsync(_couchDbName + "/" + DocId + "/" + AttName);
            CouchDBResponse couchDBResponse = new CouchDBResponse();
            if (dbResult.IsSuccessStatusCode)
            {

                couchDBResponse.IsSuccess = true;
                couchDBResponse.ImageData = dbResult.Content.ReadAsByteArrayAsync();
                //return new { IsSuccess = true, Result = await putResult.Content.ReadAsStringAsync() };
                return couchDBResponse;
            }

            //return new { IsSuccess = false, Result = putResult.ReasonPhrase };
            couchDBResponse.IsSuccess = false;
            couchDBResponse.Result = dbResult.ReasonPhrase;
            return couchDBResponse;

        }

        public async Task<CouchDBResponse> GetDocumentByteArray(string DocId, string AttName)
        {
            var dbClient = DbHttpClient();
            var dbResult = await dbClient.GetAsync(_couchDbName + "/" + DocId + "/" + AttName);
            CouchDBResponse couchDBResponse = new CouchDBResponse();
            if (dbResult.IsSuccessStatusCode)
            {

                couchDBResponse.IsSuccess = true;
                couchDBResponse.ImageData = dbResult.Content.ReadAsByteArrayAsync();
                return couchDBResponse;


            }
            couchDBResponse.IsSuccess = false;
            couchDBResponse.Result = dbResult.ReasonPhrase;
            return couchDBResponse;
        }

        public async Task<dynamic> UpdateAttachment(CouchDBRequest attachInfo)
        {
            var dbClient = DbHttpClient();
            var jsonData = JsonConvert.SerializeObject(attachInfo);
            var httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var postResult = await dbClient.PutAsync(_couchDbName + "/" + attachInfo.Id + "?rev=" + attachInfo.Rev, httpContent);
            var result = await postResult.Content.ReadAsStringAsync();
            var savedInfo = JsonConvert.DeserializeObject<CouchDBResponse>(result);
            attachInfo.Id = savedInfo.Id;
            attachInfo.Rev = savedInfo.Rev;
            var requestContent = new ByteArrayContent(attachInfo.AttachmentData);
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
            var putResult = await dbClient.PutAsync(_couchDbName + "/" + savedInfo.Id + "/" + attachInfo.FileName + "?rev=" + savedInfo.Rev, requestContent);
            if (putResult.IsSuccessStatusCode)
            {
                return new
                {
                    IsSuccess = true,
                    Result = await putResult.Content.ReadAsStringAsync()
                };
            }
            return new
            {
                IsSuccess = false,
                Result = putResult.ReasonPhrase
            };
        }

        #endregion

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        public static string GetHostName()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            return host.HostName;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)
            {
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name.ToLower(), type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static string EncryptCRY(string clearText)
        {
            string EncryptionKey = Convert.ToString("Gipllc@321");
            //string EncryptionKey = Convert.ToString("H6HnSNqHLE/r9XthI1xoTTKxmnlvf6gs8hyWuhM/YjeqQUITereqH9RZaacUU7uZ");
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                0x49,0x76,0x61,0x6e,0x20,0x4d,0x65,0x64,0x76,0x65,0x64,0x65,0x76});

                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray()).Replace('/', '_');
                    //clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string DecryptCRY(string cipherText)
        {
            string EncryptionKey = Convert.ToString("Gipllc@321");
            if (cipherText != null) cipherText = cipherText.Replace(" ", "+").Replace('_', '/');
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                0x49,0x76,0x61,0x6e,0x20,0x4d,0x65,0x64,0x76,0x65,0x64,0x65,0x76});
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static string Encrypt(string inputText)
        {
            string encryptionkey = Convert.ToString("Gipllc@321"); //AppConfiguration.EncriptionKey;
            byte[] keybytes = Encoding.ASCII.GetBytes(encryptionkey.Length.ToString());
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            byte[] plainText = Encoding.Unicode.GetBytes(inputText);
            PasswordDeriveBytes pwdbytes = new PasswordDeriveBytes(encryptionkey, keybytes);
            using (ICryptoTransform encryptrans = rijndaelCipher.CreateEncryptor(pwdbytes.GetBytes(32), pwdbytes.GetBytes(16)))
            {
                using (MemoryStream mstrm = new MemoryStream())
                {
                    using (CryptoStream cryptstm = new CryptoStream(mstrm, encryptrans, CryptoStreamMode.Write))
                    {
                        cryptstm.Write(plainText, 0, plainText.Length);
                        cryptstm.Close();
                        return Convert.ToBase64String(mstrm.ToArray());
                    }
                }
            }
        }
        public static string Decrypt(string encryptText)
        {
            string encryptionkey = Convert.ToString("Gipllc@321"); //AppConfiguration.EncriptionKey;
            byte[] keybytes = Encoding.ASCII.GetBytes(encryptionkey.Length.ToString());
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            byte[] encryptedData = Convert.FromBase64String(encryptText.Replace(" ", "+"));
            PasswordDeriveBytes pwdbytes = new PasswordDeriveBytes(encryptionkey, keybytes);
            using (ICryptoTransform decryptrans = rijndaelCipher.CreateDecryptor(pwdbytes.GetBytes(32), pwdbytes.GetBytes(16)))
            {
                using (MemoryStream mstrm = new MemoryStream(encryptedData))
                {
                    using (CryptoStream cryptstm = new CryptoStream(mstrm, decryptrans, CryptoStreamMode.Read))
                    {
                        byte[] plainText = new byte[encryptedData.Length];
                        int decryptedCount = cryptstm.Read(plainText, 0, plainText.Length);
                        return Encoding.Unicode.GetString(plainText, 0, decryptedCount);
                    }

                }
            }
        }

        public static string MaskString(string plainText)
        {
            if (plainText != null && plainText != "")
            {
                //var cardNumber = model1.AadharCardNo;

                //var firstDigits = cardNumber.Substring(0, 6);
                var lastDigits = plainText.Substring(plainText.Length - 4, 4);

                var requiredMask = new String('X', plainText.Length - lastDigits.Length);

                var maskedString = string.Concat(requiredMask, lastDigits);
                var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0 ");
                return maskedCardNumberWithSpaces;
            }
            else
            {
                return "";
            }
        }

        #region RDLC Report Generation
        public static byte[] GenerateReportExcel(DataSet ds, string RootPath, string RDLCFileName, string ReportName, string FileType, bool IsRunTime, string reportPara = "")
        {
            //string logFilePath = string.Empty;
            ////string rootPath = $"{this._webHostEnvironment.WebRootPath}";
            //if (OperatingSystem.IsWindows())
            //    logFilePath = RootPath + "\\LogFiles\\";
            //else if (OperatingSystem.IsLinux())
            //    logFilePath = RootPath + "//LogFiles//";
            //else
            //{
            //    logFilePath = "";
            //}
            try
            {

                string path = "";
                string reportName = "TestReport";
                //string reportPath = RootPath + "\\Reports\\" + RDLCFileName;

                string reportPath = string.Empty;
                if (OperatingSystem.IsWindows())
                    reportPath = RootPath + "\\Reports\\" + RDLCFileName;
                else if (OperatingSystem.IsLinux())
                    reportPath = RootPath + "//Reports//" + RDLCFileName;
                else
                {
                    reportPath = "";
                }

                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension1 = string.Empty;


                Stream reportDefinition;
                using var fs = new FileStream(reportPath, FileMode.Open);
                reportDefinition = fs;
                LocalReport report = new LocalReport();
                report.EnableExternalImages = true;
                report.LoadReportDefinition(reportDefinition);

                if (ds.Tables.Count > 0)
                {
                    if (!string.IsNullOrEmpty(reportPara))
                    {
                        ReportParameter[] parameters = new ReportParameter[1];
                        //parameters[0] = new ReportParameter("CurrentDatetime", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyy hh:mm"));
                        parameters[0] = new ReportParameter("base64Image", reportPara);
                        report.SetParameters(parameters);
                    }
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        if (i == 0)
                        {
                            ReportDataSource datasource = new ReportDataSource("DataSet", ds.Tables[0]);
                            report.DataSources.Add(datasource);
                        }
                        else
                        {
                            ReportDataSource datasource = new ReportDataSource("DataSet" + i + "", ds.Tables[i]);
                            report.DataSources.Add(datasource);
                        }
                    }
                }



                //using (StreamWriter outputFile = new StreamWriter(Path.Combine(logFilePath, "LogWrite.text"), true))
                //{
                //    outputFile.WriteLine("Before:byte");
                //}
                byte[] bytes = report.Render(FileType, null, out mimeType, out encoding, out extension1, out streamIds, out warnings);
                //using (StreamWriter outputFile = new StreamWriter(Path.Combine(logFilePath, "LogWrite.text"), true))
                //{
                //    outputFile.WriteLine("After:byte");
                //}
                fs.Dispose();

                if (IsRunTime == false)
                {
                    //string filename = Path.Combine(RootPath + "\\Reports\\", RDLCFileName);
                    string filename = string.Empty;
                    if (OperatingSystem.IsWindows())
                        filename = Path.Combine(RootPath + "\\Reports\\", RDLCFileName);
                    else if (OperatingSystem.IsLinux())
                        filename = Path.Combine(RootPath + "//Reports//", RDLCFileName);
                    else
                    {
                        filename = "";
                    }

                    using (var fstream = new FileStream(filename, FileMode.Create))
                    {
                        fstream.Write(bytes, 0, bytes.Length);
                        fstream.Close();
                    }
                    return bytes;
                }
                else
                {
                    return bytes;
                }
            }
            catch (Exception ex)
            {
                //using (StreamWriter outputFile = new StreamWriter(Path.Combine(logFilePath, "LogWrite.text"), true))
                //{
                //    outputFile.WriteLine("ex:" + ex);
                //}
                throw ex;

            }
            finally { }
        }

        public static byte[] TestGenerateReportExcel(DataSet ds, string RootPath, string RDLCFileName, string ReportName, string FileType, bool IsRunTime, string reportPara = "")
        {
            try
            {

                string path = "";
                string reportName = "TestReport";
                //string reportPath = RootPath + "\\Reports\\" + RDLCFileName;

                string reportPath = string.Empty;
                if (OperatingSystem.IsWindows())
                    reportPath = RootPath + "\\Reports\\" + RDLCFileName;
                else if (OperatingSystem.IsLinux())
                    reportPath = RootPath + "//Reports//" + RDLCFileName;
                else
                {
                    reportPath = "";
                }

                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension1 = string.Empty;


                Stream reportDefinition;
                using var fs = new FileStream(reportPath, FileMode.Open);
                reportDefinition = fs;
                LocalReport report = new LocalReport();
                report.EnableExternalImages = true;
                report.LoadReportDefinition(reportDefinition);

                byte[] bytes = report.Render(FileType, null, out mimeType, out encoding, out extension1, out streamIds, out warnings);
                fs.Dispose();

                if (IsRunTime == false)
                {
                    //string filename = Path.Combine(RootPath + "\\Reports\\", RDLCFileName);
                    string filename = string.Empty;
                    if (OperatingSystem.IsWindows())
                        filename = Path.Combine(RootPath + "\\Reports\\", RDLCFileName);
                    else if (OperatingSystem.IsLinux())
                        filename = Path.Combine(RootPath + "//Reports//", RDLCFileName);
                    else
                    {
                        filename = "";
                    }

                    using (var fstream = new FileStream(filename, FileMode.Create))
                    {
                        fstream.Write(bytes, 0, bytes.Length);
                        fstream.Close();
                    }
                    return bytes;
                }
                else
                {
                    return bytes;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion

        #region Export to CSV 

        public static byte[] ExportToCSV(DataTable exportData, string tableName, bool headerRequired = true)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                exportData.TableName = tableName;
                var ws = wb.Worksheets.Add(exportData);
                //var ws = wb.Worksheets.Add(exportData.TableName);
                if (!headerRequired)
                {

                    //ws.Range("A1:HFD1").
                    //ws.Row(0).Delete();
                    //ws.Table(0).ShowHeaderRow = false;
                    //ws.Table(0).HeadersRow().Delete();
                    ws.Range("A1", "A1").Delete(XLShiftDeletedCells.ShiftCellsUp);
                    ws.Range("B1", "B1").Delete(XLShiftDeletedCells.ShiftCellsUp);
                    ws.Range("C1", "C1").Delete(XLShiftDeletedCells.ShiftCellsUp);
                    ws.Range("D1", "D1").Delete(XLShiftDeletedCells.ShiftCellsUp);
                    ws.Range("E1", "E1").Delete(XLShiftDeletedCells.ShiftCellsUp);
                    ws.Range("F1", "F1").Delete(XLShiftDeletedCells.ShiftCellsUp);
                    ws.Range("G1", "G1").Delete(XLShiftDeletedCells.ShiftCellsUp);
                    ws.Range("H1", "H1").Delete(XLShiftDeletedCells.ShiftCellsUp);
                    ws.Range("I1", "I1").Delete(XLShiftDeletedCells.ShiftCellsUp);
                    ws.Range("J1", "J1").Delete(XLShiftDeletedCells.ShiftCellsUp);
                    ws.Range("K1", "K1").Delete(XLShiftDeletedCells.ShiftCellsUp);
                    ws.Range("L1", "L1").Delete(XLShiftDeletedCells.ShiftCellsUp);
                    ws.Table(0).Theme = XLTableTheme.None;
                }
                ws.Tables.FirstOrDefault().ShowAutoFilter = false;


                //START : set borders to each used cell in excel
                ws.CellsUsed().Style.Border.BottomBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                ws.CellsUsed().Style.Border.BottomBorderColor = ClosedXML.Excel.XLColor.Black;
                ws.CellsUsed().Style.Border.TopBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                ws.CellsUsed().Style.Border.TopBorderColor = ClosedXML.Excel.XLColor.Black;
                ws.CellsUsed().Style.Border.LeftBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                ws.CellsUsed().Style.Border.LeftBorderColor = ClosedXML.Excel.XLColor.Black;
                ws.CellsUsed().Style.Border.RightBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                ws.CellsUsed().Style.Border.RightBorderColor = ClosedXML.Excel.XLColor.Black;
                //END : set borders to each used cell in excel

                //ws.Columns().AdjustToContents();

                //ws.Range("A35:B35").Merge();
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return stream.ToArray();
                }

            }

        }

        public static byte[] ExportToCSVUsingComma(DataTable dt)
        {
            Log.Information("*************** START : ExportToCSVUsingComma Count(ExportToCSVUsingComma (Date : " + DateTime.Now.ToString("dd/MM/yyyy hhmmss") + " )  ***************");
            byte[] csvData = null;
            if (dt.Rows.Count > 0)
            {
                //string path = AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["LocalSource"];
                //{
                try
                {
                    //checked for the datatable dtCSV not empty
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        // create object for the StringBuilder class
                        StringBuilder sb = new StringBuilder();

                        //dt.Columns.Remove("Srl");
                        foreach (DataRow row in dt.Rows)
                        {
                            IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                            sb.AppendLine(string.Join(",", fields));
                            //File.WriteAllText(path + "//" + fileName, sb.ToString());
                        }
                        Log.Information("*************** START : sb (csvData (sb : " + sb.ToString() + " )  ***************");
                        csvData = Encoding.ASCII.GetBytes(sb.ToString());
                        // save the file
                        // File.WriteAllText(path + "//" + fileName, sb.ToString(), System.Text.Encoding.UTF8);
                        Log.Information("*************** START : csvData (csvData (csvData : " + csvData + " )  ***************");
                    }
                }
                catch (Exception ex)
                {
                    return csvData;
                }

            }
            return csvData;
        }

        #endregion

        public int BOCWUploadFileOnServer(string SourceFile)
        {

            Log.Information("*************** START : BOCWUploadFileOnServer (Date : " + DateTime.Now.ToString("dd/MM/yyyy hhmmss") + " )  ***************");

            #region Save File in Network Shared Folder With Credentials
            int result = 0;
            try
            {
                using (SftpClient client = new SftpClient(_sftpHost, _sftpPort, _sftpUserName, _sftpPassword))
                {
                    client.Connect();

                    if (client.IsConnected)
                    {
                        Log.Information("SFTP Client Connected");
                        client.ChangeDirectory(_sftpDestinationUploadPath);
                        using (FileStream fs = new FileStream(SourceFile, FileMode.Open))
                        {
                            //client.BufferSize = 4 * 1024;

                            if (System.IO.File.Exists(Path.Combine(_sftpDestinationUploadPath, Path.GetFileName(SourceFile))))
                            {
                                Log.Information("File Already Exist (File Name : " + SourceFile + ")");
                            }
                            else
                            {
                                client.UploadFile(fs, Path.GetFileName(SourceFile));
                                Log.Information("File Uploaded on SFTP");
                            }

                        }
                        client.Dispose();
                        result = 1;
                    }
                    else
                    {
                        Log.Information("SFTP Client Not Connected");
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
            #endregion

            Log.Information("*************** END : BOCWUploadFileOnServer (Date : " + DateTime.Now.ToString("dd/MM/yyyy hhmmss") + " )  ***************");
        }

        public int BOCWReadFileFromServer(string[] FileNameToRead, string rootPath)
        {

            Log.Information("*************** START : BOCWReadFileFromServer (Date : " + DateTime.Now.ToString("dd/MM/yyyy hhmmss") + " )  ***************");

            #region Save File in Network Shared Folder With Credentials
            int result = 0;
            try
            {
                using (var sftpClient = new SftpClient(_sftpHost, _sftpPort, _sftpUserName, _sftpPassword))
                {
                    sftpClient.Connect();
                    if (sftpClient.IsConnected)
                    {
                        Log.Information("SFTP Client Connected");
                        rootPath = Path.Combine(rootPath + "/", _BOCWResponsePath);
                        foreach (var fileItemToRead in FileNameToRead)
                        {
                            string fileName = _BOCWCorporateId + "_REV_" + fileItemToRead;
                            //var files = sftpClient.ListDirectory(_sftpDestinationReadPath + "/" + fileName);
                            var files = sftpClient.ListDirectory(_sftpDestinationReadPath);

                            Log.Information("START To Read File :" + fileName);
                            if (files != null)
                            {

                                files = files.Where(f => f.Name == fileName).ToList();

                                

                                if (!Directory.Exists(rootPath))
                                {
                                    Directory.CreateDirectory(rootPath);
                                }
                                foreach (var file in files)
                                {
                                    string FileName = Path.GetFileName(file.Name.ToString());
                                    DownloadResponseFileFromServerToLocal(sftpClient, file, rootPath);

                                    //if (FileName.StartsWith("UJ491RBI"))
                                    //{
                                    //    string filenameCltExtension = Path.GetExtension(file.Name.ToString());
                                    //    if (filenameCltExtension.Equals(".clt"))
                                    //    {

                                    //        if (!file.IsDirectory && !file.IsSymbolicLink)
                                    //        {
                                    //            DownloadCLTFileFromServer(sftpClient, file, LocalDest);
                                    //        }
                                    //        else if (file.IsSymbolicLink)
                                    //        {
                                    //            ServerWriteToFile("Ignoring symbolic link {0}" + file.Name, "ServerFileLog");
                                    //        }
                                    //    }
                                    //    else
                                    //    {

                                    //        ServerWriteToFile("Invalid File" + file.Name, "ServerFileLog");
                                    //    }
                                    //}

                                }
                                
                            }

                            Log.Information("END To Read File :" + fileName);

                        }
                    }
                    else
                    {
                        Log.Information("SFTP Client Not Connected");
                        //WriteToFile("Step 1 : Server is Not Connected. Date Time : " + DateTime.Now, "ServerInfo");
                    }
                    sftpClient.Dispose();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
            #endregion

            Log.Information("*************** END : BOCWReadFileFromServer (Date : " + DateTime.Now.ToString("dd/MM/yyyy hhmmss") + " )  ***************");
        }

        public DataTable BOCWReadCSVFileFromLocalServer(string dest, string file)
        {

            Log.Information("*************** START : BOCWReadCSVFileFromLocalServer (Date : " + DateTime.Now.ToString("dd/MM/yyyy hhmmss") + " )  ***************");
            DataTable dtCsv = new DataTable();
            Log.Information("Path:" + dest + " || File Name : " + file + "");
            try
            {
                using (StreamReader sr = new StreamReader(Path.Combine(dest, file)))
                {

                    dtCsv.Columns.Add("srno", typeof(System.String));
                    dtCsv.Columns.Add("accountholdername", typeof(System.String));
                    dtCsv.Columns.Add("place", typeof(System.String));
                    dtCsv.Columns.Add("applicationno", typeof(System.String));
                    dtCsv.Columns.Add("beneficiaryaccountno", typeof(System.String));
                    dtCsv.Columns.Add("ifsccode", typeof(System.String));
                    dtCsv.Columns.Add("amount", typeof(System.Decimal));
                    dtCsv.Columns.Add("boardname", typeof(System.String));
                    dtCsv.Columns.Add("boarddebitaccountnumber", typeof(System.String));
                    dtCsv.Columns.Add("paymenttype", typeof(System.String));
                    dtCsv.Columns.Add("uniqueid", typeof(System.String));
                    dtCsv.Columns.Add("schemename", typeof(System.String));
                    dtCsv.Columns.Add("transactiondate", typeof(System.String));
                    dtCsv.Columns.Add("transactionstatus", typeof(System.String));
                    dtCsv.Columns.Add("utrno", typeof(System.String));
                    dtCsv.Columns.Add("returnreason", typeof(System.String));



                    while (!sr.EndOfStream)
                    {
                        string FullText;
                        FullText = sr.ReadToEnd(); //read full file text  
                        string[] rows = FullText.Split('\n'); //split full file text into rows 

                        for (int i = 0; i < rows.Length - 1; i++)
                        {

                            string[] rowValues0 = rows[i].Split(','); //split each row with comma to get individual values 
                            DataRow row = dtCsv.Rows.Add();
                            for (int j = 0; j < rowValues0.Length; j++)
                            {
                                row.SetField(j, rowValues0[j]);
                            }
                        }

                    }
                    return dtCsv;
                }

                return null;
            }
            catch (Exception ex)
            {
                // WriteToFile("Error in ReadCSVFile : FileName: " + file + " : " + DateTime.Now + " Message " + ex.Message, "LocalException");
                return dtCsv;
            }

            Log.Information("*************** END : BOCWReadCSVFileFromLocalServer (Date : " + DateTime.Now.ToString("dd/MM/yyyy hhmmss") + " )  ***************");
        }


        private void DownloadResponseFileFromServerToLocal(SftpClient client, SftpFile file, string destinationDirectory)
        {
            Log.Information("*************** START : DownloadResponseFileFromServerToLocal (Date : " + DateTime.Now.ToString("dd/MM/yyyy hhmmss") + " )  ***************");
            try
            {
                using (Stream fileStream = File.OpenWrite(Path.Combine(destinationDirectory, file.Name)))
                {
                    string FileName = Path.GetFileName(file.Name.ToString());

                    if (File.Exists(Path.Combine(destinationDirectory, file.Name)))
                    {
                        if (file.Length != fileStream.Length)
                        {

                            client.DownloadFile(file.FullName, fileStream);
                            Log.Information("File Successfully Downloaded In Local Folder. File Name :" + file.FullName);
                        }
                        else
                        {
                            Log.Information("File Already Exits. File Name :" + file.FullName);
                        }
                    }
                    else
                    {
                        client.DownloadFile(file.FullName, fileStream);
                        Log.Information("File Not Exits on Server. File Name :" + file.FullName);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Information("DownloadResponseFileFromServerToLocal Exception. Exception Message : " + e.Message);
                throw;
            }
            Log.Information("*************** END : DownloadResponseFileFromServerToLocal (Date : " + DateTime.Now.ToString("dd/MM/yyyy hhmmss") + " )  ***************");
        }


        #region GLWB Payment Process
        public int GLWBUploadFileOnServer(string SourceFile)
        {

            Log.Information("*************** START : GLWBUploadFileOnServer (Date : " + DateTime.Now.ToString("dd/MM/yyyy hhmmss") + " )  ***************");

            #region Save File in Network Shared Folder With Credentials
            int result = 0;
            try
            {
                using (SftpClient client = new SftpClient(_sftpHost, _sftpPort, _sftpUserName, _sftpPassword))
                {
                    client.Connect();

                    if (client.IsConnected)
                    {
                        Log.Information("SFTP Client Connected");
                        client.ChangeDirectory(_sftpDestinationUploadPath);
                        using (FileStream fs = new FileStream(SourceFile, FileMode.Open))
                        {
                            //client.BufferSize = 4 * 1024;

                            if (System.IO.File.Exists(Path.Combine(_sftpDestinationUploadPath, Path.GetFileName(SourceFile))))
                            {
                                Log.Information("File Already Exist (File Name : " + SourceFile + ")");
                            }
                            else
                            {
                                client.UploadFile(fs, Path.GetFileName(SourceFile));
                                Log.Information("File Uploaded on SFTP");
                            }

                        }
                        client.Dispose();
                        result = 1;
                    }
                    else
                    {
                        Log.Information("SFTP Client Not Connected");
                    }

                }

            }
            catch (Exception ex)
            {
                Log.Information("SFTP Client Not Connected ex" + ex.Message);
                throw ex;
            }
            return result;
            #endregion

            Log.Information("*************** END : GLWBUploadFileOnServer (Date : " + DateTime.Now.ToString("dd/MM/yyyy hhmmss") + " )  ***************");
        }

        public int GLWBReadFileFromServer(string[] FileNameToRead, string rootPath)
        {

            Log.Information("*************** START : GLWBReadFileFromServer (Date : " + DateTime.Now.ToString("dd/MM/yyyy hhmmss") + " )  ***************");

            #region Save File in Network Shared Folder With Credentials
            int result = 0;
            try
            {
                using (var sftpClient = new SftpClient(_sftpHost, _sftpPort, _sftpUserName, _sftpPassword))
                {
                    sftpClient.Connect();
                    if (sftpClient.IsConnected)
                    {
                        Log.Information("SFTP Client Connected");
                        rootPath = Path.Combine(rootPath + "/", _GLWBResponsePath);
                        foreach (var fileItemToRead in FileNameToRead)
                        {
                            string fileName = _GLWBCorporateId + "_REV_" + fileItemToRead;
                            //var files = sftpClient.ListDirectory(_sftpDestinationReadPath + "/" + fileName);
                            var files = sftpClient.ListDirectory(_sftpDestinationReadPath);

                            Log.Information("START To Read File :" + fileName);
                            if (files != null)
                            {
                                files = files.Where(f => f.Name == fileName).ToList();

                                

                                if (!Directory.Exists(rootPath))
                                {
                                    Directory.CreateDirectory(rootPath);
                                }
                                foreach (var file in files)
                                {
                                    string FileName = Path.GetFileName(file.Name.ToString());
                                    DownloadResponseFileFromServerToLocal(sftpClient, file, rootPath);

                                    //if (FileName.StartsWith("UJ491RBI"))
                                    //{
                                    //    string filenameCltExtension = Path.GetExtension(file.Name.ToString());
                                    //    if (filenameCltExtension.Equals(".clt"))
                                    //    {

                                    //        if (!file.IsDirectory && !file.IsSymbolicLink)
                                    //        {
                                    //            DownloadCLTFileFromServer(sftpClient, file, LocalDest);
                                    //        }
                                    //        else if (file.IsSymbolicLink)
                                    //        {
                                    //            ServerWriteToFile("Ignoring symbolic link {0}" + file.Name, "ServerFileLog");
                                    //        }
                                    //    }
                                    //    else
                                    //    {

                                    //        ServerWriteToFile("Invalid File" + file.Name, "ServerFileLog");
                                    //    }
                                    //}

                                }
                                
                            }

                            Log.Information("END To Read File :" + fileName);

                        }
                    }
                    else
                    {
                        Log.Information("SFTP Client Not Connected");
                        //WriteToFile("Step 1 : Server is Not Connected. Date Time : " + DateTime.Now, "ServerInfo");
                    }
                    sftpClient.Dispose();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
            #endregion

            Log.Information("*************** END : GLWBReadFileFromServer (Date : " + DateTime.Now.ToString("dd/MM/yyyy hhmmss") + " )  ***************");
        }

        public DataTable GLWBReadCSVFileFromLocalServer(string dest, string file)
        {

            Log.Information("*************** START : GLWBReadCSVFileFromLocalServer (Date : " + DateTime.Now.ToString("dd/MM/yyyy hhmmss") + " )  ***************");
            DataTable dtCsv = new DataTable();
            Log.Information("Path:" + dest + " || File Name : " + file + "");
            try
            {
                using (StreamReader sr = new StreamReader(Path.Combine(dest, file)))
                {

                    dtCsv.Columns.Add("srno", typeof(System.String));
                    dtCsv.Columns.Add("accountholdername", typeof(System.String));
                    dtCsv.Columns.Add("place", typeof(System.String));
                    dtCsv.Columns.Add("applicationno", typeof(System.String));
                    dtCsv.Columns.Add("beneficiaryaccountno", typeof(System.String));
                    dtCsv.Columns.Add("ifsccode", typeof(System.String));
                    dtCsv.Columns.Add("amount", typeof(System.Decimal));
                    dtCsv.Columns.Add("boardname", typeof(System.String));
                    dtCsv.Columns.Add("boarddebitaccountnumber", typeof(System.String));
                    dtCsv.Columns.Add("paymenttype", typeof(System.String));
                    dtCsv.Columns.Add("uniqueid", typeof(System.String));
                    dtCsv.Columns.Add("schemename", typeof(System.String));
                    dtCsv.Columns.Add("transactiondate", typeof(System.String));
                    dtCsv.Columns.Add("transactionstatus", typeof(System.String));
                    dtCsv.Columns.Add("utrno", typeof(System.String));
                    dtCsv.Columns.Add("returnreason", typeof(System.String));



                    while (!sr.EndOfStream)
                    {
                        string FullText;
                        FullText = sr.ReadToEnd(); //read full file text  
                        string[] rows = FullText.Split('\n'); //split full file text into rows 

                        for (int i = 0; i < rows.Length - 1; i++)
                        {

                            string[] rowValues0 = rows[i].Split(','); //split each row with comma to get individual values 
                            DataRow row = dtCsv.Rows.Add();
                            for (int j = 0; j < rowValues0.Length; j++)
                            {
                                row.SetField(j, rowValues0[j]);
                            }
                        }

                    }
                    return dtCsv;
                }

                return null;
            }
            catch (Exception ex)
            {
                // WriteToFile("Error in ReadCSVFile : FileName: " + file + " : " + DateTime.Now + " Message " + ex.Message, "LocalException");
                return dtCsv;
            }

            Log.Information("*************** END : GLWBReadCSVFileFromLocalServer (Date : " + DateTime.Now.ToString("dd/MM/yyyy hhmmss") + " )  ***************");
        }
        #endregion



        private static readonly char[] Punctuations = "!@#$%^&*()_-+[{]}:>|/?".ToCharArray();
        public static string Generate(int length, int numberOfNonAlphanumericCharacters)
        {
            if (length < 1 || length > 128)
            {
                throw new ArgumentException("length");
            }

            if (numberOfNonAlphanumericCharacters > length || numberOfNonAlphanumericCharacters < 0)
            {
                throw new ArgumentException("numberOfNonAlphanumericCharacters");
            }

            using (var rng = RandomNumberGenerator.Create())
            {
                var byteBuffer = new byte[length];

                rng.GetBytes(byteBuffer);

                var count = 0;
                var characterBuffer = new char[length];

                for (var iter = 0; iter < length; iter++)
                {
                    var i = byteBuffer[iter] % 87;

                    if (i < 10)
                    {
                        characterBuffer[iter] = (char)('0' + i);
                    }
                    else if (i < 36)
                    {
                        characterBuffer[iter] = (char)('A' + i - 10);
                    }
                    else if (i < 62)
                    {
                        characterBuffer[iter] = (char)('a' + i - 36);
                    }
                    else
                    {
                        characterBuffer[iter] = Punctuations[GetRandomInt(rng, Punctuations.Length)];
                        count++;
                    }
                }

                if (count >= numberOfNonAlphanumericCharacters)
                {
                    return new string(characterBuffer);
                }

                int j;

                for (j = 0; j < numberOfNonAlphanumericCharacters - count; j++)
                {
                    int k;
                    do
                    {
                        k = GetRandomInt(rng, length);
                    }
                    while (!char.IsLetterOrDigit(characterBuffer[k]));

                    characterBuffer[k] = Punctuations[GetRandomInt(rng, Punctuations.Length)];
                }

                return new string(characterBuffer);
            }
        }

        private static int GetRandomInt(RandomNumberGenerator randomGenerator)
        {
            var buffer = new byte[4];
            randomGenerator.GetBytes(buffer);

            return BitConverter.ToInt32(buffer);
        }
        private static int GetRandomInt(RandomNumberGenerator randomGenerator, int maxInput)
        {
            return Math.Abs(GetRandomInt(randomGenerator) % maxInput);
        }


        public static string GenerateRandomStrongPassword(int length)
        {
            string alphaCaps = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string alphaLow = "abcdefghijklmnopqrstuvwxyz";
            string numerics = "1234567890";
            string special = "@#$!";
            string allChars = alphaCaps + alphaLow + numerics;

            String generatedPassword = "";
            if (length < 4)
                throw new Exception("Number of characters should be greater than 4.");

            int lowerpass, upperpass, numpass, specialchar;
            string posarray = "0123456789";
            if (length < posarray.Length)
                posarray = posarray.Substring(0, length);
            lowerpass = getRandomPosition(ref posarray);
            upperpass = getRandomPosition(ref posarray);
            numpass = getRandomPosition(ref posarray);
            specialchar = getRandomPosition(ref posarray);


            for (int i = 0; i < length; i++)
            {
                if (i == lowerpass)
                    generatedPassword += getRandomChar(alphaCaps);
                else if (i == upperpass)
                    generatedPassword += getRandomChar(alphaLow);
                else if (i == numpass)
                    generatedPassword += getRandomChar(numerics);
                else if (i == specialchar)
                    generatedPassword += getRandomChar(special);
                else
                    generatedPassword += getRandomChar(allChars);
            }
            return generatedPassword;
        }


        private static string getRandomChar(string fullString)
        {
            return fullString.ToCharArray()[(int)Math.Floor(new Random().NextDouble() * fullString.Length)].ToString();
        }

        private static int getRandomPosition(ref string posArray)
        {
            int pos;
            string randomChar = posArray.ToCharArray()[(int)Math.Floor(new Random().NextDouble() * posArray.Length)].ToString();
            pos = int.Parse(randomChar);
            posArray = posArray.Replace(randomChar, "");
            return pos;
        }

        public async Task<APIResponse> CallWebAPI(object dataobject)
        {

            try
            {


                APIResponse objApiResponse = new APIResponse();
                using (var client = new HttpClient())
                {
                    // Setting Base address.  
                    client.BaseAddress = new Uri(Convert.ToString(_bocwRegistrationAPI));


                    // Setting content type.  
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", _bocwAuthHeader);


                    // Initialization.  
                    HttpResponseMessage response = new HttpResponseMessage();


                    // HTTP GET  
                    //response = await client.GetAsync(requestUri).ConfigureAwait(false);
                    //var myObject = new
                    //{
                    //    ApplicationNo = "2201000266",
                    //    UniqueIDNumber = "889411925099",
                    //};

                    response = await client.PostAsJsonAsync(_bocwRegistrationAPI, dataobject);


                    // Verification  
                    if (response.IsSuccessStatusCode)
                    {
                        // Reading Response.  
                        var result = response.Content.ReadAsStringAsync().Result;
                        objApiResponse.Result = result;
                    }
                }
                return objApiResponse;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public static String CreateCTPPOSTForm(string url, NameValueCollection data)
        {
            try
            {
                //Set a name for the form
                string formID = "CTPPostForm";
                string outputHTML = "<html>";
                outputHTML += ("<head>");
                outputHTML += ("<title>Merchant Checkout Page</title>");
                outputHTML += ("</head>");
                outputHTML += ("<body>");
                outputHTML += "<center><h1>Please do not refresh this page...</h1></center>";
                outputHTML += "<form method='post' id='"+ formID + "' action='" + url + "' name='" + formID + "'>";
                outputHTML += "<table border='1'>";
                outputHTML += "<tbody>";
                foreach (string key in data.Keys)
                {
                    outputHTML += "<input type='hidden' name='" + key + "' value='" + data[key] + "'>'";
                }
                //foreach (string key in data)
                //{
                //    strForm.Append("<input type=\"hidden\" name=\"" + key +
                //                   "\" value=\"" + data[key] + "\">");
                //}
                outputHTML += "</tbody>";
                outputHTML += "</table>";
                outputHTML += "<script type='text/javascript'>";
                outputHTML += "document.'"+ formID + "'.submit();";
                outputHTML += "</script>";
                outputHTML += "</form>";
                outputHTML += "</body>";
                outputHTML += "</html>";
                return outputHTML;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

    }

}
