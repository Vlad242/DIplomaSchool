using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace DiplomaSchool.Login
{
    public partial class Answer : Form
    {
        private string Login;
        public MySqlConnection conn;
        private int attempt = 3;
        private string answer;

        public Answer(string login)
        {
            InitializeComponent();
            Login = login;
            DataBase.DataBaseInfo dataBase = new DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();
        }

        private void Answer_Load(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            groupBox2.Visible = true;
            try
            {
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Secret, Answer FROM Users WHERE Login='" + Login + "';")
                };
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    richTextBox1.Text = reader.GetString(0);
                    answer = reader.GetString(1);
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox1.Text != " ")
            {
                if (textBox1.Text == answer)
                {
                    groupBox1.Visible = true;
                    groupBox2.Visible = false;
                }
                else
                {
                    if (attempt < 1)
                    {
                        LogIn logIn = new LogIn();
                        logIn.Show();
                        this.Dispose();
                    }
                    else
                    {
                        attempt = --attempt;
                        MessageBox.Show("The answer is false, there are " + attempt + " more attempts!");
                    }
                }
            }
            else
            {
                MessageBox.Show("The answer field is empty, fill it out!");
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (label4.ForeColor == Color.Green && label7.ForeColor == Color.Green)
            {
                try
                {
                    string password = GetHashSha256(textBox3.Text);
                    string sql = "UPDATE Users SET Password= '" + password + "' WHERE Login= '" + Login + "';";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Password changed!");
                    conn.Close();
                    LogIn login = new LogIn();
                    login.Show();
                    this.Dispose();
                }
                catch (Exception)
                {
                    MessageBox.Show("Password rewrite error");
                }
            }
            else
            {
                MessageBox.Show("Fill in the fields correctly!");
            }
        }

        private void TextBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text == textBox2.Text)
            {
                label4.Text = "Passwords match!";
                label4.ForeColor = Color.Green;
            }
            else
            {
                label4.Text = "Passwords do not match!";
                label4.ForeColor = Color.Red;
            }
        }

        public static string GetHashSha256(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }
    
        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length < 8)
            {
                label7.Text = "Password length is less than 8!";
                label7.ForeColor = Color.Red;
            }
            else
            {
                label7.Text = "Password length is acceptable!";
                label7.ForeColor = Color.Green;
            }
        }

        private void Answer_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
            LogIn login = new LogIn();
            login.Show();
            this.Dispose();
        }
    }
}
