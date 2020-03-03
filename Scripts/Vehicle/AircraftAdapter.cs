using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bibyter.Multitank
{
    public class AircraftAdapter : MonoBehaviour, IVehicleAdapter
    {
        public void OnBeginTransformation()
        {
            GetComponent<Animator>().SetTrigger("defaultState");
        }
    }
}