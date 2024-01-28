using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

class Program
{
    static void Main()
    {
        // Chrome WebDriver'ı başlat
        IWebDriver driver = new ChromeDriver();

        try
        {
            // Test senaryosu 1: "From" ve "To" input alanlarında aynı değerin girilemediğini test et
            TestScenario1(driver);

            // Test senaryosu 2: "Found X items" yazısında X sayısı ile listelenen uçuş sayısının aynı olduğunu test et
            TestScenario2(driver);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Test senaryosu başarısız. Hata: {ex.Message}");
        }
        finally
        {
            // WebDriver'ı kapat
            driver.Quit();
        }
    }

    static void TestScenario1(IWebDriver driver)
    {
        // Test senaryosu 1: "From" ve "To" input alanlarında aynı değerin girilemediğini test et
        driver.Navigate().GoToUrl("https://flights-app.pages.dev");

        // Aynı şehirleri seçerek submit işlemi yap
        IWebElement fromInput = driver.FindElement(By.Id("headlessui-combobox-button-:R1a9lla:"));
        IWebElement toInput = driver.FindElement(By.Id("headlessui-combobox-button-:R1ahlla:"));

        fromInput.SendKeys("New York");
        toInput.SendKeys("New York");

        // Hata mesajının görüntülendiğini kontrol et
        IWebElement errorMessage = driver.FindElement(By.Id("errorMessage"));
        if (errorMessage.Text.Contains("Aynı şehir seçilemez"))
        {
            Console.WriteLine("Test senaryosu 1 başarıyla geçti.");
        }
        else
        {
            throw new Exception("Test senaryosu 1 başarısız.");
        }
    }

    static void TestScenario2(IWebDriver driver)
    {
        // Test senaryosu 2: "Found X items" yazısında X sayısı ile listelenen uçuş sayısının aynı olduğunu test et
        driver.Navigate().GoToUrl("https://flights-app.pages.dev");

        // Şehir seçimleri yaparak submit işlemi yap
        IWebElement fromInput = driver.FindElement(By.Id("headlessui-combobox-button-:R1a9lla:"));
        IWebElement toInput = driver.FindElement(By.Id("headlessui-combobox-button-:R1ahlla:"));

        fromInput.SendKeys("Istanbul");
        toInput.SendKeys("Los Angeles");
        submitButton.Click();

        // Listelenen uçuş sayısını kontrol et
        IWebElement foundItemsMessage = driver.FindElement(By.Id("foundItemsMessage"));
        int foundItemsCount = int.Parse(foundItemsMessage.Text.Split(' ')[1]);

        // Gerçek uçuş sayısını kontrol et
        IWebElement flightsList = driver.FindElement(By.Id("flightsList"));
        int actualFlightsCount = flightsList.FindElements(By.CssSelector(".flight-item")).Count;

        // Kontrolleri yap
        if (foundItemsCount == actualFlightsCount)
        {
            Console.WriteLine("Test senaryosu 2 başarıyla geçti.");
        }
        else
        {
            throw new Exception("Test senaryosu 2 başarısız.");
        }
    }
}
