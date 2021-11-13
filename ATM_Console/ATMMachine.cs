using ATM_Machine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATM_Console
{
    public class ATMMachine
    {
        private readonly ATM _atm = new();
        public IEnumerable<string> GetContents()
        {
            var contents = _atm.GetContents();
            return contents.Select(x => $"{x.value} x {x.count}");
        }

        public void Start()
        {
            bool quit = false;
            Console.WriteLine("Welcome to the ATM Machine!");
            while (!quit)
            {
                Console.WriteLine("Current inventory of the ATM is: ");
                Console.WriteLine(string.Join("\n", GetContents()));
                Console.Write("Enter amount to withdraw or enter reset to reset the atm: ");
                var input = Console.ReadLine();
                if (input.Contains("reset", StringComparison.CurrentCultureIgnoreCase))
                {
                    _atm.Reset();
                    Console.WriteLine("Resetting the ATM..");
                }
                else
                    while (!ParseInput(input))
                    {
                        Console.WriteLine("Could not withdraw the specified amount, please try again:");
                        input = Console.ReadLine();
                    }
                Thread.Sleep(3000);
                Console.Clear();
            }
        }

        public bool ParseInput(string input)
        {
            if (int.TryParse(input, out int amount))
            {
                return CanWithdrawnFromAtm(amount);
            }
            else
            {
                return false;
            }
        }

        public bool CanWithdrawnFromAtm(int amount)
        {
            var result = _atm.Withdraw(amount);
            if (result.succeeded)
            {
                Console.WriteLine("Successfully withdrew from the ATM!");
                Console.WriteLine("The bills withdrawn were:");
                var groupedBills = result.billsWithdrawn.GroupBy(x => x.Value).Select(grp => $"{grp.Key} x {grp.Count()}");
                Console.WriteLine(string.Join("\n", groupedBills));
            }
            return result.succeeded;
        }
    }
}
