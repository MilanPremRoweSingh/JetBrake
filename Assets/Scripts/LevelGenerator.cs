using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class LevelGenerator : MonoBehaviour
{
    public Camera cam;
        
    private float floorY = 0.0f;

    LinkedList<GameObject> floorList;
    public int maxFloors;
    public float minFloorDistance;
    public float maxFloorDistance;
    public float maxHoleWidth;
    public float minHoleWidth;
    public float floorThickness;


    // Start is called before the first frame update
    void Start()
    {
        floorList = new LinkedList<GameObject>();

        Floor.cam = cam;
        Floor.levelGenerator = this;

        GenerateFloor();
    }
    
    public void GenerateFloor()
    {
        GameObject floor = new GameObject(string.Format("Floor {0}", floorList.Count + 1));
        floor.transform.position = new Vector3(transform.position.x, floorY, transform.position.z);
        floor.AddComponent<Floor>();
        floorList.AddLast(floor);
        floorY -= Random.Range(minFloorDistance, maxFloorDistance);
        floor.transform.SetParent(transform);
    }

}
