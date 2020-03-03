using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bibyter.Multitank
{
    public class Multicontroller
    {
        enum State { Default, Transformation }

        public Transform tank;
        public Transform helicopter;
        public Transform aircraft;
        public CameraFollow cameraFollow;

        Transform nextVehicle;
        Transform currentVehicle;

        float timerTrasition;
        State state;
        

        
        public void Init()
        {
            currentVehicle = tank;
            cameraFollow.target = tank;

            helicopter.gameObject.SetActive(false);
            aircraft.gameObject.SetActive(false);
        }

        public void Update()
        {
            switch (state)
            {
                case State.Default:
                    if (Input.GetKeyDown(KeyCode.H))
                        BeginReplaceVehicle(helicopter);

                    if (Input.GetKeyDown(KeyCode.T))
                        BeginReplaceVehicle(tank);

                    if (Input.GetKeyDown(KeyCode.F))
                        BeginReplaceVehicle(aircraft);
                    break;

                case State.Transformation:
                    timerTrasition += Time.deltaTime;

                    if (timerTrasition > 2f)
                    {
                        state = State.Default;
                        SetCameraTarget(nextVehicle);
                        ReplaceCurrentVehicle(nextVehicle);
                        nextVehicle = null;
                    }
                    break;
            }
        }

        void BeginReplaceVehicle(Transform next)
        {
            if (next == currentVehicle) return;

            nextVehicle = next;
            state = State.Transformation;
            timerTrasition = 0f;

            NotifyVehicleAboutTransition(currentVehicle);
        }

        void NotifyVehicleAboutTransition(Transform vehicle)
        {
            if (vehicle.TryGetComponent(out IVehicleAdapter vehicleAdapter))
                vehicleAdapter.OnBeginTransformation();
        }

        void SetCameraTarget(Transform vehicle)
        {
            var nAvatar = vehicle.GetComponent<VehicleAvatar>();
            cameraFollow.target = nAvatar ? nAvatar.cameraTarget : vehicle;
        }

        void ReplaceCurrentVehicle(Transform next)
        {
            currentVehicle.gameObject.SetActive(false);

            var position = currentVehicle.position;

            if (currentVehicle.TryGetComponent(out VehicleAvatar cAvatar))
            {
                position -= currentVehicle.rotation * cAvatar.spawnOffset;
            }

            if (next.TryGetComponent(out VehicleAvatar nAvatar))
            {
                position += currentVehicle.rotation * nAvatar.spawnOffset;
            }

            next.SetPositionAndRotation(position, currentVehicle.rotation);

            var nextRb = next.GetComponent<Rigidbody>();
            nextRb.rotation = currentVehicle.rotation;
            nextRb.position = position;

            next.gameObject.SetActive(true);

            currentVehicle = next;
        }
    }
}
