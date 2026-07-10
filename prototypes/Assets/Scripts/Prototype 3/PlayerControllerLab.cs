using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerLab : MonoBehaviour
{
    public float speed;
    public InputAction moveAction;

    private Rigidbody playerRb;

    private Vector2 moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction.Enable();
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        float verticalInput = moveInput.x;
        float horizontalInput = moveInput.y;

        Vector3 moveDirection = new(horizontalInput, 0, verticalInput);

        playerRb.AddForce(moveDirection * speed);
    }
}
