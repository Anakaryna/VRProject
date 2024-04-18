/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCreator : MonoBehaviour
{
    public int countX;
    public int countY;
    public int countZ;
    public GameObject body;
    public GameObject top;

    private void OnValidate()
    {
        Apply();
    }

    private void Apply()
    {
        var originalRotation = gameObject.transform.rotation;
        
        if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
        {
            return;
        }

        if (body == null || top == null)
        {
            return;
        }

        Renderer rendererBody = body.GetComponent<Renderer>();
        Renderer rendererTop = top.GetComponent<Renderer>();
        
        

        if (rendererBody != null && rendererTop != null)
        {
            foreach (Transform t in transform)
            {
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    DestroyImmediate(t.gameObject);
                };
            }
        }
        else
        {
            print("missing object");
            return;
        }

        float lastX = 0;
        float lastY = 0;
        float lastZ = 0;

        gameObject.transform.rotation = new Quaternion();
        
        for (int i = 0; i < countZ; ++i)
        {
            for (int k = 0; k < countX; ++k)
            {
                Vector3 pos = new Vector3(lastX + transform.localPosition.x, lastY + transform.localPosition.y,
                    lastZ + transform.localPosition.z);
                
                GameObject go = Instantiate(top, pos, top.transform.rotation, transform) as GameObject;
                go.name = top.name + "_" + i + "_" + k;
                lastX += rendererTop.bounds.size.x;
            }
            lastX = 0;
            lastZ += rendererTop.bounds.size.z;
        }
        
        lastX = 0;
        lastY = -rendererBody.bounds.size.y;
        lastZ = 0;

        for (int i = 0; i < countZ; ++i)
        {
            for (int j = 0; j < countY; ++j)
            {
                for (int k = 0; k < countX; ++k)
                {
                    if (i==0 || i==countZ-1 || k==0 || k==countX-1)
                    {
                        Vector3 pos = new Vector3(lastX + transform.localPosition.x, lastY + transform.localPosition.y,
                            lastZ + transform.localPosition.z);
                
                        GameObject go = Instantiate(body, pos, body.transform.rotation, transform) as GameObject;
                        go.name = body.name + "_" + i + "_" + j + "_" + k;
                        
                    }
                    lastX += rendererBody.bounds.size.x;
                }

                lastX = 0;
                lastY -= rendererBody.bounds.size.y;
            }

            lastY = -rendererBody.bounds.size.y;;
            lastZ += rendererBody.bounds.size.z;
        }

        var height = rendererTop.bounds.size.y + (rendererBody.bounds.size.y * countY);
        var width = (rendererBody.bounds.size.x * countX);
        var weigth = (rendererBody.bounds.size.z * countZ);

        var box = gameObject.GetComponent<BoxCollider>();
        box.size = new Vector3(width, height, weigth);
        box.center = new Vector3((width / 2) - (rendererTop.bounds.size.x / 2), (-height / 2) + rendererTop.bounds.size.y,
            (weigth / 2) - (rendererTop.bounds.size.z / 2));

        gameObject.transform.rotation = originalRotation;

    }
}
*/