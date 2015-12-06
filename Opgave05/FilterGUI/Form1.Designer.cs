namespace FilterGUI
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.selectFilter = new System.Windows.Forms.ComboBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.imageFilterBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.imageFilterBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageFilterBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageFilterBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(18, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(356, 210);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(403, 16);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(90, 28);
            this.buttonLoad.TabIndex = 1;
            this.buttonLoad.Text = "Load Image";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // selectFilter
            // 
            this.selectFilter.FormattingEnabled = true;
            this.selectFilter.Items.AddRange(new object[] {
            "Original",
            "GreyScale",
            "Threshold",
            "Invert"});
            this.selectFilter.Location = new System.Drawing.Point(403, 50);
            this.selectFilter.Name = "selectFilter";
            this.selectFilter.Size = new System.Drawing.Size(90, 21);
            this.selectFilter.TabIndex = 2;
            this.selectFilter.Text = "Original";
            this.selectFilter.SelectedIndexChanged += new System.EventHandler(this.selectFilter_SelectedIndexChanged);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(18, 249);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(475, 14);
            this.progressBar1.TabIndex = 3;
            this.progressBar1.Click += new System.EventHandler(this.progressBar1_Click);
            // 
            // imageFilterBindingSource
            // 
            this.imageFilterBindingSource.DataSource = typeof(LogicImplementation.ImageFilter);
            // 
            // imageFilterBindingSource1
            // 
            this.imageFilterBindingSource1.DataSource = typeof(LogicImplementation.ImageFilter);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 282);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.selectFilter);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Filter Gui - Opgave 05";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageFilterBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageFilterBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.ComboBox selectFilter;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.BindingSource imageFilterBindingSource;
        private System.Windows.Forms.BindingSource imageFilterBindingSource1;
    }
}

