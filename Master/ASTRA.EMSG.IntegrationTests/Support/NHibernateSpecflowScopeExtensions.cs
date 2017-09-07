using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.Summarisch;
using ASTRA.EMSG.Business.Models.Identity;
using NHibernate.Linq;

namespace ASTRA.EMSG.IntegrationTests.Support
{
    public static class NHibernateSpecflowScopeExtensions
    {
        public static Mandant GetMandant(this NHibernateSpecflowScope scope, string mandant)
        {
            return scope.Session.Query<Mandant>().Single(m => m.MandantName == mandant);
        }

        public static ErfassungsPeriod GetCurrentErfassungsperiod(this NHibernateSpecflowScope scope, string mandant)
        {
            return scope.Session.Query<ErfassungsPeriod>().Single(ep => ep.Mandant.MandantName == mandant && !ep.IsClosed);
        }

        public static ErfassungsPeriod GetCurrentErfassungsperiod(this NHibernateSpecflowScope scope, Mandant mandant)
        {
            return scope.Session.Query<ErfassungsPeriod>().Single(ep => ep.Mandant == mandant && !ep.IsClosed);
        }

        public static List<ErfassungsPeriod> GetClosedErfassungsperiods(this NHibernateSpecflowScope scope, string mandant)
        {
            return scope.Session.Query<ErfassungsPeriod>().Where(ep => ep.Mandant.MandantName == mandant && ep.IsClosed).ToList();
        }

        public static NetzSummarischDetail GetNetzSummarischDetail(this NHibernateSpecflowScope scope, string belastungskategorie, Mandant mandant, ErfassungsPeriod erfassungsPeriod)
        {
            return scope.Session.Query<NetzSummarischDetail>()
                .Single(ns => ns.Belastungskategorie.Typ == belastungskategorie &&
                ns.NetzSummarisch.Mandant == mandant &&
                ns.NetzSummarisch.ErfassungsPeriod == erfassungsPeriod);
        }

        public static Belastungskategorie GetBelastungskategorie(this NHibernateSpecflowScope scope, string belastungskategorieTyp)
        {
            return scope.Session.Query<Belastungskategorie>().Single(bk => bk.Typ == belastungskategorieTyp);
        }

        public static Mandant GetCurrentMandant(this NHibernateSpecflowScope scope)
        {
            var dbCtx = new ApplicationDbContext(scope.Session.Connection.ConnectionString);
            var currentUserMandanten = dbCtx.MandantRoles.Where(m => m.User.UserName == "test")
                .Select(a => a.MandantName).Distinct().Single();
            return scope.Session.Query<Mandant>().Single(c => c.MandantName == currentUserMandanten);
        }
    }
}