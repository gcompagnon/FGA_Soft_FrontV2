using FrontV2.Action.Reco.Model;
using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace FrontV2.Action.Reco.ViewModel
{
    class NewRecommandationValeursViewModel : INotifyPropertyChanged
    {
        #region Constructor

        public NewRecommandationValeursViewModel()
        { }

        #endregion

        #region Fields

        readonly NewRecommandationValeursModel _model = new NewRecommandationValeursModel();

        private ComboBoxItem _selectedNewMXEM;
        private ComboBoxItem _selectedNewMXEUM;
        private ComboBoxItem _selectedNewMXEU;
        private ComboBoxItem _selectedNewMXUSLC;

        #endregion

        #region Properties

        public ComboBoxItem SelectedNewMXEM
        {
            get { return _selectedNewMXEM; }
            set
            {
                _selectedNewMXEM = value;
                OnPropertyChanged("SelectedNewMXEM");
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
        public ComboBoxItem SelectedNewMXEU
        {
            get { return _selectedNewMXEU; }
            set
            {
                _selectedNewMXEU = value;
                OnPropertyChanged("SelectedNewMXEU");
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
        public void Validate(String isin, String text,
            String oldMXEM, String oldMXEUM, String oldMXEU, String oldMXUSLC)
        {
            String mxem, mxeum, mxeu, mxuslc;

            if (_selectedNewMXEM == null)
                mxem = oldMXEM;
            else if (_selectedNewMXEM.Content.ToString() == "Pas de changement")
                mxem = oldMXEM;
            else if (_selectedNewMXEM.Content.ToString() == "OUT")
                mxem = "X";
            else if (_selectedNewMXEM.Content.ToString() == "Sans Reco")
                mxem = "";
            else
                mxem = _selectedNewMXEM.Content.ToString();

            if (_selectedNewMXEUM == null)
                mxeum = oldMXEUM;
            else if (_selectedNewMXEUM.Content.ToString() == "Pas de changement")
                mxeum = oldMXEUM;
            else if (_selectedNewMXEUM.Content.ToString() == "OUT")
                mxeum = "X";
            else if (_selectedNewMXEUM.Content.ToString() == "Sans Reco")
                mxeum = "";
            else
                mxeum = _selectedNewMXEUM.Content.ToString();

            if (_selectedNewMXEU == null)
                mxeu = oldMXEU;
            else if (_selectedNewMXEU.Content.ToString() == "Pas de changement")
                mxeu = oldMXEU;
            else if (_selectedNewMXEU.Content.ToString() == "OUT")
                mxeu = "X";
            else if (_selectedNewMXEU.Content.ToString() == "Sans Reco")
                mxeu = "";
            else
                mxeu = _selectedNewMXEU.Content.ToString();

            if (_selectedNewMXUSLC == null)
                mxuslc = oldMXUSLC;
            else if (_selectedNewMXUSLC.Content.ToString() == "Pas de changement")
                mxuslc = oldMXUSLC;
            else if (_selectedNewMXUSLC.Content.ToString() == "OUT")
                mxuslc = "X";
            else if (_selectedNewMXUSLC.Content.ToString() == "Sans Reco")
                mxuslc = "";
            else
                mxuslc = _selectedNewMXUSLC.Content.ToString();

            _model.AddRecommandation(isin, mxem, mxeum, mxeu, mxuslc);

            _model.UpdateReco(_model.GetIdFromIsin(isin),
                _model.GetIdChangeFromIsin(isin),
                text);
        }

        public void Cancel()
        { }


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
