using UnityEngine;

namespace Bibyter.Multitank
{
    public class TankController : MonoBehaviour
    {
        public float motorTorqueMax = 400f;
        public float brakeTorqueMax = 2000f;
        public Vector2 minMaxSpeed = new Vector2(-25f, 50f);
        [Space]
        public float steerAngleMax = 13;
        public float steerAngleRange = 5;
        [Space]
        public TankGun tankGun;
        public TankTurret tankTurret;
        public TankSound tankSound;
        public TankCaterpillar caterpillarLeft;
        public TankCaterpillar caterpillarRight;
        [Space]
        [SerializeField] bool showDegubPanel;

        [HideInInspector]
        public Vector3 goalDirection;

        Rigidbody rb3d;
        TankGUIDebug guiDebug;

        public float motorTorque01 { private set; get; }
        public float speedKMH { private set; get; }


        void Start()
        {
            guiDebug = new TankGUIDebug();
            rb3d = GetComponent<Rigidbody>();
            
            InitCenterOfMass();
        }

        void OnGUI()
        {
            if (showDegubPanel) guiDebug.UpdateGUI();
        }

        void InitCenterOfMass()
        {
            var com = transform.Find("COM");
            if (com == null)
                Debug.LogError("not find centerOfMass transform", gameObject);
            else
                rb3d.centerOfMass = transform.Find("COM").localPosition;
        }

        float CalkulateSpeed(Vector3 velocity)
        {
            if (velocity.x != 0f && velocity.z != 0f)
            {
                float magnitudeKMH = Mathf.Sqrt(velocity.x * velocity.x + velocity.z * velocity.z) * 3.6f;
                return Vector3.Dot(velocity, transform.forward) > 0f ? magnitudeKMH : -magnitudeKMH;
            }
            else
                return 0f;
        }

        void AddRecoilOfShoot()
        {
            Vector3 force = -tankTurret.transform.forward * 10000f;
            var pos = tankTurret.transform.position;
            rb3d.AddForceAtPosition(force, pos, ForceMode.Impulse);
        }

        public void Move(float accel, float steer, bool brake)
        {
            speedKMH = CalkulateSpeed(rb3d.velocity);
            float speed01 = Mathf.Clamp01(speedKMH > 0f ? speedKMH / minMaxSpeed.y : speedKMH / minMaxSpeed.x);
            motorTorque01 = Mathf.Clamp01(speed01 + (Mathf.Sqrt(steer*steer + accel*accel) * 0.3f));

            float steerAbs = steer < 0f ? -steer : steer;
            float sideFriction = 1f;
            float motorL = 0f, motorR = 0f;
            float brakeL = 0f, brakeR = 0f;
            float steerAngleTop = 0f, steerAngleBack = 0f;

            

            if (brake)
            {
                brakeL = brakeR = brakeTorqueMax;
            }
            else if (accel != 0f)
            {
                // чем больше скорость танка, тем меньше значениe + газ
                float coef1 = (1f - speed01) * accel;
                motorL = motorR = motorTorqueMax * coef1;
            }

            if (steer != 0f)
            {
                float steerAngle = (steerAngleMax * (1f - speed01) + steerAngleRange) * steer;
                steerAngleTop = steerAngle;
                steerAngleBack = -steerAngle;
            }

            if (showDegubPanel)
            {
                guiDebug.brakeL = brakeL;
                guiDebug.motorL = motorL;
                guiDebug.brakeR = brakeR;
                guiDebug.motorR = motorR;
                guiDebug.friction = sideFriction;
                guiDebug.steerAngle = steerAngleTop;
                guiDebug.speed = speedKMH;
            }

            var leftRollers = caterpillarLeft.m_rollersColliders;
            leftRollers[0].steerAngle = steerAngleTop;
            leftRollers[1].steerAngle = steerAngleTop * 0.9f;
            leftRollers[leftRollers.Length - 1].steerAngle = steerAngleBack;
            leftRollers[leftRollers.Length - 2].steerAngle = steerAngleBack * 0.9f;

            var rightRollers = caterpillarRight.m_rollersColliders;
            rightRollers[0].steerAngle = steerAngleTop;
            rightRollers[1].steerAngle = steerAngleTop * 0.9f;
            rightRollers[leftRollers.Length - 1].steerAngle = steerAngleBack;
            rightRollers[leftRollers.Length - 2].steerAngle = steerAngleBack * 0.9f;

            caterpillarLeft.SetTorques(motorL, brakeL);
            caterpillarRight.SetTorques(motorR, brakeR);
        }

        public void Shoot()
        {
            if (tankGun.isReload) return;

            tankGun.Shoot();
            tankSound.Shoot();
            AddRecoilOfShoot();
        }

        public void SetDirectionForTurret(Vector3 dir)
        {
            tankTurret.goalDirection = dir;
        }
    }

    public class TankGUIDebug
    {
        public float motorL = 0f;
        public float motorR = 0f;
        public float brakeL = 0f;
        public float brakeR = 0f;

        public float friction;
        public float steerAngle;
        public float speed;

        public void UpdateGUI()
        {
            GUI.Box(new Rect(10, 10, 200, 200), string.Empty);
            GUILayout.BeginArea(new Rect(12, 10, 200, 200));

            var w = GUILayout.Width(100);

            GUILayout.BeginHorizontal();
            GUILayout.Label($"motor {motorL}", w); GUILayout.Label($"motor {motorR}", w);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Width(100));
            GUILayout.Label($"brake {brakeL}", w); GUILayout.Label($"brake {brakeR}", w);
            GUILayout.EndHorizontal();


            GUILayout.Label($"hor {Input.GetAxis("Horizontal")}");
            GUILayout.Label($"friction {friction}");
            GUILayout.Label($"steerAngle {steerAngle}");
            GUILayout.Label($"speed {speed}");
            GUILayout.EndArea();
        }
    }
}