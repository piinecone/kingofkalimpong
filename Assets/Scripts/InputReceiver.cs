using UnityEngine;
using System.Collections;
using uLink;

public class InputReceiver : uLink.MonoBehaviour {

  private CarControl carControl;
  private Slingshot slingshot;

  void Awake(){
    carControl = GetComponent<CarControl>();
    slingshot = GetComponentInChildren<Slingshot>();
  }

  [RPC]
  void SetMousePosition(Vector3 mousePosition){
    slingshot.Aim(mousePosition);
  }

  [RPC]
  void SetSteerInput(float steerInput){
    carControl.steerInput = steerInput;
  }

  [RPC]
  void SetMotorInput(float motorInput){
    carControl.motorInput = motorInput;
  }

  [RPC]
  void SetBrakeInput(float brakeInput){
    carControl.brakeInput = brakeInput;
  }

  [RPC]
  void SetHandbrakeInput(float handbrakeInput){
    carControl.handbrakeInput = handbrakeInput;
  }

  [RPC]
  void SetGearInput(int gearInput){
    carControl.gearInput = gearInput;
  }

  [RPC]
  void AddForce(Vector3 force, ForceMode mode){
    this.rigidbody.AddForce(force, mode);
  }

  [RPC]
  void AddTorque(Vector3 torque, ForceMode mode){
    this.rigidbody.AddTorque(torque, mode);
  }

  [RPC]
  void AddForceAtPosition(Vector3 force, Vector3 position, ForceMode mode){
    this.rigidbody.AddForceAtPosition(force, position, mode);
  }

  [RPC]
  void SetVelocity(Vector3 velocity){
    this.rigidbody.velocity = velocity;
  }

  [RPC]
  void SetAngularVelocity(Vector3 angularVelocity){
    this.rigidbody.angularVelocity = angularVelocity;
  }

  [RPC]
  void SetRotation(Quaternion rotation){
    this.transform.rotation = rotation;
  }
}
