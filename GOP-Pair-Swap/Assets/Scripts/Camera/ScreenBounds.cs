using UnityEngine;

public class ScreenBounds : MonoBehaviour
{
    public static ScreenBounds Instance { get; private set; }

    public float Left { get; private set; }
    public float Right { get; private set; }
    public float Top { get; private set; }
    public float Bottom { get; private set; }
    public float Width => Right - Left;
    public float Height => Top - Bottom;

    private readonly float zDepth = 0f; // Z position where gameplay happens (probably 0)

    // Set camera in the inspector to prevent the performance inpact of using Camera.main
    [SerializeField] private Camera cam;

    void Awake()
    {
        // Singleton pattern to ensure only one instance of ScreenBounds exists
        // Set Instance to this if it's null, otherwise destroy this object
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CalculateBounds();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void CalculateBounds()
    {
        float distance = Mathf.Abs(zDepth - cam.transform.position.z);

        Vector3 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, distance));
        Vector3 topRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, distance));

        Left = bottomLeft.x;
        Right = topRight.x;
        Bottom = bottomLeft.y;
        Top = topRight.y;
    }
}