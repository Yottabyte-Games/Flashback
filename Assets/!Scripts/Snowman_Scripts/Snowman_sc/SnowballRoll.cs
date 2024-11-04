using UnityEngine;

public class SnowballRoll : MonoBehaviour
{
    float mass = 0.4f;
    
    Rigidbody rb;

    public GameObject SnowRemover;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            GrowSnowball();
        }
        if (mass > 0.8) 
        {
            gameObject.tag = "Interactable";
        }

    }

    void GrowSnowball()
    {
        //Debug.Log(rb.linearVelocity.magnitude);
        float speed = (rb.linearVelocity.magnitude * Time.deltaTime)/30;
        SetMass(speed);
    }
    

    void SetMass(float value)
    {
        mass = Mathf.Clamp(value+mass, 0.4f, 2);
        Vector3 vectorMass = new Vector3(mass, mass, mass);
        transform.localScale = vectorMass;
        SnowRemover.transform.localScale = vectorMass;
        
        float rbMass = Mathf.Clamp(mass, 1, 3);
        rb.mass = rbMass;
    }
}