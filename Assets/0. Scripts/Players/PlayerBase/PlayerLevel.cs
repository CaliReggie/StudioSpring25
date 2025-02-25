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
    public void AddAtkXP(int addAmount)
    {
        AddCriteriaXP(addAmount, ref atkLevel, ref atkXP, atkLevelThreshold,
            () => _pc.AddAttack(1),  // TODO: UPDATE ME!
            amount => UIManager.Instance.UpdateAttackIcon(_pc.PlayerID, true, amount));
    }
    
    public void AddRunXP(int addAmount )
    {
        AddCriteriaXP(addAmount, ref runLevel, ref runXP, runLevelThreshold,
            () => _pc.AddSpeed(1), 
            amount => UIManager.Instance.UpdateRunIcon(_pc.PlayerID,true, amount));
    }

    public void AddFlyXP(int addAmount)
    {
        AddCriteriaXP(addAmount, ref flyLevel, ref flyXP, flyLevelThreshold,
            () => _pc.AddJumpHeight(1), // TODO: UPDATE ME!
            amount => UIManager.Instance.UpdateFlyIcon(_pc.PlayerID, true, amount));
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
