using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// given the number of planets, generate a solar system where the planets orbit the sun

public class PlanetsController : MonoBehaviour
{



    [Header("Sun Reference")] 
    public Sun sun; // a one-of

    [Header("Planet Prefab Outlet")]
    public Planet planetPrefab; // the prototype to clone

    [Header("Color Outlets")]
    public List<Material> planetColors;

    [Header("Configurations")]
    public float sunScale = 3.0f;
    public float minScale = 0.2f;
    public float maxScale = 2.0f;

    public float minDegreesPerSecond = 60f;
    public float maxDegreesPerSecond = 180f;

    public float minDegreesOffset = 0f;
    public float maxDegreesOffset = 360f;

    public float positionDistance = 2.0f; // gap between each planet



    // private
    private List<Planet> generatedPlanets = new List<Planet>();
    private float elapsed = 0f;

    private void Start()
    {
        GenerateSS(5);
    }

    public void Update()
    {
        elapsed += Time.deltaTime;
        SetPlanetPositions(elapsed);
    }

    private void SetPlanetPositions(float elapsed)
    {
        foreach(Planet p in generatedPlanets)
        {
            // given the time, and distance, generate the position

            float dist = (positionDistance * (p.GetIndex() + 1));



            float x = dist * Mathf.Cos(SecondsToRadians(p,elapsed));
            float z = dist * Mathf.Sin(SecondsToRadians(p, elapsed));

            p.transform.localPosition = new Vector3(x, 0f, z);
            
        }



    }

    private float SecondsToRadians(Planet p, float elapsedSeconds)
    {
        float degreesPerSecond = p.GetDegreesPerSecond();

        float totalDegrees = p.GetDegreesPerSecond() * elapsedSeconds; // todo add random offset at start

        float remainderedDegs = totalDegrees % 360f;

        float totalRads = DegreesToRadians(remainderedDegs);

        return totalRads;

    }

    private static float DegreesToRadians(float degrees)
    {
        return (degrees * Mathf.PI) / 180f;
    }

    private static float RadiansToDegrees(float radians) // test for recip
    {
        return (radians * 180) / Mathf.PI;
    }


    private void GenerateSS(int numPlanets)
    {
        // initializations
        elapsed = 0f;
        generatedPlanets = new List<Planet>();

        if (numPlanets > 9 || numPlanets < 0)
        {
            Debug.LogError("Trying to generate " + numPlanets + " planets - numPlanets must be greater than 0 and less than 9");
            return;
        }

        sun.gameObject.transform.parent = this.transform;
        sun.transform.localPosition = new Vector3(0f, 0f, 0f);
        sun.transform.localScale = new Vector3(sunScale, sunScale, sunScale);

        for(int i = 0; i < numPlanets; i++)
        {



            Planet newPlanet = Instantiate(planetPrefab);

            newPlanet.transform.parent = sun.transform;

            float randomScale = UnityEngine.Random.Range(minScale, maxScale);

            float randomDegreesPerSecond = UnityEngine.Random.Range(minDegreesPerSecond, maxDegreesPerSecond);

            int randomColorIndex = UnityEngine.Random.Range(0, planetColors.Count - 1);

            newPlanet.Initialize(randomScale, planetColors[randomColorIndex], i, randomDegreesPerSecond);

            newPlanet.SetPosition(positionDistance);

            generatedPlanets.Add(newPlanet);


        }





    }



}
