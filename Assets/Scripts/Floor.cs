﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public static Camera cam;
    public static LevelGenerator levelGenerator;

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
    public bool floorCompleted = false;

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
        trigger = gameObject.AddComponent<BoxCollider>();
        trigger.isTrigger = true;
        trigger.tag = "Hole";
        trigger.size = new Vector3(floorScale, levelGenerator.floorThickness, 1.0f);
        Randomize();
    }

    public void Randomize()
    {
        floorCompleted = false;
        Vector3 position = transform.position;
        holeWidth = Random.Range(levelGenerator.minHoleWidth, levelGenerator.maxHoleWidth);
        holeX = Random.Range(holeWidth / 2.0f, 1 - holeWidth / 2.0f);

        leftSide.transform.localScale = new Vector3(floorScale, levelGenerator.floorThickness, 1.0f);
        float leftX = floorScale * (-1.0f / 2 - holeWidth / 2 - (0.5f - holeX));
        leftSide.transform.position = new Vector3(position.x + leftX, position.y, position.z);

        rightSide.transform.localScale = new Vector3(floorScale, levelGenerator.floorThickness, 1.0f);
        float rightX = floorScale + leftX + floorScale * holeWidth;
        rightSide.transform.position = new Vector3(position.x + rightX, position.y, position.z);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!floorCompleted)
        {
            levelGenerator.GenerateFloor();
            floorCompleted = true;
        }
    }
}
