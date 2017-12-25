using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DiplomaSchool.Teacher
{
    public partial class SetOrderStatus : Form
    {
        public MySqlConnection conn;
        private int Id;
        private int Order_Id;

        List<int> status_id = new List<int>();

        public SetOrderStatus(int id, int order)
        {
            InitializeComponent();
            Order_Id = order;
            Id = id;

            DataBase.DataBaseInfo dataBase = new DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();
        }

        private void SetOrderStatus_Load(object sender, EventArgs e)
        {
            try
            {
                MySqlCommand cmd1 = new MySqlCommand();
                cmd1.Connection = conn;
                //////////////////////////
                cmd1.CommandText = string.Format("SELECT Status_id, Status_name FROM Status;");
                MySqlDataReader reader2 = cmd1.ExecuteReader();
                while (reader2.Read())
                {
                    status_id.Add(reader2.GetInt32(0));
                    comboBox1.Items.Add(reader2.GetString(1));
                }
                reader2.Close();
            }
            catch (Exception ex)
            {

            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "UPDATE Orders SET Status_id=" + status_id[comboBox1.SelectedIndex] + " WHERE Order_id= " + Order_Id + ";";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Status changed to " + comboBox1.SelectedItem);

                TeacherRoom teacherRoom = new TeacherRoom(Id);
                teacherRoom.Show();
                Send();
                conn.Close();
                Dispose();
            }
            catch (Exception)
            {
            }
           
        }

        private void Send()
        {
            /////////////////////USer Email
            string userMail = "";
            string Login = "";
            MySqlCommand cmd1 = new MySqlCommand
            {
                Connection = conn,
                CommandText = string.Format("SELECT Email, Login FROM Users WHERE User_id='" + Id + "';")
            };
            MySqlDataReader reader = cmd1.ExecuteReader();
            while (reader.Read())
            {
                userMail = reader.GetString(0);
                Login = reader.GetString(1);
            }
            reader.Close();

            Mailer.Generator generator = new Mailer.Generator();
            string body = generator.GenerateCompleteBody(Login, Order_Id, "LINGVO");
            string subject = generator.GenerateSubject("LINGVO", Order_Id);
            Mailer.Mailer mailer = new Mailer.Mailer();
            mailer.SendMail(userMail, "example@gmail.com", "", subject, body);
        }
    }
}
