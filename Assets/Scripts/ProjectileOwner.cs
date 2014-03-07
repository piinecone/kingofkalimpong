using UnityEngine;
using System.Collections;

public class ProjectileOwner : uLink.MonoBehaviour {

  private SlingshotOwner slingshot;
  private uLinkSmoothRigidbodyImproved smoothRigidbody;
  private float torqueMultiplier = 150000f;
  private Vector3 releasePosition;
  private Vector3 maxScale;

  void Start(){
    maxScale = new Vector3(.3f,.3f,.3f);
  }

  void FixedUpdate(){
  //void LateUpdate(){
    if (transform.position.y > (releasePosition.y + 25f))
      transform.localScale = Vector3.Lerp(transform.localScale, maxScale, .5f * Time.deltaTime);
  }

  public void Fire(Vector3 launchForce, Vector3 relativeForce){
    Release();
    rigidbody.AddForce(relativeForce);
    rigidbody.AddForce(launchForce.x, launchForce.y, launchForce.z, ForceMode.Impulse);
    rigidbody.AddTorque(transform.right * torqueMultiplier);
    Invoke("enableCollider", .25f);
  }

  public void Loosen(){
    Release();
    enableCollider();
    rigidbody.mass = 20;
  }

  public void Release(){
    transform.parent = null;
    smoothRigidbody.enabled = true;
    rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    rigidbody.constraints = RigidbodyConstraints.None;
    rigidbody.isKinematic = false;
    Invoke("enableCollider", .25f);
  }

  private void enableCollider(){
    collider.enabled = true;
  }

  void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info){
    int ownerViewId = info.networkView.initialData.Read<int>();
    uLink.NetworkView slingshotNetworkView = uLink.NetworkView.Find(new uLink.NetworkViewID(ownerViewId));
    slingshot = slingshotNetworkView.GetComponentInChildren<SlingshotOwner>();
    slingshot.SetProjectile(this);
    smoothRigidbody = GetComponent<uLinkSmoothRigidbodyImproved>();
    disablePhysics();
    transform.parent = slingshot.transform;
    transform.position = slingshot.transform.position;
  }

  private void disablePhysics(){
    rigidbody.interpolation = RigidbodyInterpolation.None;
    rigidbody.isKinematic = true;
    collider.enabled = false;
    smoothRigidbody.enabled = false;
  }
}
