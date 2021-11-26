using UnityEngine;

public class ClientPacketsSender : MonoBehaviour
{
    #region PacketsSending
    public static void WelcomeReceived()
    {
        using (Packet packet = new Packet((int)ClientPackets.WelcomeReceived))
        {
            packet.Write(Client.Id);
            packet.Write(UIManager.UsernameField.text);

            SendTCPData(packet);
        }
    }

    public static void PlayerMovement(Vector2 position)
    {
        using (Packet packet = new Packet((int)ClientPackets.PlayerMovement))
        {
            packet.Write(position);
            SendUDPData(packet);
        }
    }
    #endregion

    #region WaysToSend
    private static void SendTCPData(Packet packet)
    {
        packet.WriteLength();
        Client.Tcp.SendData(packet);
    }

    private static void SendUDPData(Packet packet)
    {
        packet.WriteLength();
        Client.Udp.SendData(packet);
    }
    #endregion
}