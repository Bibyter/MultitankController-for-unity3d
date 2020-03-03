using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bibyter.Multitank
{
   

    public class GameStartup : MonoBehaviour
    {
        public Transform tank;
        public Transform helicopter;
        public Transform aircraft;

        public Transform directionGun;

        Multicontroller multicontroller;

        void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            var camera = FindObjectOfType<CameraFollow>();

            var tankInput = tank.GetComponent<TankInput>();
            tankInput.directionForGun = directionGun;

            multicontroller = new Multicontroller {
                tank = this.tank,
                helicopter = this.helicopter,
                aircraft = this.aircraft,
                cameraFollow = camera
            };
            multicontroller.Init();
        }

        void Update()
        {
            multicontroller.Update();
        }
    }
}