using System.Collections;
using Data.Objects.Items;
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
        [SerializeField, CanBeNull] private AudioClip _backgroundSound;
        [SerializeField, CanBeNull] private AudioClip _winSound;
       
        [Header("Volume")]
        [SerializeField, Range(0f, 1f)]
        private float _mainVolume = 1f;

        [SerializeField, Range(0f, 1f)]
        private float _spinVolume = 1f;

        [SerializeField, Range(0f, 1f)]
        private float _backgroundVolume = 0.5f;

        [SerializeField, Range(0f, 1f)]
        private float _winVolume = 1f;
        
        private AudioSource _audioSource;
        private AudioSource _spinAudioSource;
        private AudioSource _backgroundAudioSource;
        private AudioSource _backgroundWinAudioSource;
        
        private UISlotMachine _slotMachine;

        public void PlayWinSound()
        {
            _backgroundWinAudioSource.UnPause();
            _backgroundAudioSource.Pause();
        }
        
        public void PlayBackgroundSound()
        {
            _backgroundWinAudioSource.Pause();
            _backgroundAudioSource.UnPause();
        }
        
        private void Awake()
        {
            // Cache components
            _audioSource = GetComponent<AudioSource>();
            _audioSource.volume = _mainVolume;
            
            _slotMachine = GetComponent<UISlotMachine>();
            
            _spinAudioSource = gameObject.AddComponent<AudioSource>();
            _spinAudioSource.loop = true;
            _spinAudioSource.volume = _spinVolume;
            
            _backgroundAudioSource = gameObject.AddComponent<AudioSource>();
            _backgroundAudioSource.loop = true;
            _backgroundAudioSource.volume = _backgroundVolume;
            
            _backgroundWinAudioSource = gameObject.AddComponent<AudioSource>();
            _backgroundWinAudioSource.loop = true;
            _backgroundWinAudioSource.volume = _winVolume;

            _backgroundWinAudioSource.clip = _winSound;
            _backgroundWinAudioSource.Play();
            _backgroundWinAudioSource.Pause();
      
            _backgroundAudioSource.clip = _backgroundSound;
            _backgroundAudioSource.Play();
            
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

        private void HandleColumnStop()
        {
            if (_stopSound)
                _audioSource.PlayOneShot(_stopSound);
        }

        private void HandleSpinEnd(Item item)
        {
            _spinAudioSource.Stop();
        }
    }
}
