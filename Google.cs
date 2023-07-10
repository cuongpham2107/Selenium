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
        private WebDriverWait wait => new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        private IWebElement emailTextBox => wait.Until(driver => driver.FindElement(By.Id("identifierId")));
        private IWebElement passWordTextBox => wait.Until(driver => driver.FindElement(By.Name("Passwd")));
        private IWebElement elementCheckLogin => wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[2]/div/c-wiz/div/div[1]/div/h1/span")));
        public IWebElement elementAccount => wait.Until(driver => driver.FindElement(By.XPath("/html/body")));
        public void GoToUrl(string url = "http://accounts.google.com")
        {
            driver.Navigate().GoToUrl(url);
            Extension.WriteLine("Đăng nhập Google", ConsoleColor.Yellow);
        }
        public void EnterEmail(string email)
        {
            try
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
            catch (Exception e)
            {
                Extension.WriteLine($"Lỗi: {e}", ConsoleColor.Red);
            }
           
        }
        public void EnterPassword(string password){
            try
            {
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
            catch (Exception e)
            {
                Extension.WriteLine($"Lỗi: {e}", ConsoleColor.Red);
            }
            
        }
        public bool CheckLoginGoogle()
        {
            try
            {
                string check =  elementCheckLogin.Text;
                if(check == "Đăng nhập")
                {
                    return true;
                }
                else{
                    return false;
                }
            }
            catch (Exception e)
            {
                Extension.WriteLine($"Lỗi {e}",ConsoleColor.Red);
                return false;
            }
            
        }
    }
}