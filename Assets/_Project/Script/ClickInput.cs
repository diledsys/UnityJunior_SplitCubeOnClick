using UnityEngine;

public class ClickInput : MonoBehaviour
{
    public event System.Action Activated;
    [Header("0=À Ã, 1=œ Ã")]
    [SerializeField] private int activationButtonIndex = 0;

    private void Update()
    {
        if (Input.GetMouseButtonDown(activationButtonIndex))
            Activated?.Invoke();
    }
}
