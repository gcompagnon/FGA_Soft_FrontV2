using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ChartView;
using Telerik.Windows.Controls.GridView;

namespace FrontV2.Utilities
{
    public static class Helpers
    {
        public static String DisplayRoundedNumber(String numberAsString, int nbAfterDot, char separator = '.')
        {
            int dotPos = numberAsString.IndexOf(separator);
            if (dotPos == -1)
                return numberAsString;
            else if (nbAfterDot + dotPos + 1 >= numberAsString.Length)
                return numberAsString;
            else
                return numberAsString.Substring(0, nbAfterDot + dotPos + 1);
        }

        public static void ExportDataTableToPDFTableRecommandation(SaveFileDialog saveFileDialog1,
            String univers, String sector, DataTable dt)
        {
            FileStream fs = new FileStream(saveFileDialog1.FileName + ".pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            Document doc = new Document(iTextSharp.text.PageSize.A4, 40, 40, 40, 10);
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            writer.CloseStream = false;
            doc.Open();

            try
            {
                doc.Open();

                iTextSharp.text.Font font12Bold =
                    new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.UNDEFINED, 10f,
                        iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font font12Normal =
                    new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.UNDEFINED, 10f,
                        iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                PdfPTable PdfTable = new PdfPTable(1);

                if (dt != null)
                {
                    //Now add the data from datatable to pdf table
                    for (int rows = 0; rows <= dt.Rows.Count - 1; rows++)
                    {
                        String sentence = dt.Rows[rows][0].ToString() +
                            " | " + dt.Rows[rows][1].ToString() +
                            " | " + dt.Rows[rows][2].ToString() + " | ";
                        for (int column = 4; column < dt.Columns.Count; column++)
                        {
                            if (column == 7)
                            {
                                sentence += " | ";
                            }
                            sentence += dt.Columns[column].ToString() + ": " +
                                 dt.Rows[rows][column].ToString();

                            if (column < 6)
                                sentence += ", ";
                        }
                        RichTextBox b = new RichTextBox();
                        MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(dt.Rows[rows][3].ToString()));
                        b.Selection.Load(stream, DataFormats.Rtf);

                        PdfPCell cell = new PdfPCell(new Phrase(sentence, font12Bold));
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        PdfTable.AddCell(cell);

                        PdfTable.AddCell(new PdfPCell(new Phrase(b.Selection.Text, font12Normal)));

                    }
                    //Adding pdftable to the pdfdocument
                    doc.Add(PdfTable);
                    //pcb.AddTemplate(Background, 0, 0)
                }
                doc.Close();
                System.Diagnostics.Process.Start(saveFileDialog1.FileName + ".pdf");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                doc.Close();
                fs.Close();
                Process.Start(saveFileDialog1.FileName + ".pdf");
            }
        }

        public static void OpenExcel(String sector, String industry, String libelle)
        {
            if (libelle.ToLower().Contains(" class ") || libelle.ToLower().Contains(" pref"))
            {
                String[] tab = libelle.Split(' ');
                libelle = "";
                bool start = true;

                foreach (String s in tab)
                {
                    if (s == "CLASS")
                        break;
                    if (s == "PREF")
                        break;
                    if (!start)
                        libelle += " " + s;
                    else
                        libelle = s;
                    start = false;
                }
            }

            String filepath = @"\\mede1\partage\,FGA Front Office\02_Gestion_Actions\00_BASE\01_FICHES\01_VALEURS\";
            filepath += sector + @"\" + industry + @"\" + libelle + ".xlsm";

            String defaultPath = @"\\mede1\partage\,FGA Front Office\02_Gestion_Actions\00_BASE\01_FICHES\01_VALEURS\Snapshot.xltm";

            FileInfo fi = new FileInfo(filepath);
            if (fi.Exists)
                System.Diagnostics.Process.Start(filepath);
            else
                System.Diagnostics.Process.Start(defaultPath);
        }

