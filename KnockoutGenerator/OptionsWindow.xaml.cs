using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using AndreasGustafsson.KnockoutGenerator.Model;
using Clipboard = System.Windows.Clipboard;

namespace AndreasGustafsson.KnockoutGenerator
{
    public partial class OptionsWindow : Window
    {
        private ViewModel _generatedJsFile;
        
        private ObservableCollection<JsClassViewModel> _classes;
        public ObservableCollection<JsClassViewModel> Classes
        {
            get { return _classes; }
        }

        public OptionsWindow(ViewModel generatedJsFile)
        {
            _generatedJsFile = generatedJsFile;
            _classes = new ObservableCollection<JsClassViewModel>(generatedJsFile.Classes);
            
            InitializeComponent();

            this.PreviewKeyDown += OptionsWindow_PreviewKeyDown;
            this.DataContext = generatedJsFile;
            generatedJsFile.Generate();
        }

        void OptionsWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void ButtonCopyToClipBoard_Click(object sender, RoutedEventArgs e)
        {

            Clipboard.SetText(_generatedJsFile.GetJsFileContent());
            this.Close();
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            var checkbox = ((System.Windows.Controls.CheckBox)sender);

            if (checkbox == null) return;

            var jsClass = (JsClassViewModel)checkbox.DataContext;

            if (jsClass == null) return;

            foreach (var property in jsClass.Properties)
            {
                property.IsObservable = (bool)checkbox.IsChecked;
            }

        }

        private void ToggleButton_IgnoreClass_OnChecked(object sender, RoutedEventArgs e)
        {
            var checkbox = ((System.Windows.Controls.CheckBox)sender);
            if (checkbox == null) return;

            var jsClass = (JsClassViewModel)checkbox.DataContext;
            if (jsClass == null)
                return;

            jsClass.Enable = (bool)checkbox.IsChecked;
        }
    }
}
