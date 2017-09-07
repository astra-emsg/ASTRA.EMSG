using System.Configuration;
using System.Web.Mvc;
using System.Web.WebPages;
using ASTRA.EMSG.Web.App_Start;
using PrecompiledMvcViewEngine;

[assembly: WebActivator.PreApplicationStartMethod(typeof(PrecompiledMvcViewEngineStart), "Start")]

namespace ASTRA.EMSG.Web.App_Start
{
    public static class PrecompiledMvcViewEngineStart
    {
        public static void Start()
        {
            bool usePrecompiledViews;
            if (!bool.TryParse(ConfigurationManager.AppSettings["UsePrecompiledViews"] ?? "", out usePrecompiledViews))
                usePrecompiledViews = false;

            if (usePrecompiledViews)
            {
                // use dynamically compiled views
                var engine = new PrecompiledMvcEngine(typeof(PrecompiledMvcViewEngineStart).Assembly);
                ViewEngines.Engines.Insert(0, engine);
                // StartPage lookups are done by WebPages. 
                VirtualPathFactoryManager.RegisterVirtualPathFactory(engine);
            }
        }
    }
}