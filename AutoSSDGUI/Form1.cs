using System.Diagnostics;
using System.Management;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;


namespace AutoSSDGUI
{


    public partial class Form1 : Form
    {
        // https://stackoverflow.com/questions/21264112/how-to-call-powershell-cmdlets-from-c-sharp-in-visual-studio
        private Runspace runspace;
        private Pipeline pipeline;
        private List<PropertyDataCollection> result;
        List<diskstuff> diskinfo;


        public System.Windows.Forms.ListBox.ObjectCollection Items { get; }

        // convert bytes to gibibytes
        private UInt64 BytesToGib(UInt64 bytes)
        {
            return bytes / 1024 / 1024 / 1024;
        }

        // get disk info of all drivse on this pc
        private List<diskstuff> GetDiskInfo()
        {
            runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();

            pipeline = runspace.CreatePipeline();

            //pipeline.Commands.AddScript("Get-Disk (Get-Partition -DriveLetter 'D').DiskNumber");
            pipeline.Commands.AddScript("Get-Disk");

            // https://learn.microsoft.com/en-us/answers/questions/1131852/get-lun-number-and-disk-letter-using-powershell
            // https://learn.microsoft.com/en-us/answers/questions/1131852/get-lun-number-and-disk-letter-using-powershell
            // https://learn.microsoft.com/en-us/answers/questions/1131852/get-lun-number-and-disk-letter-using-powershell

            var diskResults = pipeline.Invoke();


            List<diskstuff> outout = new List<diskstuff>();

            foreach (PSObject disk in diskResults)
            {
                diskstuff addthis = new diskstuff();
                addthis.drivenum = (UInt32)disk.Properties["Number"].Value;
                addthis.size = (UInt64)disk.Properties["Size"].Value;
                addthis.sn = (string)disk.Properties["SerialNumber"].Value;
                addthis.sel = false;
                addthis.newletter = "";

                List<string> letters = new List<string>();
                pipeline = runspace.CreatePipeline();
                pipeline.Commands.AddScript($"Get-Partition {addthis.drivenum}");
                var partitionResults = pipeline.Invoke();
                foreach (PSObject partition in partitionResults)
                {
                    var buh = partition.Properties["DriveLetter"].Value.ToString();

                    //  skip any elements that are not strictly letters
                    if (!Char.IsLetter(buh[0]))
                    {
                        continue;
                    }


                    letters.Add(buh);
                }

                addthis.letters = letters;

                outout.Add(addthis);
            }


            return outout;
        }

        private string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        // enable bitlocker on the drive specified
        // THE PARAMETER IS THE INDEX OF THE DISK IN diskinfo, NOT THE ACTUAL DISK NUMBER
        // this just makes it easier for me :-)
        // (if it was the disk number, you'd need to loop to check which index matches the disk number)
        private void EnableBitLocker(int index)
        {

        }

        //TODO
        //

        // THE PARAMETER IS THE INDEX OF THE DISK IN diskinfo, NOT THE ACTUAL DISK NUMBER
        // this just makes it easier for me :-)
        // https://ndswanson.wordpress.com/2014/08/12/using-diskpart-with-c/
        private string diskpartScripter(int index/*, char newletter*/)
        {
            Process p = new Process();                                    // new instance of Process class
            p.StartInfo.UseShellExecute = false;                          // do not start a new shell
            p.StartInfo.RedirectStandardOutput = true;                    // Redirects the on screen results
            p.StartInfo.FileName = @"C:\Windows\System32\diskpart.exe";   // executable to run
            p.StartInfo.RedirectStandardInput = true;                     // Redirects the input commands
            p.Start();                                                    // Starts the process
            p.StandardInput.WriteLine($"select disk {diskinfo[index].drivenum}");                   // Issues commands to diskpart
            p.StandardInput.WriteLine("clean");                           //   |
            p.StandardInput.WriteLine("create part primary");             //   |
            p.StandardInput.WriteLine("format fs=ntfs quick");            //   |
            p.StandardInput.WriteLine("active");                          //   |
            p.StandardInput.WriteLine("assign");                          //   |
            p.StandardInput.WriteLine("exit");                            // _\|/_
            string output = p.StandardOutput.ReadToEnd();                 // Places the output to a variable
            p.WaitForExit();                                              // Waits for the exe to finish

            return output;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // https://learn.microsoft.com/en-us/dotnet/desktop/winforms/controls/disable-buttons-in-a-button-column-in-the-datagrid?view=netframeworkdesktop-4.8
            dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);
            dataGridView1.CurrentCellDirtyStateChanged += new EventHandler(dataGridView1_CurrentCellDirtyStateChanged);


            diskinfo = GetDiskInfo();
            // add row to datagridview1 for each physical drive
            foreach (var x in diskinfo)
            {
                // formatting so it looks nice!f
                var prettyLetters = "";
                if (x.letters.Count == 1)
                {
                    prettyLetters = x.letters[0];
                }
                else
                {
                    prettyLetters = String.Join(", ", x.letters.ToArray());
                }

                // add each disk's info to the gridview
                dataGridView1.Rows.Add(
                    x.sel,
                    x.drivenum,
                    prettyLetters,
                    BytesToGib(x.size) + "GiB"
                );

            }
        }

        private void WipeDrive_Click(object sender, EventArgs e)
        {
            DialogResult confirmResult = MessageBox.Show("Are you sure you want to wipe the selected drives?", "Confirm Deletion!", MessageBoxButtons.YesNo);

            // make sure C drive isn't selected
            for (int i = 0; i < diskinfo.Count; i++)
            {
                // check if the disk is even selected first (took me like 15 minutes)
                if (!diskinfo[i].sel)
                {
                    continue;
                }

                if (diskinfo[i].letters.Contains("C"))
                {
                    DialogResult cantnukec = MessageBox.Show("You cannot wipe the C drive. Please unselect the C drive and try again.", "C Drive Selected!", MessageBoxButtons.OK);
                    return;
                }
            }

            if (confirmResult == DialogResult.Yes)
            {
                // If 'Yes', run the diskpart script on all selected disks
                for (int i = 0; i < diskinfo.Count; i++)
                {
                    if (diskinfo[i].sel)
                    {
                        Debug.WriteLine($"diskparting {diskinfo[i].drivenum}");
                        Debug.WriteLine(diskpartScripter(i));

                    }
                }
            }
        }

        // check all boxes
        private void selectallbutton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = true;
                //diskinfo[i].sel = true;
            }
        }

        // uncheck all boxes
        private void deselectallbutton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = false;
                //diskinfo[i].sel = false;

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        // updates diskinfo when the checkbox values change
        // https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.datagridview.currentcelldirtystatechanged?view=windowsdesktop-7.0#system-windows-forms-datagridview-currentcelldirtystatechanged
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var row = e.RowIndex;
            var col = e.ColumnIndex;
            Debug.WriteLine(row);
            Debug.WriteLine(col);

            if (dataGridView1.Columns[col].Name == "Select")
            {
                Debug.WriteLine($"disk {row} has sel as {diskinfo[row].sel} and will change to {(bool)dataGridView1.Rows[row].Cells[0].Value}");
                diskinfo[row].sel = (bool)dataGridView1.Rows[row].Cells[0].Value;

                dataGridView1.Invalidate();
            }
        }

        void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
    }

    public class diskstuff
    {
        public UInt32 drivenum { get; set; }
        public List<string> letters { get; set; }
        public string newletter { get; set; }
        public UInt64 size { get; set; }
        public string sn { get; set; }

        public Boolean sel { get; set; }
    }
}