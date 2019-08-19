using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;

namespace FrontV2.Action
{
    [TypeConverter(typeof(DoubleFormattedConverter))]
    public class DoubleFormatted : IEquatable<DoubleFormatted>, IComparable
    {
        private String _origin = "";
        private double _number = 0;

        public String Origin
        {
            get { return _origin; }
        }
        public double Number
        {
            get { return _number; }
        }

        public DoubleFormatted(String intString)
        {
            _origin = intString;

            String str = intString;

            if (CultureInfo.CurrentCulture.Name.ToLower().Contains("fr")
                && CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ",")
                str = str.Replace(".", ",");

            if (CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator == ",")
            {
                str = str.Replace(" ", ",");
                str = str.Substring(1, str.Length - 1);
            }


            if (str == "")
                _number = 0;
            else
            {
                try
                {
                    if (str.Contains("(") && str.Contains(")"))
                    {
                        if (str.Contains("%"))
                        {
                            str = str.Replace("(", "");
                            str = str.Replace(")", "");
                            str = str.Replace("%", "");
                            _number = -1 * double.Parse(str);
                        }
                        else if (str.Contains("x"))
                        {
                            str = str.Replace("(", "");
                            str = str.Replace(")", "");
                            str = str.Replace("x", "");
                            _number = -1 * double.Parse(str);
                        }
                        else
                        {
                            str = str.Replace("(", "");
                            str = str.Replace(")", "");
                            str = str.Replace("%", "");
                            _number = -1 * double.Parse(str);
                        }
                    }
                    else if (str.Contains("x"))
                    {
                        str = str.Replace("x", "");
                        _number = double.Parse(str);
                    }
                    else if (str.Contains("%"))
                    {
                        str = str.Replace("%", "");
                        _number = double.Parse(str);
                    }
                    else
                        _number = double.Parse(str);

                }
                catch (Exception e)
                {
                    MessageBox.Show("exception sur: " + _origin);
                    MessageBox.Show(e.ToString());

                    _number = 0;
                    _origin = "Erreur";
                }
            }
        }

        bool IEquatable<DoubleFormatted>.Equals(DoubleFormatted other)
        {
            if (other == null)
                return false;

            return this.Number == other.Number;
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<DoubleFormatted>)this).Equals(obj as DoubleFormatted);
        }

        public override string ToString()
        {
            if (_origin == "")
                return "";
            return this.Origin;
        }

        public override int GetHashCode()
        {
            return this.Number.GetHashCode() ^ this.Origin.GetHashCode();
        }

        public static bool operator <(DoubleFormatted left, DoubleFormatted right)
        {
            return left.Number < right.Number;
        }

        public static bool operator <=(DoubleFormatted left, DoubleFormatted right)
        {
            return left.Number <= right.Number;
        }

        public static bool operator >(DoubleFormatted left, DoubleFormatted right)
        {
            return left.Number > right.Number;
        }

        public static bool operator >=(DoubleFormatted left, DoubleFormatted right)
        {
            return left.Number >= right.Number;
        }

        int IComparable.CompareTo(object obj)
        {
            return this.Number.CompareTo(((DoubleFormatted)obj).Number);
        }
    }
}
