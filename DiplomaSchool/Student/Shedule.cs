using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace DiplomaSchool.Student
{
    public partial class Shedule : Form
    {
        public MySqlConnection conn;
        private int GroupId;
        private int Id;
        public string Search = "";

        public Shedule(int groupId, int id)
        {
            InitializeComponent();
            GroupId = groupId;
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

        private void Shdule_Load(object sender, EventArgs e)
        {
            button3.PerformClick();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            ReSearch();
            dataGridView1.Columns.Clear();
            MySqlDataAdapter mda = new MySqlDataAdapter("SELECT DISTINCT Subject.Sub_name, Lessons_type.Type_name, Teachers.Academic_status, Teachers.T_Surname, Teachers.T_Name, Teachers.T_Fname, Classroom.Building, Classroom.Audience, Shedule.Num_lesson, Shedule.SDate FROM Shedule INNER JOIN Classroom on(Classroom.Class_id= Shedule.Class_id) INNER JOIN Workload on(Shedule.Teacher_id= Workload.Teacher_id) INNER JOIN Lessons_type on(Lessons_type.Type_id= Shedule.Type_id) INNER JOIN Teachers on(Teachers.Teacher_id= Shedule.Teacher_id) INNER JOIN Subject on(Subject.Subject_id= Shedule.Subject_id) INNER JOIN Groups on(Shedule.Group_id= Groups.Group_id) WHERE Groups.Group_id ='" + GroupId + "'" + Search + ";", conn);
            DataSet ds = new DataSet();
            mda.Fill(ds, "Subject");
            dataGridView1.DataSource = ds.Tables["Subject"];
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                dataGridView1.Rows[i].Cells[0].ReadOnly = true;
            /////columns names
            dataGridView1.Columns[0].HeaderText = "Subject type";
            dataGridView1.Columns[1].HeaderText = "Lesson type";
            dataGridView1.Columns[2].HeaderText = "Academic status";
            dataGridView1.Columns[3].HeaderText = "Teachers surname";
            dataGridView1.Columns[4].HeaderText = "Teachers name";
            dataGridView1.Columns[5].HeaderText = "Teachers fname";
            dataGridView1.Columns[6].HeaderText = "Building";
            dataGridView1.Columns[7].HeaderText = "Audience";
            dataGridView1.Columns[8].HeaderText = "Lesson number";
            dataGridView1.Columns[9].HeaderText = "Date";
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
                    case "Subject type":
                        {
                            Search += "and Subject.Sub_name" + operation + "'" + textBox2.Text + "' ";
                            break;
                        }
                    case "Lesson type":
                        {
                            Search += "and Lessons_type.Type_name" + operation + "'" + textBox2.Text + "' ";
                            break;
                        }
                    case "Academic status":
                        {
                            Search += "and Teachers.Academic_status" + operation + "'" + textBox2.Text + "' ";
                            break;
                        }
                    case "Teachers surname":
                        {
                            Search += "and Teachers.T_Surname" + operation + "'" + textBox2.Text + "' ";
                            break;
                        }
                    case "Teachers name":
                        {
                            Search += "and Teachers.T_Name" + operation + "'" + textBox2.Text + "' ";
                            break;
                        }
                    case "Teachers fname":
                        {
                            Search += "and Teachers.T_Fname" + operation + "'" + textBox2.Text + "' ";
                            break;
                        }
                    case "Building":
                        {
                            Search += "and Classroom.Building" + operation + "'" + textBox2.Text + "' ";
                            break;
                        }
                    case "Audience":
                        {
                            Search += "and Classroom.Audience" + operation + "'" + textBox2.Text + "' ";
                            break;
                        }
                    case "Lesson number":
                        {
                            Search += "and Shedule.Num_lesson" + operation + "'" + textBox2.Text + "' ";
                            break;
                        }
                    case "Date":
                        {
                            Search += "and Shedule.SDate" + operation + "'" + textBox2.Text + "' ";
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Search = "";
            dataGridView1.Columns.Clear();
            MySqlDataAdapter mda = new MySqlDataAdapter("SELECT DISTINCT Subject.Sub_name, Lessons_type.Type_name, Teachers.Academic_status, Teachers.T_Surname, Teachers.T_Name, Teachers.T_Fname, Classroom.Building, Classroom.Audience, Shedule.Num_lesson, Shedule.SDate FROM Shedule INNER JOIN Classroom on(Classroom.Class_id= Shedule.Class_id) INNER JOIN Workload on(Shedule.Teacher_id= Workload.Teacher_id) INNER JOIN Lessons_type on(Lessons_type.Type_id= Shedule.Type_id) INNER JOIN Teachers on(Teachers.Teacher_id= Shedule.Teacher_id) INNER JOIN Subject on(Subject.Subject_id= Shedule.Subject_id) INNER JOIN Groups on(Shedule.Group_id= Groups.Group_id) WHERE Groups.Group_id='" + GroupId + "';", conn);
            DataSet ds = new DataSet();
            mda.Fill(ds, "Subject");
            dataGridView1.DataSource = ds.Tables["Subject"];
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                dataGridView1.Rows[i].Cells[0].ReadOnly = true;
            /////columns names
            dataGridView1.Columns[0].HeaderText = "Subject type";
            dataGridView1.Columns[1].HeaderText = "Lesson type";
            dataGridView1.Columns[2].HeaderText = "Academic status";
            dataGridView1.Columns[3].HeaderText = "Teachers surname";
            dataGridView1.Columns[4].HeaderText = "Teachers name";
            dataGridView1.Columns[5].HeaderText = "Teachers fname";
            dataGridView1.Columns[6].HeaderText = "Building";
            dataGridView1.Columns[7].HeaderText = "Audience";
            dataGridView1.Columns[8].HeaderText = "Lesson number";
            dataGridView1.Columns[9].HeaderText = "Date";
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

        private void Shedule_FormClosing(object sender, FormClosingEventArgs e)
        {
            StudentRoom room = new StudentRoom(Id);
            room.Show();
            conn.Close();
            this.Dispose();
        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            textBox2.Text = dateTimePicker1.Text;
        }
    }
}
