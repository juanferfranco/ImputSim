using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput.Native;
using WindowsInput;
using System.IO.Ports;
using System.Timers;

namespace ImputSim
{
    internal class Program
    {

        static bool SendW = false;
        static bool SendS = false;
        static bool SendA = false;
        static bool SendD = false;

        static string serialPortArgument;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("define COM port");
                return;
            }
            else serialPortArgument = args[0];


            Thread t = new Thread(readSensor);
            t.Start();

            InputSimulator sim = new InputSimulator();

            while (true)
            {

                if(SendW == true)
                {
                    sim.Keyboard.KeyDown(VirtualKeyCode.VK_W);
                }
                else sim.Keyboard.KeyUp(VirtualKeyCode.VK_W);
                
                if (SendS == true)
                {
                    sim.Keyboard.KeyDown(VirtualKeyCode.VK_S);
                }
                else sim.Keyboard.KeyUp(VirtualKeyCode.VK_S);

                if (SendA == true)
                {
                    sim.Keyboard.KeyDown(VirtualKeyCode.VK_A);
                }
                else sim.Keyboard.KeyUp(VirtualKeyCode.VK_A);

                if (SendD == true)
                {
                    sim.Keyboard.KeyDown(VirtualKeyCode.VK_D);
                }
                else sim.Keyboard.KeyUp(VirtualKeyCode.VK_D);

                Thread.Sleep(20);

            }

        }

        static void readSensor()
        {

            SerialPort _serialPort = new SerialPort();

            try
            {
                Console.WriteLine(serialPortArgument);

                _serialPort.PortName = serialPortArgument;
                _serialPort.BaudRate = 115200;
                _serialPort.NewLine = "\n";
                _serialPort.DtrEnable = true;
                _serialPort.Open();
                Console.WriteLine("Init Comm");
                while (true)
                {

                    if (_serialPort.BytesToRead > 0)
                    {
                        string response = _serialPort.ReadLine();
                        Console.WriteLine(response);



                        if(response == "SP")
                        {
                            SendS = true;
                            SendA = false;
                            SendD = false;

                        } 
                        else if(response == "SR")
                        {
                            SendS = false;
                        }
                        
                        if(response == "WP")
                        {
                            if(SendS == false)
                            {
                                SendW = true;
                            }
                        }
                        else if(response == "WR")
                        {
                            SendW= false;
                        }

                        if(response == "AP")
                        {
                            SendD = false;
                            SendA = true;
                        }
                        else if(response == "AR")
                        {
                            SendA = false;
                        }

                        if(response == "DP")
                        {
                            SendD = true;
                            SendA = false;
                        }
                        else if(response == "DR") {
                            SendD = false;
                        }
                    }
                }


            }
            catch(Exception e)
            {
                Console.WriteLine("Serial Port error: " + e.Message);
            }

        }

    }
}


