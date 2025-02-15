using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelUpIcon : MonoBehaviour
{
    [SerializeField]
    private Image fillImage;

    private void Awake()
    {
        if (fillImage == null || fillImage.type != Image.Type.Filled)
        {
            Debug.LogError("LevelUpIcon: levelUpImage is not set, or not set correctly in the inspector");

            Destroy(gameObject);
        }
    }
    
    public void SetFillAmount(float fillAmount)
    {
        fillImage.fillAmount = fillAmount;
    }
}
