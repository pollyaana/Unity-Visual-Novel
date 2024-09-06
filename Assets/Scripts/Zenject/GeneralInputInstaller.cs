using UnityEngine;
using Zenject;

public class GeneralInputInstaller : MonoInstaller
{
    [SerializeField] private InputInstaller inputInstaller;
    public override void InstallBindings()
    {
        BindInputInstaller();
    }

    private void BindInputInstaller()
    {
        Container.Bind<InputInstaller>().FromInstance(inputInstaller).AsSingle();
    }
}
