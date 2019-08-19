using FrontV2.Action.ScoreReco;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace FrontV2.StyleSelectors
{


    class RepartitionStyleSelector : StyleSelector
    {
        public Style StyleBench { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is DataRow)
            {
                GridViewCell cell = container as GridViewCell;
                DataRow row = item as DataRow;

                if (cell.Column.Header.ToString().Contains("MX"))
                    return StyleBench;
            }
            return null;
        }
    }
}
