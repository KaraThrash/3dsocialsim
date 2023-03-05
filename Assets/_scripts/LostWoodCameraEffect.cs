using UnityEngine;

public class LostWoodCameraEffect : CameraEffect
{
    public Transform leaf0, leaf1, leaf2, leaf3;

    public override void StartEffect()
    {
        leaf0.gameObject.SetActive(true);
        leaf1.gameObject.SetActive(true);
        leaf2.gameObject.SetActive(true);
        leaf3.gameObject.SetActive(true);
    }

    public override void EndEffect()
    {
        leaf0.gameObject.SetActive(false);
        leaf1.gameObject.SetActive(false);
        leaf2.gameObject.SetActive(false);
        leaf3.gameObject.SetActive(false);
    }

    public override void Effect(Vector3 _moveDir)
    {
        //move dir is the move direction of the player

        float tempspeed = speed;
        Vector3 tempos = new Vector3(0, 0, 0);

        if (_moveDir.z > 0.1f)
        {
            tempos += new Vector3(0, 0.1f, 0);
        }
        else if (_moveDir.z < -0.1f)
        {
            tempspeed = -speed;
            tempos += new Vector3(0, -0.1f, 0);
        }

        if (_moveDir.x < -0.1f)
        {
            tempspeed = -speed;
            tempos += new Vector3(0.1f, 0, 0);
        }
        else if (_moveDir.x > 0.1f)
        {
            tempos += new Vector3(-0.1f, 0, 0);
        }

        transform.Rotate(0, 0, tempspeed * 0.5f * Time.deltaTime);
        leaf0.transform.Rotate(0, tempspeed * Time.deltaTime, 0);
        leaf1.transform.Rotate(0, tempspeed * Time.deltaTime, 0);
        leaf2.transform.Rotate(0, tempspeed * 0.5f * Time.deltaTime, 0);
        leaf3.transform.Rotate(0, tempspeed * 0.5f * Time.deltaTime, 0);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, tempos, speed * 0.01f * Time.deltaTime);
    }
}