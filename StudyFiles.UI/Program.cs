using System;

namespace StudyFiles.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client(Console.In, Console.Out);
            client.LoadMenu();
        }
    }
}
