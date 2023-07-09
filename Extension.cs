namespace Slave
{
    public static class Extension {
        public static void WriteLine(string body, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(body);
            Console.ResetColor();
        }
    }
}