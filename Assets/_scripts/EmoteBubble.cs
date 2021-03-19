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

        if (timer != -1)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity,Time.deltaTime);
                //Quaternion.identity;
            timer -= Time.deltaTime;
            if (timer <= 0) 
            {
                timer = -1;
                gameObject.SetActive(false); 
            }
        }

    }



    public void SetMaterial(Material _mat)
    {
       

        if (renderer == null)
        { return; }



        renderer.material = _mat;

        timer = displayTime; 
   

    }

}