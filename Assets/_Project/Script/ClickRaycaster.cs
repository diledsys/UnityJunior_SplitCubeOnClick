using UnityEngine;
using System;
public class ClickRaycaster : MonoBehaviour
{
    public event Action<CubeView> CubeClicked;

    [SerializeField] private Camera targetCamera;
    [SerializeField] private LayerMask cubeLayers = ~0;
    [SerializeField] private float maxDistance = 1000f;

    private void Awake()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    public void RaycastFromPointer(Vector2 pointerScreenPosition)
    {
        Ray ray = targetCamera.ScreenPointToRay(pointerScreenPosition);

        if (Physics.Raycast(ray, out var hit, maxDistance, cubeLayers, QueryTriggerInteraction.Ignore) &&
            hit.collider.TryGetComponent<CubeView>(out var cube))
        {
            CubeClicked?.Invoke(cube);
        }
    }
}
