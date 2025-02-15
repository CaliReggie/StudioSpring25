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
    
    /// <summary>
    /// Changes the color of the player icon
    /// </summary>
    /// <param name="color"></param>
    public void SetPlayerColor(Color color)
    {
        playerIcon.PlayerColor = color;
    }
    
    /// <summary>
    /// Updates Availability of the attack icon. If fill amount is included, it will also update the fill
    /// amount of the level up icon
    /// </summary>
    /// <param name="isAvailable"></param>
    /// <param name="fillAmount"></param>
    public void UpdateAttackIcon(bool isAvailable, float fillAmount = -1)
    {
        attackIcon.IsAvailable = isAvailable;
        
        if (fillAmount <= -1) return;
        
        attackLevelUpIcon.SetFillAmount(fillAmount);
    }
    
    /// <summary>
    /// Updates Availability of the fly icon. If fill amount is included, it will also update the fill
    /// amount of the level up icon
    /// </summary>
    /// <param name="isAvailable"></param>
    /// <param name="fillAmount"></param>
    public void UpdateFlyIcon(bool isAvailable, float fillAmount = -1)
    {
        flyIcon.IsAvailable = isAvailable;
        
        if (fillAmount <= -1) return;
        
        flyLevelUpIcon.SetFillAmount(fillAmount);
    }
    
    /// <summary>
    /// Updates Availability of the run icon. If fill amount is included, it will also update the fill
    /// amount of the level up icon
    /// </summary>
    /// <param name="isAvailable"></param>
    /// <param name="fillAmount"></param>
    public void UpdateRunIcon(bool isAvailable, float fillAmount = -1)
    {
        runIcon.IsAvailable = isAvailable;
        
        if (fillAmount <= -1) return;
        
        runLevelUpIcon.SetFillAmount(fillAmount);
    }
    
    /// <summary>
    /// Updates the fill amount of the stamina icon
    /// </summary>
    /// <param name="fillAmount"></param>
    public void UpdateStaminaIcon(float fillAmount)
    {
        staminaIcon.SetFillAmount(fillAmount);
    }
}
