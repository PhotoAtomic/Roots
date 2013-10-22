using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAtomic.Reflection
{
    public static class Conversion
    {
        public static bool Try(object value, Type sourceType, Type targetType, out object convertedValue)
        {
            convertedValue = null;

            if (sourceType == targetType)
            {
                convertedValue = value;
                return true;
            }

            var foundConventer = TypeDescriptor.GetConverter(targetType);
            if (foundConventer != null && foundConventer.CanConvertFrom(sourceType))
            {
                convertedValue = foundConventer.ConvertFrom(value);
                return true;
            }

            var foundReverseConventer = TypeDescriptor.GetConverter(sourceType);
            if (foundReverseConventer != null && foundReverseConventer.CanConvertTo(targetType))
            {
                convertedValue = foundConventer.ConvertTo(value, targetType);
                return true;
            }

            return false;
        }
    }
}
