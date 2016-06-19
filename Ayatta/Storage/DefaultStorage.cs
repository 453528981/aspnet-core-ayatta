using Dapper;
using System.Linq;
using Ayatta.Domain;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Ayatta.Param;
using System.Text;

namespace Ayatta.Storage
{
    public class DefaultStorage : BaseStorage
    {

        public DefaultStorage(IOptions<StorageOptions> optionsAccessor) : base(optionsAccessor)
        {

        }

        public User UserGet(int id)
        {
            var cmd = SqlBuilder
           .Select("*").From("user")
           .Where("id=@id", new { id })
           .ToCommand();
            return StoreConn.QueryFirstOrDefault<User>(cmd);
        }

        public Prod.Item ProdItemGet(int id, bool includeSkus = false)
        {
            if (includeSkus)
            {
                var sql = @"select * from item where id=@id;select * from sku where itemid=@id;";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                using (var reader = StoreConn.QueryMultiple(cmd))
                {
                    var o = reader.Read<Prod.Item>().FirstOrDefault();
                    if (o != null)
                    {
                        o.Skus = reader.Read<Prod.Sku>().ToList();
                    }
                    return o;
                }
            }
            else
            {
                var cmd = SqlBuilder
                .Select("*").From("item")
                .Where("id=@id", new { id })
                .ToCommand();
                return StoreConn.QueryFirstOrDefault<Prod.Item>(cmd);
            }
        }

        public Prod.Sku ProdSkuGet(int id)
        {
            var cmd = SqlBuilder
            .Select("*").From("sku")
            .Where("id=@id", new { id })
            .ToCommand();
            return StoreConn.QueryFirstOrDefault<Prod.Sku>(cmd);
        }

