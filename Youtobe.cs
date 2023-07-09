using System;
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
        private Actions Actions => new Actions(driver);
        private WebDriverWait wait => new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        private By elementYoutobeSearch => By.Name("search_query");
        private By elementFirstVideo => By.CssSelector("ytd-video-renderer");
        private By elementLikeButton => By.XPath("/html/body/ytd-app/div[1]/ytd-page-manager/ytd-watch-flexy/div[5]/div[1]/div/div[2]/ytd-watch-metadata/div/div[2]/div[2]/div/div/ytd-menu-renderer/div[1]/ytd-segmented-like-dislike-button-renderer/yt-smartimation/div/div[1]/ytd-toggle-button-renderer/yt-button-shape/button");
        private By elementSubscribeButton => By.XPath("/html/body/ytd-app/div[1]/ytd-page-manager/ytd-watch-flexy/div[5]/div[1]/div/div[2]/ytd-watch-metadata/div/div[2]/div[1]/div/ytd-subscribe-button-renderer/yt-smartimation/yt-button-shape/button");
        private By elementInputComment => By.XPath("/html/body/ytd-app/div[1]/ytd-page-manager/ytd-watch-flexy/div[5]/div[1]/div/div[2]/ytd-comments/ytd-item-section-renderer/div[1]/ytd-comments-header-renderer/div[5]/ytd-comment-simplebox-renderer/div[1]");
        private By elementInputCommentText => By.XPath("/html/body/ytd-app/div[1]/ytd-page-manager/ytd-watch-flexy/div[5]/div[1]/div/div[2]/ytd-comments/ytd-item-section-renderer/div[1]/ytd-comments-header-renderer/div[5]/ytd-comment-simplebox-renderer/div[3]/ytd-comment-dialog-renderer/ytd-commentbox/div[2]/div/div[2]/tp-yt-paper-input-container/div[2]/div/div[1]/ytd-emoji-input/yt-user-mention-autosuggest-input/yt-formatted-string/div");
        private By elementButtonComment => By.XPath("/html/body/ytd-app/div[1]/ytd-page-manager/ytd-watch-flexy/div[5]/div[1]/div/div[2]/ytd-comments/ytd-item-section-renderer/div[1]/ytd-comments-header-renderer/div[5]/ytd-comment-simplebox-renderer/div[3]/ytd-comment-dialog-renderer/ytd-commentbox/div[2]/div/div[4]/div[5]/ytd-button-renderer[2]/yt-button-shape/button");
        private By elementLinkTabRight => By.CssSelector(".yt-simple-endpoint.inline-block.style-scope.ytd-thumbnail");
        private By elementChannelTabRight => By.CssSelector("a.yt-simple-endpoint.style-scope.ytd-compact-video-renderer");
        //By.CssSelector("yt-formatted-string#text.style-scope.ytd-channel-name");
        private By elementChannelCurrent => By.XPath("/html/body/ytd-app/div[1]/ytd-page-manager/ytd-watch-flexy/div[5]/div[1]/div/div[2]/ytd-watch-metadata/div/div[2]/div[1]/ytd-video-owner-renderer/div[1]/ytd-channel-name/div/div/yt-formatted-string/a");
        public void GotoUrl(string url = "https://www.youtube.com")
        {
            driver.Navigate().GoToUrl(url);
        }
        public void SearchKeyword(string keyword)
        {
            try
            {
                IWebElement element = wait.Until(driver => driver.FindElement(elementYoutobeSearch));
                Actions.Click(element) // Nhấp vào phần tử để đảm bảo nó được chọn
                        .KeyDown(Keys.Control)
                        .SendKeys("a")
                        .KeyUp(Keys.Control)
                        .SendKeys(Keys.Delete)
                        .Perform();
                Random r = new();
                foreach (var a in keyword)
                {
                    element.SendKeys(a.ToString());
                    int pause = r.Next(300, 500);
                    Thread.Sleep(pause);
                }
                Actions.SendKeys(element, Keys.Enter).Perform();
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
                Actions.Click(element).Perform();
                // Thread.Sleep(TimeSpan.FromMinutes(time));
            }
            catch (NoSuchElementException)
            {
                Extension.WriteLine("Không tìm thấy phần tử.", ConsoleColor.Red);
            }
        }
        public void LikeVideo()
        {
            try
            {
                IWebElement element = wait.Until(driver => driver.FindElement(elementLikeButton));
                string classAttribute = element.GetAttribute("aria-pressed");
                if (classAttribute == "false")
                {
                    Actions.Click(element).Perform();
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
                    Actions.Click(element).Perform();
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
            if (comments.Count() > 0 || icons.Count() > 0)
            {
                Random r = new Random();
                int indexComment = r.Next(0, comments.Length);
                int indexIcon = r.Next(0, icons.Length);
                string randomComment = $"{comments[indexComment]} {icons[indexIcon]}";
                try
                {
                    IWebElement elementInput = wait.Until(driver => driver.FindElement(elementInputComment));
                    Actions.Click(elementInput).Perform();

                    IWebElement elementCommentText = wait.Until(driver => driver.FindElement(elementInputCommentText));

                   
                     Actions.Click(elementCommentText) // Nhấp vào phần tử để đảm bảo nó được chọn
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
                    elementButton.Click();
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
                        item.Click();
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
                        Actions.Click(item).Perform();
                        break;
                    }
                    else if(channelName.Contains(currentChannelName) == true)
                    {
                        Extension.WriteLine($"Không tìm thấy kênh {channel}", ConsoleColor.Red);
                        Extension.WriteLine($"Chuyển xem video tiếp theo của kênh {currentChannelName} thành công", ConsoleColor.Blue);
                        Actions.Click(item).Perform();
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