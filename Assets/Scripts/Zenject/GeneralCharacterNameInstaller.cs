using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GeneralCharacterNameInstaller : MonoInstaller
{
    [SerializeField] private CharacterNameInstaller characterInstaller;

    public override void InstallBindings()
    {
        BindNameInstaller();
    }

    private void BindNameInstaller()
    {
        Container.Bind<CharacterNameInstaller>().FromInstance(characterInstaller).AsSingle();
    }
}
