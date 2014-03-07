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
    return spawnPoints[Random.Range(0, spawnPoints.Count)].transform;
  }

  void uLink_OnPlayerConnected(uLink.NetworkPlayer player){
    Transform spawnPoint = RandomSpawnPoint();
    uLink.Network.Instantiate(player, "PlayerVehicle@Proxy", "PlayerVehicle@Owner", "PlayerVehicle@Creator", spawnPoint.position, spawnPoint.rotation, 0);
  }

  public void DestroyAndRespawnPlayer(uLink.NetworkPlayer player){
    uLink.Network.DestroyPlayerObjects(player);
    Transform spawnPoint = RandomSpawnPoint();
    uLink.Network.Instantiate(player, "PlayerVehicle@Proxy", "PlayerVehicle@Owner", "PlayerVehicle@Creator", spawnPoint.position, spawnPoint.rotation, 0);
  }
}
