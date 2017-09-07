using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using NHibernate;
using NHibernate.Criterion;
using ASTRA.EMSG.Business.Services.GIS;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries.MGeometries;
using ASTRA.EMSG.Business.Interlis.AxisImport;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Infrastructure.MappingRules;
using ASTRA.EMSG.Common.Master.Logging;

namespace ASTRA.EMSG.Business.AchsenUpdate
{
    public class AchsenUpdate
    {
        protected readonly ISession session;
        private readonly Mandant mandant;
        private readonly ErfassungsPeriod erfassungsPeriod;
        private readonly string owner;
        private readonly int lastImpNr;
        private readonly IAchsKopieMappingEngine gisKopieMappingEngine;
        public IReferenceUpdater ReferenceUpdater { get; set; }

        private AchsenUpdateStatistics statistics = new AchsenUpdateStatistics();

        private readonly IGeometryFactory gf = GISService.CreateGeometryFactory();

        private Dictionary<KopieAchse, HashSet<KopieAchsenSegment>> kopieAchsenToSegmentDict = new Dictionary<KopieAchse,HashSet<KopieAchsenSegment>>();
        private Dictionary<KopieAchsenSegment, HashSet<KopieSektor>> kopieSegmentToSektorDict = new Dictionary<KopieAchsenSegment, HashSet<KopieSektor>>();

        private Dictionary<Guid, Achse> achsenCache = new Dictionary<Guid, Achse>();

        private int maxImpNr;

        public AchsenUpdate(ISession session, Mandant mandant, ErfassungsPeriod erfassungsPeriod, string owner, int lastImpNr)
        {
            this.session = session;
            this.mandant = mandant;
            this.erfassungsPeriod = erfassungsPeriod;
            this.owner = owner;
            this.lastImpNr = lastImpNr;

            this.maxImpNr = lastImpNr;
            this.gisKopieMappingEngine = new GISKopieMappingEngine(new GISKopieMappingConfiguration(new IgnoreIdMappingRule(),new MLineStringTo2DMappingRule()));
        }

        private void ClearConflicts()
        {
            var AchsenUpdateConflictTypeName = typeof(AchsenUpdateConflict).Name;
            var mandantIdPropertyName = ExpressionHelper.GetPropertyName<Mandant, Guid>(m => m.Id);
            var mandantTypeName = typeof(Mandant).Name;
            var erfassungsPeriodIdPropertyName = ExpressionHelper.GetPropertyName<ErfassungsPeriod, Guid>(e => e.Id);
            var erfassungsPeriodTypeName = typeof(ErfassungsPeriod).Name;

            var qry = session.CreateQuery(string.Format("delete {0} where {1}.{2} = :mandant and {3}.{4} = :erfPeriod", new string[] { AchsenUpdateConflictTypeName, mandantTypeName, mandantIdPropertyName, erfassungsPeriodTypeName, erfassungsPeriodIdPropertyName }));
            
            qry.SetParameter("mandant", mandant.Id);
            qry.SetParameter("erfPeriod", erfassungsPeriod.Id);
            
            qry.ExecuteUpdate();
        }

