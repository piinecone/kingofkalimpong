using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerLogic : MonoBehaviour {
  private Camera mainCamera;
  public List<GameObject> spawnPoints = new List<GameObject>(); 
  public List<CarControl> vehicles = new List<CarControl>(); 
  private float cameraFocusedOnTargetDuration = 10f;
  private float timeLeftFocusingOnThisTarget = 0f;

  void Awake(){
    uLink.Network.isAuthoritativeServer = true;
    mainCamera = GameObject.FindWithTag("MainCamera").camera;
  }

  void Start(){
    uLink.Network.InitializeServer(8, 25002);
    uLink.MasterServer.RegisterHost("KingOfKalimpong", "Mammoth", "The King of Kalimpong");
    mainCameraFollow(spawnPoints[0].transform);
  }

  private void mainCameraFollow(Transform aTransform){
    mainCamera.GetComponent<CamSmoothFollow>().target = aTransform;
  }

  private void focusOnNextTarget(){
    Debug.Log("switching targets...");
    if (vehicles.Count > 0){
      Transform nextTarget = vehicles[Random.Range(0, vehicles.Count)].CenterOfMass;
      mainCameraFollow(nextTarget);
      timeLeftFocusingOnThisTarget = cameraFocusedOnTargetDuration;
    }
  }

  void LateUpdate(){
    if (vehicles.Count > 0 && timeLeftFocusingOnThisTarget <= 0f)
      focusOnNextTarget();
    else
      timeLeftFocusingOnThisTarget -= Time.deltaTime;
  }

  public Transform RandomSpawnPoint(){
    return spawnPoints[Random.Range(0, spawnPoints.Count)].transform;
  }

  void uLink_OnPlayerConnected(uLink.NetworkPlayer player){
    Transform spawnPoint = RandomSpawnPoint();
    GameObject playerVehicle = uLink.Network.Instantiate(player, "PlayerVehicle@Proxy", "PlayerVehicle@Owner", "PlayerVehicle@Creator", spawnPoint.position, spawnPoint.rotation, 0);
    vehicles.Add(playerVehicle.GetComponent<CarControl>());
  }

  public void DestroyAndRespawnPlayer(uLink.NetworkPlayer player){
    uLink.Network.DestroyPlayerObjects(player);
    Transform spawnPoint = RandomSpawnPoint();
    uLink.Network.Instantiate(player, "PlayerVehicle@Proxy", "PlayerVehicle@Owner", "PlayerVehicle@Creator", spawnPoint.position, spawnPoint.rotation, 0);
  }
}
