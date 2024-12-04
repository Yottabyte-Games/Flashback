using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Snowman_Scripts.Snowman_sc
{
    public class SnowballRoll : MonoBehaviour
    {
        float _mass = 0.4f;
    
        Rigidbody _rb;

        [FormerlySerializedAs("SnowRemover")] public GameObject snowRemover;
    
        void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_rb.linearVelocity.magnitude > 0.1f)
            {
                GrowSnowball();
            }
            if (_mass > 0.8) 
            {
                gameObject.tag = "Interactable";
            }

        }

        void GrowSnowball()
        {
            //Debug.Log(rb.linearVelocity.magnitude);
            float speed = (_rb.linearVelocity.magnitude * Time.deltaTime)/30;
            SetMass(speed);
        }
    

        void SetMass(float value)
        {
            _mass = Mathf.Clamp(value+_mass, 0.4f, 2);
            Vector3 vectorMass = new Vector3(_mass, _mass, _mass);
            transform.localScale = vectorMass;
            snowRemover.transform.localScale = vectorMass;
        
            float rbMass = Mathf.Clamp(_mass, 1, 3);
            _rb.mass = rbMass;
        }
    }
}