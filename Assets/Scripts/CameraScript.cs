using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;   // drag your player here
    public Vector3 offset = new Vector3(0f, 5f, -7f);

    void LateUpdate()
    {
        if (!target) return;

        transform.position = target.position + offset;
        transform.LookAt(target);
    }
}