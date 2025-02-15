using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AbilityIcon : MonoBehaviour
{
    [SerializeField]
    private Image abilityImage;
    
    [SerializeField]
    private Color  availableColor = Color.white;
    
    [SerializeField]
    private Color unavailableColor = Color.gray;
    
    private void Awake()
    {
        if (abilityImage == null)
        {
            Debug.LogError("AbilityIcon: abilityImage is not set, or not set correctly in the inspector");

            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// Set to true to make icon turn available color, false to make it turn unavailable color
    /// </summary>
    public bool IsAvailable
    {
        set
        {
            if (abilityImage == null) return;
            
            abilityImage.color = value ? availableColor : unavailableColor;
        }
    }
}
