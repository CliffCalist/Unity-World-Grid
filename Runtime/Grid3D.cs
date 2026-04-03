using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace WhiteArrow
{
    [Serializable]
    public class Grid3D
    {
        [FormerlySerializedAs("_size")]
        [SerializeField] private Vector3Int _sizeInCells;

        [SerializeField] private Grid3DCells _cells = new();

        [HideInInspector] public Transform Origin;



        public Vector3Int SizeInCells => _sizeInCells;
        public int Capacity => _sizeInCells.x * _sizeInCells.y * _sizeInCells.z;

        public Grid3DCells Cells => _cells;



        public Grid3D() { }

        public Grid3D(Grid3D template)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            _sizeInCells = template._sizeInCells;
            Origin = template.Origin;
            _cells = new(template._cells);
        }



        #region Cells Sizes
        public Vector3 GetCellSizeInWorld()
        {
            return _cells.GetCellSizeInWorld(Origin);
        }

        public Vector3 GetCellSpacingInWorld()
        {
            return _cells.GetCellSpacingInWorld(Origin);
        }
        #endregion



        #region Cells Positions
        public Vector3Int GetCellPositionInGrid(int index)
        {
            var yIndex = index / (_sizeInCells.x * _sizeInCells.z);
            var xIndex = index / _sizeInCells.z % _sizeInCells.x;
            var zIndex = index % _sizeInCells.z;

            return new Vector3Int(xIndex, yIndex, zIndex);
        }



        public Vector3 GetCellPositionLocalWorld(Vector3Int positionInGrid)
        {
            var cellSize = GetCellSizeInWorld();
            var cellSpacing = GetCellSpacingInWorld();

            var xPos = positionInGrid.x * (cellSize.x + cellSpacing.x);
            var yPos = positionInGrid.y * (cellSize.y + cellSpacing.y);
            var zPos = positionInGrid.z * (cellSize.z + cellSpacing.z);

            return new Vector3(xPos, yPos, zPos);
        }



        public Vector3 GetCellPositionInWorld(int index)
        {
            var gridPosition = GetCellPositionInGrid(index);
            return GetCellPositionInWorld(gridPosition);
        }

        public Vector3 GetCellPositionInWorld(Vector3Int positionInGrid)
        {
            var localPosition = GetCellPositionLocalWorld(positionInGrid);
            var worldPosition = Origin.position + Origin.rotation * localPosition;
            return worldPosition;
        }
        #endregion



        #region Grid Sizes
        public Vector3 GetGridSizeInWorld()
        {
            var cellSize = GetCellSizeInWorld();
            var cellSpacing = GetCellSpacingInWorld();

            var sizeX = GetGridAxisSizeInWorld(
                _sizeInCells.x,
                cellSize.x,
                cellSpacing.x,
                Origin.lossyScale.x
            );

            var sizeY = GetGridAxisSizeInWorld(
                _sizeInCells.y,
                cellSize.y,
                cellSpacing.y,
                Origin.lossyScale.y
            );

            var sizeZ = GetGridAxisSizeInWorld(
                _sizeInCells.z,
                cellSize.z,
                cellSpacing.z,
                Origin.lossyScale.z
            );

            return new Vector3(sizeX, sizeY, sizeZ);
        }

        public float GetGridWidthInWorld()
        {
            return GetGridAxisSizeInWorld(
                _sizeInCells.x,
                GetCellSizeInWorld().x,
                GetCellSpacingInWorld().x,
                Origin.lossyScale.x
            );
        }

        public float GetGridDepthInWorld()
        {
            return GetGridAxisSizeInWorld(
                _sizeInCells.z,
                GetCellSizeInWorld().z,
                GetCellSpacingInWorld().z,
                Origin.lossyScale.z
            );
        }

        public float GetGridHeightInWorld()
        {
            return GetGridAxisSizeInWorld(
                _sizeInCells.y,
                GetCellSizeInWorld().y,
                GetCellSpacingInWorld().y,
                Origin.lossyScale.y
            );
        }

        private float GetGridAxisSizeInWorld(int sizeInCells, float cellSize, float spacing, float scale)
        {
            var cells = sizeInCells * cellSize;
            var spacingSize = (sizeInCells - 1) * spacing;
            return (cells + spacingSize) * scale;
        }
        #endregion



        #region Grid Positions
        public Vector3 GetGridCenterInWorld()
        {
            return GetGridSizeInWorld() * 0.5f - GetCellSizeInWorld() / 2;
        }
        #endregion



#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            var cellSize = GetCellSizeInWorld();
            for (int i = 0; i < Capacity; i++)
            {
                var position = GetCellPositionInWorld(i);
                Gizmos.DrawWireCube(position, cellSize);
            }
        }
#endif
    }
}