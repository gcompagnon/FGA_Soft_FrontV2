using FrontV2.Action.Reco.ViewModel;
using System;
using System.Windows;
using Telerik.Windows.Documents.FormatProviders.Rtf;

namespace FrontV2.Action.Reco.View
{
    /// <summary>
    /// Logique d'interaction pour NewRecommandationView.xaml
    /// </summary>
    public partial class NewRecommandationValeurView : Window
    {
        NewRecommandationValeursViewModel _viewModel;
        public bool validated = false;

        public NewRecommandationValeurView()
        {
            InitializeComponent();

            _viewModel = new NewRecommandationValeursViewModel();
            DataContext = _viewModel;
        }
        
        public void Load(String isin, String libelle, String recoMXEU,
            String recoMXEUM, String recoMXEM,String recoMXUSLC, String ticker)
        {
            Isin.Text = isin;
            Libelle.Text = libelle;
            
            PrevMXEM.Text = recoMXEM;
            PrevMXEUM.Text = recoMXEUM;
            PrevMXEU.Text = recoMXEU;

            PrevMXUSLC.Text = recoMXUSLC;

            Ticker.Text = ticker;

            if (!GlobalInfos.isEurope)
            {
                PanelMXEM.Visibility = Visibility.Collapsed;
                PanelMXEU.Visibility = Visibility.Collapsed;
                PanelMXEUM.Visibility = Visibility.Collapsed;
                PanelMXUSLC.Visibility = Visibility.Visible;
            }
        }

        private void Validate_Click(object sender, RoutedEventArgs e)
        {
            RtfFormatProvider rtf = new RtfFormatProvider();
            _viewModel.Validate(Isin.Text, rtf.Export(TextEditor.Document),
                PrevMXEM.Text, PrevMXEUM.Text, PrevMXEU.Text, PrevMXUSLC.Text);
            validated = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Cancel();
            this.Close();
        }
    }
}
