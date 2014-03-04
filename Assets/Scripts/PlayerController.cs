using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

  [SerializeField]
  private bool nonPlayerCharacter = false;

  void Awake(){
    if (nonPlayerCharacter) GetComponentInChildren<CarControl>().readUserInput = false;
  }

  void Start () {
  }

  void Update () {
  }
}
