using Ayatta.Storage;
using ProtoBuf;
using Microsoft.AspNetCore.Mvc;

namespace Ayatta.Web.Controllers
{
    public class GlobalController : BaseController
    {

        public GlobalController(DefaultStorage defaultStorage) : base(defaultStorage)
        {

        }
        /*
                [HttpGet("/global/catgs")]
                public IActionResult Catgs(int parentId = 0)
                {
                    var data = LocalProdCache.GetCatgs(parentId);
                    return Json(data);
                }
        */

        [HttpGet("/global/catg/{id}")]
        public IActionResult Catg(int id = 50013794)
        {

            //using (var file = System.IO.File.OpenRead("F://project/vscode/cache/catg." + id + ".dat"))
            //{
            //    var data = Serializer.Deserialize<Ayatta.Model.Catg>(file);
            //    return Json(data);
            // }
            var data = DefaultStorage.CatgGet(id);
            return Json(data);
        }

        [HttpGet("/global/item/{id}")]
        public IActionResult Item(int id = 0)
        {
            var data = DefaultStorage.ProdItemGet(id, true);
            return Json(data);
        }
        
        /*

                [HttpGet("/global/props")]
                public IActionResult Props(int catgId = 0, int propId = 0)
                {
                    var props = LocalProdCache.GetProps(catgId);
                    var propVaues = LocalProdCache.GetPropValues(catgId);
                    if (propId > 0)
                    {
                        propVaues = LocalProdCache.GetPropValues(catgId).Where(x => x.PropId == propId).ToList();
                    }
                    var data = new { props, propVaues };
                    return Json(data);
                }
                */
    }
}