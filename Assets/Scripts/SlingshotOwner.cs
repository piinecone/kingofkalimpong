using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlingshotOwner : uLink.MonoBehaviour {

  [SerializeField]
  private Projectile projectilePrefab;

  private PlayerVehicleOwner vehicle;
  private Vector3 mousePosition = Vector3.zero;
  private float relativeVelocityMultiplier = 100000f;
  private float launchForce = 0f;
  private float minimumLaunchForce = 40000f;
  private float maximumLaunchForce;
  private List<GameObject> launchedProjectiles = new List<GameObject>();
  private GameObject launchedProjectile;
  private ProjectileOwner projectile;
  private Vector3 lastLaunchPosition;
  private Vector3 lastLaunchDirection;
  private bool deactivated = false;
  private bool isNonPlayerCharacter = false;
  //public ProjectileCamera projectileCamera;
  private InputSender inputSender;
  private uLink.NetworkView networkView;
  private uLink.NetworkPlayer player;

  void Awake(){
    vehicle = transform.parent.GetComponent<PlayerVehicleOwner>();
    networkView = vehicle.GetNetworkView();
    maximumLaunchForce = minimumLaunchForce * 2f;
    inputSender = transform.parent.GetComponent<InputSender>();
    //projectileCamera.gameObject.SetActive(false);
  }

  void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info){
    vehicle = transform.parent.GetComponent<PlayerVehicleOwner>();
    networkView = vehicle.GetNetworkView();
    player = uLink.Network.player;
    reload();
  }

  public void SetProjectile(ProjectileOwner projectile){
    this.projectile = projectile;
  }

  void Update () {
    if (deactivated) return;

    aim();
    chargeProjectile();
    //toggleProjectileCamera();
  }

  //private void toggleProjectileCamera(){
  //  if (Input.GetKeyDown(KeyCode.C))
  //    projectileCamera.Toggle();
  //}

  private void aim(){
    if (Camera.main == null || !Camera.main.active) return;

    // wrangle mouse input
    Vector3 mousePosition = Input.mousePosition;
    Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition);
    Vector3 lookDirection = mouseRay.direction;

    // make aiming skyward much more sensitive
    if (lookDirection.y > 0f) lookDirection.y *= 15f;

    // take the vector between the car's forward and upward position
    Vector3 carDirection = (vehicle.transform.forward + vehicle.transform.up).normalized;

    // adjust the y component of what will be the slingshot's aiming direction
    float maxVerticalAdjustment = .5f;
    float verticalAdjusment = launchForce > (minimumLaunchForce * 1.1f) ? maxVerticalAdjustment - (launchForce / maximumLaunchForce) : maxVerticalAdjustment;
    lookDirection.y = Mathf.Max((carDirection.y * .1f) + verticalAdjusment, lookDirection.y); // FIXME maybe increase the carDirection.y quantity

    // perform the rotation relative to the vehicle
    lookDirection += carDirection;

    // complete the rotation
    Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 25f * Time.deltaTime);
    vehicle.AimSlingshot(lookRotation);
    //networkView.RPC("Aim", uLink.RPCMode.Server, lookRotation);
  }

  public void chargeProjectile(){
    if (Input.GetMouseButton(0)){
      launchForce = Mathf.Max(launchForce + minimumLaunchForce/75f, minimumLaunchForce);
    } else {
      if (launchForce >= minimumLaunchForce) launchProjectile();
      launchForce = 0f;
    }
  }

  private Vector3 launchVector(){
    float forceMultiplier = Mathf.Clamp(launchForce, minimumLaunchForce, maximumLaunchForce);
    Vector3 direction = transform.forward.normalized;
    return direction * forceMultiplier;
  }

  void launchProjectile(){
    if (projectile != null){
      fireProjectile();
      recordLaunchPositionAndDirection();
    }
  }

  private void recordLaunchPositionAndDirection(){
    lastLaunchPosition = transform.position;
    lastLaunchDirection = transform.forward;
    //projectileCamera.SetLaunchedProperties(projectile: launchedProjectile, position: lastLaunchPosition, direction: lastLaunchDirection);
  }

  private void fireProjectile(){
    Vector3 launchForceVector = launchVector();
    Vector3 relativeForceVector = determineRelativeForceVector();
    networkView.RPC("LaunchProjectile", uLink.RPCMode.Server, launchForceVector, relativeForceVector, networkView.viewID.id);
    projectile.Release(); // should this be delayed 200ms?
    prepareNextProjectile();
  }

  private Vector3 determineRelativeForceVector(){
    if (vehicle.Velocity().magnitude >= 1f)
      return vehicle.Velocity() * relativeVelocityMultiplier;
    return Vector3.zero;
  }

  private void prepareNextProjectile(){
    launchedProjectile = projectile.gameObject;
    launchedProjectiles.Add(launchedProjectile);
    //projectile.Arm();
    projectile = null;
    Invoke("reload", 1f);
  }

  private void destroyLaunchedProjectile(){
    Network.Destroy(launchedProjectile);
  }

  private void reload(){
    if (deactivated) return;
    networkView.RPC("ReloadProjectile", uLink.RPCMode.Server, player, networkView.viewID.id);
  }

  // FIXME merge all this when the time comes
  //  if (deactivated) return;
  //  projectile = Instantiate(projectilePrefab, transform.position, transform.rotation) as Projectile;
  //  projectile.transform.parent = transform;
  //  projectile.Disable();
  //}

  public void Deactivate(){
    deactivated = true;
    if (projectile) projectile.Loosen();
  }

  public bool LaunchedThisProjectile(GameObject projectileGameObject){
    return launchedProjectiles.Contains(projectileGameObject);
  }

  //void OnTriggerEnter(Collider aCollider){
  //  vehicle.OnTriggerEnter(aCollider);
  //}
}
