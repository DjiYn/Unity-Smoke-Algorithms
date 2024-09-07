using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    
    private float health;

    [Header("Health Bar")]
    [SerializeField]
    private Image frontHealthBar;
    [SerializeField]
    private Image backHealthBar;
    [SerializeField]
    private TextMeshProUGUI promptText;
    [SerializeField]
    private float maxHealth = 100;
    [SerializeField]
    private float chipSpeed = 2f;

    private float lerpTimer;

    [Header("Damage Overlay")]
    [SerializeField]
    private Image damageOverlay;
    [SerializeField]
    private float duration = 0.5f;
    [SerializeField]
    private float blinkDuration = 0.25f;
    [SerializeField]
    private float fadeSpeed = 1.5f;
    [SerializeField]
    private float maxDamageOverlayAlpha = 0.8f;
    [SerializeField]
    private float lowHealthMark = 30f;

    private float durationTimer;

    private void Start()
    {
        health = maxHealth;
        damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, 0);
    }

    private void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);

        UpdateHealthUI();
        UpdateDamageOverlayAlpha();
        updateGameOver();
    }

    public void UpdateHealthUI()
    {
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        float healthFraction = health / maxHealth;

        if (fillBack > healthFraction)
        {
            frontHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
        }

        if (fillFront < healthFraction)
        {
            backHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillFront, healthFraction, percentComplete);
        }

        promptText.text = health.ToString() + " / " + maxHealth.ToString();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
        durationTimer = 0f;
        damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, maxDamageOverlayAlpha);
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }

    private void UpdateDamageOverlayAlpha()
    {
        if (damageOverlay.color.a > 0)
        {
            // Control low health blinking
            if (health < lowHealthMark)
            {
                durationTimer += Time.deltaTime;

                if (durationTimer > blinkDuration)
                {
                    float tempAlpha = damageOverlay.color.a;
                    tempAlpha -= Time.deltaTime * fadeSpeed;
                    damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, tempAlpha);
                }
                
                if (durationTimer > blinkDuration && damageOverlay.color.a < 0.1f)
                {
                    durationTimer = 0;
                    damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, maxDamageOverlayAlpha);
                }

                return;
            }

            // Control damage health
            durationTimer += Time.deltaTime;

            if (durationTimer > duration)
            {
                float tempAlpha = damageOverlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, tempAlpha);
            }
        }
    }

    private void updateGameOver()
    {
        if (health <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
