using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace test1
{
    class Program
    {
        static void Main(string[] args)
        {
            Comm comm = new Comm();
            comm.serialPort.PortName = "COM3";
            //波特率
            comm.serialPort.BaudRate = 9600;
            //数据位
            comm.serialPort.DataBits = 8;
            //两个停止位
            comm.serialPort.StopBits = System.IO.Ports.StopBits.One;
            //无奇偶校验位
            comm.serialPort.Parity = System.IO.Ports.Parity.Even;
            comm.serialPort.ReadTimeout = 100;
            comm.serialPort.WriteTimeout = -1;

            comm.Open();
            if (comm.IsOpen)
            {
                comm.DataReceived += new Comm.EventHandle(comm_DataReceived);
                byte[] send = new byte[] {0XFC,0X05,0X40,0X2B,0X15 };
                comm.WritePort(send, 0, send.Length);
            }
        }

        private static void comm_DataReceived(byte[] readBuffer)
        {
            Console.WriteLine(readBuffer);
        }
    }
    public class Comm
    {
        public delegate void EventHandle(byte[] readBuffer);
        public event EventHandle DataReceived;

        public SerialPort serialPort;
        Thread thread;
        volatile bool _keepReading;

        public Comm()
        {
            serialPort = new SerialPort();
            thread = null;
            _keepReading = false;
        }

        public bool IsOpen
        {
            get
            {
                return serialPort.IsOpen;
            }
        }

        private void StartReading()
        {
            if (!_keepReading)
            {
                _keepReading = true;
                thread = new Thread(new ThreadStart(ReadPort));
                thread.Start();
            }
        }

        private void StopReading()
        {
            if (_keepReading)
            {
                _keepReading = false;
                thread.Join();
                thread = null;
            }
        }

        private void ReadPort()
        {
            while (_keepReading)
            {
                if (serialPort.IsOpen)
                {
                    int count = serialPort.BytesToRead;
                    if (count > 0)
                    {
                        byte[] readBuffer = new byte[count];
                        try
                        {
                            serialPort.Read(readBuffer, 0, count);
                            if (DataReceived != null)
                                DataReceived(readBuffer);
                            Thread.Sleep(100);
                        }
                        catch (TimeoutException)
                        {
                        }
                    }
                }
            }
        }

        public void Open()
        {
            Close();
            serialPort.Open();
            if (serialPort.IsOpen)
            {
                StartReading();
            }
            else
            {
               
            }
        }

        public void Close()
        {
            StopReading();
            serialPort.Close();
        }

        public void WritePort(byte[] send, int offSet, int count)
        {
            if (IsOpen)
            {
                serialPort.Write(send, offSet, count);
            }
        }
    }
}
