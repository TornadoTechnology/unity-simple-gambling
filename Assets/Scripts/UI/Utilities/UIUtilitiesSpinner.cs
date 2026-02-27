using UnityEngine;

namespace UI.Utilities
{
    public class UIUtilitiesSpinner : MonoBehaviour
    {
        [Header("Rotation Settings")]
        [SerializeField] private Vector3 _rotationAxis = Vector3.forward;
        [SerializeField] private float _speed = 180f;
        [SerializeField] private float _timeScale = 1;
        [SerializeField] private bool _useUnscaledTime = true;
        
        [Header("Animation Curve")]
        [SerializeField] private AnimationCurve _speedCurve = AnimationCurve.Linear(0, 0, 1, 1);
        
        private float _timer;
        
        private void Update()
        {
            var delta = _useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            
            _timer = Mathf.Repeat(_timer + delta * (_timeScale / 60f), 1f); 
            
            var curveValue = _speedCurve.Evaluate(_timer);
            transform.Rotate(_rotationAxis.normalized * (_speed * curveValue * delta), Space.Self);
        }
    }
}