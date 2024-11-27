using UnityEngine;
using UnityEngine.Rendering;

public class Shooter : MonoBehaviour
{
    public Transform FirePoint;
    public Transform CatWeapon;
    public GameObject Fire;
    public GameObject HitPoint;
    public GameObject ballPrefab;

    void Update()
    { 
        if (Input.GetMouseButtonDown(0))
        {
            Shooting();
            GameObject bullet = Instantiate(ballPrefab, CatWeapon.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward);
        }
    }

    public void Shooting()
    {
        RaycastHit hit;

        if (Physics.Raycast(FirePoint.position, transform.TransformDirection(Vector3.forward), out hit, 100))
        {
            Debug.DrawRay(FirePoint.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            //Instantiate(Fire, FirePoint.position, Quaternion.identity);
            Instantiate(HitPoint, hit.point, Quaternion.identity);
        }
    }
}
