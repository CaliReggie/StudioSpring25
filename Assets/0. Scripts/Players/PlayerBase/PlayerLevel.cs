using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer.Internal.Converters;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    public int staminaLevel, speedLevel;
    public int staminaXP, speedXP;

    [Header("Level Thresholds")] 
    [SerializeField] private int staminaLevelThreshold;
    [SerializeField] private int speedLevelThreshold;

    [Header("Add amounts")] 
    [SerializeField] private int staminaAddAmount;
    [SerializeField] private int speedAddAmount;
    private PlayerController _pc;
    void Start()
    {
        _pc = GetComponent<PlayerController>();
    }
    
    #region Public Methods
    public void AddStaminaLevel(int amount)
    {
        staminaXP+=amount;
        if (staminaXP >= staminaLevelThreshold)
        {
            // Increase level and reset stamina XP
            staminaXP -= staminaLevelThreshold;
            staminaLevel++;
            // Update UI
            
            // Increase stamina
            
        }
    }
    
    public void AddSpeedLevel(int amount )
    {
        speedXP += amount;
        if (speedXP >= speedLevelThreshold)
        {
            // Increase level and reset speed XP
            speedXP -= speedLevelThreshold;
            speedLevel++;
            
            // Increase speed
            _pc.AddSpeed(speedAddAmount);
        }
        UIManager.Instance.UpdateStaminaIcon(_pc.PlayerID, (float)speedXP / speedLevelThreshold);
    }
    #endregion
}
