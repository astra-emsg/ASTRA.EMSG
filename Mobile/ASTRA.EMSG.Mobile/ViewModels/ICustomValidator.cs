namespace ASTRA.EMSG.Mobile.ViewModels
{
    public interface ICustomValidator
    {
        bool IsValid { get; }
        string ValidationMessage { get; }
    }
}