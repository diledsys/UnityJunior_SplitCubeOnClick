using System;
using UnityEngine;

namespace Di
{

    public class PointerInput : MonoBehaviour
    {
        public event Action Pressed;

        [Header("Mouse Button Index")]
        [SerializeField] private int pointerButtonIndex = 0;

        void Update()
        {
            if (Input.GetMouseButtonDown(pointerButtonIndex))
                Pressed?.Invoke();
        }
    }
}