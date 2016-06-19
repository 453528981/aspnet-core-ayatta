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
  <Reference>D:\project\keepme\Keepme.LocalCache\bin\Debug\net451\Keepme.LocalCache.dll</Reference>
  <NuGetReference>Dapper</NuGetReference>
  <NuGetReference>MySql.Data</NuGetReference>
  <NuGetReference>protobuf-net</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>MySql.Data.MySqlClient</Namespace>
  <Namespace>ProtoBuf</Namespace>
  <Namespace>ProtoBuf.Meta</Namespace>
  <Namespace>ProtoBuf.ServiceModel</Namespace>
  <Namespace>Keepme.LocalCache</Namespace>
</Query>

void Main()
{
	var options=new Options();
			//options.SyncEnable=true;
			options.CacheFile="d:/cache/dat2.bin";
			options.ConnectionString="Server=127.0.0.1;Database=base;Uid=root;Pwd=root;charset=utf8";
			options.SyncInterval=5;
			options.SaveInterval=20;
	var pc= new ProdCache(options);
}