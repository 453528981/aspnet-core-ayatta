
using System;
using System.Linq;
using System.Text;
using Ayatta.Domain;
using Ayatta.Storage;
using Newtonsoft.Json;
using Ayatta.Extension;
using Ayatta.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Ayatta.Model;
using System.Net.Http;
using System.IO;
using Ayatta.Param;
namespace Ayatta.Web.Controllers
{
    public class ItemController : BaseController
    {

        public ItemController(DefaultStorage defaultStorage) : base(defaultStorage)
        {

        }

        [HttpGet("/items")]
        public IActionResult Index(ProdItemSearchParam param)
        {
            var model = new ItemModel.Index();
            if (param.Keyword.IsMatch(@"^\d+$"))
            {
                param.Id = param.Keyword.AsInt();
            }
            else
            {
                param.Name = param.Keyword;
            }
            model.Param = param;
            model.Items = DefaultStorage.ProdItemSearch(param);
            return View(model);
        }


        [HttpGet("/item/{id}")]
        public IActionResult Item(int id = 0, int catgId = 0, int spuId = 0)
        {
            var data = new Prod.Mini();
            var model = new ItemModel.Item(data);
            model.CatgId = catgId;
            if (id > 0)
            {
                data = DefaultStorage.ProdMiniGet(id);
                model.CatgId = data.CatgId;
                model.Data = data;
            }
            return View(model);
        }

