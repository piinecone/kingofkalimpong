using UnityEngine;
using System.Collections;

public class PlayerVehicleCreator : uLink.MonoBehaviour {
  [SerializeField]
  private GameObject vehicleBody;

  private Camera mainCamera;
  private SlingshotCreator slingshot;
  private ProjectileCreator projectile;
  private bool destroyed = false;
  private ServerLogic server;

  void Awake(){
    server = GameObject.FindWithTag("Server").GetComponent<ServerLogic>();
    slingshot = GetComponentInChildren<SlingshotCreator>();
  }

  public Vector3 Velocity(){
    return rigidbody.velocity;
  }

  [RPC]
  public void AimSlingshot(Quaternion rotation){
    if (!destroyed){
      networkView.RPC("AimSlingshot", uLink.RPCMode.OthersExceptOwner, rotation);
      slingshot.Aim(rotation);
    }
  }

  [RPC]
  public void ReloadProjectile(uLink.NetworkPlayer player, int ownerViewId){
    if (!destroyed){
      //networkView.RPC("ReloadProjectile", uLink.RPCMode.OthersExceptOwner, ownerViewId);
      projectile = slingshot.Reload(player, ownerViewId);
    }
  }

  [RPC]
  public void LaunchProjectile(uLink.NetworkPlayer player, float launchForce, int shooterId){
    if (!destroyed && projectile != null){
      Vector3 launchVector = slingshot.GetLaunchVector(launchForce);
      Vector3 relativeForce = slingshot.GetRelativeForceVector();
      projectile.Fire(launchVector, relativeForce);
      networkView.RPC("ReleaseProjectile", player);
      networkView.RPC("ReleaseProjectile", uLink.RPCMode.OthersExceptOwner);
      slingshot.AddToTrackedProjectiles(projectile);
    }
  }

  public void OnTriggerEnter(Collider aCollider){
    if (shouldBeDestroyedBy(aCollider))
      networkView.RPC("DestroyVehicle", uLink.RPCMode.All, aCollider.transform.position);
  }

  [RPC]
  public void DestroyVehicle(Vector3 impactPosition){
    collider.isTrigger = true;
    vehicleBody.collider.isTrigger = true;
    destroyed = true;
    projectile.Loosen();
    rigidbody.constraints = RigidbodyConstraints.FreezeAll;
  }

  [RPC]
  public void RespawnPlayer(uLink.NetworkMessageInfo info){
    server.DestroyAndRespawnPlayer(info.sender);
    //networkView.RPC("ReanimateAt", uLink.RPCMode.All, server.RandomSpawnPoint().position);
  }

  private void explode(Collider aCollider){
    //foreach (VehicleComponent component in vehicleComponents)
    //  component.DetachWithForce(rigidbody.velocity);
    //generateExplosiveForceAtPosition(aCollider.transform.position); // FIXME move to coroutine and delay
    //vehicleController.readUserInput = false;
  }

  private void playDestructionSounds(){
    //destructionAudio.Play();
  }

  private bool shouldBeDestroyedBy(Collider aCollider){
    if (!destroyed && aCollider.rigidbody != null){
      float impactForce = aCollider.rigidbody.velocity.magnitude;
      if (shouldBeDestroyedByVehicleCollisionWith(aCollider, impactForce)) return true;
      if (shouldBeDestroyedByProjectileCollisionWith(aCollider, impactForce)) return true;
    }
    return false;
  }

  private bool shouldBeDestroyedByVehicleCollisionWith(Collider aCollider, float impactForce){
    return (aCollider.gameObject.tag == "Nugget" && impactForce >= 21f &&
      rigidbody.velocity.magnitude <= impactForce && impactIsCloseEnoughToImpactPoint(aCollider, impactForce + 10f));
  }

  private bool shouldBeDestroyedByProjectileCollisionWith(Collider aCollider, float impactForce){
    return (aCollider.gameObject.tag == "Rock" && impactForce >= 13f &&
        !slingshot.LaunchedThisProjectile(aCollider.gameObject) && impactIsCloseEnoughToImpactPoint(aCollider, impactForce));
  }

  private bool impactIsCloseEnoughToImpactPoint(Collider aCollider, float impactForce){
    Vector3 impactPoint = aCollider.ClosestPointOnBounds(transform.position);
    float distance = Vector3.Distance(transform.position, impactPoint);
    float thresholdDelta = Mathf.Max(0f, (impactForce - 13f) * .1f);
    return distance <= (1.5f + thresholdDelta);
  }

}

//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//
//[RequireComponent(typeof(CarControl))]
//public class PlayerVehicle : MonoBehaviour {
//
//  [SerializeField]
//  private GameObject projectile;
//
//  [SerializeField]
//  private Transform impactPoint;
//
//  [SerializeField]
//  private GameObject vehicleBody;
//
//  [SerializeField]
//  private bool nonPlayerCharacter = false;
//
//  private Camera mainCamera;
//  private ProjectileCamera projectileCamera;
//  private Slingshot slingshot;
//  private List<VehicleComponent> vehicleComponents = new List<VehicleComponent>();
//  private CarControl vehicleController;
//  private CarSound vehicleAudio;
//  private CarSettings vehicleSettings;
//  private DestructionAudio destructionAudio;
//  private bool destroyed = false;
//
//  void Awake(){
//    getVehicleComponents();
//    getSlingshot();
//    getVehicleBodyComponents();
//    setupCameras();
//    getDestructionAudio();
//    projectileCamera.Initialize(camera: mainCamera, slingshot: slingshot);
//    slingshot.projectileCamera = projectileCamera;
//    if (nonPlayerCharacter) vehicleController.readUserInput = false;
//  }
//
//  void Start(){
//    // temporary
//    if (!NonPlayerCharacter())
//      mainCamera.GetComponent<CamSmoothFollow>().target = vehicleController.CenterOfMass;
//    else
//      vehicleSettings.externalInput = true;
//  }
//
//  void Update(){
//  }
//
//  private void getVehicleComponents(){
//    vehicleController = GetComponent<CarControl>();
//    vehicleAudio = GetComponent<CarSound>();
//    vehicleSettings = GetComponent<CarSettings>();
//  }
//
//  private void getSlingshot(){
//    slingshot = GetComponentInChildren<Slingshot>();
//  }
//
//  private void getVehicleBodyComponents(){
//    VehicleComponent[] components = GetComponentsInChildren<VehicleComponent>(true);
//    foreach(VehicleComponent component in components)
//      vehicleComponents.Add(component);
//  }
//
//  private void setupCameras(){
//    mainCamera = GameObject.FindWithTag("MainCamera").camera;
//    projectileCamera = GetComponentInChildren<ProjectileCamera>() as ProjectileCamera;
//    //if (networkView.isMine){
//    mainCamera.GetComponent<CamSmoothFollow>().target = vehicleController.CenterOfMass;
//    //}
//  }
//
//  private void getDestructionAudio(){
//    destructionAudio = GetComponentInChildren<DestructionAudio>();
//  }
//
//
//  public bool NonPlayerCharacter(){
//    return !vehicleController.readUserInput;
//  }
//
//}
