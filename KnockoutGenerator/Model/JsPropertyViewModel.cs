using System.ComponentModel;

namespace AndreasGustafsson.KnockoutGenerator.Model
{
    public class JsPropertyViewModel : INotifyPropertyChanged
    {
        public JsPropertyViewModel(JsClassViewModel parentClass)
        {
            this.ParentClass = parentClass;
        }

        public JsClassViewModel ParentClass { get; set; }

        public string Name { get; set; }
        public bool IsArray { get; set; }

        private bool _ignore;
        public bool Ignore
        {
            get { return _ignore; }
            set { _ignore = value; OnPropertyChanged("Ignore"); }
        }

        private bool _isObservable;
        public bool IsObservable
        {
            get { return _isObservable; }
            set { _isObservable = value; OnPropertyChanged("IsObservable"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }

            this.ParentClass.ParentViewModel.Generate();
        }
    }
}
