namespace _006Redis中的list使用
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
            this.btnLeftPush = new System.Windows.Forms.Button();
            this.btnRightPush = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLeftPop = new System.Windows.Forms.Button();
            this.btnRightPop = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnLeftPush
            // 
            this.btnLeftPush.Location = new System.Drawing.Point(39, 71);
            this.btnLeftPush.Name = "btnLeftPush";
            this.btnLeftPush.Size = new System.Drawing.Size(75, 23);
            this.btnLeftPush.TabIndex = 0;
            this.btnLeftPush.Text = "从左侧进入";
            this.btnLeftPush.UseVisualStyleBackColor = true;
            this.btnLeftPush.Click += new System.EventHandler(this.btnLeftPush_Click);
            // 
            // btnRightPush
            // 
            this.btnRightPush.Location = new System.Drawing.Point(150, 71);
            this.btnRightPush.Name = "btnRightPush";
            this.btnRightPush.Size = new System.Drawing.Size(75, 23);
            this.btnRightPush.TabIndex = 1;
            this.btnRightPush.Text = "从右侧进入";
            this.btnRightPush.UseVisualStyleBackColor = true;
            this.btnRightPush.Click += new System.EventHandler(this.btnRightPush_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(78, 176);
            this.listBox1.Name = "listBox1";
            this.listBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.listBox1.Size = new System.Drawing.Size(120, 172);
            this.listBox1.TabIndex = 2;
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(156, 28);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(100, 21);
            this.txtInput.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "输入进入队列的数据";
            // 
            // btnLeftPop
            // 
            this.btnLeftPop.Location = new System.Drawing.Point(39, 116);
            this.btnLeftPop.Name = "btnLeftPop";
            this.btnLeftPop.Size = new System.Drawing.Size(75, 23);
            this.btnLeftPop.TabIndex = 5;
            this.btnLeftPop.Text = "从左侧弹出";
            this.btnLeftPop.UseVisualStyleBackColor = true;
            this.btnLeftPop.Click += new System.EventHandler(this.btnLeftPop_Click);
            // 
            // btnRightPop
            // 
            this.btnRightPop.Location = new System.Drawing.Point(150, 115);
            this.btnRightPop.Name = "btnRightPop";
            this.btnRightPop.Size = new System.Drawing.Size(75, 23);
            this.btnRightPop.TabIndex = 6;
            this.btnRightPop.Text = "从右侧弹出";
            this.btnRightPop.UseVisualStyleBackColor = true;
            this.btnRightPop.Click += new System.EventHandler(this.btnRightPop_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "左";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 335);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "右";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 360);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnRightPop);
            this.Controls.Add(this.btnLeftPop);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.btnRightPush);
            this.Controls.Add(this.btnLeftPush);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLeftPush;
        private System.Windows.Forms.Button btnRightPush;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLeftPop;
        private System.Windows.Forms.Button btnRightPop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

