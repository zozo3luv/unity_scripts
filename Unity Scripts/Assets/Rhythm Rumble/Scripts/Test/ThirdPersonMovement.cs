using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 6.0f;
    public float turnSmoothTime = 0.1f;
    public float jumpHeight = 2.0f;
    public float gravity = -9.81f;

    private float turnSmoothVelocity;
    private float verticalVelocity;
    private bool isGrounded;

    void Update()
    {
        // Check if the player is grounded
        isGrounded = controller.isGrounded;
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = 0f;
        }

        // Get input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Apply gravity
        verticalVelocity += gravity * Time.deltaTime;
        Vector3 gravityVector = new Vector3(0, verticalVelocity, 0);

        if (direction.magnitude >= 0.1f)
        {
            // Calculate target angle based on camera orientation
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Move the player
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime + gravityVector * Time.deltaTime);
        }
        else
        {
            // Apply gravity when not moving
            controller.Move(gravityVector * Time.deltaTime);
        }

        // Jumping mechanic
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
