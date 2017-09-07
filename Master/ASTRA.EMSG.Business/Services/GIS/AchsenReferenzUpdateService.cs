using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.AchsenUpdate.UpdateReferences;
using GeoAPI.Geometries;
using NHibernate;
using ASTRA.EMSG.Business.Services.Common;
using GeoJSON;
using System.IO;
using ASTRA.EMSG.Common.Master.Logging;

namespace ASTRA.EMSG.Business.Services.GIS
{
    public interface IAchsenReferenzUpdateService : IService
    {
        IList<Guid> UpdateAchsenReferenzen(AchsenSegmentModel newmodel, bool persistChanges);
        IList<Guid> GetModifiedEntities(Guid id, IGeometry shape);
        IList<Guid> GetModifiedEntities(string geoJson);
        IList<Guid> GetModifiedEntities(AchsenSegmentModel model);
        void DeleteAchsenReferenzen(AchsenSegment segment);
        void DeleteAchsenReferenzen(AchsenSegmentModel model);
        void DeleteAchsenReferenzen(Guid id);
    }
    public class AchsenReferenzUpdateService : IAchsenReferenzUpdateService, IService
    {
        
        private readonly ITransactionScopeProvider transactionScopeProvider;
        private readonly IAchsenSegmentService achsenSegmentService;
        private readonly IGISService gisService;

        public AchsenReferenzUpdateService(ITransactionScopeProvider transactionScopeProvider, IAchsenSegmentService achsenSegmentService, IGISService gisService)
        {
            this.transactionScopeProvider = transactionScopeProvider;
            this.achsenSegmentService = achsenSegmentService;
            this.gisService = gisService;
        }

        public IList<Guid> GetModifiedEntities(AchsenSegmentModel model) 
        {
            AchsenSegment entity = this.achsenSegmentService.GetCurrentEntities().Where(e => e.Id == model.Id).Single();
            return this.GetModifiedEntities(entity, model);
        }

        public IList<Guid> GetModifiedEntities(AchsenSegment currententity, AchsenSegmentModel newmodel)
        {
            return this.UpdateAchsenReferenzen(currententity, newmodel.Shape, false);
        }

        public IList<Guid> GetModifiedEntities(string geoJson) 
        {
            FeatureWithID feature = GeoJSONReader.ReadFeatureWithID(new StringReader(geoJson));
            if (String.IsNullOrEmpty(feature.Id)) 
            {
                return new List<Guid>();
            }
            return this.GetModifiedEntities(Guid.Parse(feature.Id), feature.Geometry);
        }

        public IList<Guid> GetModifiedEntities(Guid id, IGeometry shape) 
        {
            AchsenSegment entity = this.achsenSegmentService.GetCurrentEntities().Where(e => e.Id == id).Single();
            return this.UpdateAchsenReferenzen(entity, shape, false);
        }

        public IList<Guid> UpdateAchsenReferenzen(AchsenSegmentModel newmodel, bool persistChanges)
        {
            AchsenSegment entity = this.achsenSegmentService.GetCurrentEntities().Where(e => e.Id == newmodel.Id).Single();
            return this.UpdateAchsenReferenzen(entity, newmodel.Shape, persistChanges, newmodel.ModificationAction);
        }

