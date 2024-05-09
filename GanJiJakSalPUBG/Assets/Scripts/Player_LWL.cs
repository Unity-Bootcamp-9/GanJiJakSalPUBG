using UnityEngine;

public class PlayerController_LWL : MonoBehaviour
{
    private Camera mainCamera;
    private Vector2 input;

    public float turnSpeed = 10f;
    private float turnSpeedMultiplier;

    public float moveSpeed;
    private Rigidbody rb;

    private void Start()
    {
        mainCamera = Camera.main;
        rb= GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        Vector3 movement = transform.forward * input.y + transform.right * input.x;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }
}
