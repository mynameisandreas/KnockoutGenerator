using System.ComponentModel;
using System.Windows;

namespace AndreasGustafsson.KnockoutGenerator.Model
{
    public class ViewModelProvider
    {
        public ViewModel ViewModel
        {
            get
            {
                return
                    DesignerProperties.GetIsInDesignMode(new DependencyObject()) ?
                    new DesignTimeViewModel() : new ViewModel();
            }
        }
    }
}
