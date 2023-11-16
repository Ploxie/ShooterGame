using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player
{
    
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField, Range(0, 360.0f)] private float _yaw;
        [SerializeField, Range(0, 90.0f)] private float _pitch = 60.0f;

        [Header("Zoom")]
        [SerializeField, Range(10.0f, 50.0f)] private float _zoom = 25.0f;

        [Header("Aim")]
        [SerializeField, Range(0, 0.5f)] private float _aimInterpolation = 0.1f;

        private Camera _camera;
        private Assets.Scripts.Entity.Player _player;

        private Plane plane = new Plane(Vector2.down, 0f);

        private void Awake()
        {
            _camera = Camera.main;
            _player = GetComponent<Assets.Scripts.Entity.Player>();
        }

        private void LateUpdate()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            plane.Raycast(ray, out float distance);
            Vector3 worldPosition = ray.GetPoint(distance);

            Quaternion lookRotation = Quaternion.Euler(_pitch, _yaw, 0.0f);

            Vector3 targetPosition = transform.position + ((worldPosition - transform.position) * _aimInterpolation);
            Vector3 lookDirection = lookRotation * Vector3.forward;
            Vector3 lookPosition = targetPosition - lookDirection * _zoom;

            _camera.transform.SetPositionAndRotation(lookPosition, lookRotation);
        }

    }
}
