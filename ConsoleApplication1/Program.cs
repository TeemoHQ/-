using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] ccc = new byte[] { 0xFC, 0x05, 0X11};
            long result2 = ComputeCRC(ccc);
            Console.Read();

        }
        static long ComputeCRC(byte[] val)
        {
            long crc;
            long q;
            byte c;
            crc = 0;
            for (int i = 0; i < val.Length; i++)
            {
                c = val[i];
                q = (crc ^ c) & 0x0f;
                crc = (crc >> 4) ^ (q * 0x1081);
                q = (crc ^ (c >> 4)) & 0xf;
                crc = (crc >> 4) ^ (q * 0x1081);
            }
            byte ah = (byte)(crc >> 8);//高8位
            byte al = (byte)(crc & 0xff);//低8位
            return (byte)crc << 8 | (byte)(crc >> 8);
        }
    }
}

