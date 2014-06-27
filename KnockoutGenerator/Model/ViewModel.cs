using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using KnockoutGenerator.Core.Extensions;
using KnockoutGenerator.Core.Models;
using System.Linq;

namespace AndreasGustafsson.KnockoutGenerator.Model
{
    public class ViewModel : INotifyPropertyChanged
    {
        private bool _camelCase;
        public bool CamelCase
        {
            get { return _camelCase; }
            set { _camelCase = value; OnPropertyChanged("CamelCase"); this.Generate(); }
        }

        private bool _pascalCase;
        public bool PascalCase
        {
            get { return _pascalCase; }
            set { _pascalCase = value; OnPropertyChanged("PascalCase"); this.Generate(); }
        }

        public string FullPath { get; set; }
        public string FileName
        {
            get { return Path.GetFileName(FullPath); }
        }

        public string FileNameWithOutExtension
        {
            get { return Path.GetFileNameWithoutExtension(FullPath); }
        }

        public ObservableCollection<JsClassViewModel> Classes { get; set; }

        private string _generatedJsContent;
        public string GeneratedJsContent
        {
            get { return _generatedJsContent; }
            set { _generatedJsContent = value; OnPropertyChanged("GeneratedJsContent"); }
        }


        public void Generate()
        {
            this._generatedJsContent = this.GetJsFileContent();
            OnPropertyChanged("GeneratedJsContent");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }

        }
    }

    public static class
        ViewModelExtensions
    {
        public static string GetJsFileContent(this ViewModel file)
        {
            var str = new StringBuilder();
            foreach (var jsClass in file.Classes)
            {
                if (jsClass.Ignore) continue;

                string className = file.CamelCase ? jsClass.Name.FormatCamelCase() : jsClass.Name;

                str.AppendFormat("function {0}({3}) {1}{2}{1}\tvar self = this;{1}", jsClass.Name, Environment.NewLine, "{", className);

                foreach (var prop in jsClass.Properties)
                {
                    if (prop.Ignore)
                        continue;

                    var indexOfItem = jsClass.Properties.IndexOf(prop);

                    var observableType = prop.IsArray
                                             ? "ko.observableArray" : "ko.observable";

                    var type = prop.IsArray ? "[ ]" : "''";

                    string propertyName = file.CamelCase ? prop.Name.FormatCamelCase() : prop.Name;

                    string dataType;

                    if (!prop.IsObservable)
                    {
                        dataType =
                            string.Format("{0}.{1};", className, propertyName);
                    }
                    else
                    {
                        if (prop.OfType == null || !file.Classes.Any(c => string.CompareOrdinal(c.Name, prop.OfType) == 0))
                        {
                            dataType = string.Format("{2}({0}.{1} || {3});", className, propertyName, observableType, type);
                        }
                        else
                        {
                            if (prop.IsArray)
                            {
                                dataType = string.Format("{2}(({0}.{1} || {3}).map(function (e) {{ return new {4}(e); }}));", className, propertyName, observableType, type, prop.OfType);
                            }
                            else
                            {
                                dataType = string.Format("new {4}({0}.{1} || {{}});", className, propertyName, observableType, type, prop.OfType);
                            }
                        }
                    }

                    var s = (indexOfItem == jsClass.Properties.Count - 1)
                                   ? string.Format("\tself.{0} = {1}", propertyName, dataType)
                                   : string.Format("\tself.{0} = {1}{2}", propertyName, dataType,
                                                   Environment.NewLine);

                    str.Append(s);
                }

                str.AppendFormat(@"{0}{1}{0}{0}", Environment.NewLine, "}");

            }

            return str.ToString();
        }

        public static ViewModel MapViewModelFromJsFile(JsFile jsFile)
        {
            var viewModel = new ViewModel
                {
                    FullPath = jsFile.FullPath,
                    Classes = new ObservableCollection<JsClassViewModel>(),
                    CamelCase = true
                };

            foreach (var file in jsFile.Files)
            {
                var jsClass = new JsClassViewModel(viewModel)
                    {
                        Name = file.Name,
                        Properties = new ObservableCollection<JsPropertyViewModel>(),
                        Ignore = false,
                        Enable = false
                    };

                file.Properties.ForEach(p => jsClass.Properties.Add(new JsPropertyViewModel(jsClass)
                    {
                        Ignore = p.Ignore,
                        Name = p.Name,
                        OfType = p.OfType,
                        IsArray = p.IsArray,
                        IsObservable = p.IsObservable
                    }));

                viewModel.Classes.Add(jsClass);
            }

            return viewModel;
        }
    }
}
