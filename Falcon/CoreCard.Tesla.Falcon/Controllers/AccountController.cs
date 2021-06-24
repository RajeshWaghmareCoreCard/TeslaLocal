using CoreCard.Tesla.Falcon.DataModels.Entity;
using CoreCard.Tesla.Falcon.DataModels.Model;
using CoreCard.Tesla.Falcon.Services;
using CoreCard.Tesla.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountBAL _accountBAL;
        private readonly TimeLogger _timeLogger;
        public AccountController(IAccountBAL accountBAL, TimeLogger timeLogger)
        {
            _accountBAL = accountBAL;
            _timeLogger = timeLogger;
        }

        //// This method returns all accounts. 
        //[HttpGet("GetAll")]
        //public IActionResult GetAllAccounts()
        //{
        //    List<Account> allAccounts = _accountBAL.GetAll();

        //    if (allAccounts.Count == 0)
        //    {
        //        return BadRequest("No account exit, try creating a new account");
        //    }

        //    return Ok(allAccounts);
        //}

        //[HttpGet("Get/{id}")]
        //public async Task<IActionResult> GetAccount(Guid? id)
        //{
        //    Account account = await _accountBAL.GetAsync(id.Value);

        //    if (account == null)
        //    {
        //        return NotFound($"Account with Id {id} doesn't exit");
        //    }

        //    return Ok(account);
        //}


        //[HttpGet("GetLastAccountId")]
        //public int GetLastAccountId()
        //{
        //    return _accountBAL.GetLastAccountId();
        //}

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CustomerAddDTO customer)
        {
            _timeLogger.Start("CreateAccountController");
            BaseResponseDTO createdAccount = await _accountBAL.AddAccountAsync(customer);

            if (createdAccount != null)
            {
                _timeLogger.StopAndLog("CreateAccountController");
                _timeLogger.Dispose();
                return new CreatedAtActionResult("Create", "Account", customer, createdAccount);
            }
            else
            {
                return BadRequest("Error occured please try again.");
            }
        }

        //[HttpPut("Update")]
        //public async Task<IActionResult> UpdateAccountAsync([FromBody] Account account)
        //{
        //    await _accountBAL.UpdateAccountAsync(account);

        //    return Ok("Account updated successfully.");
        //}

        //[HttpGet("accountno/list")]
        //public async Task<BaseResponseDTO> GetAccountNoAsync()
        //{
        //    _timeLogger.Start("GetAccountNoController");
        //    BaseResponseDTO baseResponseDTO = await _accountBAL.GetAccountNoAsync();
        //    baseResponseDTO.ControllerLayerTime = _timeLogger.Stop("GetAccountNoController");
        //    return baseResponseDTO;
        //}

        [HttpGet("account/ado")]
        public async Task<IActionResult> GetAccountByADO(Guid? id)
        {
            Account account = _accountBAL.GetAccountByID_ADO(id.Value);

            if (account == null)
            {
                return NotFound($"Account with Id {id} doesn't exit");
            }

            return Ok(account);
        }
    }
}
