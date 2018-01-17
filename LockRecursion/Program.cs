using System;
using System.Threading;
using Microsoft.VisualBasic.CompilerServices;

namespace LockRecursion
{
    class Program
    {
        private static SpinLock sl = new SpinLock(true);
        static void Main(string[] args)
        {
            LockRecursion(5);
        }

        public static void LockRecursion(int x)
        {    
            Console.WriteLine($"X = {x}");
            bool lockTaken = false;
            try
            {
                sl.Enter(ref lockTaken);

            }
            catch (LockRecursionException ex)
            {
                Console.WriteLine("Exception" + ex);
            }
            finally
            {
                if (lockTaken)
                {
                    Console.WriteLine($"Took a lock x = {x}");
                    LockRecursion(x - 1);
                }
                else
                {
                    Console.WriteLine($"Failed to take a lock x = {x}");
                }

            }
        }
    }
    
}