using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCursor : MonoBehaviour
{
  public Camera MenuCamera;

  private Vector3 originalScale;

  // Start is called before the first frame update
  void Start()
  {
    originalScale = transform.localScale;
  }

  // Update is called once per frame
  void Update()
  {
    Vector3 mousePosition = Input.mousePosition;
    mousePosition = MenuCamera.ScreenToWorldPoint(new(mousePosition.x, mousePosition.y, MenuCamera.nearClipPlane));

    if (mousePosition.x < transform.position.x)
    {
      transform.localScale = new(-originalScale.x, originalScale.y, originalScale.z);
      return;
    }

    if (mousePosition.x > transform.position.x)
    {
      transform.localScale = originalScale;
      return;
    }
  }
}
