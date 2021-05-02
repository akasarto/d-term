using MaterialDesignThemes.Wpf;
using ReactiveUI;
using System;

namespace dTerm.UI.Wpf.Converters
{
    public class PackIconKindConverter : IBindingTypeConverter
    {
        public int GetAffinityForObjects(Type fromType, Type toType)
        {
            if (fromType == typeof(string) && toType == typeof(PackIconKind))
            {
                return 100;
            }

            return 0;
        }

        public bool TryConvert(object from, Type toType, object conversionHint, out object result)
        {
            if (!Enum.TryParse<PackIconKind>((from ?? string.Empty).ToString(), ignoreCase: true, out var kind))
            {
                kind = PackIconKind.QuestionMark;
            }

            result = kind;

            return true;
        }
    }
}
