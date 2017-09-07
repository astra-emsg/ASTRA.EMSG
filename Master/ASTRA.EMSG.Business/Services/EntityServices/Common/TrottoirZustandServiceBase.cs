using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Services.EntityServices.Common
{
    public interface ITrottoirZustandServiceBase
    {
        ZustandsabschnittdetailsTrottoirModel GetZustandsabschnittTrottoirModel(Guid id);
        void UpdateZustandsabschnittTrottoirModel(ZustandsabschnittdetailsTrottoirModel zustandsabschnittdetailsTrottoirModel);
        void DeleteUnusedData(ErfassungsPeriod closedPeriod);
    }

    public abstract class TrottoirZustandServiceBase : ITrottoirZustandServiceBase
    {
        protected readonly ITransactionScopeProvider transactionScopeProvider;

        protected TrottoirZustandServiceBase(ITransactionScopeProvider transactionScopeProvider)
        {
            this.transactionScopeProvider = transactionScopeProvider;
        }

        protected abstract ZustandsabschnittBase GetZustandsabschnittBase(Guid id);
        protected abstract void UpdateZustandsabschnittBase(ZustandsabschnittBase zustandsabschnittBase);
        protected abstract IEnumerable<ZustandsabschnittBase> GetAllByErfassungsPeriod(ErfassungsPeriod erfassungsPeriod);

        public ZustandsabschnittdetailsTrottoirModel GetZustandsabschnittTrottoirModel(Guid id)
        {

            //TODO: Use refleaction traslator
            ZustandsabschnittBase zustandsabschnittBase = GetZustandsabschnittBase(id);

            var zustandsabschnittdetailsTrottoirModel = new ZustandsabschnittdetailsTrottoirModel
                                                            {
                                                                Id = zustandsabschnittBase.Id,
                                                                Strassenname = zustandsabschnittBase.Strassenname,
                                                                BelastungskategorieTyp = zustandsabschnittBase.BelastungskategorieTyp,
                                                                BezeichnungVon = zustandsabschnittBase.BezeichnungVon,
                                                                BezeichnungBis = zustandsabschnittBase.BezeichnungBis,
                                                                Trottoir = zustandsabschnittBase.StrassenabschnittBase.Trottoir,
                                                            };
            
            var massnahmenvorschlagTrottoirLinks = zustandsabschnittBase.MassnahmenvorschlagTrottoirLinks;
            zustandsabschnittdetailsTrottoirModel.LinkeTrottoirDringlichkeit = zustandsabschnittBase.DringlichkeitTrottoirLinks;
            zustandsabschnittdetailsTrottoirModel.LinkeTrottoirKosten = GetKosten(massnahmenvorschlagTrottoirLinks, zustandsabschnittBase.KostenMassnahmenvorschlagTrottoirLinks);
            zustandsabschnittdetailsTrottoirModel.LinkeTrottoirMassnahmenvorschlagKatalogId = massnahmenvorschlagTrottoirLinks == null ? (Guid?)null : massnahmenvorschlagTrottoirLinks.Id;
            

            zustandsabschnittdetailsTrottoirModel.LinkeTrottoirGesamtKosten = zustandsabschnittBase.KostenTrottoirLinks;
            zustandsabschnittdetailsTrottoirModel.LinkeTrottoirZustandsindex = zustandsabschnittBase.ZustandsindexTrottoirLinks;

            var massnahmenvorschlagTrottoirRechts = zustandsabschnittBase.MassnahmenvorschlagTrottoirRechts;
            zustandsabschnittdetailsTrottoirModel.RechteTrottoirDringlichkeit = zustandsabschnittBase.DringlichkeitTrottoirRechts;
            zustandsabschnittdetailsTrottoirModel.RechteTrottoirKosten = GetKosten(massnahmenvorschlagTrottoirRechts, zustandsabschnittBase.KostenMassnahmenvorschlagTrottoirRechts);
            zustandsabschnittdetailsTrottoirModel.RechteTrottoirMassnahmenvorschlagKatalogId = massnahmenvorschlagTrottoirRechts == null ? (Guid?)null : massnahmenvorschlagTrottoirRechts.Id;
            
            zustandsabschnittdetailsTrottoirModel.RechteTrottoirGesamtKosten = zustandsabschnittBase.KostenTrottoirRechts;
            zustandsabschnittdetailsTrottoirModel.RechteTrottoirZustandsindex = zustandsabschnittBase.ZustandsindexTrottoirRechts;

            return zustandsabschnittdetailsTrottoirModel;
        }

        public void UpdateZustandsabschnittTrottoirModel(ZustandsabschnittdetailsTrottoirModel zustandsabschnittdetailsTrottoirModel)
        {
            //TODO: Use refleaction traslator
            ZustandsabschnittBase zustandsabschnittBase = GetZustandsabschnittBase(zustandsabschnittdetailsTrottoirModel.Id);

            zustandsabschnittBase.ZustandsindexTrottoirLinks = zustandsabschnittdetailsTrottoirModel.LinkeTrottoirZustandsindex;

            zustandsabschnittBase.MassnahmenvorschlagTrottoirLinks = zustandsabschnittdetailsTrottoirModel.LinkeTrottoirMassnahmenvorschlagKatalogId == null
                                                       ? null
                                                       : transactionScopeProvider.GetById<MassnahmenvorschlagKatalog>(zustandsabschnittdetailsTrottoirModel.LinkeTrottoirMassnahmenvorschlagKatalogId.Value);
            zustandsabschnittBase.KostenMassnahmenvorschlagTrottoirLinks = zustandsabschnittdetailsTrottoirModel.LinkeTrottoirKosten;
            zustandsabschnittBase.DringlichkeitTrottoirLinks = zustandsabschnittdetailsTrottoirModel.LinkeTrottoirDringlichkeit;


            zustandsabschnittBase.ZustandsindexTrottoirRechts = zustandsabschnittdetailsTrottoirModel.RechteTrottoirZustandsindex;

            zustandsabschnittBase.MassnahmenvorschlagTrottoirRechts = zustandsabschnittdetailsTrottoirModel.RechteTrottoirMassnahmenvorschlagKatalogId == null
                                                        ? null
                                                        : transactionScopeProvider.GetById<MassnahmenvorschlagKatalog>(zustandsabschnittdetailsTrottoirModel.RechteTrottoirMassnahmenvorschlagKatalogId.Value);
            zustandsabschnittBase.KostenMassnahmenvorschlagTrottoirRechts = zustandsabschnittdetailsTrottoirModel.RechteTrottoirKosten;
            zustandsabschnittBase.DringlichkeitTrottoirRechts = zustandsabschnittdetailsTrottoirModel.RechteTrottoirDringlichkeit;

            //Update back the Kosten and GesamtKosten calculated field
            zustandsabschnittdetailsTrottoirModel.LinkeTrottoirGesamtKosten = zustandsabschnittBase.KostenTrottoirLinks;
            zustandsabschnittdetailsTrottoirModel.RechteTrottoirGesamtKosten = zustandsabschnittBase.KostenTrottoirRechts;
            zustandsabschnittdetailsTrottoirModel.LinkeTrottoirKosten = GetKosten(zustandsabschnittBase.MassnahmenvorschlagTrottoirLinks, zustandsabschnittBase.KostenMassnahmenvorschlagTrottoirLinks);
            zustandsabschnittdetailsTrottoirModel.RechteTrottoirKosten = GetKosten(zustandsabschnittBase.MassnahmenvorschlagTrottoirRechts, zustandsabschnittBase.KostenMassnahmenvorschlagTrottoirRechts);

            UpdateZustandsabschnittBase(zustandsabschnittBase);
        }

        public void DeleteUnusedData(ErfassungsPeriod closedPeriod)
        {
            foreach (var zustandsabschnittBase in GetAllByErfassungsPeriod(closedPeriod))
            {
                switch (zustandsabschnittBase.StrassenabschnittBase.Trottoir)
                {
                    case TrottoirTyp.NochNichtErfasst:
                    case TrottoirTyp.KeinTrottoir:
                        CleanUpTrottoirLinks(zustandsabschnittBase);
                        CleanUpTrottoirRechts(zustandsabschnittBase);
                        break;
                    case TrottoirTyp.Links:
                        CleanUpTrottoirRechts(zustandsabschnittBase);
                        break;
                    case TrottoirTyp.Rechts:
                        CleanUpTrottoirLinks(zustandsabschnittBase);
                        break;
                    case TrottoirTyp.BeideSeiten:
                        //NOP
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static void CleanUpTrottoirLinks(ZustandsabschnittBase z)
        {
            z.MassnahmenvorschlagTrottoirLinks = null;
            z.ZustandsindexTrottoirLinks = ZustandsindexTyp.Unbekannt;
        }

        private static void CleanUpTrottoirRechts(ZustandsabschnittBase z)
        {
            z.MassnahmenvorschlagTrottoirRechts = null;
            z.ZustandsindexTrottoirRechts = ZustandsindexTyp.Unbekannt;
        }

        private decimal? GetKosten(MassnahmenvorschlagKatalog massnahmenvorschlag, decimal? kosten)
        {
           if (massnahmenvorschlag == null)
                return 0;

            return kosten ?? massnahmenvorschlag.DefaultKosten;
        }

    }
}