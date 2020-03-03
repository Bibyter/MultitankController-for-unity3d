using UnityEngine;
using UnityEngine.Events;

namespace Bibyter.Multitank
{
    public class TankGun : MonoBehaviour
    {
        public float reloadTime = 6f;
        public float speedRotation = 30f;
        public float limitAngleUp = 10f;
        public float limitAngleDown = 10f;

        [Space]
        public TankBullet prefabBullet;
        public Transform startBulletPosition;

        [SerializeField] ParticleSystem shotParticle;

        
        public bool isReload { private set; get; }
        private float timerReload;


        void Start()
        {
            isReload = true;
            timerReload = reloadTime;
        }


        void Update()
        {
            if (isReload)
            {
                timerReload -= Time.deltaTime;

                if (timerReload <= 0f)
                {
                    timerReload = 0f;
                    isReload = false;
                }
            }
        }


        public void Shoot()
        {
            if (isReload) return;

            isReload = true;
            timerReload = reloadTime;
            Instantiate(prefabBullet.gameObject, startBulletPosition.position, startBulletPosition.rotation);

            shotParticle?.Play();
        }
    }
}