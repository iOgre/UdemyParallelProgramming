﻿using System;
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
            Test();
            Console.WriteLine("Main program done");
            Console.ReadKey();
        }

        private static void Test()
        {
            var t = Task.Factory.StartNew(() => throw new InvalidOperationException("Can't do this")
            {
                Source = "t"
            });
            var t2 = Task.Factory.StartNew(() => throw new AccessViolationException("Can't access this")
            {
                Source = "t2"
            });
            try
            {
                Task.WaitAll(t, t2);
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    Console.WriteLine($"Exception {e.GetType()} from {e.Source} ");
                }
            }
        }
    }
}