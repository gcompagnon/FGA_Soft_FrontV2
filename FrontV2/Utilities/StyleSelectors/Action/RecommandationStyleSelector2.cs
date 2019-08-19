using System.Data;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace FrontV2.StyleSelectors
{
    class RecommandationStyleSelector2 : StyleSelector
    {
        public Style StyleRecoPlus { get; set; }
        public Style StyleRecoMinus { get; set; }
        public Style StyleReco { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is DataRow)
            {
                GridViewCell cell = container as GridViewCell;
                DataRow row = item as DataRow;

                if (row["RecoMXEU"].ToString() == "+" && cell.Column.Header.ToString() == "RecoMXEU")
                    return StyleRecoPlus;
                if (row["RecoMXEUM"].ToString() == "+" && cell.Column.Header.ToString() == "RecoMXEUM")
                    return StyleRecoPlus;
                if (row["RecoMXEM"].ToString() == "+" && cell.Column.Header.ToString() == "RecoMXEM")
                    return StyleRecoPlus;

                if (row["RecoMXEU"].ToString() == "++" && cell.Column.Header.ToString() == "RecoMXEU")
                    return StyleRecoPlus;
                if (row["RecoMXEUM"].ToString() == "++" && cell.Column.Header.ToString() == "RecoMXEUM")
                    return StyleRecoPlus;
                if (row["RecoMXEM"].ToString() == "++" && cell.Column.Header.ToString() == "RecoMXEM")
                    return StyleRecoPlus;

                if (row["RecoMXEU"].ToString() == "-" && cell.Column.Header.ToString() == "RecoMXEU")
                    return StyleRecoMinus;
                if (row["RecoMXEUM"].ToString() == "-" && cell.Column.Header.ToString() == "RecoMXEUM")
                    return StyleRecoMinus;
                if (row["RecoMXEM"].ToString() == "-" && cell.Column.Header.ToString() == "RecoMXEM")
                    return StyleRecoMinus;

                if (cell.Column.Header.ToString() == "RecoMXEU")
                    return StyleReco;
                if (cell.Column.Header.ToString() == "RecoMXEUM")
                    return StyleReco;
                if (cell.Column.Header.ToString() == "RecoMXEM")
                    return StyleReco;

                if (row["RecoMXUSLC"].ToString() == "+" && cell.Column.Header.ToString() == "RecoMXUSLC")
                    return StyleRecoPlus;

                if (row["RecoMXUSLC"].ToString() == "++" && cell.Column.Header.ToString() == "RecoMXUSLC")
                    return StyleRecoPlus;

                if (row["RecoMXUSLC"].ToString() == "-" && cell.Column.Header.ToString() == "RecoMXUSLC")
                    return StyleRecoMinus;

                if (cell.Column.Header.ToString() == "RecoMXUSLC")
                    return StyleReco;
            }
            return null;
        }
    }
}
