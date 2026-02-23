using Di;
using System;
using UnityEngine;

public sealed class RaycastSelector : MonoBehaviour
{
    [SerializeField] private PointerInput _pointerInput;
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _cubeMask = ~0;

    public event Action<Cube> CubeSelected;

    private void OnEnable()
    {
        _pointerInput.Clicked += OnPointerClicked;
    }

    private void OnDisable()
    {
        _pointerInput.Clicked -= OnPointerClicked;
    }

    private void OnPointerClicked(Vector2 screenPosition)
    {
        Ray ray = _camera.ScreenPointToRay(screenPosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _cubeMask))
            return;

        if (!hit.collider.TryGetComponent(out Cube cube))
            return;

        CubeSelected?.Invoke(cube);
    }
}