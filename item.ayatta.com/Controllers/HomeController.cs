//using System.Linq;

using Ayatta.Storage;
using  Ayatta.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ayatta.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(DefaultStorage defaultStorage) : base(defaultStorage)
        {

        }
        [HttpGet("/{id}.html")]
        public IActionResult Index(int id=0)
        {
            var model=new HomeModel.Index();
            model.Item=DefaultStorage.ProdItemGet(id,true);
            return View(model);
        }
    }
}