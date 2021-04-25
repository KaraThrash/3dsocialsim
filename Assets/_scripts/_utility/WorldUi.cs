using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUi : MonoBehaviour
{
    public Transform followThis;
    public float textDisplayTime,timer;

    private TextMesh text;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<TextMesh>() != null)
        { text = GetComponent<TextMesh>(); }
    }

    // Update is called once per frame
    void Update()
    {
        if (followThis != null)
        {
            transform.parent.position = followThis.position;
            transform.parent.rotation = new Quaternion(0,0,0,0);
        }

        if (timer != -1)
        {
            timer -= Time.deltaTime;
            if (timer <= 0) { SetText(""); }
        }

    }

    public void SetColor(Material _mat)
    {
        text.color =  _mat.color;
    }

    public void SetText(string _text = "")
    {
        if (GetComponent<TextMesh>() == null)
        { text = GetComponent<TextMesh>(); }
        if (text == null)
        { return; }

        text.text = _text;
        if (_text.Equals("") == false)
        { timer = textDisplayTime; }
        else
        { timer = -1; }
        
    }

    public void SetText(string _text ,float _displayTime)
    {
        if (GetComponent<TextMesh>() == null)
        { text = GetComponent<TextMesh>(); }
        if (text == null)
        { return; }

        text.text = _text;
        if (_text.Equals("") == false)
        { timer = _displayTime; }
        else
        { timer = -1; }

    }

}
