using UnityEngine;
using System.Collections;
using uLink;

public class ProjectileCreator : uLink.MonoBehaviour {

  private uLinkSmoothRigidbodyImproved smoothRigidbody;
  private int ownerViewId;
  private float torqueMultiplier = 150000f;

  void Awake(){
    smoothRigidbody = GetComponent<uLinkSmoothRigidbodyImproved>();
    Debug.Log(smoothRigidbody);
    Disable();
  }

  void Start () {
  
  }
  
  void Update () {
  
  }

  public void Fire(Vector3 launchForce, Vector3 relativeForce){
    Debug.Log("Server: releasing physics lock on projectile");
    Release();
    Debug.Log("Server: firing projectile with " + launchForce + " and " + relativeForce);
    rigidbody.AddForce(relativeForce);
    rigidbody.AddForce(launchForce.x, launchForce.y, launchForce.z, ForceMode.Impulse);
    rigidbody.AddTorque(transform.right * torqueMultiplier);
    Arm();
  }

  public void Release(){
    transform.parent = null;
    smoothRigidbody.enabled = true;
    rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    rigidbody.constraints = RigidbodyConstraints.None;
    rigidbody.isKinematic = false;
  }

  void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info){
    Debug.Log("Server: projectile creator instantiated; network view is " + networkView);
  }

  public uLink.NetworkView GetNetworkView(){
    return networkView;
  }

  public void Arm(){
    Invoke("enableCollider", .25f);
  }

  private void enableCollider(){
    collider.enabled = true;
  }

  public void Disable(){
    rigidbody.interpolation = RigidbodyInterpolation.None;
    rigidbody.isKinematic = true;
    collider.enabled = false;
    smoothRigidbody.enabled = false;
  }

  public void Loosen(){
  }
}
