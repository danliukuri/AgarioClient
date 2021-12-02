using UnityEngine;

public partial class Client : MonoBehaviour
{
    #region Properties
    public static int Id { get; set; }
    public static TCP Tcp { get; private set; }
    public static UDP Udp { get; private set; }
    #endregion

    #region Delegates
    public delegate void PacketHandler(Packet packet);
    #endregion

    #region Fields
    [SerializeField] string ip;
    [SerializeField] int port;
    static PacketHandler[] packetHandlers;
    static bool isConnected;
    static Client instance;
    #endregion

    #region Methods
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    void OnApplicationQuit()
    {
        Disconnect();
    }

    public static PacketHandler GetPacketHandler(int index) => packetHandlers[index];
    public static void ConnectToServer()
    {
        Tcp = new TCP(instance);
        Udp = new UDP(instance, instance.ip, instance.port);

        InitializeClientData();

        isConnected = true;
        Tcp.Connect(instance.ip, instance.port);
    }
    static void InitializeClientData()
    {
        packetHandlers = new PacketHandler[]
        {
            ClientPacketsHandler.Welcome,
            ClientPacketsHandler.FieldGenerated,
            ClientPacketsHandler.CurrentFieldSectorUpdate,
            ClientPacketsHandler.SpawnPlayer,
            ClientPacketsHandler.RemovePlayer,
            ClientPacketsHandler.PlayerMovement,
            ClientPacketsHandler.SpawnFood,
            ClientPacketsHandler.RemoveFood
        };
        Debug.Log("Initialized packets.");
    }
    public static void Disconnect()
    {
        if (isConnected)
        {
            isConnected = false;
            Tcp.Socket.Close();
            Udp.Socket.Close();

            Debug.Log("Disconnected from server.");
        }
    }
    #endregion
}
