using System;

namespace FrontV2.Action
{
    class DoubleFormattedConverter : System.ComponentModel.TypeConverter
    {
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context,
            Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture, object value)
        {
            var stringValue = value as string;
            if (stringValue != null)
            {
                return new DoubleFormatted(stringValue);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context,
            Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return ((DoubleFormatted)value).Origin;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }
}
