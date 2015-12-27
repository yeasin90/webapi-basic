using CacheCow.Server;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Dispatcher;
using tutWebApi1.Converters;
using tutWebApi1.Filters;
using tutWebApi1.Services;
using WebApiContrib.Formatting.Jsonp;

namespace tutWebApi1
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            // Converted to Attribute route (Web Api 2)
            //config.Routes.MapHttpRoute(
            //    name: "Food",
            //    routeTemplate: "api/nutrition/foods/{foodid}",
            //    defaults: new { controller = "foods", foodid = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
                name: "Measures",
                routeTemplate: "api/nutrition/foods/{foodid}/measures/{id}",
                defaults: new { controller = "measures", id = RouteParameter.Optional }
            );

            //config.Routes.MapHttpRoute(
            //    name: "Measures2",
            //    routeTemplate: "api/v2/nutrition/foods/{foodid}/measures/{id}",
            //    defaults: new { controller = "measuresv2", id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
                name: "Diaries",
                routeTemplate: "api/user/diaries/{diaryid}",
                defaults: new { controller = "diaries", diaryid = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DiaryEntries",
                routeTemplate: "api/user/diaries/{diaryid}/entries/{id}",
                defaults: new { controller = "diaryEntries", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DiarySummary",
                routeTemplate: "api/user/diaries/{diaryid}/summary",
                defaults: new { controller = "diarysummary" }
            );

            config.Routes.MapHttpRoute(
                name: "Token",
                routeTemplate: "api/token",
                defaults: new { controller = "token" }
            );

            // below lines are custom
            // Why??
            // read this : http://travis.io/blog/2015/10/13/how-to-get-aspnet-web-api-to-return-json-instead-of-xml-in-browser/
            // and this : http://dotnet-concept.com/Tips/2015/3/5798823/Changing-default-behaviour-to-get-JSON-response-instead-of-XML-response-in-Asp-Net-Web-API-Csharp
            var xmlMediaType = config.Formatters.OfType<XmlMediaTypeFormatter>().FirstOrDefault();
            config.Formatters.Remove(xmlMediaType);


            // client makes request to the server with api calls
            // client can specify the format in which they want to see the data from API via Accept header
            // client can specify muultiple csv in Accept header. Example = Accept : text/xml, application/json, text/html
            // we will set preference level in web api or server
            // this is where formatters will come into play.
            // below, we are saying that, if accept header conatins application/json then take it with JsonMediaTypeFormatter and convert it to lower case
            // there are other formatters like XmlMediaTypeFormatter etc.
            var jsomFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().FirstOrDefault();
            jsomFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            // custom serialization for a specific Model type (LinkModel)
            // converter is an object that knows how to convert a certain type in and out into Json
            // here, we made a cusotm Converter : LinkModelConverter (this will take care of LinkModel for conversion in and out of JSON)
            jsomFormatter.SerializerSettings.Converters.Add(new LinkModelConverter());
            // add custom media-types for JSON
            CreateMediaTypes(jsomFormatter);

            // Add support JSOP. So that, when people request JSONP, we can return
            // Nuget : webapicontrib.jsonp
            // Make request to : http://localhost:3750/api/nutrition/foods?callback=yourJavaScriptFunction
            // with , Accept : application/javascript
            // whould response something like :  yourJavaScriptFunction(HttpResponseMessageJson..)
            // In other worlds, defining a Javascript method from client and executing it in server
            var formatter = new JsonpMediaTypeFormatter(jsomFormatter, "callback");
            config.Formatters.Insert(0, formatter);

            // Replace the controller Configuration with our custom. WebApi : 1
            //config.Services.Replace(typeof(IHttpControllerSelector),
            //    new CustomWebApiControllerSelector(config));

            // Configuration Caching/Etag support
            // Nuget : Cachecow.server
            var cacheHandler = new CachingHandler(config);
            config.MessageHandlers.Add(cacheHandler);

            // Add support CORS
            // Nuget : ASP.NET Wep API2 Cross-Origin Support
            var attr = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(attr);

            // Force HTTPS on entire API
#if !DEBUG
            config.Filters.Add(new RequireHttpsCustomAttribute());
#endif
        }

        private static void CreateMediaTypes(JsonMediaTypeFormatter jsomFormatter)
        {
            // Accept : application/vnd.tutWebApi1.food.v1+json
            // or any one of the elements
            var mediaTypes = new string[]
            {
                "application/vnd.tutWebApi1.food.v1+json",
                "application/vnd.tutWebApi1.measure.v1+json",
                "application/vnd.tutWebApi1.measure.v2+json",
                "application/vnd.tutWebApi1.diary.v1+json",
                "application/vnd.tutWebApi1.diraryEntry.v1+json"
            };

            foreach (var mediaType in mediaTypes)
            {
                jsomFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(mediaType));
            }
        }
    }
}
