using System;
using System.Linq;
using GLSLWallpaper.Common;

namespace GLSLWallpaper.UI;

public partial class MainWindow {
    public MainWindow() {
        InitializeComponent();
        ConfigureComponents();
    }

    void ConfigureComponents() {
        Title = $"{Identity.NAME} v{Identity.VERSION}";
        
        ShaderList.ItemsSource = PackRegistry.Packs;

        UpdatesPerSecondValue.Content = Settings.UpdatesPerSecond;
        UpdatesPerSecondSlider.Value = Settings.UpdatesPerSecond;
        UpdatesPerSecondSlider.ValueChanged += (_, args) => {
            Settings.UpdatesPerSecond = (int)args.NewValue;
            UpdatesPerSecondValue.Content = Settings.UpdatesPerSecond;
        };

        FramePerSecondValue.Content = Settings.FramesPerSecond;
        FramePerSecondSlider.Value = Settings.FramesPerSecond;
        FramePerSecondSlider.ValueChanged += (_, args) => {
            Settings.FramesPerSecond = (int)args.NewValue;
            FramePerSecondValue.Content = Settings.FramesPerSecond;
        };

        TimeScaleValue.Content = Settings.TimeScale;
        TimeScaleSlider.Value = Settings.TimeScale;
        TimeScaleSlider.ValueChanged += (_, args) => {
            Settings.TimeScale = Math.Round(args.NewValue, 2);
            TimeScaleValue.Content = Settings.TimeScale;
        };

        MouseCheckBox.IsChecked = Settings.MouseInteract;
        MouseCheckBox.Click += (_, _) => {
            Settings.MouseInteract = MouseCheckBox.IsChecked.Value;
        };

        if (Settings.CurrentShader != null) {
            ShaderList.SelectedItem = PackRegistry.GetByHash(Settings.CurrentShader, () => PackRegistry.Packs.First());
        } else if (ShaderList.Items.Count > 0) {
            ShaderList.SelectedItem = ShaderList.Items[0];
        }

        ShaderList.SelectionChanged += (_, args) => {
            if (args.AddedItems.Count == 1 && args.AddedItems[0] is ShaderPack data) {
                Settings.CurrentShader = data.Hash;
            }
        };
    }

    protected override void OnClosed(EventArgs e) {
        base.OnClosed(e);

        Settings.Save();
    }
}
