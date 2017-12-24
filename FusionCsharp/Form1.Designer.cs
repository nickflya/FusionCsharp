namespace FusionCsharp
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel_control = new System.Windows.Forms.Panel();
            this.button_fusion = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button_colordata_camera_bmp = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox_colordata_camera_bmp = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel_draw = new System.Windows.Forms.Panel();
            this.videoSourcePlayer = new AForge.Controls.VideoSourcePlayer();
            this.openFileDialog_colordata_camera_bmp = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel_control.SuspendLayout();
            this.panel_draw.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel_control);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel_draw);
            this.splitContainer1.Size = new System.Drawing.Size(1284, 761);
            this.splitContainer1.SplitterDistance = 304;
            this.splitContainer1.TabIndex = 0;
            // 
            // panel_control
            // 
            this.panel_control.Controls.Add(this.button_fusion);
            this.panel_control.Controls.Add(this.button4);
            this.panel_control.Controls.Add(this.button_colordata_camera_bmp);
            this.panel_control.Controls.Add(this.button2);
            this.panel_control.Controls.Add(this.textBox4);
            this.panel_control.Controls.Add(this.button1);
            this.panel_control.Controls.Add(this.textBox_colordata_camera_bmp);
            this.panel_control.Controls.Add(this.textBox2);
            this.panel_control.Controls.Add(this.textBox1);
            this.panel_control.Controls.Add(this.label4);
            this.panel_control.Controls.Add(this.label3);
            this.panel_control.Controls.Add(this.label2);
            this.panel_control.Controls.Add(this.label1);
            this.panel_control.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_control.Location = new System.Drawing.Point(0, 0);
            this.panel_control.Name = "panel_control";
            this.panel_control.Size = new System.Drawing.Size(304, 761);
            this.panel_control.TabIndex = 6;
            // 
            // button_fusion
            // 
            this.button_fusion.Location = new System.Drawing.Point(92, 372);
            this.button_fusion.Name = "button_fusion";
            this.button_fusion.Size = new System.Drawing.Size(92, 23);
            this.button_fusion.TabIndex = 14;
            this.button_fusion.Text = "融合";
            this.button_fusion.UseVisualStyleBackColor = true;
            this.button_fusion.Click += new System.EventHandler(this.button_fusion_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(247, 291);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(24, 23);
            this.button4.TabIndex = 15;
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button_colordata_camera_bmp
            // 
            this.button_colordata_camera_bmp.Location = new System.Drawing.Point(247, 208);
            this.button_colordata_camera_bmp.Name = "button_colordata_camera_bmp";
            this.button_colordata_camera_bmp.Size = new System.Drawing.Size(24, 23);
            this.button_colordata_camera_bmp.TabIndex = 16;
            this.button_colordata_camera_bmp.UseVisualStyleBackColor = true;
            this.button_colordata_camera_bmp.Click += new System.EventHandler(this.button_colordata_camera_bmp_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(247, 137);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(24, 23);
            this.button2.TabIndex = 17;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(33, 293);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(208, 21);
            this.textBox4.TabIndex = 10;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(247, 64);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 23);
            this.button1.TabIndex = 18;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textBox_colordata_camera_bmp
            // 
            this.textBox_colordata_camera_bmp.Location = new System.Drawing.Point(33, 210);
            this.textBox_colordata_camera_bmp.Name = "textBox_colordata_camera_bmp";
            this.textBox_colordata_camera_bmp.ReadOnly = true;
            this.textBox_colordata_camera_bmp.Size = new System.Drawing.Size(208, 21);
            this.textBox_colordata_camera_bmp.TabIndex = 11;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(33, 137);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(208, 21);
            this.textBox2.TabIndex = 12;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(33, 66);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(208, 21);
            this.textBox1.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 258);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "摄像机深度图";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 177);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "摄像机颜色图";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "主视图深度图";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "主视图背景图";
            // 
            // panel_draw
            // 
            this.panel_draw.Controls.Add(this.videoSourcePlayer);
            this.panel_draw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_draw.Location = new System.Drawing.Point(0, 0);
            this.panel_draw.Name = "panel_draw";
            this.panel_draw.Size = new System.Drawing.Size(976, 761);
            this.panel_draw.TabIndex = 0;
            this.panel_draw.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_draw_Paint);
            // 
            // videoSourcePlayer
            // 
            this.videoSourcePlayer.AutoSizeControl = true;
            this.videoSourcePlayer.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.videoSourcePlayer.ForeColor = System.Drawing.Color.White;
            this.videoSourcePlayer.Location = new System.Drawing.Point(327, 259);
            this.videoSourcePlayer.Name = "videoSourcePlayer";
            this.videoSourcePlayer.Size = new System.Drawing.Size(322, 242);
            this.videoSourcePlayer.TabIndex = 1;
            this.videoSourcePlayer.VideoSource = null;
            this.videoSourcePlayer.Visible = false;
            // 
            // openFileDialog_colordata_camera_bmp
            // 
            this.openFileDialog_colordata_camera_bmp.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 761);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel_control.ResumeLayout(false);
            this.panel_control.PerformLayout();
            this.panel_draw.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel_control;
        private System.Windows.Forms.Button button_fusion;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button_colordata_camera_bmp;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox_colordata_camera_bmp;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog_colordata_camera_bmp;
        public System.Windows.Forms.Panel panel_draw;
        public AForge.Controls.VideoSourcePlayer videoSourcePlayer;
    }
}

