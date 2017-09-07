namespace ASTRA.EMSG.Business.Entities.Common
{
    public interface IMandantDependentEntity : IEntity
    {
        Mandant Mandant { get; set; }
    }
}