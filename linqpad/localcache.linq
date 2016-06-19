<Query Kind="Program">
  <Connection>
    <ID>94112c10-a546-4880-9fbb-9d4a73a597b7</ID>
    <Persist>true</Persist>
    <Driver Assembly="IQDriver" PublicKeyToken="5b59726538a49684">IQDriver.IQDriver</Driver>
    <Provider>Devart.Data.MySql</Provider>
    <CustomCxString>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAasoE5ACSEE6Rts5OBZSDZQAAAAACAAAAAAAQZgAAAAEAACAAAADp043c5/6hXhTB5sg591gMRNC9LPId73TGD8RGAw0rQQAAAAAOgAAAAAIAACAAAAC3RrSmfy0hkjvBLKvGqM0hhMvI0yM/c3UyXf0ggwaSBEAAAADiNmEc8NWbjR6adennRsRr7pyBASahtlOGgKfajVe2mMIyoF1Ww8zSN4353YJckicKFuh5E0MvCVQ5TON7CHrKQAAAACyF6wdM6FRiD1DxpqJ3uPBtfwu6zvANyy8NRdPoFwckHHdTlG/aTHbaFfEOIQSkx/mdPpV6SIE1k/M1+NoEpXM=</CustomCxString>
    <Server>127.0.0.1</Server>
    <Database>base</Database>
    <UserName>root</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAasoE5ACSEE6Rts5OBZSDZQAAAAACAAAAAAAQZgAAAAEAACAAAADj5T9VPa92LlIqxCh4ngiOlxW/wscaWH4Qk52+3V0USQAAAAAOgAAAAAIAACAAAAB84pU06jhv1BYE0513cVL1AjmnkEd5G93FW7b0XDrfRhAAAABeT7ZPNz3A7dbSfINpqqg0QAAAAA5ZaISqOn8cV18xZDYfPe+Aux3wDe0YiApwNHWnR2r7PoWKyDLb9nnWsrjgxDbj/PFMV4t1GFLSmH++6DiMeuQ=</Password>
    <NoCapitalization>true</NoCapitalization>
    <NoPluralization>true</NoPluralization>
    <EncryptCustomCxString>true</EncryptCustomCxString>
    <DriverData>
      <StripUnderscores>false</StripUnderscores>
      <QuietenAllCaps>false</QuietenAllCaps>
      <ExtraCxOptions>charset=utf8</ExtraCxOptions>
    </DriverData>
  </Connection>
  <NuGetReference>Dapper</NuGetReference>
  <NuGetReference>MySql.Data</NuGetReference>
  <NuGetReference>protobuf-net</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>MySql.Data.MySqlClient</Namespace>
  <Namespace>ProtoBuf</Namespace>
  <Namespace>ProtoBuf.Meta</Namespace>
  <Namespace>ProtoBuf.ServiceModel</Namespace>
</Query>

void Main()
{
	
	var lc=new LocalCache(null);
	//lc.Data.Dump();
	
	//this.Connection.ConnectionString.Dump();
}

public  class LocalCache: IDisposable
{ 
	public Data Data{get;private set;}
	public DateTime NextSyncTime{get;private set;}
	public DateTime NextSaveTime{get;private set;}
	private readonly bool saving;
	private  readonly Timer timer;
	private readonly LocalCacheOptions options;
	public LocalCache(LocalCacheOptions options)
	{
		if(options==null)
		{
			options=new LocalCacheOptions();
			options.SyncEnable=true;
			options.CacheFile="d:/cache/dat.bin";
			options.ConnectionString="Server=127.0.0.1;Database=base;Uid=root;Pwd=root;charset=utf8";
			options.SyncInterval=5;
			options.SaveInterval=20;
		}
		Data=new Data();
		this.options=options;		
		DateTime? time=null;
		var ex=false;
		if(File.Exists(options.CacheFile))
		{		
			ex=true;
			using(var file = System.IO.File.OpenRead(options.CacheFile))
			{
				Data = Serializer.Deserialize<Data>(file);					
			}
			time=File.GetLastWriteTime(options.CacheFile);
		}
		Sync(time);
		if(ex){
			Save();
		}
		
		if(options.SyncEnable){
			timer=new Timer(Callback,null,options.SyncInterval,1000);
		}
	}
	
