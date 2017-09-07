using System;

namespace ASTRA.EMSG.Mobile.ViewModels
{
    public interface IZustandsabschnittTabViewModel : ISaveable, IViewModel
    {
        string HeaderText { get; }
        bool HasError { get; }
        event EventHandler Changed;
        void Init();
        bool IsValid { get; }
        void RefreshValidation();
        void OpenHelp();
    }
}