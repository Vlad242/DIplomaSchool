using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiplomaSchool.Student
{
    public partial class NewService : Form
    {
        private bool Flag;
        private int Id;
        public MySqlConnection conn;

        private List<int> Service_id = new List<int>();
        private List<int> Prices = new List<int>();

        public NewService(bool flag, int id)
        {
            InitializeComponent();
            Flag = flag;
            Id = id;
            DataBase.DataBaseInfo dataBase = new DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();

            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
        }

        private void NewService_Load(object sender, EventArgs e)
        {
            textBox2.Text = DateTime.Now.ToString("yyy:MM:dd");

            string searchParam = "";
            if (Flag)
            {
                searchParam = "Service_id = 1 or Service_id = 2";
            }
            else
            {
                searchParam = "Service_id != 1 and Service_id != 2";
            }

            try
            {
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Service_id, Service_name, Service_price FROM Services WHERE " + searchParam  +";")
                };
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Service_id.Add(reader.GetInt32(0));
                    comboBox1.Items.Add(reader.GetString(1));
                    Prices.Add(reader.GetInt32(2));
                }
                reader.Close();
            }
            catch (Exception)
            {

            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = Prices[comboBox1.SelectedIndex].ToString();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text != string.Empty && richTextBox1.Text != string.Empty)
                {
                    string sql = "Insert into Orders (Order_id, User_id, " +
                       "Login, Service_id, Status_id, Order_date, Descriptions) values (null," +
                       Id + ", null ," + Service_id[comboBox1.SelectedIndex] + "," +
                       1 + ",'" + textBox2.Text + "','" + richTextBox1.Text + "');";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Orders added!");
                    Send();
                    StudentRoom room = new StudentRoom(Id);
                    room.Show();
                    conn.Close();
                    this.Dispose();
                }
                else
                {
                    MessageBox.Show("All fields are not fully!");
                }
            }
            catch (Exception)
            {

            }
        }

        private void Send()
        {
            //////////////////get_orderID
            int orderId = 0;
            MySqlCommand cmd1 = new MySqlCommand
            {
                Connection = conn,
                CommandText = string.Format("SELECT MAX(Order_id) FROM Orders;")
            };
            MySqlDataReader reader = cmd1.ExecuteReader();
            while (reader.Read())
            {
                orderId = reader.GetInt32(0);
            }
            reader.Close();
            /////////////////////USer Email
            string userMail = "";
            string Login = "";
            cmd1 = new MySqlCommand
            {
                Connection = conn,
                CommandText = string.Format("SELECT Email, Login FROM Users WHERE User_id='" + Id + "';")
            };
            reader = cmd1.ExecuteReader();
            while (reader.Read())
            {
                userMail = reader.GetString(0);
                Login = reader.GetString(1);
            }
            reader.Close();

            Mailer.Generator generator = new Mailer.Generator();
            string body = generator.GenerateBody(Login, orderId, "LINGVO");
            string subject = generator.GenerateSubject("LINGVO", orderId);
            Mailer.Mailer mailer = new Mailer.Mailer();
            mailer.SendMail(userMail, "example@gmail.com", "", subject, body);
        }

        private void NewService_FormClosing(object sender, FormClosingEventArgs e)
        {
            StudentRoom room = new StudentRoom(Id);
            room.Show();
            conn.Close();
            this.Dispose();
        }
    }
}
