using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GLSLWallpapers.Components {
    partial class ShaderList {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.FlowListBoxPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // FlowListBoxPanel
            // 
            this.FlowListBoxPanel.AutoScroll = true;
            this.FlowListBoxPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FlowListBoxPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.FlowListBoxPanel.Location = new System.Drawing.Point(0, 0);
            this.FlowListBoxPanel.Margin = new System.Windows.Forms.Padding(0);
            this.FlowListBoxPanel.Name = "FlowListBoxPanel";
            this.FlowListBoxPanel.Size = new System.Drawing.Size(175, 173);
            this.FlowListBoxPanel.TabIndex = 0;
            this.FlowListBoxPanel.WrapContents = false;
            this.FlowListBoxPanel.Resize += new System.EventHandler(this.FlowListBoxPanel_Resize);
            // 
            // ShaderList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.FlowListBoxPanel);
            this.Name = "ShaderList";
            this.Size = new System.Drawing.Size(175, 173);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel FlowListBoxPanel;
    }
}