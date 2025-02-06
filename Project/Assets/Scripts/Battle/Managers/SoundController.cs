using UnityEngine;

namespace ChickenOut.Battle.Managers
{
    public class SoundController : MonoBehaviour
    {
        public static SoundController instance;

        public AudioClip[] _audio;
        public AudioSource[] _source;

        void Awake()
        {
            
            if (instance != null)
                Debug.LogError($"More than one {instance} found", this);
            instance = this;
        }

        void Start() => InvokeRepeating(nameof(SoundRepeating), 3, 15);

        public void DamageSound()
        {
            _source[1].clip = _audio[0];
            _source[1].PlayOneShot(_audio[0], 1);
        }
        public void JumpSound()
        {
            _source[1].clip = _audio[1];
            _source[1].PlayOneShot(_audio[1], 1);
        }
        public void JumpSoundDisable()
        {
            _source[1].clip = _audio[1];
            _source[1].Stop();
        }
        public void DashSound()
        {
            _source[1].clip = _audio[3];
            _source[1].PlayOneShot(_audio[3], 1);
        }
        public void AbilitySound()
        {
            _source[1].clip = _audio[4];
            _source[1].PlayOneShot(_audio[4], .75f);
        }

        void SoundRepeating()
        {
            _source[0].Play();
        }
    }
}