using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace DiplomaSchool.Login
{
    public partial class ConfirmLogin : Form
    {
        public MySqlConnection conn;

        public ConfirmLogin()
        {
            InitializeComponent();
            DataBase.DataBaseInfo dataBase = new DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox1.Text != " ")
            {
                try
                {
                    int res = 0;
                    MySqlCommand cmd = new MySqlCommand
                    {
                        Connection = conn,
                        CommandText = string.Format("SELECT COUNT(User_id) FROM Users WHERE Login='" + textBox1.Text + "';")
                    };
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        res = reader.GetInt32(0);
                    }
                    reader.Close();

                    if(res == 1)
                    {
                        Answer answer = new Answer(textBox1.Text);
                        answer.Show();
                        conn.Close();
                        this.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("Даний користвач не зареєстрований в системі!");
                    }
                }
                catch (Exception)
                {
                }
            }
            else
            {
                MessageBox.Show("Будь ласка, введіть логін!");
            }
        }
    }
}
