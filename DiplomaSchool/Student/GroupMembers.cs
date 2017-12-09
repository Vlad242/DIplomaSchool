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
    public partial class GroupMembers : Form
    {
        private int Id;
        private int Group;
        public MySqlConnection conn;

        public GroupMembers(int id, int group)
        {
            InitializeComponent();
            Id = id; Group = group;
            DataBase.DataBaseInfo dataBase = new DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();
        }

        private void GroupMembers_Load(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Columns.Clear();
                MySqlDataAdapter mda = new MySqlDataAdapter("SELECT DISTINCT S_Surname, S_Name, S_Fname, S_Phone FROM Students WHERE Group_id =" + Group +";", conn);
                DataSet ds = new DataSet();
                mda.Fill(ds, "Students");
                dataGridView1.DataSource = ds.Tables["Students"];
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    dataGridView1.Rows[i].Cells[0].ReadOnly = true;
                /////columns names
                dataGridView1.Columns[0].HeaderText = "Surname";
                dataGridView1.Columns[1].HeaderText = "Name";
                dataGridView1.Columns[2].HeaderText = "Fname";
                dataGridView1.Columns[3].HeaderText = "Phone number";

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception)
            {

            }
        }

        private void GroupMembers_FormClosing(object sender, FormClosingEventArgs e)
        {
            StudentRoom room = new StudentRoom(Id);
            room.Show();
            conn.Close();
            this.Dispose();
        }
    }
}
