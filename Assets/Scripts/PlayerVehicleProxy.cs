using UnityEngine;
using System.Collections;

public class PlayerVehicleProxy : uLink.MonoBehaviour {

  // Use this for initialization
  void Start () {
  
  }
  
  // Update is called once per frame
  void Update () {
  

  }

  [RPC]
  public void DestroyVehicle(Vector3 impactPosition){
    Debug.Log("Proxy received destroy vehicle command via RPC");
    // smash into little bits here
  }
}
