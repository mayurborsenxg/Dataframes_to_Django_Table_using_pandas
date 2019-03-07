using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace HostelErpSoftware.Classes
{
    public class DbClass
    {
        private readonly SqlConnection _cn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        public void Openconn()
        {
            try { _cn.Open(); }
            catch { _cn.Close(); _cn.Open(); }
        }
        public string base64Encode(string sData)
        {
            try
            {
                byte[] encData_byte = new byte[sData.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(sData);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        public string base64Decode(string sData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(sData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char); return result;
        }
        public string GenerateVerificationCode()
        {
            Random random = new Random();
            string[] array = new string[] { "0", "2", "3", "4", "5", "6", "8", "9" };
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int ij = 0; ij < 6; ij++)
            {
                int abc = random.Next(0, 8);
                sb.Append(array[abc]);
            }
            return sb.ToString();
        }
        public string getUniqueFileName(string filename)
        {
            try
            {
                string curdate = DateTime.Now.ToString("ddMMyyyy_HHmmss");
                string[] splitfile = filename.Split('.');
                string strNewName = "img_" + curdate + "." + splitfile[1];
                return strNewName;
            }
            catch (Exception)
            {
                return filename;
            }
        }
        public void sendAllEmails(string to, string subject, string body)
        {
            try
            {
                string emailID = ConfigurationManager.AppSettings["EmailID"].ToString();
                string emailPass = ConfigurationManager.AppSettings["EmailPass"].ToString();
                string emailHost = ConfigurationManager.AppSettings["EmailHost"].ToString();
                string emailPort = ConfigurationManager.AppSettings["EmailPort"].ToString();

                MailMessage ms = new MailMessage();
                ms.From = new MailAddress(emailID);
                ms.To.Add(to);
                ms.Subject = subject;

                ms.Body = body;

                SmtpClient Sc = new SmtpClient(emailHost);
                ms.IsBodyHtml = true;
                Sc.Port = Convert.ToInt32(emailPort); ;
                Sc.UseDefaultCredentials = false;

                Sc.Credentials = new NetworkCredential(emailID, emailPass);

                Sc.EnableSsl = true;

                Sc.Send(ms);



            }
            catch (Exception)
            {
                //ignored
            }
        }
        public void sendAdminForgetPasswordEmail(string pwd, string emailId, string first_name)
        {

            string sub = "Password Details at Hostel Managment";

            string MailBody = "";
            MailBody = "Hi " + first_name + ",<br><br>";
            MailBody = MailBody + "Your auto generated password for login is: " + pwd + "<br><br>";
            MailBody = MailBody + "You can change your password anytime from change password page. <br><br>";
            MailBody = MailBody + "Thank You!<br><br>";
            MailBody = MailBody + "Regards,<br>The Hostel Team";

            sendAllEmails(emailId, sub, MailBody);
        }
        public DataTable GetStudentLoginDetails(string email_id, string password)
        {
            DataTable dt = new DataTable();
            try
            {
                string pass = base64Encode(password);
                SqlCommand cmd = new SqlCommand("getStudentLoginDetails", _cn) 
                { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("email_id", email_id);
                cmd.Parameters.AddWithValue("password", pass);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                Openconn();
                da.Fill(dt);

            }
            catch (Exception)
            {
                // ignored
            }
            finally {

                _cn.Close();
            }
            return dt;
        }
        public DataTable checkIsReg(string MobileNo)
        {
            DataTable dt = new DataTable();
            try
            {
                
                SqlCommand cmd = new SqlCommand("checkIsReg", _cn) 
                { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("MobileNo", MobileNo);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                Openconn();
                da.Fill(dt);

            }
            catch (Exception)
            {
                // ignored
            }
            finally {

                _cn.Close();
            }
            return dt;
        }
        public int RegisterStudent(string FirstName, string LastName, string MobileNo, string EmailId, string Password)
        {
            int i = 0;
            try
            {

                SqlCommand cmd = new SqlCommand("RegisterStudent", _cn) { CommandType = CommandType.StoredProcedure };
                Openconn();
                cmd.Parameters.AddWithValue("FirstName", FirstName);
                cmd.Parameters.AddWithValue("LastName", LastName);
                cmd.Parameters.AddWithValue("MobileNo", MobileNo);
                cmd.Parameters.AddWithValue("EmailId", EmailId);
                cmd.Parameters.AddWithValue("Password", Password);
                
                i = cmd.ExecuteNonQuery();
                

            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {

                _cn.Close();
            }
            return i;
        }
        public DataTable getStudentDetailsByEmailId(string EmailId)
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "Select * from tbl_studRegistration where EmailId=@EmailId and status=1";

                SqlCommand cmd = new SqlCommand(query, _cn);
                cmd.Parameters.AddWithValue("EmailId", EmailId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                Openconn();
                da.Fill(dt);
                _cn.Close();

            }
            catch (Exception)
            {
                // ignored
            }
            return dt;
        }
        public int ResetStudentPassword(string regId, string Password)
        {
            int res = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("ResetStudentPassword", _cn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("regId", regId);
                cmd.Parameters.AddWithValue("Password", Password);
                Openconn();
                res = cmd.ExecuteNonQuery();
            }catch(Exception )
            {

            }
            finally
            {
                _cn.Close();
            }
           
            return res;
        }
        public int AddUpdateBuildingInformation(int bid, string BuildingName, int FloorCount, string Address, string City, string State, string ImagePath, int CreatedBy)
        {
            int i = 0;
            try
            {

                SqlCommand cmd = new SqlCommand("RegisterStudent", _cn) { CommandType = CommandType.StoredProcedure };
                Openconn();
                cmd.Parameters.AddWithValue("bid", bid);
                cmd.Parameters.AddWithValue("BuildingName", BuildingName);
                cmd.Parameters.AddWithValue("FloorCount", FloorCount);
                cmd.Parameters.AddWithValue("Address", Address);
                cmd.Parameters.AddWithValue("City", City);
                cmd.Parameters.AddWithValue("State", State);
                cmd.Parameters.AddWithValue("ImagePath", ImagePath);
                cmd.Parameters.AddWithValue("CreatedBy", CreatedBy);
                i = cmd.ExecuteNonQuery();


            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {

                _cn.Close();
            }
            return i;
        }
    }
}