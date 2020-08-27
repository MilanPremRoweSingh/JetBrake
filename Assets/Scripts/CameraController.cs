using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public Vector3 targetOffset;
    public float startZDist = 10;
    [Range(0,1.0f)]
    public float smoothTime;

    private float downVelocity = 0.0f;

    GameObject anchor;
    private float anchorHeight;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.transform.position + targetOffset;
        anchor = player;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newPosition = transform.position;
        anchorHeight = anchor.transform.position.y;
        newPosition.y = Mathf.SmoothDamp(transform.position.y, anchorHeight, ref downVelocity, smoothTime);
        transform.position = newPosition;
    }

    void SetAnchor(GameObject newAnchor)
    {
        anchor = newAnchor;
    }
}