	private  void Callback(object state){
		if(options.SyncEnable&&timer!=null)
		{
			var now=DateTime.Now;			
			var syncTime=NextSyncTime;
			var saveTime=NextSaveTime;
			if(syncTime!=null)
			{
				if(now.ToString("yyyy-MM-dd HH:mm:ss")==syncTime.ToString("yyyy-MM-dd HH:mm:ss"))
				{				
					Sync(now);					
				}
			}
			if(saveTime!=null)
			{
				if(now.ToString("yyyy-MM-dd HH:mm:ss")==saveTime.ToString("yyyy-MM-dd HH:mm:ss"))
				{					
					Save();					
				}
			}
		}	
	}
	
	private void Sync(DateTime? time)
	{
		if(saving)
		{
			return;
		}
		var data=Read(time);
		if(!time.HasValue)
		{
			Data=data;
			NextSyncTime=DateTime.Now.AddSeconds(options.SyncInterval);	
			return;
		}
		if(data.Catgs!=null)
		{
			foreach(var o in data.Catgs){
				if(!Data.Catgs.Contains(o)){
					Data.Catgs.Add(o);
				}				
			}
		}
		if(data.Props!=null)
		{
			foreach(var o in data.Props){
				if(!Data.Props.Contains(o)){
					Data.Props.Add(o);
				}				
			}
		}
		if(data.PropValues!=null)
		{
			foreach(var o in data.PropValues){
				if(!Data.PropValues.Contains(o)){
					Data.PropValues.Add(o);
				}				
			}
		}
		NextSyncTime=DateTime.Now.AddSeconds(options.SyncInterval);	
	}
	
	private void Save()
	{
		if(saving)
		{
			return;
		}
		saving=true;
		using(var file = File.Create(options.CacheFile, 1024000, FileOptions.Asynchronous))
		{			
			Serializer.Serialize(file, Data);			
		}
		saving=false;
	   NextSaveTime=DateTime.Now.AddSeconds(options.SaveInterval);
	}
	
	private Data Read(DateTime? time)	
	{
		var data=new Data();
		var sb=new StringBuilder();
		 
		sb.Append("select * from ProdCatg where 1=1");
		
		if(time.HasValue)
		{
			sb.Append(" and ModifiedOn>='"+time.Value.ToString("yyyy-MM-dd HH:mm:ss")+"'");
		}
		sb.Append(";");
		
		sb.Append("select * from ProdProp where 1=1");
		
		if(time.HasValue)
		{
			sb.Append(" and ModifiedOn>='"+time.Value.ToString("yyyy-MM-dd HH:mm:ss")+"'");
		}
		sb.Append(";");
		
		sb.Append("select * from ProdPropValue where 1=1");
		
		if(time.HasValue)
		{
			sb.Append(" and ModifiedOn>='"+time.Value.ToString("yyyy-MM-dd HH:mm:ss")+"'");
		}
		sb.Append(";");
		
		var sql=sb.ToString();
		//sql.Dump();
		//var conn=new MySql.Data.MySqlClient.MySqlConnection(options.ConnectionString);
		//data.Catgs=conn.Query<Data.Catg>(sql).ToList();
		
		using(var conn=new MySqlConnection(options.ConnectionString))
		using(var multi=conn.QueryMultiple(sql)){
			data.Catgs=multi.Read<Data.Catg>().ToList();
			data.Props=multi.Read<Data.Prop>().ToList();
			data.PropValues=multi.Read<Data.PropValue>().ToList();
		}
		return data;
	}
	
	
	public void Dispose()
	{
		if(timer!=null)
		{
			timer.Dispose();	
		}
		Data=null;
	}

}
[ProtoContract]
public class Data
{
	[ProtoMember(1)]
	public IList<Item> Items{get;set;}
	[ProtoMember(2)]
	public IList<Catg> Catgs{get;set;}
	[ProtoMember(3)]
	public IList<Prop> Props{get;set;}
	[ProtoMember(4)]
	public IList<PropValue> PropValues{get;set;}
	
