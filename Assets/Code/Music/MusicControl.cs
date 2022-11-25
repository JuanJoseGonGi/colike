using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour
{
  public AudioClip[] music;
  private AudioSource audioSource;

  // Start is called before the first frame update
  void Start()
  {
    audioSource = GetComponent<AudioSource>();
    PlayRandomMusic();
  }

  // Update is called once per frame
  void Update()
  {
    if (!audioSource.isPlaying)
    {
      PlayRandomMusic();
    }
  }

  void PlayRandomMusic()
  {
    int index = Random.Range(0, music.Length);
    audioSource.clip = music[index];
    audioSource.Play();
  }
}
