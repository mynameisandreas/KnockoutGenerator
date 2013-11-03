namespace KnockoutGenerator.UnitTests.TestFiles
{
    public class BaseClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAwesome { get; set; }
    }

    public class Inheritance : BaseClass
    {
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
    }
}
