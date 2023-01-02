using StagePositionViewer.Views;
using Zenject;

namespace StagePositionViewer.Installers
{
    public class StagePositionMenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<SettingTabViewController>().FromNewComponentAsViewController().AsSingle().NonLazy();
        }
    }
}
