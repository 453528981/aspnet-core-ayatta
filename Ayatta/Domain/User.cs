using System;

namespace Ayatta.Domain
{
    /// <summary>
    /// User
    /// </summary>
    public partial class User : BaseEntity<int>
    {
        #region
        /// <summary>
        /// Guid
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 支付密码
        /// </summary>
        public string PayPassword { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public Enum.Role Role { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public Enum.Level Level { get; set; }

        /// <summary>
        /// 限制
        /// </summary>
        public Enum.Limit Limit { get; set; }

        /// <summary>
        /// 许可
        /// </summary>
        public Enum.Permit Permit { get; set; }

        /// <summary>
        /// 绑定邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 绑定手机
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Avatar
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public Enum.Status Status { get; set; }

        /// <summary>
        /// 是否通过真实身份验证
        /// </summary>
        public bool Authenticated { get; set; }

        /// <summary>
        /// 真实身份认证时间
        /// </summary>
        public DateTime? AuthenticatedOn { get; set; }

        /// <summary>
        /// User.Profile
        /// </summary>
        public virtual Ext.Profile Profile { get; set; }

        /*
        /// <summary>
        /// UserExtra
        /// </summary>
        public virtual UserExtra Extra { get; set; }
        */
        #endregion


    }
}