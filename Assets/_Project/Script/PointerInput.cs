using System;
using UnityEngine;

namespace Di
{
    public class PointerInput : MonoBehaviour
    {
        public event Action<Vector2> Clicked;

        [Header("Mouse Button Index")] [SerializeField]
        private int _pointerButtonIndex = 0;

        private void Update()
        {
            if (Input.GetMouseButtonDown(_pointerButtonIndex))
                Clicked?.Invoke(Input.mousePosition);
        }
    }
}