        [HttpPost("/item/{id}")]
        public IActionResult Item(int id, int catgId, int spuId, Prod.Item item)
        {
            var userId = 1;
            var form = Request.Form;
            var time = DateTime.Now;
            var propPrefix = "prop.";
            var itemPrefix = "item.";
            var result = new Result<string>();
            if (item.Name.IsNullOrEmpty() || item.Name.Length > 30)
            {
                result.Data = "name";
                return Json(result);
            }
            if (!item.Title.IsNullOrEmpty() && item.Title.Length > 30)
            {
                result.Data = "title";
                return Json(result);
            }
            if (item.Stock < 1 || item.Stock > 99999999)
            {
                result.Data = "stock";
                return Json(result);
            }
            if (item.RetailPrice < 1 || item.RetailPrice > 99999999)
            {
                result.Data = "retailPrice";
                return Json(result);
            }
            if (item.Price < 1 || item.Price > 99999999)
            {
                result.Data = "price";
                return Json(result);
            }
            if (item.AppPrice < 1 || item.AppPrice > 99999999)
            {
                result.Data = "appPrice";
                return Json(result);
            }
            var summary = form[itemPrefix + "summary"].ToString();
            if (summary.IsNullOrEmpty() || (summary.Length < 5 || summary.Length > 20000))
            {
                result.Data = "summary";
                return Json(result);
            }

            var subStock = form[itemPrefix + "subStock"].ToString().As<byte>(0);

            var isTiming = false;
            var onlineTime = time;
            var itemStatus = Prod.Status.Online;
            var onlineTimeField = form[itemPrefix + "online"];
            if (onlineTimeField == "0" || onlineTimeField == "1")
            {
                if (onlineTimeField == "1")
                {
                    itemStatus = Prod.Status.Offline;
                }
            }
            else if (!DateTime.TryParse(onlineTimeField, out onlineTime))
            {
                //var error = new { name = prefix + "onlineTime", message = "请选择日期及时间！" };
                //errors.Add(error);
                result.Data = "onlinetime";
                result.Message = "请选择日期及时间！";
                return Json(result);
            }
            else
            {
                isTiming = true;
            }

            var data = DefaultStorage.CatgGet(catgId);
            if (data == null || !data.Props.Any())
            {
                result.Message = "参数错误！";
                return Json(result);
            }

            var props = new List<Prod.Prop>();
            var propIdBuilder = new StringBuilder();
            var propStrBuilder = new StringBuilder();
            foreach (var p in data.Props)
            {
                if (p.IsEnumProp && p.Values != null)
                {
                    var val = form[propPrefix + p.Id].ToArray().Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    var array = Array.ConvertAll(val, Convert.ToInt32);

                    foreach (var o in array)
                    {
                        foreach (var v in p.Values)
                        {
                            if (o > 0 && o == v.Id)
                            {
                                var prop = new Prod.Prop();
                                prop.PId = p.Id;
                                prop.VId = v.Id;
                                prop.PName = p.Name;
                                prop.VName = v.Name;
                                prop.Extra = string.Empty;
                                if (p.IsSaleProp)
                                {
                                    prop.Extra = "sale";
                                }
                                if (p.IsColorProp)
                                {
                                    prop.Extra += " color";
                                }
                                if (p.Id == 20000)
                                {
                                    item.BrandId = v.Id;
                                    item.BrandName = v.Name;
                                }
                                props.Add(prop);

                                if (!p.IsSaleProp)
                                {
                                    propIdBuilder.Append(p.Id + ":" + v.Id + ";");
                                    propStrBuilder.Append(p.Id + ":" + v.Id + ":" + p.Name + ":" + v.Name + ";");
                                }
                            }
                        }
                    }
                }
            }

            var skus = new List<Prod.Sku>();

            var saleProps = props.Where(x => x.Extra.Contains("sale")).ToList();

            var saleKeys = new List<string[]>();
            foreach (var g in saleProps.OrderBy(x => x.PId).GroupBy(g => g.PId))
            {
                var keys = g.Select(x => g.Key + ":" + x.VId).ToArray();
                saleKeys.Add(keys);
            }

            var skuKeys = GetAllSkuKeys(saleKeys);
            if (skuKeys != null && skuKeys.Any())
            {
                foreach (var skuKey in skuKeys)
                {

                    if (skuKey.IsNullOrEmpty()) continue;
                    var codeParam = "sku.code." + skuKey;
                    var stockParam = "sku.stock." + skuKey;
                    var priceParam = "sku.price." + skuKey;
                    var appPriceParam = "sku.appPrice." + skuKey;

                    int stock;
                    decimal price, appPrice;

                    var codeVal = form[codeParam].ToString().Trim();
                    var stockVal = form[stockParam].ToString().Trim();
                    var priceVal = form[priceParam].ToString().Trim();
                    var appPriceVal = form[appPriceParam].ToString().Trim();


                    if (stockVal.IsNullOrEmpty() || !int.TryParse(stockVal, out stock) || priceVal.IsNullOrEmpty() || !decimal.TryParse(priceVal, out price) || appPriceVal.IsNullOrEmpty() || !decimal.TryParse(appPriceVal, out appPrice))
                    {
                        continue;
                    }

                    var nvs = skuKey.Split(';');
                    var propstr = string.Empty;
                    foreach (var nv in nvs)
                    {
                        var pid = nv.Split(':')[0].AsInt();
                        var vid = nv.Split(':')[1].AsInt();
                        var pname = saleProps.First(o => o.PId == pid).PName;
                        var vname = saleProps.First(o => o.VId == vid).VName;
                        propstr += string.Format("{0}:{1}:{2}:{3};", pid, vid, pname, vname);
                    }

                    var sku = new Prod.Sku();
                    sku.SpuId = spuId;
                    sku.UserId = userId;
                    sku.CatgId = catgId;
                    sku.CatgRId = 0;
                    sku.CatgMId = "";
                    sku.Code = codeVal;
                    sku.Barcode = "";
                    sku.BrandId = item.BrandId;
                    sku.Stock = stock;
                    sku.Price = price;
                    sku.AppPrice = appPrice;
                    sku.PropId = skuKey;
                    sku.PropStr = propstr.TrimEnd(';');
                    sku.SaleCount = 0;
                    sku.Status = Prod.Status.Online;
                    sku.CreatedOn = time;
                    sku.ModifiedBy = "";
                    sku.ModifiedOn = time;

                    skus.Add(sku);
                }
            }


            var colorImgDic = new Dictionary<string, string>();
            var colorProps = props.Where(x => x.Extra.Contains("color")).ToList();
            foreach (var p in colorProps)
            {
                var key = p.PId + ":" + p.VId;
                var colorParam = "color.img." + key;
                var val = form[colorParam].ToString();

                if (!val.IsNullOrEmpty())
                {
                    colorImgDic.Add(key, val);
                }
            }

            var itemImgs = form["item.img"].ToArray().Where(x => !string.IsNullOrEmpty(x)).Take(5);
            var picture = itemImgs.FirstOrDefault();
            var itemImgStr = string.Join(";", itemImgs);


            item.SpuId = spuId;
            item.UserId = userId;
            item.CatgId = catgId;
            item.CatgRId = 0;
            item.CatgMId = "";
            item.Code = item.Code;
            item.Name = item.Name.StripHtml();
            item.Title = item.Title.StripHtml();

            item.Keyword = "";
            item.PropId = propIdBuilder.ToString().TrimEnd(';');
            item.PropStr = propStrBuilder.ToString().TrimEnd(';');
            item.PropAlias = string.Empty;
            item.InputId = "";
            item.InputStr = "";
            item.Width = 0;
            item.Depth = 0;
            item.Height = 0;
            item.Weight = 0;
            item.Summary = summary;
            item.Picture = picture;
            item.ItemImgStr = itemImgStr;
            item.PropImgStr = JsonConvert.SerializeObject(colorImgDic);
            item.IsVirtual = false;
            item.IsAutoFill = false;
            item.IsTiming = isTiming;
            item.SubStock = subStock;
            item.Showcase = 1;
            item.OnlineOn = time;
            item.OfflineOn = time.AddDays(15);
            item.RewardRate = 0.5m;
            item.HasInvoice = false;
            item.HasWarranty = false;
            item.HasGuarantee = false;
            item.SaleCount = 0;
            item.CollectCount = 0;
            item.ConsultCount = 0;
            item.CommentCount = 0;
            item.Status = itemStatus;
            item.CreatedOn = time;
            item.ModifiedBy = "";
            item.ModifiedOn = time;

            item.Skus = skus;

            var newId = DefaultStorage.ProdItemCreate(item);
            result.Data = newId.ToString();
            result.Status = newId > 0;

            return Json(result);
        }

