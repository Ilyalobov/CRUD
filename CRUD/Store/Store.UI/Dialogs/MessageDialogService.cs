namespace Store.UI.Dialogs
{
    using System.Windows;

    public class MessageDialogService : IMessageDialogService
    {
        public MessageDialogResult ShowYesNoDialog(string title, string message)
        {
            return MessageBox.Show(message, title, MessageBoxButton.YesNo) == MessageBoxResult.Yes ? MessageDialogResult.Yes : MessageDialogResult.No;
        }
    }
}