using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleChatUDP
{
    internal class ChatUDP
    {
        static UdpClient udpClient;
        static bool isRunning = true;
        static string userName;

        static void Main(string[] args)
        {
            Console.Write("Введите IPv4-адрес получателя для отправки сообщений: ");
            string remoteAddress = Console.ReadLine(); 

            Console.Write("Введите порт для получения сообщений: ");
            int port = Convert.ToInt32(Console.ReadLine()); 

            Console.Write("Введите порт для отправки сообщений: ");
            int remotePort = Convert.ToInt32(Console.ReadLine()); 

            Console.Write("Введите ваше имя: ");
            userName = Console.ReadLine();

            ////////

            udpClient = new UdpClient(port);

            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("Console_Chat_UDP Starting: " + DateTime.Now);
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("Welcome to the club, " + userName);

            Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
            receiveThread.Start(); // Запуск получения сообщений

            while (isRunning)
            {
                string message = Console.ReadLine(); // Имя + текст
                SendMessage(userName + ": " + message, remoteAddress, remotePort);
            }
        }

        static void ReceiveMessage()
        {
            try
            {
                while (isRunning)
                {
                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = udpClient.Receive(ref remoteEP);
                    string message = Encoding.ASCII.GetString(data);
                    Console.WriteLine(message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erorr: " + e.ToString());
                Console.WriteLine("Перезагрузка приложения...");
                Thread.Sleep(5000);
                Process.Start(AppDomain.CurrentDomain.FriendlyName);
                Environment.Exit(0);
            }
        }

        static void SendMessage(string message, string ipAddress, int port)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            udpClient.Send(data, data.Length, ipAddress, port);
            
        }
    }
}
