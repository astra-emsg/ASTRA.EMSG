using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Business.Reports.AusgefuellteErfassungsformulareFuerOberflaechenschaeden;
using ASTRA.EMSG.Business.Services.FilterBuilders;
using Autofac;
using NUnit.Framework;

namespace ASTRA.EMSG.Tests.FilterTests
{
    [TestFixture]
    public class FiltererFactoryTest
    {
        public class FilterParameter : IStrassennameFilter
        {
            public string Strassenname { get; set; }
        }

        [Test]
        public void IntegrationSmokeTest()
        {
            var parameter = new FilterParameter()
                                {
                                    Strassenname = "apple",
                                };
            var list =
                new List<ZustandsabschnittGIS> {
                    new ZustandsabschnittGIS
                        {
                            StrassenabschnittGIS = new StrassenabschnittGIS() {Strassenname = "appleStreet"}
                        } ,
                        new ZustandsabschnittGIS
                        {
                            StrassenabschnittGIS = new StrassenabschnittGIS() {Strassenname = "otherFruit"}
                        } ,
                        new ZustandsabschnittGIS
                        {
                            StrassenabschnittGIS = new StrassenabschnittGIS() {Strassenname = "apple"}
                        } ,
                     }.AsQueryable();

            
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<FiltererFactory>().As<IFiltererFactory>().SingleInstance();

            containerBuilder.RegisterType<StrasennameFilterBuilder>().Keyed<IFilterBuilder>(typeof(IStrassennameFilter));            

            var container = containerBuilder.Build();



            var filterrerFactory = container.Resolve<IFiltererFactory>();
            var filterer = filterrerFactory.CreateFilterer<ZustandsabschnittGIS>(parameter);

            var result = filterer.Filter(list).ToList();
            Assert.AreEqual(2, result.Count);
        }
    }
}
