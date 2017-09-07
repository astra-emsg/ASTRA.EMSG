namespace ASTRA.EMSG.Business.Entities.Common
{
    public interface IFlaecheFahrbahnUndTrottoirHolder
    {
        decimal FlaecheFahrbahn { get; }
        decimal FlaecheTrottoir { get; }
        bool HasTrottoirInformation { get; }
    }

    public interface IRealisierteFlaecheHolder
    {
        decimal RealisierteFlaeche { get; }
    }
}