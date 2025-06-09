using System;
using UnityEngine;

namespace WhiteArrow
{
    [System.Serializable]
    public class Grid3DOrigin
    {
        [SerializeField] private Transform _transform;

        [Space]
        [SerializeField] private Vector3 _manualPosition;
        [SerializeField] private Quaternion _manualRotation = Quaternion.identity;
        [SerializeField] private Vector3 _manualScale = Vector3.one;



        public bool IsUsingTransform => _transform != null;
        public Transform Transform
        {
            get => _transform;
            set => _transform = value;
        }



        public Vector3 Position => _transform != null ? _transform.position : _manualPosition;
        public Quaternion Rotation => _transform != null ? _transform.rotation : _manualRotation;
        public Vector3 Scale => _transform != null ? _transform.lossyScale : _manualScale;



        public Grid3DOrigin() { }

        public Grid3DOrigin(Grid3DOrigin template)
        {
            if (template is null)
                throw new ArgumentNullException(nameof(template));

            _transform = template._transform;
            _manualPosition = template._manualPosition;
            _manualRotation = template._manualRotation;
            _manualScale = template._manualScale;
        }
    }
}