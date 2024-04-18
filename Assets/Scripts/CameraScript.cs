using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.GameObject().TryGetComponent(out ICriticalHit critical))
                {
                    critical.SendCritical(0);
                } else if (hit.collider.GameObject().TryGetComponent(out IDamagable damagable))
                {
                    damagable.Hit(35);
                }
            }
        }
    }
}
