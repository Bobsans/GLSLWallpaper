using System;
using System.Windows.Forms;
using GLSLWallpapers.Helpers;

namespace GLSLWallpapers.Components {
    public partial class ShaderList : UserControl {
        public int Count => FlowListBoxPanel.Controls.Count;

        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        public event EventHandler<ShaderInfo> ApplyButtonCLick;

        public ShaderList() {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void Add(ShaderListItem item) {
            item.ApplyButtonCLick += OnItemApplyClick;
            FlowListBoxPanel.Controls.Add(item);

            SetupAnchors();
        }

        public void Remove(int index) {
            if (index < 0 || index > FlowListBoxPanel.Controls.Count) {
                throw new Exception("Invalid index");
            }

            if (FlowListBoxPanel.Controls[index] is ShaderListItem item) {
                FlowListBoxPanel.Controls.Remove(item);
                item.ApplyButtonCLick -= OnItemApplyClick;
                item.Dispose();
                SetupAnchors();
            }
        }

        public void Clear() {
            while (FlowListBoxPanel.Controls.Count > 0) {
                Remove(0);
            }
        }

        void SetupAnchors() {
            for (int i = 0; i < FlowListBoxPanel.Controls.Count; i++) {
                if (FlowListBoxPanel.Controls[i] is ShaderListItem item) {
                    if (i == 0) {
                        item.Anchor = AnchorStyles.Left | AnchorStyles.Top;

                        if (FlowListBoxPanel.VerticalScroll.Visible) {
                            item.Width = FlowListBoxPanel.Width - SystemInformation.VerticalScrollBarWidth - Margin.Horizontal;
                        } else {
                            item.Width = FlowListBoxPanel.Width - Margin.Horizontal;
                        }
                    } else {
                        item.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                    }
                }
            }
        }

        void OnItemApplyClick(object sender, ShaderInfo info) {
            ApplyButtonCLick?.Invoke(sender, info);
        }

        void FlowListBoxPanel_Resize(object sender, EventArgs e) {
            if (FlowListBoxPanel.Controls.Count > 0) {
                if (FlowListBoxPanel.VerticalScroll.Visible) {
                    FlowListBoxPanel.Controls[0].Width = FlowListBoxPanel.Width - SystemInformation.VerticalScrollBarWidth - Margin.Horizontal;
                } else {
                    FlowListBoxPanel.Controls[0].Width = FlowListBoxPanel.Width - Margin.Horizontal;
                }

                FlowListBoxPanel.Update();
            }
        }
    }
}
