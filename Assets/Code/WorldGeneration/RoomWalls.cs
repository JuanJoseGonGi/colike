using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomWalls : MonoBehaviour
{
  private GameObject leftWall;
  private GameObject rightWall;
  private GameObject topWall;
  private GameObject bottomWall;

  const int wallHeight = 2;
  public const int WallWidth = 1;

  // Start is called before the first frame update
  void Start()
  {
    initWalls();
  }

  // Update is called once per frame
  void Update()
  {

  }

  private void initWalls()
  {
    if (leftWall == null)
    {
      leftWall = transform.GetChild(0).gameObject;
    }

    if (rightWall == null)
    {
      rightWall = transform.GetChild(1).gameObject;
    }

    if (topWall == null)
    {
      topWall = transform.GetChild(2).gameObject;
    }

    if (bottomWall == null)
    {
      bottomWall = transform.GetChild(3).gameObject;
    }
  }

  public void SetSize(int width, int height)
  {
    initWalls();

    print("Setting size to " + width + ", " + height);

    leftWall.transform.localScale = new Vector3(WallWidth, wallHeight, height);
    leftWall.transform.localPosition = new Vector3(-WallWidth / 2f, wallHeight / 2f, height / 2f);

    rightWall.transform.localScale = new Vector3(WallWidth, wallHeight, height);
    rightWall.transform.localPosition = new Vector3(width + WallWidth / 2f, wallHeight / 2f, height / 2f);

    topWall.transform.localScale = new Vector3(width + 2 * WallWidth, wallHeight, WallWidth);
    topWall.transform.localPosition = new Vector3(width / 2f, wallHeight / 2f, height + WallWidth / 2f);

    bottomWall.transform.localScale = new Vector3(width + 2 * WallWidth, wallHeight, WallWidth);
    bottomWall.transform.localPosition = new Vector3(width / 2f, wallHeight / 2f, -WallWidth / 2f);
  }
}
