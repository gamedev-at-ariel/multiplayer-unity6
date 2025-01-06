using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
using Unity.Netcode;

public class ClientPlayerMove : NetworkBehaviour
{
    [SerializeField] PlayerInput m_PlayerInput;
    [SerializeField] StarterAssetsInputs m_StarterAssetsInputs;
    [SerializeField] ThirdPersonController m_ThirdPersonController;
    void Awake() {
        m_PlayerInput.enabled = false;
        m_StarterAssetsInputs.enabled = false;
        m_ThirdPersonController.enabled = false;
    }

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        if (IsOwner) {
            m_PlayerInput.enabled = true;
            m_StarterAssetsInputs.enabled = true;
        }

        if (IsServer) {
            m_ThirdPersonController.enabled = true;
        }
    }


    // This function can be called by clients, but it runs only on the server. 
    [Rpc(SendTo.Server)]
    private void UpdateInputServerRpc(Vector2 move, Vector2 look, bool jump, bool sprint) {
        // only sets values of internal variables on the server
        m_StarterAssetsInputs.MoveInput(move);
        m_StarterAssetsInputs.LookInput(look);
        m_StarterAssetsInputs.JumpInput(jump);
        m_StarterAssetsInputs.SprintInput(sprint);
    }

    private void LateUpdate() {
        if (!IsOwner) return;

        UpdateInputServerRpc(m_StarterAssetsInputs.move, m_StarterAssetsInputs.look, m_StarterAssetsInputs.jump, m_StarterAssetsInputs.sprint);
    }
}
