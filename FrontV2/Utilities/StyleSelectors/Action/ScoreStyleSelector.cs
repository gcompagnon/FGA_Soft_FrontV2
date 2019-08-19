using FrontV2.Action.ScoreReco;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace FrontV2.StyleSelectors
{


    class ScoreStyleSelector : StyleSelector
    {
        public Style StyleLightGreen { get; set; }
        public Style StyleLimeGreen { get; set; }
        public Style StyleWhite { get; set; }
        public Style StyleDimGray { get; set; }
        public Style StyleLightGray { get; set; }
        public Style StyleLightSlateGray { get; set; }
        public Style StyleLightCoral { get; set; }
        public Style StyleLightBlue { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is DataRow)
            {
                GridViewCell cell = container as GridViewCell;
                DataRow row = item as DataRow;

                if (cell.Column.Header.ToString() == "Note")
                {
                    if (GlobalInfos.tickerQuintQaunt.ContainsKey(row["TICKER"].ToString()))
                    {
                        int quint = GlobalInfos.tickerQuintQaunt[row["TICKER"].ToString()];

                        switch (quint)
                        {
                            case (1):
                                return StyleLimeGreen;
                            case (2):
                                return StyleLightGreen;
                            case (3):
                                return StyleWhite;
                            case (4):
                                return StyleLightGray;
                            case (5):
                                return StyleDimGray;
                        }
                    }
                }                
                else if (cell.Column.Header.ToString() == "note ISR")
                    return StyleLightGreen;
                else if (cell.Column.Header.ToString() == "Value")
                    return StyleLightSlateGray;
                else if (cell.Column.Header.ToString() == "Profit")
                    return StyleLightCoral;
                else if (cell.Column.Header.ToString() == "Growth")
                    return StyleLightBlue;
                else if (cell.Column.Header.ToString() == "ISR")
                    return StyleLightGreen;
                else if (cell.Column.Header.ToString() == "EPS_CHG_NTM" ||
                         cell.Column.Header.ToString() == "SALES_CHG_NTM")
                    return StyleLightBlue;
                else if (cell.Column.Header.ToString() == "EBIT_MARGIN_NTM" ||
                         cell.Column.Header.ToString() == "NET_DEBT_EBITDA_NTM" ||
                         cell.Column.Header.ToString() == "ROE_NTM" ||
                         cell.Column.Header.ToString() == "PBT_SALES_NTM" ||
                         cell.Column.Header.ToString() == "PBT_RWA_NTM" ||
                         cell.Column.Header.ToString() == "COST_INCOME_NTM" ||
                         cell.Column.Header.ToString() == "ROTE_NTM")
                    return StyleLightCoral;
                else if (cell.Column.Header.ToString() == "PE_NTM" ||
                         cell.Column.Header.ToString() == "PB_NTM" ||
                         cell.Column.Header.ToString() == "DIV_YLD_NTM" ||
                         cell.Column.Header.ToString() == "PE_ON_MED5Y" ||
                         cell.Column.Header.ToString() == "PE_PREMIUM_ON_HIST" ||
                         cell.Column.Header.ToString() == "PB_ON_MED5Y" ||
                         cell.Column.Header.ToString() == "PB_PREMIUM_ON_HIST" ||
                         cell.Column.Header.ToString() == "P_TBV_NTM" ||
                         cell.Column.Header.ToString() == "P_TBV_ON_MED5Y")
                    return StyleLightSlateGray;
                else if (cell.Column.Header.ToString() == "EPS_TREND" ||
                         cell.Column.Header.ToString() == "EPS_VAR_RSD" ||
                         cell.Column.Header.ToString() == "SALES_TREND" ||
                         cell.Column.Header.ToString() == "SALES_VAR_RSD")
                    return StyleLightBlue;
                else if (cell.Column.Header.ToString() == "FCF_TREND")
                    return StyleLightCoral;

                return null;
            }
            return null;
        }
    }
}
