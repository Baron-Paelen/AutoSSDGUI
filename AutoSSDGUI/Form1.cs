using System.Diagnostics;
using System.IO;
using System.Management;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Linq;
using System.Windows;


namespace AutoSSDGUI
{


    public partial class Form1 : Form
    {
        // https://stackoverflow.com/questions/21264112/how-to-call-powershell-cmdlets-from-c-sharp-in-visual-studio
        private Runspace runspace;
        private Pipeline pipeline;
        private List<PropertyDataCollection> result;


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

                // TODO run Get-Partition on each disk number and pull the drive letters from that
                List<string> letters = new List<string>();
                pipeline = runspace.CreatePipeline();
                pipeline.Commands.AddScript($"Get-Partition {addthis.drivenum}");
                var partitionResults = pipeline.Invoke();
                foreach (PSObject partition in partitionResults)
                {
                    letters.Add(partition.Properties["DriveLetter"].Value.ToString());
                    var buh = partition.Properties["DriveLetter"].Value.ToString();
                }
                addthis.letters = letters.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();

                outout.Add(addthis);
            }


            return outout;
        }


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //TODO disable wipe button until user selects a drive

            var diskinfo = GetDiskInfo();
            // add row to datagridview1 for each physical drive
            foreach (var x in diskinfo)
            {
                // formatting so it looks nice!
                var prettyLetters = "";
                if (x.letters.Count == 1)
                {
                    prettyLetters = x.letters[0];
                }
                else
                {
                    prettyLetters = String.Join(", ", x.letters.ToArray());
                }
                dataGridView1.Rows.Add(false,
                    x.drivenum,
                    prettyLetters,
                    BytesToGib(x.size) + "GiB"
                );

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult confirmResult = MessageBox.Show("Are you sure you want to wipe the selected drives?", "Confirm Deletion!", MessageBoxButtons.YesNo);

            if (confirmResult == DialogResult.Yes)
            {
                // If 'Yes', do something here.

            }
            else
            {
                // If 'No', do something here
                
            }
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // if the checkbox is checked, print data of that row/disk
                if (Convert.ToBoolean(row.Cells[0].Value) == true)
                {
                    // don't let user nuke c drive
                    if (row.Cells[2].Value.ToString().Contains("C"))
                    {
                        DialogResult cantnukec = MessageBox.Show("You cannot wipe the C drive. Please unselect the C drive and try again.", "C Drive Selected!", MessageBoxButtons.OK);
                        

                        Debug.WriteLine("NOOOO DONT NUKE C DRIVE!!!");
                        return;
                    }
                    Debug.WriteLine(row.Cells[1].Value.ToString());

                   

                }
            }
        }

        // check all boxes
        private void selectallbutton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < driveList.Items.Count; i++)
            {
                driveList.SetItemChecked(i, true);
            }
        }

        private void deselectallbutton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < driveList.Items.Count; i++)
            {
                driveList.SetItemChecked(i, false);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }

    public class diskstuff
    {
        public UInt32 drivenum { get; set; }
        public List<string> letters { get; set; }
        public UInt64 size { get; set; }
        public string sn { get; set; }
    }
}