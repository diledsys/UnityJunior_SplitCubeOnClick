using UnityEngine;

public sealed class CubeClickHandler : MonoBehaviour
{
    [SerializeField] private RaycastSelector _raycastSelector;

    private void OnEnable()
    {
        _raycastSelector.CubeSelected += OnCubeSelected;
    }

    private void OnDisable()
    {
        _raycastSelector.CubeSelected -= OnCubeSelected;
    }

    private void OnCubeSelected(Cube cube)
    {
        if (cube == null)
            return;

        cube.NotifyClicked();
    }
}