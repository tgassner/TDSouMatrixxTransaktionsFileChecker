using System.Diagnostics;
using System.Security;

namespace TransparentDesign.SouMatrixxTransaktionsFileChecker
{
    public partial class MainForm : Form
    {
        private readonly Color lightGreen = Color.FromArgb(255, 150, 220, 150);
        private readonly Color lightRed = Color.FromArgb(255, 240, 150, 150);
        private readonly Color lightYellow = Color.FromArgb(255, 255, 255, 180);

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
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            buttonFindSuspiciousTransactions.Enabled = false;
            buttonDeleteSelectedOldFiles.Enabled = false;
            buttonDeleteSelectedOldNULLFiles.Enabled = false;

            string transactionDirectory = Properties.Settings.Default.transactionDirectory;
            this.textBoxTransactionDirectory.Text = transactionDirectory;

            listViewOldFiles.Columns.Clear();
            listViewOldFiles.Columns.Add("Filename", 250);
            listViewOldFiles.Columns.Add("Filesize", 50);
            listViewOldFiles.Columns.Add("Age [Days]", 80);
            listViewOldFiles.Columns.Add("Creation Date", 150);
            listViewOldFiles.Columns.Add("Path", 350);

            listViewOldFilesFilledWithNulls.Columns.Clear();
            listViewOldFilesFilledWithNulls.Columns.Add("Filename", 250);
            listViewOldFilesFilledWithNulls.Columns.Add("Filesize", 50);
            listViewOldFilesFilledWithNulls.Columns.Add("Age [Days]", 80);
            listViewOldFilesFilledWithNulls.Columns.Add("Creation Date", 150);
            listViewOldFilesFilledWithNulls.Columns.Add("Path", 350);
        }

        private void buttonFindSuspiciousTransactions_Click(object sender, EventArgs e)
        {
            this.listViewOldFiles.Items.Clear();
            this.listViewOldFilesFilledWithNulls.Items.Clear();
            string transactionDirectory = textBoxTransactionDirectory.Text;
            if (String.IsNullOrWhiteSpace(transactionDirectory) || !new DirectoryInfo(transactionDirectory).Exists)
            {
                MessageBox.Show("The path \"" + transactionDirectory + "\" is not valid!");
                return;
            }
            Properties.Settings.Default.transactionDirectory = transactionDirectory;
            Properties.Settings.Default.Save();

            try
            {
                traverseTreeForEach(transactionDirectory, (fullPathFilename) =>
                {
                    try
                    {
                        FileInfo fileInfo = new FileInfo(fullPathFilename);
                        DateTime creationTime = fileInfo.CreationTime;
                        int ageInHours = (int)DateTime.Now.Subtract(creationTime).TotalHours;
                        int ageInDays = (int)DateTime.Now.Subtract(creationTime).TotalDays;

                        if (ageInHours > 50)
                        {
                            ListViewItem itemOldFile = new ListViewItem(new string[] {
                                fileInfo.Name,
                                fileInfo.Length.ToString(),
                                ageInDays.ToString(),
                                fileInfo.CreationTime.ToShortDateString(),
                                fileInfo.DirectoryName ?? ""});
                            itemOldFile.Tag = fileInfo.FullName;
                            listViewOldFiles.Items.Add(itemOldFile);

                            // Do nothing with the data except read it.
                            byte[] data = File.ReadAllBytes(fullPathFilename);
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
                                ListViewItem itemNullFile = new ListViewItem(new string[] {
                                    fileInfo.Name,
                                    fileInfo.Length.ToString(),
                                    ageInDays.ToString(),
                                    fileInfo.CreationTime.ToShortDateString(),
                                    fileInfo.DirectoryName ?? ""});
                                itemNullFile.Tag = fileInfo.FullName;
                                this.listViewOldFilesFilledWithNulls.Items.Add(itemNullFile);
                            }
                        }
                    }
                    catch (FileNotFoundException) { }
                    catch (IOException) { }
                    catch (UnauthorizedAccessException) { }
                    catch (SecurityException) { }

                    //logToTextBox(fullPathFilename);
                });
            }
            catch (ArgumentException)
            {
                logToTextBox(@"The directory 'C:\Program Files' does not exist.");
            }
        }

        private void logToTextBox(string t)
        {
            textBoxLog.Text = t + "\r\n" + textBoxLog.Text;
        }

        public void traverseTreeForEach(string root, Action<string> action)
        {
            //Count of files traversed and timer for diagnostic output
            int fileCount = 0;
            var sw = Stopwatch.StartNew();

            // Data structure to hold names of subfolders to be examined for files.
            Stack<string> dirs = new Stack<string>();

            if (!Directory.Exists(root))
            {
                throw new ArgumentException(
                    "The given root directory doesn't exist.", nameof(root));
            }
            dirs.Push(root);

            while (dirs.Count > 0)
            {
                string currentDir = dirs.Pop();
                string[] subDirs = { };
                string[] files = { };

                try
                {
                    subDirs = Directory.GetDirectories(currentDir);
                }
                // Thrown if we do not have discovery permission on the directory.
                catch (UnauthorizedAccessException e)
                {
                    logToTextBox(e.Message);
                    continue;
                }
                // Thrown if another process has deleted the directory after we retrieved its name.
                catch (DirectoryNotFoundException e)
                {
                    logToTextBox(e.Message);
                    continue;
                }

                try
                {
                    files = Directory.GetFiles(currentDir);
                }
                catch (UnauthorizedAccessException e)
                {
                    logToTextBox(e.Message);
                    continue;
                }
                catch (DirectoryNotFoundException e)
                {
                    logToTextBox(e.Message);
                    continue;
                }
                catch (IOException e)
                {
                    logToTextBox(e.Message);
                    continue;
                }

                // Execute in parallel if there are enough files in the directory.
                // Otherwise, execute sequentially.Files are opened and processed
                // synchronously but this could be modified to perform async I/O.
                try
                {
                    foreach (var file in files)
                    {
                        action(file);
                        fileCount++;
                    }
                }
                catch (AggregateException ae)
                {
                    ae.Handle((ex) =>
                    {
                        if (ex is UnauthorizedAccessException)
                        {
                            // Here we just output a message and go on.
                            logToTextBox(ex.Message);
                            return true;
                        }
                        // Handle other exceptions here if necessary...

                        return false;
                    });
                }

                // Push the subdirectories onto the stack for traversal.
                // This could also be done before handing the files.
                foreach (string str in subDirs)
                    dirs.Push(str);
            }

            // For diagnostic purposes.
            logToTextBox($"Processed {fileCount} files in {sw.ElapsedMilliseconds} milliseconds");
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
                    DeleteSelectedFilesAndLog(item.Tag.ToString());
                }
            }
        }

        private void DeleteSelectedFilesAndLog(string filePath)
        {
            try
            {
                File.Delete(filePath);
                logToTextBox(filePath + " deleted.");
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
        }
    }
}