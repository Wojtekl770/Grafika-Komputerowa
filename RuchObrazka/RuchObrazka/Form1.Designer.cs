namespace RuchObrazka
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
        /// <param name="disposing">true if managed resources should be disposed.</param>
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
            panel1 = new BufferedPanel();
            BezierLabel = new Label();
            numberLabel = new Label();
            numberTextBox = new TextBox();
            generateButton = new Button();
            visiblePolylineCheckBox = new CheckBox();
            loadPolylineButton = new Button();
            savePolylineButton = new Button();
            loadImageButton = new Button();
            rotatingGroupBox = new GroupBox();
            naiveRadioButton = new RadioButton();
            filteringRadioButton = new RadioButton();
            animationGroupBox = new GroupBox();
            rotationRadioButton = new RadioButton();
            movingRadioButton = new RadioButton();
            startButton = new Button();
            stopButton = new Button();
            PolylineGroupBox = new GroupBox();
            VisibleControlCheckBox = new CheckBox();
            ImageGroupBox = new GroupBox();
            createButton = new Button();
            ImagePanel = new Panel();
            rotatingGroupBox.SuspendLayout();
            animationGroupBox.SuspendLayout();
            PolylineGroupBox.SuspendLayout();
            ImageGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ButtonHighlight;
            panel1.Location = new Point(244, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(510, 517);
            panel1.TabIndex = 0;
            panel1.Paint += panel1_Paint;
            panel1.MouseDown += panel1_MouseDown;
            panel1.MouseMove += panel1_MouseMove;
            panel1.MouseUp += panel1_MouseUp;
            // 
            // BezierLabel
            // 
            BezierLabel.AutoSize = true;
            BezierLabel.Font = new Font("Segoe UI", 10F);
            BezierLabel.Location = new Point(12, 12);
            BezierLabel.Name = "BezierLabel";
            BezierLabel.Size = new Size(116, 23);
            BezierLabel.TabIndex = 1;
            BezierLabel.Text = "Bezier's Curve";
            // 
            // numberLabel
            // 
            numberLabel.AutoSize = true;
            numberLabel.Location = new Point(12, 45);
            numberLabel.Name = "numberLabel";
            numberLabel.Size = new Size(129, 20);
            numberLabel.TabIndex = 2;
            numberLabel.Text = "Number of points:";
            // 
            // numberTextBox
            // 
            numberTextBox.Location = new Point(26, 67);
            numberTextBox.Name = "numberTextBox";
            numberTextBox.Size = new Size(95, 27);
            numberTextBox.TabIndex = 3;
            numberTextBox.Text = "5";
            // 
            // generateButton
            // 
            generateButton.Location = new Point(127, 67);
            generateButton.Name = "generateButton";
            generateButton.Size = new Size(94, 29);
            generateButton.TabIndex = 4;
            generateButton.Text = "Generate";
            generateButton.UseVisualStyleBackColor = true;
            generateButton.Click += generateButton_Click;
            // 
            // visiblePolylineCheckBox
            // 
            visiblePolylineCheckBox.AutoSize = true;
            visiblePolylineCheckBox.Checked = true;
            visiblePolylineCheckBox.CheckState = CheckState.Checked;
            visiblePolylineCheckBox.Location = new Point(11, 26);
            visiblePolylineCheckBox.Name = "visiblePolylineCheckBox";
            visiblePolylineCheckBox.Size = new Size(132, 24);
            visiblePolylineCheckBox.TabIndex = 5;
            visiblePolylineCheckBox.Text = "Visible polyline";
            visiblePolylineCheckBox.UseVisualStyleBackColor = true;
            visiblePolylineCheckBox.CheckedChanged += visiblePolylineCheckBox_CheckedChanged;
            // 
            // loadPolylineButton
            // 
            loadPolylineButton.Font = new Font("Segoe UI", 9F);
            loadPolylineButton.Location = new Point(6, 85);
            loadPolylineButton.Name = "loadPolylineButton";
            loadPolylineButton.Size = new Size(100, 29);
            loadPolylineButton.TabIndex = 6;
            loadPolylineButton.Text = "Load";
            loadPolylineButton.UseVisualStyleBackColor = true;
            loadPolylineButton.Click += loadPolylineButton_Click;
            // 
            // savePolylineButton
            // 
            savePolylineButton.Font = new Font("Segoe UI", 9F);
            savePolylineButton.Location = new Point(112, 85);
            savePolylineButton.Name = "savePolylineButton";
            savePolylineButton.Size = new Size(94, 29);
            savePolylineButton.TabIndex = 7;
            savePolylineButton.Text = "Save";
            savePolylineButton.UseVisualStyleBackColor = true;
            savePolylineButton.Click += savePolylineButton_Click;
            // 
            // loadImageButton
            // 
            loadImageButton.Location = new Point(5, 26);
            loadImageButton.Name = "loadImageButton";
            loadImageButton.Size = new Size(99, 29);
            loadImageButton.TabIndex = 9;
            loadImageButton.Text = "Load";
            loadImageButton.UseVisualStyleBackColor = true;
            loadImageButton.Click += loadImageButton_Click;
            // 
            // rotatingGroupBox
            // 
            rotatingGroupBox.Controls.Add(naiveRadioButton);
            rotatingGroupBox.Controls.Add(filteringRadioButton);
            rotatingGroupBox.Location = new Point(16, 336);
            rotatingGroupBox.Name = "rotatingGroupBox";
            rotatingGroupBox.Size = new Size(211, 78);
            rotatingGroupBox.TabIndex = 11;
            rotatingGroupBox.TabStop = false;
            rotatingGroupBox.Text = "Rotating";
            // 
            // naiveRadioButton
            // 
            naiveRadioButton.AutoSize = true;
            naiveRadioButton.Checked = true;
            naiveRadioButton.Location = new Point(6, 22);
            naiveRadioButton.Name = "naiveRadioButton";
            naiveRadioButton.Size = new Size(68, 24);
            naiveRadioButton.TabIndex = 0;
            naiveRadioButton.TabStop = true;
            naiveRadioButton.Text = "Naive";
            naiveRadioButton.UseVisualStyleBackColor = true;
            naiveRadioButton.CheckedChanged += naiveRadioButton_CheckedChanged;
            // 
            // filteringRadioButton
            // 
            filteringRadioButton.AutoSize = true;
            filteringRadioButton.Location = new Point(6, 46);
            filteringRadioButton.Name = "filteringRadioButton";
            filteringRadioButton.Size = new Size(117, 24);
            filteringRadioButton.TabIndex = 1;
            filteringRadioButton.Text = "With filtering";
            filteringRadioButton.UseVisualStyleBackColor = true;
            filteringRadioButton.CheckedChanged += filteringRadioButton_CheckedChanged;
            // 
            // animationGroupBox
            // 
            animationGroupBox.Controls.Add(rotationRadioButton);
            animationGroupBox.Controls.Add(movingRadioButton);
            animationGroupBox.Location = new Point(16, 420);
            animationGroupBox.Name = "animationGroupBox";
            animationGroupBox.Size = new Size(211, 78);
            animationGroupBox.TabIndex = 12;
            animationGroupBox.TabStop = false;
            animationGroupBox.Text = "Animation";
            // 
            // rotationRadioButton
            // 
            rotationRadioButton.AutoSize = true;
            rotationRadioButton.Location = new Point(6, 22);
            rotationRadioButton.Name = "rotationRadioButton";
            rotationRadioButton.Size = new Size(87, 24);
            rotationRadioButton.TabIndex = 0;
            rotationRadioButton.Text = "Rotation";
            rotationRadioButton.UseVisualStyleBackColor = true;
            rotationRadioButton.CheckedChanged += rotationRadioButton_CheckedChanged;
            // 
            // movingRadioButton
            // 
            movingRadioButton.AutoSize = true;
            movingRadioButton.Checked = true;
            movingRadioButton.Location = new Point(6, 46);
            movingRadioButton.Name = "movingRadioButton";
            movingRadioButton.Size = new Size(165, 24);
            movingRadioButton.TabIndex = 1;
            movingRadioButton.TabStop = true;
            movingRadioButton.Text = "Moving on the curve";
            movingRadioButton.UseVisualStyleBackColor = true;
            movingRadioButton.CheckedChanged += movingRadioButton_CheckedChanged;
            // 
            // startButton
            // 
            startButton.Location = new Point(21, 504);
            startButton.Name = "startButton";
            startButton.Size = new Size(94, 29);
            startButton.TabIndex = 13;
            startButton.Text = "Start";
            startButton.UseVisualStyleBackColor = true;
            startButton.Click += startButton_Click;
            // 
            // stopButton
            // 
            stopButton.Location = new Point(127, 504);
            stopButton.Name = "stopButton";
            stopButton.Size = new Size(94, 29);
            stopButton.TabIndex = 14;
            stopButton.Text = "Stop";
            stopButton.UseVisualStyleBackColor = true;
            stopButton.Click += stopButton_Click;
            // 
            // PolylineGroupBox
            // 
            PolylineGroupBox.Controls.Add(VisibleControlCheckBox);
            PolylineGroupBox.Controls.Add(loadPolylineButton);
            PolylineGroupBox.Controls.Add(visiblePolylineCheckBox);
            PolylineGroupBox.Controls.Add(savePolylineButton);
            PolylineGroupBox.Location = new Point(15, 101);
            PolylineGroupBox.Name = "PolylineGroupBox";
            PolylineGroupBox.Size = new Size(212, 123);
            PolylineGroupBox.TabIndex = 0;
            PolylineGroupBox.TabStop = false;
            PolylineGroupBox.Text = "Polyline";
            // 
            // VisibleControlCheckBox
            // 
            VisibleControlCheckBox.AutoSize = true;
            VisibleControlCheckBox.Checked = true;
            VisibleControlCheckBox.CheckState = CheckState.Checked;
            VisibleControlCheckBox.Location = new Point(11, 56);
            VisibleControlCheckBox.Name = "VisibleControlCheckBox";
            VisibleControlCheckBox.Size = new Size(118, 24);
            VisibleControlCheckBox.TabIndex = 8;
            VisibleControlCheckBox.Text = "Visible Points";
            VisibleControlCheckBox.UseVisualStyleBackColor = true;
            VisibleControlCheckBox.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // ImageGroupBox
            // 
            ImageGroupBox.Controls.Add(createButton);
            ImageGroupBox.Controls.Add(ImagePanel);
            ImageGroupBox.Controls.Add(loadImageButton);
            ImageGroupBox.Location = new Point(16, 221);
            ImageGroupBox.Name = "ImageGroupBox";
            ImageGroupBox.Size = new Size(211, 109);
            ImageGroupBox.TabIndex = 0;
            ImageGroupBox.TabStop = false;
            ImageGroupBox.Text = "Image";
            // 
            // createButton
            // 
            createButton.Location = new Point(5, 63);
            createButton.Name = "createButton";
            createButton.Size = new Size(99, 29);
            createButton.TabIndex = 11;
            createButton.Text = "Create";
            createButton.UseVisualStyleBackColor = true;
            createButton.Click += createButton_Click;
            // 
            // ImagePanel
            // 
            ImagePanel.Location = new Point(128, 26);
            ImagePanel.Name = "ImagePanel";
            ImagePanel.Size = new Size(67, 66);
            ImagePanel.TabIndex = 10;
            ImagePanel.Paint += imagePreviewPanel_Paint;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(766, 541);
            Controls.Add(ImageGroupBox);
            Controls.Add(PolylineGroupBox);
            Controls.Add(stopButton);
            Controls.Add(startButton);
            Controls.Add(animationGroupBox);
            Controls.Add(rotatingGroupBox);
            Controls.Add(generateButton);
            Controls.Add(numberTextBox);
            Controls.Add(numberLabel);
            Controls.Add(BezierLabel);
            Controls.Add(panel1);
            Name = "Form1";
            Text = "Bezier Curve Animation";
            rotatingGroupBox.ResumeLayout(false);
            rotatingGroupBox.PerformLayout();
            animationGroupBox.ResumeLayout(false);
            animationGroupBox.PerformLayout();
            PolylineGroupBox.ResumeLayout(false);
            PolylineGroupBox.PerformLayout();
            ImageGroupBox.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private BufferedPanel panel1;
        private Label BezierLabel;
        private Label numberLabel;
        private TextBox numberTextBox;
        private Button generateButton;
        private CheckBox visiblePolylineCheckBox;
        private Button loadPolylineButton;
        private Button savePolylineButton;
        private Button loadImageButton;
        private GroupBox rotatingGroupBox;
        private RadioButton naiveRadioButton;
        private RadioButton filteringRadioButton;
        private GroupBox animationGroupBox;
        private RadioButton rotationRadioButton;
        private RadioButton movingRadioButton;
        private Button startButton;
        private Button stopButton;
        private GroupBox PolylineGroupBox;
        private GroupBox ImageGroupBox;
        private Panel ImagePanel;
        private CheckBox VisibleControlCheckBox;
        private Button createButton;
    }
}
