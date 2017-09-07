using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using NHibernate;
using ASTRA.EMSG.Business.Entities;
using GeoAPI.Geometries;
using NetTopologySuite.LinearReferencing;
using NetTopologySuite.Geometries.MGeometries;
using ASTRA.EMSG.Business.Services.GIS;
using ASTRA.EMSG.Common.Master.Logging;
using ASTRA.EMSG.Common.Utils;

namespace ASTRA.EMSG.Business.AchsenUpdate.UpdateReferences
{

    public class RegisteredItem
    {
        public IAbschnittGISBase abschnittGISBase;
        public IGeometry originalGeometry;
        public List<AchsenUpdateConflict> conflicts = new List<AchsenUpdateConflict>();
    }


    public class ReferenceUpdater : IReferenceUpdater
    {
        private readonly ISession session;
        private readonly ErfassungsPeriod erfPeriod;

        private readonly IGeometryFactory gf = GISService.CreateGeometryFactory();

        private Dictionary<IAbschnittGISBase, RegisteredItem> items = new Dictionary<IAbschnittGISBase, RegisteredItem>();

        private Dictionary<Guid, DateTime> oldAxisValidFromDict = new Dictionary<Guid, DateTime>();  // axis updates happen before segment updates - therfore we have to save versionvalidfrom

        private ReferenceUpdaterStatistics statistics = new ReferenceUpdaterStatistics();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="session"></param>
        /// <param name="erfPeriod"></param>
        public ReferenceUpdater(ISession session, ErfassungsPeriod erfPeriod)
        {
            this.session = session;
            this.erfPeriod = erfPeriod;
        }

