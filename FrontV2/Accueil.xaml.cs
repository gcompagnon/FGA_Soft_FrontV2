using FrontV2.Action.Consultation.View;
using FrontV2.Action.Doublons.View;
using FrontV2.Action.Reco.View;
using FrontV2.Action.Repartition.View;
using FrontV2.Action.ScoreChange.View;
using FrontV2.Action.ScoreReco.View;
using FrontV2.Action.SimulationScore.View;
using FrontV2.TauxCredit.BaseInterpolation.View;
using FrontV2.TauxCredit.Indicateurs.View;
using FrontV2.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Deployment.Application;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace FrontV2
{
    /// <summary>
    /// Logique d'interaction pour Accueil.xaml
    /// </summary>
    public partial class Accueil : Window
    {

        #region Fields

        Connection co = new Connection();
        CompanyNameCleaner cleaner = new CompanyNameCleaner();

        public BitmapImage ImageBack
        { get; set; }

        private String _imagePath;
        private bool _doublonsDone = false;

        private WindowsApplication1.Accueil _accueilOld;

        #endregion

        public Accueil()
        {
            cleaner.FillExceptions();
            cleaner.FillExtensions();

            _accueilOld = new WindowsApplication1.Accueil();
            _accueilOld.AccueilIHM_Load(null, null);

            /*
             * Lecture des parametres de la base OMEGA du fichier Login.ini
             * WindowsApplication1.Fichier fi = new WindowsApplication1.Fichier();
             * fi.LectureFichierLog("Login.INI");
             * Tentative de connection
             */
            co.ToConnectBase();

            CreateUser();

            InitializeComponent();

            droitUtilisateur();
            SetWindowTitle();

            GetBackgroundIfExists();
        }

        #region Configuration Accueil

        public void GetBackgroundIfExists()
        {
            _imagePath = @"\\vill1\Partage\,FGA Front Office\02_Gestion_Actions\00_BASE\Base 2.0\0X_PERSONNALISATION\"
               + Utilisateur.login
               + @"\background.png";

            try
            {
                BitmapImage img = new BitmapImage(new Uri(_imagePath, UriKind.RelativeOrAbsolute));
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = img;
                TileList.Background = brush;
            }
            catch
            { }
        }

        public void CreateUser()
        {

            WindowsIdentity currentUser = WindowsIdentity.GetCurrent();
            char split = '\\';
            String login = currentUser.Name.Split(split)[1].ToString().ToUpper();

            if (co.SelectSimple("UTILISATEUR", "Id").Contains(login))
            {
                Utilisateur.login = login;
                Utilisateur.metier = co.SelectWhere("UTILISATEUR", "TypeUtilisateur", "Id", login).FirstOrDefault().ToString();
                Utilisateur.admin = Boolean.Parse(co.SelectWhere("UTILISATEUR", "admin", "Id", login).FirstOrDefault().ToString());

                co.Update("UTILISATEUR", new List<String>(new String[] { "derniere_connexion" }),
                    new List<object>(new object[] { DateTime.Now.ToString() }),
                    "id", login);
            }
        }

        public void SetWindowTitle()
        {
            String connectionBase = "";
            if (Utilisateur.metier == "Dev")
            {
                if (co.ConnectionString == "FGA_RW")
                    connectionBase = " FRONTV2: PRODUCTION";
                else if (co.ConnectionString == "FGA_PREPROD_RW")
                    connectionBase = " FRONTV2: PREPRODUCTION";
                else
                    connectionBase = " FRONTV2: Serveur inconnu";
                this.window.Title = this.window.Title + " ( version " + retourneVersionDeployee() + " ) -->" + connectionBase;
            }
            else
                this.window.Title = this.window.Title + " ( version " + retourneVersionDeployee() + " )";
        }

        public String retourneVersionDeployee()
        {
            String title = "";

            try
            {
                title = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            catch
            {
                Console.WriteLine("Exception de déploiement du a la cr&ation du titre de la feneêre, prévu pour la version de conf.");
                title = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            return title;
        }

        private void droitUtilisateur()
        {
            this.TileList.Visibility = Visibility.Collapsed;

            // Select visible menu according to user's job

            // Utilisateur.metier = "Act";

            switch (Utilisateur.metier)
            {
                case ("Act"):
                    this.ActMenu.Visibility = Visibility.Visible;
                    CheckScoreChanges();
                    CheckDoublonsThread();
                    this.TileList.Visibility = Visibility.Visible;
                    break;
                case ("Tx"):
                    this.TauxGrid.Visibility = Visibility.Visible;
                    this.TxMenu.Visibility = Visibility.Visible;
                    this.IntégrationToolStripMenuItem.Visibility = Visibility.Visible;
                    this.SolvencyToolStripMenuItem.Visibility = Visibility.Visible;
                    break;
                case ("Div"):
                    this.DivMenu.Visibility = Visibility.Visible;
                    break;
                case ("MO"):
                    this.MiddleToolStripMenuItem.Visibility = Visibility.Visible;
                    this.IntégrationToolStripMenuItem.Visibility = Visibility.Visible;
                    break;
                case ("Rep"):
                    this.ReMenu.Visibility = Visibility.Visible;
                    this.TxMenu.Visibility = Visibility.Visible;
                    break;
                case ("Ci"):
                    this.ReMenu.Visibility = Visibility.Visible;
                    this.TxMenu.Visibility = Visibility.Visible;
                    break;
                case ("Dir"):
                    this.ActMenu.Visibility = Visibility.Visible;
                    this.TxMenu.Visibility = Visibility.Visible;
                    this.DivMenu.Visibility = Visibility.Visible;
                    this.ReMenu.Visibility = Visibility.Visible;
                    this.SolvencyToolStripMenuItem.Visibility = Visibility.Visible;
                    this.IntégrationToolStripMenuItem.Visibility = Visibility.Visible;
                    this.MiddleToolStripMenuItem.Visibility = Visibility.Visible;
                    break;
                case ("Dev"):
                    this.ActMenu.Visibility = Visibility.Visible;
                    this.TxMenu.Visibility = Visibility.Visible;
                    this.DivMenu.Visibility = Visibility.Visible;
                    this.ReMenu.Visibility = Visibility.Visible;
                    this.SolvencyToolStripMenuItem.Visibility = Visibility.Visible;
                    this.IntégrationToolStripMenuItem.Visibility = Visibility.Visible;
                    this.MiddleToolStripMenuItem.Visibility = Visibility.Visible;
                    this.DebugToolStripMenuItem.Visibility = Visibility.Visible;
                    //CheckScoreChanges();
                    //CheckDoublonsThread();
                    break;
                default:
                    String message = "Votre type d'utilisateur n'est pas reconnu par l'application : " +
                                     Utilisateur.login + " -> " + Utilisateur.metier + ", " + Utilisateur.admin;
                    MessageBox.Show(message, "Metier Inconnu", MessageBoxButton.OK);
                    break;
            }

            // Select admin visibility according to user permissions
            switch (Utilisateur.admin)
            {
                case (true):
                    this.AdminMenu.Visibility = Visibility.Visible;
                    break;
                case (false):
                    this.AdminMenu.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (MessageBox.Show("Voulez vous fermer l'application et toutes les fenêres associées ?", "Quitter",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                co.ToDiconnect();
                App.Current.Shutdown();
            }
            else
                e.Cancel = true;
        }

        #endregion

        #region Actions

        #region Écrans

        private ScoreRecoView _scoreRecoView = null;
        private BaseActionConsultation _consultation = null;
        private RecoView _recommandation = null;
        private RepartitionView _repartition = null;
        private RepartitionValeurView _repartitionValeur = null;
        private SimulationScoreView _simulation = null;
        private DoublonsView _doublons = null;
        private ScoreChangeView _changementsScores = null;

        #endregion

        private void ScoreRecoMenu_Click(object sender, RoutedEventArgs e)
        {
            if (_scoreRecoView == null)
            {
                _scoreRecoView = new ScoreRecoView();
                _scoreRecoView.Show();
                _scoreRecoView.Closed += new EventHandler(setToNull);
                _scoreRecoView.Focus();
            }
            else
            {
                _scoreRecoView.Focus();
            }
        }

        private void ConsultationMenu_Click(object sender, RoutedEventArgs e)
        {
            if (_consultation == null)
            {
                _consultation = new BaseActionConsultation();
                _consultation.Show();
                _consultation.Closed += new EventHandler(setToNull);
                _consultation.Focus();
            }
            else
            {
                _consultation.Focus();
            }
        }

        private void Reco_Click(object sender, RoutedEventArgs e)
        {
            if (_recommandation == null)
            {
                _recommandation = new RecoView();
                _recommandation.Show();
                _recommandation.Closed += new EventHandler(setToNull);
            }
            else
            {
                _recommandation.Focus();
            }
        }

        private void Repartition_Click(object sender, RoutedEventArgs e)
        {
            if (_repartition == null)
            {
                _repartition = new RepartitionView();
                _repartition.Show();
                _repartition.Closed += new EventHandler(setToNull);
            }
            else
            {
                _repartition.Focus();
            }
        }

        private void RepartitionValues_Click(object sender, RoutedEventArgs e)
        {
            if (_repartitionValeur == null)
            {
                _repartitionValeur = new RepartitionValeurView();
                _repartitionValeur.Show();
                _repartitionValeur.Closed += new EventHandler(setToNull);
            }
            else
            {
                _repartitionValeur.Focus();
            }
        }

        private void SimulationScore_Click(object sender, RoutedEventArgs e)
        {
            if (_simulation == null)
            {
                _simulation = new SimulationScoreView();
                _simulation.Show();
                _simulation.Closed += new EventHandler(setToNull);
            }
            else
            {
                _simulation.Focus();
            }
        }

        private void ChangesScore_Click(object sender, RoutedEventArgs e)
        {
            if (_changementsScores == null)
            {
                _changementsScores = new ScoreChangeView();
                _changementsScores.Show();
                _changementsScores.Closed += new EventHandler(setToNull);
            }
            else
            {
                _changementsScores.Focus();
            }
        }

        private void Doublons_Click(object sender, RoutedEventArgs e)
        {
            if (_doublons == null)
            {
                _doublons = new DoublonsView();
                _doublons.Show();
                _doublons.Closed += new EventHandler(setToNull);
            }
            else
            {
                _doublons.Focus();
            }
        }

        public void setToNull(object sender, EventArgs e)
        {
            if (sender is ScoreRecoView)
                _scoreRecoView = null;
            else if (sender is BaseActionConsultation)
                _consultation = null;
            else if (sender is RecoView)
                _recommandation = null;
            else if (sender is RepartitionView)
                _repartition = null;
            else if (sender is RepartitionValeurView)
                _repartitionValeur = null;
            else if (sender is SimulationScoreView)
                _simulation = null;
            else if (sender is DoublonsView)
                _doublons = null;
            else if (sender is ScoreChangeView)
                _changementsScores = null;
        }

        #region Checks

        private void CheckScoreChanges()
        {
            String date1 = co.GetMaxDate();
            String date2 = co.sqlToListDico("SELECT distinct DATE AS 'date' FROM DATA_FACTSET WHERE date<'" + date1 + "' ORDER BY DATE DESC")[0]["date"].ToString();

            String nsql = "SELECT * " +
                        "into #qrvalues1 " +
                        "from DATA_FACTSET " +
                        "where DATE = '" + date1 + "' AND ISIN IS NOT NULL " +
                        " " +
                        "SELECT * " +
                        "into #qrvalues2 " +
                        "from DATA_FACTSET " +
                        "where DATE = '" + date2 + "'  And ISIN IS NOT NULL " +
                        " " +
                        "SELECT count(*) " +
                        "from #qrvalues1 fac1 " +
                        "	inner join #qrvalues2 fac2 on fac1.TICKER = fac2.TICKER OR fac1.ISIN = fac2.ISIn " +
                        "WHERE fac1.GARPN_QUINTILE_S <> fac2.GARPN_QUINTILE_S";

            String clear = "DROP TABLE #qrvalues1 " + " DROP TABLE #qrvalues2";

            int i = (int)co.SqlWithReturn(nsql)[0];
            co.RequeteSql(clear);

            if (i > 0)
            {
                this.Notifications.Visibility = Visibility.Visible;
                this.NotifScoresText.Text = "Il y a des changements dans les Scores. Cliquez ici pour les consulter";

                //MessageBoxResult result = MessageBox.Show("Bonjour.\n Il y a des changements dans les scores.\n Voulez-vous ouvrir l'écran des changements de scores ?", "Daily ScoreChange", MessageBoxButton.YesNo);
                //if (result == MessageBoxResult.Yes)
                //{
                //    ScoreChangeView view = new ScoreChangeView(true);
                //    view.Show();
                //}
            }
            else
                this.Notifications.Visibility = Visibility.Collapsed;
        }

        private void CheckDoublonsThread()
        {
            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += this.OnBackgroundWorkerDoWork;
            backgroundWorker.RunWorkerCompleted += OnBackgroundWorkerRunWorkerCompleted;
            backgroundWorker.RunWorkerAsync();
        }

        private void OnBackgroundWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            backgroundWorker.DoWork -= this.OnBackgroundWorkerDoWork;
            backgroundWorker.RunWorkerCompleted -= OnBackgroundWorkerRunWorkerCompleted;

            int res = (int)e.Result;

            if (res > 0)
            {
                this.NotifDoublonsText.Text = "Il y a des doublons dans la base. Cliquez ici pour les nettoyer";
                this.NotifDoublons.Visibility = Visibility.Visible;
            }
            else
                this.NotifDoublons.Visibility = Visibility.Collapsed;
            MessageBox.Show("Doublons Terminés");
        }

        private void OnBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            int i = CheckDoublons();
            e.Result = i;
        }

        private int CheckDoublons()
        {
            #region String
            const String FIND_DOUBLE_EQUITY_SQL =
                " DECLARE @lastdate AS DATETIME" +
                " SET @lastdate = (SELECT MAX(DATE) FROM DATA_FACTSET) - 3" +
                " DECLARE @sql AS	VARCHAR(max)" +
                " SELECT distinct x.COMPANY_NAME, x.ISIN, x.TICKER" +
                " INTO ##tmpdoubles" +
                " FROM" +
                " (SELECT distinct a.TICKER, a.COMPANY_NAME, a.ISIN" +
                " FROM DATA_FACTSET a" +
                " INNER JOIN DATA_FACTSET b on (a.ISIN = b.ISIN AND" +
                " (a.TICKER<>b.TICKER OR a.COMPANY_NAME<>b.COMPANY_NAME))" +
                " OR" +
                " (a.TICKER = b.TICKER AND " +
                " (a.ISIN<>b.ISIN OR a.COMPANY_NAME<>b.COMPANY_NAME))" +
                " OR" +
                " (a.COMPANY_NAME=b.COMPANY_NAME AND" +
                " (a.ISIN<>b.ISIN OR a.TICKER<>b.TICKER))" +
                " WHERE a.ISIN Is Not null AND a.date >= @lastdate AND b.date >= @lastdate )AS x" +
                " ORDER BY x.TICKER" +
                " SELECT (SELECT MAX(h.DATE) FROM DATA_FACTSET h " +
                " WHERE j.TICKER=h.TICKER AND j.ISIN=h.ISIN AND j.COMPANY_NAME=h.COMPANY_NAME) as date, " +
                " j.COMPANY_NAME as name, " +
                " j.ISIN as isin, " +
                " (SELECT TOP(1) h.COUNTRY FROM DATA_FACTSET h " +
                " WHERE j.TICKER=h.TICKER AND j.ISIN=h.ISIN AND j.COMPANY_NAME=h.COMPANY_NAME) as country, " +
                " j.TICKER as ticker " +
                " FROM ##tmpdoubles as j " +
                " ORDER BY country, j.TICKER " +
                " DROP TABLE ##tmpdoubles";
            #endregion

            try
            {
                List<Dictionary<String, object>> dico = co.sqlToListDico(FIND_DOUBLE_EQUITY_SQL);
                return dico.Count;
            }
            catch
            { }
            finally
            {
                _doublonsDone = true;
            }
            return 0;
        }

        #endregion

        #region Intéractions Panels

        private void StackPanel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var converter = new System.Windows.Media.BrushConverter();
            StackPanel obj = sender as StackPanel;

            if (obj.Name == "Scores")
                this.Scores.Background = (Brush)converter.ConvertFromString("#FF58B1B9");
            else if (obj.Name == "Consult")
                this.Consult.Background = (Brush)converter.ConvertFromString("#FF5BBF35");
            else if (obj.Name == "Recommandation")
                this.Recommandation.Background = (Brush)converter.ConvertFromString("#FF58B1B9");
            else if (obj.Name == "RepartitionG")
                this.RepartitionG.Background = (Brush)converter.ConvertFromString("#FFB66934");
            else if (obj.Name == "RepartitionV")
                this.RepartitionV.Background = (Brush)converter.ConvertFromString("#FFB66934");
            else if (obj.Name == "DoublonsT")
                this.DoublonsT.Background = (Brush)converter.ConvertFromString("#FF8D38B6");
            else if (obj.Name == "ChangeScore")
                this.ChangeScore.Background = (Brush)converter.ConvertFromString("#FF8D38B6");
            else if (obj.Name == "NotifScores")
                this.NotifScores.Background = (Brush)converter.ConvertFromString("#FFB81515");
            else if (obj.Name == "NotifDoublons")
                this.NotifDoublons.Background = (Brush)converter.ConvertFromString("#FFB81515");
        }

        private void StackPanel_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var converter = new System.Windows.Media.BrushConverter();
            StackPanel obj = sender as StackPanel;

            if (obj.Name == "Scores")
                this.Scores.Background = (Brush)converter.ConvertFromString("#FF6BD9E4");
            else if (obj.Name == "Consult")
                this.Consult.Background = (Brush)converter.ConvertFromString("#FF6EE840");
            else if (obj.Name == "Recommandation")
                this.Recommandation.Background = (Brush)converter.ConvertFromString("#FF6BD9E4");
            else if (obj.Name == "RepartitionG")
                this.RepartitionG.Background = (Brush)converter.ConvertFromString("#FFEE8842");
            else if (obj.Name == "RepartitionV")
                this.RepartitionV.Background = (Brush)converter.ConvertFromString("#FFEE8842");
            else if (obj.Name == "DoublonsT")
                this.DoublonsT.Background = (Brush)converter.ConvertFromString("#FFB046E2");
            else if (obj.Name == "ChangeScore")
                this.ChangeScore.Background = (Brush)converter.ConvertFromString("#FFB046E2");
            else if (obj.Name == "NotifScores")
                this.NotifScores.Background = (Brush)converter.ConvertFromString("#FFE21919");
            else if (obj.Name == "NotifDoublons")
                this.NotifDoublons.Background = (Brush)converter.ConvertFromString("#FFE21919");
        }

        private void StackPanel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StackPanel obj = sender as StackPanel;
            if (!_doublonsDone)
            {
                MessageBox.Show("Test sur les doublons en cours, veuillez patienter.");
                return;
            }
            if (obj.Name == "Scores")
                ScoreRecoMenu_Click(null, null);
            else if (obj.Name == "Consult")
                ConsultationMenu_Click(null, null);
            else if (obj.Name == "Recommandation")
                Reco_Click(null, null);
            else if (obj.Name == "RepartitionG")
                Repartition_Click(null, null);
            else if (obj.Name == "RepartitionV")
                RepartitionValues_Click(null, null);
            else if (obj.Name == "DoublonsT")
                Doublons_Click(null, null);
            else if (obj.Name == "ChangeScore")
                ChangesScore_Click(null, null);
            else if (obj.Name == "NotifScores")
            {
                if (_changementsScores == null)
                {
                    _changementsScores = new ScoreChangeView();
                    _changementsScores.preLoad();
                    _changementsScores.Show();
                    _changementsScores.Closed += new EventHandler(setToNull);
                }
                else
                {
                    _changementsScores.Focus();
                }
            }
            else if (obj.Name == "NotifDoublons")
                Doublons_Click(null, null);
        }

        #endregion

        #endregion

        #region Taux / Crédit

        private void IBoxxAnalyse_Click(object sender, RoutedEventArgs e)
        {
            WindowsApplication1.Taux.PrimeObligIboxx.Iboxx ib = new WindowsApplication1.Taux.PrimeObligIboxx.Iboxx();
            ib.Show();
        }

        private void BaseEmetteur_Click(object sender, RoutedEventArgs e)
        {
            WindowsApplication1.Taux.BaseEmetteurs.BaseEmetteurs be = new WindowsApplication1.Taux.BaseEmetteurs.BaseEmetteurs();
            be.Show();
        }

        private void TickerBBGImportation_Click(object sender, RoutedEventArgs e)
        {
            WindowsApplication1.Action.BaseActionImportation.UpdateTicker(@"\INPUT\TAUX\CREDIT");
        }

        private void OpenBaseInter_Click(object sender, RoutedEventArgs e)
        {
            BaseInterpolation baseInter = new BaseInterpolation();
            baseInter.Show();
        }

        private void OpenIndicateur_Click(object sender, RoutedEventArgs e)
        {
            IndicateurView indicateur = new IndicateurView();
            indicateur.Show();
        }

        #endregion

        #region Menus non classés

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            WindowsApplication1.AjoutUtilisateur adduser = new WindowsApplication1.AjoutUtilisateur();
            adduser.ShowDialog();
        }

        private void ModifUser_Click(object sender, RoutedEventArgs e)
        {
            WindowsApplication1.ModificationUtilisateur modif = new WindowsApplication1.ModificationUtilisateur();
            modif.Show();
        }

        private void ScriptBDD_Click(object sender, RoutedEventArgs e)
        {
            WindowsApplication1.ScriptBDD scriptBDD = new WindowsApplication1.ScriptBDD();
            scriptBDD.Show();
        }

        private void Importation_Click(object sender, RoutedEventArgs e)
        {
            WindowsApplication1.Referentiel.GestionTable table = new WindowsApplication1.Referentiel.GestionTable();
            table.Show();
        }

        private void Verification_Click(object sender, RoutedEventArgs e)
        {
            WindowsApplication1.Verification verif = new WindowsApplication1.Verification();
            verif.Show();
        }

        private void Transparence_Click(object sender, RoutedEventArgs e)
        {
            WindowsApplication1.Referentiel.Transparence transparence = new WindowsApplication1.Referentiel.Transparence();
            transparence.Show();
        }

        private void Reconciliation_Click(object sender, RoutedEventArgs e)
        {
            WindowsApplication1.Referentiel.Reconciliation recon = new WindowsApplication1.Referentiel.Reconciliation();
            recon.Show();
        }

        private void Affectation_Click(object sender, RoutedEventArgs e)
        {
            WindowsApplication1.AccueilTitreGrille aff = new WindowsApplication1.AccueilTitreGrille();
            aff.Show();
        }

        private void SuiviTitresManuel_Click(object sender, RoutedEventArgs e)
        {
            WindowsApplication1.SuiviTitreManuel sv = new WindowsApplication1.SuiviTitreManuel();
            sv.Show();
        }

        private void AlimentationPtfFGA_Click(object sender, RoutedEventArgs e)
        {
            WindowsApplication1.Accueil ac = new WindowsApplication1.Accueil();
            ac.AlimentationPTFFGAToolStripMenuItem_Click(null, null);
        }

        private void Allocation_Click(object sender, RoutedEventArgs e)
        {
            WindowsApplication1.AllocationGrille ag = new WindowsApplication1.AllocationGrille();
            ag.Show();
        }

        private void ExtractionReporting_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Pas encore implémenté !\n dans l'ancienne version : 'Ouvir IHM .....\n Dans la nouvelle je ne sais pas quoi en faire!, 'Benjamin.C'");
        }

        private void SolvencySimulation_Click(object sender, RoutedEventArgs e)
        {
            WindowsApplication1.Solv2 solv = new WindowsApplication1.Solv2();
            solv.Show();
        }

        private void CoefSecteurs_Click(object sender, RoutedEventArgs e)
        {
            WindowsApplication1.Action.Coefficient.BaseActionCoefIndice coef = new WindowsApplication1.Action.Coefficient.BaseActionCoefIndice();
            coef.Show();
        }

        private void CoefValeurs_Click(object sender, RoutedEventArgs e)
        {
            WindowsApplication1.Action.Coefficient.BaseActionCoefSecteur coef = new WindowsApplication1.Action.Coefficient.BaseActionCoefSecteur();
            coef.Show();
        }

        private void ConfigNote_Click(object sender, RoutedEventArgs e)
        {
            WindowsApplication1.Action.Note.BaseActionNote note = new WindowsApplication1.Action.Note.BaseActionNote();
            note.Show();
        }

        private void ImportationAction_Click(object sender, RoutedEventArgs e)
        {
            WindowsApplication1.Action.BaseActionImportation imp = new WindowsApplication1.Action.BaseActionImportation();
            imp.Show();
        }

        private void ConfigureConnection_Click(object sender, RoutedEventArgs e)
        {
            String prevCo = co.ConnectionString;
            String res = Microsoft.VisualBasic.Interaction.InputBox("Entrez le connection à utiliser:", "Connexion", "Exemple: FGA_PREPROD_RW");
            
            if (res != "")
            {
                co.ToDiconnect();
                
                WindowsApplication1.Connection.coBase.Close();
                WindowsApplication1.Connection.coBaseBis.Close();

                try
                {
                    co.ConnectionString = res;
                    _accueilOld.co.connectionString = res;
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.ToString());
                    co.ConnectionString = prevCo;

                    _accueilOld.co.connectionString = prevCo;
                }

                co.ToConnectBase();
                _accueilOld.co.ToConnectBase();
                _accueilOld.co.ToConnectBasebis();
            }
        }

        private void CleanRecommandations_Click(object sender, RoutedEventArgs e)
        {
            RecoView reco = new RecoView(false);
            reco.Show();
        }

        #endregion

    }
}
