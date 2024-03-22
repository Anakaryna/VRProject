using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionAsset _actionAsset; // Référence à notre input action Asset
    [SerializeField] private DynamicMoveProvider moveProvider; // Référence au script de déplacement
    [SerializeField] private float runSpeedMultiplier = 2f; // Multiplicateur de la vitesse de course
    private float originalSpeed; // Vitesse de déplacement d'origine
    
    void Start()
    {
        var run = _actionAsset.FindActionMap("XRI RightHand").FindAction("Run");

        // Sauvegarder la vitesse originale et activer l'action de courir
        originalSpeed = moveProvider.moveSpeed;
        run.Enable();

        // Associer les fonctions callback aux évènements performed et canceled
        run.performed += OnRunPerformed;
        run.canceled += OnRunCanceled;
    }

    private void OnRunPerformed(InputAction.CallbackContext obj)
    {
        // Augmente la vitesse de déplacement lors de l'activation de Run
        moveProvider.moveSpeed = originalSpeed * runSpeedMultiplier;
        Debug.Log("Running");
    }

    private void OnRunCanceled(InputAction.CallbackContext obj)
    {
        // Rétablit la vitesse de déplacement originale lorsque Run est désactivé
        moveProvider.moveSpeed = originalSpeed;
    }
}