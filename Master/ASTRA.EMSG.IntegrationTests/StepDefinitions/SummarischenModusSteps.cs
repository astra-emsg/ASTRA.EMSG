using System;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Entities.Summarisch;
using ASTRA.EMSG.Business.Models.Summarisch;
using ASTRA.EMSG.IntegrationTests.Support;
using ASTRA.EMSG.IntegrationTests.Support.MvcTesting;
using ASTRA.EMSG.Web.Areas.NetzverwaltungSummarisch.Controllers;
using ASTRA.EMSG.Web.Infrastructure.TelerikExtensions;
using NUnit.Framework;
using NHibernate.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using System.Linq;
using ASTRA.EMSG.Web.Areas.Auswertungen.Controllers;
using Kendo.Mvc.UI;
using ExpressionHelper = ASTRA.EMSG.Common.ExpressionHelper;

namespace ASTRA.EMSG.IntegrationTests.StepDefinitions
{
    [Binding]
    public class SummarischenModusSteps : StepsBase
    {
        private DataSourceRequest dataSourceRequest = new DataSourceRequest();
        public SummarischenModusSteps(BrowserDriver browserDriver)
            : base(browserDriver)
        {
        }
        
        [Given(@"für Mandant '(.+)' folgende summarische Zustands- und Netzinformationen existieren:")]
        public void FuerMandantFolgendeSummarischeZustandsUndNetzinformationenExistieren(string mandantName, Table table)
        {
            using (var nhScope = new NHibernateSpecflowScope())
            {
                var rows = table.CreateSet<NetzSummarischRow>();

                var mandant = nhScope.GetMandant(mandantName);
                var erfassungsPeriod = nhScope.GetCurrentErfassungsperiod(mandantName);

                foreach (var row in rows)
                {
                    var netzSummarisch = nhScope.GetNetzSummarischDetail(row.Belastungskategorie, mandant, erfassungsPeriod);
                    netzSummarisch.MittlererZustand = row.MittlererZustandValue;
                    netzSummarisch.Fahrbahnlaenge = row.MengeGesamtlangeValue ?? 0;
                    netzSummarisch.Fahrbahnflaeche = (int)(row.MengeGesamtflacheValue ?? 0);
                    nhScope.Session.Update(netzSummarisch);
                }
            }
        }

        [Given(@"folgende summarische Zustands- und Netzinformationen existieren:")]
        public void AngenommenFolgendeSummarischeZustands_UndNetzinformationenExistieren(Table table)
        {
            ScenarioContext.Current.Pending();
        }


        [When(@"ich folgende summarische Zustands- und Netzinformationen eingebe:")]
        public void WennIchFolgendeSummarischeZustands_UndNetzinformationenEingebe(Table table)
        {
            var zustandUndNetzinformationenEditRows = table.CreateSet<ZustandUndNetzinformationenEditRow>();
            var row = zustandUndNetzinformationenEditRows.Single();

            var belastungskategorieId = GetBelastungskategorieIdByTyp(row.Belastungskategorie);

            BrowserDriver.InvokeGetAction<StrassenmengeUndZustandController>(c => c.GetAll(dataSourceRequest));
            var gridModel = BrowserDriver.GetCurrentModel<SerializableDataSourceResult>();

            var summarischDetailModel = gridModel.Data.Cast<NetzSummarischDetailModel>()
                .Single(m => m.Belastungskategorie == belastungskategorieId);
            summarischDetailModel.MittlererZustand = row.MittlererZustandValue;

            summarischDetailModel.Fahrbahnflaeche = (int)row.Menge;
            summarischDetailModel.Fahrbahnlaenge = row.Fahrbahnlaenge;

            BrowserDriver.InvokePostAction<StrassenmengeUndZustandController, NetzSummarischDetailModel>((c, r) => c.Update(r), summarischDetailModel);
        }

        private Guid GetBelastungskategorieIdByTyp(string belastungskategorieTyp)
        {
            using (var nhScope = new NHibernateSpecflowScope()) { return nhScope.GetBelastungskategorie(belastungskategorieTyp).Id; }
        }

        [Then(@"'(.+)' liefert einen Validationsfehler '(.+)'")]
        public void DannMittlererZustandHatEinenValidationsfehlerNein(string fieldName, string hatValidationFehler)
        {
            bool hasFieldValidationError = false;

            if(!BrowserDriver.IsLastResultEmptyResult())
            {
                var modelStateDictionary = BrowserDriver.GetCurrentModelState();
                hasFieldValidationError = !modelStateDictionary.IsValid && modelStateDictionary[GetNetzSummarischModelPropertyName(fieldName)].Errors.Any();
            }

            Assert.AreEqual(hatValidationFehler.ParseBool(), hasFieldValidationError);
        }

        private static string GetNetzSummarischModelPropertyName(string fieldName)
        {
            string propertyName;
            switch (fieldName.ToLower())
            {
                case "mittlerer zustand":
                    propertyName = ExpressionHelper.GetPropertyName<NetzSummarischDetailModel, decimal?>(m => m.MittlererZustand);
                    break;
                case "gesamtlänge menge":
                    propertyName = ExpressionHelper.GetPropertyName<NetzSummarischDetailModel, decimal>(m => m.Fahrbahnlaenge);
                    break;
                case "gesamtfläche menge":
                    propertyName = ExpressionHelper.GetPropertyName<NetzSummarischDetailModel, int>(m => m.Fahrbahnflaeche);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(fieldName);
            }
            return propertyName;
        }

