using System.Data;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace FrontV2.StyleSelectors
{
    class ScoreChangeStyleSelector : StyleSelector
    {
        public Style LightQuintile { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is DataRow)
            {
                GridViewCell cell = container as GridViewCell;
                DataRow row = item as DataRow;

                if (cell.Column.Header.ToString() == "Quint1")
                    return LightQuintile;
                if (cell.Column.Header.ToString() == "Quint2")
                    return LightQuintile;
            }
            return null;
        }
    }
}
