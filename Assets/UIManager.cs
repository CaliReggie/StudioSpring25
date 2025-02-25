using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    [Header("Player UI")]
    
    [SerializeField]
    private GameObject playerUIGroupHolder;
    
    [SerializeField]
    private GameObject playerUIGroupPrefab;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
            //if desired to make main through scenes
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        if (playerUIGroupHolder == null)
        {
            Debug.LogError("UIManager: playerUIGroupHolder is not set in the inspector");
            
            Destroy(gameObject);
        }
        
        if (playerUIGroupPrefab == null)
        {
            Debug.LogError("UIManager: playerUIGroupPrefab is not set in the inspector");
            
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        GameManager.S.PlayerJoined += OnPlayerJoined;
    }
    
    private void OnPlayerJoined(int playerNum)
    {
        CreatePlayerUIGroups(playerNum);
    }
    
    public void UpdateAttackIcon(int playerNum, bool available, float fillAmount = -1)
    {
        if (PlayerUIGroups == null || playerNum < 1 || playerNum > PlayerUIGroups.Length)
        {
            Debug.LogError("UIManager: Player number is out of range");
            
            return;
        }
        
        PlayerUIGroups[playerNum - 1].UpdateAttackIcon(available, fillAmount);
    }
    
    public void UpdateFlyIcon(int playerNum, bool available, float fillAmount  = -1)
    {
        if (PlayerUIGroups == null || playerNum < 1 || playerNum > PlayerUIGroups.Length)
        {
            Debug.LogError("UIManager: Player number is out of range");
            
            return;
        }
        
        PlayerUIGroups[playerNum - 1].UpdateFlyIcon(available, fillAmount);
    }
    
    public void UpdateRunIcon(int playerNum, bool available, float fillAmount = -1)
    {
        if (PlayerUIGroups == null || playerNum < 1 || playerNum > PlayerUIGroups.Length)
        {
            Debug.LogError("UIManager: Player number is out of range");
            
            return;
        }
        
        PlayerUIGroups[playerNum - 1].UpdateRunIcon(available, fillAmount);
    }
    
    public void UpdateStaminaIcon(int playerNum, float fillAmount)
    {
        if (PlayerUIGroups == null || playerNum < 1 || playerNum > PlayerUIGroups.Length)
        {
            Debug.LogError("UIManager: Player number is out of range");
            
            return;
        }
        
        PlayerUIGroups[playerNum - 1].UpdateStaminaIcon(fillAmount);
    }
    
    private void CreatePlayerUIGroups(int playerCount = 1)
    {
        if (PlayerUIGroups != null)
        {
            for (int i = 0; i < PlayerUIGroups.Length; i++)
            {
                Destroy(PlayerUIGroups[i].gameObject);
            }
            
            PlayerUIGroups = null;
        }
        
        if (playerCount < 1 || playerCount > 4)
        {
            Debug.LogError("UIManager: Player count must be between 1 and 4");
            
            return;
        }
        
        PlayerUIGroups = new PlayerUIGroup[playerCount];
        
        for (int i = 0; i < playerCount; i++)
        {
            GameObject playerUIGroup = Instantiate(playerUIGroupPrefab, playerUIGroupHolder.transform);
            
            PlayerUIGroups[i] = playerUIGroup.GetComponent<PlayerUIGroup>();
        }
    }
    
    public PlayerUIGroup[] PlayerUIGroups { get; private set; }
}
