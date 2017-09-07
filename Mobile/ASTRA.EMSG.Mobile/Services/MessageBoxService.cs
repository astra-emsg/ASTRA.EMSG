using System.Windows;

namespace ASTRA.EMSG.Mobile.Services
{
    public interface IMessageBoxService
    {
        void Information(string message);
        void Warning(string message);
        void Error(string message);
        bool Comfirm(string message);
    }

    public class MessageBoxService : IMessageBoxService
    {
        public void Information(string message)
        {
            MessageBox.Show(message, "", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        public void Warning(string message)
        {
            MessageBox.Show(message, "", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        
        public bool Comfirm(string message)
        {
            return MessageBox.Show(message, "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
        }

        public void Error(string message)
        {
            MessageBox.Show(message, "", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}