using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace InfraEdgeWikiTest
{
    public class Tests
    {
        private IWebDriver driver;
        private WikipediaService wikipediaService;


        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            wikipediaService = new WikipediaService();

        }

        public int SetUniqueDate(string input)
        {
            UniqueWordExtractor extractor = new UniqueWordExtractor(input);
            Dictionary<string, int> wordOccurrences = extractor.ExtractUniqueWordsWithOccurrences();
            foreach (var kvp in wordOccurrences)
            {
                TestContext.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
            return wordOccurrences.Count;
        }

        public string GetDataFromDom()
        {
            WebPageTextExtractor webPage = new WebPageTextExtractor(driver);
            return webPage.ExtractTextFromSection();
        }

        public async Task<string> RetrieveSectionContent()
        {
            string pageTitle = "Test_automation";
            string sectionName = "Methodologies";
            string result = await wikipediaService.GetSectionContentAsync(pageTitle, sectionName);
            return result;
            }

        [Test]
        public void CompareApiAndSelenium()
        {
            string dataFromDom = GetDataFromDom();
            string dateFromApi = RetrieveSectionContent().GetAwaiter().GetResult();
            int uniqueValuesFromDom = SetUniqueDate(dataFromDom);
            int uniqueValuesFromApi = SetUniqueDate(dataFromDom);
            Assert.AreEqual(uniqueValuesFromDom, uniqueValuesFromApi);
        }

        [TearDown]
        public void Teardown()
         {
            driver.Quit(); // Close the browser and end the session
        }
    }
}