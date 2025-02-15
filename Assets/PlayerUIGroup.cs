using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIGroup : MonoBehaviour
{
    [Header("References")]
    
    [SerializeField]
    private PlayerIcon playerIcon;
    
    [SerializeField]
    private AbilityIcon attackIcon;
    
    [SerializeField]
    private LevelUpIcon attackLevelUpIcon;
    
    [SerializeField]
    private AbilityIcon flyIcon;
    
    [SerializeField]
    private LevelUpIcon flyLevelUpIcon;
    
    [SerializeField]
    private AbilityIcon runIcon;
    
    [SerializeField]
    private LevelUpIcon runLevelUpIcon;
    
    [SerializeField]
    private StaminaIcon staminaIcon;

    private void Awake()
    {
        if (playerIcon == null)
        {
            Debug.LogError("PlayerUIGroup: playerIcon is not set, or not set correctly in the inspector");

            Destroy(gameObject);
        }
        
        if (attackIcon == null)
        {
            Debug.LogError("PlayerUIGroup: attackIcon is not set, or not set correctly in the inspector");

            Destroy(gameObject);
        }
        
        if (attackLevelUpIcon == null)
        {
            Debug.LogError("PlayerUIGroup: attackLevelUpIcon is not set, or not set correctly in the inspector");

            Destroy(gameObject);
        }
        
        if (flyIcon == null)
        {
            Debug.LogError("PlayerUIGroup: flyIcon is not set, or not set correctly in the inspector");

            Destroy(gameObject);
        }
        
        if (flyLevelUpIcon == null)
        {
            Debug.LogError("PlayerUIGroup: flyLevelUpIcon is not set, or not set correctly in the inspector");

            Destroy(gameObject);
        }
        
        if (runIcon == null)
        {
            Debug.LogError("PlayerUIGroup: runIcon is not set, or not set correctly in the inspector");

            Destroy(gameObject);
        }
        
        if (runLevelUpIcon == null)
        {
            Debug.LogError("PlayerUIGroup: runLevelUpIcon is not set, or not set correctly in the inspector");

            Destroy(gameObject);
        }
        
        if (staminaIcon == null)
        {
            Debug.LogError("PlayerUIGroup: staminaIcon is not set, or not set correctly in the inspector");

            Destroy(gameObject);
        }
    }
    
    public void SetPlayerColor(Color color)
    {
        playerIcon.PlayerColor = color;
    }
    
    public void UpdateAttackIcon(bool isAvailable, float fillAmount)
    {
        attackIcon.IsAvailable = isAvailable;
        attackLevelUpIcon.SetFillAmount(fillAmount);
    }
    
    public void UpdateFlyIcon(bool isAvailable, float fillAmount)
    {
        flyIcon.IsAvailable = isAvailable;
        flyLevelUpIcon.SetFillAmount(fillAmount);
    }
    
    public void UpdateRunIcon(bool isAvailable, float fillAmount)
    {
        runIcon.IsAvailable = isAvailable;
        runLevelUpIcon.SetFillAmount(fillAmount);
    }
    
    public void UpdateStaminaIcon(float fillAmount)
    {
        staminaIcon.SetFillAmount(fillAmount);
    }
}
