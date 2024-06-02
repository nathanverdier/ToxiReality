using Assets.Scripts.Api;
using Assets.Scripts.Dialog;
using Assets.Scripts.Photo;
using MixedReality.Toolkit.UX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.Scripts.Api.ApiService;

namespace Assets.Scripts.Managers
{
    public class DialogManager : MonoBehaviour
    {
        private IDialogService _dialogService;
        
        private RecognitionRoutineManager _regcognitionRoutine;

        

        private void Start()
        {
            var dialogPool = FindObjectOfType<DialogPool>();
            _regcognitionRoutine = FindObjectOfType<RecognitionRoutineManager>();

            if (dialogPool != null)
            {
                _dialogService = new DialogService(dialogPool);
                _dialogService.ShowDialog("Welcome to Lucie's App!", "This app is a photo capture test for my study project ToxiReality", "Good Luck", OnOkButtonClicked);
            }
            else
            {
                Debug.LogError("DialogPool not assigned in DialogManager!");
            }
        }

        private void OnOkButtonClicked()
        {
            // Start recognition routine
            if (_regcognitionRoutine != null)
            {
                _regcognitionRoutine.StartRepeatingAction();
            }
            else
            {
                Debug.LogError("RepeatedActionHandler component not found in the scene.");
            }
        }
    }
}