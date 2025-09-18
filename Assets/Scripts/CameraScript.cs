using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 7f;
    public float mouseSensitivity = 2f;
    
    private float mouseX;
    private float mouseY;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void LateUpdate()
    {
        if (!target) return;
        
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        mouseX += mouseDelta.x * mouseSensitivity * Time.deltaTime;
        mouseY -= mouseDelta.y * mouseSensitivity * Time.deltaTime;
        mouseY = Mathf.Clamp(mouseY, -80f, 80f);
        
        Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0f);
        Vector3 position = target.position - rotation * Vector3.forward * distance;
        
        transform.position = position;
        transform.LookAt(target);
    }
    
    public Vector3 GetCameraForward()
    {
        return Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
    }
    
    public Vector3 GetCameraRight()
    {
        return Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized;
    }
}