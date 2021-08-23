using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dieintime : MonoBehaviour
{

    // Start is called before the first frame update
    public float lifetime, defaultLifetime = 2;
    public Transform reserveParent;
    public bool die; //or disable

       // Start is called before the first frame update
       void Start()
       {

       }

    private void OnEnable()
    {
        lifetime = defaultLifetime;


    }

    // Update is called once per frame
    void Update()
        {
                TrackLifeTime();
        }

        public virtual void TrackLifeTime()
        {
                if (lifetime != -1)
                {
                    lifetime -= Time.deltaTime;
                    if (lifetime <= 0) { Die(); }
                }
        }

        public virtual void Die()
        {
                if (reserveParent == null)
                {
                     

                    if (die)
                    {
                            Destroy(this.gameObject);
                    }
                    else 
                    {
                         gameObject.SetActive(false);
                    }

                }
                 transform.parent = reserveParent;
                    gameObject.SetActive(false);
                
        }
  }
