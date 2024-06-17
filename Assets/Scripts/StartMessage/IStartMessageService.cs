using System;

namespace Assets.Scripts.StartMessage
{
    public interface IStartMessageService 
    {
        void ShowMessage(string header, string body, string positiveButtonText, Action onPositiveButtonClicked);
    }
}