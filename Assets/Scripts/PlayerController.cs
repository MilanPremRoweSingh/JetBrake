﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AnimationCurve animationCurve;

    public CameraController cameraController;
    public LevelGenerator levelGenerator;

    private Rigidbody rigidBody;
    public float terminalVelocity;
    private int jumpsUsed = 0;
    public int maxDashCount = 2;
    private int currDashCount = 0;
    private Vector3 startPos;
    public Material baseJump;
    public Material secondJump;
    public Material thirdJump;
    public Material postFourthJump;
    public float maxDashTime = 15.0f;
    public float dashStoppingSpeed = 0.1f;
    Vector3 direction = Vector3.zero;

    private float currDashTime;

    // Start is called before the first frame update
    void Start()
    {
        UpdateMaterial();
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.drag = 0f;
        startPos = transform.position;
        currDashTime = maxDashTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (rigidBody.velocity.y > 0)
        {
            rigidBody.drag = 0;
        }
        Vector3 jumpVelocity = Vector3.zero;
        Vector3 dashVelocity = Vector3.zero;
        if (Input.GetButtonDown("Jump"))
        {
            if (rigidBody.velocity.y <= 0f)
            {
                jumpVelocity = Vector3.up * (1f + jumpsUsed * 0.5f) * terminalVelocity;
                jumpsUsed++;
                UpdateMaterial();
            }
        }
        float sign = Mathf.Round(Input.GetAxisRaw("Horizontal"));
        if (Input.GetButtonDown("Fire3") && currDashCount < maxDashCount)
        {
            currDashCount++;
            currDashTime = 0.0f;
            direction = new Vector3(sign, 0, 0);
        }
        if (currDashTime < maxDashTime)
        {
            dashVelocity = direction * animationCurve.Evaluate(currDashTime / maxDashTime) * 35f;
            currDashTime += dashStoppingSpeed;
        }
        Vector3 horzVelocity = Vector3.right * sign * terminalVelocity;
        rigidBody.velocity = new Vector3(Mathf.Abs(dashVelocity.x) > 0f ? dashVelocity.x : horzVelocity.x, jumpVelocity.y > 0f ? jumpVelocity.y : rigidBody.velocity.y);
    }

    void UpdateMaterial()
    {
        if (jumpsUsed == 0) GetComponent<MeshRenderer>().material = baseJump;
        else if (jumpsUsed == 1) GetComponent<MeshRenderer>().material = secondJump;
        else if (jumpsUsed == 2) GetComponent<MeshRenderer>().material = thirdJump;
        else GetComponent<MeshRenderer>().material = postFourthJump;
    }

    public void Die()
    {
        transform.position = startPos;
        rigidBody.velocity = Vector3.zero;
        jumpsUsed = 0;
        currDashCount = 0;
        UpdateMaterial();
        levelGenerator.ResetFloors();
        cameraController.ResetPosition();
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
                currDashCount = 0;
                UpdateMaterial();
                floor.hasResetPlayerJump = true;
            }
        }
    }
}
