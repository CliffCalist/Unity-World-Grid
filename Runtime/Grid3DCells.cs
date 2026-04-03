using System;
using UnityEngine;

namespace WhiteArrow
{
    [Serializable]
    public class Grid3DCells
    {
        [SerializeField] private Vector3 _cellSize;
        [SerializeField] private Vector3 _spacing;


        public Vector3 CellSize => _cellSize;
        public Vector3 Spacing => _spacing;




        public Grid3DCells() { }

        public Grid3DCells(Grid3DCells template)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            _cellSize = template._cellSize;
            _spacing = template._spacing;
        }



        public Vector3 GetCellPositionLocal(Vector3Int positionInGrid, Vector3 scale)
        {
            var scaledCellSize = GetCellSize(scale);

            var scaledSpacing = new Vector3(
                _spacing.x * scale.x,
                _spacing.y * scale.y,
                _spacing.z * scale.z
            );

            var xPos = positionInGrid.x * (scaledCellSize.x + scaledSpacing.x);
            var yPos = positionInGrid.y * (scaledCellSize.y + scaledSpacing.y);
            var zPos = positionInGrid.z * (scaledCellSize.z + scaledSpacing.z);

            return new Vector3(xPos, yPos, zPos);
        }



        public float GetGridWidthWorld(int widthInCells, float scale = 1)
        {
            return GetGridSizeWorldAxis(widthInCells, _cellSize.x, _spacing.x, scale);
        }

        public float GetGridDepthWorld(int depthInCells, float scale = 1)
        {
            return GetGridSizeWorldAxis(depthInCells, _cellSize.z, _spacing.z, scale);
        }

        public float GetGridHeightWorld(int heightInCells, float scale = 1)
        {
            return GetGridSizeWorldAxis(heightInCells, _cellSize.y, _spacing.y, scale);
        }

        private float GetGridSizeWorldAxis(int sizeInCells, float cellSize, float spacing, float scale = 1)
        {
            var cells = sizeInCells * cellSize;
            var spacingSize = (sizeInCells - 1) * spacing;
            return (cells + spacingSize) * scale;
        }



        public Vector3 GetCellSize(Vector3 scale)
        {
            return new Vector3(
                _cellSize.x * scale.x,
                _cellSize.y * scale.y,
                _cellSize.z * scale.z
            );
        }
    }
}