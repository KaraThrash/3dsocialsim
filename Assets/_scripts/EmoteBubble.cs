using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteBubble : MonoBehaviour
{
    public float displayTime, timer;

    public MeshRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(0, 0, 0, 0), Time.deltaTime);

        if (timer != -1)
        {
            
                //Quaternion.identity;
            timer -= Time.deltaTime;
            if (timer <= 0) 
            {
                timer = -1;
                gameObject.SetActive(false); 
            }
        }

    }



    public void SetMaterial(Material _mat, float _duration = 1)
    {
       

        if (renderer == null)
        { return; }



        renderer.material = _mat;

        timer = _duration; 
   

    }

    public void SetMaterial(Material _mat)
    {


        if (renderer == null)
        { return; }



        renderer.material = _mat;

        timer = displayTime;


    }

}