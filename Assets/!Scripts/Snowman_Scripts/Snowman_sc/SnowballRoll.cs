using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Snowman_Scripts.Snowman_sc
{
    public class SnowballRoll : MonoBehaviour
    {
<<<<<<< HEAD
        float mass = 0.4f;
    
        Rigidbody rb;

        public GameObject SnowRemover;
    
        void Start()
        {
            rb = GetComponent<Rigidbody>();
=======
        float _mass = 0.4f;
    
        Rigidbody _rb;

        [FormerlySerializedAs("SnowRemover")] public GameObject snowRemover;
    
        void Start()
        {
            _rb = GetComponent<Rigidbody>();
>>>>>>> Build
        }

        // Update is called once per frame
        void Update()
        {
<<<<<<< HEAD
            if (rb.linearVelocity.magnitude > 0.1f)
            {
                GrowSnowball();
            }
            if (mass > 0.8) 
=======
            if (_rb.linearVelocity.magnitude > 0.1f)
            {
                GrowSnowball();
            }
            if (_mass > 0.8) 
>>>>>>> Build
            {
                gameObject.tag = "Interactable";
            }

        }

        void GrowSnowball()
        {
            //Debug.Log(rb.linearVelocity.magnitude);
<<<<<<< HEAD
            float speed = (rb.linearVelocity.magnitude * Time.deltaTime)/30;
=======
            float speed = (_rb.linearVelocity.magnitude * Time.deltaTime)/30;
>>>>>>> Build
            SetMass(speed);
        }
    

        void SetMass(float value)
        {
<<<<<<< HEAD
            mass = Mathf.Clamp(value+mass, 0.4f, 2);
            Vector3 vectorMass = new Vector3(mass, mass, mass);
            transform.localScale = vectorMass;
            SnowRemover.transform.localScale = vectorMass;
        
            float rbMass = Mathf.Clamp(mass, 1, 3);
            rb.mass = rbMass;
=======
            _mass = Mathf.Clamp(value+_mass, 0.4f, 2);
            Vector3 vectorMass = new Vector3(_mass, _mass, _mass);
            transform.localScale = vectorMass;
            snowRemover.transform.localScale = vectorMass;
        
            float rbMass = Mathf.Clamp(_mass, 1, 3);
            _rb.mass = rbMass;
>>>>>>> Build
        }
    }
}