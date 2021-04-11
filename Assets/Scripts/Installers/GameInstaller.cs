using Zenject;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInstaller : MonoInstaller
{
    [Header("Managers")]
    public GameManager gameManager;
    public SoundManager soundManager;

    [Header("Location Data")]
    public LocationFactory locationFactory;
    public LocationLoader locationLoader;
    public RectTransform locationPartPrefab;
    public RectTransform scrollViewContent;
    public EventSystem eventSystem;

    public override void InstallBindings()
    {
        BindSoundManager();
        BindGameManager();

        BindLocationFactory();
        BindLocationLoader();
    }

    private void BindGameManager()
    {
        Container
            .Bind<GameManager>()
            .FromInstance(gameManager)
            .AsTransient();
    }
    private void BindSoundManager()
    {
        Container
            .Bind<SoundManager>()
            .FromInstance(soundManager)
            .AsSingle();
    }
    private void BindLocationFactory()
    {
        Container
            .Bind<RectTransform>()
            .WithId("lpPrefab")
            .FromInstance(locationPartPrefab)
            .AsTransient();

        Container
            .Bind<RectTransform>()
            .WithId("svContent")
            .FromInstance(scrollViewContent)
            .AsSingle();

        Container
            .Bind<LocationFactory>()
            .FromInstance(locationFactory)
            .AsSingle();
    }
    private void BindLocationLoader()
    {
        Container
            .BindInterfacesTo<LocationLoader>()
            .FromInstance(locationLoader)
            .AsSingle();

        Container
            .Bind<TextAsset>()
            .FromMethod(gameManager.GetJsonLocationData)
            .AsTransient();

        Container
            .Bind<LocationLoader>()
            .FromInstance(locationLoader)
            .AsTransient();
    }
}
