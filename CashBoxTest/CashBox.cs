using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CashBoxTest
{
    public class CashBox
    {
        private SerialPortUtil serialPortUtil = new SerialPortUtil("COM3", 9600, Parity.Even);
        public CashBox()
        {
            serialPortUtil.Received += PortDataRecive;
        }

        private void PortDataRecive(object sender, PortDataReciveEventArgs e)
        {
            if (e.Data == null || e.Data.Count() < 5)
            {
                //错误数据
                return;
            }
            var command = (ResponseCommand)(e.Data[2]);
            var dataList = e.Data.ToList();
            var sb = new StringBuilder();
            dataList.ForEach(p => sb.Append(p.ToString("x2") + " "));
            Console.WriteLine("当前状态：{0},回传的数据{1}", command.ToString(), sb);
            switch (command)
            {
                case ResponseCommand.DISABLE:
                    //不收钱状态
                    Console.WriteLine("禁止收钱");
                    break;
                case ResponseCommand.IDLING:
                    //待机
                    Console.WriteLine("等待塞钱");
                    break;
                case ResponseCommand.INHIBIT:
                    if (dataList[3] == 0x00)
                    {
                        Console.WriteLine("设置启动塞钱成功");
                    }
                    else
                    {
                        Console.WriteLine("设置禁止塞钱成功");
                    }

                    break;
                case ResponseCommand.ACCEPTING:
                    Console.WriteLine("开始识别");
                    break;
                case ResponseCommand.ESCROW:
                    var money = (MoneyCommand)dataList[3];
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("纸币识别完毕,收到"+money+"元");
                    if (money == MoneyCommand.Ten)
                    {
                        Console.WriteLine("收到10元主动退出");
                        this.ReturnMoney();
                    }
                    else
                    {
                        InsertStack1();
                    }
                    break;
                case ResponseCommand.STACKING:
                    Console.WriteLine("在搬运了");
                    break;
                case ResponseCommand.ACK:
                    Console.WriteLine("搬运成功");
                    break;
                case ResponseCommand.VEND_VALID:
                    Console.WriteLine("结束");
                    break;
                case ResponseCommand.STACKED:
                    break;
                case ResponseCommand.REJECTING:
                    Console.WriteLine("拒绝状态 钱有问题！！！！！！！！！");
                    break;
                case ResponseCommand.RETURNING:
                    break;
                case ResponseCommand.HOLDING:
                    break;
                case ResponseCommand.INITIALIZB:
                    break;
                case ResponseCommand.POWER_UP:
                    break;
                case ResponseCommand.POWERUP_WITH_BILL_IN_ACCEPTOR:
                    break;
                case ResponseCommand.POWERUP_WITH_BILL_IN_STACKER:
                    break;
                case ResponseCommand.STACKER_FULL:
                    break;
                case ResponseCommand.STACKER_OPEN:
                    break;
                case ResponseCommand.JAM_IN_ACCEPTOR:
                    break;
                case ResponseCommand.JAM_IN_STACKER:
                    break;
                case ResponseCommand.PAUSE:
                    break;
                case ResponseCommand.CHEATED:
                    break;
                case ResponseCommand.FAILURE:
                    break;
                case ResponseCommand.COMMUNICATION_ERRO:
                    break;
                case ResponseCommand.INVALID_COMMAND:
                    break;
                default:
                    break;
            }
            Console.WriteLine();

        }

        public void StartWork()
        {
            if (!serialPortUtil.IsOpen())
            {
                return;
            }
            //[FC 06 C3 00 04 D6] 252,6,195,0,4,214
            CommandStructure cs = new CommandStructure();
            cs.Cmd = Convert.ToByte(RequestCommand.INHIBIT);
            cs.Data = new List<byte> { 0x0 };

            byte[] SendData = cs.ToByteArray();

            var dataList = SendData.ToList();
            var sb = new StringBuilder();
            dataList.ForEach(p => sb.Append(p.ToString("x2") + " "));
            Console.WriteLine("开始收钱请求:{0}", sb);

            serialPortUtil.SendData(SendData, 0, SendData.Length);
        }

        public void StopWork()
        {
            //[FC 06 C3 01 8D C7]
            CommandStructure cs = new CommandStructure();
            cs.Cmd = Convert.ToByte(RequestCommand.INHIBIT);
            cs.Data = new List<byte> { 0x1 };

            byte[] SendData = cs.ToByteArray();
            serialPortUtil.SendData(SendData, 0, SendData.Length);

            var dataList = SendData.ToList();
            var sb = new StringBuilder();
            dataList.ForEach(p => sb.Append(p.ToString("x2") + " "));
            Console.WriteLine("停止收钱请求:{0}", sb);
        }

        public bool OpenPort()
        {
            return serialPortUtil.Open();
        }

        public void GetStatus()
        {
            //FC 05 11 27 56
            CommandStructure cs = new CommandStructure();
            cs.Cmd = Convert.ToByte(RequestCommand.STATUSREQUEST);

            byte[] SendData = cs.ToByteArray();

            var dataList = SendData.ToList();
            var sb = new StringBuilder();
            dataList.ForEach(p => sb.Append(p.ToString("x2") + " "));

            while (true)
            {
                Thread.Sleep(4000);
                Console.ResetColor();
                Console.WriteLine("请求状态数据:{0}", sb);
                serialPortUtil.SendData(SendData, 0, SendData.Length);
            }
        }

        //注意：只有在ESCROW状态下才有效
        public void ReturnMoney()
        {
            CommandStructure cs = new CommandStructure();
            cs.Cmd = Convert.ToByte(RequestCommand.RETURN);

            byte[] SendData = cs.ToByteArray();

            var dataList = SendData.ToList();
            var sb = new StringBuilder();
            dataList.ForEach(p => sb.Append(p.ToString("x2") + " "));
            Console.WriteLine("主动退钱:{0}", sb);

            serialPortUtil.SendData(SendData, 0, SendData.Length);
        }

        public void InsertStack1()
        {
            //[FC 05 41 A2 04]
            CommandStructure cs = new CommandStructure();
            cs.Cmd = Convert.ToByte(RequestCommand.STACK_1);
            byte[] SendData = cs.ToByteArray();

            var dataList = SendData.ToList();
            var sb = new StringBuilder();
            dataList.ForEach(p => sb.Append(p.ToString("x2") + " "));
            Console.WriteLine("开始搬运纸币:{0}", sb);

            serialPortUtil.SendData(SendData, 0, SendData.Length);
        }
    }
}
