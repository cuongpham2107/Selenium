using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using Common;

namespace Slave
{
    class Program
    {
        static ConfigChrome _config = ConfigManager<ConfigChrome>.Instance.Config;
        public static void Main(string[] args)
        {
            int TimeToWatchVideo = new Random().Next(5,7);
            DateTime startTime = DateTime.Now;
            TimeSpan timeOut = TimeSpan.FromDays(1);
            string[] comments = {
                "Video này thực sự tuyệt vời! Tôi rất ấn tượng với cách nó truyền đạt thông điệp sâu sắc và truyền cảm hứng.",
                "Các hình ảnh và âm nhạc được lựa chọn tốt và tạo ra một trải nghiệm tuyệt vời",
                "Cảm ơn vì đã chia sẻ video này! Nó cung cấp một cái nhìn sâu sắc và phản ánh tốt vấn đề mà nó đề cập.",
                "Tôi rất thích cách thông điệp được truyền tải một cách rõ ràng và đầy sức mạnh.",
                "Video này thật đặc biệt! Nó đã khơi dậy trong tôi những cảm xúc sâu sắc và đồng thời mang lại kiến thức mới.",
                "Đạo diễn đã làm rất tốt trong việc kết hợp các yếu tố hình ảnh, âm thanh và lời thoại để tạo nên một tác phẩm tuyệt vời.",
                "Tôi không thể ngừng xem video này! Nó rất cuốn hút và đã tạo ra một trải nghiệm thú vị.",
                "Những hình ảnh đẹp mắt và cốt truyện sáng tạo khiến nó trở thành một video đáng để xem và chia sẻ."
            };
            string[] icons = {
                "<3 <3 <3",
                ":)))",
                "=)))",
                ":3",
                "^^ ^^",
                ":v",
                ";-) ;)",
                ":-O"
            };
            string[] keywords = {
                "Tạo Bảng Chấm công và Chấm công với danh sách nhân viên, Tạo và Cập nhập Object sử dụng Devexpress",
                "Phần mềm quản lý - Sử dụng Devexpress XAF, Build 2 nền tảng Web và Win cùng 1 code logic",
                "Tạo Calendar chấm công bằng PopupWindowShowAction trong Devexpress",
                "Phần mềm quản lý đề tài khoa học công nghệ, Sử dụng C#",
                "Cách sử dụng và chạy phần mềm quản lý Kho sử dụng Devexpress Xaf",
                 "Add module google map in Devexpress XAF Blazor",
            };
            string[] channels = {
                "Phạm Mạnh Cường",
                "Em Chè ĐTCL",
                "LCK Tiếng Việt",
                "VieTalents",
                "TUI TÊN BÔ",
                "VieShows",
            };
            string email = "quynhdonghy2001@gmail.com";
            string password = "Cuonggiun1";

            string profileID = "64a3d2cd6bc6bb5b9f013500";

            ChromeOptions options = new();
            options.BinaryLocation = _config.BinaryLocation;
            options.AddArgument(_config.UserDataDir.Replace("{id}", profileID));
            options.AddArgument(_config.Extension.Replace("{id}", profileID));
            options.AddArguments("--disable-default-apps", "--disable-extensions");
            options.AddArguments(_config.Arguments);

            IWebDriver driver = new ChromeDriver();

            driver.Manage().Window.Size = new System.Drawing.Size(350, 350);
            Actions actions = new Actions(driver);
            FilmScriptLoginGoogle(driver, email, password);

            ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            while (true)
            {
                foreach (string keyword in keywords)
                {
                    Console.WriteLine("(==================================================)");
                    Extension.WriteLine($"Tìm kiếm video mới : {keyword}", ConsoleColor.Green);
                    FilmScriptYoutobe(driver, keyword, channels, comments, icons, TimeToWatchVideo);
                }
                if (DateTime.Now - startTime >= timeOut)
                {
                    break;
                }
                Thread.Sleep(1000);
            }
            driver.Quit();
        }
        public static void FilmScriptLoginGoogle(IWebDriver driver, string email, string password){
            Google google = new Google(driver);
            google.GoToUrl("https://accounts.google.com");
            if(google.CheckLoginGoogle() == true)
            {
                google.EnterEmail(email); 
                Thread.Sleep(2000);
                google.EnterPassword(password);
            }
            
        }
        public static void FilmScriptYoutobe(IWebDriver driver, string keyword, string[] channels, string[] comments, string[] icons, int TimeToWatchVideo){
            Youtobe youtobe = new Youtobe(driver);
            youtobe.GotoUrl("https://www.youtube.com");
            youtobe.SearchKeyword(keyword);
            Thread.Sleep(TimeSpan.FromMinutes(1));
            youtobe.LikeVideo();
            youtobe.Subscribe();
            youtobe.CommentVideo(comments, icons);
            int check = 0;
            while (true)
            {
                foreach (string channel in channels)
                {
                    Extension.WriteLine($"Channel cần tìm kiếm: {channel}",ConsoleColor.Green);
                    youtobe.ChannelTabVideo(channel);
                    youtobe.LikeVideo();
                    youtobe.Subscribe();
                    youtobe.CommentVideo(comments, icons);
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                    check++;
                }
                if(check == channels.Length)
                {
                    break;
                }
            }
        }
        
    }

}