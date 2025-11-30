using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private AimBehaviourBasic aimBehaviour;

    [SerializeField]
    private MoveBehaviour playerMoveBehaviour;

    [Header("Health Settings")]
    [SerializeField]
    private float maxHealth = 100f;

    public float currentHealth;

    [SerializeField]
    private Image healthBarFill;

    [Header("Hunger Settings")]
    [SerializeField]
    private float maxHunger = 100f;

    public float currentHunger;

    [SerializeField]
    private float hungerDecreaseRate;

    [SerializeField]
    private Image hungerBarFill;

    [Header("Thirst Settings")]
    [SerializeField]
    private float maxThirst = 100f;

    public float currentThirst;

    [SerializeField]
    private float thirstDecreaseRate;

    [SerializeField]
    private Image thirstBarFill;

    public float currentArmorPoints;

    [HideInInspector]
    public bool isDead = false;

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        currentThirst = maxThirst;
    }

    // Update is called once per frame
    void Update()
    {
        
        UpdateHungerAnThirstBarFill();
        //ou exclusif (l'un ou l'autre est a zero)
        if (currentHunger <= 0 ^ currentThirst <= 0)
        {
            TakeDamage(2.5f * Time.deltaTime);
        }
        //si les deux sont a zero
        else if (currentHunger <= 0 && currentThirst <= 0)
        {
            TakeDamage(50f * Time.deltaTime);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount * (1 - currentArmorPoints / 100);
        if (currentHealth < 0 && !isDead)
        {
            currentHealth = 0;
            Die();
        }
        UpdateHealthBarFill();
    }

    void UpdateHealthBarFill()
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;
    }   


    void UpdateHungerAnThirstBarFill()
    {
        currentHunger -= hungerDecreaseRate * Time.deltaTime;
        if (currentHunger < 0) currentHunger = 0;
        hungerBarFill.fillAmount = currentHunger / maxHunger;
        currentThirst -= thirstDecreaseRate * Time.deltaTime;
        if (currentThirst < 0) currentThirst = 0;
        thirstBarFill.fillAmount = currentThirst / maxThirst;
    }

    public void ConsumeItem(float health, float hunger, float thirst)
    {
        currentHealth += health;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateHealthBarFill();

        currentHunger += hunger;
        if (currentHunger > maxHunger) currentHunger = maxHunger;

        currentThirst += thirst;
        if (currentThirst > maxThirst) currentThirst = maxThirst;
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        isDead = true;
        playerMoveBehaviour.canMove = false;
        aimBehaviour.enabled = false;
        hungerDecreaseRate = 0;
        thirstDecreaseRate = 0;
        playerAnimator.SetTrigger("Die");
    }
    
}
