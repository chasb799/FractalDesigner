using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using FractalDesigner.ViewModel;

namespace FractalDesigner
{
    /// <summary>
    /// Code-behind for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new FractalVM();
        }

        /// <summary>
        /// Check if the input is a number.
        /// </summary>       
        private void InputValidation_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }
    }
}
