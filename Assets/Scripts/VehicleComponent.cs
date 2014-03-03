using UnityEngine;
using System.Collections;

public class VehicleComponent : MonoBehaviour {

  private Vector3 detachmentForce;

  void Awake(){
    disableRigidbody();
  }

  private void disableRigidbody(){
    rigidbody.isKinematic = true;
    collider.enabled = false;
  }

  private void orphan(){
    transform.parent = null;
    rigidbody.AddForce(detachmentForce);
  }

  public void DetachWithForce(Vector3 force){
    detachmentForce = force * 4000f;
    enableRigidbody();
    Invoke("orphan", .01f);
    // FIXME network destroy
    //Destroy(gameObject, Random.Range(6f, 12f));
  }

  public void enableRigidbody(){
    collider.enabled = true;
    rigidbody.isKinematic = false;
  }
}
