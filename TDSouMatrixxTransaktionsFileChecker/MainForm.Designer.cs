namespace TransparentDesign.SouMatrixxTransaktionsFileChecker
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
            contextMenuStripOldFilesListBoxContextMenuStrip = new ContextMenuStrip(components);
            toolStripMenuItemOldFilesViewWithWindowsDefaultApp = new ToolStripMenuItem();
            toolStripMenuItemOldFilesViewWithTexteditor = new ToolStripMenuItem();
            copyFullPathToClipboardToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItemOldFilesDelete = new ToolStripMenuItem();
            buttonFindTransactionDirectory = new Button();
            textBoxLog = new TextBox();
            label1 = new Label();
            labelNullContainingFiles = new Label();
            listViewFilesContainingNull = new ListView();
            contextMenuStripNullFilesListBoxContextMenuStrip = new ContextMenuStrip(components);
            toolStripMenuItemNullFilesViewWithWindowsDefaultApp = new ToolStripMenuItem();
            toolStripMenuItemNullFilesViewWithTexteditor = new ToolStripMenuItem();
            toolStripMenuItemNullFilesCopyFullPathToClipboard = new ToolStripMenuItem();
            toolStripMenuItemNullFilesDelete = new ToolStripMenuItem();
            labelLog = new Label();
            labelTransactionFilesRoot = new Label();
            toolTipChooseDirectory = new ToolTip(components);
            buttonDeleteSelectedOldFiles = new Button();
            buttonDeleteSelectedNULLFiles = new Button();
            buttonCancelSearch = new Button();
            progressBarSearch = new ProgressBar();
            labelTotalFilesProcessed = new Label();
            textBoxTotalFilesProcessed = new TextBox();
            textBoxOldFilesProcessed = new TextBox();
            labelOldFilesProcessed = new Label();
            textBoxNullFilesProcessed = new TextBox();
            labelOldNullFilesProcessed = new Label();
            buttonInfo = new Button();
            pictureBoxTdToken = new PictureBox();
            buttonCopySelectedOldFilesFullPathsToClipboard = new Button();
            buttonCopySelectedNullFilesFullPathsToClipboard = new Button();
            contextMenuStripOldFilesListBoxContextMenuStrip.SuspendLayout();
            contextMenuStripNullFilesListBoxContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxTdToken).BeginInit();
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
            textBoxTransactionDirectory.Location = new Point(154, 12);
            textBoxTransactionDirectory.Name = "textBoxTransactionDirectory";
            textBoxTransactionDirectory.Size = new Size(491, 23);
            textBoxTransactionDirectory.TabIndex = 1;
            textBoxTransactionDirectory.TextChanged += textBoxTransactionDirectory_TextChanged;
            // 
            // listViewOldFiles
            // 
            listViewOldFiles.ContextMenuStrip = contextMenuStripOldFilesListBoxContextMenuStrip;
            listViewOldFiles.Location = new Point(12, 101);
            listViewOldFiles.Name = "listViewOldFiles";
            listViewOldFiles.Size = new Size(1240, 155);
            listViewOldFiles.TabIndex = 2;
            listViewOldFiles.UseCompatibleStateImageBehavior = false;
            listViewOldFiles.View = View.Details;
            listViewOldFiles.ColumnClick += listViewOldFiles_ColumnClick;
            listViewOldFiles.SelectedIndexChanged += listViewOldFiles_SelectedIndexChanged;
            listViewOldFiles.KeyDown += listViewOldFiles_KeyDown;
            listViewOldFiles.MouseDown += listViewOldFiles_MouseDown;
            // 
            // contextMenuStripOldFilesListBoxContextMenuStrip
            // 
            contextMenuStripOldFilesListBoxContextMenuStrip.Items.AddRange(new ToolStripItem[] { toolStripMenuItemOldFilesViewWithWindowsDefaultApp, toolStripMenuItemOldFilesViewWithTexteditor, copyFullPathToClipboardToolStripMenuItem, toolStripMenuItemOldFilesDelete });
            contextMenuStripOldFilesListBoxContextMenuStrip.Name = "contextMenuStripOldFilesListBoxContextMenuStrip";
            contextMenuStripOldFilesListBoxContextMenuStrip.Size = new Size(243, 92);
            contextMenuStripOldFilesListBoxContextMenuStrip.Opening += contextMenuStripOldFilesListBoxContextMenuStrip_Opening;
            // 
            // toolStripMenuItemOldFilesViewWithWindowsDefaultApp
            // 
            toolStripMenuItemOldFilesViewWithWindowsDefaultApp.Name = "toolStripMenuItemOldFilesViewWithWindowsDefaultApp";
            toolStripMenuItemOldFilesViewWithWindowsDefaultApp.Size = new Size(242, 22);
            toolStripMenuItemOldFilesViewWithWindowsDefaultApp.Text = "View with Windows default App";
            toolStripMenuItemOldFilesViewWithWindowsDefaultApp.Click += toolStripMenuItemOldFilesViewWithWindowsDefaultApp_Click;
            // 
            // toolStripMenuItemOldFilesViewWithTexteditor
            // 
            toolStripMenuItemOldFilesViewWithTexteditor.Name = "toolStripMenuItemOldFilesViewWithTexteditor";
            toolStripMenuItemOldFilesViewWithTexteditor.Size = new Size(242, 22);
            toolStripMenuItemOldFilesViewWithTexteditor.Text = "View with Texteditor";
            toolStripMenuItemOldFilesViewWithTexteditor.Click += toolStripMenuItemNullFilesViewWithTexteditor_Click;
            // 
            // copyFullPathToClipboardToolStripMenuItem
            // 
            copyFullPathToClipboardToolStripMenuItem.Name = "copyFullPathToClipboardToolStripMenuItem";
            copyFullPathToClipboardToolStripMenuItem.Size = new Size(242, 22);
            copyFullPathToClipboardToolStripMenuItem.Text = "Copy full Path to Clipboard";
            copyFullPathToClipboardToolStripMenuItem.Click += copyFullPathToClipboardToolStripMenuItem_Click;
            // 
            // toolStripMenuItemOldFilesDelete
            // 
            toolStripMenuItemOldFilesDelete.Name = "toolStripMenuItemOldFilesDelete";
            toolStripMenuItemOldFilesDelete.Size = new Size(242, 22);
            toolStripMenuItemOldFilesDelete.Text = "Delete";
            toolStripMenuItemOldFilesDelete.Click += toolStripMenuItemOldFilesDelete_Click;
            // 
            // buttonFindTransactionDirectory
            // 
            buttonFindTransactionDirectory.Location = new Point(651, 12);
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
            textBoxLog.Location = new Point(12, 516);
            textBoxLog.Multiline = true;
            textBoxLog.Name = "textBoxLog";
            textBoxLog.ReadOnly = true;
            textBoxLog.Size = new Size(1240, 119);
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
            // labelNullContainingFiles
            // 
            labelNullContainingFiles.AutoSize = true;
            labelNullContainingFiles.Location = new Point(12, 299);
            labelNullContainingFiles.Name = "labelNullContainingFiles";
            labelNullContainingFiles.Size = new Size(173, 15);
            labelNullContainingFiles.TabIndex = 6;
            labelNullContainingFiles.Text = "NULL containing or empty Files";
            // 
            // listViewFilesContainingNull
            // 
            listViewFilesContainingNull.ContextMenuStrip = contextMenuStripNullFilesListBoxContextMenuStrip;
            listViewFilesContainingNull.Location = new Point(12, 317);
            listViewFilesContainingNull.Name = "listViewFilesContainingNull";
            listViewFilesContainingNull.Size = new Size(1240, 140);
            listViewFilesContainingNull.TabIndex = 7;
            listViewFilesContainingNull.UseCompatibleStateImageBehavior = false;
            listViewFilesContainingNull.View = View.Details;
            listViewFilesContainingNull.ColumnClick += listViewFilesFilledWithNulls_ColumnClick;
            listViewFilesContainingNull.SelectedIndexChanged += listViewOldFilesFilledWithNulls_SelectedIndexChanged;
            listViewFilesContainingNull.KeyDown += listViewFilesContainingNull_KeyDown;
            listViewFilesContainingNull.MouseDown += listViewFilesFilledWithNulls_MouseDown;
            // 
            // contextMenuStripNullFilesListBoxContextMenuStrip
            // 
            contextMenuStripNullFilesListBoxContextMenuStrip.Items.AddRange(new ToolStripItem[] { toolStripMenuItemNullFilesViewWithWindowsDefaultApp, toolStripMenuItemNullFilesViewWithTexteditor, toolStripMenuItemNullFilesCopyFullPathToClipboard, toolStripMenuItemNullFilesDelete });
            contextMenuStripNullFilesListBoxContextMenuStrip.Name = "contextMenuStrip1";
            contextMenuStripNullFilesListBoxContextMenuStrip.Size = new Size(243, 92);
            contextMenuStripNullFilesListBoxContextMenuStrip.Opening += contextMenuStripNullFilesListBoxContextMenuStrip_Opening;
            // 
            // toolStripMenuItemNullFilesViewWithWindowsDefaultApp
            // 
            toolStripMenuItemNullFilesViewWithWindowsDefaultApp.Name = "toolStripMenuItemNullFilesViewWithWindowsDefaultApp";
            toolStripMenuItemNullFilesViewWithWindowsDefaultApp.Size = new Size(242, 22);
            toolStripMenuItemNullFilesViewWithWindowsDefaultApp.Text = "View with Windows default App";
            toolStripMenuItemNullFilesViewWithWindowsDefaultApp.Click += toolStripMenuItemNullFilesViewWithWindowsDefaultApp_Click;
            // 
            // toolStripMenuItemNullFilesViewWithTexteditor
            // 
            toolStripMenuItemNullFilesViewWithTexteditor.Name = "toolStripMenuItemNullFilesViewWithTexteditor";
            toolStripMenuItemNullFilesViewWithTexteditor.Size = new Size(242, 22);
            toolStripMenuItemNullFilesViewWithTexteditor.Text = "View with Texteditor";
            toolStripMenuItemNullFilesViewWithTexteditor.Click += toolStripMenuItemOldFilesViewWithTexteditor_Click;
            // 
            // toolStripMenuItemNullFilesCopyFullPathToClipboard
            // 
            toolStripMenuItemNullFilesCopyFullPathToClipboard.Name = "toolStripMenuItemNullFilesCopyFullPathToClipboard";
            toolStripMenuItemNullFilesCopyFullPathToClipboard.Size = new Size(242, 22);
            toolStripMenuItemNullFilesCopyFullPathToClipboard.Text = "Copy full Path to Clipboard";
            toolStripMenuItemNullFilesCopyFullPathToClipboard.Click += toolStripMenuItemNullFilesCopyFullPathToClipboard_Click;
            // 
            // toolStripMenuItemNullFilesDelete
            // 
            toolStripMenuItemNullFilesDelete.Name = "toolStripMenuItemNullFilesDelete";
            toolStripMenuItemNullFilesDelete.Size = new Size(242, 22);
            toolStripMenuItemNullFilesDelete.Text = "Delete";
            toolStripMenuItemNullFilesDelete.Click += toolStripMenuItemNullFilesDelete_Click;
            // 
            // labelLog
            // 
            labelLog.AutoSize = true;
            labelLog.Location = new Point(12, 498);
            labelLog.Name = "labelLog";
            labelLog.Size = new Size(30, 15);
            labelLog.TabIndex = 8;
            labelLog.Text = "Log:";
            // 
            // labelTransactionFilesRoot
            // 
            labelTransactionFilesRoot.AutoSize = true;
            labelTransactionFilesRoot.Location = new Point(23, 15);
            labelTransactionFilesRoot.Name = "labelTransactionFilesRoot";
            labelTransactionFilesRoot.Size = new Size(125, 15);
            labelTransactionFilesRoot.TabIndex = 9;
            labelTransactionFilesRoot.Text = "Transaction Files Root:";
            // 
            // buttonDeleteSelectedOldFiles
            // 
            buttonDeleteSelectedOldFiles.Location = new Point(12, 262);
            buttonDeleteSelectedOldFiles.Name = "buttonDeleteSelectedOldFiles";
            buttonDeleteSelectedOldFiles.Size = new Size(250, 23);
            buttonDeleteSelectedOldFiles.TabIndex = 10;
            buttonDeleteSelectedOldFiles.Text = "DELETE selected Old Files";
            buttonDeleteSelectedOldFiles.UseVisualStyleBackColor = true;
            buttonDeleteSelectedOldFiles.EnabledChanged += buttonDeleteSelectedOldFiles_EnabledChanged;
            buttonDeleteSelectedOldFiles.Click += buttonDeleteSelectedOldFiles_Click;
            // 
            // buttonDeleteSelectedNULLFiles
            // 
            buttonDeleteSelectedNULLFiles.Location = new Point(12, 463);
            buttonDeleteSelectedNULLFiles.Name = "buttonDeleteSelectedNULLFiles";
            buttonDeleteSelectedNULLFiles.Size = new Size(250, 23);
            buttonDeleteSelectedNULLFiles.TabIndex = 11;
            buttonDeleteSelectedNULLFiles.Text = "DELETE seleted NULL containing Files";
            buttonDeleteSelectedNULLFiles.UseVisualStyleBackColor = true;
            buttonDeleteSelectedNULLFiles.EnabledChanged += buttonDeleteSelectedNullFiles_EnabledChanged;
            buttonDeleteSelectedNULLFiles.Click += buttonDeleteSelectedOldNULLFiles_Click;
            // 
            // buttonCancelSearch
            // 
            buttonCancelSearch.Enabled = false;
            buttonCancelSearch.Location = new Point(403, 41);
            buttonCancelSearch.Margin = new Padding(3, 2, 3, 2);
            buttonCancelSearch.Name = "buttonCancelSearch";
            buttonCancelSearch.Size = new Size(172, 39);
            buttonCancelSearch.TabIndex = 12;
            buttonCancelSearch.Text = "Cancel Search";
            buttonCancelSearch.UseVisualStyleBackColor = true;
            buttonCancelSearch.Click += buttonCancelSearch_Click;
            // 
            // progressBarSearch
            // 
            progressBarSearch.Location = new Point(581, 41);
            progressBarSearch.Margin = new Padding(3, 2, 3, 2);
            progressBarSearch.Name = "progressBarSearch";
            progressBarSearch.Size = new Size(183, 39);
            progressBarSearch.Style = ProgressBarStyle.Marquee;
            progressBarSearch.TabIndex = 13;
            progressBarSearch.Visible = false;
            // 
            // labelTotalFilesProcessed
            // 
            labelTotalFilesProcessed.AutoSize = true;
            labelTotalFilesProcessed.Location = new Point(879, 15);
            labelTotalFilesProcessed.Name = "labelTotalFilesProcessed";
            labelTotalFilesProcessed.Size = new Size(118, 15);
            labelTotalFilesProcessed.TabIndex = 14;
            labelTotalFilesProcessed.Text = "Total Files Processed:";
            // 
            // textBoxTotalFilesProcessed
            // 
            textBoxTotalFilesProcessed.Enabled = false;
            textBoxTotalFilesProcessed.Location = new Point(1003, 12);
            textBoxTotalFilesProcessed.Name = "textBoxTotalFilesProcessed";
            textBoxTotalFilesProcessed.Size = new Size(100, 23);
            textBoxTotalFilesProcessed.TabIndex = 15;
            // 
            // textBoxOldFilesProcessed
            // 
            textBoxOldFilesProcessed.Enabled = false;
            textBoxOldFilesProcessed.Location = new Point(1003, 41);
            textBoxOldFilesProcessed.Name = "textBoxOldFilesProcessed";
            textBoxOldFilesProcessed.Size = new Size(100, 23);
            textBoxOldFilesProcessed.TabIndex = 17;
            // 
            // labelOldFilesProcessed
            // 
            labelOldFilesProcessed.AutoSize = true;
            labelOldFilesProcessed.Location = new Point(886, 44);
            labelOldFilesProcessed.Name = "labelOldFilesProcessed";
            labelOldFilesProcessed.Size = new Size(111, 15);
            labelOldFilesProcessed.TabIndex = 16;
            labelOldFilesProcessed.Text = "Old Files Processed:";
            // 
            // textBoxNullFilesProcessed
            // 
            textBoxNullFilesProcessed.Enabled = false;
            textBoxNullFilesProcessed.Location = new Point(1003, 70);
            textBoxNullFilesProcessed.Name = "textBoxNullFilesProcessed";
            textBoxNullFilesProcessed.Size = new Size(100, 23);
            textBoxNullFilesProcessed.TabIndex = 19;
            // 
            // labelOldNullFilesProcessed
            // 
            labelOldNullFilesProcessed.AutoSize = true;
            labelOldNullFilesProcessed.Location = new Point(770, 73);
            labelOldNullFilesProcessed.Name = "labelOldNullFilesProcessed";
            labelOldNullFilesProcessed.Size = new Size(227, 15);
            labelOldNullFilesProcessed.TabIndex = 18;
            labelOldNullFilesProcessed.Text = "Null Contianing or empty Files Processed:";
            // 
            // buttonInfo
            // 
            buttonInfo.Location = new Point(1225, 10);
            buttonInfo.Name = "buttonInfo";
            buttonInfo.Size = new Size(27, 27);
            buttonInfo.TabIndex = 20;
            buttonInfo.Text = "?";
            buttonInfo.UseVisualStyleBackColor = true;
            buttonInfo.Click += buttonInfo_Click;
            // 
            // pictureBoxTdToken
            // 
            pictureBoxTdToken.Image = (Image)resources.GetObject("pictureBoxTdToken.Image");
            pictureBoxTdToken.Location = new Point(1136, 10);
            pictureBoxTdToken.Name = "pictureBoxTdToken";
            pictureBoxTdToken.Size = new Size(83, 83);
            pictureBoxTdToken.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxTdToken.TabIndex = 21;
            pictureBoxTdToken.TabStop = false;
            // 
            // buttonCopySelectedOldFilesFullPathsToClipboard
            // 
            buttonCopySelectedOldFilesFullPathsToClipboard.Location = new Point(268, 262);
            buttonCopySelectedOldFilesFullPathsToClipboard.Name = "buttonCopySelectedOldFilesFullPathsToClipboard";
            buttonCopySelectedOldFilesFullPathsToClipboard.Size = new Size(307, 23);
            buttonCopySelectedOldFilesFullPathsToClipboard.TabIndex = 22;
            buttonCopySelectedOldFilesFullPathsToClipboard.Text = "Copy full paths of selected Old Files to Clipboard";
            buttonCopySelectedOldFilesFullPathsToClipboard.UseVisualStyleBackColor = true;
            buttonCopySelectedOldFilesFullPathsToClipboard.Click += buttonCopySelectedOldFilesFullPathsToClipboard_Click;
            // 
            // buttonCopySelectedNullFilesFullPathsToClipboard
            // 
            buttonCopySelectedNullFilesFullPathsToClipboard.Location = new Point(268, 463);
            buttonCopySelectedNullFilesFullPathsToClipboard.Name = "buttonCopySelectedNullFilesFullPathsToClipboard";
            buttonCopySelectedNullFilesFullPathsToClipboard.Size = new Size(307, 23);
            buttonCopySelectedNullFilesFullPathsToClipboard.TabIndex = 23;
            buttonCopySelectedNullFilesFullPathsToClipboard.Text = "Copy full paths of selected Null Files to Clipboard";
            buttonCopySelectedNullFilesFullPathsToClipboard.UseVisualStyleBackColor = true;
            buttonCopySelectedNullFilesFullPathsToClipboard.Click += buttonCopySelectedNullFilesFullPathsToClipboard_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1264, 647);
            Controls.Add(buttonCopySelectedNullFilesFullPathsToClipboard);
            Controls.Add(buttonCopySelectedOldFilesFullPathsToClipboard);
            Controls.Add(pictureBoxTdToken);
            Controls.Add(buttonInfo);
            Controls.Add(textBoxNullFilesProcessed);
            Controls.Add(labelOldNullFilesProcessed);
            Controls.Add(textBoxOldFilesProcessed);
            Controls.Add(labelOldFilesProcessed);
            Controls.Add(textBoxTotalFilesProcessed);
            Controls.Add(labelTotalFilesProcessed);
            Controls.Add(progressBarSearch);
            Controls.Add(buttonCancelSearch);
            Controls.Add(buttonDeleteSelectedNULLFiles);
            Controls.Add(buttonDeleteSelectedOldFiles);
            Controls.Add(labelTransactionFilesRoot);
            Controls.Add(labelLog);
            Controls.Add(listViewFilesContainingNull);
            Controls.Add(labelNullContainingFiles);
            Controls.Add(label1);
            Controls.Add(textBoxLog);
            Controls.Add(buttonFindTransactionDirectory);
            Controls.Add(listViewOldFiles);
            Controls.Add(textBoxTransactionDirectory);
            Controls.Add(buttonFindSuspiciousTransactions);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Text = "Transparentdesign Sou.Matrixx Transaction File Checker";
            Load += MainForm_Load;
            contextMenuStripOldFilesListBoxContextMenuStrip.ResumeLayout(false);
            contextMenuStripNullFilesListBoxContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxTdToken).EndInit();
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
        private Label labelNullContainingFiles;
        private ListView listViewFilesContainingNull;
        private Label labelLog;
        private Label labelTransactionFilesRoot;
        private ToolTip toolTipChooseDirectory;
        private Button buttonDeleteSelectedOldFiles;
        private Button buttonDeleteSelectedNULLFiles;
        private Button buttonCancelSearch;
        private ProgressBar progressBarSearch;
        private Label labelTotalFilesProcessed;
        private TextBox textBoxTotalFilesProcessed;
        private TextBox textBoxOldFilesProcessed;
        private Label labelOldFilesProcessed;
        private TextBox textBoxNullFilesProcessed;
        private Label labelOldNullFilesProcessed;
        private Button buttonInfo;
        private PictureBox pictureBoxTdToken;
        private ContextMenuStrip contextMenuStripOldFilesListBoxContextMenuStrip;
        private ToolStripMenuItem toolStripMenuItemOldFilesViewWithWindowsDefaultApp;
        private ToolStripMenuItem toolStripMenuItemOldFilesViewWithTexteditor;
        private ToolStripMenuItem toolStripMenuItemOldFilesDelete;
        private ContextMenuStrip contextMenuStripNullFilesListBoxContextMenuStrip;
        private ToolStripMenuItem toolStripMenuItemNullFilesViewWithWindowsDefaultApp;
        private ToolStripMenuItem toolStripMenuItemNullFilesViewWithTexteditor;
        private ToolStripMenuItem toolStripMenuItemNullFilesDelete;
        private ToolStripMenuItem copyFullPathToClipboardToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItemNullFilesCopyFullPathToClipboard;
        private Button buttonCopySelectedOldFilesFullPathsToClipboard;
        private Button buttonCopySelectedNullFilesFullPathsToClipboard;
    }
}
