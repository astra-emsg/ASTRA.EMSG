using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using ASTRA.EMSG.Tests.Common;
using ASTRA.EMSG.Web.Infrastructure;
using Moq;
using NUnit.Framework;
using TestSessionService = ASTRA.EMSG.Tests.Common.TestServices.TestSessionService;
using System.Linq;
using ASTRA.EMSG.Business.Services.Identity;

namespace ASTRA.EMSG.Tests.Security
{
    [TestFixture]
    public class SecurityServiceTests
    {
        private readonly Guid testMandantOneId = Guid.NewGuid();
        private readonly Guid testMandantTwoId = Guid.NewGuid();

        private const string TestMandatorNameOne = "TestMandantOne";
        private const string TestMandatorNameTwo = "TestMandantTwo";

        [Test]
        public void TestGetCurrentMandantIfThereIsAlreadyOneSelectedInSession()
        {
            var testSessionService = new TestSessionService();
            var testCookieService = new TestCookieService();

            testSessionService.SelectedMandantId = testMandantOneId;
            testCookieService.SelectedMandantId = testMandantOneId;

            var securityService = GetSecurityService(testSessionService, testCookieService);

            var currentMandant = securityService.GetCurrentMandant();

            Assert.AreEqual(TestMandatorNameOne, currentMandant.MandantName);
            Assert.AreEqual(testMandantOneId, currentMandant.Id);
        }

        [Test]
        public void TestGetCurrentMandantIfThereIsNoSelectedInSession()
        {
            var testSessionService = new TestSessionService();
            var testCookieService = new TestCookieService();

            testSessionService.SelectedMandantId = null;
            testCookieService.SelectedMandantId = null;

            var securityService = GetSecurityService(testSessionService, testCookieService);

            var currentMandant = securityService.GetCurrentMandant();

            Assert.AreEqual(TestMandatorNameOne, currentMandant.MandantName);
            Assert.AreEqual(testMandantOneId, currentMandant.Id);
        }

        [Test]
        public void TestSetCurrentMandantAndThanGetCurrentMandant()
        {
            var securityService = GetSecurityService(new TestSessionService(), new TestCookieService());

            securityService.SetCurrentMandant(testMandantOneId);
            var currentMandant = securityService.GetCurrentMandant();

            Assert.AreEqual(TestMandatorNameOne, currentMandant.MandantName);
            Assert.AreEqual(testMandantOneId, currentMandant.Id);
        }

        private ISecurityService GetSecurityService(TestSessionService testSessionService, TestCookieService testCookieService)
        {
            Mock<IIdentityService> ldapWebServiceMock = new Mock<IIdentityService>();
            ldapWebServiceMock.Setup(
                m => m.GetMandatorShort(StubUserIdentityProvider.TestUsername, IdentityCacheService.ApplicationName))
                .Returns(new List<IdentityMandatorShort>
                             {
                                 new IdentityMandatorShort
                                     {
                                         Applicationname = IdentityCacheService.ApplicationName,
                                         Mandatorname = TestMandatorNameTwo
                                     },
                                 new IdentityMandatorShort
                                     {
                                         Applicationname = IdentityCacheService.ApplicationName,
                                         Mandatorname = TestMandatorNameOne
                                     }
                             });
            ldapWebServiceMock.Setup(
                m => m.GetRole(StubUserIdentityProvider.TestUsername, IdentityCacheService.ApplicationName))
                .Returns(new List<UserRole>
                             {
                                 new UserRole { MandatorName = TestMandatorNameTwo, Rolle = Rolle.DataManager},
                                 new UserRole { MandatorName = TestMandatorNameOne, Rolle = Rolle.DataManager},
                             });
            ldapWebServiceMock.Setup(
                m =>
                m.GetRoleMandator(It.IsAny<string>(), StubUserIdentityProvider.TestUsername, IdentityCacheService.ApplicationName))
                .Returns<string, string, string>((m, u, a) => new List<UserRole>() { new UserRole() { MandatorName = m, Rolle = Rolle.DataManager } });

            Mock<IMandantenService> mandantRepositoryMock = new Mock<IMandantenService>();
            mandantRepositoryMock.Setup(m => m.GetCurrentMandanten())
                .Returns(new List<Mandant>
                             {
                                 new Mandant() { Id = testMandantTwoId, MandantName = TestMandatorNameTwo},
                                 new Mandant() { Id = testMandantOneId, MandantName = TestMandatorNameOne},
                             });
            mandantRepositoryMock.Setup(m => m.GetMandantById(testMandantOneId))
                .Returns(new Mandant
                {
                    Id = testMandantOneId,
                    MandantName = TestMandatorNameOne
                });

            Mock<IServerConfigurationProvider> serverConfigurationProviderMock = new Mock<IServerConfigurationProvider>();

            return new SecurityService(new StubUserIdentityProvider(), testSessionService, mandantRepositoryMock.Object, testCookieService, new IdentityCacheService(new SessionCacheService(testSessionService), serverConfigurationProviderMock.Object, ldapWebServiceMock.Object));
        }
    }
}
