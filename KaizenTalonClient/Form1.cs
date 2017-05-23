using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TalonService;

namespace KaizenTalonClient
{
    public partial class Form1 : Form
    {
        private bool isActive = false;

        private bool isStarted = false;

        private SignalRProvider sProvider;

        private Printer _printer;

        public Form1()
        {
            InitializeComponent();

            _printer = Program.Printer;

            var host = System.Configuration.ConfigurationManager.AppSettings["url"];

            _printer.mo_name = "Поликлиника №30";

            sProvider = new SignalRProvider()
            {
                host = host,
                printer = _printer,
                operator_name = _printer.mo_name,
                IsActive = isActive
            };

            sProvider.LogInfo += SProvider_LogInfo;

            sProvider.Connect();
        }

        private void SProvider_LogInfo(object sender, string e)
        {
            //tb_log.Text += "\n" + e;
        }

        private void btn_main_Click(object sender, EventArgs e)
        {
            if (!isStarted)
            {
                isStarted = true;
                
                //sProvider.Connect();
            }

            isActive = !isActive;
            sProvider.IsActive = isActive;
            UpdateView();
        }

        private void UpdateView()
        {
            if (isActive)
            {
                lbl_info.Text = "Талоны работают";
                btn_main.Text = "Остановить";
                btn_main.ForeColor = Color.Red;
            }
            else
            {
                lbl_info.Text = "Остановлено";
                btn_main.Text = "Запустить";
                btn_main.ForeColor = Color.Green;
            }
        }

    }
}
