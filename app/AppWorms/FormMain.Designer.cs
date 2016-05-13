namespace AppWorms
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelProperty = new System.Windows.Forms.Panel();
            this.glControl = new OpenTK.GLControl();
            this.projectUserControl = new AppWorms.ProjectUserControl();
            this.assetsUserControl = new AppWorms.AssetsUserControl();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.panelProperty.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelProperty
            // 
            this.panelProperty.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelProperty.AutoScroll = true;
            this.panelProperty.Controls.Add(this.flowLayoutPanel);
            this.panelProperty.Location = new System.Drawing.Point(369, 0);
            this.panelProperty.Name = "panelProperty";
            this.panelProperty.Size = new System.Drawing.Size(161, 183);
            this.panelProperty.TabIndex = 1;
            // 
            // glControl
            // 
            this.glControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.glControl.BackColor = System.Drawing.Color.Black;
            this.glControl.Location = new System.Drawing.Point(-2, 0);
            this.glControl.Name = "glControl";
            this.glControl.Size = new System.Drawing.Size(365, 183);
            this.glControl.TabIndex = 2;
            this.glControl.VSync = false;
            // 
            // projectUserControl
            // 
            this.projectUserControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.projectUserControl.Location = new System.Drawing.Point(-2, 190);
            this.projectUserControl.Name = "projectUserControl";
            this.projectUserControl.Size = new System.Drawing.Size(263, 100);
            this.projectUserControl.TabIndex = 4;
            this.projectUserControl.OnActiveGameObjectChanged += new System.EventHandler(this.projectUserControl_OnActiveGameObjectChanged);
            // 
            // assetsUserControl
            // 
            this.assetsUserControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.assetsUserControl.Location = new System.Drawing.Point(267, 189);
            this.assetsUserControl.Name = "assetsUserControl";
            this.assetsUserControl.Size = new System.Drawing.Size(263, 101);
            this.assetsUserControl.TabIndex = 3;
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.AutoScroll = true;
            this.flowLayoutPanel.AutoSize = true;
            this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(161, 183);
            this.flowLayoutPanel.TabIndex = 0;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 292);
            this.Controls.Add(this.projectUserControl);
            this.Controls.Add(this.assetsUserControl);
            this.Controls.Add(this.glControl);
            this.Controls.Add(this.panelProperty);
            this.Name = "FormMain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.panelProperty.ResumeLayout(false);
            this.panelProperty.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelProperty;
        private AssetsUserControl assetsUserControl;
        private ProjectUserControl projectUserControl;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
    }
}

