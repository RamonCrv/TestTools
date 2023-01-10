using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenShotController : MonoBehaviour
{
    public void TakeScreenShot()
    {
        ScreenShot.TakeScreenShot("BackgroundImages", "backgroundImage");
    }
}
