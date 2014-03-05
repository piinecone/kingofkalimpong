using UnityEngine;
using System.Collections.Generic;

public class Instantiate : MonoBehaviour {
  public GameObject PlayerVehiclePrefab;
  public List<GameObject> spawnPoints = new List<GameObject>(); 

  void uLink_OnConnectedToServer() {
    GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count - 1)];
    uLink.Network.Instantiate(PlayerVehiclePrefab, spawnPoint.transform.position, spawnPoint.transform.rotation, 0);
  }
}
