using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DiplomaSchool.Teacher
{
    public partial class TeacherRoom : Form
    {
        private int Id;
        public MySqlConnection conn;
        private int teacher_id = 0;
        private string _translationSpeakUrl;
        static private Socket Client;
        private IPAddress ip = null;
        private int port = 0;
        private Thread th;

        public TeacherRoom(int id)
        {
            InitializeComponent();
            Id = id;
            DataBase.DataBaseInfo dataBase = new DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();

            button5.Visible = false;
            button4.Enabled = false;
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
                MySqlDataAdapter mda = new MySqlDataAdapter("SELECT DISTINCT Groups.Group_id, Groups.Group_name, Services.Service_name, Status.Status_name FROM Groups INNER JOIN Students on(Students.Group_id=Groups.Group_id)INNER JOIN Users on(Users.User_id=Students.User_id) INNER JOIN Orders on(Orders.User_id=Users.User_id) INNER JOIN Services on(Services.Service_id=Orders.Service_id) INNER JOIN Status on(Orders.Status_id=Status.Status_id) WHERE Groups.Teacher_id =(SELECT Teacher_id FROM Teachers WHERE User_id=" +Id +") and Orders.Service_id =1 or Orders.Service_id = 2;", conn);
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
            try
            {
                dataGridView2.Columns.Clear();
                MySqlDataAdapter mda = new MySqlDataAdapter("SELECT DISTINCT Orders.Order_id, Services.Service_name, Services.Service_price, Status.Status_name FROM Users INNER JOIN Orders on(Orders.User_id = Users.User_id) INNER JOIN Services on(Services.Service_id = Orders.Service_id) INNER JOIN Status on(Status.Status_id = Orders.Status_id) WHERE Services.Service_id != 1 and Services.Service_id != 2;", conn);
                DataSet ds = new DataSet();
                mda.Fill(ds, "Service");
                dataGridView2.DataSource = ds.Tables["Service"];
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    dataGridView2.Rows[i].Cells[0].ReadOnly = true;
                /////columns names
                dataGridView2.Columns[0].HeaderText = "Order id";
                dataGridView2.Columns[1].HeaderText = "Service name";
                dataGridView2.Columns[2].HeaderText = "Service price";
                dataGridView2.Columns[3].HeaderText = "Status name";

                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                ReColorGrid2();
            }
            catch (Exception)
            {

            }

            try
            {
                var sr = new StreamReader(@"Client_info/data_info.txt");
                string buffer = sr.ReadToEnd();
                sr.Close();
                string[] connect_info = buffer.Split(':');
                ip = IPAddress.Parse(connect_info[0]);
                port = int.Parse(connect_info[1]);

                label7.ForeColor = System.Drawing.Color.Blue;
                label7.Text = " Server IP: " + connect_info[0] + "\n Server port: " + connect_info[1];

            }
            catch (Exception)
            {
                label7.ForeColor = System.Drawing.Color.Red;
                label7.Text = "Missing settings!";
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

        private void ReColorGrid2()
        {
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                switch (dataGridView2.Rows[i].Cells[3].Value)
                {
                    case "Processing":
                        {
                            dataGridView2.Rows[i].Cells[3].Style.BackColor = Color.Orange;
                            break;
                        }
                    case "Active":
                        {
                            dataGridView2.Rows[i].Cells[3].Style.BackColor = Color.Cyan;
                            break;
                        }
                    case "Completed":
                        {
                            dataGridView2.Rows[i].Cells[3].Style.BackColor = Color.Green;
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

        private void ToolStripButton10_Click(object sender, EventArgs e)
        {
            try
            {
                SetOrderStatus orderStatus = new SetOrderStatus(Id, (int)dataGridView2.CurrentRow.Cells[0].Value);
                orderStatus.Show();
                conn.Close();
                Dispose();
            }
            catch (Exception)
            {
                MessageBox.Show("Please select order!");
            }
        }

        private void _btnTranslate_Click(object sender, EventArgs e)
        {
            // Initialize the translator
            Translator.Translator t = new Translator.Translator();

            this._editTarget.Text = string.Empty;
            this._editTarget.Update();
            this._translationSpeakUrl = null;

            // Translate the text
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this._lblStatus.Text = "Translating...";
                this._lblStatus.Update();
                this._editTarget.Text = t.Translate(this._editSourceText.Text.Trim(), (string)this._comboFrom.SelectedItem, (string)this._comboTo.SelectedItem);
                if (t.Error == null)
                {
                    this._editTarget.Update();
                    this._translationSpeakUrl = t.TranslationSpeechUrl;
                }
                else
                {
                    MessageBox.Show(t.Error.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                this._lblStatus.Text = string.Format("Translated in {0} mSec", (int)t.TranslationTime.TotalMilliseconds);
                this.Cursor = Cursors.Default;
            }
        }

        private void _btnSpeak_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this._translationSpeakUrl))
            {
                this._webBrowserCtrl.Navigate(this._translationSpeakUrl);
            }
        }

        private void _lnkReverse_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Swap translation mode
            string from = (string)this._comboFrom.SelectedItem;
            string to = (string)this._comboTo.SelectedItem;
            this._comboFrom.SelectedItem = to;
            this._comboTo.SelectedItem = from;

            // Reset text
            this._editSourceText.Text = this._editTarget.Text;
            this._editTarget.Text = string.Empty;
            this.Update();
            this._translationSpeakUrl = string.Empty;
        }

        private void _lnkSourceEnglish_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this._comboFrom.SelectedItem = "English";
        }

        private void _lnkTargetEnglish_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this._comboTo.SelectedItem = "Ukrainian";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
            Application.Exit();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != string.Empty)
                {
                    button2.Enabled = true;
                    richTextBox2.Enabled = true;
                    Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    if (ip != null)
                    {
                        Client.Connect(ip, port);
                        th = new Thread(delegate () { RecvMessage(); });
                        SendMessage(textBox1.Text + "#" + ";;;5");
                        th.Start();
                        richTextBox2.Focus();
                    }
                    button3.Visible = false;
                    button5.Visible = true;
                    button4.Enabled = true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Server not start!");
            }
           
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (th != null)
                {
                    if (Client != null)
                    {
                        Client.Close();
                    }
                    th.Abort();
                    th = null;
                    Client = null;
                }
            }
            catch (ThreadAbortException)
            {
            }
        }

        void RecvMessage()
        {
            byte[] buffer = new byte[1024];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 0;
            }
            for (; ; )
            {
                try
                {
                    Client.Receive(buffer);
                    string message = Encoding.UTF8.GetString(buffer);
                    int count = message.IndexOf(";;;5");
                    if (count == -1)
                    {
                        continue;
                    }
                    string Clear_Message = "";
                    for (int i = 0; i < count; i++)
                    {
                        Clear_Message += message[i];
                    }
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        richTextBox1.AppendText(Clear_Message + "\n");
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        void SendMessage(string message)//відправлення повідомлення
        {
            if (message != string.Empty)
            {
                byte[] buffer = new byte[1024];
                buffer = Encoding.UTF8.GetBytes(message);
                Client.Send(buffer);
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            SendMessage(textBox1.Text + ": " + richTextBox2.Text + ";;;5");
            richTextBox2.Clear();
        }
    }
}
