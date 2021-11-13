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
            Console.WriteLine("Welcome to the ATM Machine!");
            while (true)
            {
                if(_atm.AmountLeft == 0)
                {
                    Console.WriteLine("The ATM is empty, press R to reset or any other key to quit.");

                    var choice = Console.ReadKey();
                    if (choice.Key == ConsoleKey.R)
                    {
                        _atm.Reset();
                        Console.Clear();
                        continue;

                    }
                    else
                    {
                        break;
                    }
                        
                }
                Console.WriteLine("Current inventory of the ATM is: ");
                Console.WriteLine(string.Join("\n", GetContents()));
                Console.Write("Enter amount to withdraw or enter reset to reset the atm: ");

                var input = Console.ReadLine();
                if (input.ToLower() == "reset")
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
