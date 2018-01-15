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
            _balance += amount;
        }


        public void Widthdraw(int amount)
        {
            _balance -= amount;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var tasks = new List<Task>();
            var ba = new BankAccount();
            SpinLock sl = new SpinLock();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool lockTaken = false;
                        try
                        {
                            sl.Enter(ref lockTaken);
                            ba.Deposit(100);
                        }
                        finally
                        {
                            if (lockTaken) sl.Exit();
                        }

                    }
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool lockTaken = false;
                        try
                        {
                            sl.Enter(ref lockTaken);
                            ba.Widthdraw(100);
                        }
                        finally
                        {
                            if (lockTaken) sl.Exit();
                        }
                       
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"Final balance {ba.Balance}.");
            // Console.ReadKey();
        }
    }
}