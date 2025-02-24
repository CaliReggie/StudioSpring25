using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer.Internal.Converters;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    public int atkLevel, runLevel, flyLevel;
    public int atkXP, runXP, flyXP;

    [Header("Level Thresholds")] 
    [SerializeField] private int atkLevelThreshold;
    [SerializeField] private int runLevelThreshold;
    [SerializeField] private int flyLevelThreshold;


    [Header("Add amounts")] 
    [SerializeField] private int atkAddAmount;
    [SerializeField] private int runAddAmount;
    [SerializeField] private int flyAddAmount;

    private PlayerController _pc;
    void Start()
    {
        _pc = GetComponent<PlayerController>();
    }
    
    #region Public Methods
    public void AddAtkXP(int amount)
    {
        /*atkXP+=amount;
        if (atkXP >= atkLevelThreshold)
        {
            // Increase level and reset atk XP
            atkXP -= atkLevelThreshold;
            atkLevel++;
            // Update UI
            
            // Increase stamina
            
        }
        UIManager.Instance.UpdateStaminaIcon(_pc.PlayerID, (float)atkXP / atkLevelThreshold);*/
        AddCriteriaXP(amount, ref atkLevel, ref atkXP, atkLevelThreshold,
            () => _pc.AddSpeed(runAddAmount),  // TODO: UPDATE ME!
            amount => UIManager.Instance.UpdateStaminaIcon(playerNum:_pc.PlayerID, fillAmount: amount));
    }
    
    public void AddRunXP(int amount )
    {
        /*speedXP += amount;
        if (speedXP >= speedLevelThreshold)
        {
            // Increase level and reset speed XP
            speedXP -= speedLevelThreshold;
            speedLevel++;
            
            // Increase speed
            _pc.AddSpeed(speedAddAmount);
        }
        UIManager.Instance.UpdateStaminaIcon(_pc.PlayerID, (float)speedXP / speedLevelThreshold);*/
        AddCriteriaXP(amount, ref runLevel, ref runXP, runLevelThreshold,
            () => _pc.AddSpeed(runAddAmount), 
            amount => UIManager.Instance.UpdateRunIcon(playerNum:_pc.PlayerID,true, fillAmount: amount));
    }

    public void AddFlyXP(int amount)
    {
        AddCriteriaXP(amount, ref flyLevel, ref flyXP, flyLevelThreshold,
            () => _pc.AddSpeed(runAddAmount), // TODO: UPDATE ME!
            amount => UIManager.Instance.UpdateFlyIcon(playerNum:_pc.PlayerID, true, fillAmount: amount));
    }
    #endregion
    
    private void AddCriteriaXP(int amount, ref int criteriaLevel, ref int criteriaXP, int criteriaThreshold,
        Action addToPlayerController, Action<float> updateUI) {
        criteriaXP += amount;
        if (criteriaXP >= criteriaThreshold)
        {
            // Increase level and reset speed XP
            criteriaXP -= criteriaThreshold;
            criteriaLevel++;
            
            // Increase speed
            addToPlayerController();
        }
        updateUI((float)criteriaXP / criteriaThreshold);
    }
}
