using UnityEngine;
using UnityEngine.SceneManagement;

public class switchScene : MonoBehaviour
{
    public int sceneIndex;
    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject);
        SceneManager.LoadScene(sceneIndex);
    }
}
