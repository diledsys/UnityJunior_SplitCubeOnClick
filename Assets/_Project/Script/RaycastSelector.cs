
using UnityEngine;

namespace Di
{
    public class RaycasterSelector : MonoBehaviour
    {
        [SerializeField] private Camera targetCamera;
        [SerializeField] private LayerMask targLayerMask = ~0;
        [SerializeField] private float maxDistance = 1000f;

        void Awake()
        {
            if (targetCamera == null)
                targetCamera = Camera.main;
        }

        public bool TryGetTarget(out Cube cube)
        {
            Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, targLayerMask, QueryTriggerInteraction.Ignore) &&
                hit.collider.TryGetComponent(out cube))
            {
                return true;
            }

            cube = null;
            return false;
        }
    }
}
