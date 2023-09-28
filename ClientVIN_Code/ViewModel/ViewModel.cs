using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using shared_data;
using PropertyChanged;
using ClientVIN_Code.Help;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;

namespace ClientVIN_Code.ViewModel
{
    [AddINotifyPropertyChangedInterface]

    public class ViewModel
    {
        public CarData? MyCarData { get; set; }
        public List<Operation> EmptyCollection = new List<Operation>();
        public string VINCode { get; set; }
        public ViewModel()
        {
            getCarDataCmd = new((o) => GetCarData());
        }

        private readonly RelayCommand getCarDataCmd;
        public ICommand GetCarDataCmd => getCarDataCmd;
        public async void GetCarData()
        {
            // адрес та порт сервера, до якого відбувається підключення
            int port = 8080;              // порт сервера
            string address = "127.0.0.1"; // адреса сервера
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
            TcpClient client = new TcpClient();
            // підключення до віддаленого хоста
            client.Connect(ipPoint);
            try
            {
                NetworkStream ns = client.GetStream();

                StreamWriter sw = new StreamWriter(ns); // розмір буфера за замовчуванням: 1KB
                sw.WriteLine(VINCode);
                sw.Flush();
                BinaryFormatter formatter = new BinaryFormatter();
                var request = (CarData)formatter.Deserialize(ns);
                if (request != null)
                    MyCarData = request;

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                MessageBox.Show("Car not found(");
            }
            finally
            {
                // закриваємо підключення
                //client.Close();
            }
        }
    }
}
