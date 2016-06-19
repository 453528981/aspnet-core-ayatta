using System;
using ProtoBuf;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Ayatta.Storage;
using Ayatta.Domain;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Ayatta.ShoppingCart
{
    public class ShoppingCartManager
    {
        private readonly DefaultStorage storage;
        private readonly RedisCache cache;
        private ILogger logger;
        public ShoppingCartManager(DefaultStorage storage, IOptions<RedisCacheOptions> optionsAccessor, ILogger<ShoppingCartManager> logger)
        {
            if (storage == null)
            {
                throw new ArgumentNullException(nameof(storage));
            }
            this.storage = storage;
            cache = new RedisCache(optionsAccessor);
            this.logger = logger;
        }

        /// <summary>
        /// 获取购物车信息
        /// </summary>
        /// <param name="guid">购物车Guid</param>
        /// <returns></returns>
        public Status<Cart> Get(string guid)
        {
            var status = new Status<Cart>(500);

            if (guid.Length != 36)
            {
                status.Message = "参数错误(Guid)";
                return status;
            }

            try
            {
                var cart = GetOrCreatCart(guid);
                if (cart == null)
                {
                    status.Message = "获取或创建购物车缓存失败";
                    return status;
                }
                status.Code = 0;
                status.Data = cart;

                return status;
            }
            catch (Exception e)
            {
                status.Message = e.Message;
                return status;
            }
        }

        /// <summary>
        /// 商品操作
        /// Status.Code <![CDATA[ 100>=code<200 商品相关信息 200>=code<300 卖家相关信息 ]]>
        /// </summary>
        /// <param name="guid">购物车Guid</param>
        /// <param name="operate">操作方式</param>        
        /// <param name="itemId">ItemId</param>
        /// <param name="skuId">SkuId</param>
        /// <param name="quantity">数量</param>
        /// <returns></returns>
        public Status<Cart> Product(string guid, Operate operate, int itemId, int skuId = 0, int quantity = 1)
        {
            var status = new Status<Cart>(500);

            if (guid.Length != 36)
            {
                status.Message = "参数错误(Guid)";

                return status;
            }
            if (itemId < 1 && skuId < 1)
            {
                status.Message = "参数错误(ItemId,SkuId)";
                return status;
            }
            if (skuId > 0 && itemId < 1)
            {
                status.Message = "参数错误(ItemId)";
                return status;
            }
            if (quantity < 1)
            {
                status.Message = "参数错误(Quantity)";
                return status;
            }

            try
            {
                var cart = GetOrCreatCart(guid);
                if (cart == null)
                {
                    status.Message = "获取或创建购物车缓存失败";
                    return status;
                }
                var temp = ProductOpt(cart,operate, itemId, skuId,quantity);
                if (!temp)
                {
                    status.Code = temp.Code;
                    status.Message = temp.Message;
                    return status;
                }

                status.Message = "更新购物车缓存失败";
                return status;
            }
            catch (Exception e)
            {
                status.Message = e.Message;
                return status;
            }
        }
        
        /// <summary>
        /// 搭配组合套餐操作
        /// Status.Code <![CDATA[ 100>=code<200 搭配组合套餐相关信息 200>=code<300 卖家相关信息 ]]>
        /// </summary>
         /// <param name="guid">购物车Guid</param>
        /// <param name="operate">操作方式</param>
       
        /// <param name="packageId">搭配组合套餐Id</param>
        /// <param name="subIds">搭配组合套餐附属商品Ids</param>
        /// <returns></returns>
        public Status<Cart> Package(string guid,Operate operate,  int packageId, int[] subIds = null)
        {
            var status = new Status<Cart>(500);

            if (packageId < 1)
            {
                status.Message = "参数错误(PackageId)";
                return status;
            }

            try
            {
                
                var cart = GetOrCreatCart(guid);
                if (cart == null)
                {
                    status.Message = "获取或创建购物车缓存失败";
                    return status;
                }               

                var temp = PackageOpt(cart,operate, packageId, subIds);
                if (!temp)
                {
                    status.Code = temp.Code;
                    status.Message = temp.Message;
                    return status;
                }
                

                status.Message = "更新购物车缓存失败";
                return status;
            }
            catch (Exception e)
            {
                status.Message = e.Message;
                return status;
            }
        }


        /// <summary>
        /// 设置收货地址
        /// </summary>
        /// <param name="address">收货地址</param>
        /// <returns></returns>
        public Status<Cart> SetAddress(string guid, Address address)
        {
            var status = new Status<Cart>(500);
            if (address == null)
            {
                status.Message = "参数错误(Address)";
                return status;
            }
            try
            {
                var cart = GetOrCreatCart(guid);
                if (cart == null)
                {
                    status.Message = "获取或创建购物车缓存失败";
                    return status;
                }
                cart.Address = address;
                status.Code = 0;
                status.Data = cart;

                return status;
            }
            catch (Exception e)
            {
                status.Message = e.Message;
                return status;
            }
        }

        /// <summary>
        /// 清空购物车
        /// </summary>
        /// <param name="guid">购物车Guid</param>
        /// <returns></returns>
        public Status Purge(string guid)
        {
            var status = new Status(500);
            if (guid.Length != 36)
            {
                status.Message = "参数错误(Guid)";
                return status;
            }
            try
            {
                cache.Remove(guid);
                status.Code = 0;
            }
            catch (Exception e)
            {
                status.Code = 500;
                status.Message = e.Message;
            }
            return status;
        }       


        /// <summary>
        /// 商品操作
        /// Status.Code <![CDATA[ 100>=code<200 商品相关信息 200>=code<300 卖家相关信息 ]]>
        /// </summary>
        /// <param name="operate">购物车</param>
        /// <param name="operate">操作方式</param>        
        /// <param name="itemId">ItemId</param>
        /// <param name="skuId">SkuId</param>
        /// <param name="quantity">数量</param>
        /// <returns></returns>
        private Status ProductOpt(Cart cart, Operate operate,int itemId, int skuId = 0, int quantity = 1)
        {
            var status = new Status();
            var isSku = (itemId > 0 && skuId > 0);
            var key = string.Format("product.item.{0}", itemId);
            var item = cache.Put(key, () => storage.ProdItemGet(itemId, true), DateTime.Now.AddHours(2));

            if (item == null)
            {
                status.Code = 100;
                status.Message = "商品不存在";
                return status;
            }
           if (item.Status!=Prod.Status.Online)
            {
                status.Code = 101;
                status.Message = "商品已下架";
                return status;
            }
            if (item.Stock<1)
            {
                status.Code = 102;
                status.Message = "商品已售完";
                return status;
            }
            if (item.Skus != null && item.Skus.Count > 0)
            {
                if (skuId < 1)
                {
                    status.Code = 150;
                    status.Message = "参数错误(SkuId)";
                    return status;
                }
            }
            if (isSku)
            {
                if (item.Skus == null || !item.Skus.Any())
                {
                    status.Code = 150;
                    status.Message = "商品Sku不存在";
                    return status;
                }
            }
            var basket = GetOrCreatBasket(cart, item.UserId);
            if (basket == null)
            {
                status.Code = 200;
                status.Message = "卖家不存在";
                return status;
            }
            switch (operate)
            {
                case Operate.Increase:
                    {
                        if (isSku)
                        {
                            var sku = item.Skus.FirstOrDefault(x => x.Id == skuId);
                            if (sku == null)
                            {
                                status.Code = 150;
                                status.Message = "商品Sku不存在";
                                return status;
                            }
                            if (sku.Status!=Prod.Status.Online)
                            {
                                status.Code = 151;
                                status.Message = "商品Sku已下架";
                                return status;
                            }
                            if (sku.Stock<1)
                            {
                                status.Code = 152;
                                status.Message = "商品Sku已售完";
                                return status;
                            }
                            if (basket.Skus.ContainsKey(skuId))
                            {
                                //限购检查
                                quantity += basket.Skus[skuId].Quantity;                                
                                
                                var result = ValidateLimitBuy(basket.SellerId,cart.UserId, sku.ItemId, quantity);
                                if (result)//没有限购 或者没超出限购数量
                                {
                                    basket.Skus[skuId].Quantity = quantity;
                                    return status;
                                }
                                else
                                {
                                    basket.Skus[skuId].LimitBuy=result.Data;//限购数量 0为无限购
                                    status.Message = string.Format("限购{0}件，已购买过{1}件 最多还可购买{0}件",result.Data, result.Extra,quantity);
                                    status.Code = 301;
                                    return status;
                                }      
                            }
                            var data = new Cart.Basket.Sku()
                            {
                                Id = sku.Id,
                                ItemId = sku.ItemId,
                                Name = item.Name,
                                Price = sku.Price,
                                Quantity = 1,
                                Selected=true
                            };

                            status.Code = basket.Skus.TryAdd(skuId, data) ? 0 : 300;
                            if (status) return status;
                            status.Message = "添加到购物车失败";
                            return status;
                        }
                        else
                        {
                            if (basket.Items.ContainsKey(itemId))
                            {
                                //限购检查
                                quantity += basket.Items[itemId].Quantity;                                
                                
                                var result = ValidateLimitBuy(basket.SellerId,cart.UserId, itemId, quantity);
                                if (result)//没有限购 或者没超出限购数量
                                {
                                    basket.Items[itemId].Quantity = quantity;
                                    return status;
                                }
                                else
                                {
                                    basket.Items[itemId].LimitBuy=result.Data;//限购数量 0为无限购
                                    status.Message = string.Format("限购{0}件，已购买过{1}件 最多还可购买{0}件",result.Data, result.Extra,quantity);
                                    status.Code = 301;
                                    return status;
                                }      
                            }
                            var data = new Cart.Basket.Item { 
                                Id = itemId, 
                                Name = item.Name, 
                                Price = item.Price, 
                                Quantity = 1,
                                Selected=true
                                };

                            status.Code = basket.Items.TryAdd(itemId, data) ? 0 : 300;
                            if (status) return status;
                            status.Message = "添加到购物车失败";
                            return status;
                        }
                    }
                case Operate.Decrease:
                    {
                        if (isSku)
                        {
                            if (basket.Skus.ContainsKey(skuId))
                            {
                                var q = basket.Skus[skuId].Quantity;
                                if (q - quantity > 0)
                                {
                                    basket.Skus[skuId].Quantity = q - quantity;
                                    status.Code = 0;
                                    return status;
                                }
                                Cart.Basket.Sku o;
                                status.Code = basket.Skus.TryRemove(skuId, out o) ? 0 : 500;
                                if (!status)
                                {
                                    status.Message = "从购物车中移除失败";
                                }
                                return status;
                            }
                            status.Code = 100;
                            status.Message = "商品不存在";
                            return status;
                        }
                        if (basket.Items.ContainsKey(itemId))
                        {
                            var q = basket.Items[itemId].Quantity;
                            if (q - quantity > 0)
                            {
                                basket.Items[itemId].Quantity = q - quantity;
                                status.Code = 0;
                                return status;
                            }
                            Cart.Basket.Item o;
                            status.Code = basket.Items.TryRemove(itemId, out o) ? 0 : 500;
                            if (!status)
                            {
                                status.Message = "从购物车中移除失败";
                            }
                            return status;
                        }
                        status.Code = 100;
                        status.Message = "商品不存在";
                        return status;
                    }
                case Operate.Remove:
                    {
                        if (isSku)
                        {
                            if (basket.Skus.ContainsKey(skuId))
                            {
                                Cart.Basket.Sku o;
                                status.Code = basket.Skus.TryRemove(skuId, out o) ? 0 : 500;
                                if (!status)
                                {
                                    status.Message = "从购物车中移除失败";
                                }
                                return status;
                            }
                            status.Code = 0;
                            return status;
                        }
                        if (basket.Items.ContainsKey(itemId))
                        {
                            Cart.Basket.Item o;
                            status.Code = basket.Items.TryRemove(itemId, out o) ? 0 : 500;
                            if (!status)
                            {
                                status.Message = "从购物车中移除失败";
                            }
                            return status;
                        }
                        status.Code = 0;
                        return status;
                    }
            }
            return status;
        }
        
        /// <summary>
        /// 搭配组合套餐操作
        /// </summary>
        /// <param name="operate">购物车</param>
        /// <param name="operate">操作方式</param>        
        /// <param name="packageId">搭配组合套餐Id</param>
        /// <param name="subIds">搭配组合套餐附属商品Ids</param>
        /// <returns></returns>
        private Status PackageOpt(Cart cart, Operate operate,int packageId, int[] subIds = null)
        {
            var status = new Status();

            var package =storage.PromotionPackageGet(packageId, true);
            if (package == null)
            {
                status.Code = 100;
                status.Message = "套餐不存在";
                return status;
            }

            var basket = GetOrCreatBasket(cart,package.SellerId);
            if (basket == null)
            {
                status.Code = 200;
                status.Message = "卖家不存在";
                return status;
            }
            switch (operate)
            {
                case Operate.Increase:
                    {
                        if (basket.Packages.ContainsKey(packageId))
                        {
                            basket.Packages[packageId].Quantity++;
                            return status;
                        }
                        var data = new Cart.Basket.Package()
                        {
                            Id = packageId,
                            Name = package.Name,
                            Fixed = package.Fixed,
                            MainId = package.ItemId,
                            MainPrice=package.ItemPrice,
                            MainPictrue =package.ItemPictrue,
                            Quantity = 1,
                            Selected=true
                        };
                        if (package.Fixed)
                        {
                            foreach (var item in package.Items)
                            {
                                var sub = new Cart.Basket.Package.Sub();
                                sub.Id = item.Id;
                                sub.SkuId = item.SkuId;
                                sub.ItemId = item.ItemId;
                                sub.Price = item.Price;
                                sub.Name = item.Name;
                                sub.Picture = item.Picture;
                                
                                data.Subs.Add(sub);
                            }
                        }
                        else
                        {
                            foreach (var item in package.Items)
                            {
                                if (subIds != null && subIds.Contains(item.Id))
                                {
                                    var sub = new Cart.Basket.Package.Sub();
                                    sub.Id = item.Id;
                                    sub.SkuId = item.SkuId;
                                    sub.ItemId = item.ItemId;
                                    sub.Price = item.Price;
                                    sub.Name = item.Name;
                                    sub.Picture = item.Picture;
                                    
                                    data.Subs.Add(sub);
                                }
                            }
                        }

                        status.Code = basket.Packages.TryAdd(packageId, data) ? 0 : 300;
                        if (status) return status;
                        status.Message = "添加到购物车失败";
                        return status;
                    }
                case Operate.Decrease:
                    {
                        if (basket.Items.ContainsKey(packageId))
                        {
                            var q = basket.Items[packageId].Quantity;
                            if (q - 1 > 0)
                            {
                                basket.Packages[packageId].Quantity = q - 1;
                                status.Code = 0;
                                return status;
                            }
                            Cart.Basket.Package o;
                            status.Code = basket.Packages.TryRemove(packageId, out o) ? 0 : 500;
                            if (!status)
                            {
                                status.Message = "从购物车中移除失败";
                            }
                            return status;
                        }
                        status.Code = 100;
                        status.Message = "商品不存在";
                        return status;
                    }
                case Operate.Remove:
                    {
                        if (basket.Packages.ContainsKey(packageId))
                        {
                            Cart.Basket.Package o;
                            status.Code = basket.Packages.TryRemove(packageId, out o) ? 0 : 500;
                            if (!status)
                            {
                                status.Message = "从购物车中移除失败";
                            }
                            return status;
                        }
                        status.Code = 0;
                        return status;
                    }
            }
            return status;
        }
        
        private Cart GetOrCreatCart(string guid)
        {
            return cache.Put(guid, () => new Cart { Guid = guid }, DateTime.Now.AddHours(2));
        }

        private Cart.Basket GetOrCreatBasket(Cart cart, int sellerId)
        {
            if (cart.Baskets.ContainsKey(sellerId))
            {
                return cart.Baskets[sellerId];
            }
            var user = storage.UserGet(sellerId);
            if (user == null /*|| user.Role != User.Enum.Role.Seller*/) return null;
            var seller = new Cart.Basket { };
            return cart.Baskets.GetOrAdd(sellerId, seller);
        }

        /// <summary>
        /// 限购检查
        /// </summary>
        /// <param name="sellerId">卖家Id</param>
        /// <param name="buyerId">卖家Id</param>
        /// <param name="itemId">ItemId</param>    
        /// <param name="quantity">数量</param>
        /// <returns>返回是否没有超出限购 限购数量 可购数量</returns>
        private Result<int,int> ValidateLimitBuy(int sellerId,int buyerId, int itemId, int quantity)
        {
            var result = new Result<int,int>(true);
            var now = DateTime.Now;
            var date = now.ToString("yyyy-MM-dd");
            var expire = DateTime.Parse(date);
            var key = string.Format("promotion.limitbuy.{0}.{1}", sellerId, date);

            var limits = cache.Put(key, () => storage.PromotionLimitBuyList(sellerId), expire);
            if (limits == null)
            {               
                return result;
            }
            var limit = limits.FirstOrDefault(x => x.ItemId == itemId);
            
            if (limit != null)
            {
                result.Data=limit.Value;//限购数量
                //Todo 验证订单数
                var count = 0;//已购买数量
                result.Extra= count;//已购买数量
                
                if (count + quantity > limit.Value)
                {
                   result.Status=false;
                }                
            }
            return result;
        }
        
        /// <summary>
        /// 处理商品状态
        /// </summary>
        /// <param name="Cart">购物车</param>      
        /// <returns></returns>
        private void ProcessStatus(Cart cart)
        {
            foreach (var basket in cart.Baskets)
            { 
                foreach (var sku in basket.Value.Skus.Values)
                {
                    var x = storage.ProdSkuGet(sku.Id);
                    if (x != null)
                    {
                        if (x.Status == Prod.Status.Online)
                        {
                            if (x.Stock > sku.Quantity)
                            {
                                sku.Status = 0;
                            }
                            else
                            {
                                sku.Status = 1;
                                sku.Selected=false;
                            }
                        }
                    }
                    else
                    {
                        sku.Status = 2;
                        sku.Selected=false;
                    }
                }
                foreach (var item in basket.Value.Items.Values)
                {
                    var x = storage.ProdItemGet(item.Id);
                    if (x != null)
                    {
                        if (x.Status == Prod.Status.Online)
                        {
                            if (x.Stock > item.Quantity)
                            {
                                item.Status = 0;
                            }
                            else
                            {
                                item.Status = 1;
                                item.Selected=false;
                            }
                        }
                    }
                    else
                    {
                        item.Status = 2;
                        item.Selected=false;
                    }
                }
                foreach (var package in basket.Value.Packages.Values)
                {
                    
                }
            }
        }
        
        /// <summary>
        /// 处理促销
        /// </summary>
        /// <param name="cart"></param>
        private void ProcessPromition(Cart cart)
        {
            var now = DateTime.Now;
            var date = now.ToString("yyyy-MM-dd");
            var expire = DateTime.Parse(date);                    
            foreach (var basket in cart.Baskets)
            { 
                
                var key = string.Format("promotion.specialprice.{0}.{1}", basket.Key, date);
                var specialPrices =cache.Put(key,()=>storage.PromotionSpecialPriceList(basket.Key),expire);
                if (specialPrices != null && specialPrices.Any()) 
                {
                    foreach (var specialPrice in specialPrices)
                    {                        
                        foreach (var sku in basket.Value.Skus.Values.Where(x=>x.Selected))
                        {
                            var magic=specialPrice.Contains(sku.ItemId,sku.Id);
                            if(magic.First)
                            {
                                var v=magic.Second;
                                var discount=new Discount(Promotion.Catg.A,specialPrice.Id,specialPrice.Name);
                                switch (specialPrice.CatgId)
                                {
                                    case Promotion.SpecialPriceCatg.A:
                                        var amount = sku.Price - (sku.Price * v);
                                        var description = string.Format("原价{0},打{1}折,优惠{2}", sku.Price, v, amount);
                                        discount.Amount = amount;
                                        discount.Description = description;
                                        sku.Price = sku.Price * v;
                                        break;
                                    case Promotion.SpecialPriceCatg.B:
                                        amount = sku.Price - v;
                                        description = string.Format("原价{0},减{1},优惠{2}",sku.Price, v, amount);
                                        discount.Amount = amount;
                                        discount.Description = description;
                                        sku.Price = sku.Price - v;
                                        break;
                                    case Promotion.SpecialPriceCatg.C:
                                        if(sku.Price>v)
                                        {
                                            amount = sku.Price - v;
                                            description = string.Format("原价{0},促销价{1},优惠{2}", sku.Price, v, amount);
                                            discount.Amount = amount;
                                            discount.Description = description;
                                            sku.Price = v;
                                         }
                                        break;
                                }
                                basket.Value.Discounts.Add(discount);
                            }
                        }
                        
                        foreach (var item in basket.Value.Items.Values.Where(x=>x.Selected))
                        {
                            var magic=specialPrice.Contains(item.Id);
                            if(magic.First)
                            {
                                var v=magic.Second;
                                var discount=new Discount(Promotion.Catg.A,specialPrice.Id,specialPrice.Name);
                                switch (specialPrice.CatgId)
                                {
                                    case Promotion.SpecialPriceCatg.A:
                                        var amount = item.Price - (item.Price * v);
                                        var description = string.Format("原价{0},打{1}折,优惠{2}", item.Price, v, amount);
                                        discount.Amount = amount;
                                        discount.Description = description;
                                        item.Price = item.Price * v;
                                        break;
                                    case Promotion.SpecialPriceCatg.B:
                                        amount = item.Price - v;
                                        description = string.Format("原价{0},减{1},优惠{2}",item.Price, v, amount);
                                        discount.Amount = amount;
                                        discount.Description = description;
                                        item.Price = item.Price - v;
                                        break;
                                    case Promotion.SpecialPriceCatg.C:
                                        if(item.Price>v)
                                        {
                                            amount = item.Price - v;
                                            description = string.Format("原价{0},促销价{1},优惠{2}", item.Price, v, amount);
                                            discount.Amount = amount;
                                            discount.Description = description;
                                            item.Price = v;
                                        }
                                        break;
                                }
                                basket.Value.Discounts.Add(discount);
                            }
                        }
                        
                        foreach(var package in basket.Value.Packages.Values.Where(x=>x.Selected))
                        {
                            
                        }
                        
                        
                    }
                }
                
                key = string.Format("promotion.normal.{0}.{1}", basket.Key, date);  //店铺优惠key               
                var normals =cache.Put(key,()=>storage.PromotionNormalList(basket.Key),expire);
                if (normals != null && normals.Any()) 
                {
                    var tmp = normals.OrderBy(x => x.Global);//同时存在单品活动和店铺活动时 优先处理单品活动
                    var ids = tmp.SelectMany(x => x.Items).ToList();//所有单品活动中的商品
                    foreach (var promotion in normals)
                    {
                        if (promotion.IsValid())
                        {                            
                            var amount = 0m;
                            var quantity = 0;
                            foreach (var sku in basket.Value.Skus.Values.Where(x=>x.Selected))
                            {
                                if (promotion.Global && !ids.Contains(sku.ItemId)) //店铺活动 需要排除已参与了商品活动的商品
                                {
                                    quantity += sku.Quantity;
                                    amount += sku.Price * sku.Quantity;
                                }
                                else if (!promotion.Global && promotion.Items.Contains(sku.ItemId))
                                {
                                    quantity += sku.Quantity;
                                    amount += sku.Price * sku.Quantity;
                                }
                            }
                            
                            foreach (var item in basket.Value.Items.Values.Where(x=>x.Selected))
                            {
                                if (promotion.Global && !ids.Contains(item.Id)) //店铺活动 需要排除已参与了商品活动的商品
                                {
                                    quantity += item.Quantity;
                                    amount += item.Price * item.Quantity;
                                }
                                else if (!promotion.Global && promotion.Items.Contains(item.Id))
                                {
                                    quantity += item.Quantity;
                                    amount += item.Price * item.Quantity;
                                }
                            }
                            
                            foreach (var package in basket.Value.Packages.Values.Where(x=>x.Selected))
                            {

                                if (promotion.Global && !ids.Contains(package.MainId)) //店铺活动 需要排除已参与了商品活动的商品
                                {
                                    quantity += package.Quantity;
                                    amount += package.MainPrice * package.Quantity;
                                }
                                else if (!promotion.Global && promotion.Items.Contains(package.MainId))
                                {
                                    quantity += package.Quantity;
                                    amount += package.MainPrice * package.Quantity;
                                }

                                foreach (var o in package.Subs)
                                {
                                    if (promotion.Global && !ids.Contains(o.ItemId)) //店铺活动 需要排除已参与了商品活动的商品
                                    {
                                        quantity += 1;
                                        amount += o.Price;
                                    }
                                    else if (!promotion.Global && promotion.Items.Contains(o.ItemId))
                                    {
                                        quantity += 1;
                                        amount += o.Price;
                                    }
                                }
                            }
                            
                            var rule = promotion.MatchRule(amount, quantity);
                            if (rule != null)
                            {
                                var discount=new Discount(Promotion.Catg.B,promotion.Id,promotion.Name);
                                if (promotion.Discount)
                                {
                                    discount.Amount = amount * rule.Value;
                                    var description=string.Format("满{0}件,打{1}折,优惠{2}",rule.Upon, rule.Value, discount.Amount);
                                    discount.Description = description;                                    
                                }
                                else
                                {
                                    discount.Amount = amount - rule.Value;
                                    var description=string.Format("满{0}元,减{1}元,优惠{2}",rule.Upon, rule.Value, discount.Amount);
                                    discount.Description = description;                                    
                                }
                                basket.Value.Discounts.Add(discount);
                                
                                if (rule.Gifts != null)
                                {
                                    foreach (var gift in rule.Gifts)
                                    {
                                        var o = new Gift();
                                        o.SkuId = gift.SkuId;
                                        o.ItemId = gift.ItemId;
                                        o.Name = gift.Name;
                                        o.Quantity = gift.Quantity;
                                        o.PropText = gift.PropText;

                                        basket.Value.Gifts.Add(o);
                                    }
                                }

                                if (rule.Coupons != null)
                                {
                                    foreach (var coupon in rule.Coupons)
                                    {
                                        var o = new Coupon();

                                        o.Global = true;
                                        o.Value = coupon.Value;
                                        o.Count = coupon.Count;
                                        o.Plateform = coupon.Plateform;
                                        o.StartedOn = coupon.StartedOn;
                                        o.StoppedOn = coupon.StoppedOn;
                                        o.Limit = coupon.Limit;
                                        o.LimitVal = coupon.LimitVal;

                                        basket.Value.Coupons.Add(o);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

    }

    /// <summary>
    /// 购物车 缓存使用
    /// </summary>
    [ProtoContract]
    public class Cart
    {
        /// <summary>
        /// Guid
        /// </summary>
        [ProtoMember(1)]
        public string Guid { get; set; }
        
        /// <summary>
        /// 买家Id
        /// </summary>
        [ProtoMember(2)]
        public int UserId { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        [ProtoMember(3)]
        public Address Address { get; set; }

        /// <summary>
        /// Baskets
        /// </summary>
        [ProtoMember(4)]
        public ConcurrentDictionary<int, Basket> Baskets { get; set; }


        #region
        [ProtoContract]
        public class Basket
        {
            /// <summary>
            /// 卖家Id
            /// </summary>
            [ProtoMember(1)]
            public int SellerId { get; set; }

            /// <summary>
            /// 卖家名称
            /// </summary>
            [ProtoMember(2)]
            public string SellerName { get; set; }

            /// <summary>
            /// 卖家店铺名称
            /// </summary>
            [ProtoMember(3)]
            public string ShopName { get; set; }

            [ProtoMember(4)]
            public ConcurrentDictionary<int, Sku> Skus { get; set; }

            [ProtoMember(5)]
            public ConcurrentDictionary<int, Item> Items { get; set; }

            [ProtoMember(6)]
            public ConcurrentDictionary<int, Package> Packages { get; set; }

            /// <summary>
            /// 是否选中
            /// </summary>
            public bool Selected { 
                get{            
                    return Skus.Values.All(x=>x.Selected) 
                    && Items.Values.All(x=>x.Selected) 
                    && Packages.Values.All(x=>x.Selected);
                }             
            }

            /// <summary>
            /// 赠品
            /// </summary>
            public IList<Gift> Gifts { get;  set; }

            ///// <summary>
            ///// 使用的优惠券
            ///// </summary>
            //public IList<Coupon> CouponUsed { get; private set; }

            /// <summary>
            /// 赠送的优惠券
            /// </summary>
            public IList<Coupon> Coupons { get;  set; }

            /// <summary>
            /// 优惠信息
            /// </summary>
            public List<Discount> Discounts { get;  set; }

            /// <summary>
            /// 运费
            /// </summary>
            public decimal Freight { get;  set; }

            /// <summary>
            /// 商品总件数
            /// </summary>
            public int ProdQuantity { get;  set; }

            /// <summary>
            /// 商品总金额
            /// </summary>
            public decimal ProdAmount { get;  set; }

            /// <summary>
            /// 优惠总金额
            /// </summary>
            public decimal DiscountAmount { get { return Discounts.Sum(x => x.Amount); } }

            /// <summary>
            /// 订单总金额(商品总金额+运费-优惠总金额)
            /// </summary>
            public decimal TotalAmount
            {
                get
                {
                    return ProdAmount + Freight - DiscountAmount;
                }
            }
            
            /// <summary>
            /// Sku
            /// </summary>
            public class Sku
            {
                /// <summary>
                /// Id
                /// </summary>
                public int Id { get; set; }

                /// <summary>
                /// 商品Id
                /// </summary>
                public int ItemId { get; set; }

                /// <summary>
                /// 名称
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 数量
                /// </summary>
                public int Quantity { get; set; }

                /// <summary>
                /// 单价
                /// </summary>
                public decimal Price { get; set; }

                /// <summary>
                /// 限购数量 0为不限 >0为限购数量
                /// </summary>
                public int LimitBuy { get; set; }

                /// <summary>
                /// 是否选中
                /// </summary>
                public bool Selected { get; set; }

                /// <summary>
                /// 状态 0为正常 1为售完 2为下架
                /// </summary>
                public sbyte Status { get; set; }

            }
           
            /// <summary>
            /// Item
            /// </summary>
            public class Item
            {
                /// <summary>
                /// Id
                /// </summary>
                public int Id { get; set; }

                /// <summary>
                /// 名称
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 数量
                /// </summary>
                public int Quantity { get; set; }

                /// <summary>
                /// 单价
                /// </summary>
                public decimal Price { get; set; }

                /// <summary>
                /// 限购数量 0为不限 >0为限购数量
                /// </summary>
                public int LimitBuy { get; set; }

                /// <summary>
                /// 是否选中
                /// </summary>
                public bool Selected { get; set; }

                /// <summary>
                /// 状态 0为正常 1为售完 2为下架
                /// </summary>
                public sbyte Status { get; set; }

            }           
            
        
            /// <summary>
            /// 搭配组合套餐
            /// </summary>
            public class Package
            {
                /// <summary>
                /// Id
                /// </summary>
                public int Id { get; set; }

                /// <summary>
                /// 名称
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 数量
                /// </summary>
                public int Quantity { get; set; }

                /// <summary>
                /// 固定组合套餐 商品打包成套餐销售，消费者打包购买
                /// 自选商品套餐 套餐中的附属商品，消费者可以通过复选框的方式，有选择的购买
                /// </summary>
                public bool Fixed { get; set; }

                /// <summary>
                /// 主商品Id
                /// </summary>
                public int MainId { get; set; }
                

                /// <summary>
                /// 主商品价格
                /// </summary>
                public decimal MainPrice { get; set; }
                
                /// <summary>
                /// 主商品搭配图
                /// </summary>
                public string MainPictrue { get; set; }

                /// <summary>
                /// 搭配组合套餐附属商品
                /// </summary>
                public IList<Sub> Subs { get; set; }

                /// <summary>
                /// 是否选中
                /// </summary>
                public bool Selected { get; set; }

                /// <summary>
                /// 状态 0为正常 1为售完 2为下架
                /// </summary>
                public sbyte Status { get; set; }

                public Package()
                {
                    Subs = new List<Sub>(8);
                }

                /// <summary>
                /// 搭配组合套餐附属商品
                /// </summary>
                public class Sub
                {
                    public int Id { get; set; }
                    
                    /// <summary>
                    /// SkuId
                    /// </summary>
                    public int SkuId { get; set; }

                    /// <summary>
                    /// ItemId
                    /// </summary>
                    public int ItemId { get; set; }
                    
                    /// <summary>
                    /// 附属商品名称
                    /// </summary>
                    public string Name { get; set; }

                    /// <summary>
                    /// 附属商品价格 0为默认如果不设置搭配价，则执行在售价
                    /// </summary>
                    public decimal Price { get; set; }

                    /// <summary>
                    /// 附属商品图片
                    /// </summary>                
                    public string Picture { get; set; }
                    
                    /// <summary>
                    /// 状态 0为正常 1为售完 2为下架
                    /// </summary>
                    public sbyte Status { get; set; }                    

                    /// <summary>
                    /// 是否为Sku
                    /// </summary>
                    public bool IsSku { get { return SkuId > 0 && ItemId > 0; } }
                }

            }

        }
        #endregion       
          
    }

    #region 赠品
    /// <summary>
    /// 赠品
    /// </summary>
    public class Gift
    {
        /// <summary>
        /// SkuId
        /// </summary>
        public int SkuId { get; set; }

        /// <summary>
        /// 商品Id
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 销售属性
        /// </summary>
        public string PropText { get; set; }

    }
    #endregion

    #region 店铺优惠券
    /// <summary>
    /// 店铺优惠券 http://bbs.taobao.com/catalog/thread/16543510-264269834.htm
    /// </summary>
    public class Coupon
    {
        public bool Global{get;set;}
        
        /// <summary>
        /// 面额 3 5 10 20 50 100 200
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 适用范围 pc wap app 通用
        /// </summary>
        public Plateform Plateform { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime StartedOn { get; set; }

        /// <summary>
        /// 失效时间 失效时间不应早于生效时间及活动结束时间
        /// </summary>
        public DateTime StoppedOn { get; set; }

        /// <summary>
        /// 使用条件 不限/订单满x元
        /// </summary>
        public bool Limit { get; set; }

        /// <summary>
        /// 使用条件值 订单满x元
        /// </summary>
        public decimal LimitVal { get; set; }

    }
    #endregion
    
    #region 折扣
    /// <summary>
    /// 折扣
    /// </summary>
    public class Discount
    {
        /// <summary>
        /// 促销Id
        /// </summary>
        public int PromotionId { get; set; }

        /// <summary>
        /// 促销名称
        /// </summary>
        public string PromotionName { get; set; }

        /// <summary>
        /// 促销类型
        /// </summary>
        public Promotion.Catg PromotionCatg { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        public Discount(Promotion.Catg catg, int id, string name)
        {
            PromotionId = id;
            PromotionName = name;
            PromotionCatg = catg;
        }
    }

    #endregion
    #region 收货地址
    public class Address
    {
        /// <summary>
        /// 用户地址Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 区域Id
        /// </summary>
        public string AreaId { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// 固定电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 移动电话
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        public Address(int id)
        {
            Id = id;
        }
    }
    #endregion

    #region 操作
    /// <summary>
    /// 购物车操作
    /// </summary>
    public enum Operate : sbyte
    {
        /// <summary>
        /// 增加
        /// </summary>
        Increase,

        /// <summary>
        /// 减少
        /// </summary>
        Decrease,

        /// <summary>
        /// 移除
        /// </summary>
        Remove
    }
    #endregion
}