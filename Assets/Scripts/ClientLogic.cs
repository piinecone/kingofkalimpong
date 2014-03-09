using UnityEngine;
using System.Net;

[AddComponentMenu("uLink Utilities/Client GUI")]
public class ClientLogic : MonoBehaviour {

  private bool connected = false;
  private bool connecting = false;

  void Update(){
    if (connected || connecting) return;

    uLink.MasterServer.RequestHostList("KingOfKalimpong");
    uLink.HostData[] hosts = uLink.MasterServer.PollHostList();
    if (hosts != null && hosts.Length > 0){
      connecting = true;
      uLink.HostData host = hosts[0];
      Debug.Log("Connecting to " + host.gameName);
      uLink.Network.Connect(host.ip, host.port);
      //uLink.Network.Connect("127.0.0.1", 7100);
      //uLink.Network.Connect("192.168.1.111", 7100);
    }
  }
  
  void uLink_OnConnectedToServer(IPEndPoint server){
    connected = true;
    Debug.Log("Connected to server on port " + server.Port);
  }

  void uLink_OnFailedToConnect(uLink.NetworkConnectionError error){
    connecting = false;
    Debug.Log("Failed to connect to the game server. Retrying...");
  }
}
