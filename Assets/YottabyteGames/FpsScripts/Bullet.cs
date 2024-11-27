using UnityEngine;

public class Bullet : MonoBehaviour
{
    public ParticleSystem HitPoint;

    private void OnCollisionEnter(Collision collision)
    {
        HitPoint.Play();
        print(Instantiate(HitPoint, transform.position, transform.rotation));

        Destroy(gameObject);
    }
}
