using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Terminal
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
             
            var t = new Task(() =>
            {
                Console.WriteLine("Press any key to disarm, you have 5 seconds");
              bool cancelled =  token.WaitHandle.WaitOne(5000);
               Console.WriteLine(cancelled ? "Disarmed" : "Boom");
            }, token);
            Console.ReadKey();
            cts.Cancel();
            
            Console.WriteLine("Main program done");
            Console.ReadKey();
        }
    }
}