using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlingshotCreator : uLink.MonoBehaviour {

  [SerializeField]
  private Projectile projectilePrefab;

  private PlayerVehicleCreator vehicle;
  private Vector3 mousePosition = Vector3.zero;
  private float relativeVelocityMultiplier = 100000f;
  private float launchForce = 0f;
  private float minimumLaunchForce = 30000f;
  private float maximumLaunchForce;
  private List<GameObject> launchedProjectiles = new List<GameObject>();
  private GameObject launchedProjectile;
  private ProjectileCreator projectile;
  private Vector3 lastLaunchPosition;
  private Vector3 lastLaunchDirection;
  private bool deactivated = false;
  private InputSender inputSender;

  void Awake(){
    vehicle = transform.parent.GetComponent<PlayerVehicleCreator>();
    maximumLaunchForce = minimumLaunchForce * 2f;
    inputSender = transform.parent.GetComponent<InputSender>();
  }

  //[RPC]
  //public void Aim(Quaternion targetRotation){
  //  //networkView.RPC("Aim", uLink.RPCMode.OthersExceptOwner, mousePosition);
  //}

  public void Aim(Quaternion rotation){
    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 25f * Time.deltaTime);
  }

  public void ChargeProjectile(bool fireButtonPressed){
    //if (Input.GetMouseButton(0)){
    if (fireButtonPressed){
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
  }

  private void destroyLaunchedProjectile(){
    Network.Destroy(launchedProjectile);
  }

  public ProjectileCreator Reload(uLink.NetworkPlayer player, int ownerViewId){
    projectile = uLink.Network.Instantiate(player, "Projectile@Proxy", "Projectile@Owner", "Projectile@Creator", transform.position, transform.rotation, 0, ownerViewId).GetComponent<ProjectileCreator>();
    projectile.transform.parent = transform;
    return projectile;
  }

  public void AddToTrackedProjectiles(ProjectileCreator projectile){
    launchedProjectiles.Add(projectile.gameObject);
  }

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
