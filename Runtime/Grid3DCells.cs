using System;
using UnityEngine;

namespace WhiteArrow
{
    [Serializable]
    public class Grid3DCells
    {
        [SerializeField] private Vector3 _cellSize;
        [SerializeField] private Vector3 _spacing;



        /// <summary>
        /// Creates an empty cells settings instance for serialization.
        /// </summary>
        public Grid3DCells() { }

        /// <summary>
        /// Creates a copy of an existing cells settings instance.
        /// </summary>
        public Grid3DCells(Grid3DCells template)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            _cellSize = template._cellSize;
            _spacing = template._spacing;
        }



        /// <summary>
        /// Returns cell size scaled into world units using the provided origin transform.
        /// </summary>
        public Vector3 GetCellSizeInWorld(Transform origin)
        {
            return new Vector3(
                _cellSize.x * origin.lossyScale.x,
                _cellSize.y * origin.lossyScale.y,
                _cellSize.z * origin.lossyScale.z
            );
        }

        /// <summary>
        /// Returns cell spacing scaled into world units using the provided origin transform.
        /// </summary>
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
