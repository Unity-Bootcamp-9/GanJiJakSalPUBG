using UnityEngine;

public class Player_LEJ : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    public GameObject viewField;

    [SerializeField]
    private Rigidbody rb;
    private float rotationY;
    private float rotationX;

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        rotationY += Input.GetAxis("Mouse X") * rotationSpeed * 1.3f * Time.deltaTime;
        rotationX += Input.GetAxis("Mouse Y") * rotationSpeed * 0.1f * Time.deltaTime;

        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        if (!Input.GetKey(KeyCode.LeftAlt))
        {
            transform.eulerAngles = new Vector3(0, rotationY, 0);
        }

        viewField.transform.localRotation = Quaternion.Euler(-rotationX, 0f, 0f);

        Vector3 movement = transform.forward * moveY + transform.right * moveX;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }
}