	public Data(){
		Catgs=new List<Catg>(257);
		Props=new List<Prop>(409877+877);
		PropValues=new List<PropValue>(1823192+3192);
	}
	
	[ProtoContract]
	public class Item
	{
		[ProtoMember(1)]
		public int Id{get;set;}
		[ProtoMember(2)]
		public string Code{get;set;}
		[ProtoMember(3)]
		public string Name{get;set;}
		
		public override bool Equals(object o)
		{
			var x=o as Item;
			if(x!=null){
				return (x.Id==Id);
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
		public int Id{get;set;}
		[ProtoMember(2)]
		public int ParentId{get;set;}
		[ProtoMember(3)]
		public bool IsParent{get;set;}
		[ProtoMember(4)]
		public int Priority{get;set;}
		[ProtoMember(5)]
		public string Name{get;set;}
		[ProtoMember(6)]
		public byte Status{get;set;}
		
		public override bool Equals(object o)
		{
			var x=o as Catg;
			if(x!=null){
				return (x.Id==Id);
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
		public int Id{get;set;}
		[ProtoMember(2)]
		public int CatgId{get;set;}
		[ProtoMember(3)]
		public int ParentPid{get;set;}
		[ProtoMember(4)]
		public int ParentVid{get;set;}
		[ProtoMember(5)]
		public string Name{get;set;}
		[ProtoMember(6)]
		public bool Must{get;set;}		
		[ProtoMember(7)]
		public bool Multi{get;set;}		
		[ProtoMember(8)]
		public bool IsKeyProp{get;set;}
		[ProtoMember(9)]
		public bool IsSaleProp{get;set;}
		[ProtoMember(10)]
		public bool IsEnumProp{get;set;}
		[ProtoMember(11)]
		public bool IsItemProp{get;set;}
		[ProtoMember(12)]
		public bool IsColorProp{get;set;}
		[ProtoMember(13)]
		public bool IsInputProp{get;set;}
		[ProtoMember(14)]
		public bool AllowAlias{get;set;}		
		[ProtoMember(15)]
		public string Priority{get;set;}
		[ProtoMember(16)]
		public byte Status{get;set;}

		public override bool Equals(object o)
		{
			var x=o as Prop;
			if(x!=null){
				return (x.Id==Id&&x.CatgId==CatgId);
			}
			return false;						
		}

		public override int GetHashCode()
		{			
			return Id+CatgId;
		}
	}
	[ProtoContract]
	public class PropValue
	{
		[ProtoMember(1)]
		public int Id{get;set;}
		[ProtoMember(2)]
		public int CatgId{get;set;}
		[ProtoMember(3)]
		public int PropId{get;set;}
		[ProtoMember(4)]
		public string Name{get;set;}
		[ProtoMember(5)]
		public string Alias{get;set;}
		[ProtoMember(6)]
		public string Priority{get;set;}
		[ProtoMember(7)]
		public byte Status{get;set;}
		
		public override bool Equals(object o)
		{
			var x=o as PropValue;
			if(x!=null){
				return (x.Id==Id&&x.CatgId==CatgId&&x.PropId==PropId);
			}
			return false;						
		}

		public override int GetHashCode()
		{			
			return Id+CatgId+PropId;
		}
	}
}

public class LocalCacheOptions
{
	public bool SyncEnable{get;set;}
	public int SyncInterval{get;set;}
	public int SaveInterval{get;set;}
	public string CacheFile{get;set;}
	public string ConnectionString{get;set;}	
}