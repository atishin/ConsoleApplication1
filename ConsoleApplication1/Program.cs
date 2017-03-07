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
            var signalrHost = "http://localhost:59874/";
            var operator_name = "Поликлиника №30";
            if (args.Length != 0)
            {
                signalrHost = args[0];
            }
            if (args.Length > 1)
            {
                operator_name = args[1];
            }

            var printer = new Printer()
            {
                mo_name = "Поликлиника №30",
                operator_name = operator_name
            };

            ConnectSignalR(signalrHost, operator_name);
        }

        static void ConnectSignalR(string host, string operator_name)
        {
            var connection = new HubConnection(host);
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

            myHub.On<dynamic, int>("ticketNotify", (param, act) => {
                if (act == 0)
                {
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
                    printer.mo_name = "Поликлиника №30";
                    printer.operator_name = operator_name;
                    if (param.Employee.Cabinet != null)
                    {
                        printer.cabinet = param.Employee.Cabinet.Title;
                    }
                    printer.Print();
                    Console.WriteLine(param);
                }
                
            });

            Console.Read();
            connection.Stop();
        }

        
    }

}
