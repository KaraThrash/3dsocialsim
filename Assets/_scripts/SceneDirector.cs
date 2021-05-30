using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDirector : MonoBehaviour
{
    public ScriptableScene activeScene;

    public Player player;

    public Villager primary, secondary;

    public List<Villager> villagers;
    public List<MapLocation> locations;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitScene(ScriptableScene _scene)
    {

        primary = _scene.primary;
        secondary = _scene.secondary;



        activeScene = _scene;
    }


}
