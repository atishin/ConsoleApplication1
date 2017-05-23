using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TalonService
{
    public class Printer
    {
        public string printerName { get; set; } = "POS-58";
        public string cabinet { get; set; } = "";
        public string ticket { get; set; } = "";
        public string time { get; set; } = "";
        public string date { get; set; } = "";

        public string doctor_line_1 { get; set; } = "";
        public string doctor_line_2 { get; set; } = "";
        public string doctor_line_3 { get; set; } = "";

        public string patient_line_1 { get; set; } = "";
        public string patient_line_2 { get; set; } = "";
        public string patient_line_3 { get; set; } = "";
        public string operator_name { get; set; } = "";
        public string mo_name { get; set; } = "";


        public void Print()
        {
            Print(printerName, GetDocument());
        }

        private void Print(string printerName, byte[] document)
        {

            NativeMethods.DOC_INFO_1 documentInfo;
            IntPtr printerHandle;

            documentInfo = new NativeMethods.DOC_INFO_1();
            documentInfo.pDataType = "RAW";
            documentInfo.pDocName = "Receipt";

            printerHandle = new IntPtr(0);

            if (NativeMethods.OpenPrinter(printerName.Normalize(), out printerHandle, IntPtr.Zero))
            {
                if (NativeMethods.StartDocPrinter(printerHandle, 1, documentInfo))
                {
                    int bytesWritten;
                    byte[] managedData;
                    IntPtr unmanagedData;

                    managedData = document;
                    unmanagedData = Marshal.AllocCoTaskMem(managedData.Length);
                    Marshal.Copy(managedData, 0, unmanagedData, managedData.Length);

                    if (NativeMethods.StartPagePrinter(printerHandle))
                    {
                        NativeMethods.WritePrinter(
                            printerHandle,
                            unmanagedData,
                            managedData.Length,
                            out bytesWritten);
                        NativeMethods.EndPagePrinter(printerHandle);
                    }
                    else
                    {
                        throw new Win32Exception();
                    }

                    Marshal.FreeCoTaskMem(unmanagedData);

                    NativeMethods.EndDocPrinter(printerHandle);
                }
                else
                {
                    throw new Win32Exception();
                }

                NativeMethods.ClosePrinter(printerHandle);
            }
            else
            {
                throw new Win32Exception();
            }

        }

        private byte[] GetDocument()
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms, Encoding.UTF8))
            {
                // Reset the printer bws (NV images are not cleared)
                bw.Write(AsciiControlChars.Escape);
                bw.Write('@');

                // Render the logo
                //RenderLogo(bw);
                PrintReceipt(bw);

                // Feed 3 vertical motion units and cut the paper with a 1 point cut
                //bw.Write(AsciiControlChars.GroupSeparator);
                //bw.Write('V');
                //bw.Write((byte)66);
                //bw.Write((byte)3);

                bw.Flush();

                return ms.ToArray();
            }
        }

        private byte[] GetString(string str)
        {
            var utf8bytes = Encoding.UTF8.GetBytes(str);
            var win1252Bytes = Encoding.Convert(
                Encoding.UTF8, Encoding.GetEncoding("windows-1251"), utf8bytes);
            return win1252Bytes;
        }

        private void PrintReceipt(BinaryWriter bw)
        {
            if (cabinet.Length == 1) cabinet = " " + cabinet + " ";
            if (cabinet.Length == 2) cabinet += " ";
            if (ticket.Length == 1) ticket = "  " + ticket;
            if (ticket.Length == 2) ticket = " " + ticket;
            bw.Enlarged(GetString(CenterString("Кабинет   Талон", Fonts.Enlarge)));
            bw.LargeText(GetString(CenterString(cabinet + "      " + ticket, Fonts.Large)));
            bw.FeedLines(1);

            bw.NormalFont(GetString(CenterString("Дата и время приема", Fonts.Normal)));
            bw.LargeText(GetString(CenterString(time, Fonts.Large)));
            bw.LargeText(GetString(CenterString(date, Fonts.Large)));
            bw.FeedLines(1);

            bw.NormalFont(GetString(CenterString("Врач:", Fonts.Normal)));
            bw.Enlarged(GetString(CenterString(doctor_line_1.ToUpper(), Fonts.Enlarge)));
            bw.Enlarged(GetString(CenterString(doctor_line_2.ToUpper(), Fonts.Enlarge)));
            bw.Enlarged(GetString(CenterString(doctor_line_3.ToUpper(), Fonts.Enlarge)));
            bw.FeedLines(1);
            bw.NormalFont(GetString(CenterString("Пациент:", Fonts.Normal)));
            bw.Enlarged(GetString(CenterString(patient_line_1.ToUpper(), Fonts.Enlarge)));
            bw.Enlarged(GetString(CenterString(patient_line_2.ToUpper(), Fonts.Enlarge)));
            bw.Enlarged(GetString(CenterString(patient_line_3.ToUpper(), Fonts.Enlarge)));
            bw.FeedLines(2);

            bw.NormalFont(GetString(CenterString(mo_name, Fonts.Normal)));
            bw.NormalFont(GetString(CenterString("Оператор: " + operator_name, Fonts.Normal)));
            bw.NormalFont(GetString(CenterString("Регистрация: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm"), Fonts.Normal)));
            bw.FeedLines(3);

            //bw.NormalFont(GetString("01234567890123456789012345678901234567890123456789"));
            //bw.FeedLines(2);
            //bw.Enlarged(GetString("01234567890123456789012345678901234567890123456789"));
            //bw.FeedLines(2);
            //bw.High(GetString("01234567890123456789012345678901234567890123456789"));
            //bw.FeedLines(2);
            //bw.LargeText(GetString("01234567890123456789012345678901234567890123456789"));
            //bw.FeedLines(3);
            //bw.SetSize(8, 8);
            //bw.Write(GetString("22"));
            //bw.FeedLines(3);

        }

        private string GetWhitespaces(int number)
        {
            var str = "";
            for (var i = 0; i < number; i++)
            {
                str += " ";
            }
            return str;
        }

        private string CenterString(string str, Fonts font)
        {
            var length = 0;
            switch (font)
            {
                case Fonts.Normal:
                    length = 32 - str.Length;
                    return GetWhitespaces(length / 2) + str;
                case Fonts.Enlarge:
                    length = 16 - str.Length;
                    return GetWhitespaces(length / 2) + str;
                case Fonts.High:
                    length = 32 - str.Length;
                    return GetWhitespaces(length / 2) + str;
                case Fonts.Large:
                    length = 16 - str.Length;
                    return GetWhitespaces(length / 2) + str;
                default: return str;
            }
        }

        public enum Fonts
        {
            Normal,
            Large,
            Enlarge,
            High
        }
    }
}
