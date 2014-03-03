using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CarControl))]
public class PlayerVehicle : MonoBehaviour {

  [SerializeField]
  private GameObject projectile;

  [SerializeField]
  private Transform impactPoint;

  private Camera mainCamera;
  private ProjectileCamera projectileCamera;
  private Slingshot slingshot;
  private List<VehicleComponent> vehicleComponents = new List<VehicleComponent>();
  private CarControl vehicleController;

  void Awake(){
    getVehicleController();
    getSlingshot();
    getVehicleComponents();
    setupCameras();
  }

  void Start(){
  }

  void Update(){
  }

  private void getVehicleController(){
    vehicleController = GetComponent<CarControl>();
  }

  private void getSlingshot(){
    slingshot = GetComponentInChildren<Slingshot>();
  }

  private void getVehicleComponents(){
    VehicleComponent[] components = GetComponentsInChildren<VehicleComponent>(true);
    foreach(VehicleComponent component in components)
      vehicleComponents.Add(component);
  }

  private void setupCameras(){
    mainCamera = GameObject.FindWithTag("MainCamera").camera;
    projectileCamera = GetComponentInChildren<ProjectileCamera>() as ProjectileCamera;
    projectileCamera.Deactivate();
    //if (networkView.isMine){
    mainCamera.GetComponent<CamSmoothFollow>().target = vehicleController.CenterOfMass;
    //}
  }

  public Vector3 Velocity(){
    return rigidbody.velocity;
  }
}
