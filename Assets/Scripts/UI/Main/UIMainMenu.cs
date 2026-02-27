using System.Net;
using JetBrains.Annotations;
using Network;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Main
{
    public sealed class UIMainMenu : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputFieldHostname;
        [SerializeField] private TMP_InputField _inputFieldPort;

        private const string DefaultHostname = "127.0.0.1";
        private const string DefaultPort = "8000";

        private void Awake()
        {
            NetClient.Instance.OnConnected += OnConnected;
        }

        private void OnDestroy()
        {
            NetClient.Instance.OnConnected -= OnConnected;
        }
        
        [UsedImplicitly]
        public void Connect()
        {
            var hostname = string.IsNullOrEmpty(_inputFieldHostname.text)
                ? DefaultHostname
                : _inputFieldHostname.text;;
            
            var port = string.IsNullOrEmpty(_inputFieldPort.text)
                ? DefaultPort
                : _inputFieldPort.text;;

            _ = NetClient.Instance.Connect(IPAddress.Parse(hostname), int.Parse(port));
        }

        [UsedImplicitly]
        public void LoadSceneGameplay()
        {
            SceneManager.LoadScene((int) Scenes.Gameplay);
        }

        private void OnConnected()
        {
            LoadSceneGameplay();
        }
    }
}
