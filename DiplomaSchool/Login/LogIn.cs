using DiplomaSchool.LogIn;
using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace DiplomaSchool.Login
{
    public partial class LogIn : Form
    {
        private InternetConection internetConection = new InternetConection();
        public MySqlConnection conn;

        public LogIn()
        {
            InitializeComponent();
            DataBase.DataBaseInfo dataBase = new DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();
        }

        private void LogIn_Load(object sender, EventArgs e)
        {
            //////////check internet
            switch (internetConection.GetInetStaus())
            {
                case "No access to the Internet":
                    {
                        label1.Text = "No access to the Internet";
                        label1.ForeColor = Color.Red;
                        pictureBox1.Image = new Bitmap(Directory.GetCurrentDirectory() + "\\DebugImage\\disconnect.png");
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        button2.Enabled = false;
                        MessageBox.Show("Please check internet connection and restart application!");
                        break;
                    }
                case "Limited access":
                    {
                        label1.Text = "Limited access";
                        label1.ForeColor = Color.Orange;
                        pictureBox1.Image = new Bitmap(Directory.GetCurrentDirectory() + "\\DebugImage\\disconnect.png");
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        button2.Enabled = false;
                        MessageBox.Show("Please check internet connection and restart application!");
                        break;
                    }
                case "Internet connection":
                    {
                        label1.Text = "Internet connection";
                        label1.ForeColor = Color.Green;
                        pictureBox1.Image = new Bitmap(Directory.GetCurrentDirectory() + "\\DebugImage\\connect.png");
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        break;
                    }
            }
            ////////////////Login from users
            try
            {
                comboBox1.Items.Clear();

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Login FROM Users;")
                };
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string res1 = reader.GetString(0);
                    comboBox1.Items.Add(res1);
                }
                reader.Close();
            }
            catch (Exception){}
        }

        private void PictureBox5_Click(object sender, EventArgs e)
        {
            conn.Close();
            Application.Exit();
        }

        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                conn.Close();
                UserRegister register = new UserRegister();
                register.Show();
                this.Hide();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ConfirmLogin confirm = new ConfirmLogin();
            confirm.Show();
            this.Hide();
        }
    }
}
