using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionAsset _actionAsset; // Référence a notre input action Asset
    
    void Start()
    {
        //On recupere l'action qu'on a créé a partir de l'action Map 
        var run = _actionAsset.FindActionMap("XRI RightHand").FindAction("Run");
        
        //On "Active" l'action créé 
        run.Enable();

        //on associe une fonction callback que l'on donne en parametre a l'evenement Performed pour l'action créé
        run.performed += OnRunPerformed;
    }

    //On créé notre fonction callback qui contiendra toute notre logique de code
    private void OnRunPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("Running");
    }
}