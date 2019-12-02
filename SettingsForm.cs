using System;
using System.ComponentModel;
using System.Windows.Forms;
using GLSLWallpapers.Components;
using GLSLWallpapers.Helpers;

namespace GLSLWallpapers {
    public partial class SettingsForm : Form {
        public SettingsForm() {
            InitializeComponent();

            foreach (ShaderInfo info in ShaderRegistry.All()) {
                ShaderListView.Add(new ShaderListItem(info) {
                    Selected = Config.ShaderName == info.FileName
                });
            }

            ShaderListView.ApplyButtonCLick += (sender, info) => Config.ShaderName = info.FileName;
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            Text = $@"{Reference.NAME} v{Reference.VERSION}";

            TrackBarTimeScale.Value = Config.TimeScale;
            LabelTimeScaleValue.Text = $@"x{(float)Config.TimeScale / 1000:0.00}";

            TrackBarFramesPerSecond.Value = Config.FramesPerSecond;
            LabelFramesPerSecondValue.Text = $@"{Config.FramesPerSecond} fps";

            TrackBarUpdatesPerSecond.Value = Config.UpdatesPerSecond;
            LabelUpdatesPerSecondValue.Text = $@"{Config.UpdatesPerSecond} ups";

            CheckBoxMouseInteract.Checked = Config.MouseInteract;
        }

        protected override void OnClosing(CancelEventArgs e) {
            base.OnClosing(e);
            Config.Save();
        }

        void TrackBarTimeScale_Scroll(object sender, EventArgs e) {
            Config.TimeScale = TrackBarTimeScale.Value;
            LabelTimeScaleValue.Text = $@"x{(float)Config.TimeScale / 1000:0.00}";
        }

        void TrackBarFramesPerSecond_Scroll(object sender, EventArgs e) {
            Config.FramesPerSecond = TrackBarFramesPerSecond.Value;
            LabelFramesPerSecondValue.Text = $@"{Config.FramesPerSecond} fps";
        }

        void TrackBarUpdatesPerSecond_Scroll(object sender, EventArgs e) {
            Config.UpdatesPerSecond = TrackBarUpdatesPerSecond.Value;
            LabelUpdatesPerSecondValue.Text = $@"{Config.UpdatesPerSecond} ups";
        }

        void CheckBoxMouseInteract_CheckedChanged(object sender, EventArgs e) {
            Config.MouseInteract = CheckBoxMouseInteract.Checked;
        }
    }
}
