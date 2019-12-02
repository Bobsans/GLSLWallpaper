using System.ComponentModel;

namespace GLSLWallpapers.Components {
    partial class ShaderListItem {
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
            this.ButtonApply = new System.Windows.Forms.Button();
            this.PictureBoxPreview = new System.Windows.Forms.PictureBox();
            this.LabelTitle = new System.Windows.Forms.Label();
            this.LabelAuthor = new System.Windows.Forms.Label();
            this.LabelFile = new System.Windows.Forms.Label();
            this.LabelAuthorValue = new System.Windows.Forms.Label();
            this.LabelFileValue = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxPreview)).BeginInit();
            this.SuspendLayout();
            //
            // ButtonApply
            //
            this.ButtonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonApply.Location = new System.Drawing.Point(427, 57);
            this.ButtonApply.Name = "ButtonApply";
            this.ButtonApply.Size = new System.Drawing.Size(73, 24);
            this.ButtonApply.TabIndex = 0;
            this.ButtonApply.Text = "Apply";
            this.ButtonApply.UseVisualStyleBackColor = true;
            this.ButtonApply.Click += new System.EventHandler(this.ButtonApply_Click);
            //
            // PictureBoxPreview
            //
            this.PictureBoxPreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PictureBoxPreview.ErrorImage = null;
            this.PictureBoxPreview.InitialImage = null;
            this.PictureBoxPreview.Location = new System.Drawing.Point(0, 0);
            this.PictureBoxPreview.Name = "PictureBoxPreview";
            this.PictureBoxPreview.Size = new System.Drawing.Size(142, 80);
            this.PictureBoxPreview.TabIndex = 1;
            this.PictureBoxPreview.TabStop = false;
            //
            // LabelTitle
            //
            this.LabelTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.LabelTitle.Location = new System.Drawing.Point(148, 0);
            this.LabelTitle.Name = "LabelTitle";
            this.LabelTitle.Size = new System.Drawing.Size(352, 23);
            this.LabelTitle.TabIndex = 2;
            this.LabelTitle.Text = "Title";
            //
            // LabelAuthor
            //
            this.LabelAuthor.ForeColor = System.Drawing.Color.Gray;
            this.LabelAuthor.Location = new System.Drawing.Point(148, 23);
            this.LabelAuthor.Name = "LabelAuthor";
            this.LabelAuthor.Size = new System.Drawing.Size(51, 15);
            this.LabelAuthor.TabIndex = 3;
            this.LabelAuthor.Text = "Author:";
            //
            // LabelFile
            //
            this.LabelFile.ForeColor = System.Drawing.Color.Gray;
            this.LabelFile.Location = new System.Drawing.Point(148, 38);
            this.LabelFile.Name = "LabelFile";
            this.LabelFile.Size = new System.Drawing.Size(51, 15);
            this.LabelFile.TabIndex = 3;
            this.LabelFile.Text = "File:";
            //
            // LabelAuthorValue
            //
            this.LabelAuthorValue.ForeColor = System.Drawing.Color.Gray;
            this.LabelAuthorValue.Location = new System.Drawing.Point(194, 23);
            this.LabelAuthorValue.Name = "LabelAuthorValue";
            this.LabelAuthorValue.Size = new System.Drawing.Size(227, 15);
            this.LabelAuthorValue.TabIndex = 3;
            this.LabelAuthorValue.Text = "author";
            //
            // LabelFileValue
            //
            this.LabelFileValue.ForeColor = System.Drawing.Color.Gray;
            this.LabelFileValue.Location = new System.Drawing.Point(194, 38);
            this.LabelFileValue.Name = "LabelFileValue";
            this.LabelFileValue.Size = new System.Drawing.Size(227, 15);
            this.LabelFileValue.TabIndex = 3;
            this.LabelFileValue.Text = "file.ext";
            //
            // ShaderListItem
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LabelFileValue);
            this.Controls.Add(this.LabelFile);
            this.Controls.Add(this.LabelAuthorValue);
            this.Controls.Add(this.LabelAuthor);
            this.Controls.Add(this.LabelTitle);
            this.Controls.Add(this.PictureBoxPreview);
            this.Controls.Add(this.ButtonApply);
            this.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.Name = "ShaderListItem";
            this.Size = new System.Drawing.Size(500, 84);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxPreview)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label LabelTitle;
        private System.Windows.Forms.PictureBox PictureBoxPreview;
        private System.Windows.Forms.Label LabelFile;
        private System.Windows.Forms.Label LabelAuthor;
        private System.Windows.Forms.Button ButtonApply;
        private System.Windows.Forms.Label LabelFileValue;
        private System.Windows.Forms.Label LabelAuthorValue;
    }
}
