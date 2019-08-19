using FrontV2.Action.Reco.ViewModel;
using System;
using System.Windows;
using Telerik.Windows.Documents.FormatProviders.Rtf;

namespace FrontV2.Action.Reco.View
{
    /// <summary>
    /// Logique d'interaction pour NewRecommandationView.xaml
    /// </summary>
    public partial class NewRecommandationSectorsView : Window
    {
        NewRecommandationSectorsViewModel _viewModel;
        public bool validated = false;

        String place;
        public NewRecommandationSectorsView()
        {
            InitializeComponent();

            _viewModel = new NewRecommandationSectorsViewModel();
            DataContext = _viewModel;
        }

        public void Load(String id, String name, String prevMXEU, 
            String prevMXEUM, String prevMXEM, String prevMXUSLC, String place)
        {
            Id.Text = id;
            NameField.Text = name;
            PrevMXEU.Text = prevMXEU;
            PrevMXEUM.Text = prevMXEUM;
            PrevMXEM.Text = prevMXEM;
            PrevMXUSLC.Text = prevMXUSLC;
            this.place = place;

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
            _viewModel.Validate(Id.Text, NameField.Text, rtf.Export(TextEditor.Document), PrevMXEU.Text, PrevMXEUM.Text, PrevMXEM.Text, PrevMXUSLC.Text, place);
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
