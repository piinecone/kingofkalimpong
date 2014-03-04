using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Slingshot : MonoBehaviour {

  [SerializeField]
  private Projectile projectilePrefab;

  private PlayerVehicle vehicle;
  private float relativeVelocityMultiplier = 100000f;
  private float launchForce = 0f;
  private float minimumLaunchForce = 30000f;
  private float maximumLaunchForce;
  private List<GameObject> launchedProjectiles = new List<GameObject>();
  private GameObject launchedProjectile;
  private Projectile projectile;
  private Vector3 lastLaunchPosition;
  private Vector3 lastLaunchDirection;
  private bool deactivated = false;

  private bool isNonPlayerCharacter = false;

  void Awake(){
    vehicle = transform.parent.GetComponent<PlayerVehicle>();
    reload();
    maximumLaunchForce = minimumLaunchForce * 2f;
  }

  void Start () {
    isNonPlayerCharacter = vehicle.NonPlayerCharacter();
  }

  void Update () {
    if (isNonPlayerCharacter || deactivated) return;

    // if (networkView.isMine){
    aim();
    chargeProjectile();
    // }
  }

  private void aim(){
    if (!Camera.main.active) return;

    // capture mouse input
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
    //if (projectile != null) projectile.transform.rotation = transform.rotation;

    // update the reticle
    // updateReticlePosition();
  }

  private void chargeProjectile(){
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
      recordLaunchPositionAndDirection();
      fireProjectile();
    }
  }

  private void recordLaunchPositionAndDirection(){
    lastLaunchPosition = transform.position;
    lastLaunchDirection = transform.forward;
  }

  private void fireProjectile(){
    Vector3 launchForceVector = launchVector();
    Vector3 relativeForceVector = determineRelativeForceVector();
    projectile.Fire(launchForce: launchForceVector, relativeForce: relativeForceVector);
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
    projectile.Arm();
    projectile = null;
    Invoke("reload", 1f);
  }

  private void destroyLaunchedProjectile(){
    Network.Destroy(launchedProjectile);
  }

  // FIXME merge all this when the time comes
  private void reload(){
    if (deactivated) return;

    projectile = Instantiate(projectilePrefab, transform.position, transform.rotation) as Projectile;
    projectile.transform.parent = transform;
    projectile.Disable();
  }

  // uLink.Network.Instantiate(projectile, slingshot.Position(), slingshot.Rotation(), 0);
  //private void createProjectile(){
  //  //armedProjectile = Instantiate(projectile, origin, Quaternion.identity) as GameObject;
  //  armedProjectile = Network.Instantiate(projectile, slingshot.transform.position, slingshot.transform.rotation, 0) as GameObject;
  //  Debug.Log("Instantiated projectile: " + armedProjectile);
  //  armedProjectile.transform.parent = transform;
  //  disableProjectile();
  //  //networkView.RPC("SpawnProjectile", RPCMode.Server, slingshot.transform.position);
  //}

  public void Deactivate(){
    deactivated = true;
    if (projectile) projectile.Loosen();
  }

  public bool LaunchedThisProjectile(GameObject projectileGameObject){
    return launchedProjectiles.Contains(projectileGameObject);
  }

  void OnTriggerEnter(Collider aCollider){
    vehicle.OnTriggerEnter(aCollider);
  }
}
