using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.IO;
using System.ComponentModel;
using Microsoft.AspNet.SignalR.Client;

namespace ConsoleApplication1
{
    class Program
    {

        static Printer printer = new Printer();
        static void Main(string[] args)
        {
            var signalrHost = "http://212.19.138.131:7808/";
            var operator_name = "Поликлиника №30";
            if (args.Length != 0)
            {
                signalrHost = args[0];
            }
            if (args.Length > 1)
            {
                operator_name = args[1];
            }

            var printer = new Printer();
            printer.mo_name = "Поликлиника №30";
            SignalRProvider sProvider = new SignalRProvider() {
                host = signalrHost,
                printer = printer,
                operator_name = operator_name
            };
            sProvider.Connect();
            while (true) {
                var a = Console.ReadKey();
                if (a.Key == ConsoleKey.Q) return;
            }
        }
    }

}
