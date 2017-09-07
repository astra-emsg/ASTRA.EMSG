using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using HtmlAgilityPack;
using NUnit.Framework;
using MvcIntegrationTestFramework.Browsing;
using MvcIntegrationTestFramework.Hosting;
using ASTRA.EMSG.Web.Infrastructure;
using TechTalk.SpecFlow;

namespace ASTRA.EMSG.IntegrationTests.Support.MvcTesting
{
    public class BrowserDriver
    {
        private readonly List<TestResult> history = new List<TestResult>();

        private AppHost appHost;
        private AppHost AppHost
        {
            get
            {
                if (appHost == null)
                {
                    appHost = AppHostBuilder.AppHost;
                }
                return appHost;
            }
        }

        public void InvokeGetAction(string uri)
        {
            var hi = AppHost.SimulateBrowsingSession(
                    b =>
                    {
                        var result = b.Get(uri);
                        return new TestResultProcessor().Process(result, uri);
                    });
            history.Add(hi);
        }

        public void InvokeGetAction<TController>(Expression<Func<TController, ActionResult>> func)
            where TController : Controller
        {
            InvokeGetAction(GetActionUri(func));
        }

        public void InvokeGetAction<TController, TArg>(Expression<Func<TController, object, ActionResult>> func, TArg args) where TArg : class
            where TController : Controller
        {
            var uri = GetActionUri(func);
            if (args != null)
                uri = uri + "?" + string.Join("&", new RouteValueDictionary(args).Select(k => string.Format("{0}={1}", k.Key, k.Value)));

            InvokeGetAction(uri);
        }

        public void InvokeGetAction<TController, TArg>(Expression<Func<TController, TArg, ActionResult>> func, NameValueCollection getData)
            where TController : Controller
        {
            var uri = GetActionUri(func);

            if (getData != null)
            {
                uri = uri + "?" + string.Join("&", getData.AllKeys.Select(k => string.Join("&", getData.GetValues(k).Select(v => string.Format("{0}={1}", k, v)))));
                uri = uri.Replace("[", "%5B").Replace("]", "%5D");
            }

            InvokeGetAction(uri);
        }

        public void InvokePostAction(string uri, NameValueCollection postData)
        {

            var hi = AppHost.SimulateBrowsingSession(
                b =>
                {
                    var result = b.Post(uri, postData);
                    return new TestResultProcessor().Process(result, uri);
                });
            history.Add(hi);
        }

        public void InvokePostAction<TController, TArg>(Expression<Func<TController, TArg, ActionResult>> func, NameValueCollection postData)
            where TController : Controller
            where TArg : class
        {
            var uri = GetActionUri(func);
            InvokePostAction(uri, postData);
        }

        public void InvokePostAction<T, TArg>(Expression<Func<T, TArg, ActionResult>> func, TArg args, bool resuseLastRequest = true)
            where T : Controller
            where TArg : class
        {
            var postData = new NameValueCollection();
            if (resuseLastRequest)
            {
                postData = ExtractPostDataFromLastRequest();
            }

            if (args != null)
            {
                BuildPostDataFrom(args, postData);
            }
            InvokePostAction(func, postData);
        }

        private static void BuildPostDataFrom(object args, NameValueCollection postData, string prefix = "")
        {
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(args))
            {
                if (!Equals(property.GetValue(args), GetDefault(property.PropertyType)))
                {
                    if (property.PropertyType.IsValueType || property.PropertyType == typeof(string))
                        postData[prefix + property.Name] = property.Converter.ConvertToString(property.GetValue(args));
                    else
                        BuildPostDataFrom(property.GetValue(args), postData, property.Name + ".");
                }
            }
        }

