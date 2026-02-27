using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace UI.Main
{
    public class UIInitMenu : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _delay = 1.5f;

        private void Start()
        {
            StartCoroutine(WaitAndLoad());
        }

        private IEnumerator WaitAndLoad()
        {
            yield return new WaitForSeconds(_delay);
            
            SceneManager.LoadScene((int) Scenes.MainMenu);
        }
    }
}