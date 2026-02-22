using UnityEngine;

namespace Di
{
    public class CubeClickHandler : MonoBehaviour
    {
        [SerializeField] private PointerInput input;
        [SerializeField] private RaycasterSelector selector;
        [SerializeField] private CubeSplitService splitService;

        private void OnEnable()
        {
            if (input != null)
                input.Pressed += OnPressed;
        }

        private void OnDisable()
        {
            if (input != null)
                input.Pressed -= OnPressed;
        }

        private void OnPressed()
        {
            if (selector == null || splitService == null)
                return;

            if (!selector.TryGetTarget(out Cube cube))
                return;

            splitService.SplitOrDisappear(cube);
        }
    }
}