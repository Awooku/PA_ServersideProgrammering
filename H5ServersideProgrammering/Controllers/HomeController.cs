using H5ServersideProgrammering.Areas.Identity.Codes;
using H5ServersideProgrammering.Codes;
using H5ServersideProgrammering.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace H5ServersideProgrammering.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Class1 _class1;
        private readonly HashingExample _hashingExample;
        private readonly IServiceProvider _serviceProvider;
        private readonly MyUserRoleHandler _myUserRoleHandler;
        private readonly IDataProtector _dataProtector;
        private readonly CrytpExample _crytpExample;

        public HomeController(
            ILogger<HomeController> logger, 
            Class1 class1, 
            HashingExample hashingExample, 
            IServiceProvider serviceProvider,
            MyUserRoleHandler myUserRoleHandler,
            IDataProtectionProvider dataProtector,
            CrytpExample crytpExample)
        {
            _logger = logger;
            _class1 = class1;
            _hashingExample = hashingExample;
            _serviceProvider = serviceProvider;
            _myUserRoleHandler = myUserRoleHandler;
            _crytpExample = crytpExample;
            _dataProtector = dataProtector.CreateProtector("MyProject.MyClass.SomeUniqName");
        }

        [Authorize("RequireAuthenticatedUser")]
        public IActionResult Index()
        {
            string txt = "Hello World";
            string txt3 = "Hello Niels";

            string myText = _class1.GetText();
            string myText2 = _class1.GetText2();
            string myHashedText = _hashingExample.GetHashedText_MD5(txt);

            string myEncryptText = _crytpExample.Encrypt(txt3, _dataProtector);
            string myDecryptText = _crytpExample.Decrypt(myEncryptText, _dataProtector);

            IndexModel myModel = new IndexModel() { Text1 = myHashedText, Text2 = myText2, Text3 = myEncryptText, Text4 = myDecryptText };

            return View(model: myModel);
        }

        [Authorize(Policy = "RequireAdminUser")]
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
