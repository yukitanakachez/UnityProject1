using UnityEngine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 moveInput;

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);

        if (move != Vector3.zero)
        {
            // Face the movement direction instantly
            transform.rotation = Quaternion.LookRotation(move, Vector3.up);

            // Move the character
            transform.Translate(move.normalized * speed * Time.deltaTime, Space.World);
        }
    }
}