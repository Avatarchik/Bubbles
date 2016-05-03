using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraWorkspace : MonoBehaviour
{
    public Vector2 workspaceSize = new Vector2(600, 1024);

    public Vector2 WorkspaceSize
    {
        get { return workspaceSize; }
        set
        {
            workspaceSize = value;
            UpdateCamera();
        }
    }

    private Camera cameraMain;

    private Camera CameraMain { get { return cameraMain ?? (cameraMain = GetComponent<Camera>()); } }

    void Awake()
    {
        UpdateCamera();
    }

    public void UpdateCamera()
    {
        float newCamSize;

        var invertedAspect = (float)Screen.height / Screen.width;
        var c1 = workspaceSize.y * 0.5f;
        var c2 = workspaceSize.x * 0.5f * invertedAspect;

        if (c1 >= c2)
        {
            float predW = Mathf.Floor(c1 * 2f * (1f / invertedAspect));
            newCamSize = predW * invertedAspect / 2f;
        }
        else
        {
            float predH = Mathf.Floor(c2 * 2f);
            newCamSize = predH / 2f;
        }

        CameraMain.orthographicSize = Mathf.Floor(newCamSize);
    }

#if UNITY_EDITOR
    private void Update()
    {
        UpdateCamera();
    }
#endif

}
