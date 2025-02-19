using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager S;

    [Header("Inscribed")]
    public GameObject playerPrefab;
    
    //public on player joined event for things to sub to
    public event Action<int> PlayerJoined;
    
    private List<PlayerInput> players = new List<PlayerInput>();

    private PlayerInputManager playerInputManager;

    static public float DIFFSPEED = 1f;
    private float difficultyMax;
    static public Rect CAMERA_BOUNDS;

    [SerializeField] private bool broadcastPlayerJoined = false;
    
    [Range(1,4)] [SerializeField] private int broadcastPlayerNum = 1;
    
    private void OnValidate()
    {
        if (broadcastPlayerJoined)
        {
            broadcastPlayerJoined = false;
            
            BroadcastPlayerJoined(broadcastPlayerNum);
        }
    }

    private void Awake()
    {
        if (S != null)
        {
            Destroy(gameObject);
            return;
        }
        S = this;

        DontDestroyOnLoad(gameObject);
        playerInputManager = FindObjectOfType<PlayerInputManager>();
    }
    
    private void BroadcastPlayerJoined(int playerNum)
    {
        PlayerJoined?.Invoke(playerNum);
    }

    public static void LoadPlayers(List<PlayerInput> playerInputs)
    {
        S.players.Clear();
        for (int i = 0; i < playerInputs.Count; i++)
        {
            if (playerInputs[i] != null)
                S.players.Add(playerInputs[i]);
        }
        S.playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;

    }

    public static void StartGame()
    {
        S.MakePlayers();
        //S.CreateSplitScreen(S.playerInputManager.playerCount);
    }

    public void MakePlayers()
    {
        foreach (PlayerInput playerInput in players)
        {
            foreach (Transform child in playerInput.GetComponentInChildren<Transform>())
            {
                child.gameObject.SetActive(true);
                
                BroadcastPlayerJoined(playerInput.playerIndex);
            }
        }
    }

    // No Split Screen Needed

    /*public void CreateSplitScreen(int playerCount)
    {
        playerInputManager.splitScreen = true;
        List<Rect> splitScreens = new List<Rect>();
        switch (playerCount)
        {
            case 1:
                splitScreens.Add(new Rect(0, 0, 1, 1));
                break;
            case 2:
                splitScreens.Add(new Rect(0.25f, 0.5f, 0.5f, 0.5f));
                splitScreens.Add(new Rect(0.25f, 0, 0.5f, 0.5f));
                break;
            case 3:
            case 4:
                splitScreens.Add(new Rect(0, 0.5f, 0.5f, 0.5f));
                splitScreens.Add(new Rect(0.5f, 0.5f, 0.5f, 0.5f));
                splitScreens.Add(new Rect(0, 0, 0.5f, 0.5f));
                splitScreens.Add(new Rect(0.5f, 0, 0.5f, 0.5f));
                break;
            default:
                Debug.Log("More cases for players needed");
                break;
        }
        for (int i = 0; i < players.Count; i++)
        {
            players[i].GetComponentInChildren<Camera>().rect = splitScreens[i];
        }

    }*/
}
