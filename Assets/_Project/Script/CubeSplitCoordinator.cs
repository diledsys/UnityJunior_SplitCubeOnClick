using UnityEngine;

public class CubeSplitCoordinator : MonoBehaviour
{
    [SerializeField] private PointerInput _input;
    [SerializeField] private CubeRaycaster _raycaster;
    [SerializeField] private CubeSpawner _spawner;
    [SerializeField] private ExplosionApplier _exploder;

    [SerializeField] private int _minChildren = 2;
    [SerializeField] private int _maxChildren = 6;

    private const float HalfChance = 0.5f;

    private void OnEnable()
    {
        _input.Pressed += OnPressed;
        _raycaster.CubeHit += OnCubeHit;
    }

    private void OnDisable()
    {
        _input.Pressed -= OnPressed;
        _raycaster.CubeHit -= OnCubeHit;
    }

    private void OnPressed()
    {
        _raycaster.RaycastFrom(Input.mousePosition);
    }

    private void OnCubeHit(Cube cube)
    {
        if (Random.value > cube.SplitChance)
        {
            _spawner.Remove(cube);
            return;
        }

        int count = Random.Range(_minChildren, _maxChildren + 1);
        float nextChance = cube.SplitChance * HalfChance;

        Vector3 center = cube.transform.position;
        Vector3 childScale = cube.transform.localScale * _spawner.ChildScaleFactor;

        var bodies = _spawner.SpawnChildren(cube, count, nextChance);

        _exploder.Explode(center, bodies, childScale);

        _spawner.Remove(cube);
    }
}