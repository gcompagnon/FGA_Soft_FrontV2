using FrontV2.Action.Consultation;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace FrontV2.StyleSelectors
{
    class ConsultationStyleSelector : StyleSelector
    {
        public Style SectorStyle { get; set; }
        public Style IndustryStyle { get; set; }
        public Style ESGExcluStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            //return base.SelectStyle(item, container);
            if (item is DataRow)
            {
                GridViewCell cell = container as GridViewCell;
                DataRow row = item as DataRow;

                if (row["INDUSTRY"].ToString() == "")
                    return SectorStyle;
                else if (row["Isin"].ToString() == "")
                    return IndustryStyle;

                try
                {
                    if (row["ESG"].ToString().CompareTo("EXCLU") == 0)
                    {
                        GlobalInfos.rowESGExclu.Add(row.ItemArray[0].ToString());
                    }
                }
                catch
                {
                }

                if (GlobalInfos.rowESGExclu.Contains(row.ItemArray[0]))
                    return ESGExcluStyle;
            }

            return null;
        }
    }
}