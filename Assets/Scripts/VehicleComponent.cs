using UnityEngine;
using System.Collections;

public class VehicleComponent : MonoBehaviour {

  private Vector3 detachmentForce;
  private Transform parentBeforeDestroyed;
  private Vector3 positionBeforeDestroyed;
  private Quaternion rotationBeforeDestroyed;
  //private bool rejoiningParent = false;

  void Awake(){
    disableRigidbody();
  }

  void Update(){
    //if (rejoiningParent){
    //  transform.position = Vector3.Lerp(transform.position, positionBeforeDestroyed, 5f * Time.deltaTime);
    //  transform.rotation = Quaternion.Slerp(transform.rotation, rotationBeforeDestroyed, 5f * Time.deltaTime);
    //}

    //if (Mathf.Approximately(Vector3.Distance(transform.position, positionBeforeDestroyed), 0f) &&
    //      Mathf.Approximately(Quaternion.Angle(transform.rotation, rotationBeforeDestroyed), 0f))
    //  rejoiningParent = false;
  }

  void Start(){
    //recordPositionAndRotation();
  }

  //private void recordPositionAndRotation(){
  //  parentBeforeDestroyed = transform.parent;
  //  positionBeforeDestroyed = transform.parent.position - transform.position;
  //  rotationBeforeDestroyed = transform.localRotation;
  //}

  private void orphan(){
    transform.parent = null;
    rigidbody.AddForce(detachmentForce);
  }

  public void DetachWithForce(Vector3 force){
    detachmentForce = force * 4000f;
    enableRigidbody();
    orphan();
    //Invoke("orphan", .01f);
    // FIXME network destroy
    //Destroy(gameObject, Random.Range(6f, 12f));
  }

  public void enableRigidbody(){
    collider.enabled = true;
    rigidbody.isKinematic = false;
  }

  private void disableRigidbody(){
    rigidbody.isKinematic = true;
    collider.enabled = false;
  }

  //public void RejoinParent(){
  //  disableRigidbody();
  //  rejoiningParent = true;
  //  transform.parent = parentBeforeDestroyed;
  //  transform.position = transform.parent.position - positionBeforeDestroyed;
  //  transform.rotation = transform.parent.rotation * rotationBeforeDestroyed;
  //}
}
