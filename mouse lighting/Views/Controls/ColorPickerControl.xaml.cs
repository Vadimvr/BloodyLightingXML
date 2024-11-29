using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace mouse_lighting.Views.Controls
{
    /// <summary>
    /// Логика взаимодействия для ColorPickerControl.xaml
    /// </summary>
    public partial class ColorPickerControl 
    {
        #region PropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? PropertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        protected bool Set<T>(ref T filed, T value, [CallerMemberName] string? PropertyName = null)
        {
            if (Equals(filed, value)) return false;

            filed = value;
            OnPropertyChanged(PropertyName);
            return true;
        }
        #endregion
        public ColorPickerControl()
        {
            InitializeComponent();
        }



        public int FontSizeTextBox
        {
            get { return (int)GetValue(FontSizeTextBoxProperty); }
            set { SetValue(FontSizeTextBoxProperty, value); }
        }

        public static readonly DependencyProperty FontSizeTextBoxProperty =
            DependencyProperty.Register(nameof(FontSizeTextBox), typeof(int), typeof(ColorPickerControl), new PropertyMetadata(12));



        public string ColorString
        {
            get => (string)GetValue(ColorStringProperty);

            set
            {
              //  Debug.WriteLine($"SetValue(ColorStringProperty, value); {value}");
                SetValue(ColorStringProperty, value);
            }
        }

        public static readonly DependencyProperty ColorStringProperty =
            DependencyProperty.Register(
               nameof(ColorString),
                typeof(string),
                typeof(ColorPickerControl),
                new PropertyMetadata(default!, OnColorStringChanged));
        private static void OnColorStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
          //  Debug.WriteLine(e.NewValue);
            var c = d as ColorPickerControl;
            var n = e.NewValue as string;
            if (c != null && !string.IsNullOrEmpty(n)) { c.ColorBackGroundBorder = n; }
        }



        public string ColorBackGroundBorder
        {
            get { return (string)GetValue(ColorBackGroundBorderProperty); }
            set { SetValue(ColorBackGroundBorderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColorBackGroundBorder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorBackGroundBorderProperty =
            DependencyProperty.Register(
                nameof(ColorBackGroundBorder),
                typeof(string),
                typeof(ColorPickerControl),
                new PropertyMetadata("black", (a, b) => { }, CorrectColorBackGroundBorder));

        //private static int i;
        private static object CorrectColorBackGroundBorder(DependencyObject d, object baseValue)
        {
            var s = baseValue as string;
            if (!string.IsNullOrEmpty(s) && (Regex.IsMatch(s) || dictionary.ContainsKey(s.ToLower()))) { return baseValue; }
            return "Transparent";
        }

        #region Get all named colors
        private static readonly Dictionary<string, Color> dictionary =
                  typeof(Colors).GetProperties(BindingFlags.Public | BindingFlags.Static)
                 .Where(prop => prop.PropertyType == typeof(Color))
                 .ToDictionary(prop => prop.Name.ToLower(),
                               prop =>
                               {
                                   var c = prop.GetValue(null, null);
                                   if (c is Color)
                                       return (Color)c;
                                   return default!;
                               });

        static Regex Regex = new Regex("^#(?:[0-9a-fA-F]{4}){1,2}$");
        #endregion
    }
}
