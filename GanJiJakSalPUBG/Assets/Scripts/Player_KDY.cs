using Unity.VisualScripting;
using UnityEngine;

public class PlayerController_KDY : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    public GameObject viewField;

    [SerializeField]
    private Rigidbody rb;
    private float moveX;
    private float moveY;
    private float rotationY;
    [SerializeField]
    private float rotationX;

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        rotationY += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        rotationX += Input.GetAxis("Mouse Y") * rotationSpeed * 0.1f * Time.deltaTime;

        rotationX = Mathf.Clamp(rotationX, -1, 90f);

        transform.eulerAngles = new Vector3(0, rotationY, 0);
        viewField.transform.position = new Vector3(viewField.transform.position.x, rotationX, viewField.transform.position.z);
    }

    void FixedUpdate()
    {
        Vector3 movement = transform.forward * moveY + transform.right * moveX;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }
}
