using UnityEngine;

public class Cube : MonoBehaviour
{
    public float SplitChance { get; private set; } = 1f;

    public void SetSplitChance(float value)
    {
        SplitChance = Mathf.Clamp01(value);
    }
}