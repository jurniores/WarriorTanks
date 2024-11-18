using Omni.Core;
using UnityEngine;

public class Spawn : ServiceBehaviour
{
    public int team = 0;
   private void OnTriggerEnter2D(Collider2D other) {
        print("Triguei no spawn");
   }
}
