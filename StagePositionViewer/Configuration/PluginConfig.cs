using System.Runtime.CompilerServices;
using CameraUtils.Core;
using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace StagePositionViewer.Configuration
{
    internal class PluginConfig
    {
        public const float DefaultScreenPosX = 0f;
        public const float DefaultScreenPosY = 2.7f;
        public const float DefaultScreenPosZ = 20.0f;
        public static PluginConfig Instance { get; set; }

        // BSIPAが値の変更を検出し、自動的に設定を保存したい場合は、'virtual'でなければなりません。
        public virtual bool Enable { get; set; } = true;
        public virtual bool LockPosition { get; set; } = false;
        public virtual bool HMDOnly { get; set; } = false;
        public virtual bool PositionValueView { get; set; } = false;
        public virtual bool CenterSignal { get; set; } = true;
        public virtual float MovementSensitivityThreshold { get; set; } = 0.01f;  //HMD移動検出閾値
        public virtual float CenterMovementSensitivityThreshold { get; set; } = 0.001f; //中央部分のHMD移動検出閾値
        public virtual float CenterMovementSensitivityDistance { get; set; } = 0.02f; //中央部分の範囲(≧x or z)
        public virtual float LineWidth { get; set; } = 0.05f;
        public virtual float MarkSize { get; set; } = 0.3f;
        public virtual float StageWidth { get; set; } = 3f;
        public virtual float StageHight { get; set; } = 2f;
        public virtual float CanvasMargin { get; set; } = 1.25f;
        public virtual float ScreenPosX { get; set; } = DefaultScreenPosX;
        public virtual float ScreenPosY { get; set; } = DefaultScreenPosY;
        public virtual float ScreenPosZ { get; set; } = DefaultScreenPosZ;
        public virtual float ScreenRotX { get; set; } = 0;
        public virtual float ScreenRotY { get; set; } = 0;
        public virtual float ScreenRotZ { get; set; } = 0;
        public virtual float ScreenSize { get; set; } = 40f;
        [UseConverter(typeof(EnumConverter<VisibilityLayer>))]
        public virtual VisibilityLayer DefaultLayer { get; set; } = VisibilityLayer.UI;
        [UseConverter(typeof(EnumConverter<VisibilityLayer>))]
        public virtual VisibilityLayer HMDOnlyLayer { get; set; } = VisibilityLayer.HmdOnlyAndReflected;
        public virtual float FrontLimitLine { get; set; } = 0.6f;
        public virtual float BackLimitLine { get; set; } = 0.6f;
        public virtual float RightLimitLine { get; set; } = 1.3f;
        public virtual float LeftLimitLine { get; set; } = 1.3f;
        public virtual float WarningPercentage1 { get; set; } = 0.4f;
        public virtual float WarningPercentage2 { get; set; } = 0.8f;
        public virtual float CenterLimitX { get; set; } = 0.011f;
        public virtual bool CustomTargetDevice { get; set; } = false;
        public virtual string TargetDevice1 { get; set; } = "NONE";
        public virtual string TargetDevice2 { get; set; } = "NONE";
        public virtual float TargetDeviceOffsetX { get; set; } = 0;
        public virtual float TargetDeviceOffsetZ { get; set; } = 0;
    }
}
