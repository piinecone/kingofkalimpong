using UnityEngine;
using System.Collections;

public class PlayerVehicleCreator : uLink.MonoBehaviour {
  private Camera mainCamera;
  private SlingshotCreator slingshot;
  private ProjectileCreator projectile;
  private bool destroyed = false;

  void Awake(){
    slingshot = GetComponentInChildren<SlingshotCreator>();
  }

  void Start () {
    //mainCamera = GameObject.FindWithTag("MainCamera").camera;
    //mainCamera.GetComponent<CamSmoothFollow>().target = transform;
  }

  public Vector3 Velocity(){
    return rigidbody.velocity;
  }

  // FIXME this should probably be moved into the SlingshotCreator class, if it will remain accessible there
  [RPC]
  public void AimSlingshot(Quaternion rotation){
    networkView.RPC("AimSlingshot", uLink.RPCMode.OthersExceptOwner, rotation);
    slingshot.Aim(rotation);
  }

  [RPC]
  public void ReloadProjectile(uLink.NetworkPlayer player, int ownerViewId){
    //networkView.RPC("ReloadProjectile", uLink.RPCMode.OthersExceptOwner, player);
    projectile = slingshot.Reload(player, ownerViewId);
  }

  [RPC]
  public void LaunchProjectile(Vector3 launchForce, Vector3 relativeForce, int shooterId){
    if (projectile != null){
      projectile.Fire(launchForce, relativeForce);
    }
  }

  public void OnTriggerEnter(Collider aCollider){
    if (shouldBeDestroyedBy(aCollider)){
      //DestroyVehicle(aCollider);
      Debug.Log("Server: destroying vehicle");
      //destroyVehicle(aCollider);
      networkView.RPC("DestroyVehicle", uLink.RPCMode.All, aCollider.transform.position);
    }
  }

  [RPC]
  public void DestroyVehicle(Vector3 impactPosition){
    Debug.Log("Server received destroy vehicle command via RPC");
    //collider.enabled = false;
    //vehicleBody.collider.enabled = false;
    //playDestructionSounds();
    //explode(aCollider);
    //slingshot.Deactivate();
    //destroyed = true;
    //vehicleAudio.VehicleWasDestroyed();
    //vehicleAudio.enabled = false;
  }

  //public void destroyVehicle(Collider aCollider){
  //  Debug.Log("Server received destroy vehicle command");
  //}

  private void explode(Collider aCollider){
    //foreach (VehicleComponent component in vehicleComponents)
    //  component.DetachWithForce(rigidbody.velocity);
    //generateExplosiveForceAtPosition(aCollider.transform.position); // FIXME move to coroutine and delay
    //vehicleController.readUserInput = false;
  }

  private void generateExplosiveForceAtPosition(Vector3 thePosition){
    float power = 1000f;
    float radius = 10f;
    float upwardsModifier = 1f;
    Collider[] colliders = Physics.OverlapSphere(thePosition, radius);
    foreach (Collider hit in colliders){
      if (hit && hit.rigidbody)
        hit.rigidbody.AddExplosionForce(power, thePosition, radius, upwardsModifier, ForceMode.Impulse);
    }
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
    return (aCollider.gameObject.tag == "Nugget");
    //return (aCollider.gameObject.tag == "Nugget" && impactForce >= 23f &&
    //    rigidbody.velocity.magnitude <= impactForce && impactIsCloseEnoughToImpactPoint(aCollider, impactForce + 10f));
  }

  private bool shouldBeDestroyedByProjectileCollisionWith(Collider aCollider, float impactForce){
    return (aCollider.gameObject.tag == "Rock");
    //return (aCollider.gameObject.tag == "Rock" && impactForce >= 13f &&
    //    !slingshot.LaunchedThisProjectile(aCollider.gameObject) && impactIsCloseEnoughToImpactPoint(aCollider, impactForce));
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
