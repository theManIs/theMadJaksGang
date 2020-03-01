using System;
using Assets.TeamProjects.GamePrimal.SeparateComponents.InterfaceHold;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.CameraRigs.CamerasScripts
{
    [Serializable]
    public struct ClampVector3
    {
        public float Init;
        public float Min;
        public float Max;
    }

    public class FreeLookCamWithUserInput : PivotBasedCameraRig
    {
        // This script is designed to be placed on the root object of a camera rig,
        // comprising 3 gameobjects, each parented to the next:

        // 	Camera Rig
        // 		Pivot
        // 			Camera

        public ClampVector3 DistanceAgainstTarget = new ClampVector3() {Min = 14, Max = 17, Init = 14};
        public ClampVector3 TiltClampVector3 = new ClampVector3() {Min = 30, Max = 45, Init = 35};

        [Space]

        [Range(0f, 5f)] public float FollowTargetSpeed = 1f;
        [Range(0f, 1f)] public float CameraMovementSpeed = .2f;
        [Range(0f, 5f)] public float AngleRotationSpeed = 1.5f;
        public bool DoNotLooseTarget = false;
        public float m_TurnSmoothing = 0.0f;
        public bool m_LockCursor = false;
        public int FallowThreshold = 8;
        public int MaxRaycastDistance = 100;

        private float m_LookAngle;                    // The rig's y axis rotation.
        private float m_TiltAngle;                    // The pivot's x axis rotation.
        private Vector3 m_PivotEulers;
        private Quaternion m_PivotTargetRot;
        private Quaternion m_TransformTargetRot;
        private Camera localCamera;

        private float Scroll => Input.GetAxis("Mouse ScrollWheel");
        private float Horizontal => Input.GetAxis("Horizontal");
        private float Vertical => Input.GetAxis("Vertical");
        private bool Mouse2 => Input.GetKey(KeyCode.Mouse2);
        private bool Mouse0 => Input.GetKey(KeyCode.Mouse0);

        protected override void Awake()
        {
            base.Awake();
            // Lock or unlock the cursor.
            Cursor.lockState = m_LockCursor ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !m_LockCursor;
            m_PivotEulers = m_Pivot.rotation.eulerAngles;
            //            StartTilt = m_PivotEulers.x;
            m_Pivot.rotation = Quaternion.Euler(TiltClampVector3.Init, m_PivotEulers.y, m_PivotEulers.z);

            m_PivotTargetRot = m_Pivot.transform.localRotation;
            m_TransformTargetRot = transform.localRotation;
            localCamera = GetComponentInChildren<Camera>();
        }

        protected override void Start()
        {
            base.Start();
            BringToInitialZoom();

            m_TiltAngle = TiltClampVector3.Init;
        }

        protected void Update()
        {
            MoveCameraWithInput();

            if (Math.Abs(Scroll) > 0)
                ZoomInOut();

            if (Mouse2)
                HandleRotationMovement();

            if (m_LockCursor && Mouse0)
            {
                Cursor.lockState = m_LockCursor ? CursorLockMode.Locked : CursorLockMode.None;
                Cursor.visible = !m_LockCursor;
            }
        }

        private void BringToInitialZoom()
        {
            float diffDistance =  Vector3.Distance(m_Cam.position, transform.position) - DistanceAgainstTarget.Init;

            m_Cam.Translate(Vector3.forward * diffDistance, Space.Self);
        }

        private void ZoomInOut()
        {
            int iSign = Math.Sign(this.Scroll);
            Ray ray = localCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            float zoomDistance = 500 * this.Scroll * Time.deltaTime;

            if (Physics.Raycast(ray, out RaycastHit hit, MaxRaycastDistance))
                if (hit.distance > DistanceAgainstTarget.Min && iSign > 0 || iSign < 0 && hit.distance < DistanceAgainstTarget.Max)
//                    transform.Translate(ray.direction * zoomDistance, Space.World);
                    m_Cam.Translate(Vector3.forward * zoomDistance, Space.Self);
        }

        private void MoveCameraWithInput()
        {
            Vector3 MoveStep = Vertical * transform.forward * CameraMovementSpeed + Horizontal * transform.right * CameraMovementSpeed;
            //Debug.Log(MoveStep + " " + transform.position);
            transform.position = MoveStep + transform.position;
        }

        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void ReleaseTargetUnconditional()
        {
            bool previousLock = DoNotLooseTarget;
            DoNotLooseTarget = false;

            SetTarget(null);

            DoNotLooseTarget = previousLock;
        }

        public override void SetTarget(Transform newTransform)
        {
            if (DoNotLooseTarget && !newTransform)
                return;
            else
                base.SetTarget(newTransform);
        }

        protected override void FollowTarget(float deltaTime)
        {
            if (!Horizontal.Equals(0.0f) || !Vertical.Equals(0.0f))
                ReleaseTargetUnconditional();

            if (m_Target == null) return;

            // Move the rig towards target position.
//            Quaternion rotToward = Quaternion.FromToRotation(transform.forward, m_Target.position.normalized);
            transform.position = Vector3.Lerp(transform.position, m_Target.position, deltaTime * FollowTargetSpeed);
//            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotToward, Time.deltaTime);
//            transform.rotation = Quaternion.LookRotation(m_Target.transform.position.normalized, Vector3.up);
        }

        public bool HasReachedTarget()
        {
            if (!m_Target) return false;
            //            DebugInfo.Log(Math.Abs(transform.position.sqrMagnitude - m_Target.position.sqrMagnitude) + " " + FallowThreshold);
            return Math.Abs(transform.position.sqrMagnitude - m_Target.position.sqrMagnitude) < FallowThreshold;
        }


        private void HandleRotationMovement()
        {
            if (Time.timeScale < float.Epsilon)
                return;

            // Read the user input
            var x = Input.GetAxis("Mouse X");
            var y = Input.GetAxis("Mouse Y");

            // Adjust the look angle by an amount proportional to the turn speed and horizontal input.
            m_LookAngle += x * AngleRotationSpeed;

            // Rotate the rig (the root object) around Y axis only:
            m_TransformTargetRot = Quaternion.Euler(0f, m_LookAngle, 0f);


//            if (m_VerticalAutoReturn)
//            {
//                // For tilt input, we need to behave differently depending on whether we're using mouse or touch input:
//                // on mobile, vertical input is directly mapped to tilt value, so it springs back automatically when the look input is released
//                // we have to test whether above or below zero because we want to auto-return to zero even if min and max are not symmetrical.
//                m_TiltAngle = y > 0 ? Mathf.Lerp(0, -TiltClampVector3.Min, y) : Mathf.Lerp(0, TiltClampVector3.Max, -y);
//            }
//            else
//            {
                // on platforms with a mouse, we adjust the current angle based on Y mouse input and turn speed
                m_TiltAngle -= y * AngleRotationSpeed;
                // and make sure the new value is within the tilt range
                m_TiltAngle = Mathf.Clamp(m_TiltAngle, TiltClampVector3.Min, TiltClampVector3.Max);
//            }

            // Tilt input around X is applied to the pivot (the child of this object)
            m_PivotTargetRot = Quaternion.Euler(m_TiltAngle, m_PivotEulers.y, m_PivotEulers.z);
//            m_PivotTargetRot = Quaternion.Euler(_constantTiltInt, m_PivotEulers.y, m_PivotEulers.z);

            if (m_TurnSmoothing > 0)
            {
                m_Pivot.localRotation = Quaternion.Slerp(m_Pivot.localRotation, m_PivotTargetRot, m_TurnSmoothing * Time.deltaTime);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, m_TransformTargetRot, m_TurnSmoothing * Time.deltaTime);
            }
            else
            {
                m_Pivot.localRotation = m_PivotTargetRot;
                transform.localRotation = m_TransformTargetRot;
            }
        }
    }
}
