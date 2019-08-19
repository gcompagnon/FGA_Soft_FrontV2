using FrontV2.Action.Reco.Model;
using FrontV2.Utilities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Data;

namespace FrontV2.Action.Reco.ViewModel
{
    class RecoViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Constructor

        public RecoViewModel()
        {
            _supersectors.Add("");
            foreach (String s in _model.GetSectorsICB())
                _supersectors.Add(s);

            SelectedUniverse = "Europe";
        }

        #endregion

        #region Fields

        private bool _isBusy;
        private string _busyContent;

        private readonly RecoModel _model = new RecoModel();
        private CompanyNameCleaner cleaner = new CompanyNameCleaner();

        private RadObservableCollection<String> _univers = new RadObservableCollection<String> { "Europe", "USA" };
        private String _selectedUniverse;
        private RadObservableCollection<String> _supersectors = new RadObservableCollection<String>();
        private String _selectedSuperSector;
        private RadObservableCollection<String> _sectors = new RadObservableCollection<String>();
        private String _selectedSector;

        private DataTable _valuesDataSource;
        private DataTable _histoDataSource;

        private bool _allRestrictionChecked = true;
        private bool _industriesRestrictionChecked;
        private bool _valuesRestrictionChecked;

        #endregion

        #region Properties

        public RadObservableCollection<String> Univers
        {
            get
            {
                return _univers;
            }
            set
            {
                _univers = value;
                OnPropertyChanged("Univers");
            }
        }
        public String SelectedUniverse
        {
            get
            {
                return _selectedUniverse;
            }
            set
            {
                _selectedUniverse = value;
                OnPropertyChanged("SelectedUniverse");
                _sectors.Clear();
                foreach (String s in _model.GetFGAIndustries(_selectedSuperSector, _selectedUniverse))
                    _sectors.Add(s);
            }
        }

        public RadObservableCollection<String> SuperSectors
        {
            get
            {
                return _supersectors;
            }
            set
            {
                _supersectors = value;
                OnPropertyChanged("SuperSectors");
            }
        }
        public String SelectedSuperSector
        {
            get
            {
                return _selectedSuperSector;
            }
            set
            {
                _selectedSuperSector = value;
                OnPropertyChanged("SelectedSuperSector");
                _sectors.Clear();
                foreach (String s in _model.GetFGAIndustries(_selectedSuperSector, _selectedUniverse))
                    _sectors.Add(s);
            }
        }

        public RadObservableCollection<String> Sectors
        {
            get
            {
                return _sectors;
            }
            set
            {
                _sectors = value;
                OnPropertyChanged("Sectors");
            }
        }
        public String SelectedSector
        {
            get
            {
                return _selectedSector;
            }
            set
            {
                _selectedSector = value;
                OnPropertyChanged("SelectedSector");
            }
        }

        public DataTable ValuesDataSource
        {
            get { return _valuesDataSource; }
            set
            {
                _valuesDataSource = value;
                OnPropertyChanged("ValuesDataSource");
            }
        }

        public DataTable HistoDataSource
        {
            get { return _histoDataSource; }
            set
            {
                _histoDataSource = value;
                OnPropertyChanged("HistoDataSource");
            }
        }

        public bool AllRestrictionChecked
        {
            get { return _allRestrictionChecked; }
            set
            {
                _allRestrictionChecked = value;
                OnPropertyChanged("AllRestrictionChecked");
                LoadExecute();
            }
        }
        public bool IndustriesRestrictionChecked
        {
            get { return _industriesRestrictionChecked; }
            set
            {
                _industriesRestrictionChecked = value;
                OnPropertyChanged("IndustriesRestrictionChecked");
                LoadExecute();
            }
        }
        public bool ValuesRestrictionChecked
        {
            get { return _valuesRestrictionChecked; }
            set
            {
                _valuesRestrictionChecked = value;
                OnPropertyChanged("ValuesRestrictionChecked");
                LoadExecute();
            }
        }

        public bool IsBusy
        {
            get { return this._isBusy; }
            set
            {
                if (this._isBusy != value)
                {
                    this._isBusy = value;
                    this.OnPropertyChanged("IsBusy");

                    if (this._isBusy)
                    {
                        var backgroundWorker = new BackgroundWorker();
                        backgroundWorker.DoWork += this.OnBackgroundWorkerDoWork;
                        backgroundWorker.RunWorkerCompleted += OnBackgroundWorkerRunWorkerCompleted;
                        backgroundWorker.RunWorkerAsync();
                    }
                }
            }
        }

