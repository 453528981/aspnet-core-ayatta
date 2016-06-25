<Query Kind="Program">
  <Connection>
    <ID>577999c1-607c-4e5b-8d77-61164c3b7c34</ID>
    <Persist>true</Persist>
    <Driver Assembly="IQDriver" PublicKeyToken="5b59726538a49684">IQDriver.IQDriver</Driver>
    <Provider>Devart.Data.MySql</Provider>
    <CustomCxString>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAgeh4eyutP0SFuhsW6pKTLAAAAAACAAAAAAAQZgAAAAEAACAAAADSIc2H5z3y4UnDeyUyhUvRfArOqNac1JrO2wyWEz9J+gAAAAAOgAAAAAIAACAAAABaWBfD0wRSfSnQoUvxswiiwI1BSKGN3FLtZwIA8kYfFUAAAACkv5vwIJMUPkIawlx7HeKUEJPIgsb7w+ltIjiq5K25KVCC9yRuzBnle3pVxyVTyuCC2FnvQZMC6Jq1kquglAwkQAAAAOT84D2BL+MI3grgEvUiYJdWL7dQ+i7+3xD1mTGOSWXM9zNrWnblrCRbRFjADGUk9eFk+r1PHXehA2qOG+Y74ME=</CustomCxString>
    <Server>127.0.0.1</Server>
    <Database>store</Database>
    <UserName>root</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAgeh4eyutP0SFuhsW6pKTLAAAAAACAAAAAAAQZgAAAAEAACAAAACen6bGoKoESURjncTcLsqqXrRXnv24xHnbqS8tJjEAPwAAAAAOgAAAAAIAACAAAAB32TviFS7qcKwbjMKkDmAfxBpEFFb5UKTqmZdcFh4sBxAAAAA3Bq/1VriiZAKOTbzbI1ZOQAAAAFpHWF61EEHmf8fSaHxlGTw3memNs0g1w4ASh5JHBN9c8Vm/AA8ReNTOmG42MjfbGy2NcyTRMtaqKImJAQK6TbY=</Password>
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
  <Namespace>Dapper</Namespace>
  <Namespace>Devart.Data.MySql</Namespace>
</Query>

void Main()
{
	var t=GetTable("store","Item");
	GetDomain(t).Dump();
}
public string GetDomain(Table t)
{
	var i=1;
	var sb=new StringBuilder();
	sb.AppendLine("///<summary>");
	sb.AppendLine("/// "+t.Name);
	sb.AppendLine("/// created on "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
	sb.AppendLine("///<summary>");
	sb.AppendLine("[ProtoContract]");
	sb.AppendLine("public class "+t.Name+" : IEntity<>");
	sb.AppendLine("{");

	foreach(var c in t.Columns)
	{
		sb.AppendLine();
		sb.AppendLine("\t\t///<summary>");
		sb.AppendLine("\t\t/// "+c.Comment);
		sb.AppendLine("\t\t///<summary>");
		sb.AppendLine("\t\t[ProtoMember("+i+")]");
		sb.AppendFormat("\t\tpublic {0} {1} {{get;set;}}",c.Type,c.Field).AppendLine();
		
		i++;
	}
	sb.AppendLine("}");
	return sb.ToString();
}

public Table GetTable(string database,string table)
{
	var str=string.Format("server=127.0.0.1;database={0};uid=root;pwd=root;charset=utf8",database);
	var conn = new MySqlConnection(str);
	var t=new Table();
	t.Name=table;
	t.Columns=conn.Query<Table.Column>("show full columns from "+table).ToList();
	return t;
}
public class Table
{
	public string Name{get;set;}
	public IList<Column> Columns{get;set;}	

	public class Column
	{
		public string Field{get;set;}	
		public string Null{get;set;}	
		private string type;
		public string Type{
			get{		
				if(type=="tinyint(1)")
				{
					return "bool";
				}
				else{
					type=Regex.Replace(type,@"[0-9,\(\)]","");
				}
				switch(type)
				{
					case "tinyint":
						return "byte";
					case "smallint":
						return "short";
					case "mediumint":
					case "int":
					case "integer":
						return "int";
					case "double":
						return "double";
					case "float":
						return "float";
					case "decimal":
						return "decimal";
					case "numeric":
					case "real":
						return "decimal";
					case "bit":
						return "bool";
					case "date":
					case "time":
					case "year":
					case "datetime":
					case "timestamp":
						return "DateTime";
					case "tinyblob":
					case "blob":
					case "mediumblob":
					case "longblog":
					case "binary":
					case "varbinary":
						return "byte[]";
					case "char":
					case "varchar":                    
					case "tinytext":
					case "text":
					case "mediumtext":
					case "longtext":
						return "string";
					case "point":
					case "linestring":
					case "polygon":
					case "geometry":
					case "multipoint":
					case "multilinestring":
					case "multipolygon":
					case "geometrycollection":
					case "enum":
					case "set":
					default:
						return "";
				}  
			}
			set{
				type=value;
			}
		}
		public string Comment{get;set;}
		public string Extra{get;set;}	
	}
}