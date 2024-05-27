using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Dialog
{
    public interface IDialogService
    {
        void ShowDialog(string header, string body, string positiveButtonText, Action onPositiveButtonClicked);
    }
}
