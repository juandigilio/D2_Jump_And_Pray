using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private void Start()
    {
        LockCursor();
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
