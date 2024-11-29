using System.Globalization;

namespace mouse_lighting.Resources.Converters
{
    internal class ConverterDebugger : Converter
    {
        public override object Convert(object v, Type t, object p, CultureInfo c)
        {
            System.Diagnostics.Debug.WriteLine($"v = {v} type {t} p = {p} c = {c}");
            return v;
        }
        public override object ConvertBack(object v, Type t, object p, CultureInfo c)
        {
            System.Diagnostics.Debug.WriteLine($"v = {v} type {t} p = {p} c = {c}");
            return v;
        }
    }
}
