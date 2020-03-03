using UnityEngine;

namespace Bibyter.Multitank
{
    public class TankTurret : MonoBehaviour
    {
        public float speedTurret = 40f;
        public Transform root;

        [HideInInspector]
        public Vector3 goalDirection;


        void FixedUpdate()
        {
            var dir = root.InverseTransformDirection(goalDirection);
            dir.y = 0f;

            transform.localRotation = Quaternion.RotateTowards(
                transform.localRotation,
                Quaternion.LookRotation(dir, Vector3.up),
                Time.deltaTime * speedTurret
            );
        }
    }
}