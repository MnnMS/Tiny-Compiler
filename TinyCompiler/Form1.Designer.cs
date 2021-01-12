namespace TinyCompiler
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
            this.srcCodeText = new System.Windows.Forms.TextBox();
            this.compileBtn = new System.Windows.Forms.Button();
            this.tokenTable = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clearBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.errorText = new System.Windows.Forms.TextBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.tokenTable)).BeginInit();
            this.SuspendLayout();
            // 
            // srcCodeText
            // 
            this.srcCodeText.Location = new System.Drawing.Point(11, 11);
            this.srcCodeText.Margin = new System.Windows.Forms.Padding(2);
            this.srcCodeText.Multiline = true;
            this.srcCodeText.Name = "srcCodeText";
            this.srcCodeText.Size = new System.Drawing.Size(263, 373);
            this.srcCodeText.TabIndex = 1;
            // 
            // compileBtn
            // 
            this.compileBtn.Location = new System.Drawing.Point(11, 394);
            this.compileBtn.Margin = new System.Windows.Forms.Padding(2);
            this.compileBtn.Name = "compileBtn";
            this.compileBtn.Size = new System.Drawing.Size(146, 45);
            this.compileBtn.TabIndex = 3;
            this.compileBtn.Text = "Compile !";
            this.compileBtn.UseVisualStyleBackColor = true;
            this.compileBtn.Click += new System.EventHandler(this.compileBtn_Click);
            // 
            // tokenTable
            // 
            this.tokenTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tokenTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.tokenTable.Location = new System.Drawing.Point(278, 9);
            this.tokenTable.Margin = new System.Windows.Forms.Padding(2);
            this.tokenTable.Name = "tokenTable";
            this.tokenTable.RowTemplate.Height = 24;
            this.tokenTable.Size = new System.Drawing.Size(248, 373);
            this.tokenTable.TabIndex = 4;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Lexeme";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Token Class";
            this.Column2.Name = "Column2";
            // 
            // clearBtn
            // 
            this.clearBtn.Location = new System.Drawing.Point(338, 389);
            this.clearBtn.Name = "clearBtn";
            this.clearBtn.Size = new System.Drawing.Size(120, 44);
            this.clearBtn.TabIndex = 7;
            this.clearBtn.Text = "Clear";
            this.clearBtn.UseVisualStyleBackColor = true;
            this.clearBtn.Click += new System.EventHandler(this.clearBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(531, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Error List";
            // 
            // errorText
            // 
            this.errorText.Location = new System.Drawing.Point(530, 26);
            this.errorText.Margin = new System.Windows.Forms.Padding(2);
            this.errorText.Multiline = true;
            this.errorText.Name = "errorText";
            this.errorText.Size = new System.Drawing.Size(183, 356);
            this.errorText.TabIndex = 9;
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(718, 12);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(454, 427);
            this.treeView1.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 466);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.errorText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.clearBtn);
            this.Controls.Add(this.tokenTable);
            this.Controls.Add(this.compileBtn);
            this.Controls.Add(this.srcCodeText);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tokenTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox srcCodeText;
        private System.Windows.Forms.Button compileBtn;
        private System.Windows.Forms.DataGridView tokenTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.Button clearBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox errorText;
        private System.Windows.Forms.TreeView treeView1;
    }
}

