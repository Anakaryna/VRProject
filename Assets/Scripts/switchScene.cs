using UnityEngine;
using UnityEngine.SceneManagement;

public class switchScene : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject);
        SceneManager.LoadScene(1);
    }
}
