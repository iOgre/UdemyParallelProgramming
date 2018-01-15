using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace Part2Synchronisations
{
    public class BankAccount
    {
        private int _balance;

        public int Balance
        {
            get { return _balance; }
            private set { _balance = value; }
        }

        public void Deposit(int amount)
        {
            //+=
            // op1: temp <- get_Balance() + amount
            // op2: set_Balance(temp)
            Interlocked.Add(ref _balance, amount);
        }


        public void Widthdraw(int amount)
        {
            Interlocked.Add(ref _balance, -amount);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var tasks = new List<Task>();
            var ba = new BankAccount();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        ba.Deposit(100);
                    }
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        ba.Widthdraw(100);
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"Final balance {ba.Balance}.");
            // Console.ReadKey();
        }
    }
}