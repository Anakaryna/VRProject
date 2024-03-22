using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPlaceholderScript : MonoBehaviour, IDamagable, IPlayable
{

    public void SwitchScene(SceneAsset scene)
    {
        SceneManager.LoadSceneAsync(scene.name, LoadSceneMode.Single);
    }

    public GameObject player;
    
    public int Health { get; set; }

    public int startingHealth = 100;

    public void Hit(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            SelfDestroy();
        }
    }

    public void CriticalHit(int damage)
    {
        SelfDestroy();
    }

    public void SelfDestroy()
    {
        Destroy(player);
    }

    private void OnEnable()
    {
        Health = startingHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        Health = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
