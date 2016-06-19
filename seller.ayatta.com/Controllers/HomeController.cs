//using System.Linq;
using Ayatta.Storage;
using Microsoft.AspNetCore.Mvc;

namespace Ayatta.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(DefaultStorage defaultStorage) : base(defaultStorage)
        {

        }
        [HttpGet("/")]
        public IActionResult Index()
        {
            return View();
        }


    }
}