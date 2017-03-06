using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public static class PosExt
    {
        public static void Enlarged(this BinaryWriter bw, byte[] text)
        {
            bw.Write(AsciiControlChars.Escape);
            bw.Write((byte)33);
            bw.Write((byte)32);
            bw.Write(text);
            bw.Write(AsciiControlChars.Newline);
        }
        public static void High(this BinaryWriter bw, byte[] text)
        {
            bw.Write(AsciiControlChars.Escape);
            bw.Write((byte)33);
            bw.Write((byte)16);
            bw.Write(text); //Width,enlarged
            bw.Write(AsciiControlChars.Newline);
        }
        public static void LargeText(this BinaryWriter bw, byte[] text)
        {
            bw.Write(AsciiControlChars.Escape);
            bw.Write((byte)33);
            bw.Write((byte)48);
            bw.Write(text);
            bw.Write(AsciiControlChars.Newline);
        }
        public static void FeedLines(this BinaryWriter bw, int lines)
        {
            bw.Write(AsciiControlChars.Newline);
            if (lines > 0)
            {
                bw.Write(AsciiControlChars.Escape);
                bw.Write('d');
                bw.Write((byte)lines - 1);
            }
        }

        public static void SetEncoding(this BinaryWriter bw, byte[] text, int lines)
        {
            bw.Write(AsciiControlChars.Escape);
            bw.Write('t');
            bw.Write((byte)lines);
            bw.Write(text);
        }

        public static void NormalFont(this BinaryWriter bw, byte[] text, bool line = true)
        {
            bw.Write(AsciiControlChars.Escape);
            bw.Write((byte)33);
            bw.Write((byte)8);
            bw.Write(text);
            if (line)
                bw.Write(AsciiControlChars.Newline);
        }

        public static void ExecuteCommands(this BinaryWriter bw, params int[] commands)
        {
            foreach (var cmd in commands)
            {
                bw.Write((char)cmd);
            }
        }

        //1D 28 51 0E 00 31 x1L x1H y1L y1H x2L x2H y2L y2H c m1 m2 m3 m4
        public static void DrawRectangle(this BinaryWriter bw,
            int x1L, int x1H, int y1L, int y1H,
            int x2L, int x2H, int y2L, int y2H,
            int m1, int m2, int m3, int m4)
        {

            bw.ExecuteCommands(0x1B, 0x4C);
            bw.ExecuteCommands(0x1D, 0x28, 0x51, 0x0E, 0x00, 0x31);

            bw.Write((byte)x1L);
            bw.Write((byte)x1H);
            bw.Write((byte)y1L);
            bw.Write((byte)y1H);

            bw.Write((byte)x2L);
            bw.Write((byte)x2H);
            bw.Write((byte)y2L);
            bw.Write((byte)y2H);

            bw.Write('c');
            bw.Write((byte)m1);
            bw.Write((byte)m2);
            bw.Write((byte)m3);
            bw.Write((byte)m4);
        }

        private static int[] ConvertToBinary(int number) {
            List<int> res = new List<int>();
            while (number > 0)
            {
                res.Add(number % 2);
                number /= 2;
            }
            res.Reverse();
            return res.ToArray();
        }

        private static int ConvertToDecimal(int[] numbers)
        {
            int res = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                res += (int)Math.Pow(2 * numbers[i], numbers.Length - i - 1);
            }
            return res;
        }

        /// <summary>
        /// Not working
        /// </summary>
        /// <param name="bw"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void SetSize(this BinaryWriter bw, int width, int height)
        {
            bw.ExecuteCommands(0x1d, 0x21);
            var size = (byte)ConvertToDecimal(new int[] { 0, 0, 1, 1, 0, 0, 0, 1 });
            bw.Write(size);
        }

        public static void PrintBarcode(this BinaryWriter bw)
        {
            //'// Print and feed paper: Paper feeding amount = 4.94 mm (35/180 inches)
            //    ESC "J" 35
            bw.Write(AsciiControlChars.Escape);
            bw.Write('J');
            bw.Write((byte)35);
            //'// Set barcode height: in case TM-T20, 6.25 mm (50/203 inches)
            //    GS "h" 50
            bw.Write((char)0x1D);
            bw.Write('h');
            bw.Write((byte)50);
            //'// Select print position of HRI characters: Print position, below the barcode
            //    GS "H" 2
            //'// Select font for HRI characters: Font B
            //    GS "f" 1
            //'// Print barcode: (A) format, barcode system = CODE39
            //    GS "k" 4 "*00014*" 0
            //'// --- Print barcode ---<<<
        }

        public static void SelectFont(this BinaryWriter bw, int font) 
        {
            bw.ExecuteCommands(0x1B, 0x4D);
            bw.Write((byte)font);
        }

        public enum Colors { Black = 0, Red = 1 }
        public static void SelectColor(this BinaryWriter bw, Colors color)
        {
            bw.ExecuteCommands(0x1B, 0x72);
            bw.Write((byte)color);
        }

        public enum UndelineModes
        {
            Off = 0,
            On1 = 1,
            On2 = 2
        }
        public static void SetUndelineMode(this BinaryWriter bw, UndelineModes mode)
        {
            bw.ExecuteCommands(0x1B, 0x2D);
            bw.Write((byte)mode);
        }

        /*
27 33 0     ESC ! NUL    Master style: pica                              ESC/P
27 33 1     ESC ! SOH    Master style: elite                             ESC/P
27 33 2     ESC ! STX    Master style: proportional                      ESC/P
27 33 4     ESC ! EOT    Master style: condensed                         ESC/P
27 33 8     ESC ! BS     Master style: emphasised                        ESC/P
27 33 16    ESC ! DLE    Master style: enhanced (double-strike)          ESC/P
27 33 32    ESC ! SP     Master style: enlarged (double-width)           ESC/P
27 33 64    ESC ! @      Master style: italic                            ESC/P
27 33 128   ESC ! ---    Master style: underline                         ESC/P
                     Above values can be added for combined styles.
         
        bw.Write(AsciiControlChars.Escape);
        bw.Write((byte)33);
        bw.Write((byte)0);
        bw.Write("test"); //Default, Pica
        bw.Write(AsciiControlChars.Newline);
 
        bw.Write(AsciiControlChars.Escape);
        bw.Write((byte)33);
        bw.Write((byte)4);
        bw.Write("test"); //condensed
        bw.Write(AsciiControlChars.Newline);
        bw.Write(AsciiControlChars.Escape);
        bw.Write((byte)33);
        bw.Write((byte)8);
        bw.Write("test"); //emphasised
        bw.Write(AsciiControlChars.Newline);
        bw.Write(AsciiControlChars.Escape);
        bw.Write((byte)33);
        bw.Write((byte)16);
        bw.Write("test"); //Height,enhanced
        bw.Write(AsciiControlChars.Newline);
        bw.Write(AsciiControlChars.Escape);
        bw.Write((byte)33);
        bw.Write((byte)32);
        bw.Write("test"); //Width,enlarged
        bw.Write(AsciiControlChars.Newline);
        bw.Write(AsciiControlChars.Escape);
        bw.Write((byte)33);
        bw.Write((byte)48);
        bw.Write("test");   //WxH
        bw.Write(AsciiControlChars.Newline);
             */
    }
}
