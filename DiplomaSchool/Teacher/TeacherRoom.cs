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

namespace DiplomaSchool.Teacher
{
    public partial class TeacherRoom : Form
    {
        private int Id;
        public MySqlConnection conn;
        private int teacher_id = 0;
        private string _translationSpeakUrl;

        public TeacherRoom(int id)
        {
            InitializeComponent();
            Id = id;
            DataBase.DataBaseInfo dataBase = new DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();
        }

        private void TeacherRoom_Load(object sender, EventArgs e)
        {
            this._comboFrom.Items.AddRange(Translator.Translator.Languages.ToArray());
            this._comboTo.Items.AddRange(Translator.Translator.Languages.ToArray());
            this._comboFrom.SelectedItem = "English";
            this._comboTo.SelectedItem = "French";

            try
            {
                dataGridView1.Columns.Clear();
                MySqlDataAdapter mda = new MySqlDataAdapter("SELECT Groups.Group_id, Groups.Group_name, Services.Service_name, Status.Status_name FROM Groups INNER JOIN Students on(Students.Group_id=Groups.Group_id)INNER JOIN Users on(Users.User_id=Students.User_id) INNER JOIN Orders on(Orders.User_id=Users.User_id) INNER JOIN Services on(Services.Service_id=Orders.Service_id) INNER JOIN Status on(Orders.Status_id=Status.Status_id) WHERE Groups.Teacher_id =(SELECT Teacher_id FROM Teachers WHERE User_id=" +Id +");", conn);
                DataSet ds = new DataSet();
                mda.Fill(ds, "Service");
                dataGridView1.DataSource = ds.Tables["Service"];
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    dataGridView1.Rows[i].Cells[0].ReadOnly = true;
                /////columns names
                dataGridView1.Columns[0].HeaderText = "ID";
                dataGridView1.Columns[1].HeaderText = "Group name";
                dataGridView1.Columns[2].HeaderText = "Service name";
                dataGridView1.Columns[3].HeaderText = "Service status";

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Teacher_id FROM Teachers WHERE User_id=" + Id + ";")
                };
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    teacher_id = reader.GetInt32(0);
                }
                reader.Close();

                ReColorGrid();
            }
            catch (Exception)
            {

            }
        }

        private void ReColorGrid()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                switch (dataGridView1.Rows[i].Cells[3].Value)
                {
                    case "Processing":
                        {
                            dataGridView1.Rows[i].Cells[3].Style.BackColor = Color.Orange;
                            break;
                        }
                    case "Active":
                        {
                            dataGridView1.Rows[i].Cells[3].Style.BackColor = Color.Cyan;
                            break;
                        }
                    case "Completed":
                        {
                            dataGridView1.Rows[i].Cells[3].Style.BackColor = Color.Green;
                            break;
                        }
                }
            }
        }

        private void TeacherRoom_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
            Login.LogIn logIn = new Login.LogIn();
            logIn.Show();
            this.Dispose();
        }

        private void ToolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                Shedule shedule = new Shedule((int)dataGridView1.CurrentRow.Cells[0].Value, Id);
                shedule.Show();
                conn.Close();
                this.Dispose();
            }
            catch (Exception)
            {
                MessageBox.Show("Index out of range!");
            }
        }

        private void ToolStripButton4_Click(object sender, EventArgs e)
        {
            conn.Close();
            StartLesson l = new StartLesson(teacher_id);
            l.Show();
            this.Dispose();
        }
    }
}
