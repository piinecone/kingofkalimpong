using UnityEngine;
using System.Collections;
using uLink;

public class ProjectileCreator : uLink.MonoBehaviour {

  private uLinkSmoothRigidbodyImproved smoothRigidbody;

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
    Debug.Log("Server: fire projectile");
  }

  void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info){
    Debug.Log("Server: projectile creator instantiated; network view is " + networkView);
  }

  public uLink.NetworkView GetNetworkView(){
    return networkView;
  }

  public void Arm(){
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
