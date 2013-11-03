using System.IO;
using ICSharpCode.NRefactory;
using KnockoutGenerator.Core.Contracts;
using KnockoutGenerator.Core.Models;

namespace KnockoutGenerator.Core.Business
{
    sealed class CodeGenerator : ICodeGenerator
    {
        public JsFile GetJsFileFromCodeFile(string path, SupportedLanguage language)
        {
            TextReader textReader = File.OpenText(path);

            var file = new JsFile();

            using (var parser = ParserFactory.CreateParser(language, textReader))
            {
                var jsFile = new JsFile
                {
                    FullPath = path
                };

                parser.Parse();

                if (parser.Errors.Count <= 0)
                {
                    var visitor = new AstVisitor
                    {
                        Model = jsFile
                    };

                    parser.CompilationUnit.AcceptVisitor(visitor, null);

                    file = visitor.Model;
                    return file;
                }
            }

            return null;
        }

        public JsFile GetJsFileFromCodeSnippet(string codeSnippet, SupportedLanguage language)
        {
            var file = new JsFile();

            var snippetParser = new SnippetParser(language);

            var jsFile = new JsFile
            {
                FullPath = string.Empty
            };

            var parsedNode = snippetParser.Parse(codeSnippet);

            if (parsedNode.Children.Count > 0)
            {
                var visitor = new AstVisitor
                {
                    Model = jsFile
                };

                parsedNode.AcceptVisitor(visitor, null);

                file = visitor.Model;
                return file;
            }

            return null;
        }
    }
}
