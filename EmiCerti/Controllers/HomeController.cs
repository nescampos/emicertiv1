using EmiCerti.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Hashgraph;

namespace EmiCerti.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            ViewBag.Environment = _configuration["Environment"];
            return View();
        }

        public async Task<IActionResult> CreateAccount(string create)
        {
            if(string.IsNullOrEmpty(create))
            {
                return View();
            }
            var gateway = new Gateway(_configuration["HederaGatewayUrl"], 0, 0, int.Parse(_configuration["HederaNodeNumber"]));
            var payer = new Address(0, 0, long.Parse(_configuration["HederaAccountId"]));
            var privateKey = Hex.ToBytes(_configuration["HederaPrivateKey"]);
            var payerSignatory = new Signatory(privateKey);
            await using var client = new Client(ctx =>
            {
                ctx.Gateway = gateway;
                ctx.Payer = payer;
                ctx.Signatory = payerSignatory;
            });
            var createParams = new CreateAccountParams
            {
                Endorsement = new Endorsement(Hex.ToBytes(_configuration["HederaPublicKey"])),
                InitialBalance = 300_000_000
            };
            var account = await client.CreateAccountAsync(createParams);
            var address = account.Address.AccountNum;
            ViewBag.Address = address;
            ViewBag.AccountCreated = true;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}