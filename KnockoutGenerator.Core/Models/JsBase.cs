using System.Collections.Generic;

namespace KnockoutGenerator.Core.Models
{
    public class JsBase 
    {
        public string Name { get; set; }
        public List<JsAttribute> Attributes { get; set; }
        public bool Ignore { get; set; }
    }
}
