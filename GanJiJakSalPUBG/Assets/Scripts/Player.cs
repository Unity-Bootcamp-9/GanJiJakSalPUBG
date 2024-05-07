using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    [SerializeField]
    private Rigidbody rb;
    private float moveX;
    private float moveY;
    private float rotationY;

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        rotationY += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

        transform.eulerAngles = new Vector3(0, rotationY, 0);
    }

    void FixedUpdate()
    {
        Vector3 movement = transform.forward * moveY + transform.right * moveX;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }
}
