using MixedReality.Toolkit.UX;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Dialog
{
    public class DialogService : IDialogService
    {
        private readonly DialogPool _dialogPool;

        public DialogService(DialogPool dialogPool)
        {
            _dialogPool = dialogPool;
        }

        public void ShowDialog(string header, string body, string positiveButtonText, Action onPositiveButtonClicked)
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