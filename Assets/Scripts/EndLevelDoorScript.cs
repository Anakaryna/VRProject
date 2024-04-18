using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelDoorScript : MonoBehaviour
{

    public int sceneIndex;

    private void OnTriggerEnter(Collider other)
    {
        var  res = FindObjectsByType<imPlayer>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID);
        if (res.Length > 0)
        {
        }
        SceneManager.LoadScene(sceneIndex);
    }

    
}