        public int ProdItemCreate(Prod.Item o)
        {
            using (var conn = StoreConn)
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        var cmd = SqlBuilder.Insert("item")

                        .Column("SpuId", o.SpuId)
                        .Column("UserId", o.UserId)
                        .Column("CatgId", o.CatgId)
                        .Column("CatgRId", o.CatgRId)
                        .Column("CatgMId", o.CatgMId)
                        .Column("Code", o.Code)
                        .Column("Name", o.Name)
                        .Column("Title", o.Title)
                        .Column("Stock", o.Stock)
                        .Column("Price", o.Price)
                        .Column("AppPrice", o.AppPrice)
                        .Column("RetailPrice", o.RetailPrice)
                        .Column("Barcode", o.Barcode)
                        .Column("BrandId", o.BrandId)
                        .Column("BrandName", o.BrandName)
                        .Column("Keyword", o.Keyword)
                        .Column("PropId", o.PropId)
                        .Column("PropStr", o.PropStr)
                        .Column("PropAlias", o.PropAlias)
                        .Column("InputId", o.InputId)
                        .Column("InputStr", o.InputStr)
                        .Column("Width", o.Width)
                        .Column("Depth", o.Depth)
                        .Column("Height", o.Height)
                        .Column("Weight", o.Weight)
                        .Column("Summary", o.Summary)
                        .Column("Picture", o.Picture)
                        .Column("ItemImgStr", o.ItemImgStr)
                        .Column("PropImgStr", o.PropImgStr)
                        .Column("IsVirtual", o.IsVirtual)
                        .Column("IsAutoFill", o.IsAutoFill)
                        .Column("IsTiming", o.IsTiming)
                        .Column("SubStock", o.SubStock)
                        .Column("Showcase", o.Showcase)
                        .Column("OnlineOn", o.OnlineOn)
                        .Column("OfflineOn", o.OfflineOn)
                        .Column("RewardRate", o.RewardRate)
                        .Column("HasInvoice", o.HasInvoice)
                        .Column("HasWarranty", o.HasWarranty)
                        .Column("HasGuarantee", o.HasGuarantee)
                        .Column("SaleCount", o.SaleCount)
                        .Column("CollectCount", o.CollectCount)
                        .Column("ConsultCount", o.ConsultCount)
                        .Column("CommentCount", o.CommentCount)
                        .Column("Status", o.Status)
                        .Column("CreatedOn", o.CreatedOn)
                        .Column("ModifiedBy", o.ModifiedBy)
                        .Column("ModifiedOn", o.ModifiedOn)
                        .ToCommand(true, tran);

                        var status = true;
                        var id = conn.ExecuteScalar<int>(cmd);

                        if (id > 0)
                        {
                            foreach (var sku in o.Skus)
                            {
                                cmd = SqlBuilder.Insert("sku")

                                .Column("SpuId", sku.SpuId)
                                .Column("ItemId", id)
                                .Column("UserId", sku.UserId)
                                .Column("CatgId", sku.CatgId)
                                .Column("CatgRId", sku.CatgRId)
                                .Column("CatgMId", sku.CatgMId)
                                .Column("Code", sku.Code)
                                .Column("Barcode", sku.Barcode)
                                .Column("BrandId", sku.BrandId)
                                .Column("Stock", sku.Stock)
                                .Column("Price", sku.Price)
                                .Column("AppPrice", sku.AppPrice)
                                .Column("PropId", sku.PropId)
                                .Column("PropStr", sku.PropStr)
                                .Column("SaleCount", sku.SaleCount)
                                .Column("Status", sku.Status)
                                .Column("CreatedOn", sku.CreatedOn)
                                .Column("ModifiedBy", sku.ModifiedBy)
                                .Column("ModifiedOn", sku.ModifiedOn)
                                .ToCommand(false, tran);

                                status = conn.Execute(cmd) > 0;
                                if (!status)
                                {
                                    break;
                                }
                            }
                        }
                        if (id > 0 && status)
                        {
                            tran.Commit();
                            return id;
                        }
                        else
                        {
                            tran.Rollback();
                            return 0;
                        }

                    }
                    catch (System.Exception)
                    {
                        tran.Rollback();
                        return 0;
                    }
                }
            }
        }

        public Prod.Mini ProdMiniGet(int id)
        {

            var sql = @"select id,spuid,userid,catgid,catgrid,code,name,title,stock,price,appprice,retailprice,barcode,brandid,brandname,keyword,propid,propstr,inputid,inputstr,picture,ItemImgStr,PropImgStr,status from item where id=@id;
            select id,code,stock,price,propstr,appprice,propid,propstr,status from sku where itemid=@id;";
            var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
            using (var reader = StoreConn.QueryMultiple(cmd))
            {
                var o = reader.Read<Prod.Mini>().FirstOrDefault();
                if (o != null)
                {
                    o.Skus = reader.Read<Prod.Mini.Sku>().ToList();
                }
                return o;
            }
        }

        public bool ProdMiniUpdate(Prod.Mini o)
        {

            using (var conn = StoreConn)
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        var cmd = SqlBuilder
                        .Update("item")
                        .Column("Code", o.Code)
                        .Column("Name", o.Name)
                        .Column("Title", o.Title)
                        .Column("Stock", o.Stock)
                        .Column("AppPrice", o.AppPrice)
                        .Column("RetailPrice", o.RetailPrice)
                        .Column("Barcode", o.Barcode)
                        .Where("id=@id and userId=@userId", new { id = o.Id, userId = o.UserId })
                        .ToCommand(tran);

                        var v = conn.Execute(cmd) > 0;

                        if (v && o.Skus != null && o.Skus.Any())
                        {
                            foreach (var sku in o.Skus)
                            {
                                cmd = SqlBuilder
                                .Update("sku")
                                .Column("Code", sku.Code)
                                .Column("Stock", sku.Stock)
                                .Column("Price", sku.Price)
                                .Column("AppPrice", o.AppPrice)
                                .Where("id=@id and itemId=@itemId and userId=@userId", new { id = sku.Id, itemId = o.Id, userId = o.UserId })
                                .ToCommand(tran);

                                v = conn.Execute(cmd) > 0;

                                if (!v)
                                {
                                    break;
                                }
                            }
                        }
                        if (v)
                        {
                            tran.Commit();
                            return true;
                        }
                        else
                        {
                            tran.Rollback();
                            return false;
                        }
                    }
                    catch (System.Exception)
                    {
                        tran.Rollback();
                        return false;
                        throw;
                    }
                }
            }
        }

        public IPagedList<Prod.Item> ProdItemSearch(ProdItemSearchParam param)
        {
            var sb = SqlBuilder
            .Select("*")
            .From("item");
            if (param.UserId.HasValue)
            {
                sb.Where("userid=@userid", new { param.UserId });
            }
            if (param.Id > 0)
            {
                sb.Where("Id=@id", new { param.Id });
            }
            if (!string.IsNullOrEmpty(param.Name))
            {
                sb.Where("Name LIKE @Name", new { name = param.Name + "%" });
            }
            if (!string.IsNullOrEmpty(param.Code))
            {
                sb.Where("Code=@code", new { param.Code });
            }
            if (param.CRId > 0)
            {
                sb.Where("CatgRId=@CatgRId", new { CatgRId = param.CRId });
            }
            if (param.PriceFrom > 0)
            {
                sb.Where("Price>=@PriceFrom", new { param.PriceFrom });
            }
            if (param.PriceEnd > 0)
            {
                sb.Where("Price<=@PriceEnd", new { param.PriceEnd });
            }
            if (param.SaleFrom > 0)
            {
                sb.Where("SaleCount>=@SaleFrom", new { param.SaleFrom });
            }
            if (param.SaleEnd > 0)
            {
                sb.Where("SaleCount>=@SaleEnd", new { param.SaleEnd });
            }
            if (param.OrderBy != null && param.OrderBy.Count > 0)
            {
                var tmp = new StringBuilder();
                foreach (var o in param.OrderBy)
                {
                    tmp.AppendFormat("[{0}] {1},", o.Key, o.Value ? "ASC" : "DESC");
                }
                sb.OrderBy(sb.ToString().TrimEnd(','));
            }
            else
            {
                sb.OrderBy("Id DESC");
            }
            var cmd = sb.ToCommand(param.PageIndex, param.PageSize);

            return StoreConn.PagedList<Prod.Item>(param.PageIndex, param.PageSize, cmd);

        }

        public Catg CatgGet(int id)
        {
            var sql = @"select * from catg where id=@id;
                select * from catgprop where catgid=@id;
                select * from catgpropvalue where catgid=@id;
                ";
            using (BaseConn)
            using (var multi = BaseConn.QueryMultiple(sql, new { id }, commandTimeout: 60))
            {
                var catg = multi.Read<Catg>().FirstOrDefault();
                var props = multi.Read<Catg.Prop>().ToList();
                var propValues = multi.Read<Catg.Prop.Value>().ToList();

                BaseConn.Close();

                if (catg != null)
                {
                    foreach (var prop in props)
                    {
                        prop.Values = propValues.Where(x => x.PropId == prop.Id).ToList();
                    }
                }
                catg.Props = props;
                return catg;
            }
        }

        /// <summary>
        /// 获取搭配组合套餐
        /// </summary>
        /// <param name="id">搭配组合套餐Id</param>
        /// <param name="includeItems">是否包含套餐附属商品信息</param>
        /// <returns></returns>
        public Promotion.Package PromotionPackageGet(int id, bool includeItems = false)
        {
            if (includeItems)
            {
                const string sql = @"select * from Package where Id=@id;select * from PackageItem where PackageId=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return
                    StoreConn.Query<Promotion.Package, Promotion.Package.Item, int>(cmd, o => o.Id,
                        o => o.PackageId, (a, b) =>
                        {
                            a.Items = b.ToList();
                        }).FirstOrDefault();
            }
            else
            {
                var cmd = SqlBuilder.Select("*")
                        .From("Package")
                        .Where("Id=@id", new { id })
                        .ToCommand();

                return PromotionConn.Query<Promotion.Package>(cmd).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取卖家当前时间可用的限购信息
        /// </summary>
        /// <param name="sellerId">卖家Id</param>
        /// <returns></returns>
        public IList<Promotion.LimitBuy> PromotionLimitBuyList(int sellerId)
        {

            const string sql = @"select * from LimitBuy where Status=1 and StartedOn<getdate() and StoppedOn>getdate() and SellerId=@sellerId;";
            var cmd = SqlBuilder.Raw(sql, new { sellerId }).ToCommand();
            return PromotionConn.Query<Promotion.LimitBuy>(cmd).ToList();

        }

        /// <summary>
        /// 获取卖家当前时间可用的特价
        /// </summary>
        /// <param name="sellerId">卖家Id</param>
        /// <returns></returns>
        public IList<Promotion.SpecialPrice> PromotionSpecialPriceList(int sellerId)
        {
            const string sql = @"select * from SpecialPrice where Status=1 and StartedOn<getdate() and StoppedOn>getdate() and SellerId=@sellerId;
            select a.* from SpecialPriceItem a inner join SpecialPrice b on a.SpecialPriceId=b.Id where a.Status=1 and b.Status=1 and b.StartedOn<getdate() and b.StoppedOn>getdate() and a.SellerId=@sellerId";
            var cmd = SqlBuilder.Raw(sql, new { sellerId }).ToCommand();
            return PromotionConn.Query<Promotion.SpecialPrice, Promotion.SpecialPrice.Item, int>(cmd, o => o.Id, o => o.SpecialPriceId, (a, b) => { a.Items = b.Where(x => x.SpecialPriceId == a.Id).ToList(); }).ToList();

        }

        /// <summary>
        /// 获取卖家当前时间可用的搭配组合套餐
        /// </summary>
        /// <param name="sellerId">卖家Id</param>
        /// <returns></returns>
        public IList<Promotion.Package> PromotionPackageList(int sellerId)
        {
            const string sql = @"select * from Package where Status=1 and StartedOn<getdate() and StoppedOn>getdate() and SellerId=@sellerId;
            select a.* from PackageItem a inner join Package b on a.PackageId=b.Id where a.Status=1 and b.Status=1 and b.StartedOn<getdate() and b.StoppedOn>getdate() and a.SellerId=@sellerId";
            var cmd = SqlBuilder.Raw(sql, new { sellerId }).ToCommand();
            return PromotionConn.Query<Promotion.Package, Promotion.Package.Item, int>(cmd, o => o.Id, o => o.PackageId, (a, b) => { a.Items = b.Where(x => x.PackageId == a.Id).ToList(); }).ToList();
        }

        /// <summary>
        /// 获取卖家当前时间可用的店铺优惠活动
        /// </summary>
        /// <param name="sellerId">卖家Id</param>
        /// <returns></returns>
        public IList<Promotion.Normal> PromotionNormalList(int sellerId)
        {
            const string sql = @"select * from Normal where Status=1 and StartedOn<getdate() and StoppedOn>getdate() and SellerId=@sellerId;
            select a.* from NormalRule a inner join Normal b on a.NormalId=b.Id where a.Status=1 and b.Status=1 and b.StartedOn<getdate() and b.StoppedOn>getdate() and a.SellerId=@sellerId";
            var cmd = SqlBuilder.Raw(sql, new { sellerId }).ToCommand();
            return PromotionConn.Query<Promotion.Normal, Promotion.Normal.Rule, int>(cmd, o => o.Id, o => o.NormalId, (a, b) => { a.Rules = b.Where(x => x.NormalId == a.Id).OrderBy(x => x.Upon).ToList(); }).ToList();
        }

        /// <summary>
        /// 获取卖家当前时间可用的购物车促销
        /// </summary>
        /// <param name="sellerId">卖家Id</param>
        /// <returns></returns>
        public IList<Promotion.Cart> PromotionCartList(int sellerId)
        {
            const string sql = @"select * from Cart where Status=1 and StartedOn<getdate() and StoppedOn>getdate() and SellerId=@sellerId;
            select a.* from CartRule a inner join Cart b on a.CartId=b.Id where a.Status=1 and b.Status=1 and b.StartedOn<getdate() and b.StoppedOn>getdate() and a.SellerId=@sellerId";
            var cmd = SqlBuilder.Raw(sql, new { sellerId }).ToCommand();
            return PromotionConn.Query<Promotion.Cart, Promotion.Cart.Rule, int>(cmd, o => o.Id, o => o.CartId, (a, b) => { a.Rules = b.Where(x => x.CartId == a.Id).ToList(); }).ToList();
        }
    }
}