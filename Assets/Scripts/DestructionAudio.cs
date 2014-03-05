using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestructionAudio : MonoBehaviour {

  [SerializeField]
  private List<AudioSource> sounds = new List<AudioSource>();
  [SerializeField]
  private AudioSource carHorn;

  void Start () {
  }

  void Update () {
  }

  public void Play(){
    foreach(AudioSource sound in sounds) sound.Play();
    Invoke("playRandomSounds", Random.Range(.03f, .1f));
    Invoke("playRandomSounds", Random.Range(.05f, .07f));
    Invoke("playRandomSounds", Random.Range(.02f, .12f));
    Invoke("playCarHorn", .02f);
  }

  private void playRandomSounds(){
    sounds[Random.Range(0,sounds.Count - 1)].Play();
  }

  private void playCarHorn(){
    carHorn.Play();
  }
}
