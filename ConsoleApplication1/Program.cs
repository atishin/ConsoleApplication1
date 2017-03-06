﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.USBlib;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.IO;
using System.ComponentModel;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            List<string> ports = SerialPort.GetPortNames().ToList();
            Print("SUNPHOR", GetDocument());
        }

        private static void Print(string printerName, byte[] document)
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

        private static byte[] GetDocument()
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

        private static byte[] GetString(string str)
        {
            var utf8bytes = Encoding.UTF8.GetBytes(str);
            var win1252Bytes = Encoding.Convert(
                Encoding.UTF8, Encoding.GetEncoding("windows-1251"), utf8bytes);
            return win1252Bytes;
        }

        private static void PrintReceipt(BinaryWriter bw)
        {
            //bw.Enlarged(GetString(CenterString("Кабинет   Талон", Fonts.Enlarge)));
            //bw.LargeText(GetString(CenterString("254      105", Fonts.Large)));
            //bw.FeedLines(1);

            //bw.NormalFont(GetString(CenterString("Дата и время приема", Fonts.Normal)));
            //bw.LargeText(GetString(CenterString("16:15", Fonts.Large)));
            //bw.LargeText(GetString(CenterString("14.03.2017", Fonts.Large)));
            //bw.FeedLines(1);

            //bw.NormalFont(GetString(CenterString("Врач:", Fonts.Normal)));
            //bw.Enlarged(GetString(CenterString("Николаева".ToUpper(), Fonts.Enlarge)));
            //bw.Enlarged(GetString(CenterString("Ирина".ToUpper(), Fonts.Enlarge)));
            //bw.Enlarged(GetString(CenterString("Васильевна".ToUpper(), Fonts.Enlarge)));
            //bw.FeedLines(1);
            //bw.NormalFont(GetString(CenterString("Пациент:", Fonts.Normal)));
            //bw.Enlarged(GetString(CenterString("Николаева".ToUpper(), Fonts.Enlarge)));
            //bw.Enlarged(GetString(CenterString("Ирина".ToUpper(), Fonts.Enlarge)));
            //bw.Enlarged(GetString(CenterString("Васильевна".ToUpper(), Fonts.Enlarge)));
            //bw.FeedLines(2);

            //bw.NormalFont(GetString(CenterString("Поликлиника №30", Fonts.Normal)));
            //bw.NormalFont(GetString(CenterString("Оператор: Иванова А.К.", Fonts.Normal)));
            //bw.NormalFont(GetString(CenterString("Регистрация: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm"), Fonts.Normal)));
            //bw.FeedLines(3);

            //bw.NormalFont(GetString("01234567890123456789012345678901234567890123456789"));
            //bw.FeedLines(2);
            //bw.Enlarged(GetString("01234567890123456789012345678901234567890123456789"));
            //bw.FeedLines(2);
            //bw.High(GetString("01234567890123456789012345678901234567890123456789"));
            //bw.FeedLines(2);
            //bw.LargeText(GetString("01234567890123456789012345678901234567890123456789"));
            //bw.FeedLines(3);
            bw.SetSize(8, 8);
            bw.Write(GetString("22"));
            bw.FeedLines(3);

        }

        private static string GetWhitespaces(int number)
        {
            var str = "";
            for (var i = 0; i < number; i ++)
            {
                str += " ";
            }
            return str;
        }

        private static string CenterString(string str, Fonts font)
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
