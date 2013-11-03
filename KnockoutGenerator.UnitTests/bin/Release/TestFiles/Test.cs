using System;

namespace ClassLibrary1
{
    public class Test : Inner
    {
        public string Foo { get; set; }
        public int Bar { get; set; }
        public DateTime SystemTime { get; set; }

        private string Andreas { get; set; }

        public void TestMethod()
        {
            var andreas = 11;
        }

    }

    public class Inner
    {
        public string InnerProp { get; set; }
    }
}
