using UnityEngine;
using Zenject;
public class GeneralMenuGroupInstaller : MonoInstaller
{
    [SerializeField] private MenuGroupInstaller menuGroupInstaller;

    public override void InstallBindings()
    {
        BindNameInstaller();
    }
    private void BindNameInstaller()
    {
        Container.Bind<MenuGroupInstaller>().FromInstance(menuGroupInstaller).AsSingle();
    }
}
