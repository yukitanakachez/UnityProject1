using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform target;           // The player to follow
    public float distance = 7f;        // Distance from player
    public float height = 5f;          // Height above player
    public float mouseSensitivity = 2f; // Mouse sensitivity
    public float minVerticalAngle = -30f; // Minimum vertical look angle
    public float maxVerticalAngle = 60f;  // Maximum vertical look angle
    
    [Header("Smoothing")]
    public float rotationSmoothing = 5f;
    public float positionSmoothing = 5f;
    
    private float horizontalAngle = 0f;
    private float verticalAngle = 0f;
    private Vector3 currentPosition;
    private Quaternion currentRotation;
    private Vector2 mouseInput;
    
    void Start()
    {
        if (target == null)
        {
            Debug.LogError("No target assigned to ThirdPersonCamera!");
            return;
        }
        
        // Initialize angles based on current transform
        Vector3 angles = transform.eulerAngles;
        horizontalAngle = angles.y;
        verticalAngle = angles.x;
        
        // Initialize current values
        currentPosition = transform.position;
        currentRotation = transform.rotation;
        
        // Lock cursor to center of screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void LateUpdate()
    {
        if (target == null) return;
        
        HandleMouseInput();
        UpdateCameraPosition();
    }
    
    void HandleMouseInput()
    {
        // Get mouse input from Input System
        mouseInput = Mouse.current.delta.ReadValue();
        
        float mouseX = mouseInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseInput.y * mouseSensitivity * Time.deltaTime;
        
        // Update angles
        horizontalAngle += mouseX;
        verticalAngle -= mouseY; // Inverted for natural camera movement
        
        // Clamp vertical angle
        verticalAngle = Mathf.Clamp(verticalAngle, minVerticalAngle, maxVerticalAngle);
    }
    
    void UpdateCameraPosition()
    {
        // Calculate desired position
        Quaternion rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0f);
        Vector3 direction = rotation * Vector3.back; // Use back because camera looks forward
        Vector3 desiredPosition = target.position + direction * distance + Vector3.up * height;
        
        // Smooth the position and rotation
        currentPosition = Vector3.Lerp(currentPosition, desiredPosition, positionSmoothing * Time.deltaTime);
        currentRotation = Quaternion.Lerp(currentRotation, rotation, rotationSmoothing * Time.deltaTime);
        
        // Apply to transform
        transform.position = currentPosition;
        transform.rotation = currentRotation;
    }
    
    // Public method to get the camera's forward direction (useful for player movement)
    public Vector3 GetCameraForward()
    {
        return Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
    }
    
    // Public method to get the camera's right direction
    public Vector3 GetCameraRight()
    {
        return Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized;
    }
    
    void OnValidate()
    {
        // Ensure sensible values in the inspector
        distance = Mathf.Max(distance, 1f);
        mouseSensitivity = Mathf.Max(mouseSensitivity, 0.1f);
        minVerticalAngle = Mathf.Clamp(minVerticalAngle, -90f, 0f);
        maxVerticalAngle = Mathf.Clamp(maxVerticalAngle, 0f, 90f);
    }
}