using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Camera
{
   public class CameraMouseHelper : MonoBehaviour
   {
      [SerializeField] private bool lockMouse = true;

      private void Awake()
      {
         if(!lockMouse)
            return;
            
         Cursor.visible = false;
         Cursor.lockState = CursorLockMode.Locked;
      }
   }
}