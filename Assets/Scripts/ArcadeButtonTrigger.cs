using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArcadeButtonTrigger : MonoBehaviour
{

    public int sceneIndex;
    private void OnTriggerEnter(Collider other)
    {
        var  res = FindObjectsByType<imPlayer>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID);
        if (res.Length > 0)
        {
            res[0].gameObject.SetActive(false);
        }
        SceneManager.LoadScene(sceneIndex);
    }
}
