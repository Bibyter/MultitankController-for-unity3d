using UnityEngine;

namespace Bibyter.Multitank
{
    public class TankExhaustParticle : MonoBehaviour
    {
        public TankController tankController;

        ParticleSystem _particleSystem;

        void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        void FixedUpdate()
        {
            float colorSmokeRange = 0.5f - tankController.motorTorque01 * 0.5f;
            Color colorSmoke;
            colorSmoke.r = colorSmokeRange;
            colorSmoke.g = colorSmokeRange;
            colorSmoke.b = colorSmokeRange;
            colorSmoke.a = 1f;

            _particleSystem.startColor = colorSmoke;
        }
    }
}