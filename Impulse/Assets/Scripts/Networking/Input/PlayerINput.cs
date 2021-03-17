using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Networking.Input
{
    public class PlayerINput : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private Button continueButton;

        public static string DisplayName { get; private set; }

        private const string filePathName = "PlayerName";

        public void Start() => SetupInputField();

        private void SetupInputField()
        {
            //string name = FileReaderWriter.ReadString(filePathName);
            string name = "";
            //if (string.IsNullOrEmpty(name)) return;

            nameInputField.text = name;

            SetPlayerName(name);
        }

        public void SetPlayerName(string name)
        {
            continueButton.interactable = IsNameValid(name);
        }

        public void SavePlayerName()
        {
            string name = nameInputField.text;

            DisplayName = name;
            //FileReaderWriter.WriteString(filePathName, name);
        
        }

        public bool IsNameValid(string name)
        {
            return !string.IsNullOrEmpty(name);
        }
    }
}
