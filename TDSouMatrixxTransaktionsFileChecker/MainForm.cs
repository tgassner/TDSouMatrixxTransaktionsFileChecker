using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace TransparentDesign.SouMatrixxTransaktionsFileChecker
{
    public partial class MainForm : Form
    {
        private readonly Color lightGreen = Color.FromArgb(255, 150, 220, 150);
        private readonly Color lightRed = Color.FromArgb(255, 240, 150, 150);
        private readonly Color lightYellow = Color.FromArgb(255, 255, 255, 180);

        private int _sortColumnOldFiles = -1;
        private int _sortColumnNullFiles = -1;
        private SortOrder _sortOrderOldFiles = SortOrder.Ascending;
        private SortOrder _sortOrderNullFiles = SortOrder.Ascending;

        public record FileScanProgress(string Message, int TotalFilesProcessed, int OldFilesProcessed, int NullFilesProcessed, IList<ListViewItem>? OldFileItems, IList<ListViewItem>? NullFileItems);

        private CancellationTokenSource? _cts;

        private void buttonCancelSearch_Click(object sender, EventArgs e)
        {
            _cts?.Cancel();
        }

        private async void buttonFindSuspiciousTransactions_Click(object sender, EventArgs e)
        {
            listViewOldFiles.Items.Clear();
            cleanListViewSortColumn(listViewOldFiles);
            listViewFilesContainingNull.Items.Clear();
            cleanListViewSortColumn(listViewFilesContainingNull);
            textBoxTotalFilesProcessed.Text = "0";
            textBoxOldFilesProcessed.Text = "0";
            textBoxNullFilesProcessed.Text = "0";
            buttonDeleteSelectedOldFiles.Enabled = false;
            buttonDeleteSelectedNULLFiles.Enabled = false;
            buttonFindTransactionDirectory.Enabled = false;
            buttonCopySelectedOldFilesFullPathsToClipboard.Enabled = false;
            buttonCopySelectedNullFilesFullPathsToClipboard.Enabled = false;
            progressBarSearch.Visible = true;

            string transactionDirectory = textBoxTransactionDirectory.Text;
            if (string.IsNullOrWhiteSpace(transactionDirectory) || !new DirectoryInfo(transactionDirectory).Exists)
            {
                MessageBox.Show("The path \"" + transactionDirectory + "\" is not valid!");
                return;
            }
            Properties.Settings.Default.transactionDirectory = transactionDirectory;
            Properties.Settings.Default.Save();

            buttonFindSuspiciousTransactions.Enabled = false;
            buttonCancelSearch.Enabled = true;

            _cts = new CancellationTokenSource();

            var progress = new Progress<FileScanProgress>(p =>
            {
                textBoxTotalFilesProcessed.Text = p.TotalFilesProcessed >= 0 ? p.TotalFilesProcessed.ToString() : textBoxTotalFilesProcessed.Text;
                textBoxOldFilesProcessed.Text = p.OldFilesProcessed >= 0 ? p.OldFilesProcessed.ToString() : textBoxOldFilesProcessed.Text;
                textBoxNullFilesProcessed.Text = p.NullFilesProcessed >= 0 ?p.NullFilesProcessed.ToString() : textBoxNullFilesProcessed.Text;
                if (!string.IsNullOrWhiteSpace(p.Message))
                {
                    logToTextBox(p.Message);
                }

                if (p.OldFileItems != null)
                {
                    listViewOldFiles.Items.Clear();
                    listViewOldFiles.Items.AddRange(p.OldFileItems.ToArray());
                }

                if (p.NullFileItems != null)
                {
                    listViewFilesContainingNull.Items.Clear();
                    listViewFilesContainingNull.Items.AddRange(p.NullFileItems.ToArray());
                }
            });

            try
            {
                await Task.Run(() =>
                    ScanAsync(textBoxTransactionDirectory.Text, progress, _cts.Token));
            }
            catch (OperationCanceledException)
            {
                logToTextBox("aborted ");
                textBoxTotalFilesProcessed.Text = "";
                textBoxOldFilesProcessed.Text = "";
                textBoxNullFilesProcessed.Text = "";

            }
            finally
            {
                buttonFindSuspiciousTransactions.Enabled = true;
                buttonCancelSearch.Enabled = false;
                buttonFindTransactionDirectory.Enabled = true;
                progressBarSearch.Visible = false;
            }
        }

        public IEnumerable<string> SafeEnumerateFiles(string rootPath, string searchPattern, IProgress<FileScanProgress> progress)
        {
            Queue<string> pending = new Queue<string>();
            pending.Enqueue(rootPath);

            while (pending.Count > 0)
            {
                string path = pending.Dequeue();

                // 1. Dateien im aktuellen Verzeichnis abrufen
                string[] files = null;
                try
                {
                    files = Directory.GetFiles(path, searchPattern);
                }
                catch (UnauthorizedAccessException e) {
                    progress?.Report(new FileScanProgress(e.Message, -1, -1, -1, null, null));
                    continue; 
                }
                catch (DirectoryNotFoundException e) {
                    progress?.Report(new FileScanProgress(e.Message, -1, -1, -1, null, null));
                    continue; 
                }

                foreach (string file in files)
                {
                    yield return file;
                }

                // 2. Unterverzeichnisse zur Warteschlange hinzufügen
                try
                {
                    string[] subs = Directory.GetDirectories(path);
                    foreach (string sub in subs) pending.Enqueue(sub);
                }
                catch (UnauthorizedAccessException e) 
                {
                    progress?.Report(new FileScanProgress(e.Message, -1, -1 , -1 , null, null));
                }
            }
        }

        private async Task ScanAsync(
            string rootPath,
            IProgress<FileScanProgress> progress,
            CancellationToken cancellationToken)
        {
            int countTotal = 0;
            int countOld = 0;
            int countNull = 0;
            IList<ListViewItem> oldFileItems = new List<ListViewItem>();
            IList<ListViewItem> nullFileItems = new List<ListViewItem>();
            Stopwatch sw = Stopwatch.StartNew();

            foreach (string file in SafeEnumerateFiles(rootPath, "*", progress)) //Directory.EnumerateFiles(rootPath, "*", SearchOption.AllDirectories))
            {
                countTotal++;
                cancellationToken.ThrowIfCancellationRequested();
                string logStr = string.Empty;

                try
                {
                    FileInfo fileInfo = new FileInfo(file);

                    // OLD Files....
                    DateTime creationTime = fileInfo.CreationTime;
                    int ageInDays = (int)DateTime.Now.Subtract(creationTime).TotalDays;
                    if (creationTime.Date < DateTime.Now.Date) // if its from the day bevore or earlier.
                    {
                        countOld++;
                        ListViewItem itemOldFile = new ListViewItem(new string[] {
                            fileInfo.Name,
                            fileInfo.Length.ToString(),
                            ageInDays.ToString(),
                            fileInfo.CreationTime.ToShortDateString(),
                            fileInfo.DirectoryName ?? ""});
                        itemOldFile.Tag = fileInfo.FullName;
                        oldFileItems.Add(itemOldFile);
                    }
                    else if (creationTime.Date > DateTime.Now.Date)
                    {
                        logStr = "File \"" + fileInfo.FullName + "\" has a creation date in the future: " + creationTime.ToShortDateString();
                    }

                    // Files containing NULL values or empty Filed
                    byte[] data = File.ReadAllBytes(file);
                    bool containsNullValueOrIsEmpty = data.Length == 0;
                    if (!containsNullValueOrIsEmpty)
                    {
                        foreach (byte b in data)
                        {
                            if (b == 0)
                            {
                                containsNullValueOrIsEmpty = true;
                                break;
                            }
                        }
                    }
                    if (containsNullValueOrIsEmpty)
                    {
                        countNull++;
                        ListViewItem itemNullFile = new ListViewItem(new string[] {
                                fileInfo.Name,
                                fileInfo.Length.ToString(),
                                ageInDays.ToString(),
                                fileInfo.CreationTime.ToShortDateString(),
                                fileInfo.DirectoryName ?? ""});
                        itemNullFile.Tag = fileInfo.FullName;
                        nullFileItems.Add(itemNullFile);
                    }
                }
                catch (FileNotFoundException e)
                {
                    progress?.Report(new FileScanProgress(e.Message, countTotal, countOld, countNull, null, null));
                }
                catch (IOException e)
                {
                    progress?.Report(new FileScanProgress(e.Message, countTotal, countOld, countNull, null, null));
                }
                catch (UnauthorizedAccessException e)
                {
                    progress?.Report(new FileScanProgress(e.Message, countTotal, countOld, countNull, null, null));
                }
                catch (SecurityException e)
                {
                    progress?.Report(new FileScanProgress(e.Message, countTotal, countOld, countNull, null, null));
                }

                progress?.Report(new FileScanProgress(logStr, countTotal, countOld, countNull, null, null));
            }

            progress?.Report(new FileScanProgress("Search-Run finished. TotalFiles=" + countTotal + " OldFiles=" + countOld + " Old Files containing at least one binary null=" + countNull + " - " + sw.ElapsedMilliseconds + " ms", countTotal, countOld, countNull, oldFileItems, nullFileItems));
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonFindTransactionDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Multiselect = false;
            if (!string.IsNullOrWhiteSpace(textBoxTransactionDirectory.Text) && new DirectoryInfo(textBoxTransactionDirectory.Text).Exists)
            {
                dialog.SelectedPath = textBoxTransactionDirectory.Text;
            }

            DialogResult dialogResult = dialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                string transactionDirectory = textBoxTransactionDirectory.Text = dialog.SelectedPath;
            }
        }

        private void textBoxTransactionDirectory_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string path = textBox.Text;
            if (string.IsNullOrWhiteSpace(path))
            {
                textBox.BackColor = this.lightYellow;
                buttonFindSuspiciousTransactions.Enabled = false;
                return;
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            Color bkColor = (directoryInfo.Exists ? this.lightGreen : this.lightRed);
            buttonFindSuspiciousTransactions.Enabled = directoryInfo.Exists;
            textBox.BackColor = bkColor;

            listViewOldFiles.Items.Clear();
            cleanListViewSortColumn(listViewOldFiles);
            listViewFilesContainingNull.Items.Clear();
            cleanListViewSortColumn(listViewFilesContainingNull);
            textBoxTotalFilesProcessed.Text = "";
            textBoxOldFilesProcessed.Text = "";
            textBoxNullFilesProcessed.Text = "";
            buttonDeleteSelectedOldFiles.Enabled = false;
            buttonDeleteSelectedNULLFiles.Enabled = false;
            buttonCopySelectedOldFilesFullPathsToClipboard.Enabled = false;
            buttonCopySelectedNullFilesFullPathsToClipboard.Enabled = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text = this.Text + "  - Version: " + Assembly.GetExecutingAssembly().GetName().Version;

            buttonFindSuspiciousTransactions.Enabled = false;
            buttonDeleteSelectedOldFiles.Enabled = false;
            buttonDeleteSelectedNULLFiles.Enabled = false;
            buttonCopySelectedOldFilesFullPathsToClipboard.Enabled = false;
            buttonCopySelectedNullFilesFullPathsToClipboard.Enabled = false;

            string transactionDirectory = Properties.Settings.Default.transactionDirectory;
            this.textBoxTransactionDirectory.Text = transactionDirectory;

            listViewOldFiles.Columns.Clear();
            listViewOldFiles.Columns.Add("Filename", 260);
            listViewOldFiles.Columns.Add("Filesize", 70);
            listViewOldFiles.Columns.Add("Age [Days]", 100);
            listViewOldFiles.Columns.Add("Creation Date", 100);
            listViewOldFiles.Columns.Add("Path", 350);

            listViewFilesContainingNull.Columns.Clear();
            listViewFilesContainingNull.Columns.Add("Filename", 250);
            listViewFilesContainingNull.Columns.Add("Filesize", 50);
            listViewFilesContainingNull.Columns.Add("Age [Days]", 80);
            listViewFilesContainingNull.Columns.Add("Creation Date", 150);
            listViewFilesContainingNull.Columns.Add("Path", 350);
        }

        private void logToTextBox(string t)
        {
            textBoxLog.Text = t + "\r\n" + textBoxLog.Text;
        }

        private void listViewOldFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonDeleteSelectedOldFiles.Enabled = listViewOldFiles.SelectedItems.Count > 0;
            buttonCopySelectedOldFilesFullPathsToClipboard.Enabled = listViewOldFiles.SelectedItems.Count > 0;
        }

        private void listViewOldFilesFilledWithNulls_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonDeleteSelectedNULLFiles.Enabled = listViewFilesContainingNull.SelectedItems.Count > 0;
            buttonCopySelectedNullFilesFullPathsToClipboard.Enabled = listViewFilesContainingNull.SelectedItems.Count > 0;
        }

        private void buttonDeleteSelectedOldFiles_EnabledChanged(object sender, EventArgs e)
        {
            buttonDeleteSelectedOldFiles.BackColor = buttonDeleteSelectedOldFiles.Enabled
                ? Color.IndianRed
                : SystemColors.Control;

            buttonDeleteSelectedOldFiles.ForeColor = buttonDeleteSelectedOldFiles.Enabled
                ? Color.White
                : SystemColors.ControlText;
        }

        private void buttonDeleteSelectedNullFiles_EnabledChanged(object sender, EventArgs e)
        {
            buttonDeleteSelectedNULLFiles.BackColor = buttonDeleteSelectedNULLFiles.Enabled
                ? Color.IndianRed
                : SystemColors.Control;

            buttonDeleteSelectedNULLFiles.ForeColor = buttonDeleteSelectedNULLFiles.Enabled
                ? Color.White
                : SystemColors.ControlText;
        }

        private void buttonDeleteSelectedOldFiles_Click(object sender, EventArgs e)
        {
            IterateThroughSelectedAndDelete(listViewOldFiles.SelectedItems);
            CheckAndRemoveNotExistingFiles(listViewFilesContainingNull.Items);
        }

        private void buttonDeleteSelectedOldNULLFiles_Click(object sender, EventArgs e)
        {
            IterateThroughSelectedAndDelete(listViewFilesContainingNull.SelectedItems);
            CheckAndRemoveNotExistingFiles(listViewOldFiles.Items);
        }

        private void CheckAndRemoveNotExistingFiles(ListView.ListViewItemCollection itemsToBeChecked)
        {
            foreach (ListViewItem item in itemsToBeChecked)
            {
                if (item != null && item.Tag is string filePath && !string.IsNullOrEmpty(filePath) && !new FileInfo(filePath).Exists)
                {
                    item.Remove();
                    logToTextBox("File \"" + filePath + "\" does not exist anymore and was removed from the list.");
                    continue;
                }
            }
        }

        private void IterateThroughSelectedAndDelete(ListView.SelectedListViewItemCollection selectedItems)
        {
            foreach (ListViewItem item in selectedItems)
            {
                if (item != null && item.Tag is string filePath && !string.IsNullOrEmpty(filePath))
                {
                    if (!new FileInfo(filePath).Exists)
                    {
                        item.Remove();
                        logToTextBox("File \"" + filePath + "\" does not exist anymore and was removed also from the other list.");
                        continue;
                    }

                    if (DeleteSelectedFilesAndLog(filePath))
                    {
                        item.Remove();
                    }
                }
            }
        }

        private bool DeleteSelectedFilesAndLog(string filePath)
        {
            try
            {
                File.Delete(filePath);
                logToTextBox(filePath + " *** deleted. ***");
                return true;
            }
            catch (ArgumentNullException ex)
            {
                logToTextBox(ex.Message);
            }
            catch (ArgumentException ex)
            {
                logToTextBox(ex.Message);
            }
            catch (PathTooLongException ex)
            {
                logToTextBox(ex.Message);
            }
            catch (DirectoryNotFoundException ex)
            {
                logToTextBox(ex.Message);
            }
            catch (IOException ex)
            {
                logToTextBox(ex.Message);
            }
            catch (NotSupportedException ex)
            {
                logToTextBox(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                logToTextBox(ex.Message);
            }
            return false;
        }

        private void buttonInfo_Click(object sender, EventArgs e)
        {
            logToTextBox("Internal Tool of the company\r\n" +
                "„Transparent Design“ Handelsgesellschaft m.b.H.\r\n" +
                "https://www.transparentdesign.at\r\n" +
                "This tool is designed to help identify and manage old (transaction) files in the specified directory.\r\n" +
                "It scans for files that are older than a couple of hours and checks if they contain only binary null values.\r\n" +
                "You can select and delete suspicious files directly from the interface.\r\n" +
                "Please ensure you have the necessary permission to delete files in the target directory.\r\n" +
                "Always review the files before deletion to avoid accidental loss of important data.\r\n" +
                "There absolutely is no warranty, but you can use it as you want without mentioning the author.\r\n");
        }

        private void listViewOldFiles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            doSort(e, listViewOldFiles, ref _sortColumnOldFiles, ref _sortOrderOldFiles);
        }

        private void listViewFilesFilledWithNulls_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            doSort(e, listViewFilesContainingNull, ref _sortColumnNullFiles, ref _sortOrderNullFiles);
        }

        private void doSort(ColumnClickEventArgs e, ListView listView
            , ref int sortColumn, ref SortOrder sortOrder)
        {
            if (e.Column == sortColumn)
            {
                sortOrder = sortOrder == SortOrder.Ascending
                    ? SortOrder.Descending
                    : SortOrder.Ascending;
            }
            else
            {
                // neue Spalte
                sortColumn = e.Column;
                sortOrder = SortOrder.Ascending;
            }

            listView.ListViewItemSorter =
                new ListViewItemComparer(sortColumn, sortOrder);

            listView.Sort();

            cleanListViewSortColumn(listView);

            listView.Columns[e.Column].Text += sortOrder == SortOrder.Ascending ? " ↑" : " ↓";
        }

        private void cleanListViewSortColumn(ListView listView)
        {
            foreach (ColumnHeader column in listView.Columns)
            {
                column.Text = column.Text.Replace(" ↑", "").Replace(" ↓", "").Trim();
            }
        }

        private void listViewOldFiles_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var item = listViewOldFiles.GetItemAt(e.X, e.Y);

                if (item != null)
                {
                    listViewOldFiles.SelectedItems.Clear();
                    item.Selected = true;
                }
            }
        }

        private void listViewFilesFilledWithNulls_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var item = listViewFilesContainingNull.GetItemAt(e.X, e.Y);

                if (item != null)
                {
                    listViewFilesContainingNull.SelectedItems.Clear();
                    item.Selected = true;
                }
            }
        }

        private void contextMenuStripOldFilesListBoxContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool hasSelection = listViewOldFiles.SelectedItems.Count > 0;

            if (!hasSelection)
            {
                e.Cancel = true;
                return;
            }
        }

        private void contextMenuStripNullFilesListBoxContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool hasSelection = listViewFilesContainingNull.SelectedItems.Count > 0;

            if (!hasSelection)
            {
                e.Cancel = true;
                return;
            }
        }

        private void toolStripMenuItemOldFilesViewWithWindowsDefaultApp_Click(object sender, EventArgs e)
        {
            viewWithWindowsDefaultApp(sender);
        }

        private void toolStripMenuItemOldFilesViewWithTexteditor_Click(object sender, EventArgs e)
        {
            viewWithTextEditor(sender);
        }

        private void toolStripMenuItemNullFilesViewWithTexteditor_Click(object sender, EventArgs e)
        {
            viewWithTextEditor(sender);
        }

        private void toolStripMenuItemNullFilesViewWithWindowsDefaultApp_Click(object sender, EventArgs e)
        {
            viewWithWindowsDefaultApp(sender);
        }

        private void viewWithWindowsDefaultApp(object sender)
        {
            var menu = ((ToolStripItem)sender).Owner as ContextMenuStrip;
            var listView = menu?.SourceControl as ListView;

            if (listView == null || listView.SelectedItems.Count == 0)
                return;

            var item = listView.SelectedItems[0];
            string? filePath = item.Tag?.ToString();

            if (File.Exists(filePath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            else
            {
                MessageBox.Show("The File: " + filePath + " doesn't exist any more!");
            }
        }

        private void viewWithTextEditor(object sender)
        {
            var menu = ((ToolStripItem)sender).Owner as ContextMenuStrip;
            var listView = menu?.SourceControl as ListView;

            if (listView == null || listView.SelectedItems.Count == 0)
                return;

            var item = listView.SelectedItems[0];
            string? filePath = item.Tag?.ToString();

            if (File.Exists(filePath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = File.Exists(@"C:\Program Files\Notepad++\notepad++.exe") ? @"C:\Program Files\Notepad++\notepad++.exe" : "notepad.exe",
                    Arguments = $"\"{filePath}\"",
                    UseShellExecute = true
                });
            }
            else
            {
                MessageBox.Show("The File: " + filePath + " doesn't exist any more!");
            }
        }

        private void toolStripMenuItemOldFilesDelete_Click(object sender, EventArgs e)
        {
            IterateThroughSelectedAndDelete(listViewOldFiles.SelectedItems);
            CheckAndRemoveNotExistingFiles(listViewFilesContainingNull.Items);
        }

        private void toolStripMenuItemNullFilesDelete_Click(object sender, EventArgs e)
        {
            IterateThroughSelectedAndDelete(listViewFilesContainingNull.SelectedItems);
            CheckAndRemoveNotExistingFiles(listViewOldFiles.Items);
        }

        private void copyFullPathToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copyFullPathToClipboard(sender);
        }

        private void toolStripMenuItemNullFilesCopyFullPathToClipboard_Click(object sender, EventArgs e)
        {
            copyFullPathToClipboard(sender);
        }

        private void copyFullPathToClipboard(object sender)
        {
            var menu = ((ToolStripItem)sender).Owner as ContextMenuStrip;
            var listView = menu?.SourceControl as ListView;

            if (listView == null || listView.SelectedItems.Count == 0)
                return;

            var item = listView.SelectedItems[0];
            string? filePath = item.Tag?.ToString();

            if (!string.IsNullOrWhiteSpace(filePath))
            {
                try
                {
                    Clipboard.SetText(filePath);
                    logToTextBox("\"" + filePath + "\" copied to Clipboard.");
                }
                catch (ExternalException)
                {
                    logToTextBox("Could not copy to Clipboard, it is currently used by an other process");
                }
            }
        }

        private void buttonCopySelectedOldFilesFullPathsToClipboard_Click(object sender, EventArgs e)
        {
            IterateThroughSelectedAndCopyToClipboar(listViewOldFiles.SelectedItems);
        }

        private void buttonCopySelectedNullFilesFullPathsToClipboard_Click(object sender, EventArgs e)
        {
            IterateThroughSelectedAndCopyToClipboar(listViewFilesContainingNull.SelectedItems);
        }

        private void IterateThroughSelectedAndCopyToClipboar(ListView.SelectedListViewItemCollection selectedItems)
        {
            StringBuilder fileList = new StringBuilder();
            foreach (ListViewItem item in selectedItems)
            {
                if (item != null && item.Tag is string filePath && !string.IsNullOrEmpty(filePath))
                {
                    if (new FileInfo(filePath).Exists)
                    {
                        fileList.AppendLine(filePath);
                    }
                    else
                    {
                        fileList.AppendLine("not Existing NOW: " + filePath);
                    }
                }
            }

            try
            {
                string list = fileList.ToString();
                Clipboard.SetText(list);
                logToTextBox("File Path List copied to Clipboard.");
            }
            catch (ExternalException)
            {
                logToTextBox("Could not copy to Clipboard, it is currently used by an other process");
            }
        }

        private void listViewOldFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                SelectAllItemsInListView(listViewOldFiles);

                // Verhindern, dass das System ein "Ping"-Geräusch macht
                e.SuppressKeyPress = true;
            }
        }

        private void listViewFilesContainingNull_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                SelectAllItemsInListView(listViewFilesContainingNull);

                // Verhindern, dass das System ein "Ping"-Geräusch macht
                e.SuppressKeyPress = true;
            }
        }

        private void SelectAllItemsInListView(ListView listView)
        {
            foreach (ListViewItem item in listView.Items)
            {
                item.Selected = true;
            }
        }
    }
}