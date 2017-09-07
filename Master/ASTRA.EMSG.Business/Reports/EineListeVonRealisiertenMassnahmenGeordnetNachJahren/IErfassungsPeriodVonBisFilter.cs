using System;

namespace ASTRA.EMSG.Business.Reports.EineListeVonRealisiertenMassnahmenGeordnetNachJahren
{
    public interface IErfassungsPeriodVonBisFilter
    {
        Guid ErfassungsPeriodIdVon { get; }
        Guid ErfassungsPeriodIdBis { get; }
    }
}