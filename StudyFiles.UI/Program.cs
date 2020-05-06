using System;
using System.Text;

namespace StudyFiles.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var client = new Client(Console.In, Console.Out);
            client.LoadMenu();
        }
    }
}
