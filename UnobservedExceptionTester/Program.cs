using System;
using System.Threading.Tasks;

namespace UnobservedExceptionTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var validInput = false;

            do
            {
                Console.WriteLine("\r\nWhat would you like to do? (c)rash the app, (l)ive to fight another day, (n)othing - configuration will decide?");

                var key = Console.ReadKey();

                Console.WriteLine();

                if (key.KeyChar == 'c')
                {
                    TaskScheduler.UnobservedTaskException += (s, e) => throw new Exception();
                    validInput = true;
                }
                else if (key.KeyChar == 'l')
                {
                    TaskScheduler.UnobservedTaskException += (s, e) => Console.WriteLine("Congratulations, you survived!");
                    validInput = true;
                }
                else if (key.KeyChar == 'n')
                    validInput = true;
                else
                    Console.WriteLine("Invalid key");

            } while (!validInput);

            var myClass = new TestClass();
            myClass.ExceptionThrower();

            myClass = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("Done... press any key to close");
            Console.ReadKey();

            Console.WriteLine("bye, bye!");
        }

        private class TestClass
        {
            public void ExceptionThrower()
            {
                var res = Task.Run(() =>
                {
                    throw new Exception("this is a bad idea");
                    return "yeah right";
                });

                ((IAsyncResult)res).AsyncWaitHandle.WaitOne(); // Wait for the task to complete
                res = null; // Allow the task to be GC'ed
            }
        }
    }
}
