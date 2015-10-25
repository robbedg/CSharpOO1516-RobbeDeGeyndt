namespace Opgave_3b
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
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.buttonRecalculate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericHeightstep = new System.Windows.Forms.NumericUpDown();
            this.numericIterations = new System.Windows.Forms.NumericUpDown();
            this.textNaam = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericHeightstep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericIterations)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(12, 47);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(715, 374);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // buttonRecalculate
            // 
            this.buttonRecalculate.Location = new System.Drawing.Point(736, 386);
            this.buttonRecalculate.Name = "buttonRecalculate";
            this.buttonRecalculate.Size = new System.Drawing.Size(155, 35);
            this.buttonRecalculate.TabIndex = 1;
            this.buttonRecalculate.Text = "Recalculate";
            this.buttonRecalculate.UseVisualStyleBackColor = true;
            this.buttonRecalculate.Click += new System.EventHandler(this.buttonRecalculate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(733, 336);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Heightstep:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(741, 362);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Iterations:";
            // 
            // numericHeightstep
            // 
            this.numericHeightstep.Location = new System.Drawing.Point(800, 334);
            this.numericHeightstep.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.numericHeightstep.Name = "numericHeightstep";
            this.numericHeightstep.Size = new System.Drawing.Size(91, 20);
            this.numericHeightstep.TabIndex = 4;
            this.numericHeightstep.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // numericIterations
            // 
            this.numericIterations.Location = new System.Drawing.Point(800, 360);
            this.numericIterations.Name = "numericIterations";
            this.numericIterations.Size = new System.Drawing.Size(91, 20);
            this.numericIterations.TabIndex = 5;
            this.numericIterations.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numericIterations.ValueChanged += new System.EventHandler(this.numericIterations_ValueChanged);
            // 
            // textNaam
            // 
            this.textNaam.Location = new System.Drawing.Point(736, 63);
            this.textNaam.Name = "textNaam";
            this.textNaam.Size = new System.Drawing.Size(155, 20);
            this.textNaam.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(733, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Name:";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(736, 92);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(154, 33);
            this.buttonSave.TabIndex = 8;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(737, 131);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(154, 33);
            this.buttonLoad.TabIndex = 9;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 432);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textNaam);
            this.Controls.Add(this.numericIterations);
            this.Controls.Add(this.numericHeightstep);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonRecalculate);
            this.Controls.Add(this.pictureBox);
            this.Name = "Form1";
            this.Text = "Landscape generator";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericHeightstep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericIterations)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button buttonRecalculate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericHeightstep;
        private System.Windows.Forms.NumericUpDown numericIterations;
        private System.Windows.Forms.TextBox textNaam;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonLoad;
    }
}

