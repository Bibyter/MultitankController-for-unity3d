using System.Collections;
using UnityEngine;


namespace Bibyter.Multitank
{
    public class TankSound : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float m_RangePich = 0.3f;
        [SerializeField, Range(0f, 1f)] private float m_VolumeShot = 0.8f;
        [SerializeField, Range(0f, 1f)] private float m_VolumeReload = 0.8f;

        public TankController m_TankController;
        public TankGun m_TankGun;
        [Space]
        public AudioSource m_SourceMinTorque;
        public AudioSource m_SourceMaxTorque;
        [Space]
        public AudioClip m_SoundMinTorque;
        public AudioClip m_SoundMaxTorque;
        public AudioClip m_SoundReload;
        public AudioClip[] m_SoundsShots;

        [HideInInspector]
        public bool isRun;

        AudioSource m_AudioSourceShoot;


        void Start()
        {
            isRun = true;

            m_SourceMinTorque.clip = m_SoundMinTorque;
            m_SourceMaxTorque.clip = m_SoundMaxTorque;

            m_SourceMaxTorque.Play();
            m_SourceMinTorque.Play();
            
            m_AudioSourceShoot = m_TankGun.startBulletPosition.GetComponent<AudioSource>();
            Invoke("PlayEndReload", m_TankGun.reloadTime - m_SoundReload.length);
        }


        void FixedUpdate()
        {
            float speed = m_TankController.motorTorque01;

            if (isRun)
            {
                m_SourceMaxTorque.volume = speed;
                m_SourceMinTorque.volume = Mathf.Clamp01(1f - speed - 0.2f);
            }
            else
            {
                m_SourceMaxTorque.volume = Mathf.MoveTowards(m_SourceMaxTorque.volume, 0f, Time.deltaTime / 2f);
                m_SourceMinTorque.volume = Mathf.MoveTowards(m_SourceMinTorque.volume, 0f, Time.deltaTime / 2f);
            }

            m_SourceMaxTorque.pitch = 1f + m_RangePich * (speed - 0.5f);
            m_SourceMinTorque.pitch = 1f + m_RangePich * (Mathf.Clamp01(speed * 1.8f) - 0.5f);
        }
        

        IEnumerator PlaySoundReload()
        {
            m_AudioSourceShoot.volume = m_VolumeShot;
            m_AudioSourceShoot.clip = m_SoundsShots[Random.Range(0, m_SoundsShots.Length)];
            m_AudioSourceShoot.Play();

            yield return new WaitForSeconds(m_TankGun.reloadTime - m_SoundReload.length);
            PlayEndReload();
        }

        void PlayEndReload()
        {
            m_AudioSourceShoot.volume = m_VolumeReload;
            m_AudioSourceShoot.clip = m_SoundReload;
            m_AudioSourceShoot.Play();
        }

        public void Shoot()
        {
            StartCoroutine(PlaySoundReload());
        }
    }
}