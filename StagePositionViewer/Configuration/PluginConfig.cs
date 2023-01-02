using System.Runtime.CompilerServices;
using IPA.Config.Stores;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace StagePositionViewer.Configuration
{
    internal class PluginConfig
    {
        public static PluginConfig Instance { get; set; }

        // BSIPAが値の変更を検出し、自動的に設定を保存したい場合は、'virtual'でなければなりません。
        public virtual bool Enable { get; set; } = true;
        public virtual bool LockPosition { get; set; } = false;
        public virtual bool HmdOnly { get; set; } = false;
        public virtual bool PositionValueView { get; set; } = false;
        public virtual float MovementSensitivityThreshold { get; set; } = 0.01f;  //HMD移動検出閾値
        public virtual float CenterMovementSensitivityThreshold { get; set; } = 0.001f; //中央部分のHMD移動検出閾値
        public virtual float CenterMovementSensitivityDistance { get; set; } = 0.02f; //中央部分の範囲(≧x or z)
        public virtual float ScreenPosX { get; set; } = 0f;
        public virtual float ScreenPosY { get; set; } = 3.5f;
        public virtual float ScreenPosZ { get; set; } = 7.0f;
        public virtual float ScreenRotX { get; set; } = 0;
        public virtual float ScreenRotY { get; set; } = 0;
        public virtual float ScreenRotZ { get; set; } = 0;
        public virtual float ScreenSize { get; set; } = 40f;
        public virtual int ScreenLayer { get; set; } = 5;
        public virtual float FrontLimitLine { get; set; } = 0.7f;
        public virtual float BackLimitLine { get; set; } = 0.5f;
        public virtual float RightLimitLine { get; set; } = 1f;
        public virtual float LeftLimitLine { get; set; } = 1f;
        public virtual float WarningPercentage1 { get; set; } = 0.4f;
        public virtual float WarningPercentage2 { get; set; } = 0.8f;
        public virtual float CenterLimitX { get; set; } = 0.011f;
    }
}
