using System;
using UnityEngine;
using Zenject;

public class BubblesInsatller : MonoInstaller
{
    [SerializeField]
    private Settings settings;

    public override void InstallBindings()
    {
        Container.Bind<TCPServer>().ToSinglePrefabResource<TCPServer>("Server");
        Container.Bind<Spawner>().ToSinglePrefabResource<Spawner>("Spawner");
        Container.Bind<TCPClient>().ToSinglePrefabResource<TCPClient>("Client");
        Container.Bind<CommandHadler>().ToSinglePrefabResource<CommandHadler>("CommandHandler");
        Container.Bind<Menu>().ToSinglePrefabResource<Menu>("UI");
        Container.Bind<ITextureManager>().ToSinglePrefabResource<TextureManager>("TextureManager");

        Container.Bind<GameLogic>().ToSinglePrefabResource<GameLogic>("GameRoot");
        Container.Bind<IInitializable>().ToSinglePrefabResource<GameLogic>("GameRoot");
    }
}

[Serializable]
public class Settings
{
    public GameObject server;
    public GameObject client;
    public GameObject commandHadler;
    public GameObject menu;
}