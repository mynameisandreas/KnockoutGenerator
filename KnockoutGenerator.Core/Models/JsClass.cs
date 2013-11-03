using System.Collections.Generic;

namespace KnockoutGenerator.Core.Models
{
    public class JsClass : JsBase
    {
        public List<JsProperty> Properties { get; set; }
        public bool Enable { get; set; }
    }
}
