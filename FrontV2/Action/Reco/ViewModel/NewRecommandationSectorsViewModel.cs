using FrontV2.Action.Reco.Model;
using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace FrontV2.Action.Reco.ViewModel
{
    class NewRecommandationSectorsViewModel : INotifyPropertyChanged
    {
        #region Constructor

        public NewRecommandationSectorsViewModel()
        {
        }

        #endregion

        #region Fields

        readonly NewRecommandationSectorsModel _model = new NewRecommandationSectorsModel();

        private ComboBoxItem _selectedNewMXEU;
        private ComboBoxItem _selectedNewMXEUM;
        private ComboBoxItem _selectedNewMXEM;
        private ComboBoxItem _selectedNewMXUSLC;

        #endregion

        #region Properties

        public ComboBoxItem SelectedNewMXEU
        {
            get { return _selectedNewMXEU; }
            set
            {
                _selectedNewMXEU = value;
                OnPropertyChanged("SelectedNewMXEU");
            }
        }
        public ComboBoxItem SelectedNewMXEUM
        {
            get { return _selectedNewMXEUM; }
            set
            {
                _selectedNewMXEUM = value;
                OnPropertyChanged("SelectedNewMXEUM");
            }
        }
        public ComboBoxItem SelectedNewMXEM
        {
            get { return _selectedNewMXEM; }
            set
            {
                _selectedNewMXEM = value;
                OnPropertyChanged("SelectedNewMXEM");
            }
        }
        public ComboBoxItem SelectedNewMXUSLC
        {
            get { return _selectedNewMXUSLC; }
            set
            {
                _selectedNewMXUSLC = value;
                OnPropertyChanged("SelectedNewMXUSLC");
            }
        }

        #endregion
        /// <summary>
        /// If at least one ot the comboxbox is not "pas de changement" that means we have to add a recommandation
        /// </summary>
        public void Validate(String idd, String name, String text,
            String oldReco1, String oldReco2, String oldReco3, String oldReco4, String place)
        {
            String mxeu;
            String mxeum;
            String mxem;
            String mxuslc;

            if (_selectedNewMXEU == null)
                mxeu = oldReco1;
            else if (_selectedNewMXEU.Content.ToString() == "Pas de changement")
                mxeu = oldReco1;
            else if (_selectedNewMXEU.Content.ToString() == "OUT")
                mxeu = "X";
            else if (_selectedNewMXEU.Content.ToString() == "Sans Reco")
                mxeu = "";
            else
                mxeu = _selectedNewMXEU.Content.ToString();

            if (_selectedNewMXEUM == null)
                mxeum = oldReco2;
            else if (_selectedNewMXEUM.Content.ToString() == "Pas de changement")
                mxeum = oldReco2;
            else if (_selectedNewMXEUM.Content.ToString() == "OUT")
                mxeum = "X";
            else if (_selectedNewMXEUM.Content.ToString() == "Sans Reco")
                mxeum = "";
            else
                mxeum = _selectedNewMXEUM.Content.ToString();

            if (_selectedNewMXEM == null)
                mxem = oldReco3;
            else if (_selectedNewMXEM.Content.ToString() == "Pas de changement")
                mxem = oldReco3;
            else if (_selectedNewMXEM.Content.ToString() == "OUT")
                mxem = "X";
            else if (_selectedNewMXEM.Content.ToString() == "Sans Reco")
                mxem = "";
            else
                mxem = _selectedNewMXEM.Content.ToString();

            if (_selectedNewMXUSLC == null)
                mxuslc = oldReco4;
            else if (_selectedNewMXUSLC.Content.ToString() == "Pas de changement")
                mxuslc = oldReco4;
            else if (_selectedNewMXUSLC.Content.ToString() == "OUT")
                mxuslc = "X";
            else if (_selectedNewMXUSLC.Content.ToString() == "Sans Reco")
                mxuslc = "";
            else
                mxuslc = _selectedNewMXUSLC.Content.ToString();

            _model.AddRecommandation(idd, mxeu, mxeum, mxem, mxuslc, place);

            int id = _model.GetIdFromIsin(idd);

            int id_change = _model.GetIdChangeFromIsin(idd);

            _model.UpdateReco(id, id_change, text);

        }

        public void Cancel()
        {

        }


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
