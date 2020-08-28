using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float startZDist;
    [Range(0,10.0f)]
    public float smoothTime;
    public float snapThreshold;

    LevelGenerator levelGenerator;
    GameObject anchor;
    private float downVelocity = 0.0f;
    float anchorOffset;

    // Start is called before the first frame update
    void Start()
    {
        levelGenerator = FindObjectOfType<LevelGenerator>();
        transform.position = player.transform.position + Vector3.back * startZDist;
        anchor = player;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newPosition = transform.position;
        float anchorHeight = anchor.transform.position.y;

        if (Mathf.Abs(anchorHeight - transform.position.y) < levelGenerator.maxFloorDistance + Mathf.Epsilon) 
        {
            newPosition.y = Mathf.SmoothDamp(transform.position.y, anchorHeight + anchorOffset, ref downVelocity, smoothTime);
        }
        else
        {
            newPosition.y = anchorHeight;
        }
        transform.position = newPosition;
    }

    public void SetAnchor(GameObject newAnchor)
    {
        Debug.Log("Anchor Changed to " + newAnchor.name);
        anchorOffset = 0.0f;
        anchor = newAnchor;
    }


    public void SetAnchor(GameObject newAnchor, float newAnchorOffset)
    {
        Debug.Log("Anchor Changed to " + newAnchor.name);
        anchorOffset = newAnchorOffset;
        anchor = newAnchor;
    }
}
