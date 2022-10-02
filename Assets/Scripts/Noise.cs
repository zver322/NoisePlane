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
    public double sampleSizeX = 6.0;
    public double sampleSizeY = 6.0;

    // Add size to the offset
    public double sampleOffsetX = 3.0;
    public double sampleOffsetY = 4.0;

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

    private void Generate()
    {
        // Instantiate Perlin noise module
        var myPerlin = new Perlin();

        // Set this module into base class for noise modules
        ModuleBase myModule = myPerlin;

        
        // Generate a heightmap (two-dimensional array)
        // It stores oherent-noise values generated by a noise module
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
