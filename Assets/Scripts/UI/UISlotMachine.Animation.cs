using System.Collections;
using Data.Items;
using UnityEngine;

namespace UI
{
    public partial class UISlotMachine
    {
        
        private void SpinRoutine(Item result)
        {
            StartCoroutine(CreateRoutine(result));
        }
        
        private IEnumerator CreateRoutine(Item result)
        {
            if (SpunAny)
                yield break;

            Spun = true;
            OnSpinStart?.Invoke();

            var spinData = PrepareSpin(result);
            
            foreach (var container in _containers)
            {
                yield return new WaitForSeconds(_delay);
                
                SpinColumn(container, spinData[container]);
            }
            
            while (SpunAny)
                yield return null;

            Spun = false;
            OnSpinEnd?.Invoke();
        }

        private void SpinColumn(RectTransform container, SpinData data)
        {
            StartCoroutine(CreateSpinColumn(container, data));
        }
        
        private IEnumerator CreateSpinColumn(RectTransform container, SpinData data)
        {
            ContainerSpinning[container] = true;
            OnColumnStart?.Invoke();
     
            var elapsed = 0f;
            var duration = _spinDuration;
            var totalHeight = data.Height;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;

                var t = Mathf.Clamp01(elapsed / duration);
                var eased = _decelerationCurve.Evaluate(t);
                var rawPosition = Mathf.Lerp(data.Start, data.End, eased);
                var looped = Mathf.Repeat(rawPosition, totalHeight);

                container.anchoredPosition = Vector2.up * looped;

                yield return null;
            }

            container.anchoredPosition = Vector2.up * data.Target;

            ContainerSpinning[container] = false;
            OnColumnStop?.Invoke();
        }
    }
}