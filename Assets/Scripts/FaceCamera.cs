// Useful for Text Meshes that should face the camera.

using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    // LateUpdate so that all camera updates are finished.
    private void LateUpdate()
    {
        if (Camera.main != null) transform.forward = Camera.main.transform.forward;
    }
}