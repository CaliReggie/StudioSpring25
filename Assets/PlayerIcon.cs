using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour
{
    [SerializeField]
    private Image playerImage;

    private void Awake()
    {
        if (playerImage == null)
        {
            Debug.LogError("PlayerIcon: playerImage is not set, or not set correctly in the inspector");

            Destroy(gameObject);
        }
    }
    
    public Color PlayerColor
    {
        set
        {
            if (playerImage == null) return;
            
            playerImage.color = value;
        }
    }
}
