using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace DiplomaSchool.Teacher
{
    public partial class StartLesson : Form
    {
        public MySqlConnection conn;
        public string connStr;
        int Id = 0;
        public List<int> Sub_id = new List<int>();
        public List<int> Gro_id = new List<int>();
        public string Search = "";
        public string Login = "";

        public StartLesson(int id)
        {
            ControlBox = false;
            InitializeComponent();
            DataBase.DataBaseInfo dataBase = new DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();
            Id = id;

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyy:MM:dd";
            dateTimePicker1.Visible = false;
            dateTimePicker1.Enabled = false;
        }

        private void StartLesson_Load(object sender, EventArgs e)
        {
            try
            {
                MySqlCommand cmd1 = new MySqlCommand
                {
                    Connection = conn,
                    //////////////////////////
                    CommandText = string.Format("SELECT DISTINCT Subject.Subject_id, Subject.Sub_name FROM Subject INNER JOIN Workload on(Subject.Subject_id=Workload.Subject_id) INNER JOIN Teachers on (Workload.Teacher_id=Teachers.Teacher_id) WHERE Workload.Teacher_id=" + Id + ";")
                };
                MySqlDataReader reader2 = cmd1.ExecuteReader();
                while (reader2.Read())
                {
                    Sub_id.Add(reader2.GetInt32(0));
                    comboBox1.Items.Add(reader2.GetString(1));
                }
                reader2.Close();
            }
            catch (Exception)
            {

            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                comboBox2.Items.Clear();
                MySqlCommand cmd1 = new MySqlCommand
                {
                    Connection = conn,
                    //////////////////////////
                    CommandText = string.Format("SELECT DISTINCT Groups.Group_id, Groups.Group_name FROM Groups INNER JOIN Shedule on(Shedule.Group_id=Groups.Group_id) WHERE Shedule.Subject_id=" + Sub_id[comboBox1.SelectedIndex] + ";")
                };
                MySqlDataReader reader2 = cmd1.ExecuteReader();
                while (reader2.Read())
                {
                    Gro_id.Add(reader2.GetInt32(0));
                    comboBox2.Items.Add(reader2.GetString(1));
                }
                reader2.Close();
            }
            catch (Exception)
            {

            }
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.ClearSelection();
                MySqlDataAdapter mda = new MySqlDataAdapter("SELECT DISTINCT Lessons_type.Type_name, Shedule.Semester, Classroom.Audience, Shedule.Num_lesson, Shedule.SDate FROM Shedule INNER JOIN Workload on(Shedule.Teacher_id=Workload.Teacher_id) INNER JOIN Lessons_type on(Lessons_type.Type_id=Shedule.Type_id) INNER JOIN Classroom on(Shedule.Class_id=Classroom.Class_id) WHERE Shedule.Teacher_id= " + Id + " and Shedule.Group_id= " + Gro_id[comboBox2.SelectedIndex] + " and Shedule.Subject_id=" + Sub_id[comboBox1.SelectedIndex] + ";", conn);
                DataSet ds = new DataSet();
                mda.Fill(ds, "Shedule");
                dataGridView1.DataSource = ds.Tables["Shedule"];
                /////columns names
                dataGridView1.Columns[0].HeaderText = "Lesson type";
                dataGridView1.Columns[1].HeaderText = "Semester";
                dataGridView1.Columns[2].HeaderText = "Audience";
                dataGridView1.Columns[3].HeaderText = "Lesson number";
                dataGridView1.Columns[4].HeaderText = "Date";
            }
            catch (Exception)
            {
                MessageBox.Show("Something wrong!");
            }
        }

        private void ComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.SelectedItem.ToString() == "Date")
            {
                dateTimePicker1.Visible = true;
                dateTimePicker1.Enabled = true;
            }
            if (comboBox4.SelectedItem.ToString() != "Date")
            {
                dateTimePicker1.Visible = false;
                dateTimePicker1.Enabled = false;
            }
        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            textBox3.Text = dateTimePicker1.Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox4.Text != "" && textBox3.Text != "")
            {
                try
                {
                    ReSearch();
                    dataGridView1.ClearSelection();
                    MySqlDataAdapter mda = new MySqlDataAdapter("SELECT DISTINCT Lessons_type.Type_name, Shedule.Semester, Classroom.Audience, Shedule.Num_lesson, Shedule.SDate FROM Shedule INNER JOIN Workload on(Shedule.Teacher_id=Workload.Teacher_id) INNER JOIN Lessons_type on(Lessons_type.Type_id=Shedule.Type_id) INNER JOIN Classroom on(Shedule.Class_id=Classroom.Class_id) WHERE Shedule.Teacher_id= " + Id + " and Shedule.Group_id= " + Gro_id[comboBox2.SelectedIndex] + " and Shedule.Subject_id=" + Sub_id[comboBox1.SelectedIndex] + " " + Search + ";", conn);
                    DataSet ds = new DataSet();
                    mda.Fill(ds, "Shedule");
                    dataGridView1.DataSource = ds.Tables["Shedule"];
                    /////columns names
                    dataGridView1.Columns[0].HeaderText = "Lesson type";
                    dataGridView1.Columns[1].HeaderText = "Semester";
                    dataGridView1.Columns[2].HeaderText = "Audience";
                    dataGridView1.Columns[3].HeaderText = "Lesson number";
                    dataGridView1.Columns[4].HeaderText = "Date";
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                MessageBox.Show("Choose a group!");
            }
        }

        public void ReSearch()
        {
            if (comboBox4.Text != "" && textBox3.Text != "")
            {
                string operation = "";
                switch (comboBox3.Text)
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

                switch (comboBox4.Text)
                {
                    case "Lesson type":
                        {
                            Search += "and Lessons_type.Type_name" + operation + "'" + textBox3.Text + "' ";
                            break;
                        }
                    case "Semester":
                        {
                            Search += "and Shedule.Semester" + operation + "'" + textBox3.Text + "' ";
                            break;
                        }
                    case "Audience":
                        {
                            Search += "and Classroom.Audience" + operation + "'" + textBox3.Text + "' ";
                            break;
                        }
                    case "Lesson number":
                        {
                            Search += "and Shedule.Num_lesson" + operation + "'" + textBox3.Text + "' ";
                            break;
                        }
                    case "Date":
                        {
                            Search += "and Shedule.SDate" + operation + "'" + textBox3.Text + "' ";
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Search = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && comboBox2.Text != "")
            {
                int Group = Gro_id[comboBox2.SelectedIndex];
                int Teacher = Id;
                int Subject = Sub_id[comboBox1.SelectedIndex];
                int Type_id = 0;
                int Semester = 0;
                int Class_id = 0;
                string S_date = "";
                int Number = 0;

                S_date = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[4].Value.ToString().Substring(0, 10).Replace('.', '-');
                string newdate = S_date.Substring(S_date.LastIndexOf('-') + 1, S_date.Length - S_date.LastIndexOf('-') - 1) + "-" + S_date.Substring(S_date.IndexOf('-') + 1, 2) + "-" + S_date.Substring(0, 2);

                try
                {
                    MySqlCommand cmd1 = new MySqlCommand
                    {
                        Connection = conn,
                        //////////////////////////
                        CommandText = string.Format("SELECT DISTINCT Lessons_type.Type_id, Shedule.Semester, Classroom.Class_id, Shedule.Num_lesson, Shedule.SDate FROM Shedule INNER JOIN Workload on(Shedule.Teacher_id=Workload.Teacher_id) INNER JOIN Lessons_type on(Lessons_type.Type_id=Shedule.Type_id) INNER JOIN Classroom on(Shedule.Class_id=Classroom.Class_id) WHERE Shedule.Teacher_id= " + Id + " and Shedule.Group_id= " + Gro_id[comboBox2.SelectedIndex] + " and Shedule.Subject_id=" + Sub_id[comboBox1.SelectedIndex] + " and Lessons_type.Type_name='" + dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value + "' and Shedule.Semester=" + dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value + " and Classroom.Audience=" + dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value + " and Shedule.Num_lesson=" + dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[3].Value + " and Shedule.SDate='" + newdate + "';")
                    };
                    MySqlDataReader reader2 = cmd1.ExecuteReader();
                    while (reader2.Read())
                    {
                        Type_id = reader2.GetInt32(0);
                        Semester = reader2.GetInt32(1);
                        Class_id = reader2.GetInt32(2);
                        Number = reader2.GetInt32(3);
                        S_date = reader2.GetString(4);
                    }
                    reader2.Close();
                }
                catch (Exception ex)
                {

                }

                conn.Close();
                Visit_analis v = new Visit_analis(Teacher, Subject, Type_id, Semester, Class_id, S_date, Number, Group);
                v.Show();
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Check if the input fields are full!");
            }
        }
    }
}
