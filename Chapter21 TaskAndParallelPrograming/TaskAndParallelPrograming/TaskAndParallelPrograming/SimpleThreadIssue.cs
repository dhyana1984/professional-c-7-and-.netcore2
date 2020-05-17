using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TaskAndParallelPrograming
{

   
    public class StateObject
    {
        private int _state = 5;
        private object _sync = new object();

        public void ChangeState(int loop)
        {
            //            lock (sync)
            {
                if (_state == 5)
                {
                    _state++;
                    if (_state != 6)
                    {
                        Console.WriteLine($"Race condition occured after {loop} loops");
                        Trace.Fail($"race condition at {loop}");
                    }
                }
                _state = 5;
            }
        }
    }

    public static class SimpleThreadIssue
    {
        public static void DisplaySample()
        {
            var o = new StateObject();
            for (int i = 0; i < 200; i++)
            {
                Task.Run(() => RaceCondition(o));
            }
            Console.ReadLine();
        }


         static void RaceCondition(object o)
        {
            Trace.Assert(o is StateObject, "o must be of type StateObject");
            StateObject state = o as StateObject;
            Console.WriteLine("starting RaceCondition - when does the issue occur?");

            int i = 0;
            while (true)
            {
                //此时锁住state对象，就不会有多个线程同时访问state
                lock (state) // no race condition with this lock
                {
                    state.ChangeState(i++);
                }
            }
        }
    }
}

