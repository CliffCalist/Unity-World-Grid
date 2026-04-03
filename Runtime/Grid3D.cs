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



        [HideInInspector]
        public Transform Origin;



        public Vector3Int SizeInCells => _sizeInCells;
        public int Capacity => _sizeInCells.x * _sizeInCells.y * _sizeInCells.z;

        public float WidthInWorld => _cells.GetGridWidthInWorld(_sizeInCells.x, Origin.lossyScale.x);
        public float DepthInWorld => _cells.GetGridWidthInWorld(_sizeInCells.z, Origin.lossyScale.z);
        public float HeightInWorld => _cells.GetGridWidthInWorld(_sizeInCells.y, Origin.lossyScale.y);
        public Vector3 SizeInWorld => new(WidthInWorld, HeightInWorld, DepthInWorld);
        public Vector3 CenterInWorld => new Vector3(WidthInWorld, HeightInWorld, DepthInWorld) * 0.5f - _cells.CellSize / 2;



        public Grid3D() { }

        public Grid3D(Grid3D template)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            _sizeInCells = template._sizeInCells;
            Origin = template.Origin;
            _cells = new(template._cells);
        }



        public Vector3Int GetCellPositionInGrid(int index)
        {
            var yIndex = index / (_sizeInCells.x * _sizeInCells.z);
            var xIndex = index / _sizeInCells.z % _sizeInCells.x;
            var zIndex = index % _sizeInCells.z;

            return new Vector3Int(xIndex, yIndex, zIndex);
        }



        public Vector3 GetCellPositionInWorld(Vector3Int gridPosition)
        {
            var localPosition = _cells.GetCellPositionLocal(gridPosition, Origin.lossyScale);
            var worldPosition = Origin.position + Origin.rotation * localPosition;
            return worldPosition;
        }

        public Vector3 GetCellPositionInWorld(int index)
        {
            var gridPosition = GetCellPositionInGrid(index);
            return GetCellPositionInWorld(gridPosition);
        }



#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            var cellSize = _cells.GetScaledCellSize(Origin.lossyScale);
            for (int i = 0; i < Capacity; i++)
            {
                var position = GetCellPositionInWorld(i);
                Gizmos.DrawWireCube(position, cellSize);
            }
        }
#endif
    }
}