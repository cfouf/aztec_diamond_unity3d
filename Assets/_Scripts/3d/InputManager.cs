using _Scripts.Utilities;
using TMPro;
using UnityEngine;

namespace _Scripts._3d
{
    public class InputManager : MonoBehaviourService<InputManager>
    {
        [SerializeField] private TMP_InputField inputFieldSize;
        [SerializeField] private TMP_InputField inputFieldSpeed;
        [SerializeField] private GameObject button;

        private bool isOneInputDone;

        public void InputSize(string no)
        {
            _3d.GameManager.Get().size = int.Parse(inputFieldSize.text);
            ManageInput();
        }

        public void InputSpeed(string no)
        {
            _3d.GameManager.Get().iterationsPerFrame = int.Parse(inputFieldSpeed.text);
            ManageInput();
        }

        private void ManageInput()
        {
            if (isOneInputDone)
            {
                _3d.GameManager.Get().StartScene();
                inputFieldSize.gameObject.SetActive(false);
                inputFieldSpeed.gameObject.SetActive(false);
                button.SetActive(true);
            }

            isOneInputDone = true;
        }
        protected override void OnCreateService()
        {
        }

        protected override void OnDestroyService()
        {
        }
    }
}