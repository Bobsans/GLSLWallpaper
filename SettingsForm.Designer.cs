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
            this.LabelUpdateFrequencyValue = new System.Windows.Forms.Label();
            this.LabelTimeScaleValue = new System.Windows.Forms.Label();
            this.LabelUpdateFrequency = new System.Windows.Forms.Label();
            this.TimeScaleLabel = new System.Windows.Forms.Label();
            this.TrackBarUpdateFrequency = new System.Windows.Forms.TrackBar();
            this.TrackBarTimeScale = new System.Windows.Forms.TrackBar();
            this.ShaderListView = new GLSLWallpapers.Components.ShaderList();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarUpdateFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarTimeScale)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CheckBoxMouseInteract);
            this.groupBox1.Controls.Add(this.LabelUpdateFrequencyValue);
            this.groupBox1.Controls.Add(this.LabelTimeScaleValue);
            this.groupBox1.Controls.Add(this.LabelUpdateFrequency);
            this.groupBox1.Controls.Add(this.TimeScaleLabel);
            this.groupBox1.Controls.Add(this.TrackBarUpdateFrequency);
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
            this.CheckBoxMouseInteract.Location = new System.Drawing.Point(124, 93);
            this.CheckBoxMouseInteract.Name = "CheckBoxMouseInteract";
            this.CheckBoxMouseInteract.Size = new System.Drawing.Size(412, 24);
            this.CheckBoxMouseInteract.TabIndex = 4;
            this.CheckBoxMouseInteract.Text = "Mouse interact";
            this.CheckBoxMouseInteract.UseVisualStyleBackColor = true;
            this.CheckBoxMouseInteract.CheckedChanged += new System.EventHandler(this.CheckBoxMouseInteract_CheckedChanged);
            // 
            // LabelUpdateFrequencyValue
            // 
            this.LabelUpdateFrequencyValue.Location = new System.Drawing.Point(6, 73);
            this.LabelUpdateFrequencyValue.Name = "LabelUpdateFrequencyValue";
            this.LabelUpdateFrequencyValue.Size = new System.Drawing.Size(120, 18);
            this.LabelUpdateFrequencyValue.TabIndex = 3;
            this.LabelUpdateFrequencyValue.Text = "60 fps";
            this.LabelUpdateFrequencyValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
            // LabelUpdateFrequency
            // 
            this.LabelUpdateFrequency.Location = new System.Drawing.Point(6, 57);
            this.LabelUpdateFrequency.Name = "LabelUpdateFrequency";
            this.LabelUpdateFrequency.Size = new System.Drawing.Size(120, 22);
            this.LabelUpdateFrequency.TabIndex = 2;
            this.LabelUpdateFrequency.Text = "Update frequency";
            this.LabelUpdateFrequency.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
            // TrackBarUpdateFrequency
            // 
            this.TrackBarUpdateFrequency.LargeChange = 10;
            this.TrackBarUpdateFrequency.Location = new System.Drawing.Point(124, 57);
            this.TrackBarUpdateFrequency.Maximum = 120;
            this.TrackBarUpdateFrequency.Minimum = 1;
            this.TrackBarUpdateFrequency.Name = "TrackBarUpdateFrequency";
            this.TrackBarUpdateFrequency.Size = new System.Drawing.Size(412, 45);
            this.TrackBarUpdateFrequency.TabIndex = 1;
            this.TrackBarUpdateFrequency.TickFrequency = 10;
            this.TrackBarUpdateFrequency.Value = 1;
            this.TrackBarUpdateFrequency.Scroll += new System.EventHandler(this.TrackBarUpdateFrequency_Scroll);
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
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarUpdateFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarTimeScale)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private GLSLWallpapers.Components.ShaderList ShaderListView;
        private System.Windows.Forms.TrackBar TrackBarTimeScale;
        private System.Windows.Forms.Label TimeScaleLabel;
        private System.Windows.Forms.Label LabelTimeScaleValue;
        private System.Windows.Forms.TrackBar TrackBarUpdateFrequency;
        private System.Windows.Forms.Label LabelUpdateFrequency;
        private System.Windows.Forms.Label LabelUpdateFrequencyValue;
        private System.Windows.Forms.CheckBox CheckBoxMouseInteract;
    }
}