using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

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
    
    [Header("Player Management")]
    
    public List<GameObject> playersHit = new List<GameObject>();
    
    public Transform pos1Respawn;
    
    public Transform pos2Respawn;
    
    
    [Header("TargetPositions")]
    public Transform pos1;
    public Transform pos2;
    Vector3 targetPosition;
    public float firstMoveSpeed = 10f;

    public float secondMoveSpeed = 20f;
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
        
        //wait until static state
        yield return new WaitUntil(() => camState == eCamState.StaticSafe);
        
        //reset the players
        Pos1Respawn();
        
        yield return new WaitForSeconds(timeWaitBeforeMoveFromStatic);
        
        camState = eCamState.MoveToPos2;
        
        //wait until trigger state
        yield return new WaitUntil(() => camState == eCamState.StaticUnsafe);
        
        //reset the players
        Pos2Respawn();
        
        //done!
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit");
            
            if (!playersHit.Contains(other.gameObject))
            {
                playersHit.Add(other.gameObject);
                
                //turn off player
                other.gameObject.SetActive(false);
            }
        }
    }
    
    private void Pos1Respawn()
    {
        foreach (var player in playersHit)
        {
            player.transform.position = pos1Respawn.position;
            player.SetActive(true);
        }
        
        playersHit.Clear();
    }
    
    private void Pos2Respawn()
    {
        foreach (var player in playersHit)
        {
            player.transform.position = pos2Respawn.position;
            player.SetActive(true);
        }
        
        playersHit.Clear();
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
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x,targetPosition.y,-10), firstMoveSpeed*Time.deltaTime);

            if (transform.position == targetPosition)
                camState = eCamState.StaticSafe;
            
            break;

            case eCamState.MoveToPos2:
            colliderLeft.isTrigger = true;
            colliderDown.isTrigger = true;
            targetPosition = pos2.position;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x,targetPosition.y,-10), secondMoveSpeed*Time.deltaTime);
            if (transform.position == targetPosition)
            camState = eCamState.StaticUnsafe;
            break;


        }
    }
}
