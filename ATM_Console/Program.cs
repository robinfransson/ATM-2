using ATM_Machine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ATM_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var atmMachine = new ATMMachine();
            atmMachine.Start();
        }

    }
}
