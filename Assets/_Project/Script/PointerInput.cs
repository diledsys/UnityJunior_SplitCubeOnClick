using System;
using UnityEngine;

public class PointerInput : MonoBehaviour
{
    [SerializeField] private int _pointerButtonIndex = 0;

    public event Action Pressed;

    private void Update()
    {
        if (Input.GetMouseButtonDown(_pointerButtonIndex))
            Pressed?.Invoke();
    }
}