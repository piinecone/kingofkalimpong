using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerLogic : MonoBehaviour {
  private Camera mainCamera;
  public List<GameObject> spawnPoints = new List<GameObject>(); 

  void Awake(){
    uLink.Network.isAuthoritativeServer = true;
    mainCamera = GameObject.FindWithTag("MainCamera").camera;
  }

  void Start(){
    mainCamera.GetComponent<CamSmoothFollow>().target = spawnPoints[0].transform;
  }

  public Transform RandomSpawnPoint(){
    return spawnPoints[Random.Range(0, spawnPoints.Count - 1)].transform;
  }

  void uLink_OnPlayerConnected(uLink.NetworkPlayer player){
    GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count - 1)];
    uLink.Network.Instantiate(player, "PlayerVehicle@Proxy", "PlayerVehicle@Owner", "PlayerVehicle@Creator", spawnPoint.transform.position, spawnPoint.transform.rotation, 0);
  }

  public void DestroyAndRespawnPlayer(uLink.NetworkPlayer player){
    uLink.Network.DestroyPlayerObjects(player);
    GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count - 1)];
    uLink.Network.Instantiate(player, "PlayerVehicle@Proxy", "PlayerVehicle@Owner", "PlayerVehicle@Creator", spawnPoint.transform.position, spawnPoint.transform.rotation, 0);
  }
}
