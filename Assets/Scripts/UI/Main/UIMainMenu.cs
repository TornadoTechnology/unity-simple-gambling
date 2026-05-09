using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Main
{
    public sealed class UIMainMenu : MonoBehaviour
    {
        [UsedImplicitly]
        public void LoadSceneGameplay()
        {
            SceneManager.LoadScene((int) Scenes.Gameplay);
        }
    }
}
