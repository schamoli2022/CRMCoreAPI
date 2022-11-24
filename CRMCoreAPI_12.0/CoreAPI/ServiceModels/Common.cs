using CRMCoreAPI.ServiceModels;
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CRMCoreAPI.Common
{
    public class CommonClass
    {
        private readonly string connectionString = null;
        private IConfiguration configuration;

        public CommonClass(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = configuration.GetSection("ConnectionStrings").GetSection("CRMCoreAPIContextConnection").Value;
        }
        public static string ReturnMessage(string returnData)
        {
            List<ListString> message = new List<ListString>();
            if (!string.IsNullOrWhiteSpace(returnData))
            {
                var returnobj = JsonConvert.DeserializeObject(Convert.ToString(returnData));
                string code = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["CODE"]);
                string number = "";
                string leadwid = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["MAIN_ID"]);
                try
                {
                    number = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["ORDER_NUMBER"]);
                }
                catch { }
                if (code == "1")
                {
                    message.Add(new ListString()
                    {
                        Status = "Success",
                        Code = code,
                        Name = number,
                        Value= leadwid
                    });
                }
                else if (code == "2")
                {
                    message.Add(new ListString()
                    {
                        Status = "Exist",
                        Code = code,
                        Name = number,
                    });
                }
                else
                {
                    message.Add(new ListString()
                    {
                        Status = "Failure",
                        Code = code
                    });
                    return JsonConvert.SerializeObject(message);
                }
                return JsonConvert.SerializeObject(message);
            }
            else
            {
                message.Add(new ListString()
                {
                    Status = "Failure",
                    Code = "0"
                });
                return JsonConvert.SerializeObject(message);
            }
        }

        //public static void Main11()
        //{
        //    string original = "5623865";

        //    // Create a new instance of the Aes
        //    // class.  This generates a new key and initialization 
        //    // vector (IV).
        //    string name = HttpUtility.UrlEncode(Encrypt(original));

        //    string newname=Decrypt(HttpUtility.UrlDecode(name));
        //    //using (Aes myAes = Aes.Create())
        //    //{


        //    //    // Encrypt the string to an array of bytes.
        //    //    byte[] encrypted = EncryptStringToBytes_Aes(original, myAes.Key, myAes.IV);

        //    //    // Decrypt the bytes to a string.

        //    //    //string abc= System.Text.Encoding.UTF8.GetString(encrypted);
        //    //    StringBuilder builder = new StringBuilder();
        //    //    //for (int i = 0; i < encrypted.Length; i++)
        //    //    //{
        //    //    //    builder.Append(encrypted[i].ToString("x2"));
        //    //    //}
        //    //    //string abc = builder.ToString();
        //    //    string strModified = System.Text.Encoding.Unicode.GetString(encrypted);

        //    //    //Encoding.ASCII.GetString


        //    //    byte[] b = Encoding.Unicode.GetBytes(strModified);

        //    //    //string value = System.Text.Encoding.UTF8.GetString(encrypted);


        //    //    string roundtrip = DecryptStringFromBytes_Aes(b, myAes.Key, myAes.IV);
        //    //    //byte[] array = Encoding.UTF8.GetBytes(value);

        //    //    //var result = System.Text.Encoding.Unicode.GetBytes(abc);
        //    //    //Display the original data and the decrypted data.
        //    //    Console.WriteLine("Original:   {0}", original);
        //    //    Console.WriteLine("Round Trip: {0}", roundtrip);
        //    //}
        //}

        public void SetInboxNotification(object returnobj, string typeCode,string resourceMessageName, string title)
        {
            long companyId = Convert.ToInt64(JObject.Parse(Convert.ToString(returnobj))["COMPANY_ID"]);
            long createdBy = Convert.ToInt64(JObject.Parse(Convert.ToString(returnobj))["CREATED_BY"]);
            string notificationMessage = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["NOTIFICATION_MESSAGE"]);
            string notificationFor = Convert.ToString(JObject.Parse(Convert.ToString(returnobj))["NOTIFICATION_FOR"]);
            long primaryKeyValue = Convert.ToInt64(JObject.Parse(Convert.ToString(returnobj))["MAIN_ID"]);

           // string title = Resources.Resources.ResourceManager.GetString(ENotificationTypeCode.CRM_LEAD_INSERT.ToString());

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@MESSAGE", Convert.ToString(notificationMessage));
                dynamicParameters.Add("@USER_IDS", Convert.ToString(notificationFor));
                dynamicParameters.Add("@COMPANY_ID", Convert.ToString(companyId));
                dynamicParameters.Add("@CREATED_BY", Convert.ToString(createdBy));
                dynamicParameters.Add("@DATE", DateTime.UtcNow);
                dynamicParameters.Add("@TYPE_CODE", Convert.ToString(typeCode));
                dynamicParameters.Add("@REFERENCE_ID", Convert.ToString(primaryKeyValue));
                dynamicParameters.Add("@REFERENCE_ID2", 0);
                var returnData = sqlConnection.Query<string>("TALYGEN_SP_SERVICE_INBOX_SET", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                string message = Resources.Resources.ResourceManager.GetString(resourceMessageName);

                SetEmailNotification(notificationMessage, message, notificationFor, title, companyId, createdBy);


            }
        }
        private void SetEmailNotification(string dataMessage, string resourceMessage, string notificationFor, string title, long companyId, long createdBy)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@RETURN_JSON", Convert.ToString(dataMessage));
                dynamicParameters.Add("@RESOURCE_MESSAGE", Convert.ToString(resourceMessage));
                dynamicParameters.Add("@NOTIFICATION_FOR", Convert.ToString(notificationFor));
                dynamicParameters.Add("@COMPANY_ID", Convert.ToString(companyId));
                dynamicParameters.Add("@CREATED_BY", Convert.ToString(createdBy));
                dynamicParameters.Add("@SUBJECT", title);
                var returnData = sqlConnection.Query<string>("TALYGEN_SERVICE_CRM_SET_EMAIL_TEMPLATE_CONTENT_EMAIL_LOG", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
        }
        //static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        //{
        //    // Check arguments.
        //    if (plainText == null || plainText.Length <= 0)
        //        throw new ArgumentNullException("plainText");
        //    if (Key == null || Key.Length <= 0)
        //        throw new ArgumentNullException("Key");
        //    if (IV == null || IV.Length <= 0)
        //        throw new ArgumentNullException("IV");
        //    byte[] encrypted;

        //    // Create an Aes object
        //    // with the specified key and IV.
        //    using (Aes aesAlg = Aes.Create())
        //    {
        //        aesAlg.Key = Key;
        //        aesAlg.IV = IV;

        //        // Create an encryptor to perform the stream transform.
        //        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        //        // Create the streams used for encryption.
        //        using (MemoryStream msEncrypt = new MemoryStream())
        //        {
        //            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        //            {
        //                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
        //                {
        //                    //Write all data to the stream.
        //                    swEncrypt.Write(plainText);
        //                }
        //                encrypted = msEncrypt.ToArray();
        //            }
        //        }
        //    }


        //    // Return the encrypted bytes from the memory stream.
        //    return encrypted;

        //}

        //static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        //{
        //    // Check arguments.
        //    if (cipherText == null || cipherText.Length <= 0)
        //        throw new ArgumentNullException("cipherText");
        //    if (Key == null || Key.Length <= 0)
        //        throw new ArgumentNullException("Key");
        //    if (IV == null || IV.Length <= 0)
        //        throw new ArgumentNullException("IV");

        //    // Declare the string used to hold
        //    // the decrypted text.
        //    string plaintext = null;

        //    // Create an Aes object
        //    // with the specified key and IV.
        //    using (Aes aesAlg = Aes.Create())
        //    {
        //        aesAlg.Key = Key;
        //        aesAlg.IV = IV;

        //        // Create a decryptor to perform the stream transform.
        //        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        //        // Create the streams used for decryption.
        //        using (MemoryStream msDecrypt = new MemoryStream(cipherText))
        //        {
        //            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
        //            {
        //                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
        //                {

        //                    // Read the decrypted bytes from the decrypting stream
        //                    // and place them in a string.
        //                    plaintext = srDecrypt.ReadToEnd();
        //                }
        //            }
        //        }

        //    }

        //    return plaintext;

        //}

        /// <summary>
        /// For encrypting the string using System.Security.CryptoGraphy
        /// </summary>
        /// <param name="clearText"></param>
        /// <returns></returns>
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        /// <summary>
        /// For decrypting the string using System.Security.CryptoGraphy
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
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
    }
    public class ListString
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
    }
    public class InboxEmailNotification
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }




    #region TimeAttendance


    public class AttendanceReport
    {
        public string EmpCode { get; set; }

        public string EmpName { get; set; }

        public string EmpShift { get; set; }

        public string EmpDepartment { get; set; }

        public string EmpLocation { get; set; }

        public string MinDate { get; set; }

        public string MaxDate { get; set; }

        public string InOutDate { get; set; }

        public string In_Time { get; set; }

        public string Out_Time { get; set; }

        public string ShiftStartTime { get; set; }

        public string ShiftEndTime { get; set; }

        public string Early_Entry { get; set; }

        public string Late_Entry { get; set; }

        public string Early_Exit { get; set; }

        public string Late_Exit { get; set; }

        public string Adjustment_Hours { get; set; }

        public string WorkingHours { get; set; }

        public string Mark { get; set; }

    }


    public class AttendanceReportList
    {

        public List<AttendanceReport> GetAtndReport { get; set; }
    }

    public class GetEmployeeNames
    {
        public long USER_ID { get; set; }

        public string USERNAME { get; set; }
    }

    public class GetDeptNames
    {
        public long DEPARTMENT_ID { get; set; }

        public string DEPARTMENT_NAME { get; set; }
    }

    public class GetShiftNames
    {
        public long SHIFT_ID { get; set; }

        public string shift_name { get; set; }
    }
    #endregion
}