        public static void OpenPDF(String sector, String industry, String libelle)
        {
            if (libelle.ToLower().Contains(" class ") || libelle.ToLower().Contains(" pref"))
            {
                String[] tab = libelle.Split(' ');
                libelle = "";
                bool start = true;

                foreach (String s in tab)
                {
                    if (s == "CLASS")
                        break;
                    if (s == "PREF")
                        break;
                    if (!start)
                        libelle += " " + s;
                    else
                        libelle = s;
                    start = false;
                }
            }
            String filepath = @"\\mede1\partage\,FGA Front Office\02_Gestion_Actions\00_BASE\01_FICHES\01_VALEURS\";
            filepath += sector + @"\" + industry + @"\pdf\" + libelle + ".pdf";

            FileInfo fi = new FileInfo(filepath);
            if (fi.Exists)
                System.Diagnostics.Process.Start(filepath);
            else
                MessageBox.Show("Pas de fiche PDF pour: \n'" + libelle + "'.");
        }

        public static void ToBloomberg(String ticker, String indice)
        {
            if (ticker == null || ticker == "")
                return;
            try
            {               
                var excelApp = new Microsoft.Office.Interop.Excel.Application();
                int nChannel;

                String connection;
                if (indice == "GR_EUROPE")
                    connection = @"<blp-1>" + ticker + "<Equity>MXEU<Index>GR YTD<GO>";
                else if (indice == "GR_USA")
                    connection = @"<blp-1>" + ticker + "<Equity>MXUSLC<Index>GR YTD<GO>";
                else
                    connection = @"<blp-1>" + ticker + "<Equity>" + indice + "<GO>";

                nChannel = excelApp.DDEInitiate("winblp", "bbk");
                excelApp.DDEExecute(nChannel, connection);
                excelApp.DDETerminate(nChannel);
                excelApp.Quit();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public static void ExportToBMP(String nom, RadChartBase chart)
        {
            String origin = @"\\mede1\partage\,FGA Front Office\02_Gestion_Actions\00_BASE\04_TEMP\";
            String pathTmpFolder = origin + nom + ".bmp";

            Telerik.Windows.Media.Imaging.ExportExtensions.ExportToImage(chart, pathTmpFolder, new PngBitmapEncoder());

            System.Diagnostics.Process.Start("mspaint", "\"" + pathTmpFolder + "\"");
        }

        public static void ExportToBMP(String nom, System.Windows.Controls.GroupBox box)
        {
            String origin = @"\\mede1\partage\,FGA Front Office\02_Gestion_Actions\00_BASE\04_TEMP\";
            String pathTmpFolder = origin + nom + ".bmp";

            Telerik.Windows.Media.Imaging.ExportExtensions.ExportToImage(box, pathTmpFolder, new PngBitmapEncoder());

            System.Diagnostics.Process.Start("mspaint", "\"" + pathTmpFolder + "\"");
        }

        public static void ExportToBMP(String nom, Panel panel)
        {
            String origin = @"\\mede1\partage\,FGA Front Office\02_Gestion_Actions\00_BASE\04_TEMP\";
            String pathTmpFolder = origin + nom + ".bmp";

            Telerik.Windows.Media.Imaging.ExportExtensions.ExportToImage(panel, pathTmpFolder, new PngBitmapEncoder());

            System.Diagnostics.Process.Start("mspaint", "\"" + pathTmpFolder + "\"");
        }

        public static void AddToolTips(GridViewDataColumn column)
        {
            // Attention a conserver l'ordre de test pricnipalement pour 
            // AVEURO/AVEUOPE et MXEU/MXEUM

            if (column.Header.ToString().Contains("6100024"))
            {
                var style = new Style(typeof(GridViewHeaderCell));
                style.Setters.Add(new Setter(ToolTipService.ToolTipProperty, "FEDERIS NORTH AMERICA"));
                column.HeaderCellStyle = style;
            }
            else if (column.Header.ToString().Contains("6100026"))
            {
                var style = new Style(typeof(GridViewHeaderCell));
                style.Setters.Add(new Setter(ToolTipService.ToolTipProperty, "FEDERIS EUROPE ACTIONS"));
                column.HeaderCellStyle = style;
            }
            else if (column.Header.ToString().Contains("6100062"))
            {
                var style = new Style(typeof(GridViewHeaderCell));
                style.Setters.Add(new Setter(ToolTipService.ToolTipProperty, "FEDERIS EX EURO"));
                column.HeaderCellStyle = style;
            }
            else if (column.Header.ToString().Contains("6100030"))
            {
                var style = new Style(typeof(GridViewHeaderCell));
                style.Setters.Add(new Setter(ToolTipService.ToolTipProperty, "FEDERIS EURO ACTIONS"));
                column.HeaderCellStyle = style;
            }
            else if (column.Header.ToString().Contains("AVEUROPE"))
            {
                var style = new Style(typeof(GridViewHeaderCell));
                style.Setters.Add(new Setter(ToolTipService.ToolTipProperty, "FEDERIS VALUE EURO"));
                column.HeaderCellStyle = style;
            }
            else if (column.Header.ToString().Contains("AVEURO"))
            {
                var style = new Style(typeof(GridViewHeaderCell));
                style.Setters.Add(new Setter(ToolTipService.ToolTipProperty, "AVENIR EURO"));
                column.HeaderCellStyle = style;
            }
            else if (column.Header.ToString().Contains("6100004"))
            {
                var style = new Style(typeof(GridViewHeaderCell));
                style.Setters.Add(new Setter(ToolTipService.ToolTipProperty, "FEDERIS ISR EURO"));
                column.HeaderCellStyle = style;
            }
            else if (column.Header.ToString().Contains("6100001"))
            {
                var style = new Style(typeof(GridViewHeaderCell));
                style.Setters.Add(new Setter(ToolTipService.ToolTipProperty, "FEDERIS ACTIONS"));
                column.HeaderCellStyle = style;
            }
            else if (column.Header.ToString().Contains("6100033"))
            {
                var style = new Style(typeof(GridViewHeaderCell));
                style.Setters.Add(new Setter(ToolTipService.ToolTipProperty, "FEDERIS IRC ACTIONS"));
                column.HeaderCellStyle = style;
            }
            else if (column.Header.ToString().Contains("6100063"))
            {
                var style = new Style(typeof(GridViewHeaderCell));
                style.Setters.Add(new Setter(ToolTipService.ToolTipProperty, "FEDERIS CROISSANCE EURO"));
                column.HeaderCellStyle = style;
            }            
            else if (column.Header.ToString().Contains("6100002"))
            {
                var style = new Style(typeof(GridViewHeaderCell));
                style.Setters.Add(new Setter(ToolTipService.ToolTipProperty, "FEDERIS FRANCE ACTIONS"));
                column.HeaderCellStyle = style;
            }
            else if (column.Header.ToString().Contains("61000666"))
            {
                var style = new Style(typeof(GridViewHeaderCell));
                style.Setters.Add(new Setter(ToolTipService.ToolTipProperty, "FEDERIS ISR AMERIQUE"));
                column.HeaderCellStyle = style;
            }
            else if (column.Header.ToString().Contains("MXFR"))
            {
                var style = new Style(typeof(GridViewHeaderCell));
                style.Setters.Add(new Setter(ToolTipService.ToolTipProperty, "MSCI FRANCE"));
                column.HeaderCellStyle = style;
            }
            else if (column.Header.ToString().Contains("MXEM"))
            {
                var style = new Style(typeof(GridViewHeaderCell));
                style.Setters.Add(new Setter(ToolTipService.ToolTipProperty, "MSCI EMU"));
                column.HeaderCellStyle = style;
            }
            else if (column.Header.ToString().Contains("MXEUM"))
            {
                var style = new Style(typeof(GridViewHeaderCell));
                style.Setters.Add(new Setter(ToolTipService.ToolTipProperty, "MSCI EUROPE EX EMU"));
                column.HeaderCellStyle = style;
            }
            else if (column.Header.ToString().Contains("MXEU"))
            {
                var style = new Style(typeof(GridViewHeaderCell));
                style.Setters.Add(new Setter(ToolTipService.ToolTipProperty, "MSCI EUROPE"));
                column.HeaderCellStyle = style;
            }
            
            else if (column.Header.ToString().Contains("MXUSLC"))
            {
                var style = new Style(typeof(GridViewHeaderCell));
                style.Setters.Add(new Setter(ToolTipService.ToolTipProperty, "MSCI USA LC"));
                column.HeaderCellStyle = style;
            }
        }

    }
}