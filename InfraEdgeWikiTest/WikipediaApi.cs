using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Html.Parser;

public class WikipediaService
{
    private static readonly HttpClient httpClient = new HttpClient();

    public async Task<string> GetSectionContentAsync(string pageTitle, string sectionName)
    {
        string baseUrl = "https://en.wikipedia.org/w/api.php";
        string query = $"?action=parse&format=json&page={Uri.EscapeDataString(pageTitle)}&prop=text";
        string url = baseUrl + query;

        HttpResponseMessage response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();

        // Parse the JSON response to extract HTML content
        var htmlContent = ExtractHtmlContentFromJson(responseBody);

        // Extract the section content from the HTML
        return ExtractSection(htmlContent, sectionName);
    }

    private static string ExtractHtmlContentFromJson(string jsonResponse)
    {
        // Extract HTML content from the JSON response
        var json = Newtonsoft.Json.Linq.JObject.Parse(jsonResponse);
        return json["parse"]["text"]["*"].ToString();
    }

    private static string ExtractSection(string htmlContent, string sectionName)
    {
        var config = Configuration.Default;
        var context = BrowsingContext.New(config);
        var parser = context.GetService<IHtmlParser>();
        var document = parser.ParseDocumentAsync(htmlContent).Result;

        // Find the section header
        var sectionHeader = document.QuerySelector($"#{sectionName}");
        if (sectionHeader == null)
        {
            return $"Section '{sectionName}' not found.";
        }

        // Extract content under the section header
        var parentElement = sectionHeader.ParentElement;
        if (parentElement == null)
        {
            return "Parent element not found.";
        }

        // Find the next sibling element that is a paragraph
        var nextParagraph = parentElement.NextElementSibling;
        while (nextParagraph != null && nextParagraph.TagName != "P")
        {
            nextParagraph = nextParagraph.NextElementSibling;
        }

        return nextParagraph != null ? nextParagraph.TextContent.Trim() : "Following paragraph not found.";
    }

}
