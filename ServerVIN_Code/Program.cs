using System.Net;
using System.Net.Sockets;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using shared_data;
using System.Net.Http.Json;
using System.Text.Json;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;

namespace ServerVIN_Code
{
    public class Program
    {
        // порт та адреса для приймання вхідних підключень
        static int port = 8080;
        static string address = "127.0.0.1"; // localhost
        static async Task Main(string[] args)
        {
            string apiKey = ReadAPIKeyFromFile();

            Console.OutputEncoding = Encoding.UTF8;
            // створення кінцевої точки для запуску сервера
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

            // створюємо сокет на вказаній кінцевій точці
            TcpListener listener = new TcpListener(ipPoint);

            // запуск приймання підключень на сервер
            listener.Start(10);

            while (true)
            {
                Console.WriteLine("Server started! Waiting for connection...");
                TcpClient client = listener.AcceptTcpClient(); // wait until connection

                try
                {
                    while (client.Connected)
                    {
                        using (NetworkStream ns = client.GetStream())
                        {
                            string vinCode;
                            //Отримуємо від клієнта VIN код
                            StreamReader sr = new StreamReader(ns);
                            vinCode = sr.ReadLine();

                            //Показуємо на сервері який VIN код отримали та від кого
                            Console.WriteLine($"Request data: {vinCode} from {client.Client.RemoteEndPoint}");
                            //Отримуємо від API дані по VIN коду.
                            var response = await APIGetData(apiKey, vinCode);
                            // Приходить object (ентіті CarData/string якщо не знайдено).
                            if (response is CarData carData)
                            {
                                BinaryFormatter formatter = new BinaryFormatter();
                                formatter.Serialize(ns, carData);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    // закриваємо сокет
                    //client.Close();
                }
            }
            listener.Stop();
        }
        public static async Task<CarData?> APIGetData(string apiKey, string vinCode)
        {
            Console.OutputEncoding = Encoding.UTF8;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);

                string apiUrl = $"https://baza-gai.com.ua/vin/{vinCode}";

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    CarData carData = JsonSerializer.Deserialize<CarData>(content);
                    WriteLog(vinCode, content);
                    return carData;
                }
                else
                {
                    Console.WriteLine($"HTTP Error: {response.StatusCode}");
                    return null;
                }
            }
        }
        public static string ReadAPIKeyFromFile()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "APIKey.txt");
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            else
            {
                Console.WriteLine("API key file not found. EXIT the program!");
                Environment.Exit(1);
                return string.Empty;
            }
        }
        public static void WriteLog(string filenNameJSON, string content)
        {
            //Логуємо запити і JSON відповіді в окрему папку на сервері
            string logDirectoryPath = "Logs"; // Шлях до директорії для логів
            string logFileName = Path.Combine("Logs", filenNameJSON + ".json"); // Повний шлях до файлу логу
            if (!Directory.Exists(logDirectoryPath))
            {
                // Якщо директорія не існує, створіть її
                Directory.CreateDirectory(logDirectoryPath);
            }
            using (StreamWriter writer = File.AppendText(logFileName))
            {
                writer.WriteLine(content);
            }
        }
    }
}