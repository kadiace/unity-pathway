using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Movement tuning (editable in Inspector)
    public float speed = 5f;
    public float turnSpeed = 5;

    // Input System action exposed in Inspector for binding (WASD/Arrow keys)
    public InputAction MoveAction;

    // Current input value (x = left/right, y = forward/back), kept private for internal use
    private Vector2 moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Enable the MoveAction so it starts reading input
        MoveAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        // Read the 2D vector from the MoveAction (x: horizaontal, y: vertical)
        moveInput = MoveAction.ReadValue<Vector2>();

        // Move forward/back along local Z using the y component
        transform.Translate(Time.deltaTime * speed * moveInput.y * Vector3.forward);

        // Rotate arount local Y (yaw) using the x component
        transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * moveInput.x);
    }
}
