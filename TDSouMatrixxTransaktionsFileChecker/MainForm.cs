using System.Diagnostics;
using System.Reflection;
using System.Security;
using System.Windows.Forms;

namespace TransparentDesign.SouMatrixxTransaktionsFileChecker
{
    public partial class MainForm : Form
    {
        private readonly Color lightGreen = Color.FromArgb(255, 150, 220, 150);
        private readonly Color lightRed = Color.FromArgb(255, 240, 150, 150);
        private readonly Color lightYellow = Color.FromArgb(255, 255, 255, 180);

        public record FileScanProgress(string Message, int TotalFilesProcessed, int OldFilesProcessed, int OldNullFilesProcessed);

        private CancellationTokenSource? _cts;


        private void buttonCancelSearch_Click(object sender, EventArgs e)
        {
            _cts?.Cancel();
        }

        private async void buttonFindSuspiciousTransactions_Click(object sender, EventArgs e)
        {
            listViewOldFiles.Items.Clear();
            listViewOldFilesFilledWithNulls.Items.Clear();
            textBoxTotalFilesProcessed.Text = "0";
            textBoxOldFilesProcessed.Text = "0";
            textBoxOldNullFilesProcessed.Text = "0";
            buttonDeleteSelectedOldFiles.Enabled = false;
            buttonDeleteSelectedOldNULLFiles.Enabled = false;
            buttonFindTransactionDirectory.Enabled = false;
            progressBarSearch.Visible = true;

            string transactionDirectory = textBoxTransactionDirectory.Text;
            if (String.IsNullOrWhiteSpace(transactionDirectory) || !new DirectoryInfo(transactionDirectory).Exists)
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
                textBoxTotalFilesProcessed.Text = p.TotalFilesProcessed.ToString();
                textBoxOldFilesProcessed.Text = p.OldFilesProcessed.ToString();
                textBoxOldNullFilesProcessed.Text = p.OldNullFilesProcessed.ToString();
                if (!string.IsNullOrWhiteSpace(p.Message))
                {
                    logToTextBox(p.Message);
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
                textBoxOldNullFilesProcessed.Text = "";

            }
            finally
            {
                buttonFindSuspiciousTransactions.Enabled = true;
                buttonCancelSearch.Enabled = false;
                buttonFindTransactionDirectory.Enabled = true;
                progressBarSearch.Visible = false;
            }
        }

        private async Task ScanAsync(
            string rootPath,
            IProgress<FileScanProgress> progress,
            CancellationToken cancellationToken)
        {
            int countTotal = 0;
            int countOld = 0;
            int countOldNull = 0;
            IList<ListViewItem> oldFileItems = new List<ListViewItem>();
            IList<ListViewItem> nullFileItems = new List<ListViewItem>();
            Stopwatch sw = Stopwatch.StartNew();

            foreach (string file in Directory.EnumerateFiles(rootPath, "*", SearchOption.AllDirectories))
            {
                countTotal++;
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    FileInfo fileInfo = new FileInfo(file);
                    DateTime creationTime = fileInfo.CreationTime;
                    int ageInHours = (int)DateTime.Now.Subtract(creationTime).TotalHours;
                    int ageInDays = (int)DateTime.Now.Subtract(creationTime).TotalDays;

                    if (ageInHours > 50)
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

                        // Do nothing with the data except read it.
                        byte[] data = File.ReadAllBytes(file);
                        bool otherThanNull = false;
                        foreach (byte b in data)
                        {
                            if (b != 0)
                            {
                                otherThanNull = true;
                                break;
                            }
                        }
                        if (!otherThanNull)
                        {
                            countOldNull++;
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
                }
                catch (FileNotFoundException e)
                {
                    progress?.Report(new FileScanProgress(e.Message, countTotal, countOld, countOldNull));
                }
                catch (IOException e)
                {
                    progress?.Report(new FileScanProgress(e.Message, countTotal, countOld, countOldNull));
                }
                catch (UnauthorizedAccessException e)
                {
                    progress?.Report(new FileScanProgress(e.Message, countTotal, countOld, countOldNull));
                }
                catch (SecurityException e)
                {
                    progress?.Report(new FileScanProgress(e.Message, countTotal, countOld, countOldNull));
                }

                progress?.Report(new FileScanProgress(null, countTotal, countOld, countOldNull));
            }

            listViewOldFiles.Items.AddRange(oldFileItems.ToArray());
            listViewOldFilesFilledWithNulls.Items.AddRange(nullFileItems.ToArray());
            progress?.Report(new FileScanProgress("Search-Run finished. TotalFiles=" + countTotal + " OldFiles=" + countOld + " Old Files containing only binary null=" + countOldNull + " " + sw.ElapsedMilliseconds + " ms", countTotal, countOld, countOldNull));
        }


        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonFindTransactionDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Multiselect = false;
            if (!String.IsNullOrWhiteSpace(textBoxTransactionDirectory.Text) && new DirectoryInfo(textBoxTransactionDirectory.Text).Exists)
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
            if (String.IsNullOrWhiteSpace(path))
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
            listViewOldFilesFilledWithNulls.Items.Clear();
            textBoxTotalFilesProcessed.Text = "";
            textBoxOldFilesProcessed.Text = "";
            textBoxOldNullFilesProcessed.Text = "";
            buttonDeleteSelectedOldFiles.Enabled = false;
            buttonDeleteSelectedOldNULLFiles.Enabled = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text = this.Text + "  - Version: " + Assembly.GetExecutingAssembly().GetName().Version;

