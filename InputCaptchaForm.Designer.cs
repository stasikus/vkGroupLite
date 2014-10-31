namespace vkGroupWall
{
    partial class InputCaptchaForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputCaptchaForm));
            this.captcha_input_pic = new System.Windows.Forms.PictureBox();
            this.captcha_input_TB = new System.Windows.Forms.TextBox();
            this.captcha_input_btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.captcha_input_pic)).BeginInit();
            this.SuspendLayout();
            // 
            // captcha_input_pic
            // 
            this.captcha_input_pic.Location = new System.Drawing.Point(12, 12);
            this.captcha_input_pic.Name = "captcha_input_pic";
            this.captcha_input_pic.Size = new System.Drawing.Size(125, 50);
            this.captcha_input_pic.TabIndex = 0;
            this.captcha_input_pic.TabStop = false;
            // 
            // captcha_input_TB
            // 
            this.captcha_input_TB.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.captcha_input_TB.Location = new System.Drawing.Point(12, 68);
            this.captcha_input_TB.Name = "captcha_input_TB";
            this.captcha_input_TB.Size = new System.Drawing.Size(125, 22);
            this.captcha_input_TB.TabIndex = 1;
            // 
            // captcha_input_btn
            // 
            this.captcha_input_btn.BackColor = System.Drawing.Color.DarkGray;
            this.captcha_input_btn.Location = new System.Drawing.Point(36, 94);
            this.captcha_input_btn.Name = "captcha_input_btn";
            this.captcha_input_btn.Size = new System.Drawing.Size(75, 23);
            this.captcha_input_btn.TabIndex = 2;
            this.captcha_input_btn.Text = "OK";
            this.captcha_input_btn.UseVisualStyleBackColor = false;
            this.captcha_input_btn.Click += new System.EventHandler(this.captcha_input_btn_Click);
            // 
            // InputCaptchaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GrayText;
            this.ClientSize = new System.Drawing.Size(149, 122);
            this.Controls.Add(this.captcha_input_btn);
            this.Controls.Add(this.captcha_input_TB);
            this.Controls.Add(this.captcha_input_pic);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(165, 160);
            this.MinimumSize = new System.Drawing.Size(165, 160);
            this.Name = "InputCaptchaForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            ((System.ComponentModel.ISupportInitialize)(this.captcha_input_pic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox captcha_input_pic;
        private System.Windows.Forms.TextBox captcha_input_TB;
        private System.Windows.Forms.Button captcha_input_btn;
    }
}