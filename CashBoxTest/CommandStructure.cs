using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashBoxTest
{
    public class CommandStructure
    {
        public byte SYNC=0xFC;//信息发送开始代码
        public byte Length;//数据长度
        public byte Cmd;//指令状态
        public List<byte> Data;//指令需要的数据
        public byte CRCcodeHigh;//CRC校验码高位
        public byte CRCcodeLow;//CRC校验码低位
        public byte[] ToByteArray()
        {
            this.Length = this.Data == null ? Convert.ToByte(5) : Convert.ToByte(5 + this.Data.Count());

            var result = new List<byte>() { SYNC, Length,Cmd };
            if (Data != null)
            {
                Data.ForEach(p=>result.Add(p));
            }
            //CRC校验
            var CRC = CRCCCITT.Convert(result.ToArray());
            this.CRCcodeHigh = CRC[0];
            this.CRCcodeLow = CRC[1];
            result.Add(this.CRCcodeLow);
            result.Add(this.CRCcodeHigh);

            return result.ToArray();
        }
    }
}
