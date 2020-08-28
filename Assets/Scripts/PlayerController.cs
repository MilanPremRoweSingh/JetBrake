using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CameraController cameraController;
    public LevelGenerator levelGenerator;

    private Rigidbody rigidBody;
    public float terminalVelocity;
    private int jumpsUsed = 0;
    private Vector3 startPos;
    public Material noJump;
    public Material baseJump;
    public Material secondJump;
    public Material thirdJump;
    public Material fourthJump;

    // Start is called before the first frame update
    void Start()
    {
        UpdateMaterial();
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.drag = 0f;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (rigidBody.velocity.y > 0)
        {
            rigidBody.drag = 0;
        }
        Vector3 jumpVelocity = Vector3.zero;
        if (Input.GetKeyDown("space"))
        {
            if (rigidBody.velocity.y <= 0f && jumpsUsed < 4)
            {
                jumpVelocity = Vector3.up * (1f + jumpsUsed * 0.5f) * terminalVelocity;
                jumpsUsed++;
                UpdateMaterial();
            }
        }
        float sign = Mathf.Round(Input.GetAxisRaw("Horizontal"));
        Vector3 horzVelocity = Vector3.right * sign * terminalVelocity;
        rigidBody.velocity = new Vector3(horzVelocity.x, jumpVelocity.y > 0f ? jumpVelocity.y : rigidBody.velocity.y);
    }

    void UpdateMaterial()
    {
        if (jumpsUsed == 0) GetComponent<MeshRenderer>().material = baseJump;
        else if (jumpsUsed == 1) GetComponent<MeshRenderer>().material = secondJump;
        else if (jumpsUsed == 2) GetComponent<MeshRenderer>().material = thirdJump;
        else if (jumpsUsed == 3) GetComponent<MeshRenderer>().material = fourthJump;
        else GetComponent<MeshRenderer>().material = noJump;
    }

    public void Die()
    {
        transform.position = startPos;
        rigidBody.velocity = Vector3.zero;
        jumpsUsed = 0;
        UpdateMaterial();
        levelGenerator.ResetFloors();
        cameraController.SetAnchor(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Murderer")
        {
            Die();
        }
        else if (other.gameObject.tag == "Hole")
        {
            Floor floor = other.gameObject.GetComponent<Floor>();
            if (!floor.hasResetPlayerJump)
            {
                jumpsUsed = 0;
                UpdateMaterial();
                floor.hasResetPlayerJump = true;
            }
        }
    }
}
