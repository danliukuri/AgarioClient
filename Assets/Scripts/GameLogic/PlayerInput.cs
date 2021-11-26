using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    #region Fields
    [SerializeField] Camera playerCamera;
    #endregion

    #region Methods
    void FixedUpdate()
    {
        SendToServer();
    }

    void SendToServer()
    {
        Vector2 position = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        ClientPacketsSender.PlayerMovement(position);
    }
    #endregion
}
