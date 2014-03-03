using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

  private float torqueMultiplier = 150000f;

  void Awake(){
  }

  void Start () {
  }

  void Update () {
  }

  void OnNetworkInstantiate(NetworkMessageInfo info){
  }

  public void Activate(){ // used to be enableProjectile
    Release();
    enableCollider();
  }

  public void Disable(){
    rigidbody.isKinematic = true;
    collider.enabled = false;
  }

  public void Release(){
    transform.parent = null;
    rigidbody.constraints = RigidbodyConstraints.None;
    rigidbody.isKinematic = false;
    rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
  }

  public void Fire(Vector3 launchForce, Vector3 relativeForce){
    Release();
    rigidbody.AddForce(relativeForce);
    rigidbody.AddForce(launchForce.x, launchForce.y, launchForce.z, ForceMode.Impulse);
    rigidbody.AddTorque(transform.right * torqueMultiplier);
  }

  public void Arm(){
    Invoke("enableCollider", .25f);
    // destroy launched projectile, 15f
  }

  private void enableCollider(){
    collider.enabled = true;
  }

  private void scale(){
    //Vector3 targetScale = new Vector3(.8f,.8f,.8f);
    //foreach(GameObject launchedProjectile in launchedProjectiles){
    //  if (launchedProjectile != null && launchedProjectile.transform.position.y > (transform.position.y + 15f))
    //    launchedProjectile.transform.localScale = Vector3.Lerp(launchedProjectile.transform.localScale, targetScale, .5f * Time.deltaTime);
    //}
  }
}
