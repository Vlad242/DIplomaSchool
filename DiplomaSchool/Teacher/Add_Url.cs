using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DiplomaSchool.Teacher
{
    public partial class Add_Url : Form
    {
        public MySqlConnection conn;
        int Teacher = 0;
        int Subject = 0;
        int Type_id = 0;
        int Semester = 0;
        int Class_id = 0;
        string S_date = "";
        int Number = 0;
        string path = "";

        int out_id;

        public Add_Url(int teach, int sub, int typ, int sem, int clas, string dat, int num)
        {
            InitializeComponent();
            DataBase.DataBaseInfo dataBase = new DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();
            
            Teacher = teach;
            Subject = sub;
            Type_id = typ;
            Semester = sem;
            Class_id = clas;
            S_date = dat;
            Number = num;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                try
                {
                    CopyFile(@path, Directory.GetCurrentDirectory() + "\\Files\\" + textBox2.Text + textBox3.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка копіювання файлу!");
                }

                try
                {
                    string path = Directory.GetCurrentDirectory() + "\\Files\\" + textBox2.Text + textBox3.Text;
                    path = path.Replace('\\', '*');
                    string sql = "Insert into URL (Teacher_id, Subject_id, Type_id, Semester, Class_id, SDate, Num_lesson, URL_name, Link) values (" + Teacher + "," + Subject + "," + Type_id + "," + Semester + "," + Class_id + ",'" + S_date + "'," + Number + ",'" + textBox2.Text + "','" + path + "');";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("File " + textBox2.Text + " loaded!");

                    button1.Enabled = false;
                    button1.Visible = false;
                    button2.Enabled = true;
                    button2.Visible = true;

                }
                catch (Exception ex)
                {

                }
            }
        }

        private void Add_Url_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button2.Visible = false;
        }

        void CopyFile(string sourcefn, string destinfn)
        {
            FileInfo fn = new FileInfo(sourcefn);
            fn.CopyTo(destinfn, true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog s = new OpenFileDialog();

            if (s.ShowDialog() == DialogResult.OK)
            {
                path = s.FileName;
            }
            textBox1.Text = path;
            textBox3.Text = path.Substring(path.IndexOf('.'));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                //////////////////////////
                cmd.CommandText = string.Format("SELECT User_id FROM Teachers WHERE Teacher_id=" + Teacher + "; ");
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    out_id= reader.GetInt32(0);
                }
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            conn.Close();
            TeacherRoom t = new TeacherRoom(out_id);
            t.Show();
            this.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            bool flag = true;
            string str = "";
            char[] symbol = new char[] { Convert.ToChar(47), Convert.ToChar(92), Convert.ToChar(58), Convert.ToChar(42), Convert.ToChar(63), Convert.ToChar(34), Convert.ToChar(60), Convert.ToChar(62), Convert.ToChar(124) };
            foreach (var item in symbol)
            {
                str += item;
                if (textBox2.Text.Contains(Convert.ToString(item)))
                {
                    flag = false;
                }
            }

            if (!flag)
            {
                label3.Text = "The file name can not contain: " + str;
                label3.ForeColor = Color.Red;
            }
            else
            {
                label3.Text = "The file name is correct!";
                label3.ForeColor = Color.Green;
            }
        }
    }
}
