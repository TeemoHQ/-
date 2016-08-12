using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CashBoxTest
{
    /// <summary>  
    /// 循环冗余检验：CRC-16-CCITT查表法  
    /// </summary>  
    public class CRCCCITT
    {
        public static byte[] Convert(byte[] val)
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
            return new byte[] {ah,al };
        }
    }
}
