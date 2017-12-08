using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DiplomaSchool.Login
{
    public partial class UserRegister : Form
    {
        private string ConfirmPasswordGlobal = "Superadmin";
        private bool ConfirmedAdmin = false;
        private bool ConfirmedLogin = false;
        private bool ConfirmedPassword = false;
        private bool ConfirmedEmail = false;
        private int UserType = 5;
        public MySqlConnection conn;

        public List<int> Groups = new List<int>();

        public UserRegister()
        {
            InitializeComponent();
            DataBase.DataBaseInfo dataBase = new DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy:MM:dd";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "yyyy:MM:dd";
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox14.Clear();
            textBox15.Clear();
            richTextBox1.Clear();

            ConfirmedAdmin = false;
            ConfirmedLogin = false;
            ConfirmedPassword = false;
            ConfirmedEmail = false;

            label1.Text = "#";
            label14.Text = "#";
            label6.Text = "#";
            label7.Text = "#";
            label10.Text = "#";
            label25.Text = "#";

            label1.ForeColor = Color.Black;
            label6.ForeColor = Color.Black;
            label4.ForeColor = Color.Black;
            label7.ForeColor = Color.Black;
            label10.ForeColor = Color.Black;
            label25.ForeColor = Color.Black;

            switch (comboBox1.Text)
            {
                case "Admin":
                    {
                        StudentBox.Visible = false;
                        TeacherBox.Visible = false;

                        if (!ConfirmedAdmin)
                        {
                            button1.Enabled = false;
                            textBox8.Visible = true;
                            label14.Visible = true;
                            label14.Text = "NOT CONFIRM";
                            label14.ForeColor = Color.Red;
                            UserType = 1;
                        }
                        break;
                    }
                case "User":
                    {
                        StudentBox.Visible = false;
                        TeacherBox.Visible = false;
                        ConfirmedAdmin = false;
                        UserType = 3;
                        break;
                    }
                case "Teacher":
                    {
                        StudentBox.Visible = false;
                        TeacherBox.Visible = true;

                        UserType = 2;
                        ConfirmedAdmin = false;
                        break;
                    }
                case "Student":
                    {
                        StudentBox.Visible = true;
                        TeacherBox.Visible = false;

                        UserType = 4;
                        ConfirmedAdmin = false;
                        break;
                    }
                default:
                    {
                        UserType = 5;
                        break;
                    }
            }
        }

        private void UserRegister_Load(object sender, EventArgs e)
        {
            StudentBox.Visible = false;
            TeacherBox.Visible = false;
            textBox8.Visible = false;
            Groups.Clear();
            comboBox3.Items.Clear();

            try
            {
                ////////////Users
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Group_id, Group_name FROM Groups;")
                };
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox3.Items.Add(reader.GetString(1));
                    Groups.Add(reader.GetInt32(0));
                }
                reader.Close();
            }
            catch (Exception)
            {

            }
        }

        private void TextBox8_TextChanged(object sender, EventArgs e)
        {
            if (textBox8.Text != "")
            {
                if (textBox8.Text == ConfirmPasswordGlobal)
                {
                    button1.Enabled = true;
                    label14.Visible = true;
                    label14.Text = "CONFIRMED!!!";
                    label14.ForeColor = Color.Green;
                    textBox8.Enabled = false;
                    textBox8.Visible = false;
                    ConfirmedAdmin = true;
                    textBox8.Clear();
                    ConfirmedAdmin = true;
                }
                else
                {
                    button1.Enabled = false;
                    textBox8.Visible = true;
                    label14.Visible = true;
                    label14.Text = "NOT CONFIRM";
                    label14.ForeColor = Color.Red;
                    ConfirmedAdmin = false;
                }
            }
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length < 8)
            {
                label1.Text = "To short";
                label1.ForeColor = Color.Red;
            }
            else
            {
                label1.Text = "OK";
                label1.ForeColor = Color.Green;
            }
        }

        private void TextBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length >= 8 && textBox2.Text == textBox4.Text)
            {
                label6.Text = "Does not match";
                label6.ForeColor = Color.Red;
                ConfirmedPassword = false;
            }
            else
            {
                label6.Text = "Confirmed";
                label6.ForeColor = Color.Green;
                ConfirmedPassword = true;
            }
        }

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length <= 101)
            {
                label7.Text = "Confirmed";
                label7.ForeColor = Color.Green;
            }
            else
            {
                richTextBox1.Text = richTextBox1.Text.Remove(richTextBox1.Text.Length-1, 1);
                label7.Text = "To long";
                label7.ForeColor = Color.Red;
            }
        }

        private void TextBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox7.Text.Length <= 51)
            {
                label10.Text = "Confirmed";
                label10.ForeColor = Color.Green;
            }
            else
            {
                textBox7.Text = textBox7.Text.Remove(textBox7.Text.Length - 1, 1);
                label10.Text = "To long";
                label10.ForeColor = Color.Red;
            }
        }

        private void TextBox15_TextChanged(object sender, EventArgs e)
        {
            if (textBox15.Text.Contains('@'))
            {
                label25.Text = "Acceptable";
                label25.ForeColor = Color.Green;
                ConfirmedEmail = true;
            }
            else
            {
                label25.Text = "Not acceptable";
                label25.ForeColor = Color.Red;
                ConfirmedEmail = false;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            switch (UserType)
            {
                case 5://////Null
                    {
                        MessageBox.Show("PLEASE SELECT USER TYPE!");
                        break;
                    }
                case 1://////ADMIN
                    {
                        if (ConfirmedAdmin)
                        {
                            if (ConfirmedLogin)
                            {
                                if (ConfirmedPassword)
                                {
                                    if (ConfirmedEmail)
                                    {
                                        if (textBox1.Text != string.Empty &&
                                            textBox2.Text != string.Empty &&
                                            textBox4.Text != string.Empty &&
                                            richTextBox1.Text != string.Empty &&
                                            textBox7.Text != string.Empty &&
                                            textBox15.Text != string.Empty)
                                        {
                                            try
                                            {
                                                DataBase.PasswordHash password = new DataBase.PasswordHash();
                                                string pass = password.GetHashSha256(textBox4.Text);
                                                /////////////////////Password

                                                string sql = "insert into Users(User_id, Login, UType_id," +
                                                    " Password, Secret, Answer, Email) values (null, '" +
                                                      textBox1.Text + "', " + UserType + ", '"
                                                    + pass + "', '" + richTextBox1.Text + "', '"
                                                    + textBox7.Text + "', '" + textBox15.Text + "')";
                                                MySqlCommand cmd = new MySqlCommand(sql, conn);
                                                cmd.ExecuteNonQuery();
                                                MessageBox.Show("Registration was successful!");
                                                this.Close();
                                            }
                                            catch (Exception)
                                            {
                                                MessageBox.Show("Something wrong!");
                                            }
                                        }
                                    else
                                    {
                                        MessageBox.Show("Not all fields are full! Please check correct fields!");
                                    }
                                    }
                                    else
                                    {
                                        MessageBox.Show("EMAILINCORRECT!");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("PASSWORD INCORRECT!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("LOGIN INCORRECT!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("ADMIN NOT CONFIRMED SU PASSWORD!");
                        }
                        break;
                    }
                case 2://////TEACHER
                    {
                        if (ConfirmedLogin)
                        {
                            if (ConfirmedPassword)
                            {
                                if (ConfirmedEmail)
                                {
                                    if (textBox1.Text != string.Empty &&
                                        textBox2.Text != string.Empty &&
                                        textBox4.Text != string.Empty &&
                                        richTextBox1.Text != string.Empty &&
                                        textBox7.Text != string.Empty &&
                                        textBox15.Text != string.Empty &&

                                        textBox11.Text != string.Empty &&
                                        textBox10.Text != string.Empty &&
                                        textBox12.Text != string.Empty &&
                                        dateTimePicker2.Text != string.Empty &&
                                        comboBox4.Text != string.Empty &&
                                        textBox14.Text != string.Empty)
                                    {
                                        try
                                        {
                                            DataBase.PasswordHash password = new DataBase.PasswordHash();
                                            string pass = password.GetHashSha256(textBox4.Text);
                                            /////////////////////Password

                                            string sql = "insert into Users(User_id, Login, UType_id," +
                                                " Password, Secret, Answer, Email) values (null, '" +
                                                  textBox1.Text + "', " + UserType + ", '"
                                                + pass + "', '" + richTextBox1.Text + "', '"
                                                + textBox7.Text + "', '" + textBox15.Text + "')";
                                            MySqlCommand cmd = new MySqlCommand(sql, conn);
                                            cmd.ExecuteNonQuery();

                                            ////////////ID
                                            int id = 0;
                                            cmd = new MySqlCommand
                                            {
                                                Connection = conn,
                                                CommandText = string.Format("SELECT MAX(User_id) FROM Users;")
                                            };
                                            MySqlDataReader reader = cmd.ExecuteReader();
                                            while (reader.Read())
                                            {
                                                id = reader.GetInt32(0);
                                            }
                                            reader.Close();

                                            //////////////////ADD TEACHER
                                            sql = "insert into Teachers(Teacher_id, User_id, Login," +
                                              " T_Name, T_Surname, T_Fname, T_Bdate, T_Gender, Academic_status) values (null, " +
                                               id + ", " + null + ", '"
                                              + textBox11.Text + "', '" + textBox10.Text + "', '" +
                                              textBox12.Text + "', '" + dateTimePicker2.Text + "', '"
                                              + comboBox4.Text + "', '" + textBox14.Text + "')";
                                            cmd = new MySqlCommand(sql, conn);
                                            cmd.ExecuteNonQuery();

                                            ///////////////////
                                            MessageBox.Show("Registration was successful!");
                                            this.Close();
                                        }
                                        catch (Exception)
                                        {
                                            MessageBox.Show("Something wrong!");
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Not all fields are full! Please check correct fields!");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("EMAILINCORRECT!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("PASSWORD INCORRECT!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("LOGIN INCORRECT!");
                        }
                        break;
                    }
                case 3://////USER
                    {
                        if (ConfirmedLogin)
                        {
                            if (ConfirmedPassword)
                            {
                                if (ConfirmedEmail)
                                {
                                    if (textBox1.Text != string.Empty &&
                                        textBox2.Text != string.Empty &&
                                        textBox4.Text != string.Empty &&
                                        richTextBox1.Text != string.Empty &&
                                        textBox7.Text != string.Empty &&
                                        textBox15.Text != string.Empty)
                                    {
                                        try
                                        {
                                            DataBase.PasswordHash password = new DataBase.PasswordHash();
                                            string pass = password.GetHashSha256(textBox4.Text);
                                            /////////////////////Password

                                            string sql = "insert into Users(User_id, Login, UType_id," +
                                                " Password, Secret, Answer, Email) values (null, '" +
                                                  textBox1.Text + "', " + UserType + ", '"
                                                + pass + "', '" + richTextBox1.Text + "', '"
                                                + textBox7.Text + "', '" + textBox15.Text + "')";
                                            MySqlCommand cmd = new MySqlCommand(sql, conn);
                                            cmd.ExecuteNonQuery();
                                            MessageBox.Show("Registration was successful!");
                                            this.Close();
                                        }
                                        catch (Exception)
                                        {
                                            MessageBox.Show("Something wrong!");
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Not all fields are full! Please check correct fields!");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("EMAILINCORRECT!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("PASSWORD INCORRECT!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("LOGIN INCORRECT!");
                        }
                        break;
                    }
                case 4://////STUDENT
                    {
                        if (ConfirmedLogin)
                        {
                            if (ConfirmedPassword)
                            {
                                if (ConfirmedEmail)
                                {
                                    if (textBox1.Text != string.Empty &&
                                        textBox2.Text != string.Empty &&
                                        textBox4.Text != string.Empty &&
                                        richTextBox1.Text != string.Empty &&
                                        textBox7.Text != string.Empty &&
                                        textBox15.Text != string.Empty &&

                                        textBox5.Text != string.Empty &&
                                        textBox3.Text != string.Empty &&
                                        textBox6.Text != string.Empty &&
                                        dateTimePicker1.Text != string.Empty &&
                                        comboBox2.Text != string.Empty &&
                                        comboBox3.Text != string.Empty &&
                                        textBox9.Text != string.Empty)
                                    {
                                        try
                                        {
                                            DataBase.PasswordHash password = new DataBase.PasswordHash();
                                            string pass = password.GetHashSha256(textBox4.Text);
                                            /////////////////////Password

                                            string sql = "insert into Users(User_id, Login, UType_id," +
                                                " Password, Secret, Answer, Email) values (null, '" +
                                                  textBox1.Text + "', " + UserType + ", '"
                                                + pass + "', '" + richTextBox1.Text + "', '"
                                                + textBox7.Text + "', '" + textBox15.Text + "')";
                                            MySqlCommand cmd = new MySqlCommand(sql, conn);
                                            cmd.ExecuteNonQuery();

                                            ////////////ID
                                            int id = 0;
                                            cmd = new MySqlCommand
                                            {
                                                Connection = conn,
                                                CommandText = string.Format("SELECT MAX(User_id) FROM Users;")
                                            };
                                            MySqlDataReader reader = cmd.ExecuteReader();
                                            while (reader.Read())
                                            {
                                                id = reader.GetInt32(0);
                                            }
                                            reader.Close();

                                            //////////////////ADD TEACHER
                                            sql = "insert into Students(Student_id, Group_id, User_id," +
                                              " Login, S_Name, S_Surname, S_Fname, S_Gender, S_Phone) values (null, " +
                                               Groups[comboBox3.SelectedIndex] + ", " + id + ", " + null + ", '"
                                              + textBox5.Text + "', '" + textBox3.Text + "', '" +
                                              textBox6.Text + "', '" + dateTimePicker1.Text + "', '"
                                              + comboBox2.Text + "', '" + textBox9.Text + "')";
                                            cmd = new MySqlCommand(sql, conn);
                                            cmd.ExecuteNonQuery();

                                            ///////////////////
                                            MessageBox.Show("Registration was successful!");
                                            this.Close();
                                        }
                                        catch (Exception)
                                        {
                                            MessageBox.Show("Something wrong!");
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Not all fields are full! Please check correct fields!");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("EMAILINCORRECT!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("PASSWORD INCORRECT!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("LOGIN INCORRECT!");
                        }
                        break;
                    }
            }
        }

        private void TextBox9_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox9.Text, "[^0-9]"))
            {
                textBox9.Text = textBox9.Text.Remove(textBox9.Text.Length - 1);
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ////////////Users
                int result = 0;
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT COUNT(User_id) FROM Users WHERE Login='" + textBox1.Text + "';")
                };
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result = reader.GetInt32(0);
                }
                reader.Close();

                if (result > 0)
                {
                    label27.Text = "Login exists!";
                    label27.ForeColor = Color.Red;
                    ConfirmedLogin = false;
                }
                else
                {
                    label27.Text = "Acceptable!";
                    label27.ForeColor = Color.Green;
                    ConfirmedLogin = true;
                }
            }
            catch (Exception)
            {

            }

        }

        private void UserRegister_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
            LogIn lg = new LogIn();
            lg.Show();
            this.Dispose();
        }
    }
}
