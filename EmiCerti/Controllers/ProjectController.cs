using Microsoft.AspNetCore.Mvc;
using Hashgraph;
using EmiCerti.Data;
using EmiCerti.Models.ProjectModel;
using HelloSign.Client;
using HelloSign.Api;
using HelloSign.Model;
using IronPdf;

namespace EmiCerti.Controllers
{
    public class ProjectController : Controller
    {

        private readonly ILogger<ProjectController> _logger;
        private ApplicationDbContext _db;
        private IConfiguration _configuration;
        private HtmlToPdf _pdfRenderer = new HtmlToPdf();

        public ProjectController(ILogger<ProjectController> logger, ApplicationDbContext db, IConfiguration configuration)
        {
            _logger = logger;
            _db = db;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            IndexProjectViewModel model = new IndexProjectViewModel(_db);
            return View(model);
        }

        public IActionResult Create()
        {
            CreateProjectViewModel model = new CreateProjectViewModel(_db);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectFormModel Form)
        {
            CreateProjectViewModel model = new CreateProjectViewModel(_db);
            model.Form = Form;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var gateway = new Gateway(_configuration["HederaGatewayUrl"], 0, 0, int.Parse(_configuration["HederaNodeNumber"]));
                var payer = new Address(0, 0, long.Parse(_configuration["HederaAccountId"]));
                var payerSignatory = new Signatory(Hex.ToBytes(_configuration["HederaPrivateKey"]));
                var tokenEndorsement = new Endorsement(Hex.ToBytes(_configuration["HederaPublicKey"]));
                var tokenSignatory = new Signatory(Hex.ToBytes(_configuration["HederaPrivateKey"]));
                var createParams = new CreateTokenParams
                {
                    Name = Form.Name,
                    Symbol = Form.TokenSymbol,
                    Circulation = Form.TokenQuantity.Value,
                    Decimals = 0,
                    Treasury = payer,
                    Administrator = tokenEndorsement,
                    GrantKycEndorsement = null,
                    SuspendEndorsement = tokenEndorsement,
                    ConfiscateEndorsement = tokenEndorsement,
                    SupplyEndorsement = tokenEndorsement,
                    InitializeSuspended = false,
                    Expiration = DateTime.UtcNow.AddDays(90),
                    RenewAccount = payer,
                    RenewPeriod = TimeSpan.FromDays(90),
                    Signatory = tokenSignatory
                };

                ;

                await using var client = new Client(ctx =>
                {
                    ctx.Gateway = gateway;
                    ctx.Payer = payer;
                    ctx.Signatory = payerSignatory;
                });
                var receipt = await client.CreateTokenAsync(createParams);
                if(receipt != null)
                {
                    
                    //HelloSign
                    var config = new Configuration();
                    config.Username = _configuration["HelloSignAPI"];
                    var apiInstance = new SignatureRequestApi(config);
                    var signer1 = new SubSignatureRequestSigner(
                        emailAddress: Form.Email,
                        order: 0, name: "Signer"
                    );
                    var signingOptions = new SubSigningOptions(
                        draw: true,
                        type: true,
                        upload: true,
                        phone: true,
                        defaultType: SubSigningOptions.DefaultTypeEnum.Draw
                    );
                    var subFieldOptions = new SubFieldOptions(
                        dateFormat: SubFieldOptions.DateFormatEnum.DDMMYYYY
                    );
                    var metadata = new Dictionary<string, object>()
                    {
                        ["custom_token"] = Form.TokenSymbol
                    };
                    var data = new SignatureRequestSendRequest(
                        title: "Agreement to sell tokens through EmiCerti",
                        subject: "Sell "+Form.TokenSymbol+" tokens",
                        message: "Sign this agreement to authorize and publish your project on EmiCerti to sell your tokens.",
                        signers: new List<SubSignatureRequestSigner>() { signer1 },
                        file: new List<Stream>() { _pdfRenderer.RenderHtmlAsPdf("<h2>I authorize EmiCerti to create and sell "+Form.TokenSymbol+ " tokens on behalf of my project "+Form.Name+"</h2>").Stream },
                        metadata: metadata,
                        signingOptions: signingOptions,
                        fieldOptions: subFieldOptions,
                        testMode: true
                    );
                    try
                    {
                        var result = apiInstance.SignatureRequestSend(data);
                        Project project = new Project
                        {
                            Verifier = Form.Verifier,
                            TokenQuantity = Form.TokenQuantity,
                            Certifier = Form.Certifier,
                            CreatedAt = DateTime.UtcNow,
                            Location = Form.Location,
                            Name = Form.Name,
                            OffsetCategory = Form.OffsetCategory,
                            ProjectType = Form.ProjectType,
                            Protocol = Form.Protocol,
                            ReductionsPeriodEnd = Form.ReductionsPeriodEnd,
                            ReductionsPeriodStart = Form.ReductionsPeriodStart,
                            ReductionstCO2e = Form.ReductionstCO2e,
                            TokenSymbol = Form.TokenSymbol,
                            TokenId = receipt.Token.AccountNum,
                            Enable = false,
                            WebSite = Form.WebSite  ,
                            ImageUrl = Form.ImageUrl,
                            OwnerAccountNumber = Form.OwnerAccountNumber,
                            Expiration = Form.Expiration,
                            HBARsent = false, signatureRequestId = result.SignatureRequest.SignatureRequestId
                        };
                        _db.Projects.Add(project);
                        _db.SaveChanges();
                        return RedirectToAction("View", new { id = project.Id });
                    }
                    catch (ApiException e)
                    {
                    }
                    ModelState.AddModelError("", "Error creating token");
                    return View(model);

                }
                ModelState.AddModelError("", "Error creating token");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        public async Task<IActionResult> View(int id)
        {
            ViewProjectViewModel model = new ViewProjectViewModel(_db, id);
            var gateway = new Gateway(_configuration["HederaGatewayUrl"], 0, 0, int.Parse(_configuration["HederaNodeNumber"]));
            var payer = new Address(0, 0, long.Parse(_configuration["HederaAccountId"]));
            var token = new Address(0, 0, model.Project.TokenId);
            var payerSignatory = new Signatory(Hex.ToBytes(_configuration["HederaPrivateKey"]));
            await using var client = new Client(ctx =>
            {
                ctx.Gateway = gateway;
                ctx.Payer = payer;
                ctx.Signatory = payerSignatory;
            });
            model.Transactions = _db.Transactions.Where(x => x.Id == id).OrderByDescending(x => x.CreatedAt);
            
            return View(model);
        }

        //public IActionResult Buy(int id, long amount)
        //{
        //    BuyTokenViewModel model = new BuyTokenViewModel(_db, id);
        //    model.Form.Amount = amount;
        //    return View(model);
        //}

        [HttpPost]
        public async Task<IActionResult> View(BuyTokenFormModel Form)
        {
            ViewProjectViewModel model = new ViewProjectViewModel(_db, Form.Id);
            model.Form = Form;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var gateway = new Gateway(_configuration["HederaGatewayUrl"], 0, 0, int.Parse(_configuration["HederaNodeNumber"]));
                var payer = new Address(0, 0, long.Parse(_configuration["HederaAccountId"]));
                var token = new Address(0, 0, model.Project.TokenId);
                var account = new Address(0, 0, Form.AccountNumber.Value);
                var payerSignatory = new Signatory(Hex.ToBytes(_configuration["HederaPrivateKey"]));
                var accountSignatory = new Signatory(Hex.ToBytes(Form.PrivateKey));
                await using var client = new Client(ctx =>
                {
                    ctx.Gateway = gateway;
                    ctx.Payer = payer;
                    ctx.Signatory = payerSignatory;
                });
                try
                {
                    var receipt = await client.AssociateTokenAsync(token, account, accountSignatory);

                }
                catch { }
                
                //if (receipt.Status == ResponseCode.Success)
                //{
                    var sendHBAR = await client.TransferAsync(account, payer, Form.Amount.Value * 100_000_000);
                    var receiptToken = await client.TransferTokensAsync(token, payer, account, Form.Amount.Value);
                
                    if (receiptToken.Status == ResponseCode.Success && sendHBAR.Status == ResponseCode.Success)
                    {
                        Transaction transaction = new Transaction
                        {
                             CreatedAt = DateTime.UtcNow, ProjectId = Form.Id, AccountNum = receiptToken.Id.Address.AccountNum,
                            ValidStartNanos = receiptToken.Id.ValidStartNanos,
                            ValidStartSeconds = receiptToken.Id.ValidStartSeconds, From = Form.AccountNumber.Value, Quantity = Form.Amount.Value
                        };
                        _db.Transactions.Add(transaction);
                        _db.SaveChanges();
                        ViewBag.BuyConfirmed = true;
                        return RedirectToAction("View", new { id = Form.Id });
                    }
                //}
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
            return View(model);

        }

        //public async Task<IActionResult> ConfirmTransaction(string id)
        //{
        //    var gateway = new Gateway(_configuration["HederaGatewayUrl"], 0, 0, int.Parse(_configuration["HederaNodeNumber"]));
        //    var payer = new Address(0, 0, long.Parse(_configuration["HederaAccountId"]));
        //    var payerSignatory = new Signatory(Hex.ToBytes(_configuration["HederaPrivateKey"]));
        //    await using var client = new Client(ctx =>
        //    {
        //        ctx.Gateway = gateway;
        //        ctx.Payer = payer;
        //        ctx.Signatory = payerSignatory;
        //    });
        //    var payer = new Address(0, 0, long.Parse(_configuration["HederaAccountId"]));
        //    TxId txId = new TxId(payer,);
        //    var receiptToken = await client.Get
        //}
    }
}
