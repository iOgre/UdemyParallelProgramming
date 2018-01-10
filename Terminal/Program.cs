using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Terminal
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var cts = new CancellationTokenSource(3000);

            var token = cts.Token;
            token.Register(() => { Console.WriteLine("Cancellation requested"); });
            var t = new Task(() =>
            {
                int i = 0;
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    Console.WriteLine($"{i++} \t");
                }
            }, token);
            Task.Factory.StartNew(() =>
            {
                token.WaitHandle.WaitOne();
                Console.WriteLine("Wait handle released, cancelaltion was requested");
            });
            t.Start();
            Console.ReadLine();
            cts.Cancel();
            Console.WriteLine("Main program done");
            Console.ReadKey();
        }
    }
}