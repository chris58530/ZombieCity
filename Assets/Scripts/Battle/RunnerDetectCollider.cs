using System;
using UnityEngine;

public class RunnerDetectCollider : MonoBehaviour
{
    public Action onDetectRunner;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<BuffRunner>(out var buffRunner))
        {
            onDetectRunner?.Invoke();
        }
    }
}