        private IList<Guid> UpdateAchsenReferenzen(AchsenSegment currententity, IGeometry shape, bool persistChanges, AchseModificationAction modificationAction = AchseModificationAction.Change)
        {
            Dictionary<AchsenReferenz, GeometryTransformerResult> ret = new Dictionary<AchsenReferenz, GeometryTransformerResult>();
            foreach (AchsenReferenz referenz in currententity.AchsenReferenzen.ToList())
            {
                ILineString oldRefGeometry = (ILineString)referenz.Shape;
                ILineString newSegmentGeometry = (ILineString)shape; 
                GeometryTransformer gt = new GeometryTransformer(oldRefGeometry, newSegmentGeometry);

                GeometryTransformerResult result = gt.Transform();
                if (result.ResultState != GeometryTransformerResultState.Success || !result.NewRefGeometry.EqualsExact(oldRefGeometry))
                {
                    ret.Add(referenz, result);
                }
            }
            //check if strabs are overlapping other strabs
            foreach (var pair in ret.Where(p => p.Key.ReferenzGruppe.StrassenabschnittGIS != null && p.Value.ResultState == GeometryTransformerResultState.Success).ToList()) 
            {
                IEnumerable<AchsenReferenz> strabsar = pair.Key.AchsenSegment.AchsenReferenzen.Where(ar => ar.Id != pair.Key.Id && ar.ReferenzGruppe.StrassenabschnittGIS != null);

                foreach (AchsenReferenz strabref in strabsar)
                {
                    IGeometry currentstrabref = strabref.Shape;
                    bool exists = true;
                    KeyValuePair<AchsenReferenz, GeometryTransformerResult> newZabref = ret.Where(p => p.Key.Id.Equals(strabref.Id)).SingleOrDefault();
                    if (!newZabref.Equals(default(KeyValuePair<AchsenReferenz, GeometryTransformerResult>)))
                    {
                        currentstrabref = newZabref.Value.NewRefGeometry;
                        exists = newZabref.Value.ResultState == GeometryTransformerResultState.Success;
                    }
                    if (exists)
                    {
                        if (!gisService.CheckOverlapp(new List<IGeometry>() { currentstrabref }, pair.Value.NewRefGeometry))
                        {
                            ret[pair.Key] = new GeometryTransformerResult(GeometryTransformerResultState.FailedWouldBeOutside, pair.Value.NewRefGeometry);
                        }
                    }
                }
            }

            //check if zabs still within strabs and update results if not, ensure zabs are not overlapping others
            foreach (var pair in ret.Where(p => p.Key.ReferenzGruppe.ZustandsabschnittGIS != null && p.Value.ResultState == GeometryTransformerResultState.Success).ToList()) {
                ZustandsabschnittGIS zab = pair.Key.ReferenzGruppe.ZustandsabschnittGIS;
                if (zab != null && pair.Value.ResultState == GeometryTransformerResultState.Success)
                {
                    IEnumerable<AchsenReferenz> strabrefs = ret.Where(p => p.Key.ReferenzGruppe.StrassenabschnittGIS != null 
                        && p.Key.ReferenzGruppe.StrassenabschnittGIS.Id == zab.StrassenabschnittGIS.Id 
                        && p.Value.ResultState == GeometryTransformerResultState.Success).Select(p => p.Key).ToList();

                    IList<IGeometry> zabrefs = new List<IGeometry>() { pair.Value.NewRefGeometry };
                    foreach(AchsenReferenz achsref in strabrefs)
                    { 
                        bool isWithin = false;
                        if(ret[achsref].ResultState == GeometryTransformerResultState.Success)
                        {
                            isWithin = isWithin || gisService.CheckGeometriesIsInControlGeometry(zabrefs, ret[achsref].NewRefGeometry);
                        }
                        if (!isWithin)
                        {
                            ret[pair.Key] = new GeometryTransformerResult(GeometryTransformerResultState.FailedWouldBeOutside, pair.Value.NewRefGeometry);
                        }
                    }
                    IEnumerable<AchsenReferenz> allzabrefs = zab.StrassenabschnittGIS.Zustandsabschnitten.SelectMany(z => z.ReferenzGruppe.AchsenReferenzen).Where(ar => !ar.Id.Equals(pair.Key.Id));
                    foreach (AchsenReferenz zabref in allzabrefs)
                    {
                        IGeometry currentzabref = zabref.Shape;
                        bool exists = true;
                        KeyValuePair<AchsenReferenz, GeometryTransformerResult> newZabref = ret.Where(p => p.Key.Id.Equals(zabref.Id)).SingleOrDefault();
                        if (!newZabref.Equals(default(KeyValuePair<AchsenReferenz, GeometryTransformerResult>))) 
                        {
                            currentzabref = newZabref.Value.NewRefGeometry;
                            exists = newZabref.Value.ResultState == GeometryTransformerResultState.Success;
                        }
                        if (exists)
                        {
                            if (!gisService.CheckOverlapp(new List<IGeometry>() {currentzabref }, pair.Value.NewRefGeometry))
                            {
                                ret[pair.Key] = new GeometryTransformerResult(GeometryTransformerResultState.FailedWouldBeOutside, pair.Value.NewRefGeometry);
                            }
                        }
                    }
                }
            }

            if (persistChanges)
            {
                foreach (var pair in ret)
                {
                    this.UpdateAchsenReferenz(pair.Key, pair.Value, modificationAction == AchseModificationAction.Delete);
                }
            }
            return ret.Keys.Select(k => k.Id).ToList();
        }
        /// <summary>
        /// Update a Achsenreferenz using a <code>GeometryTransformerResult</code>
        /// </summary>
        /// <param name="referenz">Achsenreferenz to update</param>
        /// <param name="result">Result of <seealso cref='GeometryTransformer.Transform()' />GeometryTransformer.Transform()</param>
        /// <param name="doDelete">Always delete Achsenreferenz even on <code>GeometryTransformerResultState.Success</code></param>
        private void UpdateAchsenReferenz(AchsenReferenz referenz, GeometryTransformerResult result, bool doDelete)
        {
            switch (result.ResultState)
            {
                case GeometryTransformerResultState.Success:
                    if (doDelete)
                    {
                        this.DeleteAchsenReferenz(referenz);
                    }
                    else 
                    {
                        this.UpdateAchsenReferenz(referenz, result.NewRefGeometry);
                    }
                    break;
                case GeometryTransformerResultState.FailedWouldBeTooShort:             
                case GeometryTransformerResultState.FailedWouldBeOutside:
                    this.DeleteAchsenReferenz(referenz);
                    break;
                default:
                    break;
            }
        }

