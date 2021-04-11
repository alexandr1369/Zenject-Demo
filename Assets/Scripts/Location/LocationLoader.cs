using Zenject;
using UnityEngine;

public class LocationLoader : MonoBehaviour, IInitializable
{
    private TextAsset _jsonLocationData;
    private LocationFactory _locationFactory;
    private LocationData _locationData;
    public LocationData LocationData { get => _locationData; }

    [Inject]
    public void Construct(TextAsset jsonLocationData, LocationFactory locationFactory)
    {
        _jsonLocationData = jsonLocationData;
        _locationFactory = locationFactory;

        // load json location part data
        _locationData = JsonUtility.FromJson<LocationData>(_jsonLocationData.text);
    }
    public void Initialize()
    {
        _locationFactory.Create(_locationData);
    }
}
