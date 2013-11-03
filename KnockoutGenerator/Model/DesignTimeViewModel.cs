using System.Collections.ObjectModel;

namespace AndreasGustafsson.KnockoutGenerator.Model
{
    public class DesignTimeViewModel : ViewModel
    {
        public DesignTimeViewModel()
        {
            this.FullPath = "FooBar.cs";
            this.Classes = new ObservableCollection<JsClassViewModel>();

            var jsClass = new JsClassViewModel(this) {Name = "Foo"};

            jsClass.Properties = new ObservableCollection<JsPropertyViewModel>()
                {
                    new JsPropertyViewModel(jsClass) {Name = "Foo1ReallyLongPropertyNameWhatIsHappeningNow"},
                    new JsPropertyViewModel(jsClass) {Name = "Foo2"},
                    new JsPropertyViewModel(jsClass) {Name = "Foo3"}
                };
        
            jsClass.Name = "Bar";
            jsClass.Properties = new ObservableCollection<JsPropertyViewModel>()
                {
                    new JsPropertyViewModel(jsClass) {Name = "Foo1ReallyLongPropertyNameWhatIsHappeningNow"},
                    new JsPropertyViewModel(jsClass) {Name = "Foo2"},
                    new JsPropertyViewModel(jsClass) {Name = "Foo3"}
                };
                
        }
    }

}