        public string BusyContent
        {
            get { return this._busyContent; }
            set
            {
                if (this._busyContent != value)
                {
                    this._busyContent = value;
                    this.OnPropertyChanged("BusyContent");
                }
            }
        }

        #endregion

        #region Command

        public void LoadExecute()
        {
            IsBusy = true;
        }

        #endregion

        public void format()
        {
            DateTimeFormatInfo frFormat = new CultureInfo("fr-FR", false).DateTimeFormat;

            foreach (DataRow row in _valuesDataSource.Rows)
            {
                row["Secteur FGA"] = row["Secteur FGA"].ToString().TrimEnd();
                if (row["Date"] != DBNull.Value)
                    row["Date"] = Convert.ToDateTime(row["Date"], frFormat);

                if (row["Note Quant"].ToString() != "")
                    row["Note Quant"] = Double.Parse(Helpers.DisplayRoundedNumber(row["Note Quant"].ToString(), 2, ','));

                if (SelectedUniverse == "Europe")
                {
                    if (row["Poids MXEU"].ToString() != "")
                        row["Poids MXEU"] = Double.Parse(Helpers.DisplayRoundedNumber(row["Poids MXEU"].ToString(), 2));
                    if (row["Poids MXEUM"].ToString() != "")
                        row["Poids MXEUM"] = Double.Parse(Helpers.DisplayRoundedNumber(row["Poids MXEUM"].ToString(), 2));
                    if (row["Poids MXEM"].ToString() != "")
                        row["Poids MXEM"] = Double.Parse(Helpers.DisplayRoundedNumber(row["Poids MXEM"].ToString(), 2));
                }
                else if (SelectedUniverse == "USA")
                {
                    if (row["Poids MXUSLC"].ToString() != "")
                        row["Poids MXUSLC"] = Double.Parse(Helpers.DisplayRoundedNumber(row["Poids MXUSLC"].ToString(), 2));
                }
            }
            ValuesDataSource = _valuesDataSource;
        }

        public void FillHistoDataSource(String isin, String id_sector_fga, String sector_fga, String libelle)
        {
            HistoDataSource = _model.GetHistoData(isin, id_sector_fga, sector_fga, libelle);
            HistoDataSource.DefaultView.Sort = "Date DESC";
        }

        public void FillHistoDataSourceSector(String sectorFGA, String sectorICB, String id_fga, String id_icb)
        {
            HistoDataSource = _model.GetHistoDataSector(SelectedUniverse.ToString(), sectorFGA, sectorICB, id_fga, id_icb);
            HistoDataSource.DefaultView.Sort = "Date DESC";
        }

        public object FillRecoTextBox(String id)
        {
            return _model.GetRecommandationText(id);
        }

        public void UpdateRecoValue(int id, String newDate,
            String newRecoMXEU, String newRecoMXEUM, String newRecoMXEM, String newRecoMXUSLC)
        {
            _model.UpdateRecoValue(id, newDate, newRecoMXEU, newRecoMXEUM, newRecoMXEM, newRecoMXUSLC);
        }

        public void UpdateRecoSector(String prevDate, String prevType, int id,
            String newDate, String newRecoMXEU, String newRecoMXEUM, String newRecoMXEM, String newRecoMXUSLC)
        {
            _model.UpdateRecoSector(prevDate, prevType, id, newDate, newRecoMXEU, newRecoMXEUM, newRecoMXEM, newRecoMXUSLC);
        }

        public void DeleteRecoValeur(int id_valeur, int id_comment, int id_comment_change)
        {
            _model.DeleteRecoValeur(id_valeur, id_comment, id_comment_change);
        }

        public void DeleteRecoSector(String prevDate, String prevType, int id_sector,
            int id_comment, int id_comment_change)
        {
            _model.DeleteRecoSector(prevDate, prevType, id_sector, id_comment, id_comment_change);
        }

        #region Extraction

