using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody), typeof(Renderer))]
public class CubeView : MonoBehaviour
{
    [SerializeField] private int generation;
    public int Generation => generation;

    public Rigidbody Rigidbody { get; private set; }
    public Renderer Renderer { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Renderer = GetComponent<Renderer>();
    }

    public void SetGeneration(int value) => generation = value;
}
