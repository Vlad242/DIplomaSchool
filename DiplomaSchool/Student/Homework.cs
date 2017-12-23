using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace DiplomaSchool.Student
{
    public partial class Homework : Form
    {
        private int Id;
        private int Student_id;
        private int group;
        public MySqlConnection conn;
        public string Search = "";

        private List<string> URL_name = new List<string>();
        private List<string> URL = new List<string>();

        public Homework(int id, int student)
        {
            InitializeComponent();
            Id = student;
            Student_id = id;
            DataBase.DataBaseInfo dataBase = new DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();

            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyy:MM:dd";
            dateTimePicker1.Visible = false;
            dateTimePicker1.Enabled = false;
        }

        private void Homework_Load(object sender, EventArgs e)
        {
            try
            {
                MySqlCommand cmd1 = new MySqlCommand
                {
                    Connection = conn,
                    //////////////////////////
                    CommandText = string.Format("SELECT Groups.Group_name, Groups.Group_id FROM Groups INNER JOIN Students on(Students.Group_id=Groups.Group_id) WHERE Students.Student_id = '" + Id + "';")
                };
                MySqlDataReader reader2 = cmd1.ExecuteReader();
                while (reader2.Read())
                {
                    this.Text = "Attached files for group students " + reader2.GetString(0);
                    group = reader2.GetInt32(1);
                }
                reader2.Close();

                MySqlCommand cmd2 = new MySqlCommand
                {
                    Connection = conn,
                    //////////////////////////
                    CommandText = string.Format("Select URL.URL_name, URL.Link FROM URL inner join Shedule on (Shedule.SDate = URL.SDate) Where Shedule.Group_id = '" + group + "';")
                };
                MySqlDataReader reader = cmd2.ExecuteReader();
                while (reader.Read())
                {

                    URL_name.Add(reader.GetString(0));
                    URL.Add(reader.GetString(1));
                }
                reader.Close();

                button3.PerformClick();
            }
            catch (Exception)
            {

            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Search = "";
            dataGridView1.Columns.Clear();
            MySqlDataAdapter mda = new MySqlDataAdapter("SELECT DISTINCT Subject.Sub_name, Lessons_type.Type_name ,Teachers.T_Surname, URL.SDate, URL.URL_name FROM URL INNER JOIN Shedule on(Shedule.Teacher_id = URL.Teacher_id)INNER JOIN Lessons_type on(Lessons_type.Type_id= URL.Type_id) INNER JOIN Teachers on(Teachers.Teacher_id= URL.Teacher_id) INNER JOIN Subject on(Subject.Subject_id= URL.Subject_id)WHERE Shedule.Group_id ='" + group + "';", conn);
            DataSet ds = new DataSet();
            mda.Fill(ds, "Marks");
            dataGridView1.DataSource = ds.Tables["Marks"];
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                dataGridView1.Rows[i].Cells[0].ReadOnly = true;
            /////columns names
            dataGridView1.Columns[0].HeaderText = "Subject name";
            dataGridView1.Columns[1].HeaderText = "Lesson type";
            dataGridView1.Columns[2].HeaderText = "Teacher surname";
            dataGridView1.Columns[3].HeaderText = "Date";
            dataGridView1.Columns[4].HeaderText = "Document title";
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            ReSearch();
            dataGridView1.Columns.Clear();
            MySqlDataAdapter mda = new MySqlDataAdapter("SELECT DISTINCT Subject.Sub_name, Lessons_type.Type_name ,Teachers.T_Surname, URL.SDate, URL.URL_name FROM URL INNER JOIN Shedule on(Shedule.Teacher_id = URL.Teacher_id)INNER JOIN Lessons_type on(Lessons_type.Type_id= URL.Type_id) INNER JOIN Teachers on(Teachers.Teacher_id= URL.Teacher_id) INNER JOIN Subject on(Subject.Subject_id= URL.Subject_id)WHERE Shedule.Group_id ='" + group + "'" + Search + ";", conn);
            DataSet ds = new DataSet();
            mda.Fill(ds, "Subject");
            dataGridView1.DataSource = ds.Tables["Subject"];
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                dataGridView1.Rows[i].Cells[0].ReadOnly = true;
            /////columns names
            dataGridView1.Columns[0].HeaderText = "Subject name";
            dataGridView1.Columns[1].HeaderText = "Lesson type";
            dataGridView1.Columns[2].HeaderText = "Teacher surname";
            dataGridView1.Columns[3].HeaderText = "Date";
            dataGridView1.Columns[4].HeaderText = "Document title";
        }

        public void ReSearch()
        {
            if (comboBox1.Text != "" && textBox2.Text != "")
            {
                string operation = "";
                switch (comboBox2.Text)
                {
                    case ">":
                        operation = ">";
                        break;
                    case "<":
                        operation = "<";
                        break;
                    case "=":
                        operation = "=";
                        break;
                    case ">=":
                        operation = ">=";
                        break;
                    case "<=":
                        operation = "<=";
                        break;
                    case "!=":
                        operation = "!=";
                        break;
                    default:
                        operation = " LIKE ";
                        break;
                }
                switch (comboBox1.Text)
                {
                    case "Subject name":
                        {
                            Search += "and Subject.Sub_name" + operation + "'" + textBox2.Text + "' ";
                            break;
                        }
                    case "Lesson type":
                        {
                            Search += "and Lessons_type.Type_name" + operation + "'" + textBox2.Text + "' ";
                            break;
                        }
                    case "Teacher surname":
                        {
                            Search += "and Teachers.T_Surname" + operation + "'" + textBox2.Text + "' ";
                            break;
                        }
                    case "Date":
                        {
                            Search += "and Shedule.SDate" + operation + "'" + textBox2.Text + "' ";
                            break;
                        }
                    case "Document title":
                        {
                            Search += "and URL.URL_name" + operation + "'" + textBox2.Text + "' ";
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            textBox2.Text = dateTimePicker1.Text;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string path = "";
            FolderBrowserDialog fd = new FolderBrowserDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                path = fd.SelectedPath;
            }

            bool flag = false;
            try
            {
                for (int i = 0; i < URL_name.Count; i++)
                {
                    if (URL_name[i] == textBox1.Text)
                    {
                        path = path + "\\" + URL[i].Substring(URL[i].LastIndexOf('*') + 1);
                        File.Copy(URL[i].Replace('*', '\\'), path, false);
                        flag = true;
                        break;
                    }
                }

                if (flag == true)
                {
                    MessageBox.Show("File copied!");
                }
                else
                {
                    MessageBox.Show("File not copied!");
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Problems with downloading files!");
            }

        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "Date")
            {
                dateTimePicker1.Visible = true;
                dateTimePicker1.Enabled = true;
            }
            if (comboBox1.SelectedItem.ToString() != "Date")
            {
                dateTimePicker1.Visible = false;
                dateTimePicker1.Enabled = false;
            }
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[4].Value.ToString();
        }

        private void Homework_FormClosing(object sender, FormClosingEventArgs e)
        {
            StudentRoom room = new StudentRoom(Student_id);
            room.Show();
            conn.Close();
            this.Dispose();
        }
    }
}
