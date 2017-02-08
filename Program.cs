using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Example1
{
    class Program
    {
        static ConcurrentQueue<int> conQueue = new ConcurrentQueue<int>();
        static void Main(string[] args)
        {
            new Thread(funcPush).Start();

            conQueue.Pop();
            Console.ReadKey();
        }

        static void funcPush()
        {
            int msec = 2000;
            int val = 1777;
            Console.WriteLine("Thread 1 sleeps " + msec + " msec");
            Thread.Sleep(msec);
            Console.WriteLine("Thread 1 set " + val + " into queue");
            conQueue.Push(val);
        }
    }


    class ConcurrentQueue<T>
    {
        private Queue<T> queue = new Queue<T>();

        public void Push(T val)
        {
            Monitor.Enter(this);
                queue.Enqueue(val);
            Monitor.Pulse(this);
            Monitor.Exit(this);
        }

        public T Pop()
        {
            while (true)
            {
                Monitor.Enter(this);
                if (queue.Count > 0)
                {
                    T val = queue.Dequeue();
                    Console.WriteLine("Thread 2 get value " + val + " from queue");
                    return val;
                }
                else
                {
                    Monitor.Wait(this);
                }
                Monitor.Exit(this);
            }
        }
    }


}
