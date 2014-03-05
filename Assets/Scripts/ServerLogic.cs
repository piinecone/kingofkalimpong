using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerLogic : MonoBehaviour {
  public List<GameObject> spawnPoints = new List<GameObject>(); 

  void uLink_OnPlayerConnected(uLink.NetworkPlayer player){
    GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count - 1)];
    uLink.Network.Instantiate(player, "PlayerVehicle@Proxy", "PlayerVehicle@Owner", "PlayerVehicle@Creator", spawnPoint.transform.position, spawnPoint.transform.rotation, 0);
  }
}
