using Barebone.Controllers;
using Manufactures.Application.LogHistories.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [ApiController]
    [Authorize]
    [Route("log-history")]
    public class LogHistoryController : ControllerApiBase
    {
        public LogHistoryController(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetData(DateTime dateFrom, DateTime dateTo)
        {
            VerifyUser();
            LogHistoryQuery query = new LogHistoryQuery(dateFrom,dateTo, WorkContext.Token);
            var viewModel = await Mediator.Send(query);

            return Ok(viewModel.data);
        }
    }
}