        private void ReadAchsKopieData()
        {
            // HQL doesnt support left joins for foreign keys that aren't mapped
            // therefore we need three queries

            {
                var list = session.QueryOver<KopieAchse>().Where(ka => ka.Owner == owner).List();

                foreach (object obj in list)
                {
                    KopieAchse achse = (KopieAchse)obj;

                    if (!kopieAchsenToSegmentDict.ContainsKey(achse))
                    {
                        kopieAchsenToSegmentDict.Add(achse, new HashSet<KopieAchsenSegment>() { });
                    }
                    else
                    {
                    }
                }
            }

            {
                var kopieAchseTypeName = typeof(KopieAchse).Name;
                var kopieAchsenSegmentTypeName = typeof(KopieAchsenSegment).Name;
                var kopieAchseIdPropertyName = ExpressionHelper.GetPropertyName<KopieAchse, Guid>(ka => ka.Id);
                var kopieAchsenSegmentAchsenIdPropertyName = ExpressionHelper.GetPropertyName<KopieAchsenSegment, Guid>(kas => kas.AchsenId);
                var kopieAchseOwnerPropertyName = ExpressionHelper.GetPropertyName<KopieAchse, string>(ka => ka.Owner);

                var qry = session.CreateQuery(string.Format("from {0} achse, {1} segment where achse.{2} = segment.{3} and achse.{4} = :owner",
                    new string[] { kopieAchseTypeName, kopieAchsenSegmentTypeName, kopieAchseIdPropertyName, kopieAchsenSegmentAchsenIdPropertyName, kopieAchseOwnerPropertyName }));
                qry.SetString("owner", owner);
                qry.SetReadOnly(true);
                var list = qry.List();

                foreach (object arr in list)
                {
                    KopieAchse achse = (KopieAchse)((object[])arr)[0];
                    KopieAchsenSegment achsenSegment = (KopieAchsenSegment)((object[])arr)[1];

                    if (!kopieAchsenToSegmentDict.ContainsKey(achse))
                    {
                        kopieAchsenToSegmentDict.Add(achse, new HashSet<KopieAchsenSegment>() { achsenSegment });
                    }
                    else
                    {
                        kopieAchsenToSegmentDict[achse].Add(achsenSegment);
                    }

                    if (!kopieSegmentToSektorDict.ContainsKey(achsenSegment))
                    {
                        kopieSegmentToSektorDict.Add(achsenSegment, new HashSet<KopieSektor>() { });
                    }

                }
            }


            {
                var kopieAchseTypeName = typeof(KopieAchse).Name;
                var kopieAchsenSegmentTypeName = typeof(KopieAchsenSegment).Name;
                var kopieSektorTypeName = typeof(KopieSektor).Name;
                var kopieAchseIdPropertyName = ExpressionHelper.GetPropertyName<KopieAchse, Guid>(ka => ka.Id);
                var kopieAchsenSegmentAchsenIdPropertyName = ExpressionHelper.GetPropertyName<KopieAchsenSegment, Guid>(kas => kas.AchsenId);
                var kopieAchsenSegmentIdPropertyName = ExpressionHelper.GetPropertyName<KopieAchsenSegment, Guid>(kas => kas.Id);
                var kopieAchsenSektorSegmentIdPropertyName = ExpressionHelper.GetPropertyName<KopieSektor, Guid>(ks => ks.SegmentId);
                var kopieAchseOwnerPropertyName = ExpressionHelper.GetPropertyName<KopieAchse, string>(ka => ka.Owner);

                var qry = session.CreateQuery(string.Format("from {0} achse, {1} segment, {2} sektor where achse.{3} = segment.{4} and segment.{5} = sektor.{6} and achse.{7} = :owner",
                    new string[] { kopieAchseTypeName, kopieAchsenSegmentTypeName, kopieSektorTypeName, 
                        kopieAchseIdPropertyName, kopieAchsenSegmentAchsenIdPropertyName, kopieAchsenSegmentIdPropertyName,
                        kopieAchsenSektorSegmentIdPropertyName, kopieAchseOwnerPropertyName}));
                qry.SetString("owner", owner);
                qry.SetReadOnly(true);
                var list = qry.List();

                foreach (object arr in list)
                {
                    KopieAchse achse = (KopieAchse)((object[])arr)[0];
                    KopieAchsenSegment achsenSegment = (KopieAchsenSegment)((object[])arr)[1];
                    KopieSektor sektor = (KopieSektor)((object[])arr)[2];

                    if (!kopieAchsenToSegmentDict.ContainsKey(achse))
                    {
                        kopieAchsenToSegmentDict.Add(achse, new HashSet<KopieAchsenSegment>() { achsenSegment });
                    }
                    else
                    {
                        kopieAchsenToSegmentDict[achse].Add(achsenSegment);
                    }


                    if (!kopieSegmentToSektorDict.ContainsKey(achsenSegment))
                    {
                        kopieSegmentToSektorDict.Add(achsenSegment, new HashSet<KopieSektor>() { sektor });
                    }
                    else
                    {
                        kopieSegmentToSektorDict[achsenSegment].Add(sektor);
                    }

                }
            }
        }


        public virtual void Start()
        {
            ClearConflicts();
            ReadAchsKopieData();
            fillAchsenCache();
            if (ReferenceUpdater != null)
            {
                ReferenceUpdater.CreateValidFromDict(achsenCache.Values);
            }
            foreach (KeyValuePair<KopieAchse, HashSet<KopieAchsenSegment>> pair in kopieAchsenToSegmentDict)
            {
                KopieAchse kopieAchse = pair.Key;

                UpdateAchse(kopieAchse, kopieAchse.Operation);
            }

            if (ReferenceUpdater != null) ReferenceUpdater.PostWork();
        }

        private void fillAchsenCache()
        {
            var list = session.QueryOver<Achse>().Where(a => a.Mandant == mandant && a.ErfassungsPeriod == erfassungsPeriod).List();

            foreach (Achse achse in list)
            {
                achsenCache.Add(achse.BsId, achse);
            }
        }


