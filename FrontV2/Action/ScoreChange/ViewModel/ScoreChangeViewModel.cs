using FrontV2.Action.ScoreChange.Model;
using FrontV2.Utilities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ChartView;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Data;

namespace FrontV2.Action.ScoreChange.ViewModel
{
    class ScoreChangeViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ScoreChangeViewModel()
        {
            _model = new ScoreChangeModel();

            _changesDataSource = new DataTable();

            _dates = _model.GetDates();

            _selectedDate1 = _dates[0];
            _selectedDate2 = _dates[1];

            _categroriesSelectorQuint = new GenericDataPointBinding<KeyValuePair<String, int>, String>();
            _categroriesSelectorQuint.ValueSelector = pair => pair.Key;
            _valuesSelectorQuint = new GenericDataPointBinding<KeyValuePair<String, int>, int>();
            _valuesSelectorQuint.ValueSelector = pair => pair.Value;
        }

        #region Fields

        private ScoreChangeModel _model;

        private DataTable _changesDataSource;

        private RadObservableCollection<String> _dates;

        private String _selectedDate1;
        private String _selectedDate2;

        private RadObservableCollection<KeyValuePair<String, int>> _chartDataSource;

        private readonly GenericDataPointBinding<KeyValuePair<String, int>, String> _categroriesSelectorQuint;
        private readonly GenericDataPointBinding<KeyValuePair<String, int>, int> _valuesSelectorQuint;

        private int _interval = 10;
        #endregion

        #region Properties

        public RadObservableCollection<String> Dates
        {
            get { return _dates; }
            set
            {
                _dates = value;
                OnPropertyChanged("Dates");
            }
        }

        public DataTable ChangesDataSource
        {
            get { return _changesDataSource; }
            set
            {
                _changesDataSource = value;
                OnPropertyChanged("ChangesDataSource");
            }
        }

        public String SelectedDate1
        {
            get { return _selectedDate1; }
            set
            {
                _selectedDate1 = value;
                OnPropertyChanged("SelectedDate1");
            }
        }
        public String SelectedDate2
        {
            get { return _selectedDate2; }
            set
            {
                _selectedDate2 = value;
                OnPropertyChanged("SelectedDate2");
            }
        }

        public RadObservableCollection<KeyValuePair<String, int>> ChartDataSource
        {
            get { return _chartDataSource; }
            set
            {
                _chartDataSource = value;
                OnPropertyChanged("ChartDataSource");
            }
        }

        public GenericDataPointBinding<KeyValuePair<String, int>, String> CategoriesSelectorQuint
        {
            get
            {
                return _categroriesSelectorQuint;
            }
            private set
            {
            }
        }
        public GenericDataPointBinding<KeyValuePair<String, int>, int> ValuesSelectorQuint
        {
            get
            {
                return _valuesSelectorQuint;
            }
            private set
            {
            }
        }

        public int Interval
        {
            get { return _interval; }
            set
            {
                _interval = value;
                OnPropertyChanged("Interval");
            }
        }
        #endregion

        public void LoadChanges(bool filter1, bool isStart = false)
        {
            CompanyNameCleaner cleaner = new CompanyNameCleaner();
            DateTime dmin = DateTime.Parse(SelectedDate2);
            DateTime dmax = DateTime.Parse(SelectedDate1);
            if (dmin > dmax)
            {
                String tmp = SelectedDate2;
                SelectedDate2 = SelectedDate1;
                SelectedDate1 = tmp;
            }
            ChangesDataSource = cleaner.CleanCompanyName(_model.GetChanges(SelectedDate2, SelectedDate1, filter1), "TICKER", "COMPANY");

            if (isStart)
            {
                String name = "dailyScoreChange_" + SelectedDate1.Replace("/", "-");
                try
                {
                    ExtractTableToPDF(ChangesDataSource, name);
                }
                catch
                { }
            }
        }

        public void LoadChart(String ticker)
        {
            double chartWidth = 520;
            double labelSize = 20;
            int nbMax = (int)(chartWidth / labelSize);

            _chartDataSource = _model.GetQuintiles(ticker, SelectedDate1, SelectedDate2);

            if (_chartDataSource.Count > nbMax)
                Interval = _chartDataSource.Count / nbMax;
            else
                Interval = 1;

            ChartDataSource = _chartDataSource;
        }

