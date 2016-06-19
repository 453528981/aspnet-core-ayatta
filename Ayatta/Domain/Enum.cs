using System;

namespace Ayatta.Domain
{

    #region 平台
    /// <summary>
    /// 平台
    /// </summary>
    [Flags]
    public enum Plateform : byte
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Pc
        /// </summary>
        Pc = 1,
        /// <summary>
        /// Wap
        /// </summary>
        Wap = 2,
        /// <summary>
        /// App
        /// </summary>
        App = 4
    }
    #endregion

    #region 付款方式
    /// <summary>
    /// 付款方式
    /// </summary>
    [Flags]
    public enum PayMethod : byte
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// 在线支付
        /// </summary>
        Online = 1,
        /// <summary>
        /// 货到付款
        /// </summary>
        Cod = 2
    }
    #endregion
}
