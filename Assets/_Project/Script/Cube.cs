using System;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public sealed class Cube : MonoBehaviour
{
    private const float MinSplitChance = 0f;
    private const float MaxSplitChance = 1f;

    [SerializeField, Range(MinSplitChance, MaxSplitChance)]
    private float _splitChance = 1f;

    private Rigidbody _rigidbody;
    private Renderer _renderer;

    public event Action<Cube> Clicked;

    public float SplitChance => _splitChance;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
    }

    public void Initialize(
        float splitChance,
        Vector3 scale,
        float mass,
        Color color,
        int layer,
        bool useGravity = true)
    {
        _splitChance = Mathf.Clamp(splitChance, MinSplitChance, MaxSplitChance);

        transform.localScale = scale;
        gameObject.layer = layer;

        ConfigurePhysics(mass, useGravity);
        ApplyColor(color);
    }

    public void NotifyClicked()
    {
        Clicked?.Invoke(this);
    }

    private void ConfigurePhysics(float mass, bool useGravity)
    {
        _rigidbody.mass = mass;
        _rigidbody.useGravity = useGravity;
        _rigidbody.isKinematic = false;
    }

    private void ApplyColor(Color color)
    {
        if (_renderer == null)
            return;

        _renderer.material.color = color;
    }
}