using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalonService
{
    public class SignalRProvider
    {
        public string host { get; set; }
        public string operator_name { get; set; }

        public Printer printer { get; set; }

        private HubConnection connection { get; set; }

        public bool IsActive { get; set; }

        public event EventHandler<string> LogInfo;

        ~SignalRProvider()
        {
            connection.Stop();
        }

        public void Connect()
        {
            connection = new HubConnection(host);
            var myHub = connection.CreateHubProxy("VisitsHub");

            connection.Start().ContinueWith(task => {
                if (task.IsFaulted)
                {
                    if (LogInfo != null)
                    {
                        LogInfo(this, string.Format("There was an error opening the connection:{0}", task.Exception.GetBaseException()));
                    }
                }
                else
                {
                    if (LogInfo != null)
                    {
                        LogInfo(this, "Connected");
                    }
                }

            }).Wait();

            myHub.On<dynamic, int>("ticketNotify", (param, act) => {

                if (!IsActive)
                    return;

                if (act == 0)
                {
                    try
                    {
                        DateTime date = param.RegisteredOn;
                        //DateTime.Now.ToString("dd.MM.yyyy HH:mm")
                        printer.date = date.ToString("dd.MM.yyyy");
                        printer.time = date.ToString("HH:mm");

                        printer.ticket = param.TicketNumber;

                        printer.patient_line_1 = param.Patient.Person.LastName;
                        printer.patient_line_2 = param.Patient.Person.FirstName;
                        printer.patient_line_3 = param.Patient.Person.MidleName;

                        printer.doctor_line_1 = param.Employee.Person.LastName;
                        printer.doctor_line_2 = param.Employee.Person.FirstName;
                        printer.doctor_line_3 = param.Employee.Person.MidleName;
                        printer.mo_name = "Поликлиника №30";
                        printer.operator_name = operator_name;
                        if (param.Employee.Cabinet != null)
                        {
                            printer.cabinet = param.Employee.Cabinet.Title;
                        }
                        printer.Print();
                    }
                    catch (Exception ex)
                    {
                        if (LogInfo != null)
                        {
                            LogInfo(this, ex.Message);
                        }
                    }
                    //Console.WriteLine(param);
                }

            });
        }
    }
}
