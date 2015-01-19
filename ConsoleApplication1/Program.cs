using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            StartOptions options = new StartOptions("http://dummy.oppedijk.com:12345") {
                ServerFactory = "Microsoft.Owin.Host.HttpListener"
            };

            using (WebApp.Start<Startup>(options))
            {
                Console.ReadLine();
            }
        }
    }
}
