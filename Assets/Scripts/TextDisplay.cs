using UnityEngine;
using UnityEngine.UI;

public class TextDisplay : MonoBehaviour
{
    public Text text;

    [Zenject.Inject]
    public void Construct(LocationLoader locationLoader)
    {
        text.text = locationLoader.LocationData.List[0].Id;
    }
}
