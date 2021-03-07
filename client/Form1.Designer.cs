namespace client
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_ip = new System.Windows.Forms.TextBox();
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.button_connect = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.button_disconnect = new System.Windows.Forms.Button();
            this.logs = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button_send = new System.Windows.Forms.Button();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.fileSystemWatcher2 = new System.IO.FileSystemWatcher();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.button_browse = new System.Windows.Forms.Button();
            this.button_download = new System.Windows.Forms.Button();
            this.textBox_delete = new System.Windows.Forms.TextBox();
            this.textBox_path = new System.Windows.Forms.TextBox();
            this.button_browse2 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.button_request = new System.Windows.Forms.Button();
            this.button_copy = new System.Windows.Forms.Button();
            this.textBox_download = new System.Windows.Forms.TextBox();
            this.textBox_copy = new System.Windows.Forms.TextBox();
            this.button_delete = new System.Windows.Forms.Button();
            this.textBox_public = new System.Windows.Forms.TextBox();
            this.button_public = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port:";
            // 
            // textBox_ip
            // 
            this.textBox_ip.Location = new System.Drawing.Point(81, 61);
            this.textBox_ip.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_ip.Name = "textBox_ip";
            this.textBox_ip.Size = new System.Drawing.Size(219, 26);
            this.textBox_ip.TabIndex = 2;
            // 
            // textBox_port
            // 
            this.textBox_port.Location = new System.Drawing.Point(81, 98);
            this.textBox_port.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(219, 26);
            this.textBox_port.TabIndex = 3;
            // 
            // button_connect
            // 
            this.button_connect.Location = new System.Drawing.Point(346, 45);
            this.button_connect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(124, 52);
            this.button_connect.TabIndex = 4;
            this.button_connect.Text = "Connect";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Name:";
            // 
            // textBox_name
            // 
            this.textBox_name.Location = new System.Drawing.Point(81, 141);
            this.textBox_name.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(219, 26);
            this.textBox_name.TabIndex = 6;
            // 
            // button_disconnect
            // 
            this.button_disconnect.Enabled = false;
            this.button_disconnect.Location = new System.Drawing.Point(346, 121);
            this.button_disconnect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_disconnect.Name = "button_disconnect";
            this.button_disconnect.Size = new System.Drawing.Size(124, 60);
            this.button_disconnect.TabIndex = 7;
            this.button_disconnect.Text = "Disconnect";
            this.button_disconnect.UseVisualStyleBackColor = true;
            this.button_disconnect.Click += new System.EventHandler(this.button_disconnect_Click);
            // 
            // logs
            // 
            this.logs.Enabled = false;
            this.logs.Location = new System.Drawing.Point(61, 268);
            this.logs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.logs.Name = "logs";
            this.logs.Size = new System.Drawing.Size(408, 133);
            this.logs.TabIndex = 8;
            this.logs.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 244);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Server Logs:";
            // 
            // button_send
            // 
            this.button_send.Enabled = false;
            this.button_send.Location = new System.Drawing.Point(540, 450);
            this.button_send.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(117, 42);
            this.button_send.TabIndex = 11;
            this.button_send.Text = "Send";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button_send_Click);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // fileSystemWatcher2
            // 
            this.fileSystemWatcher2.EnableRaisingEvents = true;
            this.fileSystemWatcher2.SynchronizingObject = this;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtFile
            // 
            this.txtFile.Enabled = false;
            this.txtFile.Location = new System.Drawing.Point(71, 458);
            this.txtFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(274, 26);
            this.txtFile.TabIndex = 13;
            // 
            // button_browse
            // 
            this.button_browse.Enabled = false;
            this.button_browse.Location = new System.Drawing.Point(374, 451);
            this.button_browse.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_browse.Name = "button_browse";
            this.button_browse.Size = new System.Drawing.Size(126, 41);
            this.button_browse.TabIndex = 14;
            this.button_browse.Text = "Browse";
            this.button_browse.UseVisualStyleBackColor = true;
            this.button_browse.Click += new System.EventHandler(this.button_browse_Click);
            // 
            // button_download
            // 
            this.button_download.Enabled = false;
            this.button_download.Location = new System.Drawing.Point(374, 499);
            this.button_download.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_download.Name = "button_download";
            this.button_download.Size = new System.Drawing.Size(126, 34);
            this.button_download.TabIndex = 15;
            this.button_download.Text = "download";
            this.button_download.UseVisualStyleBackColor = true;
            this.button_download.Click += new System.EventHandler(this.button_download_Click);
            // 
            // textBox_delete
            // 
            this.textBox_delete.Location = new System.Drawing.Point(71, 589);
            this.textBox_delete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_delete.Name = "textBox_delete";
            this.textBox_delete.Size = new System.Drawing.Size(274, 26);
            this.textBox_delete.TabIndex = 16;
            // 
            // textBox_path
            // 
            this.textBox_path.Location = new System.Drawing.Point(81, 196);
            this.textBox_path.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_path.Name = "textBox_path";
            this.textBox_path.Size = new System.Drawing.Size(219, 26);
            this.textBox_path.TabIndex = 17;
            // 
            // button_browse2
            // 
            this.button_browse2.Location = new System.Drawing.Point(346, 196);
            this.button_browse2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_browse2.Name = "button_browse2";
            this.button_browse2.Size = new System.Drawing.Size(123, 50);
            this.button_browse2.TabIndex = 18;
            this.button_browse2.Text = "browse";
            this.button_browse2.UseVisualStyleBackColor = true;
            this.button_browse2.Click += new System.EventHandler(this.button_browse2_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 196);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 20);
            this.label5.TabIndex = 19;
            this.label5.Text = "Path";
            // 
            // button_request
            // 
            this.button_request.Location = new System.Drawing.Point(511, 268);
            this.button_request.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_request.Name = "button_request";
            this.button_request.Size = new System.Drawing.Size(126, 51);
            this.button_request.TabIndex = 20;
            this.button_request.Text = "request";
            this.button_request.UseVisualStyleBackColor = true;
            this.button_request.Click += new System.EventHandler(this.button_request_Click);
            // 
            // button_copy
            // 
            this.button_copy.Location = new System.Drawing.Point(374, 540);
            this.button_copy.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_copy.Name = "button_copy";
            this.button_copy.Size = new System.Drawing.Size(126, 36);
            this.button_copy.TabIndex = 21;
            this.button_copy.Text = "Copy";
            this.button_copy.UseVisualStyleBackColor = true;
            this.button_copy.Click += new System.EventHandler(this.button_copy_Click);
            // 
            // textBox_download
            // 
            this.textBox_download.Enabled = false;
            this.textBox_download.Location = new System.Drawing.Point(71, 501);
            this.textBox_download.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_download.Name = "textBox_download";
            this.textBox_download.Size = new System.Drawing.Size(274, 26);
            this.textBox_download.TabIndex = 22;
            // 
            // textBox_copy
            // 
            this.textBox_copy.Location = new System.Drawing.Point(71, 544);
            this.textBox_copy.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_copy.Name = "textBox_copy";
            this.textBox_copy.Size = new System.Drawing.Size(274, 26);
            this.textBox_copy.TabIndex = 23;
            // 
            // button_delete
            // 
            this.button_delete.Location = new System.Drawing.Point(374, 584);
            this.button_delete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(126, 38);
            this.button_delete.TabIndex = 24;
            this.button_delete.Text = "Delete";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            // 
            // textBox_public
            // 
            this.textBox_public.Location = new System.Drawing.Point(71, 630);
            this.textBox_public.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_public.Name = "textBox_public";
            this.textBox_public.Size = new System.Drawing.Size(274, 26);
            this.textBox_public.TabIndex = 25;
            // 
            // button_public
            // 
            this.button_public.Location = new System.Drawing.Point(374, 630);
            this.button_public.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_public.Name = "button_public";
            this.button_public.Size = new System.Drawing.Size(126, 29);
            this.button_public.TabIndex = 26;
            this.button_public.Text = "public";
            this.button_public.UseVisualStyleBackColor = true;
            this.button_public.Click += new System.EventHandler(this.button_public_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 674);
            this.Controls.Add(this.button_public);
            this.Controls.Add(this.textBox_public);
            this.Controls.Add(this.button_delete);
            this.Controls.Add(this.textBox_copy);
            this.Controls.Add(this.textBox_download);
            this.Controls.Add(this.button_copy);
            this.Controls.Add(this.button_request);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button_browse2);
            this.Controls.Add(this.textBox_path);
            this.Controls.Add(this.textBox_delete);
            this.Controls.Add(this.button_download);
            this.Controls.Add(this.button_browse);
            this.Controls.Add(this.txtFile);
            this.Controls.Add(this.button_send);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.logs);
            this.Controls.Add(this.button_disconnect);
            this.Controls.Add(this.textBox_name);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_connect);
            this.Controls.Add(this.textBox_port);
            this.Controls.Add(this.textBox_ip);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Client";
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_ip;
        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.Button button_connect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Button button_disconnect;
        private System.Windows.Forms.RichTextBox logs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_send;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.IO.FileSystemWatcher fileSystemWatcher2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button_browse;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.TextBox textBox_copy;
        private System.Windows.Forms.TextBox textBox_download;
        private System.Windows.Forms.Button button_copy;
        private System.Windows.Forms.Button button_request;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button_browse2;
        private System.Windows.Forms.TextBox textBox_path;
        private System.Windows.Forms.TextBox textBox_delete;
        private System.Windows.Forms.Button button_download;
        private System.Windows.Forms.Button button_delete;
        private System.Windows.Forms.Button button_public;
        private System.Windows.Forms.TextBox textBox_public;
    }
}

