using UnityEngine;
using UnityEngine.EventSystems;

namespace Bibyter
{
    public class CameraFollow : MonoBehaviour
    {
        public enum ModeMove { FixedPos, smooth };


        public ModeMove modeMove = ModeMove.FixedPos;
        public float speedMove = 1f;
        public int countSteps = 10;
        public int startStep = 3;
        public Transform target;

        
        Transform pivot;
        Transform tCamera;
        int currentApproximation = 0;



        void Start()
        {
            pivot = transform.Find("Pivot");
            tCamera = pivot.Find("MainCamera");

            if (startStep <= countSteps)
                for (int i = 0; i < startStep; i++)
                    ShiftApproximation(-1);
        }


        void LateUpdate()
        {
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");


            if (h != 0f)
                transform.Rotate(0f, h, 0f, Space.Self);
            if (v != 0f)
            {
                if (pivot.localEulerAngles.x < 180f)
                {
                    if (pivot.localEulerAngles.y == 180f)
                        if (v > 0f) v = 0f;
                }
                else
                {
                    if (pivot.localEulerAngles.x < 360f)
                        if (v < 0f) v = 0f;
                }

                pivot.Rotate(v, 0f, 0f, Space.Self);
            }

            // apprpoximation
            float r = Input.GetAxisRaw("Mouse ScrollWheel");

            if (r < 0f)
            {
                if (currentApproximation < countSteps)
                    ShiftApproximation(-1);
            }
            else if (r > 0f)
            {
                if (currentApproximation > 0)
                    ShiftApproximation(1);
            }

            // move
            if (modeMove == ModeMove.FixedPos)
                transform.position = target.position;
            else
                transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speedMove);
        }


        private void ShiftApproximation(int i)
        {
            Vector3 curPos = tCamera.localPosition;
            curPos.z += i * 2f;
            curPos.y += i / 2f;
            tCamera.localPosition = curPos;
            currentApproximation -= i;
        }
    }
}