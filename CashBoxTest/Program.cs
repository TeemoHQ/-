using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashBoxTest
{
    class Program
    {
        static void Main(string[] args)
        {

            CashBox cashBox = new CashBox();
            Console.WriteLine("打开串口:" + cashBox.OpenPort());
            Task.Factory.StartNew(cashBox.GetStatus);

            GetCommand(cashBox);
        }

        private static void GetCommand(CashBox cashBox)
        {
            var command = Console.ReadLine();
            switch (command)
            {
                case "start":
                    cashBox.StartWork();
                    break;
                case "stop":
                    cashBox.StopWork();
                    break;
                default:
                    break;
            }
            GetCommand(cashBox);
        }
    }
}
