using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [Header("TargetPositions")]
    public Transform pos1;
    public Transform pos2;
    Vector3 targetPosition;
    public float moveSpeed = 10f;
    public Collider2D colliderTop;
    public Collider2D colliderLeft;
    public Collider2D colliderRight;
    public Collider2D colliderDown;

    public enum eCamState
    {
        Static,
        MoveToPos1,
        MoveToPos2,
    }
    public eCamState camState;
    void Start()
    {
        camState = eCamState.Static;
    }

    void Update()
    {
       CamBehaviour(); 
    }
    void CamBehaviour()
    {
        switch(camState)
        {
            case eCamState.Static:
            colliderTop.isTrigger = false;
            colliderLeft.isTrigger = false;
            colliderRight.isTrigger = false;
            colliderDown.isTrigger = true;
            break;

            case eCamState.MoveToPos1:
            colliderDown.isTrigger = true;
            targetPosition = pos1.position;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x,targetPosition.y,-10), moveSpeed*Time.deltaTime);
            
            if (transform.position == targetPosition)
            camState = eCamState.Static;
            break;

            case eCamState.MoveToPos2:
            colliderLeft.isTrigger = true;
            colliderDown.isTrigger = true;
            targetPosition = pos2.position;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x,targetPosition.y,-10), moveSpeed*Time.deltaTime);
            if (transform.position == targetPosition)
            camState = eCamState.Static;
            break;


        }
    }
}
