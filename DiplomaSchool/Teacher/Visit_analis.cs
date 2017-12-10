using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DiplomaSchool.Teacher
{
    public partial class Visit_analis : Form
    {
        public MySqlConnection conn;
        int Group;
        int Teacher;
        int Subject;
        int Type_id;
        int Semester;
        int Class_id;
        string S_date = "";
        int Number = 0;

        public List<int> Student_id = new List<int>();
        public List<int> Register = new List<int>();


        public Visit_analis(int teach, int sub, int typ, int sem, int clas, string dat, int num, int gro)
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
            Group = gro;
        }

        private void Visit_analis_Load(object sender, EventArgs e)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                //////////////////////////
                cmd.CommandText = string.Format("SELECT Group_name FROM Groups WHERE Group_id=" + Group + "; ");
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    this.Text = "Visiting the group " + reader.GetString(0) + " mark the VOTES !!!";
                }
                reader.Close();
            }
            catch (Exception ex)
            {

            }

            try
            {
                MySqlCommand cmd1 = new MySqlCommand();
                cmd1.Connection = conn;
                //////////////////////////
                cmd1.CommandText = string.Format("SELECT Student_id, S_Surname, S_Name, S_Fname FROM Students WHERE Group_id=" + Group + "; ");
                MySqlDataReader reader2 = cmd1.ExecuteReader();
                while (reader2.Read())
                {
                    Student_id.Add(reader2.GetInt32(0));
                    checkedListBox1.Items.Add(reader2.GetString(1) + " " + reader2.GetString(2) + " " + reader2.GetString(3));
                }
                reader2.Close();
            }
            catch (Exception ex)
            {

            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Register.Clear();
            bool complete = true;
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    Register.Add(Student_id[i]);
                }
            }

            S_date = S_date.Substring(0, 10).Replace('.', '-');
            string newdate = S_date.Substring(S_date.LastIndexOf('-') + 1, S_date.Length - S_date.LastIndexOf('-') - 1) + ":" + S_date.Substring(S_date.IndexOf('-') + 1, 2) + ":" + S_date.Substring(0, 2);

            foreach (var item in Register)
            {
                try
                {
                    string sql = "Insert into Missing (Student_id, Teacher_id, Subject_id, Type_id, Semester, Class_id, SDate, Num_lesson ) values (" + item + "," + Teacher + "," + Subject + "," + Type_id + "," + Semester + "," + Class_id + ",'" + newdate + "'," + Number + ");";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    complete = false;
                }
            }

            if (complete)
            {
                conn.Close();
                MessageBox.Show("Visits are exhibited!");
                Marks_analis m = new Marks_analis(Teacher, Subject, Type_id, Semester, Class_id, newdate, Number, Group, Register);
                m.Show();
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Visits not exhibited!");
            }
        }
    }
}
