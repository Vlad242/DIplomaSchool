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
                    comboBox1.Items.Add(reader.GetString(0));
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

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            ConfirmLogin confirm = new ConfirmLogin();
            confirm.Show();
            this.Hide();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != string.Empty && textBox2.Text != string.Empty)
            {
                string login = comboBox1.Text;
                string password = textBox2.Text;

                bool flag = false;
                foreach (string item in comboBox1.Items)
                {
                    if (login == item)
                    {
                        flag = true;
                        break;
                    }
                }

                if (flag)
                {
                    if (SearchPass(login, password))
                    {
                        int ID = 0;
                        int Type = 0;
                        MySqlCommand cmd2 = new MySqlCommand
                        {
                            Connection = conn,
                            CommandText = string.Format("Select User_id, UType_id FROM Users Where Login = '" + login +"';")
                        };
                        MySqlDataReader reader = cmd2.ExecuteReader();
                        while (reader.Read())
                        {
                            ID = reader.GetInt32(0);
                            Type = reader.GetInt32(1);
                        }
                        reader.Close();
                        ///////////////REDIRECT
                        MessageBox.Show("Welcome!");
                        switch (Type)
                        {
                            case 1:
                                {
                                    Admin.AdminRoom admin = new Admin.AdminRoom(ID);
                                    admin.Show();
                                    conn.Close();
                                    Hide();
                                    break;
                                }
                            case 2:
                                {
                                    Teacher.TeacherRoom teacher = new Teacher.TeacherRoom(ID);
                                    teacher.Show();
                                    conn.Close();
                                    Hide();
                                    break;
                                }
                            case 3:
                                {
                                    User.UserRoom user = new User.UserRoom(ID);
                                    user.Show();
                                    conn.Close();
                                    Hide();
                                    break;
                                }
                            case 4:
                                {
                                    Student.StudentRoom student = new Student.StudentRoom(ID);
                                    student.Show();
                                    conn.Close();
                                    Hide();
                                    break;
                                }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error logging, check the correct data entry!");
                    }
                }
                else
                {
                    MessageBox.Show("This user is not registered on the system!");
                }
            }
            else
            {
                MessageBox.Show("Fill in all the logon fields!");
            }
        }

        public bool SearchPass(string Login, string password)
        {
            DataBase.PasswordHash hash = new DataBase.PasswordHash();
            string pass = hash.GetHashSha256(password);
            int result = 0;

            MySqlCommand cmd2 = new MySqlCommand
            {
                Connection = conn,
                CommandText = string.Format("Select User_id FROM Users Where Login = '" + Login + "' and password = '" + pass + "';")
            };
            MySqlDataReader reader = cmd2.ExecuteReader();
            while (reader.Read())
            {
                result = reader.GetInt32(0);
            }
            reader.Close();

            if (result != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