        public NameValueCollection ExtractPostDataFromLastRequest()
        {
            var postData = new NameValueCollection();
            foreach (var input in CurrentHtmlResponse.DocumentNode.Descendants("input").Where(n => !new[] { "submit", "reset", "button", "file", "checkbox" }.Contains(n.Attributes["type"].Value.ToLower())))
            {
                postData.Add(input.Attributes["name"].Value, input.Attributes["value"] != null ? input.Attributes["value"].Value : "");
            }
            return postData;
        }

        public void FollowRedirection()
        {
            var uri = GetFullRedirectionUrl();
            if (uri == null) return;

            var hi = AppHost.SimulateBrowsingSession(b =>
            {
                var result = b.Get(uri);
                return new TestResultProcessor().Process(result, uri);
            });
            history.Add(hi);
        }


        [Serializable]
        private class TestResultProcessor
        {
            public TestResult Process(RequestResult result, string uri)
            {
                TestResult testResult = null;

                if (result.ActionExecutedContext != null && result.ActionExecutedContext.Exception != null)
                    return new TestErrorResult(uri, result.ResponseText) { Exception = result.ActionExecutedContext.Exception };

                var emptyResult = result.ActionExecutedContext != null ? result.ActionExecutedContext.Result as EmsgEmptyResult : null;
                if (emptyResult != null)
                    testResult = new TestEmptyResult(uri, result.ResponseText);

                var viewResult = result.ActionExecutedContext != null ? result.ActionExecutedContext.Result as ViewResult : null;
                if (viewResult != null)
                    testResult = new TestViewResult(uri, result.ResponseText, viewResult.Model, viewResult.ViewData.ModelState);

                var jsonResult = result.ActionExecutedContext != null ? result.ActionExecutedContext.Result as JsonResult : null;
                if (jsonResult != null)
                    testResult = new TestJsonResult(uri, result.ResponseText, jsonResult.Data);

                var partialViewResult = result.ActionExecutedContext != null ? result.ActionExecutedContext.Result as PartialViewResult : null;
                if (partialViewResult != null)
                    testResult = new TestPartialViewResult(uri, result.ResponseText, partialViewResult.Model, partialViewResult.ViewData.ModelState);

                var fileContentResult = result.ActionExecutedContext != null ? result.ActionExecutedContext.Result as FileContentResult : null;
                if (fileContentResult != null)
                    testResult = new TestFileContentResult(uri, result.ResponseText, fileContentResult.FileContents, fileContentResult.ContentType, fileContentResult.FileDownloadName);

                var redirectResult = result.ResultExecutedContext != null ? result.ResultExecutedContext.Result as RedirectResult : null;
                if (redirectResult != null)
                    testResult = new TestRedirectResult(uri, result.ResponseText, redirectResult.Url);

                var redirectToRouteResult = result.ResultExecutedContext != null ? result.ResultExecutedContext.Result as RedirectToRouteResult : null;
                if (redirectToRouteResult != null)
                    testResult = new TestRedirectToRouteResult(uri, result.ResponseText, redirectToRouteResult.RouteValues.ToDictionary(k => k.Key, v => v.Value));

                if (result.Response != null && result.Response.StatusCode == 401) // Unauthorized
                    testResult = new TestHttpUnauthorizedResult(uri);

                if (result.Response != null && result.Response.StatusCode == 404) // Unauthorized
                    testResult = new TestHttpNotFoundResult(uri);

                if (testResult == null)
                    testResult = new TestErrorResult(uri, result.ResponseText);

                //ToDo: PermissionKey
                //if (result.ActionExecutedContext != null)
                //    testResult.Permission = (Permission?)result.ActionExecutedContext.HttpContext.Items["PermissionKey"] ?? Permission.None;
                //else if (result.ResultExecutedContext != null)
                //    testResult.Permission = (Permission?)result.ResultExecutedContext.HttpContext.Items["PermissionKey"] ?? Permission.None;

                return testResult;
            }
        }

        private static object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        private static string GetActionUri<TController>(Expression<Func<TController, ActionResult>> func)
            where TController : Controller
        {
            return GetActionUri<TController>((MethodCallExpression)func.Body);
        }

