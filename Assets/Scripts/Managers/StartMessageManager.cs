using MixedReality.Toolkit.UX;
using Assets.Scripts.StartMessage;
using UnityEngine;

namespace Assets.Scripts.Managers
{ 
    public class StartMessageManager : MonoBehaviour
    {
        private IStartMessageService _dialogService;
        
        private RecognitionRoutineManager _regcognitionRoutine;

        

        private void Start()
        {
            var dialogPool = FindObjectOfType<DialogPool>();
            _regcognitionRoutine = FindObjectOfType<RecognitionRoutineManager>();

            if (dialogPool != null)
            {
                _dialogService = new DialogService(dialogPool);
                _dialogService.ShowMessage("Welcome to Lucie's App!", "This app is a photo capture test for my study project ToxiReality", "Good Luck", OnOkButtonClicked);
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