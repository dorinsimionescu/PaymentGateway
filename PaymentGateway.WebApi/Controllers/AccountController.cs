using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Application.WriteOperations;
using PaymentGateway.PublishedLanguage.WriteSide;

namespace PaymentGateway.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly CreateAccount _createAccountCommandHandler;
        public AccountController(CreateAccount createAccountCommandHandler)
        {
            _createAccountCommandHandler = createAccountCommandHandler;
        }

        [HttpPost]
        [Route("Create")]
        public string CreateAccount(MakeNewAccount command)
        {
            //CreateAccount request = new CreateAccount(new EventSender());
            _createAccountCommandHandler.PerformOperation(command);
            return "OK";
        }
    }
}
