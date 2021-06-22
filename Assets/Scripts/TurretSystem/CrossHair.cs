using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
   [SerializeField]private float size;
   [SerializeField]private Texture2D image;
   private void OnGUI()
   {
      Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

      screenPos.y = Screen.height - screenPos.y;
      
      RaycastHit hit; // declare the RaycastHit variable
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
   
      if (Physics.Raycast(ray, out hit)) {
         transform.LookAt(hit.transform);
      }
      GUI.DrawTexture(new Rect(screenPos.x + 10, screenPos.y,size,size), image);
   }
}
