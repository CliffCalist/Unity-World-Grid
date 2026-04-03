using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace WhiteArrow
{
    [Serializable]
    public class Grid3D
    {
        [FormerlySerializedAs("_size")]
        [SerializeField] private Vector3Int _sizeCell;

        [SerializeField] private Grid3DCells _cells = new();



        [HideInInspector]
        public Transform Origin;



        public Vector3Int SizeCell => _sizeCell;
        public int Capacity => _sizeCell.x * _sizeCell.y * _sizeCell.z;

        public float WidthWorld => _cells.GetGridWidthWorld(_sizeCell.x, Origin.lossyScale.x);
        public float DepthWorld => _cells.GetGridWidthWorld(_sizeCell.z, Origin.lossyScale.z);
        public float HeightWorld => _cells.GetGridWidthWorld(_sizeCell.y, Origin.lossyScale.y);
        public Vector3 SizeWorld => new(WidthWorld, HeightWorld, DepthWorld);
        public Vector3 CenterWorld => new Vector3(WidthWorld, HeightWorld, DepthWorld) * 0.5f - _cells.CellSize / 2;



        public Grid3D() { }

        public Grid3D(Grid3D template)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            _sizeCell = template._sizeCell;
            Origin = template.Origin;
            _cells = new(template._cells);
        }



        public Vector3Int GetCellPositionInGrid(int index)
        {
            var yIndex = index / (_sizeCell.x * _sizeCell.z);
            var xIndex = index / _sizeCell.z % _sizeCell.x;
            var zIndex = index % _sizeCell.z;

            return new Vector3Int(xIndex, yIndex, zIndex);
        }



        public Vector3 GetCellPositionWorld(Vector3Int gridPosition)
        {
            var localPosition = _cells.GetCellPositionLocal(gridPosition, Origin.lossyScale);
            var worldPosition = Origin.position + Origin.rotation * localPosition;
            return worldPosition;
        }

        public Vector3 GetCellPositionWorld(int index)
        {
            var gridPosition = GetCellPositionInGrid(index);
            return GetCellPositionWorld(gridPosition);
        }



#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            var cellSize = _cells.GetCellSize(Origin.lossyScale);
            for (int i = 0; i < Capacity; i++)
            {
                var position = GetCellPositionWorld(i);
                Gizmos.DrawWireCube(position, cellSize);
            }
        }
#endif
    }
}