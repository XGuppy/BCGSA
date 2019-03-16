using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBT
{
    class Program
    {
        static void Main(string[] args)
        {
            ReceiverBluetoothService receiver = new ReceiverBluetoothService();
            receiver.Start((str) => Console.WriteLine(str));
            Console.ReadLine();
        }
    }
}
