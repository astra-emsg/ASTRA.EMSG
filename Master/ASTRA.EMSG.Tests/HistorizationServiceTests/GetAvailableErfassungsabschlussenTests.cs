using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Models;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.Services;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Historization;
using Moq;
using NUnit.Framework;

namespace ASTRA.EMSG.Tests.HistorizationServiceTests
{
    [TestFixture]
    public class GetAvailableErfassungsabschlussenTests
    {
        private Mock<ITimeService> timeServiceMock;
        private Mock<IErfassungsPeriodService> erfassungsPeriodServiceMock;
        private HistorizationService historizationService;

        [SetUp]
        public void Setup()
        {
            timeServiceMock = new Mock<ITimeService>();
            erfassungsPeriodServiceMock = new Mock<IErfassungsPeriodService>();
            historizationService = new HistorizationService(erfassungsPeriodServiceMock.Object, timeServiceMock.Object);
            timeServiceMock.Setup(s => s.Now).Returns(new DateTime(2011, 1, 1));
        }

        [Test]
        public void ShouldReturnLastTenYearsIfThereIsNoClosedErfassungsPeriod()
        {
            erfassungsPeriodServiceMock.Setup(s => s.GetClosedErfassungsPeriodModels())
                .Returns(new List<ErfassungsPeriodModel>());

            var result = GetAvailableYears();
            CollectionAssert.AreEquivalent(new[] { 2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011 }, result);
        }

        [Test]
        public void ShouldReturnLastTenYearsIfTheLastClosedErfassungsPeriodOlderThenTenYears()
        {
            erfassungsPeriodServiceMock.Setup(s => s.GetClosedErfassungsPeriodModels())
                .Returns(new List<ErfassungsPeriodModel>
                             {
                                 new ErfassungsPeriodModel { IsClosed = true, Erfassungsjahr = new DateTime(1981, 1, 1) }
                             });

            var result = GetAvailableYears();
            CollectionAssert.AreEquivalent(new[] { 2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011 }, result);
        }

        [Test]
        public void ShouldReturnYearsSinceTheLastClosedErfassungPeriod()
        {
            erfassungsPeriodServiceMock.Setup(s => s.GetClosedErfassungsPeriodModels())
                .Returns(new List<ErfassungsPeriodModel>
                             {
                                 new ErfassungsPeriodModel { IsClosed = true, Erfassungsjahr = new DateTime(2002, 1, 1) },
                                 new ErfassungsPeriodModel { IsClosed = true, Erfassungsjahr = new DateTime(2005, 1, 1) }
                             });

            var result = GetAvailableYears();
            CollectionAssert.AreEquivalent(new[] { 2006, 2007, 2008, 2009, 2010, 2011 }, result);
        }

        [Test]
        public void ShouldReturnEmptyListIfTheErfassungsPeriodClosedForTheCurrentYear()
        {
            erfassungsPeriodServiceMock.Setup(s => s.GetClosedErfassungsPeriodModels())
                .Returns(new List<ErfassungsPeriodModel>
                             {
                                 new ErfassungsPeriodModel { IsClosed = true, Erfassungsjahr = new DateTime(2011, 1, 1) },
                             });

            var result = GetAvailableYears();
            CollectionAssert.IsEmpty(result);
        }

        private IEnumerable<int> GetAvailableYears()
        {
            return historizationService.GetAvailableErfassungsabschlussen().Select(m => m.AbschlussDate.Year).ToArray();
        }
    }
}
