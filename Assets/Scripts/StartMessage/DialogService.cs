using System;
using MixedReality.Toolkit.UX;

namespace Assets.Scripts.StartMessage
{
    public class DialogService : IStartMessageService
    {
        private readonly DialogPool _dialogPool;

        public DialogService(DialogPool dialogPool)
        {
            _dialogPool = dialogPool;
        }

        public void ShowMessage(string header, string body, string positiveButtonText, Action onPositiveButtonClicked)
        {
            var dialog = _dialogPool.Get()
                .SetHeader(header)
                .SetBody(body)
                .SetPositive(positiveButtonText, args =>
                {
                    if (args.ButtonType == DialogButtonType.Positive)
                    {
                        onPositiveButtonClicked?.Invoke();
                    }
                })
                .Show();
        }
    }
}