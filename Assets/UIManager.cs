using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    [Header("Debug")]
    
    [SerializeField]
    private bool addPlayerUIGroups = false;
    
    [Range(1, 4)] 
    [SerializeField] 
    private int playerCount = 1;
    
    [SerializeField]
    private bool removePlayerUIGroups = false;
    
    [Space]
    
    [Range(1, 4)]
    [SerializeField] 
    private int targetUpdateGroup = 1;
    
    [SerializeField]
    private Color targetPlayerIconColor = Color.white;
    
    [SerializeField]
    private bool attackIconAvailable = true;
    
    [Range(0,1)]
    [SerializeField]
    private float attackLevelUpFillAmount = 0.5f;
    
    [SerializeField]
    private bool flyIconAvailable = true;
    
    [Range(0,1)]
    [SerializeField]
    private float flyLevelUpFillAmount = 0.5f;
    
    [SerializeField]
    private bool runIconAvailable = true;
    
    [Range(0,1)]
    [SerializeField]
    private float runLevelUpFillAmount = 0.5f;
    
    [Range(0,1)]
    [SerializeField]
    private float staminaFillAmount = 0.5f;
    
    [Space]
    
    [SerializeField]
    private bool updateTargetGroup = false;
    
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
    
    private void Update()
    {
        //all of this is debug, can be removed once the game is working, simply demonstrating how to use the UIManager
        if (addPlayerUIGroups)
        {
            addPlayerUIGroups = false;
            
            if (PlayerUIGroups != null) return;
            
            CreatePlayerUIGroups(playerCount);
        }
        
        if (removePlayerUIGroups)
        {
            removePlayerUIGroups = false;
            
            if (PlayerUIGroups == null) return;
            
            for (int i = 0; i < PlayerUIGroups.Length; i++)
            {
                Destroy(PlayerUIGroups[i].gameObject);
            }
            
            PlayerUIGroups = null;
        }
        
        if (updateTargetGroup && PlayerUIGroups != null && targetUpdateGroup <= PlayerUIGroups.Length)
        {
            updateTargetGroup = false;
            
            PlayerUIGroup targetGroup = PlayerUIGroups[targetUpdateGroup - 1];
            
            targetGroup.SetPlayerColor( targetPlayerIconColor );
            
            targetGroup.UpdateAttackIcon( attackIconAvailable, attackLevelUpFillAmount);
            
            targetGroup.UpdateFlyIcon( flyIconAvailable, flyLevelUpFillAmount);
            
            targetGroup.UpdateRunIcon( runIconAvailable, runLevelUpFillAmount);
            
            targetGroup.UpdateStaminaIcon( staminaFillAmount );
        }
    }
    
    
    public void CreatePlayerUIGroups(int playerCount = 1)
    {
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
