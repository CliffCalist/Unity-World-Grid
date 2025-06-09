using System;
using UnityEngine;

namespace WhiteArrow
{
    [Serializable]
    public class Grid3D
    {
        [SerializeField] private Vector3Int _size;
        [SerializeField] private Grid3DOrigin _origin;
        [SerializeField] private Grid3DCells _cells;



        public Grid3DOrigin Origin => _origin;

        public Vector3Int Size => _size;
        public int Capacity => _size.x * _size.y * _size.z;

        public float WorldWidth => _cells.CalculateGridWidthInWorld(_size.x, _origin.Scale.x);
        public float WorldDepth => _cells.CalculateGridWidthInWorld(_size.z, _origin.Scale.z);
        public float WorldHeight => _cells.CalculateGridWidthInWorld(_size.y, _origin.Scale.y);
        public Vector3 WorldSize => new(WorldWidth, WorldHeight, WorldDepth);


        public Vector3 LocalCenter => new Vector3(WorldWidth, WorldHeight, WorldDepth) * 0.5f - _cells.CellSize / 2;



        public Grid3D() { }

        public Grid3D(Grid3D template)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            _size = template._size;
            _origin = new(template._origin);
            _cells = new(template._cells);
        }



        public Vector3Int GetCellPositionInGrid(int index)
        {
            var yIndex = index / (_size.x * _size.z);
            var xIndex = index / _size.z % _size.x;
            var zIndex = index % _size.z;

            return new Vector3Int(xIndex, yIndex, zIndex);
        }



        public Vector3 GetCellPositionInWorld(Vector3Int gridPosition)
        {
            var localPosition = _cells.GetCellPositionInWorld(gridPosition, _origin.Scale);
            var worldPosition = _origin.Position + _origin.Rotation * localPosition;
            return worldPosition;
        }

        public Vector3 GetCellPositionInWorld(int index)
        {
            var gridPosition = GetCellPositionInGrid(index);
            return GetCellPositionInWorld(gridPosition);
        }



        public float Min(params float[] valuesParams)
        {
            var min = float.MaxValue;
            foreach (var value in valuesParams)
            {
                if (value < min)
                    min = value;
            }

            return min;
        }

        public Vector3 ClampVectorAxes(Vector3 vector, float maxValue)
        {
            return new Vector3(
                Mathf.Min(vector.x, maxValue),
                Mathf.Min(vector.y, maxValue),
                Mathf.Min(vector.z, maxValue));
        }



#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            var cellSize = _cells.GetCellSize(_origin.Scale);
            for (int i = 0; i < Capacity; i++)
            {
                var position = GetCellPositionInWorld(i);
                Gizmos.DrawWireCube(position, cellSize);
            }
        }
#endif
    }
}