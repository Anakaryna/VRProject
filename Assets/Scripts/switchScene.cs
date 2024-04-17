using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class switchScene : MonoBehaviour
{
    public SceneAsset scene;
    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject);
        SceneManager.LoadScene(scene.name);
    }
}
