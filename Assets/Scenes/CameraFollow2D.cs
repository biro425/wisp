using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField] private Transform target;     // 따라갈 대상(플레이어)
    [SerializeField] private Vector2 offset = new Vector2(0f, 0f);
    [SerializeField, Range(0f, 1f)] private float smooth = 0.15f; // 0=즉시, 1=매우 느림

    private Vector3 velocity = Vector3.zero;
    private float initialZ;

    private void Awake()
    {
        initialZ = transform.position.z; // 카메라 Z 고정
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = new Vector3(target.position.x + offset.x,
                                       target.position.y + offset.y,
                                       initialZ);

        transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smooth);
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }
}
