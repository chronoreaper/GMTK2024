using Cinemachine;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Header("View Cameras")]
    [SerializeField] private CinemachineVirtualCamera galaxyViewCamera;
    [SerializeField] private CinemachineVirtualCamera planetViewCamera;
    
    public enum PlanetType
    {
        Forest,
        Stone,
        Fire
    };

    [Space]
    [Header("Planet")]
    [SerializeField] private PlanetType planetType;
    [SerializeField] private GameObject planetGround;

    public delegate void OnSwitchPlanetView();
    public static OnSwitchPlanetView onSwitchPlanetView;

    public delegate void OnSwitchGalaxyView();
    public static OnSwitchGalaxyView onSwitchGalaxyView;

    // this function is for testing only it should be changed to use new input system
    // public void OnPlanetMouseDown()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //         if (Physics.Raycast(ray, out RaycastHit hit))
    //         {
    //             if (hit.collider.CompareTag("Planet"))
    //             {
    //                 Debug.Log("Go Into Planet View");

    //                 onPlanetSelected?.Invoke(gameObject);

    //                 SwitchToPlanetView();
    //             }
    //         }
    //     }
    // }

    public void SwitchToPlanetView()
    {
        onSwitchPlanetView?.Invoke();

        galaxyViewCamera.gameObject.SetActive(false);

        planetViewCamera.gameObject.SetActive(true);
        planetGround.SetActive(true);
    }

    public void SwitchToGalaxyView()
    {
        onSwitchGalaxyView?.Invoke();
        
        galaxyViewCamera.gameObject.SetActive(true);

        planetViewCamera.gameObject.SetActive(false);
        planetGround.SetActive(false);
    }
}
