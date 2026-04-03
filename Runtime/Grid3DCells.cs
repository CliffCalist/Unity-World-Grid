using System;
using UnityEngine;

namespace WhiteArrow
{
    [Serializable]
    public class Grid3DCells
    {
        [SerializeField] private Vector3 _cellSize;
        [SerializeField] private Vector3 _spacing;



        public Grid3DCells() { }

        public Grid3DCells(Grid3DCells template)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            _cellSize = template._cellSize;
            _spacing = template._spacing;
        }



        public Vector3 GetCellSizeInWorld(Transform origin)
        {
            return new Vector3(
                _cellSize.x * origin.lossyScale.x,
                _cellSize.y * origin.lossyScale.y,
                _cellSize.z * origin.lossyScale.z
            );
        }

        public Vector3 GetCellSpacingInWorld(Transform origin)
        {
            return new Vector3(
                _spacing.x * origin.lossyScale.x,
                _spacing.y * origin.lossyScale.y,
                _spacing.z * origin.lossyScale.z
            );
        }
    }
}