        private Achse lookupAchseByBSID(Guid bsId)
        {
            if (!achsenCache.ContainsKey(bsId))
                return null;

            return achsenCache[bsId];
        }

        private Achse UpdateAchse(KopieAchse kopieAchse, int operation)
        {
            if (kopieAchse.ImpNr <= lastImpNr) return null;

            maxImpNr = Math.Max(maxImpNr, kopieAchse.ImpNr);

            switch (operation)
            {
                case AxisImportOperation.DELETE:
                    {
                        Achse achse = lookupAchseByBSID(kopieAchse.Id);
                        if (achse == null)
                        {
                            Loggers.TechLogger.Warn(String.Format("AchsenUpdate Delete: Achse {0} not found",kopieAchse.Id));
                            return null;
                        }

                        if (ReferenceUpdater != null) ReferenceUpdater.BeforeDeleteAchse(achse);


                        foreach (KopieAchsenSegment kopieSegment in kopieAchsenToSegmentDict[kopieAchse])
                        {
                            UpdateSegment(achse, kopieAchse, kopieSegment, kopieSegment.Operation); 
                        }

                        session.Delete(achse);

                        statistics.NumAchsen.Deletes++;
                        return null;
                    }
                case AxisImportOperation.UPDATE:
                    {
                        Achse achse = lookupAchseByBSID(kopieAchse.Id);
                        if (achse == null)
                        {
                            return UpdateAchse(kopieAchse, AxisImportOperation.INSERT);
                        }

                        if (ReferenceUpdater != null) ReferenceUpdater.BeforeUpdateAchse(achse, kopieAchse);
                        achse = gisKopieMappingEngine.Translate<KopieAchse, Achse>(kopieAchse, achse);
                      

                        if (achse.Mandant != mandant)
                        {
                            throw new Exception("achse.Mandant != mandant");
                        }

                        foreach (KopieAchsenSegment kopieSegment in kopieAchsenToSegmentDict[kopieAchse])
                        {
                            UpdateSegment(achse, kopieAchse, kopieSegment, kopieSegment.Operation);
                        }

                        statistics.NumAchsen.Updates++;
                        return achse;
                    }
                case AxisImportOperation.INSERT:
                    {
                        if (lookupAchseByBSID(kopieAchse.Id) != null)
                            throw new Exception("Cannot insert: KopieAchse " + kopieAchse.Id + " already exists in Achsen table!");

                        Achse achse = gisKopieMappingEngine.Translate<KopieAchse, Achse>(kopieAchse);
                        
                        achse.BsId = kopieAchse.Id;
                        achse.Mandant = mandant;
                        achse.ErfassungsPeriod = erfassungsPeriod;

                        foreach (KopieAchsenSegment kopieSegment in kopieAchsenToSegmentDict[kopieAchse])
                        {
                            UpdateSegment(achse, kopieAchse, kopieSegment, kopieSegment.Operation);
                        }

                        session.Persist(achse);

                        statistics.NumAchsen.Inserts++;
                        return achse;
                    }
            }
            return null;
        }

        private AchsenSegment findSegment(Achse achse, KopieAchsenSegment kopieSegment)
        {           
            var list = achse.AchsenSegmente.Where(o => o.BsId == kopieSegment.Id);
            if (list.Count() == 0)
            {
                list = achsenCache.SelectMany(a => a.Value.AchsenSegmente).Where(s => s.BsId == kopieSegment.Id);
                if (!list.Any())
                {
                    return null;
                }
            }                

            return list.First();
        }

