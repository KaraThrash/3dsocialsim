using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public  class GameScenePicker : MonoBehaviour
{
    public  void LoadScene(int _scene)
    {
        SceneManager.LoadScene(_scene);
    }
}
