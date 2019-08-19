using FrontV2.Action.Reco;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace FrontV2.StyleSelectors
{
    class RecommandationStyleSelector : StyleSelector
    {
        public Style StyleClassement1 { get; set; }
        public Style StyleClassement2 { get; set; }
        public Style StyleClassement3 { get; set; }
        public Style StyleClassement4 { get; set; }
        public Style StyleClassement5 { get; set; }
        public Style StyleSector { get; set; }
        public Style StyleIndustry { get; set; }
        public Style StyleRecoPlus { get; set; }
        public Style StyleRecoPlusPlus { get; set; }
        public Style StyleRecoMinus { get; set; }
        public Style StyleRecoPlusIndustry { get; set; }
        public Style StyleRecoPlusPlusIndustry { get; set; }
        public Style StyleRecoPlusIsin { get; set; }
        public Style StyleRecoPlusPlusIsin { get; set; }
        public Style StyleRecoMinusIndustry { get; set; }
        public Style StyleRecoMinusIsin { get; set; }
        public Style StyleRecoSector { get; set; }
        public Style StyleRecoIndustry { get; set; }
        public Style StyleReco{ get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is DataRow)
            {
                GridViewCell cell = container as GridViewCell;
                DataRow row = item as DataRow;

                if (cell.Column.Header.ToString() == "Q")
                {
                    switch (row["Q"].ToString())
                    {
                        case "1":
                            return StyleClassement1;
                        case "2":
                            return StyleClassement2;
                        case "3":
                            return StyleClassement3;
                        case "4":
                            return StyleClassement4;
                        case "5":
                            return StyleClassement5;
                    }
                }

                if (row["Label_Industry"].ToString() == "")
                {
                    if (GlobalInfos.isEurope)
                    {
                        if (row["RecoMXEU"].ToString() == "+" && cell.Column.Header.ToString() == "RecoMXEU")
                            return StyleRecoPlusIndustry;
                        if (row["RecoMXEUM"].ToString() == "+" && cell.Column.Header.ToString() == "RecoMXEUM")
                            return StyleRecoPlusIndustry;
                        if (row["RecoMXEM"].ToString() == "+" && cell.Column.Header.ToString() == "RecoMXEM")
                            return StyleRecoPlusIndustry;

                        if (row["RecoMXEU"].ToString() == "++" && cell.Column.Header.ToString() == "RecoMXEU")
                            return StyleRecoPlusPlusIndustry;
                        if (row["RecoMXEUM"].ToString() == "++" && cell.Column.Header.ToString() == "RecoMXEUM")
                            return StyleRecoPlusPlusIndustry;
                        if (row["RecoMXEM"].ToString() == "++" && cell.Column.Header.ToString() == "RecoMXEM")
                            return StyleRecoPlusPlusIndustry;

                        if (row["RecoMXEU"].ToString() == "-" && cell.Column.Header.ToString() == "RecoMXEU")
                            return StyleRecoMinusIndustry;
                        if (row["RecoMXEUM"].ToString() == "-" && cell.Column.Header.ToString() == "RecoMXEUM")
                            return StyleRecoMinusIndustry;
                        if (row["RecoMXEM"].ToString() == "-" && cell.Column.Header.ToString() == "RecoMXEM")
                            return StyleRecoMinusIndustry;

                        if (cell.Column.Header.ToString() == "RecoMXEU")
                            return StyleRecoSector;
                        if (cell.Column.Header.ToString() == "RecoMXEUM")
                            return StyleRecoSector;
                        if (cell.Column.Header.ToString() == "RecoMXEM")
                            return StyleRecoSector;
                    }
                    else
                    {
                        if (row["RecoMXUSLC"].ToString() == "+" && cell.Column.Header.ToString() == "RecoMXUSLC")
                            return StyleRecoPlusIndustry;

                        if (row["RecoMXUSLC"].ToString() == "++" && cell.Column.Header.ToString() == "RecoMXUSLC")
                            return StyleRecoPlusPlusIndustry;

                        if (row["RecoMXUSLC"].ToString() == "-" && cell.Column.Header.ToString() == "RecoMXUSLC")
                            return StyleRecoMinusIndustry;

                        if (cell.Column.Header.ToString() == "RecoMXUSLC")
                            return StyleRecoSector;
                    }

                    return StyleSector;
                }
                if (row["ISIN"].ToString() == "")
                {
                    if (GlobalInfos.isEurope)
                    {
                        if (row["RecoMXEU"].ToString() == "+" && cell.Column.Header.ToString() == "RecoMXEU")
                            return StyleRecoPlusIsin;
                        if (row["RecoMXEUM"].ToString() == "+" && cell.Column.Header.ToString() == "RecoMXEUM")
                            return StyleRecoPlusIsin;
                        if (row["RecoMXEM"].ToString() == "+" && cell.Column.Header.ToString() == "RecoMXEM")
                            return StyleRecoPlusIsin;

                        if (row["RecoMXEU"].ToString() == "++" && cell.Column.Header.ToString() == "RecoMXEU")
                            return StyleRecoPlusPlusIsin;
                        if (row["RecoMXEUM"].ToString() == "++" && cell.Column.Header.ToString() == "RecoMXEUM")
                            return StyleRecoPlusPlusIsin;
                        if (row["RecoMXEM"].ToString() == "++" && cell.Column.Header.ToString() == "RecoMXEM")
                            return StyleRecoPlusPlusIsin;


                        if (row["RecoMXEU"].ToString() == "-" && cell.Column.Header.ToString() == "RecoMXEU")
                            return StyleRecoMinusIsin;
                        if (row["RecoMXEUM"].ToString() == "-" && cell.Column.Header.ToString() == "RecoMXEUM")
                            return StyleRecoMinusIsin;
                        if (row["RecoMXEM"].ToString() == "-" && cell.Column.Header.ToString() == "RecoMXEM")
                            return StyleRecoMinusIsin;

                        if (cell.Column.Header.ToString() == "RecoMXEU")
                            return StyleRecoIndustry;
                        if (cell.Column.Header.ToString() == "RecoMXEUM")
                            return StyleRecoIndustry;
                        if (cell.Column.Header.ToString() == "RecoMXEM")
                            return StyleRecoIndustry;
                    }
                    else
                    {
                        if (row["RecoMXUSLC"].ToString() == "+" && cell.Column.Header.ToString() == "RecoMXUSLC")
                            return StyleRecoPlusIsin;

                        if (row["RecoMXUSLC"].ToString() == "++" && cell.Column.Header.ToString() == "RecoMXUSLC")
                            return StyleRecoPlusPlusIsin;


                        if (row["RecoMXUSLC"].ToString() == "-" && cell.Column.Header.ToString() == "RecoMXUSLC")
                            return StyleRecoMinusIsin;

                        if (cell.Column.Header.ToString() == "RecoMXUSLC")
                            return StyleRecoIndustry;
                    }
                    return StyleIndustry;
                }

                if (GlobalInfos.isEurope)
                {
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
                }
                else
                {
                    if (row["RecoMXUSLC"].ToString() == "+" && cell.Column.Header.ToString() == "RecoMXUSLC")
                        return StyleRecoPlus;

                    if (row["RecoMXUSLC"].ToString() == "++" && cell.Column.Header.ToString() == "RecoMXUSLC")
                        return StyleRecoPlus;

                    if (row["RecoMXUSLC"].ToString() == "-" && cell.Column.Header.ToString() == "RecoMXUSLC")
                        return StyleRecoMinus;

                    if (cell.Column.Header.ToString() == "RecoMXUSLC")
                        return StyleReco;
                }
            }
            return null;
        }
    }
}
