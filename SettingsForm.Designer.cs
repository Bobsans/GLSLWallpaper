namespace GLSLWallpapers {
    partial class SettingsForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CheckBoxMouseInteract = new System.Windows.Forms.CheckBox();
            this.LabelUpdatesPerSecondValue = new System.Windows.Forms.Label();
            this.LabelFramesPerSecondValue = new System.Windows.Forms.Label();
            this.LabelTimeScaleValue = new System.Windows.Forms.Label();
            this.LabelUpdatesPerSecond = new System.Windows.Forms.Label();
            this.LabelFramesPerSecond = new System.Windows.Forms.Label();
            this.TimeScaleLabel = new System.Windows.Forms.Label();
            this.TrackBarUpdatesPerSecond = new System.Windows.Forms.TrackBar();
            this.TrackBarFramesPerSecond = new System.Windows.Forms.TrackBar();
            this.TrackBarTimeScale = new System.Windows.Forms.TrackBar();
            this.ShaderListView = new GLSLWallpapers.Components.ShaderList();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarUpdatesPerSecond)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarFramesPerSecond)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarTimeScale)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CheckBoxMouseInteract);
            this.groupBox1.Controls.Add(this.LabelUpdatesPerSecondValue);
            this.groupBox1.Controls.Add(this.LabelFramesPerSecondValue);
            this.groupBox1.Controls.Add(this.LabelTimeScaleValue);
            this.groupBox1.Controls.Add(this.LabelUpdatesPerSecond);
            this.groupBox1.Controls.Add(this.LabelFramesPerSecond);
            this.groupBox1.Controls.Add(this.TimeScaleLabel);
            this.groupBox1.Controls.Add(this.TrackBarUpdatesPerSecond);
            this.groupBox1.Controls.Add(this.TrackBarFramesPerSecond);
            this.groupBox1.Controls.Add(this.TrackBarTimeScale);
            this.groupBox1.Location = new System.Drawing.Point(12, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(542, 522);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // CheckBoxMouseInteract
            // 
            this.CheckBoxMouseInteract.Location = new System.Drawing.Point(124, 132);
            this.CheckBoxMouseInteract.Name = "CheckBoxMouseInteract";
            this.CheckBoxMouseInteract.Size = new System.Drawing.Size(412, 24);
            this.CheckBoxMouseInteract.TabIndex = 4;
            this.CheckBoxMouseInteract.Text = "Mouse interact";
            this.CheckBoxMouseInteract.UseVisualStyleBackColor = true;
            this.CheckBoxMouseInteract.CheckedChanged += new System.EventHandler(this.CheckBoxMouseInteract_CheckedChanged);
            // 
            // LabelUpdatesPerSecondValue
            // 
            this.LabelUpdatesPerSecondValue.Location = new System.Drawing.Point(6, 110);
            this.LabelUpdatesPerSecondValue.Name = "LabelUpdatesPerSecondValue";
            this.LabelUpdatesPerSecondValue.Size = new System.Drawing.Size(120, 18);
            this.LabelUpdatesPerSecondValue.TabIndex = 3;
            this.LabelUpdatesPerSecondValue.Text = "60 ups";
            this.LabelUpdatesPerSecondValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // LabelFramesPerSecondValue
            // 
            this.LabelFramesPerSecondValue.Location = new System.Drawing.Point(6, 73);
            this.LabelFramesPerSecondValue.Name = "LabelFramesPerSecondValue";
            this.LabelFramesPerSecondValue.Size = new System.Drawing.Size(120, 18);
            this.LabelFramesPerSecondValue.TabIndex = 3;
            this.LabelFramesPerSecondValue.Text = "60 fps";
            this.LabelFramesPerSecondValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // LabelTimeScaleValue
            // 
            this.LabelTimeScaleValue.Location = new System.Drawing.Point(6, 35);
            this.LabelTimeScaleValue.Name = "LabelTimeScaleValue";
            this.LabelTimeScaleValue.Size = new System.Drawing.Size(120, 18);
            this.LabelTimeScaleValue.TabIndex = 3;
            this.LabelTimeScaleValue.Text = "x1.0";
            this.LabelTimeScaleValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // LabelUpdatesPerSecond
            // 
            this.LabelUpdatesPerSecond.Location = new System.Drawing.Point(6, 93);
            this.LabelUpdatesPerSecond.Name = "LabelUpdatesPerSecond";
            this.LabelUpdatesPerSecond.Size = new System.Drawing.Size(120, 22);
            this.LabelUpdatesPerSecond.TabIndex = 2;
            this.LabelUpdatesPerSecond.Text = "Updates per second";
            this.LabelUpdatesPerSecond.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // LabelFramesPerSecond
            // 
            this.LabelFramesPerSecond.Location = new System.Drawing.Point(6, 57);
            this.LabelFramesPerSecond.Name = "LabelFramesPerSecond";
            this.LabelFramesPerSecond.Size = new System.Drawing.Size(120, 22);
            this.LabelFramesPerSecond.TabIndex = 2;
            this.LabelFramesPerSecond.Text = "Frames per second";
            this.LabelFramesPerSecond.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // TimeScaleLabel
            // 
            this.TimeScaleLabel.Location = new System.Drawing.Point(6, 18);
            this.TimeScaleLabel.Name = "TimeScaleLabel";
            this.TimeScaleLabel.Size = new System.Drawing.Size(120, 22);
            this.TimeScaleLabel.TabIndex = 2;
            this.TimeScaleLabel.Text = "Time scale";
            this.TimeScaleLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // TrackBarUpdatesPerSecond
            // 
            this.TrackBarUpdatesPerSecond.LargeChange = 10;
            this.TrackBarUpdatesPerSecond.Location = new System.Drawing.Point(124, 93);
            this.TrackBarUpdatesPerSecond.Maximum = 120;
            this.TrackBarUpdatesPerSecond.Minimum = 1;
            this.TrackBarUpdatesPerSecond.Name = "TrackBarUpdatesPerSecond";
            this.TrackBarUpdatesPerSecond.Size = new System.Drawing.Size(412, 45);
            this.TrackBarUpdatesPerSecond.TabIndex = 1;
            this.TrackBarUpdatesPerSecond.TickFrequency = 10;
            this.TrackBarUpdatesPerSecond.Value = 1;
            this.TrackBarUpdatesPerSecond.Scroll += new System.EventHandler(this.TrackBarUpdatesPerSecond_Scroll);
            // 
            // TrackBarFramesPerSecond
            // 
            this.TrackBarFramesPerSecond.LargeChange = 10;
            this.TrackBarFramesPerSecond.Location = new System.Drawing.Point(124, 57);
            this.TrackBarFramesPerSecond.Maximum = 120;
            this.TrackBarFramesPerSecond.Minimum = 1;
            this.TrackBarFramesPerSecond.Name = "TrackBarFramesPerSecond";
            this.TrackBarFramesPerSecond.Size = new System.Drawing.Size(412, 45);
            this.TrackBarFramesPerSecond.TabIndex = 1;
            this.TrackBarFramesPerSecond.TickFrequency = 10;
            this.TrackBarFramesPerSecond.Value = 1;
            this.TrackBarFramesPerSecond.Scroll += new System.EventHandler(this.TrackBarFramesPerSecond_Scroll);
            // 
            // TrackBarTimeScale
            // 
            this.TrackBarTimeScale.LargeChange = 10;
            this.TrackBarTimeScale.Location = new System.Drawing.Point(124, 18);
            this.TrackBarTimeScale.Maximum = 2000;
            this.TrackBarTimeScale.Minimum = 1;
            this.TrackBarTimeScale.Name = "TrackBarTimeScale";
            this.TrackBarTimeScale.Size = new System.Drawing.Size(412, 45);
            this.TrackBarTimeScale.TabIndex = 1;
            this.TrackBarTimeScale.TickFrequency = 50;
            this.TrackBarTimeScale.Value = 1;
            this.TrackBarTimeScale.Scroll += new System.EventHandler(this.TrackBarTimeScale_Scroll);
            // 
            // ShaderListView
            // 
            this.ShaderListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.ShaderListView.BackColor = System.Drawing.SystemColors.Window;
            this.ShaderListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ShaderListView.Location = new System.Drawing.Point(561, 12);
            this.ShaderListView.Name = "ShaderListView";
            this.ShaderListView.Size = new System.Drawing.Size(434, 513);
            this.ShaderListView.TabIndex = 1;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 537);
            this.Controls.Add(this.ShaderListView);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1024, 576);
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarUpdatesPerSecond)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarFramesPerSecond)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarTimeScale)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private GLSLWallpapers.Components.ShaderList ShaderListView;
        private System.Windows.Forms.TrackBar TrackBarTimeScale;
        private System.Windows.Forms.Label TimeScaleLabel;
        private System.Windows.Forms.Label LabelTimeScaleValue;
        private System.Windows.Forms.CheckBox CheckBoxMouseInteract;
        private System.Windows.Forms.TrackBar TrackBarUpdatesPerSecond;
        private System.Windows.Forms.Label LabelUpdatesPerSecond;
        private System.Windows.Forms.Label LabelUpdatesPerSecondValue;
        private System.Windows.Forms.Label LabelFramesPerSecondValue;
        private System.Windows.Forms.Label LabelFramesPerSecond;
        private System.Windows.Forms.TrackBar TrackBarFramesPerSecond;
    }
}