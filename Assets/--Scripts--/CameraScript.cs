using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [Header("TargetPositions")]
    public Transform pos1;
    public Transform pos2;
    Vector2 currentPosition;
    Vector2 targetPosition;
    public float moveSpeed = 10f;

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
            break;

            case eCamState.MoveToPos1:
            targetPosition = pos1.position;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x,targetPosition.y,-10), moveSpeed*Time.deltaTime);
            break;

            case eCamState.MoveToPos2:
            targetPosition = pos2.position;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x,targetPosition.y,-10), moveSpeed*Time.deltaTime);
            break;


        }
    }
}
