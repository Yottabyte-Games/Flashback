using UnityEngine;
using UnityEngine.AI;

//namespace _Scripts.Enemy
public class Enemy : MonoBehaviour
{
    GameObject Player;

    NavMeshAgent NMA;

    [SerializeField] private float maxHealth, health = 100f;

    [SerializeField] FloatingHealthBar healthBar;

    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    void Start()
    {
        NMA = GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        NMA.destination = Player.transform.position;
        healthBar.UpdateHealthBar(health, maxHealth);
    }

    public void TakeDamage(float damageAmount)
    {

        health -= damageAmount;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0)
        {
            Die();
        }
        print(health);
    }

    private void Die()
    {
        //GetComponent<LootBag>().InstantiateLoot(transform.position);
        Destroy(gameObject);
    }
}

