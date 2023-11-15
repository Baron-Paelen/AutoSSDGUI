namespace AutoSSDGUI
{
    partial class Form1
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
            Drives = new Label();
            wipe_button = new Button();
            selectallbutton = new Button();
            deselectallbutton = new Button();
            dataGridView1 = new DataGridView();
            Select = new DataGridViewCheckBoxColumn();
            DriveNum = new DataGridViewTextBoxColumn();
            Volumes = new DataGridViewTextBoxColumn();
            DriveLet = new DataGridViewTextBoxColumn();
            diskstuffBindingSource = new BindingSource(components);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)diskstuffBindingSource).BeginInit();
            SuspendLayout();
            // 
            // Drives
            // 
            Drives.AutoSize = true;
            Drives.Location = new Point(12, 9);
            Drives.Name = "Drives";
            Drives.Size = new Size(39, 15);
            Drives.TabIndex = 2;
            Drives.Text = "Drives";
            // 
            // wipe_button
            // 
            wipe_button.Location = new Point(404, 385);
            wipe_button.Name = "wipe_button";
            wipe_button.Size = new Size(79, 23);
            wipe_button.TabIndex = 4;
            wipe_button.Text = "Wipe Drives";
            wipe_button.UseVisualStyleBackColor = true;
            wipe_button.Click += WipeDrive_Click;
            // 
            // selectallbutton
            // 
            selectallbutton.Location = new Point(12, 357);
            selectallbutton.Name = "selectallbutton";
            selectallbutton.Size = new Size(83, 23);
            selectallbutton.TabIndex = 5;
            selectallbutton.Text = "Select All";
            selectallbutton.UseVisualStyleBackColor = true;
            selectallbutton.Click += selectallbutton_Click;
            // 
            // deselectallbutton
            // 
            deselectallbutton.Location = new Point(12, 386);
            deselectallbutton.Name = "deselectallbutton";
            deselectallbutton.Size = new Size(83, 23);
            deselectallbutton.TabIndex = 6;
            deselectallbutton.Text = "Deselect All";
            deselectallbutton.UseVisualStyleBackColor = true;
            deselectallbutton.Click += deselectallbutton_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            dataGridView1.BorderStyle = BorderStyle.Fixed3D;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Select, DriveNum, Volumes, DriveLet });
            dataGridView1.Location = new Point(12, 27);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.ScrollBars = ScrollBars.Vertical;
            dataGridView1.Size = new Size(201, 324);
            dataGridView1.TabIndex = 7;
            // 
            // Select
            // 
            Select.HeaderText = "Sel";
            Select.Name = "Select";
            Select.Width = 28;
            // 
            // DriveNum
            // 
            DriveNum.HeaderText = "Drive";
            DriveNum.Name = "DriveNum";
            DriveNum.ReadOnly = true;
            DriveNum.Resizable = DataGridViewTriState.True;
            DriveNum.SortMode = DataGridViewColumnSortMode.NotSortable;
            DriveNum.Width = 40;
            // 
            // Volumes
            // 
            Volumes.HeaderText = "Volumes";
            Volumes.Name = "Volumes";
            Volumes.ReadOnly = true;
            Volumes.Width = 77;
            // 
            // DriveLet
            // 
            DriveLet.HeaderText = "Size";
            DriveLet.Name = "DriveLet";
            DriveLet.ReadOnly = true;
            DriveLet.Width = 52;
            // 
            // diskstuffBindingSource
            // 
            diskstuffBindingSource.DataSource = typeof(diskstuff);
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(495, 420);
            Controls.Add(dataGridView1);
            Controls.Add(deselectallbutton);
            Controls.Add(selectallbutton);
            Controls.Add(wipe_button);
            Controls.Add(Drives);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)diskstuffBindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label Drives;
        private Button wipe_button;
        private Button selectallbutton;
        private Button deselectallbutton;
        private DataGridView dataGridView1;
        private BindingSource diskstuffBindingSource;
        private DataGridViewCheckBoxColumn Select;
        private DataGridViewTextBoxColumn DriveNum;
        private DataGridViewTextBoxColumn Volumes;
        private DataGridViewTextBoxColumn DriveLet;
    }
}