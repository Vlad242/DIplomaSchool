using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiplomaSchool.Student
{
    public partial class StudentMarks : Form
    {
        private int Id;
        public MySqlConnection conn;
        public string Search = "";

        public StudentMarks(int id)
        {
            InitializeComponent();
            Id = id;
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

        private void StudentMarks_Load(object sender, EventArgs e)
        {
            try
            {
                MySqlCommand cmd2 = new MySqlCommand();
                cmd2.Connection = conn;
                //////////////////////////
                cmd2.CommandText = string.Format("Select S_Surname, S_Name FROM Students Where Student_id = '" + Id + "';");
                MySqlDataReader reader = cmd2.ExecuteReader();
                while (reader.Read())
                {
                    //label1.Text += reader.GetString(0) + " ";

                    this.Text = "Student assessment " + reader.GetString(0);
                    this.Text += " " + reader.GetString(1);
                }
                reader.Close();
                //////////////////////////
                MySqlCommand cmd3 = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT count(Mark) From Marks Where Mark <= 2 and Student_id='" + Id + "';")
                };
                MySqlDataReader reader3 = cmd3.ExecuteReader();
                while (reader3.Read())
                {
                    label1.Text += "Total 1-2: " + reader3.GetInt32(0);
                }
                reader3.Close();

                MySqlCommand cmd4 = new MySqlCommand
                {
                    Connection = conn,
                    //////////////////////////
                    CommandText = string.Format("SELECT count(Mark) From Marks Where Mark = 3 and Student_id='" + Id + "';")
                };
                MySqlDataReader reader4 = cmd4.ExecuteReader();
                while (reader4.Read())
                {
                    label2.Text += "Total 3: " + reader4.GetInt32(0);
                }
                reader4.Close();

                MySqlCommand cmd1 = new MySqlCommand
                {
                    Connection = conn,
                    //////////////////////////
                    CommandText = string.Format("SELECT count(Mark) From Marks Where Mark = 4 and Student_id='" + Id + "';")
                };
                MySqlDataReader reader0 = cmd1.ExecuteReader();
                while (reader0.Read())
                {
                    label3.Text += "Total 4: " + reader0.GetInt32(0);
                }
                reader0.Close();

                MySqlCommand cmd5 = new MySqlCommand
                {
                    Connection = conn,
                    //////////////////////////
                    CommandText = string.Format("SELECT count(Mark) From Marks Where Mark = 5 and Student_id='" + Id + "';")
                };
                MySqlDataReader reader5 = cmd5.ExecuteReader();
                while (reader5.Read())
                {
                    label4.Text += "Total 5: " + reader5.GetInt32(0);
                }
                reader5.Close();
            }
            catch (Exception)
            {

            }
            button3.PerformClick();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ReSearch();

            dataGridView1.Columns.Clear();
            MySqlDataAdapter mda = new MySqlDataAdapter("SELECT DISTINCT Subject.Sub_name, Teachers.T_Surname, Teachers.T_Name, Teachers.T_Fname, Marks.Mark, Marks.SDate FROM Marks INNER JOIN Shedule on(Marks.Subject_id = Shedule.Subject_id) INNER JOIN Workload on(Shedule.Subject_id= Shedule.Subject_id) INNER JOIN Subject on(Subject.Subject_id= Marks.Subject_id) INNER JOIN Teachers on(Marks.Teacher_id= Teachers.Teacher_id) WHERE Marks.Student_id = '" + Id + "' " + Search + ";", conn);
            DataSet ds = new DataSet();
            mda.Fill(ds, "Marks");
            dataGridView1.DataSource = ds.Tables["Marks"];
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                dataGridView1.Rows[i].Cells[0].ReadOnly = true;
            /////columns names
            dataGridView1.Columns[0].HeaderText = "Subject name";
            dataGridView1.Columns[1].HeaderText = "Teacher surname";
            dataGridView1.Columns[2].HeaderText = "Teacher name";
            dataGridView1.Columns[3].HeaderText = "Teacher fname";
            dataGridView1.Columns[4].HeaderText = "Mark";
            dataGridView1.Columns[5].HeaderText = "Date";
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

        private void Button3_Click(object sender, EventArgs e)
        {
            Search = "";
            dataGridView1.Columns.Clear();
            MySqlDataAdapter mda = new MySqlDataAdapter("SELECT DISTINCT Subject.Sub_name, Teachers.T_Surname, Teachers.T_Name, Teachers.T_Fname, Marks.Mark, Marks.SDate FROM Marks INNER JOIN Shedule on(Marks.Subject_id = Shedule.Subject_id) INNER JOIN Workload on(Shedule.Subject_id= Shedule.Subject_id) INNER JOIN Subject on(Subject.Subject_id= Marks.Subject_id) INNER JOIN Teachers on(Marks.Teacher_id= Teachers.Teacher_id) WHERE Marks.Student_id = '" + Id + "';", conn);
            DataSet ds = new DataSet();
            mda.Fill(ds, "Marks");
            dataGridView1.DataSource = ds.Tables["Marks"];
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                dataGridView1.Rows[i].Cells[0].ReadOnly = true;
            /////columns names
            dataGridView1.Columns[0].HeaderText = "Subject name";
            dataGridView1.Columns[1].HeaderText = "Teacher surname";
            dataGridView1.Columns[2].HeaderText = "Teacher name";
            dataGridView1.Columns[3].HeaderText = "Teacher fname";
            dataGridView1.Columns[4].HeaderText = "Mark";
            dataGridView1.Columns[5].HeaderText = "Date";
        }
        public void ReSearch()
        {
            if (comboBox1.Text != "" && textBox1.Text != "")
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
                            Search += "and Subject.Sub_name" + operation + "'" + textBox1.Text + "' ";
                            break;
                        }
                    case "Teacher surname":
                        {
                            Search += "and Teachers.T_Surname" + operation + "'" + textBox1.Text + "' ";
                            break;
                        }
                    case "Teacher name":
                        {
                            Search += "and Teachers.T_Name" + operation + "'" + textBox1.Text + "' ";
                            break;
                        }
                    case "Mark":
                        {
                            Search += "and Marks.Mark" + operation + "'" + textBox1.Text + "' ";
                            break;
                        }
                    case "Date":
                        {
                            Search += "and Marks.SDate" + operation + "'" + textBox1.Text + "' ";
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
            textBox1.Text = dateTimePicker1.Text;
        }

        private void StudentMarks_FormClosing(object sender, FormClosingEventArgs e)
        {
            StudentRoom student = new StudentRoom(Id);
            student.Show();
            conn.Close();
            this.Dispose();
        }
    }
}