        private static string GetActionUri<TController, TArg>(Expression<Func<TController, TArg, ActionResult>> func)
            where TController : Controller
        {
            return GetActionUri<TController>((MethodCallExpression)func.Body);
        }

        private static string GetActionUri<TController>(MethodCallExpression methodCallExpression)
            where TController : Controller
        {
            var actionName = methodCallExpression.Method.Name;
            var controllerType = typeof(TController);
            var controllerName = controllerType.Name.Replace("Controller", "");
            var areaName = controllerType.GetAreaName();
            var uri = string.IsNullOrEmpty(areaName)
                          ? string.Format("/{0}/{1}", controllerName, actionName)
                          : string.Format("/{0}/{1}/{2}", areaName, controllerName, actionName);
            return uri;
        }

        public HtmlDocument CurrentHtmlResponse
        {
            get
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(history.Last().ResponseText);
                return doc;
            }
        }

        public string GetCurrentRedirection()
        {
            var redirect = history.LastOrDefault() as TestRedirectResultBase;
            return redirect == null ? null : redirect.NewUri;
        }

        public string GetFullRedirectionUrl()
        {
            var redirect = history.LastOrDefault() as TestRedirectResultBase;
            if (redirect == null) return null;
            var getData = redirect.RedirectData.AllKeys.Select(key => string.Format("{0}={1}", key, redirect.RedirectData.Get(key)));
            return string.Format("{0}?{1}", redirect.NewUri, string.Join("&", getData));
        }

        public NameValueCollection GetCurrentRedirectData()
        {
            var redirect = history.LastOrDefault() as TestRedirectResultBase;
            return redirect != null ? redirect.RedirectData : null;
        }

        public bool WasLastResultAnError()
        {
            return history.LastOrDefault() is TestErrorResult;
        }

        public void AssertLastResultWasNotAnError()
        {
            if (WasLastResultAnError())
            {
                Assert.Fail(GetRequestResult<TestErrorResult>().ToString());
            }
        }

        public TModel GetCurrentModel<TModel>(int stepsBack = 0) where TModel : class
        {
            var viewResult = history.Reverse<TestResult>().Skip(stepsBack).FirstOrDefault() as IResultWithModel;
            return viewResult != null ? viewResult.Model as TModel : null;
        }

        public string GetCurrentDownloadedFileName()
        {
            var fileContentResult = history.LastOrDefault() as TestFileContentResult;
            return fileContentResult != null ? fileContentResult.FileDownloadName : null;
        }

        public ModelStateDictionary GetCurrentModelState()
        {
            var viewResult = history.LastOrDefault() as TestViewResult;
            return viewResult != null ? viewResult.ModelState : null;
        }

        public bool IsLastResultEmptyResult()
        {
            return history.LastOrDefault() is TestEmptyResult;
        }

        public bool IsCurrentOfType<TModel>()
        {
            var viewResult = history.LastOrDefault() as TestViewResult;
            return viewResult != null && viewResult.Model is TModel;
        }

        public TResult GetRequestResult<TResult>(int stepsBack = 0) where TResult : TestResult
        {
            var testResult = history.Reverse<TestResult>().Skip(stepsBack).FirstOrDefault();
            Assert.IsNotNull(testResult, "The browser history is not containing {0} element", stepsBack);
            var result = testResult as TResult;
            Assert.IsNotNull(result, "The expected result was {0}, but the current result is {1}.\nResponse text: {2}",
                             typeof(TResult).Name, testResult.GetType().Name, testResult.ResponseText);
            return result;
        }
    }



    [Serializable]
    public class TestResult
    {
        public TestResult(string requestUri, string responseText)
        {
            RequestUri = requestUri;
            ResponseText = responseText;
        }

        public string RequestUri { get; private set; }
        public string ResponseText { get; set; }
        //public Permission Permission { get; set; }
    }

    [Serializable]
    public class TestErrorResult : TestResult
    {
        public TestErrorResult(string requestUri, string responseText)
            : base(requestUri, responseText)
        {
        }

        public Exception Exception { get; set; }

        public override string ToString()
        {
            if (Exception != null)
                return Exception.ToString();
            return ResponseText;
        }
    }

    [Serializable]
    public class TestRedirectResultBase : TestResult
    {
        public TestRedirectResultBase(string requestUri, string responseText)
            : base(requestUri, responseText)
        {
        }

        public NameValueCollection RedirectData { get; protected set; }
        public string NewUri { get; protected set; }
    }

    [Serializable]
    public class TestRedirectResult : TestRedirectResultBase
    {
        public TestRedirectResult(string requestUri, string responseText, string redirectUrl)
            : base(requestUri, responseText)
        {
            var dashPosition = Math.Max(0, redirectUrl.IndexOf('/'));
            var qmarkPosition = redirectUrl.IndexOf('?', dashPosition);
            var qmarkPositon = qmarkPosition == -1 ? redirectUrl.Length : qmarkPosition;
            NewUri = redirectUrl.Substring(dashPosition, qmarkPositon - dashPosition);
            RedirectData = HttpUtility.ParseQueryString(redirectUrl.Substring(qmarkPositon).TrimStart('?'));
        }
    }

    [Serializable]
    public class TestRedirectToRouteResult : TestRedirectResultBase
    {
        public TestRedirectToRouteResult(string requestUri, string responseText, Dictionary<string, object> routeValues)
            : base(requestUri, responseText)
        {
            NewUri = string.Format("/{0}/{1}", routeValues["controller"], routeValues["action"]);
            RedirectData = new NameValueCollection();
            routeValues.Where(r => r.Key != "controller" && r.Key != "action").ToList().ForEach(r => RedirectData.Add(r.Key, (r.Value ?? "").ToString()));
        }
    }

    public interface IResultWithModel
    {
        object Model { get; }
    }

    [Serializable]
    public class TestViewResult : TestResult, IResultWithModel
    {
        public TestViewResult(string requestUri, string responseText, object model, ModelStateDictionary modelState)
            : base(requestUri, responseText)
        {
            Model = model;
            ModelState = modelState;
        }

        public object Model { get; private set; }
        public ModelStateDictionary ModelState { get; private set; }
    }

    [Serializable]
    public class TestJsonResult : TestResult, IResultWithModel
    {
        public TestJsonResult(string requestUri, string responseText, object model)
            : base(requestUri, responseText)
        {
            Model = model;
        }

        public object Model { get; private set; }
    }

    [Serializable]
    public class TestEmptyResult : TestResult
    {
        public TestEmptyResult(string requestUri, string responseText)
            : base(requestUri, responseText)
        {
        }
    }

    [Serializable]
    public class TestPartialViewResult : TestViewResult
    {
        public TestPartialViewResult(string requestUri, string responseText, object model, ModelStateDictionary modelState)
            : base(requestUri, responseText, model, modelState)
        {
        }
    }

    [Serializable]
    public class TestFileContentResult : TestResult
    {
        public TestFileContentResult(string requestUri, string responseText, byte[] content, string contentType, string fileDownloadName)
            : base(requestUri, responseText)
        {
            Content = content;
            FileDownloadName = fileDownloadName;
            ContentType = contentType;
        }

        public byte[] Content { get; private set; }
        public string FileDownloadName { get; private set; }
        public string ContentType { get; private set; }
    }

    [Serializable]
    public class TestHttpUnauthorizedResult : TestErrorResult
    {
        public TestHttpUnauthorizedResult(string requestUri) : base(requestUri, "401") { }
    }
    
    [Serializable]
    public class TestHttpNotFoundResult : TestErrorResult
    {
        public TestHttpNotFoundResult(string requestUri) : base(requestUri, "404") { }
    }
}