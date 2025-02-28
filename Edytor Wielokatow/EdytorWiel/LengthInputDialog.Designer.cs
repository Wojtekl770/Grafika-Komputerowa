namespace EdytorWiel
{
    partial class LengthInputDialog
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TrackBar lengthTrackBar;
        private System.Windows.Forms.Label lengthLabel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;

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
            lengthTrackBar = new TrackBar();
            lengthLabel = new Label();
            okButton = new Button();
            cancelButton = new Button();
            ((System.ComponentModel.ISupportInitialize)lengthTrackBar).BeginInit();
            SuspendLayout();
            // 
            // lengthTrackBar
            // 
            lengthTrackBar.Location = new Point(12, 15);
            lengthTrackBar.Maximum = 1000;
            lengthTrackBar.Minimum = 1;
            lengthTrackBar.Name = "lengthTrackBar";
            lengthTrackBar.Size = new Size(300, 56);
            lengthTrackBar.TabIndex = 0;
            lengthTrackBar.TickFrequency = 10;
            lengthTrackBar.Value = 100;
            lengthTrackBar.Scroll += lengthTrackBar_Scroll;
            // 
            // lengthLabel
            // 
            lengthLabel.Location = new Point(40, 74);
            lengthLabel.Name = "lengthLabel";
            lengthLabel.Size = new Size(272, 23);
            lengthLabel.TabIndex = 1;
            lengthLabel.Text = "Długość: 100";
            // 
            // okButton
            // 
            okButton.Location = new Point(40, 100);
            okButton.Name = "okButton";
            okButton.Size = new Size(82, 38);
            okButton.TabIndex = 2;
            okButton.Text = "OK";
            okButton.Click += okButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(150, 100);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(82, 38);
            cancelButton.TabIndex = 3;
            cancelButton.Text = "Anuluj";
            cancelButton.Click += cancelButton_Click;
            // 
            // LengthInputDialog
            // 
            ClientSize = new Size(350, 150);
            Controls.Add(lengthTrackBar);
            Controls.Add(lengthLabel);
            Controls.Add(okButton);
            Controls.Add(cancelButton);
            Name = "LengthInputDialog";
            Text = "Wybierz długość krawędzi";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog; // Zablokowanie zmiany rozmiaru
            this.ControlBox = false; // Wyłączenie przycisków zamknięcia
            this.StartPosition = FormStartPosition.CenterScreen; // Opcjonalnie centrowanie okna
            this.Text = "Wybierz długość krawędzi";
            ((System.ComponentModel.ISupportInitialize)lengthTrackBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
