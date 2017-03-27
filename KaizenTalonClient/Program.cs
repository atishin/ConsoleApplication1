using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TalonService;

namespace KaizenTalonClient
{
    static class Program
    {
        private static Printer _printer;
        public static Printer Printer
        {
            get
            {
                if (_printer == null)
                {
                    _printer = new Printer();
                }

                return _printer;
            }
        }

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
