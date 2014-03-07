using UnityEngine;
using System.Collections;
using uLink;

public class ProjectileCreator : uLink.MonoBehaviour {

  private uLinkSmoothRigidbodyImproved smoothRigidbody;
  private int ownerViewId;
  private float torqueMultiplier = 150000f;
  private Vector3 releasePosition;
  private Vector3 maxScale;

  void Awake(){
    smoothRigidbody = GetComponent<uLinkSmoothRigidbodyImproved>();
    Disable();
  }

  void Start(){
    maxScale = new Vector3(.3f,.3f,.3f);
  }

  void FixedUpdate(){
  //void LateUpdate(){
    if (transform.position.y > (releasePosition.y + 25f))
      transform.localScale = Vector3.Lerp(transform.localScale, maxScale, .5f * Time.deltaTime);
  }

  public void Loosen(){
    Release();
    enableCollider();
    rigidbody.mass = 20;
  }

  public void Fire(Vector3 launchForce, Vector3 relativeForce){
    Release();
    rigidbody.AddForce(relativeForce);
    rigidbody.AddForce(launchForce.x, launchForce.y, launchForce.z, ForceMode.Impulse);
    rigidbody.AddTorque(transform.right * torqueMultiplier);
    Arm();
  }

  public void Release(){
    releasePosition = transform.position;
    transform.parent = null;
    smoothRigidbody.enabled = true;
    rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    rigidbody.constraints = RigidbodyConstraints.None;
    rigidbody.isKinematic = false;
  }

  void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info){
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
}
