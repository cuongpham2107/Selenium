using System;
using System.Xml.Linq;
using Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace Slave
{
    public class Youtobe : DriverHelper
    {
        public Youtobe(IWebDriver driver)
        {
            this.driver = driver;
        }
        static ConfigYoutobe _config = ConfigManager<ConfigYoutobe>.Instance.Config;
        private Actions Actions => new Actions(driver);
        private WebDriverWait wait => new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        private By elementButtonSearch => By.XPath(_config.ButtonIconSearch);
        private By elementYoutobeSearch => By.Name(_config.InputSearch);
        private By elementFirstVideo => By.CssSelector(_config.FirstVideo);
        private By elementLikeButton => By.XPath(_config.LikeVideo);
        private By elementSubscribeButton => By.XPath(_config.SubscribeVideo);
        private By elementInputComment => By.XPath(_config.Commnent);
        private By elementInputCommentText => By.XPath(_config.InputComment);
        private By elementButtonComment => By.XPath(_config.ButtonComment);
        private By elementLinkTabRight => By.CssSelector("");
        private By elementChannelTabRight => By.CssSelector(_config.ChannelTabsRight);
        private By elementChannelCurrent => By.XPath(_config.ChannelCurrent);
        public void GotoUrl(string url = "https://www.youtube.com")
        {
            driver.Navigate().GoToUrl(url);
        }
        public void SearchKeyword(string keyword)
        {
            try
            {
                IWebElement buttonSearch = wait.Until(driver => driver.FindElement(elementButtonSearch));
                Actions.MoveToElement(buttonSearch).Click().Perform();
                IWebElement element = wait.Until(driver => driver.FindElement(elementYoutobeSearch));
                Actions.MoveToElement(element).Click() // Nhấp vào phần tử để đảm bảo nó được chọn
                        .KeyDown(Keys.Control)
                        .SendKeys("a")
                        .KeyUp(Keys.Control)
                        .SendKeys(Keys.Delete)
                        .Perform();
                Random r = new();
                foreach (var a in keyword)
                {
                    element.SendKeys(a.ToString());
                    int pause = r.Next(100, 300);
                    Thread.Sleep(pause);
                }
                Actions.SendKeys(element, Keys.Enter).Perform();
                Thread.Sleep(2000);
                ChooseVideo();
            }
            catch (NoSuchElementException)
            {
                Extension.WriteLine("Không tìm thấy phần tử.", ConsoleColor.Red);
            }
        }
        public void ChooseVideo()
        {
            try
            {
                IWebElement element = wait.Until(driver => driver.FindElement(elementFirstVideo));
                Thread.Sleep(2000);
                Actions.MoveToElement(element).Click().Perform();
                // Thread.Sleep(TimeSpan.FromMinutes(time));
            }
            catch (NoSuchElementException)
            {
                Extension.WriteLine("Không tìm thấy phần tử.", ConsoleColor.Red);
            }
        }
        public void LikeVideo()
        {
            Random r = new Random();
            int yPst = 0;
            for (int i = 0; i < 5; i++)
            {
                yPst += 500;
                Extension.ScrollTo(driver, xPosition: 0, yPosition: yPst);
                int pause = r.Next(500, 2000);
                Thread.Sleep(pause);
            }
            try
            {
                IWebElement element = wait.Until(driver => driver.FindElement(elementLikeButton));
                string classAttribute = element.GetAttribute("aria-pressed");
                if (classAttribute == "false")
                {
                    Actions.MoveToElement(element).Click().Perform();
                    Extension.WriteLine("Đã thích video thành công", ConsoleColor.Blue);
                }
                else
                {
                    Extension.WriteLine("The video has been liked !!!", ConsoleColor.Yellow);
                }
            }
            catch (Exception e)
            {
                Extension.WriteLine($"Lỗi: {e}", ConsoleColor.Red);
            }
        }
        public void Subscribe()
        {
            try
            {
                IWebElement element = wait.Until(driver => driver.FindElement(elementSubscribeButton));
                // Lấy văn bản trong nút "Subcribe"
                IWebElement checkSubcribe = wait.Until(driver => driver.FindElement(By.XPath("/html/body/ytd-app/div[1]/ytd-page-manager/ytd-watch-flexy/div[5]/div[1]/div/div[2]/ytd-watch-metadata/div/div[2]/div[1]/div/ytd-subscribe-button-renderer/yt-smartimation/yt-button-shape/button/div/span")));

                if (string.Equals(checkSubcribe.Text, "Đăng ký", StringComparison.OrdinalIgnoreCase) == true)
                {
                    Actions.MoveToElement(element).Click().Perform();
                    Extension.WriteLine("Đã đăng ký video thành công", ConsoleColor.Blue);
                  
                }
                else
                {
                    Extension.WriteLine("Đã đăng ký channel rồi", ConsoleColor.Yellow);
                }
            }
            catch (Exception e)
            {
                Extension.WriteLine($"Lỗi: {e}", ConsoleColor.Red);
            }
        }
        public void CommentVideo(string[] comments, string[] icons)
        {
            Random r = new Random();
            int yPst = 0;
            for (int i = 0; i < 10; i++)
            {
                yPst += 500;
                Extension.ScrollTo(driver, xPosition: 0, yPosition: yPst);
                int pause = r.Next(500, 2000);
                Thread.Sleep(pause);
            }

            if (comments.Count() > 0 || icons.Count() > 0)
            {
               
                int indexComment = r.Next(0, comments.Length);
                int indexIcon = r.Next(0, icons.Length);
                string randomComment = $"{comments[indexComment]} {icons[indexIcon]}";
                try
                {
                   
                    IWebElement elementInput = wait.Until(driver => driver.FindElement(elementInputComment));
                    
                    
                    Actions.MoveToElement(elementInput).Click().Perform();

                    IWebElement elementCommentText = wait.Until(driver => driver.FindElement(elementInputCommentText));

                   
                     Actions.MoveToElement(elementCommentText).Click() // Nhấp vào phần tử để đảm bảo nó được chọn
                            .KeyDown(Keys.Control)
                            .SendKeys("a")
                            .KeyUp(Keys.Control)
                            .SendKeys(Keys.Delete)
                            .Perform();
                    foreach (var a in randomComment)
                    {
                        elementCommentText.SendKeys(a.ToString());
                        int pause = r.Next(300, 500);
                        Thread.Sleep(pause);
                    }
                    IWebElement elementButton = wait.Until(driver => driver.FindElement(elementButtonComment));
                    Actions.MoveToElement(elementButton).Click().Perform();
                    Extension.WriteLine("Comment video thành công.", ConsoleColor.Blue);
                }
                catch (Exception e)
                {
                    Extension.WriteLine($"Comment không thành công {e}", ConsoleColor.Red);
                }
            }
        }
        public void ChooseLinkTabsVideo(string[] urls)
        {
            List<IWebElement> elements = wait.Until(driver => driver.FindElements(elementLinkTabRight).ToList());
            bool found = false;
            foreach (IWebElement item in elements)
            {
                string href = item.GetAttribute("href");
                Console.WriteLine(href);
                foreach (string url in urls)
                {
                    if (href == url)
                    {
                        Console.WriteLine("===========");
                        Console.WriteLine(href, url);
                        Actions.MoveToElement(item).Click().Perform();
                        found = true;
                        break;
                    }
                }
                if (found)
                {
                    break;
                }
            }
            //driver.Quit();
        }
        public void ChannelTabVideo(string channel)
        {
            try
            {
                IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
                //Current Channel
                IWebElement element = wait.Until(driver => driver.FindElement(elementChannelCurrent));
                string currentChannelName = (string)jsExecutor.ExecuteScript("return arguments[0].textContent;", element);
                
                //List Channel Tabs Right
                List<IWebElement> elements = wait.Until(driver => driver.FindElements(elementChannelTabRight)).ToList();

                foreach(WebElement item in elements)
                {
                    string channelName = (string)jsExecutor.ExecuteScript("return arguments[0].textContent;", item);
                    
                    if(channelName.Contains(channel) == true)
                    {
                        Extension.WriteLine($"Chuyển xem video tiếp theo của kênh {channel} thành công", ConsoleColor.Blue);
                        Actions.MoveToElement(item).Click().Perform();
                        break;
                    }
                    else if(channelName.Contains(currentChannelName) == true)
                    {
                        Extension.WriteLine($"Không tìm thấy kênh {channel}", ConsoleColor.Red);
                        Extension.WriteLine($"Chuyển xem video tiếp theo của kênh {currentChannelName} thành công", ConsoleColor.Blue);
                        Actions.MoveToElement(item).Click().Perform();
                        break;
                    }
                }
            }
            catch (System.Exception e)
            {
                Extension.WriteLine($"Chuyển xem video tiếp thêm không thành công {e}", ConsoleColor.Red);
            }
        }
    }
}