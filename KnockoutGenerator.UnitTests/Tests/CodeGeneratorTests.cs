using System;
using System.IO;
using System.Linq;
using ICSharpCode.NRefactory;
using KnockoutGenerator.Core.Contracts;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace KnockoutGenerator.UnitTests.Tests
{
    [TestFixture]
    public class CodeGeneratorTests : BaseTestSetup
    {
        private ICodeGenerator _codeGenerator;

        [SetUp]
        public void TestSetUp()
        {
            _codeGenerator = container.Resolve<ICodeGenerator>();
        }

        [TestCase("Test.vb", 4)]
        [TestCase("Test.cs", 3)]
        public void CanGetPropertiesFromCodeFile(string fileName, int expected)
        {
            var path = GetPathToFile(fileName);

            var fileExtension = Path.GetExtension(path);
            var language = fileExtension.Equals(".cs") ? SupportedLanguage.CSharp : SupportedLanguage.VBNet;
            var jsFiles = _codeGenerator.GetJsFileFromCodeFile(path, language);

            Assert.AreEqual(expected, jsFiles.Files.FirstOrDefault().Properties.Count);
        }

        [Test]
        public void CanResolveArrayPropertyAsJavascriptArray()
        {
            var path = GetPathToFile("Array.cs");

            var jsFile = _codeGenerator.GetJsFileFromCodeFile(path, SupportedLanguage.CSharp);

            Assert.AreEqual(1, jsFile.Files.FirstOrDefault().Properties.Count);

            var property = jsFile.Files.FirstOrDefault().Properties.FirstOrDefault();
            Assert.IsTrue(property.IsArray);
        }


        [Test]
        public void CanParseCSharpSnippetFromSelection()
        {
            var codeSnippet = "public class RebateSchemeRank\r\n        {\r\n            public Guid Id { get; set; }\r\n            public string Name { get; set; }\r\n           public int Rank { get; set; }\r\n            public List<RebateSchemeRankBrand> IncludedVolumes { get; set; }\r\n            public List<RebateSchemeRankBrand> IncludedPayouts { get; set; }\r\n        }";

            var jsFile = _codeGenerator.GetJsFileFromCodeSnippet(codeSnippet, SupportedLanguage.CSharp);

            Assert.AreEqual(1, jsFile.Files.Count);
            Assert.AreEqual(5, jsFile.Files.FirstOrDefault().Properties.Count);
        }

        [Test]
        public void CanNotParseCSharpSnippetThatHasParseErrorsFromSelection()
        {
            var codeSnippet = "public clas RebateSchemeRank\r\n        {\r\n            public Guid Id { get; set; }\r\n            public string Name { get; set; }\r\n           public int Rank { get; set; }\r\n            public List<RebateSchemeRankBrand> IncludedVolumes { get; set; }\r\n            public List<RebateSchemeRankBrand> IncludedPayouts { get; set; }\r\n        ";

            var jsFile = _codeGenerator.GetJsFileFromCodeSnippet(codeSnippet, SupportedLanguage.CSharp);
            Assert.IsTrue(jsFile == null);
        }


        [Test]
        [Ignore]
        public void CanGetPropertiesFromBaseClass()
        {
            /*
             Not implemented
             */
            var path = GetPathToFile("Inheritance.cs");
            var jsFile = _codeGenerator.GetJsFileFromCodeFile(path, SupportedLanguage.CSharp);

            Assert.AreEqual(3, jsFile.Files[0].Properties.Count);
            Assert.AreEqual(3, jsFile.Files[1].Properties.Count);

        }


        private string GetPathToFile(string fileName)
        {
            return Path.Combine(Environment.CurrentDirectory, "TestFiles", fileName);
        }
    }
}
