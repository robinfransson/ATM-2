using System;
using Xunit;
using ATM_Machine;
using Xunit.Abstractions;
using System.Linq;
using System.Collections.Generic;
using Xunit.Extensions.Ordering;
using ATM_Machine.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ATM_Tests
{
    public class UnitTest1
    {
        private readonly ATM _atm = new();

        [Fact] 
        public void Withdraw_Should_Empty_ATM_Bills()
        {
            int startAmount = _atm.AmountLeft;
            var results = _atm.Withdraw(1500,700,400,1100,1000,700,300);
            var withdrawn = results.Where(x => x.billsWithdrawn is not null).SelectMany(x => x.billsWithdrawn).Sum(x => x.Value);
            Assert.Equal(0, _atm.AmountLeft);
            Assert.Equal(startAmount, withdrawn);

        }

        [Fact]
        public void Withdraw_Should_Return_Result_False()
        {
            var bills = new List<Bill>()
            {
                new Bill(1000),
                new Bill(500)
            };
            var atm = new ATM(bills);

            var result = atm.Withdraw(100);
            Assert.False(result.success);
            Assert.Equal(1500, atm.AmountLeft);


        }

        [Fact]
        public void Withdraw_Less_Than_100_Should_Return_Result_False()
        {
            int startAmount = _atm.AmountLeft;
            var result = _atm.Withdraw(10);
            Assert.False(result.success);
            Assert.Equal(startAmount, _atm.AmountLeft);
        }

        [Fact]
        public void Post_Controller_Should_Return_OK()
        {
            var controller = new ATMController(_atm);
            var result = controller.OnPost(100);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Post_Controller_Should_Return_BadRequest()
        {
            var bills = new List<Bill>()
            {
                new(1000),
                new(1000),
                new(500)
            };
            var atm = new ATM(bills);
            var controller = new ATMController(atm);
            var result = controller.OnPost(100);
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
