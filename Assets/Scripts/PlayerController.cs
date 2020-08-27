using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rigidBody;
    public float terminalVelocity;
    private int jumpsUsed = 0;
    private Vector3 startPos;
    public Material baseJump;
    public Material secondJump;
    public Material thirdJump;
    public Material postFourthJump;

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
        if(rigidBody.velocity.y > 0)
        {
            rigidBody.drag = 0;
        }
        Vector3 jumpVelocity = Vector3.zero;
        if (Input.GetKeyDown("space"))
        {
            if (rigidBody.velocity.y <= 0f)
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
        if(jumpsUsed == 0) GetComponent<MeshRenderer>().material = baseJump;
        else if (jumpsUsed == 1) GetComponent<MeshRenderer>().material = secondJump;
        else if (jumpsUsed == 2) GetComponent<MeshRenderer>().material = thirdJump;
        else GetComponent<MeshRenderer>().material = postFourthJump;
    }

    public void Die()
    {
        transform.position = startPos;
        rigidBody.velocity = Vector3.zero;
        jumpsUsed = 0;
        UpdateMaterial();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Murderer")
        {
            Die();
        }
        else if (other.gameObject.tag == "Hole")
        {
            if (other.transform.gameObject.transform.position.y < rigidBody.position.y)
            {
                jumpsUsed = 0;
                UpdateMaterial();
            }
        }
    }
}
