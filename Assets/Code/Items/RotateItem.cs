using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateItem : MonoBehaviour
{
  public float RotationSpeed = 10f;

  void Update()
  {
    transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
  }
}