using FrontV2.Action.Consultation.Recommandation.Model;
using FrontV2.Action.Reco;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FrontV2.Action.Consultation.Recommandation.ViewModel
{
    class RecommandationViewModel
    {

        #region Constructor && Fields && Load

        public RecommandationViewModel() { }

        /// <summary>
        /// Load recommandation and load display to show recommandation as a 'grid' with textblock and richtextbox
        /// </summary>
        public void load()
        {
            _model = new RecommandationModel();

            Datas = _model.GetRecommandations(Date, Univers, Sector);

            string s = null;
            int i = 0;
            foreach (DataRow row in Datas.Rows)
            {
                TextBox rtb1 = new TextBox();
                rtb1.Text = row["Secteur"] + " | " + row["IndustryFGA"] + " | " + row["AssetName"];
                TextBlock rtb3 = new TextBlock();
                rtb3.Inlines.Add(row["Secteur"] + " | " + row["IndustryFGA"] + " | ");
                rtb3.Inlines.Add(row["AssetName"].ToString());
                rtb3.Inlines.Add(" | ");
                //-----------------------------------------------------------------
                if (!object.ReferenceEquals(row["MXEU"], DBNull.Value) && !string.IsNullOrEmpty(row["MXEU"].ToString()))
                {
                    rtb3.Inlines.Add("MXEU: ");
                    rtb3.Inlines.Add(row["MXEU"].ToString());
                }
                if (!object.ReferenceEquals(row["MXEM"], DBNull.Value) && !string.IsNullOrEmpty(row["MXEM"].ToString()))
                {
                    rtb3.Inlines.Add(" , MXEM: ");
                    rtb3.Inlines.Add(row["MXEM"].ToString());
                }
                if (!object.ReferenceEquals(row["MXEUM"], DBNull.Value) && !string.IsNullOrEmpty(row["MXEUM"].ToString()))
                {
                    rtb3.Inlines.Add(" , MXEUM: ");
                    rtb3.Inlines.Add(row["MXEUM"].ToString());
                }
                //-----------------------------------------------------------------
                if (!object.ReferenceEquals(row["MXUSLC"], DBNull.Value) && !string.IsNullOrEmpty(row["MXUSLC"].ToString()))
                {
                    rtb3.Inlines.Add(" | MXUSLC: ");
                    rtb3.Inlines.Add(row["MXUSLC"].ToString());
                }

                RichTextBox rtb2 = new RichTextBox();
                s = row["Recommandation"].ToString();
                MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(s));
                rtb2.Selection.Load(stream, DataFormats.Rtf);
                rtb3.Background = new SolidColorBrush(Colors.PaleGoldenrod);
                Grid.SetRow(rtb3, i);
                i += 1;
                Grid.SetRow(rtb2, i);
                i += 1;
                AddTextBlockToGrid(rtb3);
                AddRichTextBoxToGrid(rtb2);
            }

        }

        private RecommandationModel _model;
        public DataTable d = new DataTable();

        #endregion

        #region Properties

        public String Date { get; set; }
        public String Univers { get; set; }
        public Sector Sector { get; set; }

        public DataTable Datas { get; set; }
        public IView View { get; set; }

        #endregion

        #region Method

        /// <summary>
        /// Call the view add text block method
        /// </summary>
        /// <param name="b"></param>
        public void AddTextBlockToGrid(TextBlock b)
        {
            if (View == null) return;
            View.AddTextBlockToGrid(b);
        }
        
        /// <summary>
        /// Call the view add rich text box method
        /// </summary>
        /// <param name="b"></param>
        public void AddRichTextBoxToGrid(RichTextBox b)
        {
            if (View == null) return;
            View.AddRichTextBoxToGrid(b);
        }

        #endregion
              
    }
}