        [Then(@"sind folgende summarische Zustands- und Netzinformationen im System:")]
        public void DannSindFolgendeSummarischeZustandsUndNetzinformationenImSystem(Table table)
        {
            using (var nhScope = new NHibernateSpecflowScope())
            {
                var row = table.CreateSet<NetzSummarischRow>().Single();

                var netzSummarischen = nhScope.Session.Query<NetzSummarischDetail>();

                if (row.Belastungskategorie.IsNotNullOrEmpty())
                    netzSummarischen = netzSummarischen.Where(ns => ns.Belastungskategorie.Id == GetBelastungskategorieIdByTyp(row.Belastungskategorie));

                if (row.MittlererZustand.IsNotNullOrEmpty())
                    netzSummarischen = netzSummarischen.Where(ns => ns.MittlererZustand == row.MittlererZustandValue);

                if (row.MengeGesamtlänge.IsNotNullOrEmpty())
                    netzSummarischen = netzSummarischen.Where(ns => ns.Fahrbahnlaenge == row.MengeGesamtlangeValue);

                if (row.MengeGesamtfläche.IsNotNullOrEmpty())
                    netzSummarischen = netzSummarischen.Where(ns => ns.Fahrbahnflaeche == row.MengeGesamtflacheValue);

                int count;
                if (row.Mandant.IsNotNullOrEmpty())
                    count = netzSummarischen.ToList().Count(ns => ns.Mandant.MandantName == row.Mandant);
                else
                    count = netzSummarischen.Count();

                Assert.AreEqual(1, count);
            }
        }

        [When(@"ich Mittleres Alter '(.+)' eingebe")]
        public void WennIchMittleresAlterEingebe(string mittleresAlter)
        {
            BrowserDriver.InvokePostAction<StrassenmengeUndZustandController, ActionResult>((c, r) => c.Index(), null);

            var netzSummarischModel = new NetzSummarischModel();

            using (var nh = new NHibernateSpecflowScope())
            {
                NetzSummarisch netzSummarisch = nh.Session.Query<NetzSummarisch>().Single();
                netzSummarischModel.Id = netzSummarisch.Id;
            }

            netzSummarischModel.MittleresErhebungsJahr = mittleresAlter.ParseNullableDateTime();

            BrowserDriver.InvokePostAction<StrassenmengeUndZustandController, NetzSummarischModel>((c, r) => c.SaveMittleresErhebungsjahr(r), netzSummarischModel);
        }

        [Then(@"Mittleres Alter hat einen Validationsfehler '(.+)'")]
        public void DannMittleresAlterHatEinenValidationsfehlerNein(string hasValidationError)
        {
            var modelStateDictionary = BrowserDriver.GetCurrentModelState();
            Assert.AreEqual(hasValidationError.ParseBool(), modelStateDictionary != null && !modelStateDictionary.IsValid);
        }

        [Then(@"sind folgende Mittlere Alter im System:")]
        public void DannSindFolgendeMittlereAlterImSystem(Table table)
        {
            if (HasFieldValidationError())
                return;

            var row = table.CreateSet<MittleresAlterRow>().Single();

            using (var nh = new NHibernateSpecflowScope())
            {
                var mittleresErhebungsjahr = nh.Session.Query<NetzSummarisch>().Single();

                if(!string.IsNullOrEmpty(row.Mandant))
                    Assert.AreEqual(row.Mandant, mittleresErhebungsjahr.Mandant.MandantName);

                Assert.AreEqual(row.MittleresAlterValue, mittleresErhebungsjahr.MittleresErhebungsJahr);
            }
        }
    }

    public class MittleresAlterRow
    {
        public string Mandant { get; set; }
        public string MittleresAlter { get; set; }
        public DateTime? MittleresAlterValue { get { return MittleresAlter.ParseNullableDateTime(); } }
    }

    public class NetzSummarischRow
    {
        public string Mandant { get; set; }
        public string Belastungskategorie { get; set; }
        public string MittlererZustand { get; set; }
        public decimal? MittlererZustandValue { get { return MittlererZustand.ParseNullableDecimal(); } }
        public string MengeGesamtlänge { get; set; }
        public decimal? MengeGesamtlangeValue { get { return MengeGesamtlänge.ParseNullableDecimal(); } }
        public string MengeGesamtfläche { get; set; }
        public decimal? MengeGesamtflacheValue { get { return MengeGesamtfläche.ParseNullableDecimal(); } }
    }

    public class EinstellungenRow
    {
        public string Mandant { get; set; }
        public string Modus { get; set; }
    }

    public class ZustandUndNetzinformationenEditRow
    {
        public string Belastungskategorie { get; set; }
        public string MittlererZustand { get; set; }
        public decimal? MittlererZustandValue { get { return MittlererZustand.ParseNullableDecimal(); } }
        public decimal Menge { get; set; }
        public decimal Fahrbahnlaenge { get; set; }
    }
}
