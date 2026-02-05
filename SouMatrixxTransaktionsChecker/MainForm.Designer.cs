namespace Transparentdesign.SouMatrixxTransaktionsChecker
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            buttonFindSuspiciousTransactions = new Button();
            textBoxTransactionDirectory = new TextBox();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            listViewOldFiles = new ListView();
            buttonFindTransactionDirectory = new Button();
            textBoxLog = new TextBox();
            label1 = new Label();
            labelOlderNullFilledFiles = new Label();
            listViewOldFilesFilledWithNulls = new ListView();
            labelLog = new Label();
            labelTransactionFilesRoot = new Label();
            toolTipChooseDirectory = new ToolTip(components);
            buttonDeleteSelectedOldFiles = new Button();
            buttonDeleteSelectedOldNULLFiles = new Button();
            SuspendLayout();
            // 
            // buttonFindSuspiciousTransactions
            // 
            buttonFindSuspiciousTransactions.Location = new Point(12, 41);
            buttonFindSuspiciousTransactions.Name = "buttonFindSuspiciousTransactions";
            buttonFindSuspiciousTransactions.Size = new Size(386, 39);
            buttonFindSuspiciousTransactions.TabIndex = 0;
            buttonFindSuspiciousTransactions.Text = "### Find Suspicious Transactions ###";
            buttonFindSuspiciousTransactions.UseVisualStyleBackColor = true;
            buttonFindSuspiciousTransactions.Click += buttonFindSuspiciousTransactions_Click;
            // 
            // textBoxTransactionDirectory
            // 
            textBoxTransactionDirectory.Location = new Point(143, 12);
            textBoxTransactionDirectory.Name = "textBoxTransactionDirectory";
            textBoxTransactionDirectory.Size = new Size(491, 23);
            textBoxTransactionDirectory.TabIndex = 1;
            textBoxTransactionDirectory.TextChanged += textBoxTransactionDirectory_TextChanged;
            // 
            // listViewOldFiles
            // 
            listViewOldFiles.Location = new Point(12, 101);
            listViewOldFiles.Name = "listViewOldFiles";
            listViewOldFiles.Size = new Size(1240, 202);
            listViewOldFiles.TabIndex = 2;
            listViewOldFiles.UseCompatibleStateImageBehavior = false;
            listViewOldFiles.View = View.Details;
            listViewOldFiles.SelectedIndexChanged += listViewOldFiles_SelectedIndexChanged;
            // 
            // buttonFindTransactionDirectory
            // 
            buttonFindTransactionDirectory.Location = new Point(640, 12);
            buttonFindTransactionDirectory.Name = "buttonFindTransactionDirectory";
            buttonFindTransactionDirectory.Size = new Size(25, 23);
            buttonFindTransactionDirectory.TabIndex = 3;
            buttonFindTransactionDirectory.Text = "…";
            toolTipChooseDirectory.SetToolTip(buttonFindTransactionDirectory, "choose Directory…");
            buttonFindTransactionDirectory.UseVisualStyleBackColor = true;
            buttonFindTransactionDirectory.Click += buttonFindTransactionDirectory_Click;
            // 
            // textBoxLog
            // 
            textBoxLog.Location = new Point(12, 563);
            textBoxLog.Multiline = true;
            textBoxLog.Name = "textBoxLog";
            textBoxLog.ReadOnly = true;
            textBoxLog.Size = new Size(1240, 186);
            textBoxLog.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 83);
            label1.Name = "label1";
            label1.Size = new Size(65, 15);
            label1.TabIndex = 5;
            label1.Text = "Older Files:";
            // 
            // labelOlderNullFilledFiles
            // 
            labelOlderNullFilledFiles.AutoSize = true;
            labelOlderNullFilledFiles.Location = new Point(12, 346);
            labelOlderNullFilledFiles.Name = "labelOlderNullFilledFiles";
            labelOlderNullFilledFiles.Size = new Size(177, 15);
            labelOlderNullFilledFiles.TabIndex = 6;
            labelOlderNullFilledFiles.Text = "Older NULL  filled or empty Files";
            // 
            // listViewOldFilesFilledWithNulls
            // 
            listViewOldFilesFilledWithNulls.Location = new Point(12, 364);
            listViewOldFilesFilledWithNulls.Name = "listViewOldFilesFilledWithNulls";
            listViewOldFilesFilledWithNulls.Size = new Size(1240, 140);
            listViewOldFilesFilledWithNulls.TabIndex = 7;
            listViewOldFilesFilledWithNulls.UseCompatibleStateImageBehavior = false;
            listViewOldFilesFilledWithNulls.View = View.Details;
            listViewOldFilesFilledWithNulls.SelectedIndexChanged += listViewOldFilesFilledWithNulls_SelectedIndexChanged;
            // 
            // labelLog
            // 
            labelLog.AutoSize = true;
            labelLog.Location = new Point(12, 545);
            labelLog.Name = "labelLog";
            labelLog.Size = new Size(30, 15);
            labelLog.TabIndex = 8;
            labelLog.Text = "Log:";
            // 
            // labelTransactionFilesRoot
            // 
            labelTransactionFilesRoot.AutoSize = true;
            labelTransactionFilesRoot.Location = new Point(12, 15);
            labelTransactionFilesRoot.Name = "labelTransactionFilesRoot";
            labelTransactionFilesRoot.Size = new Size(125, 15);
            labelTransactionFilesRoot.TabIndex = 9;
            labelTransactionFilesRoot.Text = "Transaction Files Root:";
            // 
            // buttonDeleteSelectedOldFiles
            // 
            buttonDeleteSelectedOldFiles.Location = new Point(12, 309);
            buttonDeleteSelectedOldFiles.Name = "buttonDeleteSelectedOldFiles";
            buttonDeleteSelectedOldFiles.Size = new Size(250, 23);
            buttonDeleteSelectedOldFiles.TabIndex = 10;
            buttonDeleteSelectedOldFiles.Text = "DELETE selected Old Files";
            buttonDeleteSelectedOldFiles.UseVisualStyleBackColor = true;
            buttonDeleteSelectedOldFiles.EnabledChanged += buttonDeleteSelectedOldFiles_EnabledChanged;
            buttonDeleteSelectedOldFiles.Click += buttonDeleteSelectedOldFiles_Click;
            // 
            // buttonDeleteSelectedOldNULLFiles
            // 
            buttonDeleteSelectedOldNULLFiles.Location = new Point(12, 510);
            buttonDeleteSelectedOldNULLFiles.Name = "buttonDeleteSelectedOldNULLFiles";
            buttonDeleteSelectedOldNULLFiles.Size = new Size(250, 23);
            buttonDeleteSelectedOldNULLFiles.TabIndex = 11;
            buttonDeleteSelectedOldNULLFiles.Text = "DELETE seleted Old NULL Files";
            buttonDeleteSelectedOldNULLFiles.UseVisualStyleBackColor = true;
            buttonDeleteSelectedOldNULLFiles.EnabledChanged += buttonDeleteSelectedOldNULLFiles_EnabledChanged;
            buttonDeleteSelectedOldNULLFiles.Click += buttonDeleteSelectedOldNULLFiles_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1264, 761);
            Controls.Add(buttonDeleteSelectedOldNULLFiles);
            Controls.Add(buttonDeleteSelectedOldFiles);
            Controls.Add(labelTransactionFilesRoot);
            Controls.Add(labelLog);
            Controls.Add(listViewOldFilesFilledWithNulls);
            Controls.Add(labelOlderNullFilledFiles);
            Controls.Add(label1);
            Controls.Add(textBoxLog);
            Controls.Add(buttonFindTransactionDirectory);
            Controls.Add(listViewOldFiles);
            Controls.Add(textBoxTransactionDirectory);
            Controls.Add(buttonFindSuspiciousTransactions);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Text = "Sou.Matrixx Transaction Checker";
            Load += MainForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonFindSuspiciousTransactions;
        private TextBox textBoxTransactionDirectory;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private ListView listViewOldFiles;
        private Button buttonFindTransactionDirectory;
        private TextBox textBoxLog;
        private Label label1;
        private Label labelOlderNullFilledFiles;
        private ListView listViewOldFilesFilledWithNulls;
        private Label labelLog;
        private Label labelTransactionFilesRoot;
        private ToolTip toolTipChooseDirectory;
        private Button buttonDeleteSelectedOldFiles;
        private Button buttonDeleteSelectedOldNULLFiles;
    }
}
