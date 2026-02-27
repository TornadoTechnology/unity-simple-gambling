using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(UISlotMachine))]
    public class UISlotMachineAudio : MonoBehaviour
    {
        [Header("Audio Clips")] [SerializeField, CanBeNull]
        private AudioClip _startSound;

        [SerializeField, CanBeNull] private AudioClip _spinSound;
        [SerializeField, CanBeNull] private AudioClip _stopSound;

        private AudioSource _audioSource;
        private AudioSource _spinAudioSource;

        private UISlotMachine _slotMachine;

        private void Awake()
        {
            // Cache components
            _audioSource = GetComponent<AudioSource>();
            _slotMachine = GetComponent<UISlotMachine>();

            _spinAudioSource = gameObject.AddComponent<AudioSource>();
            _spinAudioSource.loop = true;

            // Subscribe events
            _slotMachine.OnSpinStart += HandleSpinStart;
            _slotMachine.OnColumnStop += HandleColumnStop;
            _slotMachine.OnSpinEnd += HandleSpinEnd;
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            _slotMachine.OnSpinStart -= HandleSpinStart;
            _slotMachine.OnColumnStop -= HandleColumnStop;
            _slotMachine.OnSpinEnd -= HandleSpinEnd;
        }

        private void HandleSpinStart()
        {
            StartCoroutine(PlaySpinAfterStart());
        }

        private IEnumerator PlaySpinAfterStart()
        {
            if (_startSound)
                _audioSource.PlayOneShot(_startSound);
            
            if (_startSound)
                yield return new WaitForSeconds(_startSound.length);

            if (_spinSound)
            {
                _spinAudioSource.clip = _spinSound;
                _spinAudioSource.Play();
            }
        }

        private void HandleColumnStop(int columnIndex)
        {
            if (_stopSound)
                _audioSource.PlayOneShot(_stopSound);
        }

        private void HandleSpinEnd()
        {
            _spinAudioSource.Stop();
        }
    }
}
