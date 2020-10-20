using System;
using UnityEngine;

namespace Game
{
    public class CameraFaceLooker : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private bool _isAutoDetectCamera = true;
        private void Awake()
        {
            if (_isAutoDetectCamera == true)
            {
                _camera = FindObjectOfType<Camera>();
            }
            if (_camera == null)
            {
                _camera = Camera.main;
            }
        }

        private void Update()
        {
            if (_camera == null)
                return;
            
            Vector3 posToLook = transform.position + _camera.transform.forward;
            transform.LookAt(posToLook);
        }

        private void OnDrawGizmos()
        {
            if (_camera == null)
            {
                return;
            }
            
            Gizmos.color = Color.red;
            var dir = _camera.transform.position + _camera.transform.forward;
            Gizmos.DrawLine(_camera.transform.position, dir);
        }
    }
}

