using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{ 
    public GameObject player;
    public float startZDist;
    public float downSpeed;

    LevelGenerator levelGenerator;
    Camera cam;
    GameObject deathBlock;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();

        levelGenerator = FindObjectOfType<LevelGenerator>();
        ResetPosition();

        deathBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
        deathBlock.transform.localScale = new Vector3(cam.orthographicSize * 2 * cam.aspect, 0.1f, 1.0f);
        deathBlock.transform.position = player.transform.position + Vector3.up * cam.orthographicSize;
        deathBlock.GetComponent<MeshRenderer>().enabled = false;
        deathBlock.GetComponent<BoxCollider>().isTrigger = true;
        deathBlock.tag = "Murderer";
        deathBlock.transform.SetParent(transform);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newPosition = transform.position + Vector3.down * downSpeed * Time.fixedDeltaTime;
        transform.position = newPosition;
    }

    public void ResetPosition()
    {
        transform.position = player.transform.position + Vector3.back * startZDist;
    }
}