        public void ExtractGridToPDF(RadGridView grid, String filename)
        {

            String formatDate = "dd-MM-yyyy";
            String dateT1 = DateTime.Parse(SelectedDate2).ToString(formatDate);
            String dateT2 = DateTime.Parse(SelectedDate1).ToString(formatDate);

            String title = "Changements de scores du " + dateT1 + " au " + dateT2;

            String filepath = @"\\mede1\partage\,FGA Front Office\02_Gestion_Actions\00_BASE\02_EVOLUTION SCORES\";
            String pathToFile = filepath + filename + ".pdf";


            try
            {
                #region Fonts

                Font arial = new Font(FontFactory.GetFont("Arial", 8, Font.NORMAL));
                Font arialBold = new Font(FontFactory.GetFont("Arial", 8, Font.BOLD));
                Font arialTitle = new Font(FontFactory.GetFont("Arial", 14, Font.BOLD));

                #endregion

                #region Define document

                Document newPdfDoc = new Document(iTextSharp.text.PageSize.A4, 0, 0, 10, 10);
                PdfWriter wri = PdfWriter.GetInstance(newPdfDoc,
                    new FileStream(pathToFile, FileMode.OpenOrCreate));
                newPdfDoc.Open();

                float[] scheduleTableColumnWidths = new float[11];

                scheduleTableColumnWidths[0] = 130; //Label_Secteur
                scheduleTableColumnWidths[1] = 130;  //Industries
                scheduleTableColumnWidths[2] = 80; //TICKER 
                scheduleTableColumnWidths[3] = 150;  //Comapny_Name
                scheduleTableColumnWidths[4] = 100; //Date1
                scheduleTableColumnWidths[5] = 60;  //Rang1
                scheduleTableColumnWidths[6] = 60;  //Quint1
                scheduleTableColumnWidths[7] = 100;  //Date2
                scheduleTableColumnWidths[8] = 60;  //Rang2
                scheduleTableColumnWidths[9] = 60;  //Quint2
                scheduleTableColumnWidths[10] = 60;  //SUIVI

                PdfPTable table = new PdfPTable(scheduleTableColumnWidths);
                table.DefaultCell.Border = Rectangle.NO_BORDER;
                table.HeaderRows = 2;

                #endregion

                #region Title

                PdfPCell cell = new PdfPCell(new Phrase(title, arialTitle));
                cell.Border = Rectangle.NO_BORDER;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Colspan = 11;
                cell.HorizontalAlignment = 1; //0=Left, 1=center, 2=right
                table.AddCell(cell);

                #endregion

                #region Headers

                table.AddCell(new PdfPCell(new Phrase("Secteur", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = 1, VerticalAlignment = 1 }); //Label_Secteur
                table.AddCell(new PdfPCell(new Phrase("Industrie", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = 1, VerticalAlignment = 1 }); //Label_Industry
                table.AddCell(new PdfPCell(new Phrase("Ticker", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = 1, VerticalAlignment = 1 }); //TICKER
                table.AddCell(new PdfPCell(new Phrase("Compagnie", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = 1, VerticalAlignment = 1 });  //Company_Name
                table.AddCell(new PdfPCell(new Phrase("Date1", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = 1, VerticalAlignment = 1 }); ; //date1
                table.AddCell(new PdfPCell(new Phrase("Rang1", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = 1, VerticalAlignment = 1 }); ; //rang1
                table.AddCell(new PdfPCell(new Phrase("Quint1", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = 1, VerticalAlignment = 1 }); //quint1
                table.AddCell(new PdfPCell(new Phrase("Date2", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = 1, VerticalAlignment = 1 }); ; //date2
                table.AddCell(new PdfPCell(new Phrase("Rang2", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = 1, VerticalAlignment = 1 }); ; //rang2
                table.AddCell(new PdfPCell(new Phrase("Quint2", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = 1, VerticalAlignment = 1 }); //quint2
                table.AddCell(new PdfPCell(new Phrase("Suivi", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = 1, VerticalAlignment = 1 }); //suivi

                #endregion

                var rows = grid.ChildrenOfType<GridViewRow>();
                bool pos = true;
                foreach (GridViewRow row in rows)
                {
                    #region Fields

                    String secteur = ((TextBlock)row.Cells[0].Content).Text; //Label_Secteur
                    String industry = ((TextBlock)row.Cells[1].Content).Text; //Label_Industry
                    String ticker = ((TextBlock)row.Cells[3].Content).Text; //TICKER
                    String name = ((TextBlock)row.Cells[4].Content).Text; //Company_Name
                    String date1 = ((TextBlock)row.Cells[5].Content).Text; //date1
                    String rang1 = ((TextBlock)row.Cells[6].Content).Text; //rang1
                    String quint1 = ((TextBlock)row.Cells[7].Content).Text; //quint1
                    String date2 = ((TextBlock)row.Cells[8].Content).Text; //date2
                    String rang2 = ((TextBlock)row.Cells[9].Content).Text; //rang2
                    String quint2 = ((TextBlock)row.Cells[10].Content).Text; //quint2
                    String suivi = ((TextBlock)row.Cells[2].Content).Text; //suivi

                    BaseColor colorAltern = pos ? new BaseColor(255, 255, 255) : new BaseColor(230, 230, 230);

                    Font arialRed = new Font(FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.RED));

                    Font usedFont = arial;
                    if (int.Parse(quint1) < int.Parse(quint2))
                        usedFont = arialRed;

                    #endregion

                    table.AddCell(new PdfPCell(new Phrase(secteur, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = colorAltern }); //Label_Secteur
                    table.AddCell(new PdfPCell(new Phrase(industry, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = colorAltern }); //Label_Industry
                    table.AddCell(new PdfPCell(new Phrase(ticker, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = colorAltern }); //TICKER
                    table.AddCell(new PdfPCell(new Phrase(name, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = colorAltern }); //Company_Name
                    table.AddCell(new PdfPCell(new Phrase(date1, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = colorAltern }); //date1
                    table.AddCell(new PdfPCell(new Phrase(rang1, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = colorAltern }); //rang1
                    table.AddCell(new PdfPCell(new Phrase(quint1, usedFont)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = colorAltern }); //quint1
                    table.AddCell(new PdfPCell(new Phrase(date2, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = colorAltern }); //date2
                    table.AddCell(new PdfPCell(new Phrase(rang2, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = colorAltern }); //rang2
                    table.AddCell(new PdfPCell(new Phrase(quint2, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = colorAltern });//quint2
                    table.AddCell(new PdfPCell(new Phrase(suivi, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = colorAltern });//quint2

                    pos = !pos;
                }
                newPdfDoc.Add(table);
                newPdfDoc.Close();

                System.Diagnostics.Process.Start(pathToFile);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void ExtractTableToPDF(DataTable dataTable, String filename)
        {

            String formatDate = "dd-MM-yyyy";
            String dateT1 = DateTime.Parse(SelectedDate2).ToString(formatDate);
            String dateT2 = DateTime.Parse(SelectedDate1).ToString(formatDate);

            String title = "Changements de scores du " + dateT1 + " au " + dateT2;

            String filepath = @"\\mede1\partage\,FGA Front Office\02_Gestion_Actions\00_BASE\02_EVOLUTION SCORES\";
            String pathToFile = filepath + filename + ".pdf";


            try
            {
                #region Fonts

                Font arial = new Font(FontFactory.GetFont("Arial", 8, Font.NORMAL));
                Font arialBold = new Font(FontFactory.GetFont("Arial", 8, Font.BOLD));
                Font arialTitle = new Font(FontFactory.GetFont("Arial", 14, Font.BOLD));

                #endregion

                #region Define document

                Document newPdfDoc = new Document(iTextSharp.text.PageSize.A4, 0, 0, 10, 10);
                PdfWriter wri = PdfWriter.GetInstance(newPdfDoc,
                    new FileStream(pathToFile, FileMode.OpenOrCreate));
                newPdfDoc.Open();

                float[] scheduleTableColumnWidths = new float[11];

                scheduleTableColumnWidths[0] = 130; //Label_Secteur
                scheduleTableColumnWidths[1] = 130;  //Industries
                scheduleTableColumnWidths[2] = 80; //TICKER 
                scheduleTableColumnWidths[3] = 150;  //Comapny_Name
                scheduleTableColumnWidths[4] = 100; //Date1
                scheduleTableColumnWidths[5] = 60;  //Rang1
                scheduleTableColumnWidths[6] = 60;  //Quint1
                scheduleTableColumnWidths[7] = 100;  //Date2
                scheduleTableColumnWidths[8] = 60;  //Rang2
                scheduleTableColumnWidths[9] = 60;  //Quint2
                scheduleTableColumnWidths[10] = 60;  //SUIVI

                PdfPTable table = new PdfPTable(scheduleTableColumnWidths);
                table.DefaultCell.Border = Rectangle.NO_BORDER;
                table.HeaderRows = 2;

                #endregion

                #region Title

                PdfPCell cell = new PdfPCell(new Phrase(title, arialTitle));
                cell.Border = Rectangle.NO_BORDER;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Colspan = 11;
                cell.HorizontalAlignment = 1; //0=Left, 1=center, 2=right
                table.AddCell(cell);

                #endregion

                #region Headers

                table.AddCell(new PdfPCell(new Phrase("Secteur", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = 1, VerticalAlignment = 1 }); //Label_Secteur
                table.AddCell(new PdfPCell(new Phrase("Industrie", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = 1, VerticalAlignment = 1 }); //Label_Industry
                table.AddCell(new PdfPCell(new Phrase("Ticker", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = 1, VerticalAlignment = 1 }); //TICKER
                table.AddCell(new PdfPCell(new Phrase("Compagnie", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = 1, VerticalAlignment = 1 });  //Company_Name
                table.AddCell(new PdfPCell(new Phrase("Date1", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = 1, VerticalAlignment = 1 }); ; //date1
                table.AddCell(new PdfPCell(new Phrase("Rang1", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = 1, VerticalAlignment = 1 }); ; //rang1
                table.AddCell(new PdfPCell(new Phrase("Quint1", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = 1, VerticalAlignment = 1 }); //quint1
                table.AddCell(new PdfPCell(new Phrase("Date2", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = 1, VerticalAlignment = 1 }); ; //date2
                table.AddCell(new PdfPCell(new Phrase("Rang2", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = 1, VerticalAlignment = 1 }); ; //rang2
                table.AddCell(new PdfPCell(new Phrase("Quint2", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = 1, VerticalAlignment = 1 }); //quint2
                table.AddCell(new PdfPCell(new Phrase("Suivi", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = 1, VerticalAlignment = 1 }); //suivi

                #endregion

                var rows = dataTable.Rows;
                bool pos = true;
                foreach (DataRow row in rows)
                {
                    #region Fields

                    String secteur = row[0].ToString(); //Label_Secteur
                    String industry = row[1].ToString(); //Label_Industry
                    String ticker = row[3].ToString(); //TICKER
                    String name = row[4].ToString(); //Company_Name
                    String date1 = row[5].ToString().Substring(0, 10); //date1
                    String rang1 = row[6].ToString(); //rang1
                    String quint1 = row[7].ToString(); //quint1
                    String date2 = row[8].ToString().Substring(0, 10); //date2
                    String rang2 = row[9].ToString(); //rang2
                    String quint2 = row[10].ToString(); //quint2
                    String suivi = row[2].ToString(); //suivi

                    BaseColor colorAltern = pos ? new BaseColor(255, 255, 255) : new BaseColor(230, 230, 230);

                    Font arialRed = new Font(FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.RED));

                    Font usedFont = arial;
                    if (int.Parse(quint1) < int.Parse(quint2))
                        usedFont = arialRed;

                    #endregion

                    table.AddCell(new PdfPCell(new Phrase(secteur, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = colorAltern }); //Label_Secteur
                    table.AddCell(new PdfPCell(new Phrase(industry, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = colorAltern }); //Label_Industry
                    table.AddCell(new PdfPCell(new Phrase(ticker, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = colorAltern }); //TICKER
                    table.AddCell(new PdfPCell(new Phrase(name, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = colorAltern }); //Company_Name
                    table.AddCell(new PdfPCell(new Phrase(date1, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = colorAltern }); //date1
                    table.AddCell(new PdfPCell(new Phrase(rang1, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = colorAltern }); //rang1
                    table.AddCell(new PdfPCell(new Phrase(quint1, usedFont)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = colorAltern }); //quint1
                    table.AddCell(new PdfPCell(new Phrase(date2, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = colorAltern }); //date2
                    table.AddCell(new PdfPCell(new Phrase(rang2, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = colorAltern }); //rang2
                    table.AddCell(new PdfPCell(new Phrase(quint2, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = colorAltern });//quint2
                    table.AddCell(new PdfPCell(new Phrase(suivi, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = colorAltern });//quint2

                    pos = !pos;
                }
                newPdfDoc.Add(table);
                newPdfDoc.Close();

                System.Diagnostics.Process.Start(pathToFile);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        #region INotifyPropertyChanged

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
        }

        #endregion
    }
}
