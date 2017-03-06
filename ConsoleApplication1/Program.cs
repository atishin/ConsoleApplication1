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

            var printer = new Printer()
            {
                mo_name = "Поликлиника №30",
                operator_name = "Поликлиника №30"
            };

            ConnectSignalR();
        }

        static void ConnectSignalR()
        {
            var connection = new HubConnection("http://localhost:59874/");
            //Make proxy to hub based on hub name on server
            var myHub = connection.CreateHubProxy("VisitsHub");
            //Start connection

            connection.Start().ContinueWith(task => {
                if (task.IsFaulted)
                {
                    Console.WriteLine("There was an error opening the connection:{0}",
                                      task.Exception.GetBaseException());
                }
                else
                {
                    Console.WriteLine("Connected");
                }

            }).Wait();

            myHub.On<dynamic>("ticketNotify", param => {

                DateTime date = param.RegisteredOn;
                printer.date = date.ToString("dd.MM.yyyy");
                printer.time = date.ToString("hh:mm");

                printer.ticket = param.TicketNumber;

                printer.patient_line_1 = param.Patient.Person.LastName;
                printer.patient_line_2 = param.Patient.Person.FirstName;
                printer.patient_line_3 = param.Patient.Person.MidleName;

                printer.doctor_line_1 = param.Employee.Person.LastName;
                printer.doctor_line_2 = param.Employee.Person.FirstName;
                printer.doctor_line_3 = param.Employee.Person.MidleName;
                if (param.Employee.Cabinet != null)
                {
                    printer.cabinet = param.Employee.Cabinet.Title;
                }
                printer.Print();
                Console.WriteLine(param);
            });

            Console.Read();
            connection.Stop();
        }

        
    }

}