            buttonFindSuspiciousTransactions.Enabled = false;
            buttonDeleteSelectedOldFiles.Enabled = false;
            buttonDeleteSelectedOldNULLFiles.Enabled = false;

            string transactionDirectory = Properties.Settings.Default.transactionDirectory;
            this.textBoxTransactionDirectory.Text = transactionDirectory;

            listViewOldFiles.Columns.Clear();
            listViewOldFiles.Columns.Add("Filename", 260);
            listViewOldFiles.Columns.Add("Filesize", 70);
            listViewOldFiles.Columns.Add("Age [Days]", 100);
            listViewOldFiles.Columns.Add("Creation Date", 100);
            listViewOldFiles.Columns.Add("Path", 350);

            listViewOldFilesFilledWithNulls.Columns.Clear();
            listViewOldFilesFilledWithNulls.Columns.Add("Filename", 250);
            listViewOldFilesFilledWithNulls.Columns.Add("Filesize", 50);
            listViewOldFilesFilledWithNulls.Columns.Add("Age [Days]", 80);
            listViewOldFilesFilledWithNulls.Columns.Add("Creation Date", 150);
            listViewOldFilesFilledWithNulls.Columns.Add("Path", 350);
        }

        private void logToTextBox(string t)
        {
            textBoxLog.Text = t + "\r\n" + textBoxLog.Text;
        }

        private void listViewOldFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonDeleteSelectedOldFiles.Enabled = listViewOldFiles.SelectedItems.Count > 0;
        }

        private void listViewOldFilesFilledWithNulls_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonDeleteSelectedOldNULLFiles.Enabled = listViewOldFilesFilledWithNulls.SelectedItems.Count > 0;
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

        private void buttonDeleteSelectedOldNULLFiles_EnabledChanged(object sender, EventArgs e)
        {
            buttonDeleteSelectedOldNULLFiles.BackColor = buttonDeleteSelectedOldNULLFiles.Enabled
                ? Color.IndianRed
                : SystemColors.Control;

            buttonDeleteSelectedOldNULLFiles.ForeColor = buttonDeleteSelectedOldNULLFiles.Enabled
                ? Color.White
                : SystemColors.ControlText;
        }

        private void buttonDeleteSelectedOldFiles_Click(object sender, EventArgs e)
        {
            IterateThroughSelectedAndDelete(listViewOldFiles.SelectedItems);
        }

        private void buttonDeleteSelectedOldNULLFiles_Click(object sender, EventArgs e)
        {
            IterateThroughSelectedAndDelete(listViewOldFilesFilledWithNulls.SelectedItems);
        }

        private void IterateThroughSelectedAndDelete(ListView.SelectedListViewItemCollection selectedItems)
        {
            foreach (ListViewItem item in selectedItems)
            {
                if (item != null && item.Tag != null && new FileInfo(item.Tag.ToString()).Exists)
                {
                    if (DeleteSelectedFilesAndLog(item.Tag.ToString()))
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
    }
}