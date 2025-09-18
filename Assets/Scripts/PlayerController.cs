using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonPlayer : MonoBehaviour
{
    public float speed = 5f;
    public ThirdPersonCamera cameraController;
    
    private Vector2 moveInput;
    
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    
    void Update()
    {
        if (moveInput == Vector2.zero) return;
        
        Vector3 forward = cameraController.GetCameraForward();
        Vector3 right = cameraController.GetCameraRight();
        Vector3 moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;
        
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
            transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
        }
    }
}