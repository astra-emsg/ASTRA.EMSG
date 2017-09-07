using System;

namespace ASTRA.EMSG.Mobile.ViewModels
{
    public class SaveEventArgs : EventArgs
    {
        public SaveEventArgs(bool closeWindow)
        {
            CloseWindow = closeWindow;
        }

        public bool CloseWindow { get; set; }
    }
}