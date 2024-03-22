using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelDoorScript : MonoBehaviour
{

    public SceneAsset nextScene;

    private void OnTriggerEnter(Collider other)
    {
        print("coucou");
        if (other.GetComponent<Collider>().gameObject.TryGetComponent(out IPlayable player))
        {
            player.SwitchScene(nextScene);
        }
        else
        {
            print(other.GetComponent<Collider>().gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
