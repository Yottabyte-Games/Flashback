using UnityEngine;

//namespace _Scripts.Enemy
public class Bullet : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    public ParticleSystem HitPoint;

    [SerializeField] private float damageAmount = 10;

    private void OnCollisionEnter(Collision col)
    {
        HitPoint.Play();
        print(Instantiate(HitPoint, transform.position, transform.rotation));

        if (col.gameObject.tag == "Enemy")
        {
            enemy = col.gameObject.GetComponent<Enemy>();

            enemy.TakeDamage(damageAmount);
            Debug.Log("HIIIT");
        }

        Destroy(gameObject);
    }
}
