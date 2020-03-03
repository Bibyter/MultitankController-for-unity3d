using UnityEngine;

namespace Bibyter.Multitank
{
    public class TankBullet : MonoBehaviour
    {
        public float timeLife = 7f;
        public float speed = 2f;

        Vector3 velocity;
        Rigidbody rb3d;


        void Start()
        {
            rb3d = GetComponent<Rigidbody>();
            velocity = transform.forward * speed;
            Destroy(gameObject, timeLife);
        }

        void FixedUpdate()
        {
            rb3d.MovePosition(velocity);
        }

        void OnTriggerEnter(Collider other)
        {
            GetComponent<Collider>().enabled = false;
            velocity = Vector3.zero;
            Destroy(gameObject, 0.5f);
        }
    }
}