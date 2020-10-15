using System;
using UnityEngine;

namespace Game.DebugHelpers
{
    public class GizmosHighlighter : MonoBehaviour
    {
        [SerializeField] private bool _isSpherical = true;
        [SerializeField] private bool _isBox = false;
        [SerializeField] private Color _color = Color.yellow;
        [SerializeField] private float _size = 0.5f;
        private void OnDrawGizmos()
        {
            Gizmos.color = _color;
            
            if (_isSpherical)
            {
                Gizmos.DrawSphere(transform.position, _size);
            }

            if (_isBox)
            {
                Gizmos.DrawCube(transform.position, Vector3.one * _size);
            }
        }
    }

}
