using UnityEngine;

namespace Bibyter.Multitank
{
    public class TankCaterpillar : MonoBehaviour
    {
        public float m_offsetRollersY = 0.06f;
        public Transform m_meshRollerForward;
        public Transform m_meshRollerBack;
        public Transform[] m_bonesRollers;
        public Transform[] m_meshesRollers;
        public WheelCollider[] m_rollersColliders;

        private SkinnedMeshRenderer caterpillarRenderer;
        private Vector2 offsetTextureCaterpillar;
        private float lastTotalRPM;
        private float transferValueBackRoller;
        private float transferValueTopRoller;
        private float radiusBackRoller;


        private void Start()
        {
            caterpillarRenderer = GetComponent<SkinnedMeshRenderer>();

            transferValueBackRoller = TransferValue(m_meshRollerBack.GetComponent<Renderer>(), m_meshesRollers[0].GetComponent<Renderer>());
            transferValueTopRoller = TransferValue(m_meshRollerForward.GetComponent<Renderer>(), m_meshesRollers[0].GetComponent<Renderer>());
            radiusBackRoller = m_meshRollerBack.GetComponent<Renderer>().bounds.size.y / 2f;
        }


        private void Update()
        {
            float deltaAngle = CalkTotalRPM() * 6.0f * Time.deltaTime;

            for (int i = 0; i < m_meshesRollers.Length; i++)
                m_meshesRollers[i].Rotate(deltaAngle, 0f, 0f, Space.Self);

            m_meshRollerForward.Rotate(deltaAngle * transferValueTopRoller, 0f, 0f, Space.Self);
            m_meshRollerBack.Rotate(deltaAngle * transferValueBackRoller, 0f, 0f, Space.Self);

            TranslateTexture(deltaAngle);
        }


        private void FixedUpdate()
        {
            for (int i = 0; i < m_rollersColliders.Length; i++)
            {
                m_rollersColliders[i].GetWorldPose(out Vector3 pos, out Quaternion _);
                pos.y += m_offsetRollersY;
                m_bonesRollers[i].position = pos;
                m_meshesRollers[i].position = pos;
            }
        }


        private float TransferValue(Renderer main, Renderer ren2)
        {
            float r1 = main.bounds.size.y;
            float r2 = ren2.bounds.size.y;

            return r2 / r1;
        }

        private void TranslateTexture(float angle)
        {
            offsetTextureCaterpillar.y -= angle * radiusBackRoller * Time.deltaTime;
            if (offsetTextureCaterpillar.y < -3f)
                offsetTextureCaterpillar.y += 3f;
            caterpillarRenderer.material.mainTextureOffset = offsetTextureCaterpillar;
        }


        private float CalkTotalRPM()
        {
            float totalRPM = 0f;
            int countWheelOnGround = 0;

            for (int i = 0; i < m_rollersColliders.Length; i++)
            {
                if (m_rollersColliders[i].isGrounded)
                {
                    totalRPM += m_rollersColliders[i].rpm;
                    countWheelOnGround++;
                }
            }

            if (countWheelOnGround == 0)
            {
                totalRPM = lastTotalRPM;
            }
            else
            {
                totalRPM /= countWheelOnGround;
                lastTotalRPM = totalRPM;
            }

            return totalRPM;
        }


        public void SetTorques(float motorTorque, float brakeTorque)
        {
            for (int i = 0; i < m_rollersColliders.Length; i++)
            {
                m_rollersColliders[i].brakeTorque = brakeTorque;
                m_rollersColliders[i].motorTorque = motorTorque;
            }
        }
    }
}