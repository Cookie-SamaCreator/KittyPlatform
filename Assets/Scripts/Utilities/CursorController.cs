using UnityEngine;

public sealed class CursorController : MonoBehaviour
{
    private void Start()
    {
        LockCursor();
    }

    private void OnDisable()
    {
        UnlockCursor();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && isActiveAndEnabled)
        {
            LockCursor();
        }
        else
        {
            UnlockCursor();
        }
    }

    private static void LockCursor()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Cursor.visible)
        {
            Cursor.visible = false;
        }
    }

    private static void UnlockCursor()
    {
        // Release the cursor when the game is not actively controlling it.
        if (Cursor.lockState != CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (!Cursor.visible)
        {
            Cursor.visible = true;
        }
    }
}