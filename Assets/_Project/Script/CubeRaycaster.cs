using System;
using UnityEngine;

public class CubeRaycaster : MonoBehaviour
{
    [SerializeField] private Camera _targetCamera;
    [SerializeField] private LayerMask _cubeLayers = ~0;
    [SerializeField] private float _maxDistance = 1000f;

    public event Action<Cube> CubeHit;

    private void Awake()
    {
        if (_targetCamera == null)
            _targetCamera = Camera.main;
    }

    public void RaycastFrom(Vector2 screenPosition)
    {
        Ray ray = _targetCamera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out var hit, _maxDistance, _cubeLayers, QueryTriggerInteraction.Ignore) &&
            hit.collider.TryGetComponent(out Cube cube))
        {
            CubeHit?.Invoke(cube);
        }
    }
}