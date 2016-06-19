using System;
using System.Collections.Generic;

namespace Ayatta.Domain
{
    /*
    public class Order: BaseEntity<string>
    {
        #region Properties

        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderCategory Category { get; set; }

        /// <summary>
        /// 订单商品总件数
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 商品总金额
        /// </summary>
        public decimal SubTotal { get; set; }

        /// <summary>
        /// 邮费
        /// </summary>
        public decimal Postage { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// 待付总金额
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// 实付金额
        /// </summary>
        public decimal Paid { get; set; }

        /// <summary>
        /// 使用积分
        /// </summary>
        public int IntegralUse { get; set; }

        /// <summary>
        /// 买家实际使用积分（扣除部分退款使用的积分）
        /// </summary>
        public int IntegralRealUse { get; set; }

        /// <summary>
        /// 赠送积分
        /// </summary>
        public int IntegralReward { get; set; }

        /// <summary>
        /// 总重量
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// 优惠券
        /// </summary>
        public string Coupon { get; set; }

        /// <summary>
        /// 优惠券抵消金额
        /// </summary>
        public decimal CouponUse { get; set; }

        /// <summary>
        /// 礼品卡
        /// </summary>
        public string GiftCard { get; set; }

        /// <summary>
        /// 礼品卡抵消金额
        /// </summary>
        public decimal GiftCardUse { get; set; }

        /// <summary>
        /// 电子凭证的垂直信息
        /// </summary>
        public string ETicket { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public OrderPayment Payment { get; set; }

        /// <summary>
        /// 物流方式
        /// </summary>
        public OrderShipment Shipment { get; set; }

        /// <summary>
        /// 是否是多次发货的订单
        /// </summary>
        public bool MultipleConsign { get; set; }

        /// <summary>
        /// 付款时间 格式:yyyy-MM-dd HH:mm:ss
        /// </summary>
        public DateTime? PaidOn { get; set; }

        /// <summary>
        /// 发货时间 格式:yyyy-MM-dd HH:mm:ss
        /// </summary>
        public DateTime? ConsignOn { get; set; }

        /// <summary>
        /// 结束时间 交易成功时间(更新交易状态为成功的同时更新)/确认收货时间或者交易关闭时间 。格式:yyyy-MM-dd HH:mm:ss
        /// </summary>
        public DateTime? FinishedOn { get; set; }

        /// <summary>
        /// 超时到期时间 格式:yyyy-MM-dd HH:mm:ss
        /// </summary>
        public DateTime ExpirationOn { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// 收货人固定电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 收货人移动电话
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 收货人所在省市区Id
        /// </summary>
        public string AreaId { get; set; }

        /// <summary>
        /// 收货人所在省
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 收货人所在市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 收货人所在区
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// 收货人详细地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// 是否开发票
        /// </summary>
        public bool HasInvoice { get; set; }

        /// <summary>
        /// 发票类型
        /// </summary>
        public string InvoiceCategory { get; set; }

        /// <summary>
        /// 发票抬头
        /// </summary>
        public string InvoiceTitle { get; set; }

        /// <summary>
        /// 发票内容
        /// </summary>
        public string InvoiceContent { get; set; }

        /// <summary>
        /// 买家Id
        /// </summary>
        public int BuyerId { get; set; }

        /// <summary>
        /// 买家用户名
        /// </summary>
        public string BuyerName { get; set; }

        /// <summary>
        /// 买家备注旗帜（只有买家才能查看该字段）
        /// </summary>
        public int BuyerFlag { get; set; }

        /// <summary>
        /// 买家备注
        /// </summary>
        public string BuyerMemo { get; set; }

        /// <summary>
        /// 买家留言
        /// </summary>
        public string BuyerMessage { get; set; }

        /// <summary>
        /// 买家是否已评价 可选值:true(已评价),false(未评价) 如买家只评价未打分，此字段仍返回false
        /// </summary>
        public bool BuyerRate { get; set; }

        /// <summary>
        /// 卖家Id
        /// </summary>
        public int SellerId { get; set; }

        /// <summary>
        /// 卖家昵称
        /// </summary>
        public string SellerName { get; set; }

        /// <summary>
        /// 卖家备注Flag
        /// </summary>
        public int SellerFlag { get; set; }

        /// <summary>
        /// 卖家备注
        /// </summary>
        public string SellerMemo { get; set; }

        /// <summary>
        /// 卖家是否已评价
        /// </summary>
        public bool SellerRate { get; set; }

        /// <summary>
        /// 订单取消类型 0为系统取消 1为买家取消 2为卖家取消
        /// </summary>
        public byte? Cancel { get; set; }

        /// <summary>
        /// 订单取消原因
        /// </summary>
        public string CancelReason { get; set; }

        /// <summary>
        /// 订单兑换
        /// 用户成功竞购获得的拍品可兑换为积分或免费拍币
        /// 用户补差购买时可选择兑换为积分或免费拍币
        /// </summary>
        public OrderExchange Exchange { get; set; }

        /// <summary>
        /// 关联Id
        /// </summary>
        public string AssociateId { get; set; }

        /// <summary>
        /// 扩展信息
        /// </summary>
        public string Extra { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus Status { get; set; }

        /// <summary>
        /// 下单Ip
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 订单跟踪码
        /// </summary>
        public string TraceCode { get; set; }

        /// <summary>
        /// 最后一次编辑时间
        /// </summary>
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// 未支付金额（待支付）
        /// </summary>
        public decimal Unpaid { get { return Total - Paid; } }

        public static IDictionary<OrderCategory, string> CategoryDic
        {
            get
            {
                var dic = new Dictionary<OrderCategory, string>();
                dic.Add(OrderCategory.Normal, "网购订单");
                dic.Add(OrderCategory.Auction, "竞购订单");
                dic.Add(OrderCategory.AuctionVariant, "竞购补差订单");
                dic.Add(OrderCategory.IntegralExchange, "积分兑换订单");
                return dic;
            }
        }

        #endregion

        public string CancelText
        {
            get
            {
                var s = string.Empty;
                if (Cancel.HasValue)
                {
                    switch (Cancel)
                    {
                        case 0:
                            s = "系统"; break;
                        case 1:
                            s = "买家"; break;
                        case 2:
                            s = "卖家"; break;

                    }
                }
                return s;
            }
        }

        #region Navigation Properties

        /// <summary>
        /// 订单扩展信息
        /// </summary>
        //public virtual OrderExtra OrderExtra { get; set; }

        /// <summary>
        /// 子订单
        /// </summary>
        public virtual IList<OrderItem> OrderItems { get; set; }

        /// <summary>
        /// 订单备注
        /// </summary>
        public virtual IList<OrderNote> OrderNotes { get; set; }

        /// <summary>
        /// 订单使用的折扣
        /// </summary>
        public virtual IList<DiscountUsageHistory> DiscountUsageHistory { get; set; }

        ///// <summary>
        ///// 订单使用的礼品卡
        ///// </summary>
        //public virtual IList<GiftCardUsageHistory> GiftCardUsageHistory { get; set; }

        #endregion

        public decimal GetRefundAmount(string id)
        {
            var items = OrderItems;
            if (items != null)
            {
                if (items.Count == 1)
                {
                    var item = items.FirstOrDefault(o => o.Id == id);
                    if (item != null)
                    {
                        return item.Total - (Discount * item.Total / item.Total) + Postage;
                    }
                }
                else
                {
                    var item = items.FirstOrDefault(o => o.Id == id);
                    if (item != null)
                    {
                        var isLast = items.Count(o => o.Id != id && !string.IsNullOrEmpty(o.RefundId) && o.RefundStatus.HasValue) == items.Count - 1;
                        if (isLast)
                        {
                            return item.Total - (Discount * item.Total / items.Sum(o => o.Total)) + Postage;
                        }
                        return item.Total - (Discount * item.Total / items.Sum(o => o.Total));
                    }
                }
            }
            return 0;
        }
}
    */
}