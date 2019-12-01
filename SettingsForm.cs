using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using GLSLWallpapers.Components;
using GLSLWallpapers.Helpers;

namespace GLSLWallpapers {
    public partial class SettingsForm : Form {
        public SettingsForm() {
            InitializeComponent();
            
            foreach (KeyValuePair<string,ShaderInfo> pair in ShaderRegistry.Shaders) {
                ShaderListView.Add(new ShaderListItem(pair.Value));
            }
            
            ShaderListView.ApplyButtonCLick += (sender, info) => Config.ShaderName = info.FileName;
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            Text = $@"{Reference.NAME} v{Reference.VERSION}";

            TrackBarTimeScale.Value = Config.TimeScale;
            LabelTimeScaleValue.Text = $@"x{(float)Config.TimeScale / 1000:0.00}";

            TrackBarUpdateFrequency.Value = Config.UpdateFrequency;
            LabelUpdateFrequencyValue.Text = $@"{Config.UpdateFrequency} fps";

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

        void TrackBarUpdateFrequency_Scroll(object sender, EventArgs e) {
            Config.UpdateFrequency = TrackBarUpdateFrequency.Value;
            LabelUpdateFrequencyValue.Text = $@"{Config.UpdateFrequency} fps";
        }

        void CheckBoxMouseInteract_CheckedChanged(object sender, EventArgs e) {
            Config.MouseInteract = CheckBoxMouseInteract.Checked;
        }
    }
}