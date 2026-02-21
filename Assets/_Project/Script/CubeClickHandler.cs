using Di;
using UnityEngine;
using UnityEngine.Serialization;

namespace Di
{

    public class CubeClickHandler : MonoBehaviour
    {
        [SerializeField] private PointerInput input;
        [SerializeField] private RaycasterSelector selector;
        [FormerlySerializedAs("spawner")] [SerializeField] private CubeSplitterSpawner splitterSpawner;

        private void OnEnable()
        {
            input.Pressed += OnPressed;
        }

        private void OnDisable()
        {
            input.Pressed -= OnPressed;
        }

        private void OnPressed()
        {
            if (!selector.TryGetTarget(out var cube))
                return;

            splitterSpawner.SplitOrDisappear(cube);
        }
    }
}