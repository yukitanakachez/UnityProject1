using UnityEngine;

public class PlanetOrbit : MonoBehaviour
{
    [Header("Orbit Settings")]
    public Transform sun;     
    public float orbitSpeed;  
    public float orbitRadius;   

    [Header("Rotation Settings")]
    public float rotationSpeed; 

    private float currentAngle; 

    void Start()
    {
        if (sun != null)
        {
            Vector3 offset = new Vector3(orbitRadius, 0, 0);
            transform.position = sun.position + offset;
        }
    }

    void Update()
    {
        if (sun != null)
        {
            currentAngle += orbitSpeed * Time.deltaTime;
            float rad = currentAngle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * orbitRadius;
            transform.position = sun.position + offset;
        }
        
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}