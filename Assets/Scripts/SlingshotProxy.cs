using UnityEngine;
using System.Collections;

public class SlingshotProxy : MonoBehaviour {

  private bool deactivated = false;
  private ProjectileProxy projectile;

  void Start () {
  
  }
  
  void Update () {
  
  }

  public void Deactivate(){
    deactivated = true;
    if (projectile) projectile.Loosen();
  }

  public void Aim(Quaternion rotation){
    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 25f * Time.deltaTime);
  }

  public void SetProjectile(ProjectileProxy projectile){
    this.projectile = projectile;
  }

  public void ReleaseProjectile(){
    projectile.Release();
  }
}
