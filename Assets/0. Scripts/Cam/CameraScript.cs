using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class CameraScript : MonoBehaviour
{
    [Header("Movement Criteria")]
    
    [SerializeField]
    private int playersToStartMoving = 2;
    
    [SerializeField]
    private float timeWaitBeforeMoveFromStatic = 5f;
    
    [Header("Display State Text")]
    
    [SerializeField]
    private TextMeshProUGUI stateText;
    
    
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
        StaticUnsafe,
        StaticSafe,
        MoveToPos1,
        MoveToPos2,
    }
    public eCamState camState;
    void Start()
    {
        camState = eCamState.StaticSafe;
        
        GameManager.S.PlayerJoined += PlayerJoined;
    }
    
    private void PlayerJoined(int playerNum)
    {
        if (playerNum >= playersToStartMoving)
        {
            StartCoroutine(StartCamMovement());
        }
    }
    
    //coroutine that acts independantly once called handling cam movement the whole level. 
    //when called, starts cycle of waiting the wait time, moving to pos1, waiting time, moving to 2, and that's it
    //on start, not wait time yet, but static safe
    //on wait before move to pos1, static safe
    //on wait to move to pos2, static safe
    //once at pos2, static trigger
    
    private IEnumerator StartCamMovement()
    {
        
        yield return new WaitForSeconds(timeWaitBeforeMoveFromStatic);
        
        camState = eCamState.MoveToPos1;
        
        //wait until trigger state
        yield return new WaitUntil(() => camState == eCamState.StaticUnsafe);
        
        yield return new WaitForSeconds(timeWaitBeforeMoveFromStatic);
        
        camState = eCamState.MoveToPos2;
    }
    
    
    void Update()
    {
       CamBehaviour(); 
       
       if (stateText != null)
       {
           stateText.text = ( "State: " + camState);
       }
    }
    void CamBehaviour()
    {
        switch(camState)
        {
            case eCamState.StaticSafe:
            colliderTop.isTrigger = false;
            colliderLeft.isTrigger = false;
            colliderRight.isTrigger = false;
            colliderDown.isTrigger = false;
            break;
            
            case eCamState.StaticUnsafe:
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
                camState = eCamState.StaticSafe;
            
            break;

            case eCamState.MoveToPos2:
            colliderLeft.isTrigger = true;
            colliderDown.isTrigger = true;
            targetPosition = pos2.position;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x,targetPosition.y,-10), moveSpeed*Time.deltaTime);
            if (transform.position == targetPosition)
            camState = eCamState.StaticUnsafe;
            break;


        }
    }
}
