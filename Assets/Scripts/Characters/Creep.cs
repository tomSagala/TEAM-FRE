using UnityEngine;
using System.Collections;

public class Creep : GameSingleton<Creep> {
    public Texture2D creepTexture;
    public Material creepMaterial;

    public int resolution = 128;
    public GameObject quadForScale;

    private GameObject m_creepGameObject;
    private Texture2D m_creepTexture;
    private Texture2D m_internalCreepTexture;
    private float[,] m_internalArray;
    private Material m_internalCreepMaterial;
    private RenderTexture m_creepRenderTexture;

    private Material m_materialForRendering;

    void Awake()
    {
        // MeshRenderer meshRenderer = _creepGameObject.AddComponent<MeshRenderer>();
        MeshRenderer meshRenderer = quadForScale.GetComponent<MeshRenderer>();
        meshRenderer.enabled = true;
        m_materialForRendering = meshRenderer.material = creepMaterial;

        // Initialize the render texture
        m_creepRenderTexture = new RenderTexture(creepTexture.width, creepTexture.height, 1, RenderTextureFormat.ARGB32);
        m_creepRenderTexture.wrapMode = TextureWrapMode.Repeat;

        // Initialize the internal texture
        m_internalCreepTexture = new Texture2D(resolution, resolution, TextureFormat.Alpha8, false);
        Color32[] colors = new Color32[resolution * resolution];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i].a = 0;
        }

        m_internalCreepTexture.SetPixels32(colors);
        m_internalCreepTexture.Apply();

        m_internalCreepMaterial = new Material(Shader.Find("Hidden/CreepEffect"));

        m_internalArray = new float[resolution, resolution];
        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                m_internalArray[i, j] = 0.0f;
            }
        }
    }

    void Update()
    {
        m_internalCreepMaterial.SetTexture("_CreepTexture", creepTexture);
        Graphics.Blit(m_internalCreepTexture, m_creepRenderTexture, m_internalCreepMaterial);
        m_materialForRendering.mainTexture = m_creepRenderTexture;
    }

    public void AddCreepAtPosition(Vector3 position, int radius, float value)
    {
        float squaredRadius = radius * radius;

        // Transform world position into texture space
        int posX = (int)(((position.x - quadForScale.transform.lossyScale.x * 0.5f) / quadForScale.transform.lossyScale.x) * (float)(resolution + 1)) + resolution;
        int posY = (int)(((position.z - quadForScale.transform.lossyScale.y * 0.5f) / quadForScale.transform.lossyScale.y) * (float)(resolution + 1)) + resolution;

        Debug.Log(posX + " " + posY);

        // Draw a radius around the point
        for (int x = -radius; x < radius; x++)
        {
            int _x = posX + x;
            if (_x < 0 || _x >= resolution)
            {
                continue;
            }

            for (int y = -radius; y < radius; y++)
            {
                int _y = posY + y;
                if (_y < 0 || _y >= resolution)
                {
                    continue;
                }

                int squaredDist = x*x + y*y;
                if (squaredDist > squaredRadius)
                {
                    continue;
                }

                if (m_internalCreepTexture.GetPixel(_x, _y).a < value)
                {
                    m_internalCreepTexture.SetPixel(_x, _y, new Color(0.0f, 0.0f, 0.0f, value));
                    m_internalArray[_x, _y] = value;
                }
            }
        }

        m_internalCreepTexture.Apply();
    }

    [PunRPC]
    public void RemoveCreepAtPosition(Vector3 position, int radius, float value)
    {
        float squaredRadius = radius * radius;

        // Transform world position into texture space
        int posX = (int)(((position.x - quadForScale.transform.lossyScale.x * 0.5f) / quadForScale.transform.lossyScale.x) * (float)(resolution + 1)) + resolution;
        int posY = (int)(((position.z - quadForScale.transform.lossyScale.y * 0.5f) / quadForScale.transform.lossyScale.y) * (float)(resolution + 1)) + resolution;

        // Draw a radius around the point
        for (int x = -radius; x < radius; x++)
        {
            int _x = posX + x;
            if (_x < 0 || _x >= resolution)
            {
                continue;
            }

            for (int y = -radius; y < radius; y++)
            {
                int _y = posY + y;
                if (_y < 0 || _y >= resolution)
                {
                    continue;
                }

                int squaredDist = x*x + y*y;
                if (squaredDist > squaredRadius)
                {
                    continue;
                }

                if (m_internalCreepTexture.GetPixel(_x, _y).a > value)
                {
                    m_internalCreepTexture.SetPixel(_x, _y, new Color(0.0f, 0.0f, 0.0f, value));
                    m_internalArray[_x, _y] = value;
                }
            }
        }

        m_internalCreepTexture.Apply();
    }

    public bool CheckIfPosOnCreep(Vector3 position)
    {
        int posX = (int)(((position.x - quadForScale.transform.lossyScale.x * 0.5f) / quadForScale.transform.lossyScale.x) * (float)(resolution + 1)) + resolution;
        int posY = (int)(((position.z - quadForScale.transform.lossyScale.y * 0.5f) / quadForScale.transform.lossyScale.y) * (float)(resolution + 1)) + resolution;

        if (posX < 0 || posX >= resolution || posY < 0 || posY >= resolution)
        {
            return false;
        }

        return m_internalArray[posX, posY] > 0.75f;
    }
}