        public void ExtractRecoMethod(RadGridView grid)
        {
            String format = "yyyy-MM-dd";
            String dateFile = DateTime.Now.ToString(format);

            String formatTitre = "dd-MM-yyyy";
            String dateTitre = DateTime.Now.ToString(formatTitre);

            String filepath = @"\\vill1\partage\,FGA Front Office\02_Gestion_Actions\00_BASE\03_LISTES ARGUMENTAIRES\";

            String country = "EUROPE_";
            if (!GlobalInfos.isEurope)
                country = "USA_";

            String filename = "ARG_" + country + dateFile;

            if (GlobalInfos.areEnsemble)
                filename += "_ENSEMBLE";
            if (GlobalInfos.areIndustries)
                filename += "_INDUSTRIES";
            if (GlobalInfos.areValues)
                filename += "_VALEURS";

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
                PdfPTable table = new PdfPTable(1);

                #endregion

                #region Title
                String titre = "Argumentaires Europe ";
                if (!GlobalInfos.isEurope)
                    titre = "Argumentaires USA ";
                PdfPCell cell = new PdfPCell(new Phrase(titre + dateTitre, arialTitle));
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Colspan = 14;
                cell.HorizontalAlignment = 1; //0=Left, 1=center, 2=right
                table.AddCell(cell);

                #endregion

                var rows = grid.ChildrenOfType<GridViewRow>();
                foreach (GridViewRow row in rows)
                {
                    #region Fields

                    String secteur = ((TextBlock)row.Cells[1].Content).Text; //Label_Secteur
                    String initiales = ((TextBlock)row.Cells[2].Content).Text; ; //Initiales
                    String industry = ((TextBlock)row.Cells[3].Content).Text; //Label_Industry
                    String ticker = ((TextBlock)row.Cells[4].Content).Text; //TICKER
                    String name = ((TextBlock)row.Cells[5].Content).Text; //Company_Name
                    String crncy = ((TextBlock)row.Cells[6].Content).Text; ; //Currency
                    String classement = ((TextBlock)row.Cells[7].Content).Text; //Q
                    String date = ((TextBlock)row.Cells[8].Content).Text; //Date
                    String recoMXEM = "";
                    String recoMXEU = "";
                    String recoMXEUM = "";
                    String recoMXUSLC = "";
                    if (GlobalInfos.isEurope)
                    {
                        recoMXEM = ((TextBlock)row.Cells[9].Content).Text; //RecoMXEM
                        recoMXEU = ((TextBlock)row.Cells[11].Content).Text; //RecoMXEU
                        recoMXEUM = ((TextBlock)row.Cells[13].Content).Text; //RecoMXEUM
                    }
                    else
                        recoMXUSLC = ((TextBlock)row.Cells[9].Content).Text; //RecoMXEUM

                    #endregion

                    if (name == "" && industry == "")
                        name = "Secteur";
                    if (name == "")
                        name = "Industrie";

                    PdfPCell nameCell = new PdfPCell(new Phrase(name, arialBold));
                    nameCell.Colspan = 1;
                    nameCell.HorizontalAlignment = 0; //0=Left, 1=center, 2=right
                    nameCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(nameCell);

                    String infos = secteur + " | " + industry + " | " + ticker + " | " + crncy + " | " + initiales;
                    PdfPCell infoCell = new PdfPCell(new Phrase(infos, arial));
                    infoCell.Colspan = 1;
                    infoCell.HorizontalAlignment = 0; //0=Left, 1=center, 2=right
                    infoCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(infoCell);

                    String recoInfo;
                    if (GlobalInfos.isEurope)
                    {
                        recoInfo = "Classement: " + classement + " | Date: " + date
                            + " | MXEM: " + recoMXEM + " | MXEU: " + recoMXEU + " | MXEUM: " + recoMXEUM;
                    }
                    else
                    {
                        recoInfo = "Classement: " + classement + " | Date: " + date
                           + " | MXUSLC: " + recoMXUSLC;
                    }

                    PdfPCell recosCell = new PdfPCell(new Phrase(recoInfo, arial));
                    recosCell.Colspan = 1;
                    recosCell.HorizontalAlignment = 0; //0=Left, 1=center, 2=right
                    recosCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(recosCell);

                    if (date != "")
                    {
                        System.Windows.Forms.RichTextBox rtBox = new System.Windows.Forms.RichTextBox();

                        if (ticker != "")
                        {
                            String recoText = _model.GetRecoTextVal(ticker, date);
                            rtBox.Rtf = recoText;
                            string plainText = rtBox.Text;
                            if (plainText == "")
                                plainText = "Pas de commentaire";
                            PdfPCell reco = new PdfPCell(new Phrase(plainText, arial));
                            reco.Colspan = 14;
                            reco.HorizontalAlignment = 0; //0=Left, 1=center, 2=right
                            table.AddCell(reco);
                        }
                        else
                        {
                            bool isGICS = industry == "" ? true : false;
                            String label = isGICS ? secteur : industry;

                            String recoText = _model.GetRecoTextSec(label, date, isGICS);
                            rtBox.Rtf = recoText;
                            string plainText = rtBox.Text;
                            if (plainText == "")
                                plainText = "Pas de commentaire";
                            PdfPCell reco = new PdfPCell(new Phrase(plainText, arial));
                            reco.Colspan = 14;
                            reco.HorizontalAlignment = 0; //0=Left, 1=center, 2=right
                            table.AddCell(reco);
                        }
                    }
                    else
                    {
                        PdfPCell reco = new PdfPCell(new Phrase("Aucun texte disponible", arial));
                        reco.Colspan = 14;
                        reco.HorizontalAlignment = 0; //0=Left, 1=center, 2=right
                        table.AddCell(reco);
                    }

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

        public void ExtractForPrintRecoMethod(RadGridView grid)
        {
            String format = "yyyy-MM-dd";
            String dateFile = DateTime.Now.ToString(format);

            String formatTitre = "dd-MM-yyyy";
            String dateTitre = DateTime.Now.ToString(formatTitre);

            String filepath = @"\\vill1\partage\,FGA Front Office\02_Gestion_Actions\00_BASE\03_LISTES RECOMMANDATIONS\";

            String country = "EUROPE_";
            if (!GlobalInfos.isEurope)
                country = "USA_";

            String filename = "RECO_" + country + dateFile;

            if (GlobalInfos.areEnsemble)
                filename += "_ENSEMBLE";
            if (GlobalInfos.areIndustries)
                filename += "_INDUSTRIES";
            if (GlobalInfos.areValues)
                filename += "_VALEURS";

            String pathToFile = filepath + filename + ".pdf";

            try
            {
                #region Fonts

                Font arial = new Font(FontFactory.GetFont("Arial", 8, Font.NORMAL));
                Font arialBold = new Font(FontFactory.GetFont("Arial", 8, Font.BOLD));
                Font arialTitle = new Font(FontFactory.GetFont("Arial", 14, Font.BOLD));

                #endregion

                #region Define document

                Document newPdfDoc = new Document(iTextSharp.text.PageSize.A4, -50, -50, 10, 10);
                PdfWriter wri = PdfWriter.GetInstance(newPdfDoc,
                    new FileStream(pathToFile, FileMode.OpenOrCreate));
                newPdfDoc.Open();
                int size = GlobalInfos.isEurope ? 14 : 10;
                float[] scheduleTableColumnWidths = new float[size];

                scheduleTableColumnWidths[0] = 130; //Label_Secteur
                scheduleTableColumnWidths[1] = 40;  //Initiales
                scheduleTableColumnWidths[2] = 130; //Label_Industry
                scheduleTableColumnWidths[3] = 100;  //TICKER
                scheduleTableColumnWidths[4] = 150; //Company_Name
                scheduleTableColumnWidths[5] = 60;  //Currency
                scheduleTableColumnWidths[6] = 25;  //Q
                scheduleTableColumnWidths[7] = 110;  //Date
                scheduleTableColumnWidths[8] = 70;  //RecoMXEM
                scheduleTableColumnWidths[9] = 70;  //PoidsMXEM
                if (GlobalInfos.isEurope)
                {
                    scheduleTableColumnWidths[10] = 70; //RecoMXEU
                    scheduleTableColumnWidths[11] = 70; //PoidsMXEU
                    scheduleTableColumnWidths[12] = 70; //RecoMXEUM
                    scheduleTableColumnWidths[13] = 70; //PoidsMXEUM
                }

                PdfPTable table = new PdfPTable(scheduleTableColumnWidths);
                table.DefaultCell.Border = Rectangle.NO_BORDER;
                table.HeaderRows = 3;

                #endregion

                var rows = grid.ChildrenOfType<GridViewRow>();

                //Alignement --- 0=Left, 1=center, 2=right
                String titre = "Recommandations Europe ";
                if (!GlobalInfos.isEurope)
                    titre = "Recommandations USA ";
                PdfPCell cell = new PdfPCell(new Phrase(titre + dateTitre, arialTitle)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, Colspan = 14, HorizontalAlignment = 1 };
                table.AddCell(cell);

                table.AddCell(new PdfPCell(new Phrase("Secteur", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, Rowspan = 2, HorizontalAlignment = 1, VerticalAlignment = 1 }); //Label_Secteur
                table.AddCell(new PdfPCell(new Phrase("##", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, Rowspan = 2, HorizontalAlignment = 1, VerticalAlignment = 1 }); //Initiales
                table.AddCell(new PdfPCell(new Phrase("Industrie", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, Rowspan = 2, HorizontalAlignment = 1, VerticalAlignment = 1 }); //Label_Industry
                table.AddCell(new PdfPCell(new Phrase("Ticker", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, Rowspan = 2, HorizontalAlignment = 1, VerticalAlignment = 1 }); //TICKER
                table.AddCell(new PdfPCell(new Phrase("Compagnie", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, Rowspan = 2, HorizontalAlignment = 1, VerticalAlignment = 1 }); //Company_Name
                table.AddCell(new PdfPCell(new Phrase("Dev", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, Rowspan = 2, HorizontalAlignment = 1, VerticalAlignment = 1 }); //Currency
                table.AddCell(new PdfPCell(new Phrase("Q", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, Rowspan = 2, HorizontalAlignment = 1, VerticalAlignment = 1 }); //Q
                table.AddCell(new PdfPCell(new Phrase("Date", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, Rowspan = 2, HorizontalAlignment = 1, VerticalAlignment = 1 }); //Date

                if (GlobalInfos.isEurope)
                {
                    table.AddCell(new PdfPCell(new Phrase("EMU", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, Colspan = 2, Rowspan = 1, HorizontalAlignment = 1, VerticalAlignment = 1 });
                    table.AddCell(new PdfPCell(new Phrase("EUROPE", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, Colspan = 2, Rowspan = 1, HorizontalAlignment = 1, VerticalAlignment = 1 });
                    table.AddCell(new PdfPCell(new Phrase("EX EMU", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, Colspan = 2, Rowspan = 1, HorizontalAlignment = 1, VerticalAlignment = 1 });
                }
                else
                    table.AddCell(new PdfPCell(new Phrase("USA", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, Colspan = 2, Rowspan = 1, HorizontalAlignment = 1, VerticalAlignment = 1 });

                table.AddCell(new PdfPCell(new Phrase("Reco", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, Rowspan = 1, HorizontalAlignment = 1, VerticalAlignment = 1 }); //RecoMXEM
                table.AddCell(new PdfPCell(new Phrase("Pds", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, Rowspan = 1, HorizontalAlignment = 1, VerticalAlignment = 1 }); //PoidsMXEM
                if (GlobalInfos.isEurope)
                {
                    table.AddCell(new PdfPCell(new Phrase("Reco", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, Rowspan = 1, HorizontalAlignment = 1, VerticalAlignment = 1 }); //RecoMXEU
                    table.AddCell(new PdfPCell(new Phrase("Pds", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, Rowspan = 1, HorizontalAlignment = 1, VerticalAlignment = 1 }); //PoidsMXEU
                    table.AddCell(new PdfPCell(new Phrase("Reco", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, Rowspan = 1, HorizontalAlignment = 1, VerticalAlignment = 1 }); //RecoMXEUM
                    table.AddCell(new PdfPCell(new Phrase("Pds", arialBold)) { Border = Rectangle.NO_BORDER, BackgroundColor = BaseColor.LIGHT_GRAY, Rowspan = 1, HorizontalAlignment = 1, VerticalAlignment = 1 }); ; //PoidsMXEUM
                }
                bool pos = true;
                foreach (GridViewRow row in rows)
                {
                    #region Fields

                    String secteur = ((TextBlock)row.Cells[1].Content).Text.Replace("Telecommunication", "Telecom"); //Label_Secteur
                    String initiales = ((TextBlock)row.Cells[2].Content).Text; ; //Initiales
                    String industry = ((TextBlock)row.Cells[3].Content).Text.Replace("Telecommunication", "Telecom"); //Label_Industry
                    String ticker = ((TextBlock)row.Cells[4].Content).Text; //TICKER
                    String name = ((TextBlock)row.Cells[5].Content).Text; //Company_Name
                    String crncy = ((TextBlock)row.Cells[6].Content).Text; ; //Currency
                    String classement = ((TextBlock)row.Cells[7].Content).Text; //Q
                    String date = ((TextBlock)row.Cells[8].Content).Text; //Date

                    String recoMXEM = ""; //RecoMXEM
                    String recoMXEU = ""; //RecoMXEU
                    String recoMXEUM = ""; //RecoMXEUM
                    String recoMXUSLC = ""; //RecoMXEUM

                    String poidsMXEM = ""; //PoidsMXEM
                    String poidsMXEU = ""; //PoidsMXEU
                    String poidsMXEUM = ""; //PoidsMXEUM
                    String poidsMXUSLC = ""; //RecoMXEUM


                    if (GlobalInfos.isEurope)
                    {
                        recoMXEM = ((TextBlock)row.Cells[9].Content).Text; //RecoMXEM
                        recoMXEU = ((TextBlock)row.Cells[11].Content).Text; //RecoMXEU
                        recoMXEUM = ((TextBlock)row.Cells[13].Content).Text; //RecoMXEUM

                        poidsMXEM = ((TextBlock)row.Cells[10].Content).Text; //PoidsMXEM
                        poidsMXEU = ((TextBlock)row.Cells[12].Content).Text; //PoidsMXEU
                        poidsMXEUM = ((TextBlock)row.Cells[14].Content).Text; //PoidsMXEUM
                    }
                    else
                    {
                        recoMXUSLC = ((TextBlock)row.Cells[9].Content).Text;
                        poidsMXUSLC = ((TextBlock)row.Cells[10].Content).Text;
                    }


                    BaseColor colorAltern = (GlobalInfos.areValues) ? (pos ? new BaseColor(255, 255, 255) : new BaseColor(230, 230, 230)) : BaseColor.WHITE;
                    BaseColor colorSector = new BaseColor(210, 180, 140);
                    BaseColor colorIndustry = new BaseColor(238, 232, 170);

                    BaseColor fontColorMXEM = (recoMXEM == "+" || recoMXEM == "++") ? new BaseColor(50, 155, 61) : ((recoMXEM == "-" || recoMXEM == "--") ? new BaseColor(236, 5, 0) : BaseColor.BLACK);
                    BaseColor fontColorMXEU = (recoMXEU == "+" || recoMXEU == "++") ? new BaseColor(50, 155, 61) : ((recoMXEU == "-" || recoMXEU == "--") ? new BaseColor(236, 5, 0) : BaseColor.BLACK);
                    BaseColor fontColorMXEUM = (recoMXEUM == "+" || recoMXEUM == "++") ? new BaseColor(50, 155, 61) : ((recoMXEUM == "-" || recoMXEUM == "--") ? new BaseColor(236, 5, 0) : BaseColor.BLACK); ;
                    BaseColor fontColorMXUSLC = (recoMXUSLC == "+" || recoMXUSLC == "++") ? new BaseColor(50, 155, 61) : ((recoMXUSLC == "-" || recoMXUSLC == "--") ? new BaseColor(236, 5, 0) : BaseColor.BLACK); ;

                    Font arialRecoMXEM = new Font(FontFactory.GetFont("Arial", 8, Font.NORMAL, fontColorMXEM));
                    Font arialRecoMXEU = new Font(FontFactory.GetFont("Arial", 8, Font.NORMAL, fontColorMXEU));
                    Font arialRecoMXEUM = new Font(FontFactory.GetFont("Arial", 8, Font.NORMAL, fontColorMXEUM));
                    Font arialRecoMXUSLC = new Font(FontFactory.GetFont("Arial", 8, Font.NORMAL, fontColorMXUSLC));

                    #endregion

                    table.AddCell(new PdfPCell(new Phrase(secteur, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = (industry == "" ? colorSector : (name == "" ? colorIndustry : colorAltern)) }); //Label_Secteur
                    table.AddCell(new PdfPCell(new Phrase(initiales, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = (industry == "" ? colorSector : (name == "" ? colorIndustry : colorAltern)) }); //Initiales
                    table.AddCell(new PdfPCell(new Phrase(industry, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = (industry == "" ? colorSector : (name == "" ? colorIndustry : colorAltern)) }); //Label_Industry
                    table.AddCell(new PdfPCell(new Phrase(ticker, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = (industry == "" ? colorSector : (name == "" ? colorIndustry : colorAltern)) }); //TICKER
                    table.AddCell(new PdfPCell(new Phrase(name, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = (industry == "" ? colorSector : (name == "" ? colorIndustry : colorAltern)) }); //Company_Name
                    table.AddCell(new PdfPCell(new Phrase(crncy, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = (industry == "" ? colorSector : (name == "" ? colorIndustry : colorAltern)) }); //Currency
                    table.AddCell(new PdfPCell(new Phrase(classement, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = (industry == "" ? colorSector : (name == "" ? colorIndustry : colorAltern)) }); //Q
                    table.AddCell(new PdfPCell(new Phrase(date, arial)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = (industry == "" ? colorSector : (name == "" ? colorIndustry : colorAltern)) }); //Date

                    if (GlobalInfos.isEurope)
                    {
                        table.AddCell(new PdfPCell(new Phrase(recoMXEM, arialRecoMXEM)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = (industry == "" ? colorSector : (name == "" ? colorIndustry : colorAltern)) }); //RecoMXEM
                        table.AddCell(new PdfPCell(new Phrase(poidsMXEM, arialRecoMXEM)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = (industry == "" ? colorSector : (name == "" ? colorIndustry : colorAltern)) }); //PoidsMXEM
                        table.AddCell(new PdfPCell(new Phrase(recoMXEU, arialRecoMXEU)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = (industry == "" ? colorSector : (name == "" ? colorIndustry : colorAltern)) }); //RecoMXEU
                        table.AddCell(new PdfPCell(new Phrase(poidsMXEU, arialRecoMXEU)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = (industry == "" ? colorSector : (name == "" ? colorIndustry : colorAltern)) }); //PoidsMXEU
                        table.AddCell(new PdfPCell(new Phrase(recoMXEUM, arialRecoMXEUM)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = (industry == "" ? colorSector : (name == "" ? colorIndustry : colorAltern)) }); //RecoMXEUM
                        table.AddCell(new PdfPCell(new Phrase(poidsMXEUM, arialRecoMXEUM)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = (industry == "" ? colorSector : (name == "" ? colorIndustry : colorAltern)) }); //PoidsMXEUM
                    }
                    else
                    {
                        table.AddCell(new PdfPCell(new Phrase(recoMXUSLC, arialRecoMXUSLC)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = (industry == "" ? colorSector : (name == "" ? colorIndustry : colorAltern)) });
                        table.AddCell(new PdfPCell(new Phrase(poidsMXUSLC, arialRecoMXUSLC)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 1, VerticalAlignment = 1, BackgroundColor = (industry == "" ? colorSector : (name == "" ? colorIndustry : colorAltern)) });
                    }

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

        #endregion

        #region BackgroundWorker

        private void OnBackgroundWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            backgroundWorker.DoWork -= this.OnBackgroundWorkerDoWork;
            backgroundWorker.RunWorkerCompleted -= OnBackgroundWorkerRunWorkerCompleted;

            InvokeOnUIThread(() =>
            {
                this.IsBusy = false;
            });
        }

        private void OnBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            ProceedDatas();
        }

        public void ProceedDatas()
        {
            String selectedRestriction = "All";

            if (ValuesRestrictionChecked)
            {
                selectedRestriction = "Valeurs";

                GlobalInfos.areValues = true;
                GlobalInfos.areEnsemble = false;
                GlobalInfos.areIndustries = false;
            }
            else if (IndustriesRestrictionChecked)
            {
                selectedRestriction = "Industries";

                GlobalInfos.areValues = false;
                GlobalInfos.areEnsemble = false;
                GlobalInfos.areIndustries = true;
            }
            else
            {
                selectedRestriction = "All";

                GlobalInfos.areValues = false;
                GlobalInfos.areEnsemble = true;
                GlobalInfos.areIndustries = false;
            }

            if (SelectedSuperSector != null && SelectedSuperSector != "")
            {
                GlobalInfos.areValues = false;
                GlobalInfos.areEnsemble = false;
                GlobalInfos.areIndustries = false;
            }

            ValuesDataSource = cleaner.CleanCompanyName(_model.GetValuesData(SelectedUniverse, SelectedSuperSector, SelectedSector, selectedRestriction));
            HistoDataSource = new DataTable();
        }


        #endregion

        #region INotifyPropertyChanged

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
        }

        #endregion
    }
}
