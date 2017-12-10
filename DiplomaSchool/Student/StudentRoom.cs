using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DiplomaSchool.Student
{
    public partial class StudentRoom : Form
    {
        private int Id;
        public MySqlConnection conn;
        private int student_id = 0;
        private string _translationSpeakUrl;

        public StudentRoom(int id)
        {
            InitializeComponent();
            Id = id;
            DataBase.DataBaseInfo dataBase = new DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();
        }

        private void StudentRoom_Load(object sender, EventArgs e)
        {
            this._comboFrom.Items.AddRange(Translator.Translator.Languages.ToArray());
            this._comboTo.Items.AddRange(Translator.Translator.Languages.ToArray());
            this._comboFrom.SelectedItem = "English";
            this._comboTo.SelectedItem = "French";
            try
            {
                dataGridView1.Columns.Clear();
                MySqlDataAdapter mda = new MySqlDataAdapter("SELECT DISTINCT Groups.Group_id,Groups.Group_name, Services.Service_name, Groups.Status FROM Students INNER JOIN Users on(Users.User_id=Students.User_id) INNER JOIN Orders on(Orders.User_id=Users.User_id) INNER JOIN Services on(Services.Service_id=Orders.Service_id) INNER JOIN Groups on(Students.Group_id=Groups.Group_id) WHERE Students.User_id =" + Id + " and Services.Service_id = 1 OR Services.Service_id = 2;", conn);
                DataSet ds = new DataSet();
                mda.Fill(ds, "Service");
                dataGridView1.DataSource = ds.Tables["Service"];
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    dataGridView1.Rows[i].Cells[0].ReadOnly = true;
                /////columns names
                dataGridView1.Columns[0].HeaderText = "ID";
                dataGridView1.Columns[1].HeaderText = "Group name";
                dataGridView1.Columns[2].HeaderText = "Service name";
                dataGridView1.Columns[3].HeaderText = "Groups status";

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Student_id FROM Students WHERE User_id=" + Id + ";")
                };
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    student_id = reader.GetInt32(0);
                }
                reader.Close();
                            }
            catch (Exception)
            {

            }

            try
            {
                dataGridView2.Columns.Clear();
                MySqlDataAdapter mda = new MySqlDataAdapter("SELECT Services.Service_name, Services.Service_price , Status.Status_name FROM Students INNER JOIN Users on(Users.User_id=Students.User_id) INNER JOIN Orders on(Orders.User_id=Users.User_id) INNER JOIN Services on(Services.Service_id=Orders.Service_id) INNER JOIN Status on(Status.Status_id=Orders.Status_id) WHERE Students.User_id =" + Id + " and Services.Service_id != 1 and Services.Service_id != 2;", conn);
                DataSet ds = new DataSet();
                mda.Fill(ds, "Service");
                dataGridView2.DataSource = ds.Tables["Service"];
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    dataGridView2.Rows[i].Cells[0].ReadOnly = true;
                /////columns names
                dataGridView2.Columns[0].HeaderText = "Service name";
                dataGridView2.Columns[1].HeaderText = "Service price";
                dataGridView2.Columns[2].HeaderText = "Status name";

                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception)
            {

            }
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
            try
            {
                GroupMembers group = new GroupMembers(Id, (int)dataGridView1.CurrentRow.Cells[0].Value);
                group.Show();
                conn.Close();
                this.Dispose();
            }
            catch (Exception)
            {
                MessageBox.Show("Index out of range!");
            }
        }

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                StudentMarks marks = new StudentMarks(student_id);
                marks.Show();
                conn.Close();
                this.Dispose();
            }
            catch (Exception)
            {

            }

        }

        private void ToolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                Homework homework = new Homework(student_id);
                homework.Show();
                conn.Close();
                this.Dispose();
            }
            catch (Exception)
            {

            }
        }

        private void StudentRoom_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
            Login.LogIn logIn = new Login.LogIn();
            logIn.Show();
            this.Dispose();
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
        
        private void ToolStripButton10_Click(object sender, EventArgs e)
        {
            try
            {
                if ((int)dataGridView1.CurrentRow.Cells[3].Value != 0)
                {
                    MessageBox.Show("You already have active course!First complete the started course!");
                }
                else
                {
                    NewService service = new NewService(true, Id);
                    service.Show();
                    conn.Close();
                    this.Dispose();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Select Group on page 1!");
            }
           
        }

        private void ToolStripButton5_Click(object sender, EventArgs e)
        {
            NewService service = new NewService(false, Id);
            service.Show();
            conn.Close();
            this.Dispose();
        }

        private void ToolStripButton7_Click(object sender, EventArgs e)
        {
            SaveFileDialog s = new SaveFileDialog
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
            };
            string path = "";

            if (s.ShowDialog() == DialogResult.OK)
            {
                path = s.FileName;
            }

            try
            {
                FileStream fs = new FileStream(@path, FileMode.Create);
                StreamWriter streamWriter = new StreamWriter(fs);


                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    streamWriter.Write(dataGridView2.Columns[j].HeaderText + " ");
                }
                streamWriter.WriteLine();

                for (int j = 0; j < dataGridView2.Rows.Count; j++)
                {
                    for (int i = 0; i < dataGridView2.Rows[j].Cells.Count; i++)
                    {
                        streamWriter.Write(dataGridView2.Rows[j].Cells[i].Value + " ");
                    }

                    streamWriter.WriteLine();
                }

                streamWriter.Close();
                fs.Close();

                MessageBox.Show("File saved successfully");
            }
            catch
            {
                MessageBox.Show("Saving file Error!");
            }
        }

        private void ToolStripButton8_Click(object sender, EventArgs e)
        {
            SaveFileDialog s = new SaveFileDialog
            {
                Filter = "pdf files (*.pdf)|*.pdf|All files (*.*)|*.*"
            };
            string path = "";

            if (s.ShowDialog() == DialogResult.OK)
            {
                path = s.FileName;
            }

            try
            {
                MySqlDataAdapter mda = new MySqlDataAdapter("SELECT DISTINCT Services.Service_name, Services.Service_price, Status.Status_name FROM Students INNER JOIN Users on(Users.User_id = Students.User_id) INNER JOIN Orders on(Orders.User_id = Users.User_id) INNER JOIN Services on(Services.Service_id = Orders.Service_id) INNER JOIN Status on(Status.Status_id = Orders.Status_id) WHERE Students.User_id = " + Id + " and Services.Service_id != 1 and Services.Service_id != 2; ", conn);
                DataSet ds = new DataSet();
                mda.Fill(ds, "Services");

                iTextSharp.text.Document doc = new iTextSharp.text.Document();

                PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));

                doc.Open();

                BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);

                PdfPTable table = new PdfPTable(ds.Tables["Services"].Columns.Count);

                PdfPCell cell = new PdfPCell(new Phrase(" " + "Using services" + " ", font))
                {
                    Colspan = ds.Tables["Services"].Columns.Count,
                    HorizontalAlignment = PdfPCell.ALIGN_CENTER,
                    Border = 0
                };
                table.AddCell(cell);
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    cell = new PdfPCell(new Phrase(new Phrase(dataGridView1.Columns[j].HeaderText, font)))
                    {
                        BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY
                    };
                    table.AddCell(cell);
                }

                for (int j = 0; j < ds.Tables["Services"].Rows.Count; j++)
                {
                    for (int k = 0; k < ds.Tables["Services"].Columns.Count; k++)
                    {
                        table.AddCell(new Phrase(ds.Tables["Services"].Rows[j][k].ToString(), font));
                    }
                }
                doc.Add(table);

                doc.Close();

                MessageBox.Show("Pdf-document saved!");
            }
            catch (Exception)
            {
                MessageBox.Show("Problems with saving to PDF format!");
            }
        }

        private void ToolStripButton6_Click(object sender, EventArgs e)
        {
            SaveFileDialog s = new SaveFileDialog
            {
                Filter = "xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*"
            };
            string path = "";

            if (s.ShowDialog() == DialogResult.OK)
            {
                path = s.FileName;
            }
            try
            {
                SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook);

                WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.
                    AppendChild<Sheets>(new Sheets());

                Sheet sheet = new Sheet()
                {
                    Id = spreadsheetDocument.WorkbookPart.
                    GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Services"
                };
                sheets.Append(sheet);
                spreadsheetDocument.Close();

                Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                ExcelApp.Application.Workbooks.Open(path);
                ExcelApp.Columns.ColumnWidth = 15;

                ExcelApp.Cells[1, 1] = "Service name";
                ExcelApp.Cells[1, 2] = "Service price";
                ExcelApp.Cells[1, 3] = "Status name";

                for (int i = 0; i < dataGridView2.ColumnCount; i++)
                {
                    for (int j = 0; j < dataGridView2.RowCount - 1; j++)
                    {
                        ExcelApp.Cells[j + 2, i + 1] = (dataGridView2.Rows[j].Cells[i].Value).ToString();
                    }
                }
                ExcelApp.Quit();
            }
            catch (Exception)
            {

            }
        }

        private void ToolStripButton9_Click(object sender, EventArgs e)
        {
            try
            {
                bmp = new Bitmap(dataGridView1.Size.Width + 10, dataGridView1.Size.Height + 10);
                dataGridView1.DrawToBitmap(bmp, dataGridView1.Bounds);
                if ((dataGridView1.Size.Width + 10) > 210)
                {
                    printDocument1.DefaultPageSettings.Landscape = true;
                }
                else
                {
                    printDocument1.DefaultPageSettings.Landscape = false;
                }
                printDocument1.DefaultPageSettings.Color = false;

                printPreviewDialog1.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Printing Error!");
            }
        }

        Bitmap bmp;
        private void PrintDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bmp, 0, 0);
        }

        private void _lnkSourceEnglish_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this._comboFrom.SelectedItem = "English";
        }

        private void _lnkTargetEnglish_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this._comboTo.SelectedItem = "Ukrainian";
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
    }
}
