
using Dapper;
using ProtoBuf;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
namespace Ayatta.LocalCache
{
    public interface IProdLocalCache
    {
        Data.Catg GetCatg(int id);
        IList<Data.Catg> GetCatgs(int parentId = 0);
        Data.Prop GetProp(int id);
        IList<Data.Prop> GetProps(int catgId);
        Data.PropValue GetPropValue(int id);
        IList<Data.PropValue> GetPropValues(int catgId);

    }
    public sealed class ProdLocalCache : BaseLocalCache<Data>, IProdLocalCache
    {
        public ProdLocalCache(IOptions<LocalCacheOptions> optionsAccessor) : base(optionsAccessor)
        {
        }

        protected override Data Read(System.DateTime? time)
        {
            var data = new Data();
            var sb = new System.Text.StringBuilder();

            sb.Append("select * from ProdCatg where 1=1");

            if (time.HasValue)
            {
                sb.Append(" and ModifiedOn>='" + time.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'");
            }
            sb.Append(";");

            sb.Append("select * from ProdProp where 1=1");

            if (time.HasValue)
            {
                sb.Append(" and ModifiedOn>='" + time.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'");
            }
            sb.Append(";");

            sb.Append("select * from ProdPropValue where 1=1");

            if (time.HasValue)
            {
                sb.Append(" and ModifiedOn>='" + time.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'");
            }
            sb.Append(";");

            var sql = sb.ToString();
            //sql.Dump();
            //var conn=new MySql.Data.MySqlClient.MySqlConnection(options.ConnectionString);
            //data.Catgs=conn.Query<Data.Catg>(sql).ToList();

            using (var conn = new MySqlConnection(options.ConnectionString))
            using (var multi = conn.QueryMultiple(sql))
            {
                data.Catgs = multi.Read<Data.Catg>().ToList();
                data.Props = multi.Read<Data.Prop>().ToList();
                data.PropValues = multi.Read<Data.PropValue>().ToList();
            }
            return data;
        }

        protected override void ReadCallback(Data data)
        {
            if (data.Catgs != null)
            {
                foreach (var o in data.Catgs)
                {
                    if (!Data.Catgs.Contains(o))
                    {
                        Data.Catgs.Add(o);
                    }
                }
            }
            if (data.Props != null)
            {
                foreach (var o in data.Props)
                {
                    if (!Data.Props.Contains(o))
                    {
                        Data.Props.Add(o);
                    }
                }
            }
            if (data.PropValues != null)
            {
                foreach (var o in data.PropValues)
                {
                    if (!Data.PropValues.Contains(o))
                    {
                        Data.PropValues.Add(o);
                    }
                }
            }
        }

        public Data.Catg GetCatg(int id)
        {
            return Data.Catgs.FirstOrDefault(x => x.Id == id);
        }

        public IList<Data.Catg> GetCatgs(int parentId = 0)
        {
            return Data.Catgs.Where(x => x.ParentId == parentId).ToList();
        }

        public Data.Prop GetProp(int id)
        {
            return Data.Props.FirstOrDefault(x => x.Id == id);
        }
        
        public IList<Data.Prop> GetProps(int catgId)
        {
            return Data.Props.Where(x => x.CatgId == catgId).ToList();
        }
        
        public Data.PropValue GetPropValue(int id)
        {
            return Data.PropValues.FirstOrDefault(x => x.Id == id);
        }
        
        public IList<Data.PropValue> GetPropValues(int catgId)
        {
            return Data.PropValues.Where(x => x.CatgId == catgId).ToList();
        }
    }


    [ProtoContract]
    public class Data
    {
        [ProtoMember(1)]
        public IList<Item> Items { get; set; }
        [ProtoMember(2)]
        public IList<Catg> Catgs { get; set; }
        [ProtoMember(3)]
        public IList<Prop> Props { get; set; }
        [ProtoMember(4)]
        public IList<PropValue> PropValues { get; set; }

        public Data()
        {
            Catgs = new List<Catg>(257);
            Props = new List<Prop>(409877 + 877);
            PropValues = new List<PropValue>(1823192 + 3192);
        }

        [ProtoContract]
        public class Item
        {
            [ProtoMember(1)]
            public int Id { get; set; }
            [ProtoMember(2)]
            public string Code { get; set; }
            [ProtoMember(3)]
            public string Name { get; set; }

            public override bool Equals(object o)
            {
                var x = o as Item;
                if (x != null)
                {
                    return (x.Id == Id);
                }
                return false;
            }

            public override int GetHashCode()
            {
                return Id;
            }
        }
        [ProtoContract]
        public class Catg
        {
            [ProtoMember(1)]
            public int Id { get; set; }
            [ProtoMember(2)]
            public int ParentId { get; set; }
            [ProtoMember(3)]
            public bool IsParent { get; set; }
            [ProtoMember(4)]
            public int Priority { get; set; }
            [ProtoMember(5)]
            public string Name { get; set; }
            [ProtoMember(6)]
            public byte Status { get; set; }

            public override bool Equals(object o)
            {
                var x = o as Catg;
                if (x != null)
                {
                    return (x.Id == Id);
                }
                return false;
            }

            public override int GetHashCode()
            {
                return Id;
            }
        }

        [ProtoContract]
        public class Prop
        {
            [ProtoMember(1)]
            public int Id { get; set; }
            [ProtoMember(2)]
            public int CatgId { get; set; }
            [ProtoMember(3)]
            public int ParentPid { get; set; }
            [ProtoMember(4)]
            public int ParentVid { get; set; }
            [ProtoMember(5)]
            public string Name { get; set; }
            [ProtoMember(6)]
            public bool Must { get; set; }
            [ProtoMember(7)]
            public bool Multi { get; set; }
            [ProtoMember(8)]
            public bool IsKeyProp { get; set; }
            [ProtoMember(9)]
            public bool IsSaleProp { get; set; }
            [ProtoMember(10)]
            public bool IsEnumProp { get; set; }
            [ProtoMember(11)]
            public bool IsItemProp { get; set; }
            [ProtoMember(12)]
            public bool IsColorProp { get; set; }
            [ProtoMember(13)]
            public bool IsInputProp { get; set; }
            [ProtoMember(14)]
            public bool AllowAlias { get; set; }
            [ProtoMember(15)]
            public string Priority { get; set; }
            [ProtoMember(16)]
            public byte Status { get; set; }

            public override bool Equals(object o)
            {
                var x = o as Prop;
                if (x != null)
                {
                    return (x.Id == Id && x.CatgId == CatgId);
                }
                return false;
            }

            public override int GetHashCode()
            {
                return Id + CatgId;
            }
        }
        [ProtoContract]
        public class PropValue
        {
            [ProtoMember(1)]
            public int Id { get; set; }
            [ProtoMember(2)]
            public int CatgId { get; set; }
            [ProtoMember(3)]
            public int PropId { get; set; }
            [ProtoMember(4)]
            public string Name { get; set; }
            [ProtoMember(5)]
            public string Alias { get; set; }
            [ProtoMember(6)]
            public string Priority { get; set; }
            [ProtoMember(7)]
            public byte Status { get; set; }

            public override bool Equals(object o)
            {
                var x = o as PropValue;
                if (x != null)
                {
                    return (x.Id == Id && x.CatgId == CatgId && x.PropId == PropId);
                }
                return false;
            }

            public override int GetHashCode()
            {
                return Id + CatgId + PropId;
            }
        }
    }
}




