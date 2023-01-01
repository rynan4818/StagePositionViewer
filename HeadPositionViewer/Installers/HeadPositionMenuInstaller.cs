using HeadPositionViewer.Views;
using Zenject;

namespace HeadPositionViewer.Installers
{
    public class HeadPositionMenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<SettingTabViewController>().FromNewComponentAsViewController().AsSingle().NonLazy();
        }
    }
}
