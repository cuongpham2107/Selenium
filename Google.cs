using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
namespace Slave
{
    public class Google : DriverHelper
    {
         
        public Google(IWebDriver driver)
        {
           this.driver = driver;
        }
        private Actions Actions => new Actions(driver);
        private WebDriverWait wait => new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        private IWebElement emailTextBox => wait.Until(driver => driver.FindElement(By.Id("identifierId")));
        private IWebElement passWordTextBox => wait.Until(driver => driver.FindElement(By.Name("Passwd")));
        public void GoToUrl(string url = "http://accounts.google.com")
        {
            driver.Navigate().GoToUrl(url);
            Extension.WriteLine("Đăng nhập Google", ConsoleColor.Yellow);
        }
        public void EnterEmail(string email)
        {
           
           Actions.Click(emailTextBox) // Nhấp vào phần tử để đảm bảo nó được chọn
                    .KeyDown(Keys.Control)
                    .SendKeys("a")
                    .KeyUp(Keys.Control)
                    .SendKeys(Keys.Delete)
                    .Perform();
            Random r = new();
            foreach(var a in email){
                emailTextBox.SendKeys(a.ToString());
                int pause = r.Next(300,500);
                Thread.Sleep(pause);
            }
            Actions.SendKeys(emailTextBox, Keys.Enter).Perform();
        }
        public void EnterPassword(string password){
            Actions.Click(passWordTextBox) // Nhấp vào phần tử để đảm bảo nó được chọn
                    .KeyDown(Keys.Control)
                    .SendKeys("a")
                    .KeyUp(Keys.Control)
                    .SendKeys(Keys.Delete)
                    .Perform();
            Random r = new();
            foreach(var a in password){
                passWordTextBox.SendKeys(a.ToString());
                int pause = r.Next(300,500);
                Thread.Sleep(pause);
            }
            Actions.SendKeys(passWordTextBox, Keys.Enter).Perform();
            Extension.WriteLine("Đăng nhập Google thành công", ConsoleColor.DarkMagenta);
        }
        public bool CheckLoginGoogle()
        {
            string currentUrl =  driver.Url;
            if(currentUrl != "http://accounts.google.com")
            {
                return true;
            }
            else{
                return false;
            }
        }
    }
}