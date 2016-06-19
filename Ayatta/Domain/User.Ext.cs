using System;
using System.Collections.Generic;

namespace Ayatta.Domain
{
    public partial class User
    {
        public static class Ext
        {
            /// <summary>
            /// Profile
            /// </summary>
            public class Profile : IEntity<int>
            {
                public int Id { get; set; }

                /// <summary>
                /// 身份证号码
                /// </summary>
                public string Code { get; set; }

                /// <summary>
                /// 真实姓名
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 性别
                /// </summary>
                public Enum.Gender Gender { get; set; }

                /// <summary>
                /// 婚姻状态
                /// </summary>
                public Enum.Marital Marital { get; set; }

                /// <summary>
                /// 出生日期
                /// </summary>
                public DateTime? Birthday { get; set; }

                /// <summary>
                /// 固定电话
                /// </summary>
                public string Phone { get; set; }

                /// <summary>
                /// 移动电话
                /// </summary>
                public string Mobile { get; set; }

                /// <summary>
                /// 所属省市区
                /// </summary>
                public string AreaId { get; set; }

                /// <summary>
                /// 详细住址
                /// </summary>
                public string Address { get; set; }

                /// <summary>
                /// 注册Ip
                /// </summary>
                public string SignUpIp { get; set; }

                /// <summary>
                /// 注册方式 0为通过用户名注册，1为通过邮箱注册，2为通过手机号码注册，3为通过手机短信注册
                /// </summary>
                public byte SignUpBy { get; set; }

                /// <summary>
                /// 注册跟踪码
                /// </summary>
                public string TraceCode { get; set; }

                /// <summary>
                /// 最后一次登录Ip
                /// </summary>
                public string LastSignInIp { get; set; }

                /// <summary>
                /// 最后一次登录时间
                /// </summary>
                public DateTime LastSignInOn { get; set; }

                /// <summary>
                /// 性别字典
                /// </summary>
                public static IDictionary<Enum.Gender, string> UserGenderDic
                {
                    get
                    {
                        var dic = new Dictionary<Enum.Gender, string>();
                        dic.Add(Enum.Gender.Secrect, "保密");
                        dic.Add(Enum.Gender.Male, "男");
                        dic.Add(Enum.Gender.Female, "女");
                        return dic;
                    }
                }

                /// <summary>
                /// 婚姻状态字典
                /// </summary>
                public static IDictionary<Enum.Marital, string> MaritalStatusDic
                {
                    get
                    {
                        var dic = new Dictionary<Enum.Marital, string>();
                        dic.Add(Enum.Marital.Secrect, "保密");
                        dic.Add(Enum.Marital.Single, "未婚");
                        dic.Add(Enum.Marital.Married, "已婚");
                        return dic;
                    }
                }
            }
        }

    }
}