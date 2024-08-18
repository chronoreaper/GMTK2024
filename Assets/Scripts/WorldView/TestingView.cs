using UnityEngine;

public class TestingView : WorldView
{
    protected override void WorldViewChanged(Views newView)
    {
        if (newView == Views.GalaxyView)
        {
            Debug.Log("u r currently in the galaxy view");
        } else if (newView == Views.PlanetView)
        {
            Debug.Log("u are currently in the Planet View");
        }
    }

    public void ChangeToGalaxyView()
    {
        ChangeWorldViewToGalaxy();
    }

    public void ChangeToPlanetView()
    {
        ChangeWorldViewToPlanet();
    }
}
