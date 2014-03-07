using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerVehicleProxy : uLink.MonoBehaviour {
  [SerializeField]
  private GameObject vehicleBody;

  private CarControl vehicleController;
  private CarSound vehicleAudio;
  private CarSettings vehicleSettings;
  private List<VehicleComponent> vehicleComponents = new List<VehicleComponent>();
  private SlingshotProxy slingshot;
  private bool destroyed = false;
  private DestructionAudio destructionAudio;

  void Awake(){
    getVehicleComponents();
    getVehicleBodyComponents();
    getSlingshot();
  }

  private void getVehicleComponents(){
    vehicleController = GetComponent<CarControl>();
    vehicleController.readUserInput = true;
    vehicleAudio = GetComponent<CarSound>();
    vehicleSettings = GetComponent<CarSettings>();
  }

  private void getVehicleBodyComponents(){
    VehicleComponent[] components = GetComponentsInChildren<VehicleComponent>(true);
    foreach(VehicleComponent component in components)
      vehicleComponents.Add(component);
  }

  private void getSlingshot(){
    slingshot = GetComponentInChildren<SlingshotProxy>();
  }

  [RPC]
  public void AimSlingshot(Quaternion rotation){
    if (!destroyed) slingshot.Aim(rotation);
  }

  [RPC]
  public void DestroyVehicle(Vector3 impactPosition){
    Debug.Log("Proxy received destroy vehicle command via RPC");
    collider.enabled = false;
    vehicleBody.collider.enabled = false;
    //playDestructionSounds();
    explode(impactPosition);
    slingshot.Deactivate();
    vehicleAudio.VehicleWasDestroyed();
    vehicleAudio.enabled = false;
    destroyed = true;
  }

  private void explode(Vector3 impactPosition){
    foreach (VehicleComponent component in vehicleComponents)
      component.DetachWithForce(rigidbody.velocity);
    generateExplosiveForceAtPosition(impactPosition);
    vehicleController.readUserInput = false;
  }

  private void generateExplosiveForceAtPosition(Vector3 thePosition){
    float power = 1000f;
    float radius = 10f;
    float upwardsModifier = 1f;
    Collider[] colliders = Physics.OverlapSphere(thePosition, radius);
    foreach (Collider hit in colliders){
      if (hit && hit.rigidbody)
        hit.rigidbody.AddExplosionForce(power, thePosition, radius, upwardsModifier, ForceMode.Impulse);
    }
  }
}
