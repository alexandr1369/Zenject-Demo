using Zenject;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LocationFactory : MonoBehaviour, IFactory
{
    private RectTransform _locationPartPrefab;
    private RectTransform _scrollViewContent;
    private GridLayoutGroup _glgContent;

    private const string _locationPath = "Textures/Location/";
    private const int _locationGridCellSize = 512;

    #region Initialization

    [Inject]
    public void Construct(
    [Inject(Id = "lpPrefab")] RectTransform locationPartPrefab,
    [Inject(Id = "svContent")] RectTransform scrollViewContent)
    {
        _locationPartPrefab = locationPartPrefab;
        _scrollViewContent = scrollViewContent;
    }
    private void Awake()
    {
        _glgContent = _scrollViewContent.GetComponent<GridLayoutGroup>();
    }

    #endregion

    /// <summary>
    /// Create and spawn location grid.
    /// </summary>
    public void Create(LocationData locationData)
    {
        // location grid always have to be [X x Y]
        // get last list element to get grid = [maxX; maxY]
        LocationPart lastElement = locationData.List[locationData.List.Count - 1];
        LocationDataVector partData = new LocationDataVector(-1);
        foreach (string part in lastElement.Id.Split('_'))
        {
            int number;
            if (int.TryParse(part, out number))
            {
                if (partData.y == -1)
                    partData.y = number;
                else
                    partData.x = number;
            }
        }

        // get full width and height of location container
        float width = 0, height = 0;
        for (int i = 0; i <= partData.x; i++) width += _locationGridCellSize;
        for (int i = 0; i <= partData.y; i++) height += _locationGridCellSize;

        // set location container width, height and local position (left top)
        _scrollViewContent.sizeDelta = new Vector2(width, height);
        _scrollViewContent.localPosition = new Vector3(_scrollViewContent.sizeDelta.x / 2, -_scrollViewContent.sizeDelta.y / 2);

        // set grid layout group cell size and fixed column count
        _glgContent.cellSize = new Vector2(_locationGridCellSize, _locationGridCellSize);
        _glgContent.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _glgContent.constraintCount = partData.x + 1;

        // load location parts sprites
        List<RectTransform> locationPartsTransforms = new List<RectTransform>();
        for(int i = 0; i < locationData.List.Count; i++)
        {
            string spritePath = _locationPath + locationData.List[i].Id;
            Sprite locationPartSprite = Resources.Load<Sprite>(spritePath);
            RectTransform locationPart = Instantiate(_locationPartPrefab,  _scrollViewContent.transform);
            locationPart.GetComponent<Image>().sprite = locationPartSprite;
            locationPartsTransforms.Add(locationPart);
        }

        // update container to the right size
        float newWidth = width - 512 + lastElement.Width * 100;
        float newHeight = height - 512 + lastElement.Height * 100;
        _scrollViewContent.sizeDelta = new Vector2(newWidth, newHeight);

        // update location parts data after grid creation with saved grid elements position
        StartCoroutine(UpdateOutsideLocationParts(partData, locationData, locationPartsTransforms));
    }

    // makes sence when there're cut locations parts outside of the grid
    // (location part width and height != cell width and height)
    private IEnumerator UpdateOutsideLocationParts
    (LocationDataVector partData, LocationData locData, List<RectTransform> lpTransforms)
    {
        yield return new WaitForEndOfFrame();

        // disable grid layout group 
        _glgContent.enabled = false;

        // update outside (right and bottom because grid begins from left top) location parts to the right size
        for (int i = 0; i <= partData.y; i++)
        {
            int listIndex = partData.x * (i < 0? 1 : i + 1) + i;
            LocationPart locationPart = locData.List[listIndex];
            RectTransform locationPartTransform = lpTransforms[listIndex];

            locationPartTransform.sizeDelta = new Vector2(locationPart.Width * 100, locationPartTransform.sizeDelta.y);

            //print(_scrollViewContent.sizeDelta.x - locationPart.Width * 100 / 2);
            float locationPartXLocalPosition = _scrollViewContent.sizeDelta.x / 2 - locationPart.Width * 100 / 2;
            locationPartTransform.localPosition = new Vector2(locationPartXLocalPosition, locationPartTransform.localPosition.y);
        }
    }
}