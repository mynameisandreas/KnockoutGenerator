using ICSharpCode.NRefactory;
using KnockoutGenerator.Core.Models;

namespace KnockoutGenerator.Core.Contracts
{
    public interface ICodeGenerator
    {
        JsFile GetJsFileFromCodeFile(string path, SupportedLanguage language);
        JsFile GetJsFileFromCodeSnippet(string codeSnippet, SupportedLanguage language);
    }
}
