namespace ASTRA.EMSG.Business.Entities.Common
{
    public interface IErfassungsPeriodDependentEntity : IMandantDependentEntity
    {
        ErfassungsPeriod ErfassungsPeriod { get; set; }
    }
}