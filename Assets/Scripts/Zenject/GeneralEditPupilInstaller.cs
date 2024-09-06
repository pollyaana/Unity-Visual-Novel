using UnityEngine;
using Zenject;

public class GeneralEditPupileInstaller : MonoInstaller
{
    [SerializeField] private EditPupilInstaller editPupilInstaller;

    public override void InstallBindings()
    {
        BindNameInstaller();
    }

    private void BindNameInstaller()
    {
        Container.Bind <EditPupilInstaller>().FromInstance(editPupilInstaller).AsSingle();
    }
}