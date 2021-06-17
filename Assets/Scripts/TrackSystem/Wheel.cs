using UnityEngine;

namespace TrackSystem
{
     public class Wheel
        {
            private float _suspensionOffset;
            private Transform _wheelTransform;
            private Transform _boneTransform;
            private WheelCollider _collider;
            private Vector3 _wheelStartPos;
            private Vector3 _boneStartPos;
            private float _rotationAngle = 0.0f;
            private Quaternion _startWheelAngle;

            public WheelCollider Collider => _collider;

            public float RotationAngle => _rotationAngle;

            public Quaternion StartWheelAngle => _startWheelAngle;

            public Wheel(Transform transform, Transform bone)
            {
                _wheelTransform = transform;
                _boneTransform = bone;

                _suspensionOffset = 0.05f;

                _collider = _wheelTransform.gameObject.GetComponent<WheelCollider>();

                _wheelStartPos = transform.localPosition;
                _boneStartPos = bone.localPosition;
                _startWheelAngle = transform.localRotation;
            }

            public void Update(Transform tankTransform, float trackRpm)
            {
                _wheelTransform.localPosition = CalculateWheelPosition(_wheelTransform, _wheelStartPos, tankTransform);
                _boneTransform.localPosition = CalculateWheelPosition(_boneTransform, _boneStartPos, tankTransform);

                _rotationAngle = Mathf.Repeat(_rotationAngle + Time.fixedDeltaTime * trackRpm * 360f / 60f, 360f);
                _wheelTransform.localRotation =
                    Quaternion.Euler(_rotationAngle, _startWheelAngle.y, _startWheelAngle.z);
            }


            private Vector3 CalculateWheelPosition(Transform wheel, Vector3 startPos, Transform tankTransform)
            {
                WheelHit hit;

                Vector3 localPos = wheel.localPosition;
                if (_collider.GetGroundHit(out hit))
                {
                    localPos.y -= Vector3.Dot(wheel.position - hit.point, tankTransform.up) - _collider.radius;
                }
                else
                {
                    localPos.y = startPos.y - _suspensionOffset;
                }

                return localPos;
            }
        }
}