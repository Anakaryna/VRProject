using UnityEngine;

public class WeaponAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip impactSound;
    public string destructibleTag = "Destructible";

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(destructibleTag))
        {
            audioSource.PlayOneShot(impactSound);
        }
    }
}