using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



public class ImageLoader
{
    public static List<Sprite> LoadImages(string spriteFolder)
    {
        //Create an array of file paths from which to choose
        string folderPath = Application.streamingAssetsPath +"/"+ spriteFolder;  //Get path of folder

        if (Directory.Exists(folderPath) == false)
        {
            folderPath = Application.streamingAssetsPath + "/NoImage";
        }
        
        string[] filePaths = Directory.GetFiles(folderPath, "*.png"); // Get all files of type .png in this folder
        List<Sprite> spritesLoaded = new List<Sprite>();
        for (int i = 0; i < filePaths.Length; i++)
        {
            byte[] pngBytes = File.ReadAllBytes(filePaths[i]);

            //Creates texture and loads byte array data to create image
            Texture2D newTexture = new Texture2D(2, 2);
            newTexture.LoadImage(pngBytes);

            //Creates a new Sprite based on the Texture2D
            Sprite newSprite = Sprite.Create(newTexture, new Rect(0.0f, 0.0f, newTexture.width, newTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            newSprite.name = i.ToString();
            spritesLoaded.Add(newSprite);
        }

        return spritesLoaded;
        
    }
   

}