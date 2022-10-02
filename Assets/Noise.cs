using UnityEngine;

using LibNoise;
using LibNoise.Generator;
using LibNoise.Operator;

public class Noise : MonoBehaviour
{
    public int mapSizeX = 256;
    public int mapSizeY = 256;

    public float sampleSizeX = 20.0f;
    public float sampleSizeY = 20.0f;

    public float sampleOffsetX = 5.0f;
    public float sampleOffsetY = 5.0f;

    public Renderer planeRenderer;
    private Texture2D texture;

    void Start()
    {
        Generate();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Generate();
    }

    private void Generate()
    {
        Perlin myPerlin = new Perlin();
        ModuleBase myModule = myPerlin;

        Noise2D heightMap;
        heightMap = new Noise2D(mapSizeX, mapSizeY, myModule);

        heightMap.GeneratePlanar(
            sampleOffsetX,
            sampleOffsetX + sampleSizeX,
            sampleOffsetY,
            sampleOffsetY + sampleSizeY
            );
        texture = heightMap.GetTexture(GradientPresets.Grayscale);
        planeRenderer.material.mainTexture = texture;
    }


}
