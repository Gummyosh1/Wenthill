using UnityEngine;

public class MaskFloat : MonoBehaviour
{
    private float height = 0.2f;
    private float cyclesPerSecond = 0.3f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.localPosition;
    }

    private void Update()
    {
        float offset = Mathf.Sin(
            Time.time * cyclesPerSecond * Mathf.PI * 2f
        ) * height;

        transform.localPosition =
            startPosition + Vector3.up * offset;
    }
}
