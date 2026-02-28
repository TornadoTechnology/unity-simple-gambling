using Items;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    [RequireComponent(typeof(UISlotMachine), typeof(UISlotMachineStarter))]
    public sealed class UISlotMachineInput : MonoBehaviour
    {
        [SerializeField] private InputActionReference _inputAction;
        
        private UISlotMachine _slotMachine;
        private UISlotMachineStarter _slotMachineStarter;

        private void Awake()
        {
            _slotMachine = GetComponent<UISlotMachine>();
            _slotMachineStarter = GetComponent<UISlotMachineStarter>();
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

        private void OnScreenPressed(InputAction.CallbackContext obj)
        {
            if (!_slotMachine.Initialized || _slotMachine.Spun)
                return;
            
            _slotMachineStarter.StartSpin(ItemsContainer.Instance.Pick()); 
        }
    }
}