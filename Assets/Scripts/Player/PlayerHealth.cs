using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image healthDisplay;
    public Image manaDisplay;
    public float mana = 100f;
    public float health = 100f;
    public float timer = 0f;
    public float manaCD = 2f;
    public float manaRegen = 5f;

    public Damageable playerHP;
    public PlayerMana playerMana;

    private void Awake()
    {
        playerHP = gameObject.GetComponent<Damageable>();
    }

    private void Start()
    {
        UpdateHealthDisplay();
    }

    private void Update()
    {
        mana = playerMana.Mana;
        health = playerHP.Health;

        UpdateHealthDisplay();
        
        timer += Time.deltaTime;
        if(timer >= manaCD)
        {
            ManaRestore(manaRegen);
            timer = 0f;
        }

        if(mana > 0)
        {
            playerHP.enabled = false;
        }
        else
        {
            playerHP.enabled = true;
        }
    }

    private void UpdateHealthDisplay()
    {
        manaDisplay.fillAmount = mana / 100;
        healthDisplay.fillAmount = health / 100;
    }

    private void ManaRestore(float amount)
    {
        playerMana.Mana += amount;
    }
}
