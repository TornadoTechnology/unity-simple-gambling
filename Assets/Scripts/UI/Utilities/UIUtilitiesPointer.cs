using UnityEngine;
using TMPro;
using System.Collections;

namespace UI.Utilities
{
    [RequireComponent(typeof(TMP_Text))]
    public class UIUtilitiesPointer : MonoBehaviour
    {
        [SerializeField] private float _delay = 0.5f;
        
        private TMP_Text _text;
        private string _baseText;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            _baseText = _text.text;
        }

        private void OnEnable()
        {
            StartCoroutine(AnimateDots());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator AnimateDots()
        {
            var dotCount = 0;
            while (true)
            {
                _text.text = _baseText + new string('.', dotCount);

                dotCount++;
                
                if (dotCount > 3)
                    dotCount = 0;

                yield return new WaitForSeconds(_delay);
            }
        }
    }
}