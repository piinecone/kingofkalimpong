using UnityEngine;
using System.Collections;

public class ProjectileOwner : uLink.MonoBehaviour {

  private SlingshotOwner slingshot;
  private uLinkSmoothRigidbodyImproved smoothRigidbody;

  void Awake(){
    Debug.Log("Projectile owner is awake");
    smoothRigidbody = GetComponent<uLinkSmoothRigidbodyImproved>();
    disablePhysics();
  }

  void Start () {
  
  }
  
  void Update () {
  
  }

  void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info){
    int ownerViewId = info.networkView.initialData.Read<int>();
    uLink.NetworkView slingshotNetworkView = uLink.NetworkView.Find(new uLink.NetworkViewID(ownerViewId));
    slingshot = slingshotNetworkView.GetComponentInChildren<SlingshotOwner>();
    transform.parent = slingshot.transform;
  }

  private void disablePhysics(){
    rigidbody.interpolation = RigidbodyInterpolation.None;
    rigidbody.isKinematic = true;
    collider.enabled = false;
    smoothRigidbody.enabled = false;
  }
}
