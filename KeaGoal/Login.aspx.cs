using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KeaGoal
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private const string V = "Server=localhost;Database=Users;Integrated Security=True;";

        // สร้าง connection string
        private string connectionString = V;

        // เมื่อผู้ใช้คลิกปุ่ม Login
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // ตรวจสอบการเข้าสู่ระบบ
            if (IsValidUser(username, password))
            {
                // Login สำเร็จ
                lblMessage.Text = "Login Successful!";
                lblMessage.ForeColor = System.Drawing.Color.Green;
                // Redirect
                Response.Redirect("Default.aspx");
            }
            else
            {
                // Login ล้มเหลว
                lblMessage.Text = "Invalid username or password.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        // ฟังก์ชันตรวจสอบผู้ใช้จากฐานข้อมูล
        private bool IsValidUser(string username, string password)
        {
            bool isValid = false;

            // เชื่อมต่อกับฐานข้อมูล
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // คำสั่ง SQL สำหรับตรวจสอบชื่อผู้ใช้และรหัสผ่าน
                    string query = "SELECT COUNT(1) FROM Users WHERE Username=@Username AND Password=@Password";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password); // หากใช้รหัสผ่านแบบเข้ารหัส ควรใช้การเข้ารหัสที่ปลอดภัย

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 1)
                    {
                        isValid = true; // ผู้ใช้ถูกต้อง
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }

            return isValid;
        }
    }
}