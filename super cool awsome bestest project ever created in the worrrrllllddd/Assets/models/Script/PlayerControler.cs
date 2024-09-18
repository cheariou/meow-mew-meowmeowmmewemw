using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    Rigidbody myRB;
    Camera playerCam;

    Vector2 camRotation;
    [Header("Player Stats")]
        public int health = 5;
    public int maxHealth = 10;
    public int healtPickupAmt = 5;

    [Header("weapon stats")]
    public Transform WeaponSlot;

    [Header("Movement Stats")]
    public bool sprinting = false;
    public bool sprintToggle = false;
    public float speed = 10f;
    public float sprintMult = 1.5f;
    public float jumpHeight = 5f;
    public float groundDection = 1f;
    public float mouseSensitivity = 2.0f;
    public float xsensitivity = 2.0f;
    public float ysensitivity = 2.0f;

    [Header("User Settings")]
    public float camrotationLimit = 90f;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        playerCam = transform.GetChild(0).GetComponent<Camera>();

        camRotation = Vector2.zero;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        camRotation.x += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        camRotation.y += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        camRotation.y = Mathf.Clamp(camRotation.y, -90, 90);

        playerCam.transform.localRotation = Quaternion.AngleAxis(camRotation.y, Vector3.left);
        transform.localRotation = Quaternion.AngleAxis(camRotation.x, Vector3.up);

        if (!sprinting && !sprintToggle && Input.GetKey(KeyCode.LeftShift))
            sprinting = true;

        if (!sprinting && sprintToggle && (Input.GetAxisRaw("vertical") > 0) && Input.GetKey(KeyCode.LeftShift))
            sprinting = true;

        Vector3 temp = myRB.velocity;

        temp.x = Input.GetAxisRaw("Horizontal") * speed;
        temp.z = Input.GetAxisRaw("Vertical") * speed;

        if (sprinting)
            temp.z *= sprintMult;

        if (sprinting && Input.GetKeyUp(KeyCode.LeftShift))
            sprinting = false;

        if (sprinting && sprintToggle && (Input.GetAxisRaw("vertical") <= 0))
            sprinting = false;

        if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(transform.position, -transform.up, groundDection))
            temp.y = jumpHeight;

        myRB.velocity = (transform.forward * temp.z) + (transform.right * temp.x) + (transform.up * temp.y);
    }
    private void OnCollisionEnter(Collision collision)

    {
        if(((collision).gameObject.tag == "health pick up tag") && health < maxHealth)
                {
            if (health + healtPickupAmt > maxHealth)
            health = maxHealth;

            else
                health += healtPickupAmt;
            Destroy(collision.gameObject);
        }

    }
    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.tag == "weapon")
        {
            other.transform.position = WeaponSlot.position;
            other.transform.rotation = WeaponSlot.rotation;
            other.transform.SetParent(WeaponSlot);
        }

    }

}

