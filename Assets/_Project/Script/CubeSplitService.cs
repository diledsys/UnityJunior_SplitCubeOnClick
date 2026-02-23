using UnityEngine;

public sealed class CubeSplitService : MonoBehaviour
{
    private const float MinChance = 0f;
    private const float MaxChance = 1f;

    [Header("Children Count")] [SerializeField]
    private int _minChildren = 2;

    [SerializeField] private int _maxChildren = 6;

    [Header("Scaling")] [SerializeField] private float _childScaleFactor = 0.5f;

    [Header("Split Chance")] [SerializeField]
    private float _splitChanceDecay = 0.5f;

    public SplitResult Calculate(Vector3 parentScale, float parentSplitChance)
    {
        float clampedChance = Mathf.Clamp(parentSplitChance, MinChance, MaxChance);

        bool shouldSplit = Random.value <= clampedChance;

        if (!shouldSplit)
        {
            return SplitResult.Fail;
        }

        int childCount = Random.Range(_minChildren, _maxChildren + 1);

        Vector3 childScale = parentScale * _childScaleFactor;
        float childSplitChance = Mathf.Clamp(clampedChance * _splitChanceDecay, MinChance, MaxChance);

        return new SplitResult(true, childCount, childScale, childSplitChance);
    }

    public readonly struct SplitResult
    {
        public static readonly SplitResult Fail = new SplitResult(false, 0, Vector3.zero, 0f);

        public readonly bool ShouldSplit;
        public readonly int ChildCount;
        public readonly Vector3 ChildScale;
        public readonly float ChildSplitChance;

        public SplitResult(bool shouldSplit, int childCount, Vector3 childScale, float childSplitChance)
        {
            ShouldSplit = shouldSplit;
            ChildCount = childCount;
            ChildScale = childScale;
            ChildSplitChance = childSplitChance;
        }
    }
}