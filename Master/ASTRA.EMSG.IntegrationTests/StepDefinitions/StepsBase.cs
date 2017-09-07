using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.IntegrationTests.Support;
using ASTRA.EMSG.IntegrationTests.Support.MvcTesting;
using ASTRA.EMSG.IntegrationTests.Support.ObjectReader;
using NHibernate.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace ASTRA.EMSG.IntegrationTests.StepDefinitions
{
    public class StepsBase
    {
        public BrowserDriver BrowserDriver { get; private set; }

        public StepsBase(BrowserDriver browserDriver)
        {
            BrowserDriver = browserDriver;
        }
        
        public void AssertValidationFehler(bool hasValidationError, string propertyName)
        {
            Assert.AreEqual(hasValidationError, HasFieldValidationError(propertyName));
        }
        
        public void AssertValidationFehler(bool hasValidationError)
        {
            Assert.AreEqual(hasValidationError, HasFieldValidationError());
        }

        public bool HasFieldValidationError(string propertyName)
        {
            var modelStateDictionary = BrowserDriver.GetCurrentModelState();
            if (modelStateDictionary.IsValid)
                return false;

            return modelStateDictionary[propertyName].Errors.Any();
        }
        
        public bool HasFieldValidationError()
        {
            var modelStateDictionary = BrowserDriver.GetCurrentModelState();
            
            if(modelStateDictionary == null)
                return false;

            if (modelStateDictionary.IsValid)
                return false;

            return true;
        }

        protected TObject GetObject<TObject>(Table table) where TObject : new()
        {
            return GetObjectReader().GetObject<TObject>(table);
        }

        protected TObject GetObject<TObject>(Table table, TObject obj) where TObject : new()
        {
            return GetObjectReader().GetObject(table, obj);
        }

        protected List<TObject> GetObjectList<TObject>(Table table) where TObject : new()
        {
            return GetObjectReader().GetObjectList<TObject>(table);
        }

        protected ObjectReader GetObjectReader()
        {
            return new ObjectReader(GetObjectReaderConfiguration());
        }

        protected IObjectReaderConfiguration GetObjectReaderConfiguration()
        {
            var objectReaderConfiguration = new ObjectReaderConfiguration()
                .ConverterFor(typeof(string), (s, pi) => s.IsNull() ? null : s)
                .ConverterFor(typeof(Guid), (s, pi) => s.ParseGuid())
                .ConverterFor(typeof(Guid?), (s, pi) => s.ParseNullableGuid())
                .ConverterFor(typeof(DateTime), (s, pi) => s.ParseDateTime())
                .ConverterFor(typeof(DateTime?), (s, pi) => s.ParseNullableDateTime())
                .ConverterFor(typeof(decimal), (s, pi) => s.ParseDecimal())
                .ConverterFor(typeof(decimal?), (s, pi) => s.ParseNullableDecimal())
                .ConverterFor(typeof(int), (s, pi) => s.ParseInt())
                .ConverterFor(typeof(int?), (s, pi) => s.ParseNullableInt())
                .ConverterFor(typeof(bool), (s, pi) => s.ParseBool())
                .ConverterFor(typeof(Enum), (s, pi) => s.IsNull() ? null : Enum.Parse(pi.PropertyType, s));

            objectReaderConfiguration.SetGenericPropertyNameResolver(header => header.Replace("ä", "ae").Replace("ü", "ue").Replace("ö", "oe"));

            return objectReaderConfiguration;
        }

        protected IObjectReaderConfigurationForType<TObject> GetObjectReaderConfigurationFor<TObject>()
        {
            return GetObjectReaderConfiguration().ConfigurationFor<TObject>();
        }

        protected object ConvertId(string s, Dictionary<int, Guid> idDictionary)
        {
            var id = s.ParseInt();
            Guid guid;
            if (!idDictionary.TryGetValue(id, out guid))
            {
                guid = Guid.NewGuid();
                idDictionary.Add(id, guid);
            }

            return guid;
        }

        protected Belastungskategorie ConvertBelastungskategorie(string s)
        {
            return s.IsNull() ? null : ScenarioContextWrapper.CurrentScope.Session.Query<Belastungskategorie>().Single(bk => bk.Typ == s);
        }

        protected MassnahmenvorschlagKatalog ConvertMassnahmenvorschlagKatalog(string s, Mandant currentMandant, Belastungskategorie belastungskategorie)
        {
            var currentPeriod = ScenarioContextWrapper.CurrentScope.Session.Query<ErfassungsPeriod>().Single( e => e.Mandant == currentMandant && !e.IsClosed);
            return s.IsNull() ? null : ScenarioContextWrapper.CurrentScope.Session.Query<MassnahmenvorschlagKatalog>().SingleOrDefault(mk => mk.ErfassungsPeriod == currentPeriod && mk.Belastungskategorie == belastungskategorie && mk.Parent.Typ == s);
        }
        
        protected Guid? ConvertBelastungskategorieId(string s, PropertyInfo pi)
        {
            var belastungskategorie = ConvertBelastungskategorie(s);
            return belastungskategorie == null ? (Guid?)null : belastungskategorie.Id;
        }
        
        protected NetzErfassungsmodus GetModus(string modusString)
        {
            modusString = modusString.ToLower();
            switch (modusString)
            {
                case "summarisch": return NetzErfassungsmodus.Summarisch;
                case "strassennamen": return NetzErfassungsmodus.Tabellarisch;
                case "gis": return NetzErfassungsmodus.Gis;
                case "": return NetzErfassungsmodus.Summarisch;
                case null: return NetzErfassungsmodus.Summarisch;
                default: throw new ArgumentOutOfRangeException(modusString);
            }
        }
    }
}