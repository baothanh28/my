using System.Threading.Tasks;

namespace My.CrossCuttingConcerns.HtmlGenerator;

public interface IHtmlGenerator
{
    Task<string> GenerateAsync(string template, object model);
}