        public void BeforeDeleteSegment(AchsenSegment segment)
        {
            if (segment.AchsenReferenzen.Count > 0)
            {
                // Delete References and Strabs and ZustandsAbs...
                foreach (AchsenReferenz achsRef in new List<AchsenReferenz>(segment.AchsenReferenzen))
                {
                    RegisterConflict(achsRef, CreateConflict(AchsenUpdateConflict.CONFLICTTYPE_PARTIAL_SEGMENT_DELETED, segment.Id, achsRef.Shape));
                    RemoveReference(achsRef);
                }
            }

            AchsenUpdateConflict conflict = CreateConflict(AchsenUpdateConflict.CONFLICTTYPE_PARTIAL_SEGMENT_DELETED, segment.Id, segment.Shape);
            conflict.ItemType = AchsenUpdateConflict.ITEMTYPE_SEGMENT;
            conflict.ItemId = segment.Id;
            conflict.Name = segment.Name;
            session.Persist(conflict);
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

        private void CheckIfLocked(IAbschnittGISBase abschnittGISBase)
        {
            if (abschnittGISBase is StrassenabschnittGIS)
            {
                StrassenabschnittGIS straGis = (StrassenabschnittGIS)abschnittGISBase;
                if (straGis.IsLocked)
                {
                    throw new AchsenUpdateAbschnittLockedException();
                }
            }
        }

        private void RegisterConflict(AchsenReferenz achsRef, AchsenUpdateConflict conflict)
        {
            if (Register(achsRef))
            {
                items[GetAbschnittGISBase(achsRef)].conflicts.Add(conflict);
            }
        }

        private bool Register(AchsenReferenz achsRef)
        {
            IAbschnittGISBase abschnittGISBase = GetAbschnittGISBase(achsRef);

            if (abschnittGISBase == null) return false;

            CheckIfLocked(abschnittGISBase);

            if (items.ContainsKey(abschnittGISBase)) return true;


            RegisteredItem rItem = new RegisteredItem();
            rItem.abschnittGISBase = abschnittGISBase;
            rItem.originalGeometry = abschnittGISBase.Shape;
            items.Add(abschnittGISBase, rItem);
            return true;

        }


        private void RemoveReference(AchsenReferenz reference)
        {
            reference.AchsenSegment.AchsenReferenzen.Remove(reference);
            reference.ReferenzGruppe.AchsenReferenzen.Remove(reference);
            session.Delete(reference);
        }

        private void UpdateGeometryFromReferences(IAbschnittGISBase abschnittGISBase)
        {
            IGeometry newShape = null;

            foreach (AchsenReferenz achsRef in abschnittGISBase.ReferenzGruppe.AchsenReferenzen)
            {
                if (newShape == null)
                {
                    newShape = achsRef.Shape;
                }
                else
                {
                    newShape = newShape.Union(achsRef.Shape);
                }
            }
            abschnittGISBase.Shape = newShape;
        }

        public void BeforeDeleteAchse(Achse targetAchse)
        {
            // noop
        }
        public void CreateValidFromDict(IEnumerable<Achse> achsen)
        {
            foreach (Achse achse in achsen)
            {
                oldAxisValidFromDict.Add(achse.BsId, achse.VersionValidFrom);
            }
        }
        public void BeforeUpdateAchse(Achse targetAchse, KopieAchse kopieAchse)
        {
            if(!oldAxisValidFromDict.ContainsKey(targetAchse.BsId))
                oldAxisValidFromDict.Add(targetAchse.BsId, targetAchse.VersionValidFrom);
        }

        public void BeforeUpdateSegment(AchsenSegment targetSegment, KopieAchse kopieAchse, KopieAchsenSegment kopieSegment)
        {
            DateTime oldVersionValidFrom = oldAxisValidFromDict[targetSegment.Achse.BsId];
            DateTime newVersionValidFrom = kopieAchse.VersionValidFrom;


            if (targetSegment.AchsenReferenzen.Count > 0)
            {

                foreach (AchsenReferenz achsRef in new List<AchsenReferenz>(targetSegment.AchsenReferenzen))
                {
                    ILineString oldRefGeometry = (ILineString)achsRef.Shape;
                    ILineString newSegmentGeometry = GeometryUtils.ConvertMLineStringTo2D(gf, (MLineString)kopieSegment.Shape);
                    
                    GeometryTransformer gt = new GeometryTransformer(oldRefGeometry, newSegmentGeometry);

                    GeometryTransformerResult result = gt.Transform();

                    switch (result.ResultState)
                    {
                        case GeometryTransformerResultState.Success: 
                            {
                                achsRef.Shape = result.NewRefGeometry;
                                Register(achsRef);
                                statistics.UpdatedReferences++;
                                break; 
                            }

                        case GeometryTransformerResultState.FailedWouldBeOutside: 
                            {
                                RegisterConflict(achsRef, CreateConflict(AchsenUpdateConflict.CONFLICTTYPE_PARTIAL_REFERENCE_OUTSIDE, targetSegment.Id, oldRefGeometry));
                                RemoveReference(achsRef);
                                statistics.DeletedReferences++;
                                break; 
                            }

                        case GeometryTransformerResultState.FailedWouldBeTooShort: 
                            {
                                RegisterConflict(achsRef, CreateConflict(AchsenUpdateConflict.CONFLICTTYPE_PARTIAL_REFERENCE_TOOSHORT, targetSegment.Id, oldRefGeometry));
                                RemoveReference(achsRef);
                                statistics.DeletedReferences++;
                                break; 
                            }

                    }
                }
            }
        }

        private AchsenUpdateConflict CreateConflict(int conflictType, Guid segmentID, IGeometry shape)
        {
            AchsenUpdateConflict conflict = new AchsenUpdateConflict();
            conflict.Mandant = erfPeriod.Mandant;
            conflict.ErfassungsPeriod = erfPeriod;
            conflict.ConflictType = conflictType;
            conflict.SegmentId = segmentID;
            conflict.Shape = shape;
            return conflict;
        }


        public void AfterCreateSegment(AchsenSegment segment)
        {
            if (segment.ImpNr > 1)  // incremental update, we suppress those warnings for full updates
            {
                AchsenUpdateConflict conflict = CreateConflict(AchsenUpdateConflict.CONFLICTTYPE_NEWSEGMENT, segment.Id, segment.Shape);
                conflict.ItemType = AchsenUpdateConflict.ITEMTYPE_SEGMENT;
                conflict.ItemId = segment.Id;
                conflict.Name = segment.Name;
                session.Persist(conflict);
            }
        }


        public void PostWork()
        {
            foreach (RegisteredItem item in items.Values)
            {
                bool completeLoss = false;

                int itemType = AchsenUpdateConflict.ItemTypeFromClass(item.abschnittGISBase);
                string name = item.abschnittGISBase.DisplayName;

                if (item.abschnittGISBase.ReferenzGruppe.AchsenReferenzen.Count == 0)
                {
                    completeLoss = true;
                    session.Delete(item.abschnittGISBase.ReferenzGruppe);
                    session.Delete(item.abschnittGISBase);

                    statistics.ReportRemoved(item.abschnittGISBase);
                }
                else
                {
                    UpdateGeometryFromReferences(item.abschnittGISBase);

                    statistics.ReportUpdated(item.abschnittGISBase);
                }

                foreach (AchsenUpdateConflict conflict in item.conflicts)
                {
                    if (completeLoss) conflict.ConflictType |= 16;
                    conflict.Name = name;
                    conflict.ItemType = itemType;
                    conflict.ItemId = item.abschnittGISBase.Id;
                    session.Persist(conflict);    
                }
            }
        }

        public ReferenceUpdaterStatistics Statistics { get { return statistics; } }
    }
}
