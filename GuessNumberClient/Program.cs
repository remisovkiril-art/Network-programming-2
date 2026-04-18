using System;
using System.Net.Sockets;
using System.Text;
class Program
{
    static void Main()
    {
        TcpClient client = new TcpClient("127.0.0.1", 5000);
        NetworkStream stream = client.GetStream();
        Console.WriteLine("Подключено к серверу");
        while (true)
        {
            Console.Write("Введите число: ");
            string? guess = Console.ReadLine();
            byte[] data = Encoding.UTF8.GetBytes(guess ?? "");
            stream.Write(data, 0, data.Length);
            byte[] buffer = new byte[1024];
            int bytes = stream.Read(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytes);
            Console.WriteLine("Ответ сервера: " + response);
            if (response.Contains("Верно"))
                break;
        }
        client.Close();
        Console.WriteLine("\nНажмите Enter, чтобы закрыть окно...");
        Console.ReadLine();
    }
}
