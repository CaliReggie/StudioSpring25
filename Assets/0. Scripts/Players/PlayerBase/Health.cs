using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// This class handles the health state of a game object.
/// 
/// Implementation Notes: 2D Rigidbodies must be set to never sleep for this to interact with trigger stay damage
/// </summary>
public class Health : MonoBehaviour
{
    [Header("Team Settings")]
    public int teamID = 0;
    [Header("Health Settings")]
    public int defaultHealth = 1;
    public int maximumHealth = 2;
    public int currentHealth = 1;
    [Header("Invincibility Settings")]
    public float invincibleTime = 3f;
    public bool isAlwaysInvincible = false;
    public float flashSpeed = 10f;
    public Color flashColor;
    [Header("Lives Settings")]
    public bool useLives = false;
    public int curLives = 2;
    public int maxLives = 2;
    public float respawnTime = 1f;

    private bool isInvincible = false;

    public GameObject unaliveEffect;
    
    public Action OnTakeDamage;
    public Action OnRespawn;
    public Action OnDespawn;

    private Material originalMat;
    private Color originalCol;

    private void Start()
    {
        teamID = GetComponent<PlayerController>().PlayerID;
    }

    #region Unity Events
    private void OnEnable()
    {
        Renderer renderer = null;
        if (gameObject.CompareTag("Player"))
        {
            // Because of the way the player is set up, the renderer is a child object
            renderer = GetComponentInChildren<Renderer>();
        }
        else
        {
            renderer = GetComponent<Renderer>();
        }

        if (!renderer)
        {
            Debug.Log("Health: No renderer found");
        }
        originalMat = renderer.material;
        originalCol = originalMat.color;
        if (isAlwaysInvincible)
        {
            isInvincible = true;
        }
    }

    void Update()
    {
        // Check if the object is invincible and apply flashing effect
        if (isInvincible)
        {
            // Apply the flashing effect by lerping between original and flash colors
            float lerp = Mathf.PingPong(Time.time * flashSpeed, 1f); // PingPong for smooth flashing
            Color targetColor = Color.Lerp(originalCol, flashColor, lerp);

            // Apply the color to the material
            originalMat.color = targetColor;
        }
        else
        {
            // If not invincible, reset the material color to original color
            originalMat.color = originalCol;
        }
    }
    #endregion


    /// <summary>
    /// Activate invincibility for *invincibleTime* seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator ActivateInvicibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }
    
    /// <summary>
    /// Check if the object is dead
    /// </summary>
    /// <returns>Whether the object is dead or nah</returns>
    private bool CheckDeath()
    {
        if (currentHealth <= 0)
        {
            if (useLives && curLives > 0)
            {
                // Respawn
                StartCoroutine(DieButRespawn());
                curLives--;
                currentHealth = maximumHealth;
                return true;
            }

            if (!useLives)
            {
                // Respawn
                StartCoroutine(DieButRespawn());
                currentHealth = maximumHealth;
                return true;
            }
            Die();
            return true;
        }
        return false;
    }

    #region Public Functions
        
    /// <summary>
    /// Take damage for the object
    /// </summary>
    /// <param name="amount">Amount of damage to take</param>
    public void TakeDamage(int amount)
    {
        if (isInvincible) return;
        // if not invincible right now
        Debug.Log("Taking damage!");
        currentHealth -= amount;
        if (CheckDeath() == false) // if not despawning
        {
            StartCoroutine(ActivateInvicibility());
            OnTakeDamage?.Invoke();
        }
    }
    /// <summary>
    /// Add health to the object
    /// </summary>
    /// <param name="amount"></param>
    public void AddHealth(int amount = 1)
    {
        currentHealth += amount;
        if (currentHealth > maximumHealth) currentHealth = maximumHealth;
    }

    /// <summary>
    /// Add life to the object
    /// </summary>
    /// <param name="amount"></param>
    public void AddLife(int amount = 1)
    {
        curLives += amount;
        if (curLives > maxLives) curLives = maxLives;
    }

    /// <summary>
    /// Toggle invincibility
    /// </summary>
    /// <param name="invincibility"></param>
    public void ToggleInvicibility(bool invincibility)
    {
        isInvincible = invincibility;
    }

    #endregion

    /// <summary>
    /// Event for when the object dies
    /// When the player is dead it will trigger the game over event.
    /// </summary>
    private void Die()
    {
        if (unaliveEffect)
        {
            Instantiate(unaliveEffect, transform.position, Quaternion.identity);
        }
        OnDespawn?.Invoke();
        // TODO: Implement GameManager
        /*if (gameObject.CompareTag("Player"))
        {
            GameManager.Instance.GameOver();
        }*/
            Destroy(gameObject);
    }

    /// <summary>
    /// Event for when the object dies but respawns
    /// </summary>
    /// <returns></returns>
    private IEnumerator DieButRespawn()
    {
        if (unaliveEffect)
        {
            Instantiate(unaliveEffect, transform.position, Quaternion.identity);
        }
        OnDespawn?.Invoke();
        yield return new WaitForSeconds(respawnTime);
        OnRespawn?.Invoke();
    }
}