using UnityEngine;

using LibNoise;
using LibNoise.Generator;
using LibNoise.Operator;

public class Noise : MonoBehaviour
{
    // Specify the size of the noise map to generate.
    public int mapSizeX = 256;
    public int mapSizeY = 256;

    // Perlin sample size
    public double sampleSizeX = 8.0;
    public double sampleSizeY = 8.0;

    // Add size to the offset
    public double sampleOffsetX = 5.0;
    public double sampleOffsetY = 6.0;


    // This class helps us to render the texture
    public Renderer planeRenderer;

    // This class helps us to keep new texture
    private Texture2D texture;

    // Is called before any of the Update
    private void Start()
    {
        Generate();
    }

    // Is called every frame (60 frames per second)
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Generate();
    }

    public double baseflatFrequency = 3.0;

    public double flatScale = 0.15;
    public double flatBias = -0.8;

    public double terraintypeFrequency = 0.6;
    public double terraintypePersistence = 0.4;

    public double finalTerrainEdgeFalloff = 0.14;

    private void Generate()
    {
        // Generates the rough mountainous terrain
        var mountainTerrain = new RidgedMultifractal();

        // Generates the base values for the flat terrain.
        var baseFlatTerrain = new Billow();
        baseFlatTerrain.Frequency = baseflatFrequency;

        // Applies a scaling factor and a bias to the output of the noise module connected to it.
        var flatTerrain =  new ScaleBias(flatScale, flatBias, baseFlatTerrain);

        // Defines the areas that these terrains will appear in the final height map.
        var terrainType = new Perlin();
        terrainType.Frequency = terraintypeFrequency;
        terrainType.Persistence = terraintypePersistence;

        // Combines all these noise modules together and generate the final terrain.
        var finalTerrain = new Select(flatTerrain, mountainTerrain, terrainType);
        finalTerrain.SetBounds(0.0, 1000.0);
        finalTerrain.FallOff = finalTerrainEdgeFalloff;

        // Genrating final terrain
        ModuleBase myModule = finalTerrain;


        // Generate a heightmap (two-dimensional array)
        // It stores coherent-noise values generated by a noise module
        var heightMap = new Noise2D(mapSizeX, mapSizeY, myModule);


        // Generates a non-seamless planar projection of the noise map.
        heightMap.GeneratePlanar(
            sampleOffsetX,
            sampleOffsetX + sampleSizeX,
            sampleOffsetY,
            sampleOffsetY + sampleSizeY
            );

        // Get the generated texture
        texture = heightMap.GetTexture(GradientPresets.Grayscale);

        // Set the render material texture of a plane
        // (Mesh Renderer component -> Materials -> Texture)
        planeRenderer.material.mainTexture = texture;
    }


}
