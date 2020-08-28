using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public static Camera cam;
    public static CameraController cameraController;
    public static LevelGenerator levelGenerator;

    private Floor next = null;

    // x in [0,1];  y in world space 
    float holeWidth;
    float holeX;
    float height;

    GameObject leftSide;
    GameObject rightSide;
    BoxCollider leftTrigger;
    BoxCollider rightTrigger;
    BoxCollider trigger;

    float floorScale;
    float triggerScale;

    [HideInInspector]
    public bool hasGeneratedNewFloor = false;
    [HideInInspector]
    public bool hasResetPlayerJump = false;

    void Start()
    {
        floorScale = 2 * cam.orthographicSize * cam.aspect;

        leftSide = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leftSide.transform.SetParent(transform);
        leftTrigger = leftSide.GetComponent<BoxCollider>();
        leftTrigger.isTrigger = true;
        leftSide.tag = "Murderer";

        rightSide = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rightSide.transform.SetParent(transform);
        rightTrigger = rightSide.GetComponent<BoxCollider>();
        rightTrigger.isTrigger = true;
        rightSide.tag = "Murderer";

        // Trigger is resized and recentred based on holeWidth in Randomize()
        trigger = gameObject.AddComponent<BoxCollider>();
        trigger.isTrigger = true;
        trigger.tag = "Hole";
        Randomize();
    }

    public void Randomize()
    {
        hasGeneratedNewFloor = false;
        hasResetPlayerJump = false;
        Vector3 position = transform.position;
        holeWidth = Random.Range(levelGenerator.minHoleWidth, levelGenerator.maxHoleWidth);
        holeX = Random.Range(holeWidth / 2.0f, 1 - holeWidth / 2.0f);

        leftSide.transform.localScale = new Vector3(floorScale, levelGenerator.floorThickness, 1.0f);
        float leftX = floorScale * (-1.0f / 2 - holeWidth / 2 - (0.5f - holeX));
        leftSide.transform.position = new Vector3(position.x + leftX, position.y, position.z);

        rightSide.transform.localScale = new Vector3(floorScale, levelGenerator.floorThickness, 1.0f);
        float rightX = floorScale + leftX + floorScale * holeWidth;
        rightSide.transform.position = new Vector3(position.x + rightX, position.y, position.z);

        trigger.size = new Vector3(holeWidth * floorScale, levelGenerator.floorThickness, 1.0f);
        trigger.center = new Vector3(leftX + 0.5f * floorScale * (1 + holeWidth), 0.0f, 0.0f);
    }

    public void Reset()
    {
        hasResetPlayerJump = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!hasGeneratedNewFloor)
        {
            next = levelGenerator.GenerateFloor();
            hasGeneratedNewFloor = true;
        }
        //cameraController.SetAnchor(gameObject, -levelGenerator.maxFloorDistance / 2);
    }
}
