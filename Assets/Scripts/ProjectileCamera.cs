using UnityEngine;
using System.Collections;

public class ProjectileCamera : MonoBehaviour {
  void Awake(){
  }

  void Start () {
  }

  void Update () {
    //if (active){
    //  if (launchedProjectile != null){
    //    Vector3 targetPosition = launchedProjectile.transform.position;
    //    Vector3 lineOfSight = launchedProjectile.transform.position - lastLaunchPosition;
    //    targetPosition -= lineOfSight.normalized * 15f;
    //    targetPosition.y += 20f;
    //    projectileCamera.transform.position = Vector3.Lerp(projectileCamera.transform.position, targetPosition, Time.deltaTime);
    //    Vector3 lookDirection = lastLaunchDirection + (2f * Vector3.down) + (launchedProjectile.transform.position - projectileCamera.transform.position);
    //    projectileCamera.transform.rotation = Quaternion.Slerp(projectileCamera.transform.rotation, Quaternion.LookRotation(lookDirection.normalized), 50f * Time.deltaTime);
    //  } else {
    //    projectileCameraMode = false;
    //  }
    //} else if (armedProjectile != null) {
    //  Vector3 targetPosition = armedProjectile.transform.position;
    //  targetPosition += armedProjectile.transform.forward * 5f;
    //  projectileCamera.transform.position = targetPosition;
    //  projectileCamera.transform.LookAt(armedProjectile.transform.position);
    //}
  }

  public void Deactivate(){
    gameObject.SetActive(false);
  }
}
