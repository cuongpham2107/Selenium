using OpenQA.Selenium;

namespace Slave
{
    public static class Extension
    {
        public static void WriteLine(string body, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(body);
            Console.ResetColor();
        }

        public static void ScrollTo(IWebDriver driver, int xPosition = 0, int yPosition = 0)
        {
            var js = String.Format("window.scrollTo({0}, {1})", xPosition, yPosition);
            ((IJavaScriptExecutor)driver).ExecuteScript(js);
        }

        public static IWebElement ScrollToView(IWebDriver driver, By selector)
        {
            var element = driver.FindElement(selector);
            ScrollToView(driver, element);
            return element;
        }

        public static void ScrollToView(IWebDriver driver, IWebElement element)
        {
            if (element.Location.Y > 200)
            {
                ScrollTo(driver, 0, element.Location.Y - 100); //Đảm bảo phần tử nằm trong dạng xem nhưng bên dưới ngăn dẫn hướng trên cùng
            }

        }
    }
}