using System;
using System.Drawing;
using System.Windows.Forms;
using GLSLWallpapers.Helpers;

namespace GLSLWallpapers.Components {
    public partial class ShaderListItem : UserControl {
        public event EventHandler<ShaderInfo> ApplyButtonCLick;

        public ShaderInfo ShaderInfo { get; }

        public bool Selected { get; set; }

        public ShaderListItem(ShaderInfo info) {
            InitializeComponent();

            ShaderInfo = info;

            LabelTitle.Text = info.Name;
            LabelAuthorValue.Text = info.Author;
            LabelFileValue.Text = info.FileName;
            PictureBoxPreview.BackgroundImage = info.Image;

            Config.ShaderChange += (sender, s) => Selected = s == info.FileName;
        }

        protected override void OnPaint(PaintEventArgs e) {
            if (Selected) {
                e.Graphics.FillRectangle(Brushes.Bisque, Bounds);
            }

            base.OnPaint(e);
            e.Graphics.DrawLine(Pens.Lavender, 0, Height - 1, Width - 1, Height - 1);
        }

        void ButtonApply_Click(object sender, EventArgs e) {
            ApplyButtonCLick?.Invoke(this, ShaderInfo);
        }
    }
}