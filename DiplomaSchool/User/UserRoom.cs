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
using System.Windows.Forms;

namespace DiplomaSchool.User
{
    public partial class UserRoom : Form
    {
        private int Id;
        public MySqlConnection conn;

        public UserRoom(int id)
        {
            InitializeComponent();
            Id = id;
            DataBase.DataBaseInfo dataBase = new DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();
        }

        private void UserRoom_Load(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Columns.Clear();
                MySqlDataAdapter mda = new MySqlDataAdapter("SELECT Services.Service_name, Services.Service_price, Orders.Order_date, Orders.Descriptions, Status.Status_name FROM Orders INNER JOIN Status on(Status.Status_id=Orders.Status_id) INNER JOIN Services on(Services.Service_id=Orders.Service_id) WHERE Orders.User_id=" + Id + ";", conn);
                DataSet ds = new DataSet();
                mda.Fill(ds, "Orders");
                dataGridView1.DataSource = ds.Tables["Orders"];
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    dataGridView1.Rows[i].Cells[0].ReadOnly = true;
                /////columns names
                dataGridView1.Columns[0].HeaderText = "Service";
                dataGridView1.Columns[1].HeaderText = "Price";
                dataGridView1.Columns[2].HeaderText = "Date";
                dataGridView1.Columns[3].HeaderText = "Descriptions";
                dataGridView1.Columns[4].HeaderText = "Status";

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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
                switch (dataGridView1.Rows[i].Cells[4].Value)
                {
                    case "Processing":
                        {
                            dataGridView1.Rows[i].Cells[4].Style.BackColor = System.Drawing.Color.Orange;
                            break;
                        }
                    case "Active":
                        {
                            dataGridView1.Rows[i].Cells[4].Style.BackColor = System.Drawing.Color.Cyan;
                            break;
                        }
                    case "Completed":
                        {
                            dataGridView1.Rows[i].Cells[4].Style.BackColor = System.Drawing.Color.Green;
                            break;
                        }
                }
            }
        }

        private void ServicesListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ServiceList service = new ServiceList(Id);
            service.Show();
            conn.Close();
            this.Dispose();
        }

        private void LogOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login.LogIn logIn = new Login.LogIn();
            logIn.Show();
            conn.Close();
            this.Dispose();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            conn.Close();
            this.Dispose();
            Application.Exit();
        }

        private void DataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ReColorGrid();
        }

        private void TXTToolStripMenuItem_Click(object sender, EventArgs e)
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
                    streamWriter.Write(dataGridView1.Columns[j].HeaderText + " ");
                }
                streamWriter.WriteLine();

                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                {
                    for (int i = 0; i < dataGridView1.Rows[j].Cells.Count; i++)
                    {
                        streamWriter.Write(dataGridView1.Rows[j].Cells[i].Value + " ");
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

        private void PDFToolStripMenuItem_Click(object sender, EventArgs e)
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
                string sql = "SELECT Services.Service_name, Services.Service_price, Orders.Order_date, Orders.Descriptions, Status.Status_name FROM Orders INNER JOIN Status on(Status.Status_id = Orders.Status_id) INNER JOIN Services on(Services.Service_id= Orders.Service_id) WHERE Orders.User_id = " + Id + "; ";
                MySqlDataAdapter mda = new MySqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                mda.Fill(ds, "order");

                iTextSharp.text.Document doc = new iTextSharp.text.Document();

                PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));

                doc.Open();

                BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);

                PdfPTable table = new PdfPTable(ds.Tables["order"].Columns.Count);

                PdfPCell cell = new PdfPCell(new Phrase(" " + "User orders" + " ", font))
                {
                    Colspan = ds.Tables["order"].Columns.Count,
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

                for (int j = 0; j < ds.Tables["order"].Rows.Count; j++)
                {
                    for (int k = 0; k < ds.Tables["order"].Columns.Count; k++)
                    {
                        table.AddCell(new Phrase(ds.Tables["order"].Rows[j][k].ToString(), font));
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

        private void XLSToolStripMenuItem_Click(object sender, EventArgs e)
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
                    Name = "Detail_order"
                };
                sheets.Append(sheet);
                spreadsheetDocument.Close();

                Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                ExcelApp.Application.Workbooks.Open(path);
                ExcelApp.Columns.ColumnWidth = 15;

                ExcelApp.Cells[1, 1] = "Service";
                ExcelApp.Cells[1, 2] = "Price";
                ExcelApp.Cells[1, 3] = "Date";
                ExcelApp.Cells[1, 4] = "Descriptions";
                ExcelApp.Cells[1, 5] = "Status";

                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    for (int j = 0; j < dataGridView1.RowCount - 1; j++)
                    {
                        ExcelApp.Cells[j + 2, i + 1] = (dataGridView1.Rows[j].Cells[i].Value).ToString();
                    }
                }
                ExcelApp.Quit();
            }
            catch (Exception)
            {

            }
        }

        private void PRINTToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void NewOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewOrder newOrder = new NewOrder(Id);
            newOrder.Show();
            conn.Close();
            this.Dispose();
        }
    }
}
