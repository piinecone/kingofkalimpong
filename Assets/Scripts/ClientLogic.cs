using UnityEngine;
using System.Net;

[AddComponentMenu("uLink Utilities/Client GUI")]
public class ClientLogic : MonoBehaviour {

  void Start () {
    uLink.Network.Connect("127.0.0.1", 7100);
  }
  
  void uLink_OnConnectedToServer(IPEndPoint server){
     Debug.Log("Connected to server on port " + server.Port);
  }
}
