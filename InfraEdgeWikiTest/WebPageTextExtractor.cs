
using InfraEdgeWikiTest;
using OpenQA.Selenium;

public class WebPageTextExtractor
{
    private readonly IWebDriver driver;

    const string UiPath = "https://en.wikipedia.org/wiki/Test_automation";
    public WebPageTextExtractor(IWebDriver driver)
    {
        this.driver = driver;
    }

    public string ExtractTextFromSection()
    {
        // Navigate to the web page
        driver.Navigate().GoToUrl(UiPath);

        // Locate the section of the page from which you want to extract text
        IWebElement title = driver.FindElement(By.XPath("//*[@id=\"Methodologies\"]"));
        string titleText = title.Text;
        IWebElement titleParentElement = driver.FindElement(By.XPath("//*[@id='Methodologies']/.."));
        IWebElement paragraph = titleParentElement.FindElement(By.XPath("following-sibling::p"));
        string paragraphText = paragraph.Text;
        string jointText = titleText + paragraphText;
        // Get the text from the section
        return jointText;
    }
}
