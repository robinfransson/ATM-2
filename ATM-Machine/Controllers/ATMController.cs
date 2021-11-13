using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace ATM_Machine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ATMController : ControllerBase
    {

        private readonly ATM _atm;

        public ATMController(ATM atm)
        {
            _atm = atm;
        }

        [HttpGet]
        public IActionResult OnGet()
        {
            if (_atm is not null)
                return Ok(new
                {
                    contents = _atm.GetContents()
                                   .Select(x => new { value = x.value, amount = x.count }),
                    total = _atm.AmountLeft
                });


            return NoContent();
        }

        [HttpPost]
        public IActionResult OnPost([FromBody] int amount)
        {
            if (_atm is null)
                return BadRequest("ATM is not available");

            var result = _atm.Withdraw(amount);
            if (result.succeeded)
            { 
                return Ok(new 
                {
                    contents = _atm.GetContents()
                                   .Select(x => new { value = x.value, amount = x.count }),
                    total = _atm.AmountLeft, 
                    withdrawn = result.billsWithdrawn.GroupBy(x => x.Value)
                                                     .Select(group => new {
                                                         value = group.Key, 
                                                         amount= group.Count()
                                                     })
                });
            }


            return BadRequest("Could not withdraw the specified amount");

        }

        [HttpGet]
        [Route("reset")]
        public IActionResult OnGetResetATM()
        {
            _atm.Reset();
            if (_atm.AmountLeft == 4000)
                return Ok();

            return BadRequest("Could not reset the inventory of the ATM");

        }
    }
}