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
    public partial class SetOrderStatus : Form
    {
        public MySqlConnection conn;
        private int Id;
        private int Order_Id;

        List<int> status_id = new List<int>();

        public SetOrderStatus(int id, int order)
        {
            InitializeComponent();
            Order_Id = order;
            Id = id;

            DataBase.DataBaseInfo dataBase = new DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();
        }

        private void SetOrderStatus_Load(object sender, EventArgs e)
        {
            try
            {
                MySqlCommand cmd1 = new MySqlCommand();
                cmd1.Connection = conn;
                //////////////////////////
                cmd1.CommandText = string.Format("SELECT Status_id, Status_name FROM Status;");
                MySqlDataReader reader2 = cmd1.ExecuteReader();
                while (reader2.Read())
                {
                    status_id.Add(reader2.GetInt32(0));
                    comboBox1.Items.Add(reader2.GetString(1));
                }
                reader2.Close();
            }
            catch (Exception ex)
            {

            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "UPDATE Orders SET Status_id=" + status_id[comboBox1.SelectedIndex] + " WHERE Order_id= " + Order_Id + ";";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Status changed to " + comboBox1.SelectedItem);

                TeacherRoom teacherRoom = new TeacherRoom(Id);
                teacherRoom.Show();
                conn.Close();
                Dispose();
            }
            catch (Exception)
            {
            }
           
        }
    }
}
