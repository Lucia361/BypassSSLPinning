using System.Windows.Forms;

namespace BypassSSLPinning
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
            this.DeviceBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ReloadBtn = new System.Windows.Forms.Button();
            this.PatchBtn = new System.Windows.Forms.Button();
            this.LogsBox = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SearchBox = new HintTextBox();
            this.SelectedAppBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select the device";
            // 
            // DeviceBox
            // 
            this.DeviceBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DeviceBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeviceBox.FormattingEnabled = true;
            this.DeviceBox.Location = new System.Drawing.Point(16, 33);
            this.DeviceBox.Name = "DeviceBox";
            this.DeviceBox.Size = new System.Drawing.Size(450, 25);
            this.DeviceBox.TabIndex = 1;
            this.DeviceBox.SelectedIndexChanged += new System.EventHandler(this.ReloadCacheList);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Search app by package ID";
            // 
            // ReloadBtn
            // 
            this.ReloadBtn.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReloadBtn.Location = new System.Drawing.Point(472, 33);
            this.ReloadBtn.Name = "ReloadBtn";
            this.ReloadBtn.Size = new System.Drawing.Size(100, 25);
            this.ReloadBtn.TabIndex = 4;
            this.ReloadBtn.Text = "Reload";
            this.ReloadBtn.UseVisualStyleBackColor = true;
            this.ReloadBtn.Click += new System.EventHandler(this.HandleReloadDevices);
            // 
            // PatchBtn
            // 
            this.PatchBtn.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PatchBtn.Location = new System.Drawing.Point(225, 134);
            this.PatchBtn.Name = "PatchBtn";
            this.PatchBtn.Size = new System.Drawing.Size(135, 37);
            this.PatchBtn.TabIndex = 5;
            this.PatchBtn.Text = "Patch!";
            this.PatchBtn.UseVisualStyleBackColor = true;
            this.PatchBtn.Click += new System.EventHandler(this.HandlePatch);
            // 
            // LogsBox
            // 
            this.LogsBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogsBox.Location = new System.Drawing.Point(16, 200);
            this.LogsBox.Name = "LogsBox";
            this.LogsBox.ReadOnly = true;
            this.LogsBox.Size = new System.Drawing.Size(556, 149);
            this.LogsBox.TabIndex = 6;
            this.LogsBox.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(15, 176);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Logs";
            // 
            // SearchBox
            // 
            this.SearchBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SearchBox.Location = new System.Drawing.Point(18, 95);
            this.SearchBox.Name = "SearchBox";
            this.SearchBox.Size = new System.Drawing.Size(310, 25);
            this.SearchBox.TabIndex = 8;
            this.SearchBox.Hint = "Type here to search";
            this.SearchBox.TextChanged += new System.EventHandler(this.HandleSearch);
            // 
            // SelectedAppBox
            // 
            this.SelectedAppBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectedAppBox.Location = new System.Drawing.Point(334, 95);
            this.SelectedAppBox.Name = "SelectedAppBox";
            this.SelectedAppBox.ReadOnly = true;
            this.SelectedAppBox.Size = new System.Drawing.Size(238, 25);
            this.SelectedAppBox.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.SelectedAppBox);
            this.Controls.Add(this.SearchBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LogsBox);
            this.Controls.Add(this.PatchBtn);
            this.Controls.Add(this.ReloadBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DeviceBox);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Form1";
            this.Text = "Bypass SSL Pinning";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox DeviceBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ReloadBtn;
        private System.Windows.Forms.Button PatchBtn;
        private System.Windows.Forms.RichTextBox LogsBox;
        private System.Windows.Forms.Label label3;
        private HintTextBox SearchBox;
        private TextBox SelectedAppBox;
    }
}

