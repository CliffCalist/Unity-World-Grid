using UnityEngine;

namespace WhiteArrow
{
    public class MonoGrid3D : MonoBehaviour
    {
        [SerializeField] private Grid3D _grid = new();



        public Grid3D Grid => _grid;



        private void Awake()
        {
            EnsureGridInitialized();
        }



#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            EnsureGridInitialized();
            _grid.OnDrawGizmos();
        }
#endif

        private bool EnsureGridInitialized()
        {
            var wasNull = _grid == null;
            _grid ??= new Grid3D();

            var originChanged = _grid.Origin != transform;
            if (originChanged)
                _grid.Init(transform);

            return wasNull || originChanged;
        }
    }
}
