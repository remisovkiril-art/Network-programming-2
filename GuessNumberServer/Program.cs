using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
class Program
{
    static void Main()
    {
        TcpListener server = new TcpListener(IPAddress.Any, 5000);
        server.Start();
        Console.WriteLine("Сервер запущен");
        TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("Клиент подключен");
        NetworkStream stream = client.GetStream();
        Random rand = new Random();
        int number = rand.Next(1, 51);
        int attempts = 0;
        DateTime start = DateTime.Now;
        byte[] buffer = new byte[1024];
        while (true)
        {
            int bytes = stream.Read(buffer, 0, buffer.Length);
            if (bytes == 0) break;
            string input = Encoding.UTF8.GetString(buffer, 0, bytes);
            if (!int.TryParse(input, out int guess))
            {
                Send(stream, "Введите корректное число");
                continue;
            }
            attempts++;
            if (guess < number)
                Send(stream, "Больше");
            else if (guess > number)
                Send(stream, "Меньше");
            else
            {
                double time = (DateTime.Now - start).TotalSeconds;
                Send(stream, $"Верно, неудачных попыток: {attempts - 1}, Время: {time:F2} сек.");
                break;
            }
        }
        client.Close();
        server.Stop();
        Console.WriteLine("Сервер остановлен.");
    }
    static void Send(NetworkStream stream, string msg)
    {
        byte[] data = Encoding.UTF8.GetBytes(msg);
        stream.Write(data, 0, data.Length);
    }
}
