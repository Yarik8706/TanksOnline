// Useful for Text Meshes that should face the camera.

using UnityEngine;

namespace Gameplay
{
    public class FaceCamera : MonoBehaviour
    {
        // LateUpdate so that all camera updates are finished.
        private void LateUpdate()
        {
            if(Camera.current != null) transform.forward = Camera.current.transform.forward;
        }
    }
}