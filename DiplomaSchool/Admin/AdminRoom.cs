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

namespace DiplomaSchool.Admin
{
    public partial class AdminRoom : Form
    {
        private int Id;
        public MySqlConnection conn;

        List<int> teacher_id = new List<int>();
        List<int> lesson_type = new List<int>();
        List<int> subject = new List<int>();
        List<int> groups = new List<int>();
        List<int> classrom = new List<int>();

        public AdminRoom(int id)
        {
            Id = id;
            InitializeComponent();

            DataBase.DataBaseInfo dataBase = new DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy:MM:dd";
        }

        private void AdminRoom_Load(object sender, EventArgs e)
        {
            try
            {////////////////////teacher
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Teacher_id, T_surname, T_name, T_fname FROM Teachers;")
                };
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    teacher_id.Add(reader.GetInt32(0));
                    comboBox1.Items.Add(reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3));
                    comboBox4.Items.Add(reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3));
                }
                reader.Close();
                ////////////////lesson type
                cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Type_id, Type_name FROM Lessons_type;")
                };
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lesson_type.Add(reader.GetInt32(0));
                    comboBox2.Items.Add(reader.GetString(1));
                }
                reader.Close();
                ///////////////Subject
                cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Subject_id, Sub_name FROM Subject;")
                };
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    subject.Add(reader.GetInt32(0));
                    comboBox3.Items.Add(reader.GetString(1));
                }
                reader.Close();
                ///////////////groups
                cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Group_id, Group_name FROM Groups;")
                };
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    groups.Add(reader.GetInt32(0));
                    comboBox6.Items.Add(reader.GetString(1));
                }
                reader.Close();
                ///////////////groups
                cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Class_id, Type_of_class FROM Classroom;")
                };
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    classrom.Add(reader.GetInt32(0));
                    comboBox5.Items.Add(reader.GetString(1));
                }
                reader.Close();

                ReBuildData();
            }
            catch (Exception ex)
            {

            }
           
        }

        public void ReBuildData()
        {
            try
            {
                dataGridView1.Columns.Clear();
                MySqlDataAdapter mda = new MySqlDataAdapter("SELECT * from groups;", conn);
                DataSet ds = new DataSet();
                mda.Fill(ds, "groups");
                dataGridView1.DataSource = ds.Tables["groups"];
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    dataGridView1.Rows[i].Cells[0].ReadOnly = true;
                /////columns names
                dataGridView1.Columns[0].HeaderText = "ID";
                dataGridView1.Columns[1].HeaderText = "Teacher";
                dataGridView1.Columns[2].HeaderText = "Group name";
                dataGridView1.Columns[3].HeaderText = "Status";

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                //////////////////////////////

                dataGridView2.Columns.Clear();
                mda = new MySqlDataAdapter("SELECT * from Subject;", conn);
                ds = new DataSet();
                mda.Fill(ds, "Subject");
                dataGridView2.DataSource = ds.Tables["Subject"];
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    dataGridView2.Rows[i].Cells[0].ReadOnly = true;
                /////columns names
                dataGridView2.Columns[0].HeaderText = "ID";
                dataGridView2.Columns[1].HeaderText = "Name";

                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                ////////////////////////
                dataGridView3.Columns.Clear();
                mda = new MySqlDataAdapter("SELECT Lessons_type.Type_name, Subject.Sub_name, Teachers.Teacher_id, Teachers.T_Surname, Teachers.T_Name, Workload.Semester, Workload.Hours FROM Workload INNER JOIN Lessons_type on(Workload.Type_id=Lessons_type.Type_id) INNER JOIN Subject on(Subject.Subject_id=Workload.Subject_id) INNER JOIN Teachers on(Teachers.Teacher_id=Workload.Teacher_id);", conn);
                ds = new DataSet();
                mda.Fill(ds, "Subject");
                dataGridView3.DataSource = ds.Tables["Subject"];
                for (int i = 0; i < dataGridView3.Rows.Count; i++)
                    dataGridView3.Rows[i].Cells[0].ReadOnly = true;
                /////columns names
                dataGridView3.Columns[0].HeaderText = "Lesson type";
                dataGridView3.Columns[1].HeaderText = "Subject";
                dataGridView3.Columns[2].HeaderText = "Teacher id";
                dataGridView3.Columns[3].HeaderText = "Teacher surname";
                dataGridView3.Columns[4].HeaderText = "Teacher name";
                dataGridView3.Columns[5].HeaderText = "Semester";
                dataGridView3.Columns[6].HeaderText = "Hours";

                dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception)
            {

            }

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if(comboBox1.Text != string.Empty && textBox1.Text != string.Empty)
            {
                try
                {
                    string sql = "Insert into Groups (Group_id, Teacher_id, Group_name, Status) values (null," +
                                 teacher_id[comboBox1.SelectedIndex] + ",'" + textBox1.Text + "', 1);";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Group " + textBox1.Text + " was added!");
                    textBox1.Text = "";
                    ReBuildData();
                }
                catch (Exception)
                {
                }
            }
            else
            {
                MessageBox.Show("Fill out all form fields");
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != string.Empty)
            {
                try
                {
                    string sql = "Insert into Subject (Subject_id, Sub_name) values (null, '" +
                                textBox2.Text + "');";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Subject " + textBox2.Text + " was added!");
                    textBox2.Text = "";
                    ReBuildData();
                }
                catch (Exception)
                {
                }
            }
            else
            {
                MessageBox.Show("Fill out all form fields");
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != string.Empty)
            {
                try
                {
                    string sql = "Insert into Classroom (Class_id, Building, Audience, Type_of_class, Capacity) values (null," +
                                 numericUpDown4.Value + "," + numericUpDown5.Value + ",'" + textBox3.Text + "'," + numericUpDown6.Value + ");";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Classroom " + textBox3.Text + " was added!");
                    textBox3.Text = "";
                    ReBuildData();
                }
                catch (Exception)
                {
                }
            }
            else
            {
                MessageBox.Show("Fill out all form fields");
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text != string.Empty && comboBox3.Text != string.Empty && comboBox4.Text != string.Empty)
            {
                try
                {
                    string sql = "Insert into Workload (Type_id, Subject_id, Teacher_id, Semester, Hours) values (" +
                                 lesson_type[comboBox2.SelectedIndex] + "," + subject[comboBox3.SelectedIndex] + "," + teacher_id[comboBox4.SelectedIndex] + "," + numericUpDown1.Value + "," + numericUpDown2.Value + ");";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    ReBuildData();
                    MessageBox.Show("Workload created!");
                }
                catch (Exception)
                {
                }
            }
            else
            {
                MessageBox.Show("Fill out all form fields");
            }

        }

        private void Button4_Click(object sender, EventArgs e)
        {
            if (comboBox5.Text != string.Empty && comboBox6.Text != string.Empty)
            {
                try
                {
                    int class_id = 0;
                    int typeid = 0;
                    int subject_id = 0;
                    int teacher = 0;
                    int semester = 0;
                    int hours = 0;
                    int group = 0;

                    try
                    {
                        //////////select type
                        MySqlCommand cmd1 = new MySqlCommand
                        {
                            Connection = conn,
                            CommandText = string.Format("SELECT Type_id FROM Lessons_type WHERE Type_name= '" +dataGridView3.CurrentRow.Cells[0].Value + "';")
                        };
                        MySqlDataReader reader = cmd1.ExecuteReader();
                        while (reader.Read())
                        {
                            typeid = reader.GetInt32(0);

                        }
                        reader.Close();
                        ////////////select subject
                        cmd1 = new MySqlCommand
                        {
                            Connection = conn,
                            CommandText = string.Format("SELECT Subject_id FROM Subject WHERE Sub_name= '" + dataGridView3.CurrentRow.Cells[1].Value + "';")
                        };
                        reader = cmd1.ExecuteReader();
                        while (reader.Read())
                        {
                            subject_id = reader.GetInt32(0);

                        }
                        reader.Close();

                        class_id = classrom[comboBox5.SelectedIndex];
                        teacher = (int)dataGridView3.CurrentRow.Cells[2].Value;
                        semester = (int)dataGridView3.CurrentRow.Cells[5].Value;
                        hours = (int)dataGridView3.CurrentRow.Cells[6].Value;
                        group = groups[comboBox6.SelectedIndex];
                    }
                    catch (Exception)
                    {
                    }
                    string sql = "Insert into Shedule (Class_id, Type_id, Subject_id, Teacher_id, Semester, SDate, Num_lesson, Group_id) values (" +
                                class_id + "," + typeid + "," + subject_id + "," + teacher + ","+semester + ",'"+dateTimePicker1.Text+ "'," + numericUpDown3.Value + ","+ group + ");";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Shedule created!");
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                MessageBox.Show("Fill out all form fields");
            }
        }

        private void AdminRoom_FormClosing(object sender, FormClosingEventArgs e)
        {
            Login.LogIn logIn = new Login.LogIn();
            logIn.Show();
            conn.Close();
            this.Dispose();
        }
    }
}
