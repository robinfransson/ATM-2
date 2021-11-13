using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ATM_Machine
{
    public class ATM
    {

        private List<Bill> Bills { get; set; }
        public int AmountLeft => Bills.Sum(x => x.Value);
        public ATM()
        {
            Bills = new List<Bill>()
                {
                    new Bill(1000),
                    new Bill(1000),
                    new Bill(500),
                    new Bill(500),
                    new Bill(500),
                    new Bill(100),
                    new Bill(100),
                    new Bill(100),
                    new Bill(100),
                    new Bill(100)
                };
        }

        public ATM(List<Bill> bills)
        {
            Bills = bills;
        }

        public List<(bool succeeded, List<Bill> billsWithdrawn)> Withdraw(params int[] amounts)
        {
            var results = new List<(bool success, List<Bill> billsWithdrawn)>();
            for (int i = 0; i < amounts.Length; i++)
            {
                var amount = amounts[i];
                results.Add(Withdraw(amount));
            }
            return results;
        }



        public (bool succeeded, List<Bill> billsWithdrawn) Withdraw(int amount)
        {
            if (amount % 100 is not 0 || AmountLeft < amount || amount is 0)
                return (false, null);

            return TryWithdraw(amount);
        }



        private (bool succeeded, List<Bill> billsWithdrawn) TryWithdraw(int amountToWithdraw)
        {
            var withdrawnBills = new List<Bill>();

            if (!Bills.Any())
                return (false, null);

            var thousandsToWithdraw = amountToWithdraw / 1000;
            if(thousandsToWithdraw > 0)
                amountToWithdraw -= Remove(1000, thousandsToWithdraw, withdrawnBills);


            var fiveHundredsToWithdraw = amountToWithdraw / 500;
            if (fiveHundredsToWithdraw > 0)
                amountToWithdraw -= Remove(500, fiveHundredsToWithdraw, withdrawnBills);


            var hundredsToWithdraw = amountToWithdraw / 100;
            if (hundredsToWithdraw > 0)
                amountToWithdraw -= Remove(100, hundredsToWithdraw, withdrawnBills);


            if (amountToWithdraw == 0)
                return (true, withdrawnBills);

            Bills.AddRange(withdrawnBills);
            return (false, null);

        }

        private int Remove(int value, int amount, List<Bill> withdrawnBills)
        {
            var billsToRemove = Bills.Where(x => x.Value == value).Take(amount).ToList();

            if (billsToRemove.Any())
            {
                billsToRemove.ForEach(x => Bills.Remove(x));
                withdrawnBills.AddRange(billsToRemove);
            }

            return billsToRemove.Sum(x => x.Value);
        }
        public List<(int value, int count)> GetContents()
        {
            var thousands = Bills.Count(x => x.Value == 1000);
            var fiveHundreds = Bills.Count(x => x.Value == 500);
            var hundreds = Bills.Count(x => x.Value == 100);

            return new()
            {
                new(1000, thousands),
                new(500, fiveHundreds),
                new(100, hundreds)
            };

        }

        public void Reset()
        {
            Bills = new List<Bill>()
                {
                    new Bill(1000),
                    new Bill(1000),
                    new Bill(500),
                    new Bill(500),
                    new Bill(500),
                    new Bill(100),
                    new Bill(100),
                    new Bill(100),
                    new Bill(100),
                    new Bill(100)
                };
        }

    }
}
