using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GeneralShowAnswersInstaller : MonoInstaller
{
    [SerializeField] private ShowAnswersInstaller showAnswersInstaller;

    public override void InstallBindings()
    {
        BindShowAnswersInstaller();
    }

    private void BindShowAnswersInstaller()
    {
        Container.Bind<ShowAnswersInstaller>().FromInstance(showAnswersInstaller).AsSingle();
    }
}
    