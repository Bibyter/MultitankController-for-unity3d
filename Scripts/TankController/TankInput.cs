using UnityEngine;
using UnityEngine.UI;

namespace Bibyter.Multitank
{
    public class TankInput : MonoBehaviour
    {
        public TankController tankController;

        [HideInInspector]
        public Transform directionForGun;


        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                tankController.Shoot();

            tankController.SetDirectionForTurret(directionForGun.forward);
        }


        private void FixedUpdate()
        {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");
            bool b = Input.GetKey(KeyCode.Space);

            tankController.Move(v, h, b);
        }
    }
}