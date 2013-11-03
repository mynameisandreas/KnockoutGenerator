using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AndreasGustafsson.KnockoutGenerator.Model
{
    public class JsClassViewModel : INotifyPropertyChanged
    {

        public JsClassViewModel(ViewModel parentViewModel)
        {
            this.ParentViewModel = parentViewModel;
        }

        public ViewModel ParentViewModel { get; set; }

        public string Name { get; set; }
        public ObservableCollection<JsPropertyViewModel> Properties { get; set; }

        private bool _ignore;
        public bool Ignore
        {
            get { return _ignore; }
            set { _ignore = value; OnPropertyChanged("Ignore"); }
        }

        public bool Visible
        {
            get { return !Ignore; }
        }

        private bool _enable;
        public bool Enable
        {
            get { return !_enable; }
            set { _enable = value; OnPropertyChanged("Enable"); }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }

            this.ParentViewModel.Generate();
        }
    }
}
