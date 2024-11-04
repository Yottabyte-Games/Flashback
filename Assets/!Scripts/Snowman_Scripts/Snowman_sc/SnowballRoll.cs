using UnityEngine;

namespace _Scripts.Snowman_Scripts.Snowman_sc
{
<<<<<<< HEAD
    private float mass = 0.4f;
    
    private Rigidbody rb;

    public GameObject SnowRemover;
    
    void Start()
=======
    public class SnowballRoll : MonoBehaviour
>>>>>>> main
    {
        private float mass = 0.5f;
    
        private Rigidbody rb;

        public GameObject SnowRemover;
    
        void Start()
        {
<<<<<<< HEAD
            GrowSnowball();
        }
        if (mass > 0.8) 
        {
            gameObject.tag = "Interactable";
=======
            rb = GetComponent<Rigidbody>();
>>>>>>> main
        }

        // Update is called once per frame
        void Update()
        {
            if (rb.linearVelocity.magnitude > 0.1f)
            {
                GrowSnowball();
            }
            if (mass > 1) 
            {
                gameObject.tag = "Interactable";
            }

<<<<<<< HEAD
    void GrowSnowball()
    {
        //Debug.Log(rb.linearVelocity.magnitude);
        float speed = (rb.linearVelocity.magnitude * Time.deltaTime)/30;
        SetMass(speed);
    }
=======
        }

        void GrowSnowball()
        {
            Debug.Log(rb.linearVelocity.magnitude);
            var speed = (rb.linearVelocity.magnitude * Time.deltaTime)/30;
            SetMass(speed);
        }
>>>>>>> main
    

        void SetMass(float value)
        {
            mass = Mathf.Clamp(value+mass, 0.4f, 2);
            var vectorMass = new Vector3(mass, mass, mass);
            transform.localScale = vectorMass;
            SnowRemover.transform.localScale = vectorMass;
        
            var rbMass = Mathf.Clamp(mass, 1, 3);
            rb.mass = rbMass;
        }
    }
}
