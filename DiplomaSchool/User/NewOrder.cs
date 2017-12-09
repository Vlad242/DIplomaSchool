using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DiplomaSchool.User
{
    public partial class NewOrder : Form
    {
        private int Id;
        public MySqlConnection conn;

        private List<int> Service_id = new List<int>();
        private List<int> Prices = new List<int>();

        private List<int> Group_id = new List<int>();

        public NewOrder(int id)
        {
            InitializeComponent();
            Id = id;
            DataBase.DataBaseInfo dataBase = new DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy:MM:dd";
        }

        private void NewOrder_Load(object sender, EventArgs e)
        {
            textBox2.Text = DateTime.Now.ToString("yyy:MM:dd");
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            button2.Enabled = false;
            this.Size = new Size(359, 342);

            try
            {
                Service_id.Clear();
                comboBox1.Items.Clear();
                Prices.Clear();

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Service_id, Service_name, Service_price FROM Services;")
                };
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Service_id.Add(reader.GetInt32(0));
                    comboBox1.Items.Add(reader.GetString(1));
                    Prices.Add(reader.GetInt32(2));
                }
                reader.Close();

                /////////////groups
                cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Group_id, Group_name FROM Groups WHERE Status = 1;")
                };
                 reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Group_id.Add(reader.GetInt32(0));
                    comboBox3.Items.Add(reader.GetString(1));
                }
                reader.Close();
            }
            catch (Exception)
            {

            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.Text == "Foreign language courses" || comboBox1.Text == "Distance Learning")
            {
                this.Size = new Size(687, 342);
            }
            else
            {
                this.Size = new Size(359, 342);
            }

            textBox1.Text = Prices[comboBox1.SelectedIndex].ToString();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Foreign language courses" || comboBox1.Text == "Distance Learning")
            {
                if (comboBox1.Text != string.Empty && richTextBox1.Text != string.Empty &&
                       textBox3.Text != string.Empty && textBox4.Text != string.Empty &&
                        textBox5.Text != string.Empty && dateTimePicker1.Text != string.Empty &&
                         comboBox2.Text != string.Empty && textBox6.Text != string.Empty && comboBox3.Text != string.Empty)
                {

                    button2.Enabled = true;
                    button1.Enabled = false;

                    string sql = "Insert into Orders (Order_id, User_id, " +
                       "Login, Service_id, Status_id, Order_date, Descriptions) values (null," +
                       Id + ", null ," + Service_id[comboBox1.SelectedIndex] + "," +
                       1 + ",'" + textBox2.Text + "','" + richTextBox1.Text + "');";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Please set user info and press confirm!");
                }
                else
                {
                    MessageBox.Show("All fields are not fully!");
                }
                
            }
            else
            {
                if (comboBox1.Text != string.Empty && richTextBox1.Text != string.Empty)
                {
                    //User
                    string sql = "Insert into Orders (Order_id, User_id, Login, Service_id, Status_id, Order_date, Descriptions) values (null," +
                       Id + ", null ," + Service_id[comboBox1.SelectedIndex]+ "," +
                       1 + ",'" + textBox2.Text + "','" + richTextBox1.Text + "');";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Order added!");

                    UserRoom room = new UserRoom(Id);
                    room.Show();
                    conn.Close();
                    this.Dispose();
                }
            }
        }

        private void TextBox6_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox6.Text, "[^0-9]"))
            {
                textBox6.Text = textBox6.Text.Remove(textBox6.Text.Length - 1);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text != string.Empty && richTextBox1.Text != string.Empty &&
                    textBox3.Text != string.Empty && textBox4.Text != string.Empty &&
                     textBox5.Text != string.Empty && dateTimePicker1.Text != string.Empty &&
                      comboBox2.Text != string.Empty && textBox6.Text != string.Empty && comboBox3.Text != string.Empty)
                   {
                    ////////add student
                    string sql = "Insert into Students (Student_id, Group_id, User_id, Login, S_Name, S_Surname, S_Fname, S_Bdate, S_Gender, S_Phone) values (null," +
                             Group_id[comboBox3.SelectedIndex] +"," + Id + ", null ,'" +
                              textBox3.Text + "','" + textBox4.Text + "','" +  textBox5.Text + "','" + 
                              dateTimePicker1.Text + "','" + comboBox2.Text + "'," + textBox6.Text + ");";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();

                    ////update index
                    sql = "UPDATE Users SET UType_id=4;";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Order added!User update to type STUDENT, please login newly!");

                    Login.LogIn logIn = new Login.LogIn();
                    logIn.Show();
                    conn.Close();
                    this.Dispose();
                }
                else
                {
                    MessageBox.Show("All fields are not fully!");
                }
            }
            catch (Exception ex)
            {

            }
          
        }
    }
}
