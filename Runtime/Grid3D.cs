using System;
using UnityEngine;

namespace WhiteArrow
{
    [Serializable]
    public class Grid3D
    {
        [SerializeField] private Vector3Int _size;
        [SerializeField] private Grid3DCells _cells = new();



        [HideInInspector]
        public Transform Origin;



        public Vector3Int SizeCell => _size;
        public int Capacity => _size.x * _size.y * _size.z;

        public float WidthWorld => _cells.GetGridWidthWorld(_size.x, Origin.lossyScale.x);
        public float DepthWorld => _cells.GetGridWidthWorld(_size.z, Origin.lossyScale.z);
        public float HeightWorld => _cells.GetGridWidthWorld(_size.y, Origin.lossyScale.y);
        public Vector3 SizeWorld => new(WidthWorld, HeightWorld, DepthWorld);
        public Vector3 CenterWorld => new Vector3(WidthWorld, HeightWorld, DepthWorld) * 0.5f - _cells.CellSize / 2;



        public Grid3D() { }

        public Grid3D(Grid3D template)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            _size = template._size;
            Origin = template.Origin;
            _cells = new(template._cells);
        }



        public Vector3Int GetCellPositionInGrid(int index)
        {
            var yIndex = index / (_size.x * _size.z);
            var xIndex = index / _size.z % _size.x;
            var zIndex = index % _size.z;

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