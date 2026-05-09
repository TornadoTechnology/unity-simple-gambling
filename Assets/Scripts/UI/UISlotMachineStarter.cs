using System.Collections;
using Data;
using Data.Objects.Items;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
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
        private static readonly int Hide = Animator.StringToHash("Hide");
        private static readonly int Show = Animator.StringToHash("Show");

        [SerializeField] private InputActionReference _inputAction;
        [SerializeField] private AudioClip _clickSound;
        [SerializeField] private UISlotMachineAudio _slotMachineAudio;
        
        [SerializeField] private Animator _screenAnimator;
        [SerializeField] private Animator _screenTextAnimator;
        [SerializeField] private Animator _glowAnimator;
        [SerializeField] private Animator _buttonAnimator;
        [SerializeField] private Animator _winAnimator;

        [SerializeField] private TMP_Text _winText;
        [SerializeField] private RawImage _winImage;
        
        [SerializeField] private TMP_Text _screenText;

        private UISlotMachine _slotMachine;
        private AudioSource _audioSource;
        
        private bool _started;
        private bool _allowPress;
        private bool _allowStart = true;
        
        private int _clicks;
        
        private void Awake()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.loop = false;
            _audioSource.volume = 0.24f;
            
            _slotMachine = GetComponent<UISlotMachine>();
            _slotMachine.OnSpinEnd += item =>
            {
                StartCoroutine(CreateWin(item));
            };
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

        public void StartSpin()
        {
            if (_started || !_allowStart)
                return;
            
            _allowStart = false;
            _clicks = 0;
            _screenText.SetText(string.Empty);
            _screenTextAnimator.SetBool(Reset, true);
            
            StartCoroutine(CreateSpin());
        }

        private IEnumerator CreateSpin()
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

            _allowPress = true;
            
            _screenAnimator.SetTrigger(Final);
            _glowAnimator.SetTrigger(Reset);
            yield return new WaitForSeconds(5f);
            
            _screenTextAnimator.SetBool(Reset, false);
            _screenTextAnimator.SetTrigger(Hide);
            
            var rank = ResourceGameContainer.GetRank(_clicks);
            FastLog.Log(rank);
            
            _slotMachine.Spin(ResourceGameContainer.PickItem(rank));

            _screenAnimator.SetTrigger(Reset);
            
            _started = false;
            _allowPress = false;
        }

        private IEnumerator CreateWin(Item item)
        {
            _winText.SetText(item.Name);
            _winImage.texture = item.Sprite.texture;
            _winAnimator.SetTrigger(Show);

            _slotMachineAudio.PlayWinSound();
            
            yield return new WaitForSeconds(5f);
            
            _slotMachineAudio.PlayBackgroundSound();
            
            _winAnimator.SetTrigger(Hide);
            _allowStart = true;
        }
        
        private void OnScreenPressed(InputAction.CallbackContext obj)
        {
            if (!_started || _slotMachine.Spun)
                return;
            
            if (!_allowPress)
                return;
            
            _audioSource.PlayOneShot(_clickSound);
            
            _clicks++;
            _screenText.SetText($"{_clicks} нажатий!!!");
            
            _buttonAnimator.SetTrigger(Press);
        }
    }
}