        [HttpGet("/item/mini/{id}")]
        public IActionResult Mini(int id)
        {
            var model = DefaultStorage.ProdMiniGet(id);

            return PartialView(model);
        }

        [HttpPost("/item/mini/{id}")]
        public IActionResult Mini(int id, Prod.Mini item)
        {
            var result = new Result();
            item.Id = id;
            item.UserId = 1;
            item.Barcode = "aa";
            if (item.Skus != null && item.Skus.Any())
            {
                item.Price = item.Skus.Min(x => x.Price);
                item.Stock = item.Skus.Sum(x => x.Stock);
            }
            result.Status = DefaultStorage.ProdMiniUpdate(item);
            return Json(result);
        }

        protected string[] GetAllSkuKeys(IList<string[]> keys)
        {
            var len = keys.Count;
            if (len < 1)
            {
                return null;
            }
            if (len == 1)
            {
                return keys[0];
            }
            if (len > 1)
            {
                var first = keys[0];
                var next = keys[1];
                var array = new List<string>();
                foreach (var t in first)
                {
                    array.AddRange(next.Select(x => t + ';' + x));
                }
                if (len == 2)
                {
                    return array.ToArray();
                }
                keys.RemoveAt(0);
                keys.RemoveAt(1);
                keys[0] = array.ToArray();

                keys[0] = GetAllSkuKeys(keys);
            }
            return keys[0];
        }

        /// <summary>
        /// 商品图片上传
        /// </summary>
        /// <param name="param"></param>
        /// <param name="prop"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPost("/item/upload/{param}")]
        public ActionResult Upload(string param, IFormFile image = null)
        {
            var result = new Result<string, string>();
            //var identity = User.Identity.Identity();
            if (image != null)
            {
                var fileName = image.FileName.ToLower();

                var extension = fileName.Substring(fileName.LastIndexOf('.'));
                var extensions = new[] { ".gif", ".png", ".jpg", ".jpeg" };
                if (extensions.Contains(extension))
                {
                    var size = image.Length;
                    if (size > 1024 * 1024)
                    {
                        result.Message = "图片类型只能为gif,png,jpg,jpeg，且大小不超过1M！";
                    }
                    else
                    {
                        var name = Guid.NewGuid() + extension;

                        try
                        {

                            var pathName = "/1/img/" + name;
                            var client = new WeedClient("http://localhost:8888");

                            var r = client.UploadFile(pathName, image.OpenReadStream());
                            if (r.Status)
                            {
                                result.Status = true;
                                result.Data = pathName;
                                result.Extra = param;
                            }

                        }
                        catch (Exception e)
                        {
                            result.Message = e.Message;
                        }
                    }
                }
                else
                {
                    result.Message = "图片类型只能为gif,png,jpg,jpeg，且大小不超过1M！";
                }
            }
            else
            {
                result.Message = "请选择图片！";
            }
            var data = JsonConvert.SerializeObject(result);
            var js = string.Format("<script type='text/javascript'>window.parent.item.new.imageUploadCallback({0});</script>", data);
            return Content(js, "text/html");
        }


    }

    public class WeedClient
    {
        private readonly HttpClient hc;
        public WeedClient(string baseUri)
        {
            hc = new HttpClient();
            hc.BaseAddress = new Uri(baseUri);
        }
        public Result UploadFile(string pathFile, Stream stream)
        {
            var result = new Result();
            var content = new System.Net.Http.MultipartFormDataContent();
            content.Add(new System.Net.Http.StreamContent(stream));
            var msg = hc.PostAsync(pathFile, content).Result.Content.ReadAsStringAsync().Result;
            if (!msg.Contains("error"))
            {
                result.Status = true;
            }
            return result;
        }
    }

    public class WeedFiler
    {
        public int UserId { get; private set; }
        public string BaseUrl { get; private set; }

        private readonly HttpClient hc;
        public WeedFiler(int userId)
        {
            UserId = userId;
            hc = new HttpClient();
            hc.BaseAddress = new Uri(BaseUrl);
        }
    }
}