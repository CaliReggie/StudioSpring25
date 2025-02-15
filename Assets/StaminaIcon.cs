using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaIcon : MonoBehaviour
{
    [SerializeField]
    private Image staminaImage;

    private void Awake()
    {
        if (staminaImage == null || staminaImage.type != Image.Type.Filled)
        {
            Debug.LogError("StaminaIcon: StaminaImage is not set, or not set correctly in the inspector");

            Destroy(gameObject);
        }
    }
    
    public void SetFillAmount(float fillAmount)
    {
        staminaImage.fillAmount = fillAmount;
    }
}
