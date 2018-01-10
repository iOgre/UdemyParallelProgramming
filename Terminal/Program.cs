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
                Console.WriteLine("I take 5 seconds");
                for (int i = 0; i < 5; i++)
                {
                    token.ThrowIfCancellationRequested();
                    Thread.Sleep(1000);
                }
                Console.WriteLine("I am done in 5 seconds");
                
            }, token);
            t.Start();
            Task t2 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("I take 3 seconds");
                Thread.Sleep(3000);
                Console.WriteLine("3 seconds wait done");
            }, token);


            Task.WaitAll( new [] {t, t2}, 4000);
             Console.WriteLine($" Task t status is {t.Status}, task t2 status is {t2.Status}");
            //Console.ReadKey();
            Console.WriteLine("Main program done");
            Console.ReadKey();
        }
    }
}