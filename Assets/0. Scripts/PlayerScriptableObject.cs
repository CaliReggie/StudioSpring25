using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

[CreateAssetMenu(fileName = "Hero", menuName = "ScriptableObjs/Hero", order = 2)]
public class PlayerScriptableObject : ScriptableObject
{
    public PlayerStats PlayerStats;
    //public SkillScript SkillScript;
}
