# webapi-basic
Basics of ASP.NET Web Api with WebApi 1 and WebApi 2 .

Topics covered : 
* Basic Routing in WebApi.Config (webapi 1)
* Dependency Injection with Ninject in webapi (check NinjectWebCommon.cs)
* object vs model vs entity as Response from WebApi controller's action (check FoodsController.cs Get action)
* Serialization Formatters (Json, XML, Jsonp). Check WebApi.Config
* Object specific model Converter. Check LinkModelConverter.cs and WebApi.Config
* Versioning with Custom Controller Selector. Check WebApi.Config and CustomWebApiControllerSelector.cs (webapi 1)
* Custom Auhthorization Attribute. Check TestBasicAuthorizeAttribute.cs, DiariesController.cs, FoodsController.cs
* Basic Authentication and Token Authentication. Check TestBasicAuthorizeAttribute.cs
* Username and Password submit in WebApi. Check TokenController.cs, 
* Attribute Routing. Check StatsController.cs (webapi 2)
* CORS -> Global, Controller based, Controller action based. Check WebApi.Config (webapi 2)
* HttpResponseMessage vs IHttpActionResult. Check VersionedActionResult.cs and Get of FoodsController.cs
