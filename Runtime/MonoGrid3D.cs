using UnityEngine;

namespace WhiteArrow
{
    public class MonoGrid3D : MonoBehaviour
    {
        [SerializeField] private Grid3D _grid;

        public Grid3D Core => _grid;


        private void Awake()
        {
            if (_grid.Origin.Transform != transform)
                _grid.Origin.Transform = transform;
        }



#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_grid.Origin.Transform == transform)
            {
                _grid.Origin.Transform = transform;
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }

        private void OnDrawGizmos()
        {
            _grid.OnDrawGizmos();
        }
#endif
    }
}