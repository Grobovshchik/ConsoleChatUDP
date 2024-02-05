using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ConsoleChatUDP
{
    internal class ChatUDP
    {
        static UdpClient udpClient;
        static bool isRunning = true;
        static string userName;

        static void Main(string[] args)
        {
            // Установка заголовка консоли
            Console.Title = "ConsoleChatUDP_Имуков_В.И._149/А-21"; 

            // Получение информации от пользователя 
            Console.Write("Введите IPv4-адрес получателя для отправки сообщений: ");
            string remoteAddress = Console.ReadLine(); 

            Console.Write("Введите порт для получения сообщений: ");
            int port = Convert.ToInt32(Console.ReadLine()); 

            Console.Write("Введите порт для отправки сообщений: ");
            int remotePort = Convert.ToInt32(Console.ReadLine()); 

            Console.Write("Введите ваше имя: ");
            userName = Console.ReadLine();

            // Создание UdpClient для отправки и получения сообщений
            udpClient = new UdpClient(port);

            // Вывод информационных сообщений в консоль
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("Console_Chat_UDP Starting: " + DateTime.Now);
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("Добро пожаловать на борт, " + userName);

            // Запуск потока для получения сообщений
            Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage)); 
            receiveThread.Start();

            // Цикл для получения и отправки сообщений + Имя
            while (isRunning)
            {
                string message = Console.ReadLine(); 
                SendMessage(userName + ": " + message, remoteAddress, remotePort);
            }
        }

        // Метод для получения сообщений
        static void ReceiveMessage() 
        {
            try
            {
                // Получение сообщения
                while (isRunning) 
                {
                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = udpClient.Receive(ref remoteEP);
                    string message = Encoding.UTF8.GetString(data);
                    Console.WriteLine(message);
                }
            }
            // Вывод сообщения об ошибке и перезапуск приложения
            catch (Exception e) 
            {
                Console.WriteLine("Erorr: " + e.ToString());
                Console.WriteLine("Перезагрузка приложения...");
                Thread.Sleep(5000);
                Process.Start(AppDomain.CurrentDomain.FriendlyName);
                Environment.Exit(0);
            }
        }

        // Метод для отправки сообщений
        static void SendMessage(string message, string ipAddress, int port) 
        {
            // Кодирование сообщения в байты и отправка
            byte[] data = Encoding.UTF8.GetBytes(message);
            udpClient.Send(data, data.Length, ipAddress, port);
        }
    }
}
