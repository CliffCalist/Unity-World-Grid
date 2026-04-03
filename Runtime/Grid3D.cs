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



        private Transform _origin;



        public Transform Origin => _origin;
        public Vector3Int SizeInCells => _sizeInCells;
        public int Capacity => _sizeInCells.x * _sizeInCells.y * _sizeInCells.z;
        public Grid3DCells Cells => _cells;




        /// <summary>
        /// Creates an empty grid instance for serialization and manual setup.
        /// </summary>
        public Grid3D() { }

        /// <summary>
        /// Creates a copy of an existing grid instance.
        /// </summary>
        public Grid3D(Grid3D template)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            _sizeInCells = template._sizeInCells;
            _origin = template._origin;
            _cells = new(template._cells);
        }



        /// <summary>
        /// Initializes this grid with a non-null origin transform.
        /// </summary>
        public void Init(Transform origin)
        {
            _origin = origin ?? throw new ArgumentNullException(nameof(origin));
        }



        #region Cells Sizes
        /// <summary>
        /// Returns one cell size in world units using the current origin scale.
        /// </summary>
        public Vector3 GetCellSizeInWorld()
        {
            EnsureInitialized();
            return _cells.GetCellSizeInWorld(_origin);
        }

        /// <summary>
        /// Returns spacing between cells in world units using the current origin scale.
        /// </summary>
        public Vector3 GetCellSpacingInWorld()
        {
            EnsureInitialized();
            return _cells.GetCellSpacingInWorld(_origin);
        }
        #endregion



        #region Cells Positions
        /// <summary>
        /// Converts a linear index into 3D cell coordinates in the grid.
        /// </summary>
        public Vector3Int GetCellPositionInGrid(int index)
        {
            EnsureInitialized();
            var yIndex = index / (_sizeInCells.x * _sizeInCells.z);
            var xIndex = index / _sizeInCells.z % _sizeInCells.x;
            var zIndex = index % _sizeInCells.z;

            return new Vector3Int(xIndex, yIndex, zIndex);
        }



        /// <summary>
        /// Returns a cell offset in origin space in world units.
        /// Does not apply origin translation or rotation.
        /// </summary>
        public Vector3 GetCellPositionInOriginSpace(Vector3Int positionInGrid)
        {
            EnsureInitialized();
            var cellSize = GetCellSizeInWorld();
            var cellSpacing = GetCellSpacingInWorld();

            var xPos = positionInGrid.x * (cellSize.x + cellSpacing.x);
            var yPos = positionInGrid.y * (cellSize.y + cellSpacing.y);
            var zPos = positionInGrid.z * (cellSize.z + cellSpacing.z);

            return new Vector3(xPos, yPos, zPos);
        }



        /// <summary>
        /// Returns a cell world position by linear index.
        /// </summary>
        public Vector3 GetCellPositionInWorld(int index)
        {
            EnsureInitialized();
            var gridPosition = GetCellPositionInGrid(index);
            return GetCellPositionInWorld(gridPosition);
        }

        /// <summary>
        /// Returns a cell world position by applying the origin transform to the origin-space offset.
        /// </summary>
        public Vector3 GetCellPositionInWorld(Vector3Int positionInGrid)
        {
            EnsureInitialized();
            var localPosition = GetCellPositionInOriginSpace(positionInGrid);
            var worldPosition = _origin.TransformPoint(localPosition);
            return worldPosition;
        }
        #endregion



        #region Grid Sizes
        /// <summary>
        /// Returns total grid size in world units.
        /// </summary>
        public Vector3 GetGridSizeInWorld()
        {
            EnsureInitialized();
            var cellSize = GetCellSizeInWorld();
            var cellSpacing = GetCellSpacingInWorld();

            var sizeX = GetGridAxisSizeInWorld(
                _sizeInCells.x,
                cellSize.x,
                cellSpacing.x,
                _origin.lossyScale.x
            );

            var sizeY = GetGridAxisSizeInWorld(
                _sizeInCells.y,
                cellSize.y,
                cellSpacing.y,
                _origin.lossyScale.y
            );

            var sizeZ = GetGridAxisSizeInWorld(
                _sizeInCells.z,
                cellSize.z,
                cellSpacing.z,
                _origin.lossyScale.z
            );

            return new Vector3(sizeX, sizeY, sizeZ);
        }

        /// <summary>
        /// Returns total grid width in world units (X axis).
        /// </summary>
        public float GetGridWidthInWorld()
        {
            EnsureInitialized();
            return GetGridAxisSizeInWorld(
                _sizeInCells.x,
                GetCellSizeInWorld().x,
                GetCellSpacingInWorld().x,
                _origin.lossyScale.x
            );
        }

        /// <summary>
        /// Returns total grid depth in world units (Z axis).
        /// </summary>
        public float GetGridDepthInWorld()
        {
            EnsureInitialized();
            return GetGridAxisSizeInWorld(
                _sizeInCells.z,
                GetCellSizeInWorld().z,
                GetCellSpacingInWorld().z,
                _origin.lossyScale.z
            );
        }

        /// <summary>
        /// Returns total grid height in world units (Y axis).
        /// </summary>
        public float GetGridHeightInWorld()
        {
            EnsureInitialized();
            return GetGridAxisSizeInWorld(
                _sizeInCells.y,
                GetCellSizeInWorld().y,
                GetCellSpacingInWorld().y,
                _origin.lossyScale.y
            );
        }

        /// <summary>
        /// Calculates world size for a single axis using cell count, cell size, spacing, and scale.
        /// </summary>
        private float GetGridAxisSizeInWorld(int sizeInCells, float cellSize, float spacing, float scale)
        {
            var cells = sizeInCells * cellSize;
            var spacingSize = (sizeInCells - 1) * spacing;
            return (cells + spacingSize) * scale;
        }
        #endregion



        #region Grid Positions
        /// <summary>
        /// Returns the grid center offset in origin space, expressed in world units.
        /// </summary>
        public Vector3 GetGridCenterInWorld()
        {
            EnsureInitialized();
            return GetGridSizeInWorld() * 0.5f - GetCellSizeInWorld() / 2;
        }
        #endregion



#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            EnsureInitialized();
            Gizmos.color = Color.green;

            var cellSize = GetCellSizeInWorld();
            var previousMatrix = Gizmos.matrix;
            for (int i = 0; i < Capacity; i++)
            {
                var position = GetCellPositionInWorld(i);
                Gizmos.matrix = Matrix4x4.TRS(position, _origin.rotation, Vector3.one);
                Gizmos.DrawWireCube(Vector3.zero, cellSize);
            }

            Gizmos.matrix = previousMatrix;
        }
#endif



        private void EnsureInitialized()
        {
            if (_origin == null)
                throw new InvalidOperationException($"{nameof(Grid3D)} is not initialized. Call {nameof(Init)} before using it.");
        }
    }
}
