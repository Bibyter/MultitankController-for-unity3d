using UnityEngine;

namespace Bibyter.Multitank
{
    public class TankAdapter : MonoBehaviour, IVehicleAdapter
    {
        public TankSound tankSound;

        public void OnBeginTransformation()
        {
            GetComponent<TankInput>().enabled = false;
            tankSound.isRun = false;

            var ctrl = GetComponent<TankController>();
            ctrl.SetDirectionForTurret(transform.forward);
        }

        void OnEnable()
        {
            GetComponent<TankInput>().enabled = true;
            tankSound.isRun = true;
        }
    }
}