using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DiplomaSchool.Teacher
{
    public partial class Marks_analis : Form
    {
        public MySqlConnection conn;
        int Group = 0;
        int Teacher = 0;
        int Subject = 0;
        int Type_id = 0;
        int Semester = 0;
        int Class_id = 0;
        string S_date = "";
        int Number = 0;
        string Search = "";
        int out_id;

        public List<int> Student_id = new List<int>();
        public List<string> Register = new List<string>();
        public List<int> Student = new List<int>();

        public Marks_analis(int teach, int sub, int typ, int sem, int clas, string dat, int num, int gro, List<int> stud)
        {
            InitializeComponent();
            DataBase.DataBaseInfo dataBase = new DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();
            
            Student_id = stud;
            Teacher = teach;
            Subject = sub;
            Type_id = typ;
            Semester = sem;
            Class_id = clas;
            S_date = dat;
            Number = num;
            Group = gro;
        }

        public void Reload()
        {
            foreach (var item in Student_id)
            {
                Search += " and Student_id!=" + item;
            }
        }

        private void Marks_analis_Load(object sender, EventArgs e)
        {
            Reload();
            Student_id.Clear();
            try
            {
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    //////////////////////////
                    CommandText = string.Format("SELECT Group_name FROM Groups WHERE Group_id=" + Group + "; ")
                };
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    this.Text = "Оцінювання студентів групи " + reader.GetString(0);
                }
                reader.Close();
            }
            catch (Exception)
            {

            }

            try
            {
                dataGridView1.ColumnCount = 4;
                dataGridView1.ColumnHeadersVisible = true;

                dataGridView1.Columns[0].HeaderText = "Surname";
                dataGridView1.Columns[1].HeaderText = "Name";
                dataGridView1.Columns[2].HeaderText = "Fname";
                dataGridView1.Columns[3].HeaderText = "Mark";

                MySqlCommand cmd1 = new MySqlCommand
                {
                    Connection = conn,
                    //////////////////////////
                    CommandText = string.Format("SELECT Student_id, S_Surname, S_Name, S_Fname FROM Students WHERE Group_id=" + Group + Search + ";")
                };
                MySqlDataReader reader2 = cmd1.ExecuteReader();
                int RowIndex = 0;
                while (reader2.Read())
                {
                    Student_id.Add(reader2.GetInt32(0));
                    dataGridView1.Rows.Add("", "", "", 0);
                    dataGridView1.Rows[RowIndex].Cells[0].Value = reader2.GetString(1);
                    dataGridView1.Rows[RowIndex].Cells[1].Value = reader2.GetString(2);
                    dataGridView1.Rows[RowIndex].Cells[2].Value = reader2.GetString(3);
                    dataGridView1.Rows[RowIndex].Cells[3].Value = 0;
                    RowIndex++;
                }
                reader2.Close();
            }
            catch (Exception ex)
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Register.Clear();
            Student.Clear();
            bool complete = true;
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                if (dataGridView1.Rows[i].Cells[3].Value.ToString() != "0")
                {
                    Register.Add(dataGridView1.Rows[i].Cells[3].Value.ToString());
                    Student.Add(Student_id[i]);
                }
            }

            int counter = 0;
            foreach (var item in Register)
            {
                try
                {
                    string sql = "Insert into Marks (Teacher_id, Subject_id, Type_id, Semester, Class_id, SDate, Num_lesson, Student_id, Mark) values (" + Teacher + "," + Subject + "," + Type_id + "," + Semester + "," + Class_id + ",'" + S_date + "'," + Number + "," + Student[counter] + "," + item + ");";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    counter++;
                }
                catch (Exception ex)
                {
                    complete = false;
                }
            }

            if (complete)
            {
                MessageBox.Show("Evaluation is done!");
                button1.Enabled = false;
                button1.Visible = false;

                conn.Close();
                Add_Url t = new Add_Url(Teacher, Subject, Type_id, Semester, Class_id, S_date, Number);
                t.Show();
                this.Close();
            }
            else
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
                        out_id = reader.GetInt32(0);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {

                }

                MessageBox.Show("Evaluation has not been done! Maybe evaluation has already been done!");
                conn.Close();
                TeacherRoom t = new TeacherRoom(out_id);
                t.Show();
                this.Close();
            }
        }
    }
}
