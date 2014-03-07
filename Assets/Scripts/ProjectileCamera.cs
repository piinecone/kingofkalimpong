using UnityEngine;
using System.Collections;

public class ProjectileCamera : MonoBehaviour {

  private Camera mainCamera;
  private SlingshotOwner slingshot;
  public GameObject projectile;
  public Vector3 launchPosition;
  public Vector3 launchDirection;

  void LateUpdate () {
    trackProjectile();
  }

  public void Initialize(Camera camera, SlingshotOwner slingshot){
    this.mainCamera = camera;
    this.slingshot = slingshot;
    deactivate();
  }

  private void trackProjectile(){
    if (this.active){
      if (projectile != null){
        Vector3 targetPosition = projectile.transform.position;
        Vector3 lineOfSight = projectile.transform.position - launchPosition;
        targetPosition -= lineOfSight.normalized * 15f;
        targetPosition.y += 20f;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);
        Vector3 lookDirection = launchDirection + (2f * Vector3.down) + (projectile.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection.normalized), 50f * Time.deltaTime);
      } else {
        reset();
      }
    } else {
      reset();
    }
  }

  public void Toggle(){
    bool switchToProjectileMode = false;
    if (projectile != null && projectile.rigidbody.velocity.magnitude >= 5f)
      switchToProjectileMode = !this.active;
    if (!switchToProjectileMode) reset();
    this.active = switchToProjectileMode;
    mainCamera.active = !switchToProjectileMode;
  }

  private void reset(){
    Vector3 targetPosition = slingshot.transform.position;
    targetPosition += slingshot.transform.forward * 5f;
    transform.position = targetPosition;
    transform.LookAt(slingshot.transform);
    this.active = false;
    mainCamera.active = true;
  }

  public void deactivate(){
    this.active = false;
    mainCamera.active = !active;
  }

  public void SetLaunchedProperties(GameObject projectile, Vector3 position, Vector3 direction){
    launchPosition = position;
    launchDirection = direction;
    this.projectile = projectile;
  }
}
