namespace server_proj408
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
            this.label_path = new System.Windows.Forms.Label();
            this.label_port = new System.Windows.Forms.Label();
            this.logs = new System.Windows.Forms.RichTextBox();
            this.textBox_path = new System.Windows.Forms.TextBox();
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.button_browse = new System.Windows.Forms.Button();
            this.button_listen = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_path
            // 
            this.label_path.AutoSize = true;
            this.label_path.Location = new System.Drawing.Point(68, 54);
            this.label_path.Name = "label_path";
            this.label_path.Size = new System.Drawing.Size(62, 17);
            this.label_path.TabIndex = 0;
            this.label_path.Text = "File path";
            // 
            // label_port
            // 
            this.label_port.AutoSize = true;
            this.label_port.Location = new System.Drawing.Point(68, 91);
            this.label_port.Name = "label_port";
            this.label_port.Size = new System.Drawing.Size(33, 17);
            this.label_port.TabIndex = 1;
            this.label_port.Text = "port";
            // 
            // logs
            // 
            this.logs.Location = new System.Drawing.Point(57, 141);
            this.logs.Name = "logs";
            this.logs.Size = new System.Drawing.Size(324, 174);
            this.logs.TabIndex = 2;
            this.logs.Text = "";
            // 
            // textBox_path
            // 
            this.textBox_path.Location = new System.Drawing.Point(170, 48);
            this.textBox_path.Name = "textBox_path";
            this.textBox_path.Size = new System.Drawing.Size(134, 22);
            this.textBox_path.TabIndex = 3;
            // 
            // textBox_port
            // 
            this.textBox_port.Location = new System.Drawing.Point(170, 91);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(134, 22);
            this.textBox_port.TabIndex = 4;
            // 
            // button_browse
            // 
            this.button_browse.Location = new System.Drawing.Point(344, 47);
            this.button_browse.Name = "button_browse";
            this.button_browse.Size = new System.Drawing.Size(75, 23);
            this.button_browse.TabIndex = 5;
            this.button_browse.Text = "browse";
            this.button_browse.UseVisualStyleBackColor = true;
            this.button_browse.Click += new System.EventHandler(this.button_browse_Click);
            // 
            // button_listen
            // 
            this.button_listen.Enabled = false;
            this.button_listen.Location = new System.Drawing.Point(344, 91);
            this.button_listen.Name = "button_listen";
            this.button_listen.Size = new System.Drawing.Size(75, 23);
            this.button_listen.TabIndex = 6;
            this.button_listen.Text = "listen";
            this.button_listen.UseVisualStyleBackColor = true;
            this.button_listen.Click += new System.EventHandler(this.button_listen_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 369);
            this.Controls.Add(this.button_listen);
            this.Controls.Add(this.button_browse);
            this.Controls.Add(this.textBox_port);
            this.Controls.Add(this.textBox_path);
            this.Controls.Add(this.logs);
            this.Controls.Add(this.label_port);
            this.Controls.Add(this.label_path);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_path;
        private System.Windows.Forms.Label label_port;
        private System.Windows.Forms.RichTextBox logs;
        private System.Windows.Forms.TextBox textBox_path;
        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.Button button_browse;
        private System.Windows.Forms.Button button_listen;
    }
}