        private void UpdateAchsenReferenz(AchsenReferenz referenz, ILineString iLineString)
        {   
            referenz.Shape = iLineString;
            IAbschnittGISBase abschnitt = this.GetAbschnittGISBase(referenz);
            if (abschnitt == null)
            {
                Loggers.ApplicationLogger.Warn(String.Format("No Abschnitt found for Referenzgruppe id: {0}", referenz.ReferenzGruppe.Id.ToString()));
            }
            else
            {
                this.UpdateAbschnitt(abschnitt);
            }
        }
        private void UpdateAbschnitt(IAbschnittGISBase abschnitt) {
            IGeometry shape = null;
            foreach (AchsenReferenz aref in abschnitt.ReferenzGruppe.AchsenReferenzen)
            {
                if (shape != null)
                {
                    shape = shape.Union(aref.Shape);
                }
                else
                {
                    shape = aref.Shape;
                }
            }
            abschnitt.Shape = shape;
            abschnitt.Laenge = (decimal)Math.Round(shape.Length * 10) / 10;
            this.transactionScopeProvider.CurrentTransactionScope.Session.Update(abschnitt);

        }
        public void DeleteAchsenReferenzen(AchsenSegment segment)
        {
            foreach (AchsenReferenz aref in segment.AchsenReferenzen.ToList()) 
            {
                this.DeleteAchsenReferenz(aref);
            }
        }
        public void DeleteAchsenReferenzen(AchsenSegmentModel model)
        {
            AchsenSegment segment = this.achsenSegmentService.GetCurrentEntities().Where(s => s.Id == model.Id).Single();
            this.DeleteAchsenReferenzen(segment);
        }
        public void DeleteAchsenReferenzen(Guid id)
        {
            AchsenSegment segment = this.achsenSegmentService.GetCurrentEntities().Where(s => s.Id == id).Single();
            this.DeleteAchsenReferenzen(segment);
        }
        private void DeleteAchsenReferenz(AchsenReferenz referenz)
        {
            ISession session = transactionScopeProvider.CurrentTransactionScope.Session;
            referenz.ReferenzGruppe.AchsenReferenzen.Remove(referenz);
            referenz.AchsenSegment.AchsenReferenzen.Remove(referenz);
            session.Delete(referenz);
            IAbschnittGISBase abschnitt = this.GetAbschnittGISBase(referenz);
            if (abschnitt == null) { 
                Loggers.ApplicationLogger.Warn(String.Format("No Abschnitt found for Referenzgruppe id: {0}", referenz.ReferenzGruppe.Id.ToString()));
            }
            else
            {
                if (referenz.ReferenzGruppe.AchsenReferenzen.Count < 1)
                {
                    session.Delete(referenz.ReferenzGruppe);
                    ZustandsabschnittGIS zab = abschnitt as ZustandsabschnittGIS;
                    if (zab != null)
                    {
                        zab.StrassenabschnittGIS.Zustandsabschnitten.Remove(zab);
                    }

                    session.Delete(abschnitt);
                }
                else
                {
                    this.UpdateAbschnitt(abschnitt);
                }
            }

        }
        private IAbschnittGISBase GetAbschnittGISBase(AchsenReferenz achsRef)
        {
            if (achsRef.ReferenzGruppe.StrassenabschnittGIS != null)
            {
                return achsRef.ReferenzGruppe.StrassenabschnittGIS;
            }
            else if (achsRef.ReferenzGruppe.ZustandsabschnittGIS != null)
            {
                return achsRef.ReferenzGruppe.ZustandsabschnittGIS;
            }
            else if (achsRef.ReferenzGruppe.KoordinierteMassnahmeGIS != null)
            {
                return achsRef.ReferenzGruppe.KoordinierteMassnahmeGIS;
            }
            else if (achsRef.ReferenzGruppe.MassnahmenvorschlagTeilsystemeGIS != null)
            {
                return achsRef.ReferenzGruppe.MassnahmenvorschlagTeilsystemeGIS;
            }
            else if (achsRef.ReferenzGruppe.RealisierteMassnahmeGIS != null)
            {
                return achsRef.ReferenzGruppe.RealisierteMassnahmeGIS;
            }
            else
            {
                return null;
            }
        }
    }
}
