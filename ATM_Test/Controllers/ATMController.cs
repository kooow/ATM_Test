using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM_Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ATMController : ControllerBase
    {
        private readonly ILogger<ATMController> _logger;

        public ATMController(ILogger<ATMController> logger)
        {
            _logger = logger;
        }

    }
}
