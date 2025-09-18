using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonPlayer : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float rotationSpeed = 10f;
    
    [Header("References")]
    public ThirdPersonCamera cameraController;
    
    private Vector2 moveInput;
    private CharacterController characterController;
    
    void Start()
    {
        // Try to get CharacterController component
        characterController = GetComponent<CharacterController>();
        
        // If no camera reference is set, try to find one
        if (cameraController == null)
        {
            cameraController = FindObjectOfType<ThirdPersonCamera>();
            if (cameraController == null)
            {
                Debug.LogWarning("No ThirdPersonCamera found! Player movement will use world directions instead of camera-relative movement.");
            }
        }
    }
    
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    
    void Update()
    {
        HandleMovement();
    }
    
    void HandleMovement()
    {
        if (moveInput == Vector2.zero) return;
        
        Vector3 moveDirection;
        
        // Calculate movement direction relative to camera
        if (cameraController != null)
        {
            // Get camera directions
            Vector3 cameraForward = cameraController.GetCameraForward();
            Vector3 cameraRight = cameraController.GetCameraRight();
            
            // Calculate movement direction relative to camera
            moveDirection = (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;
        }
        else
        {
            // Fallback to world directions if no camera
            moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        }
        
        // Only rotate and move if there's actual movement
        if (moveDirection != Vector3.zero)
        {
            // Rotate player to face movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
            // Move the player
            if (characterController != null)
            {
                // Use CharacterController if available (handles collisions better)
                Vector3 moveVelocity = moveDirection * speed;
                
                // Add gravity if using CharacterController
                if (!characterController.isGrounded)
                {
                    moveVelocity.y = -9.81f;
                }
                
                characterController.Move(moveVelocity * Time.deltaTime);
            }
            else
            {
                // Fallback to transform movement
                transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
            }
        }
    }
    
    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}