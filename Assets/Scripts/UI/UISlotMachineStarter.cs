using System.Collections;
using Data.Items;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace UI
{
    [RequireComponent(typeof(UISlotMachine))]
    public sealed class UISlotMachineStarter : MonoBehaviour
    {
        private static readonly int Num1 = Animator.StringToHash("Num1");
        private static readonly int Num2 = Animator.StringToHash("Num2");
        private static readonly int Num3 = Animator.StringToHash("Num3");
        private static readonly int Final = Animator.StringToHash("Final");
        private static readonly int Reset = Animator.StringToHash("Reset");
        private static readonly int Glow = Animator.StringToHash("Glow");
        private static readonly int Press = Animator.StringToHash("Press");

        [SerializeField] private InputActionReference _inputAction;
        
        [SerializeField] private Animator _screenAnimator;
        [SerializeField] private Animator _glowAnimator;
        [SerializeField] private Animator _buttonAnimator;

        private UISlotMachine _slotMachine;
        private bool _started;
        
        private void Awake()
        {
            _slotMachine = GetComponent<UISlotMachine>();
        }

        private void OnEnable()
        {
            _inputAction.action.Enable();
            _inputAction.action.performed += OnScreenPressed;
        }

        private void OnDisable()
        {
            _inputAction.action.performed -= OnScreenPressed;
            _inputAction.action.Disable();
        }

        public void StartSpin(Item result)
        {
            if (_started)
                return;
            
            StartCoroutine(CreateSpin(result));
        }

        private IEnumerator CreateSpin(Item result)
        {
            _started = true;
            
            _screenAnimator.SetTrigger(Num1);
            _glowAnimator.SetTrigger(Glow);
            yield return new WaitForSeconds(1f);
            
            _screenAnimator.SetTrigger(Num2);
            _glowAnimator.SetTrigger(Glow);
            yield return new WaitForSeconds(1f);
   
            _screenAnimator.SetTrigger(Num3);
            _glowAnimator.SetTrigger(Glow);
            yield return new WaitForSeconds(1f);

            _screenAnimator.SetTrigger(Final);
            _glowAnimator.SetTrigger(Reset);
            yield return new WaitForSeconds(5f);

            _slotMachine.Spin(result);

            _screenAnimator.SetTrigger(Reset);
            _started = false;
        }

        private void OnScreenPressed(InputAction.CallbackContext obj)
        {
            if (!_started || _slotMachine.Spun)
                return;
            
            FastLog.Log("Press");
            
            _buttonAnimator.SetTrigger(Press);
        }
    }
}