        private AchsenSegment UpdateSegment(Achse achse, KopieAchse kopieAchse, KopieAchsenSegment kopieSegment,  int operation)
        {
            if (kopieSegment.ImpNr <= lastImpNr) return null;

            maxImpNr = Math.Max(maxImpNr, kopieSegment.ImpNr);

            switch (operation)
            {
                case AxisImportOperation.DELETE:
                    {

                        AchsenSegment segment =
                            findSegment(achse, kopieSegment);

                        if (segment == null)
                            return null;

                        if (ReferenceUpdater != null) ReferenceUpdater.BeforeDeleteSegment(segment);

                        foreach (KopieSektor kopieSektor in kopieSegmentToSektorDict[kopieSegment])
                        {
                            UpdateSektor(segment, kopieSektor, kopieSektor.Operation);
                        }

                        achse.AchsenSegmente.Remove(segment);
                        session.Delete(segment);

                        statistics.NumSegment.Deletes++;
                        return null;
                    }
                case AxisImportOperation.UPDATE:
                    {

                        AchsenSegment segment = 
                            findSegment(achse, kopieSegment);

                        if (segment == null)
                        {
                            return UpdateSegment(achse, kopieAchse, kopieSegment, AxisImportOperation.INSERT);
                        }

                        if (ReferenceUpdater != null) ReferenceUpdater.BeforeUpdateSegment(segment, kopieAchse, kopieSegment);

                        segment = gisKopieMappingEngine.Translate<KopieAchsenSegment, AchsenSegment>(kopieSegment, segment);
                        segment.Shape4d = kopieSegment.Shape;

                        if (segment.Mandant != mandant)
                        {
                            throw new Exception("segment.Mandant != mandant");
                        }

                        foreach (KopieSektor kopieSektor in kopieSegmentToSektorDict[kopieSegment])
                        {
                            UpdateSektor(segment, kopieSektor, kopieSektor.Operation);
                        }

                        statistics.NumSegment.Updates++;
                        return segment;
                    }
                case AxisImportOperation.INSERT:
                    {
                        if (findSegment(achse, kopieSegment) != null)
                            throw new Exception("Cannot insert: KopieSegment " + kopieSegment.Id + " already exists in AchsenSegment table!");

                        AchsenSegment segment = gisKopieMappingEngine.Translate<KopieAchsenSegment, AchsenSegment>(kopieSegment);
                        segment.BsId = kopieSegment.Id;

                        segment.Shape4d = kopieSegment.Shape;

                        segment.Mandant = mandant; 
                        segment.ErfassungsPeriod = erfassungsPeriod;

                        segment.Achse = achse;
                        achse.AchsenSegmente.Add(segment);

                        foreach (KopieSektor kopieSektor in kopieSegmentToSektorDict[kopieSegment])
                        {
                            UpdateSektor(segment, kopieSektor, kopieSektor.Operation);
                        }

                        if (ReferenceUpdater != null) ReferenceUpdater.AfterCreateSegment(segment);

                        statistics.NumSegment.Inserts++;
                        return segment;
                    }
            }
            return null;
        }

        private Sektor findSektor(AchsenSegment achsenSegment, KopieSektor kopieSektor)
        {           
            var list = achsenSegment.Sektoren.Where(o => o.BsId == kopieSektor.Id);
            if (list.Count() == 0)
            {
                list = achsenCache.SelectMany(a => a.Value.AchsenSegmente).SelectMany(s => s.Sektoren).Where(se => se.BsId == kopieSektor.Id);
                if (!list.Any())
                {
                    return null;
                }
            }
            return list.First();
        }

        private Sektor UpdateSektor(AchsenSegment achsenSegment, KopieSektor kopieSektor, int operation)
        {
            if (kopieSektor.ImpNr <= lastImpNr) return null;

            maxImpNr = Math.Max(maxImpNr, kopieSektor.ImpNr);

            switch (operation)
            {
                case AxisImportOperation.DELETE:
                    {

                        Sektor sektor =
                            findSektor(achsenSegment, kopieSektor);

                        if (sektor == null)
                            return null;

                        achsenSegment.Sektoren.Remove(sektor);
                        session.Delete(sektor);

                        statistics.NumSector.Deletes++;
                        return null;
                    }
                case AxisImportOperation.UPDATE:
                    {

                        Sektor sektor =
                            findSektor(achsenSegment, kopieSektor);

                        if (sektor == null)
                        {
                            return UpdateSektor(achsenSegment, kopieSektor, AxisImportOperation.INSERT);
                        }
                        sektor = gisKopieMappingEngine.Translate<KopieSektor, Sektor>(kopieSektor, sektor);

                        statistics.NumSector.Updates++;
                        return sektor;
                    }
                case AxisImportOperation.INSERT:
                    {
                        if (findSektor(achsenSegment, kopieSektor) != null)
                            throw new Exception("Cannot insert: KopieSektor " + kopieSektor.Id + " already exists in Sektor Table!");

                        Sektor sektor =  gisKopieMappingEngine.Translate<KopieSektor, Sektor>(kopieSektor);
                        sektor.BsId = kopieSektor.Id;
                        sektor.AchsenSegment = achsenSegment;
                        achsenSegment.Sektoren.Add(sektor);

                        statistics.NumSector.Inserts++;
                        return sektor;
                    }
            }
            return null;
        }

        public Mandant Mandant { get { return mandant; } }

        public ErfassungsPeriod ErfassungsPeriod { get { return erfassungsPeriod; } }

        public int LastImpNr { get { return lastImpNr; } }

        public int MaxImpNr { get { return maxImpNr; } }

        public AchsenUpdateStatistics Statistics
        {
            get
            {
                return statistics;
            }
        }
    }
}
