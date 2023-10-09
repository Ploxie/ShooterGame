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
        private PlayerController _playerController;

        private void Awake()
        {
            _camera = Camera.main;
            _playerController = GetComponent<PlayerController>();
        }

        private void LateUpdate()
        {
            Quaternion lookRotation = Quaternion.Euler(_pitch, _yaw, 0.0f);

            Vector3 targetPosition = transform.position + ((_playerController.AimPosition - transform.position) * _aimInterpolation);
            Vector3 lookDirection = lookRotation * Vector3.forward;
            Vector3 lookPosition = targetPosition - lookDirection * _zoom;

            _camera.transform.SetPositionAndRotation(lookPosition, lookRotation);
        }

    }
}
