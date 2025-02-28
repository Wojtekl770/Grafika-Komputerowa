namespace SiatkaTroj
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox pictureBoxCanvas;
        private System.Windows.Forms.TrackBar trackBarAlpha;
        private System.Windows.Forms.TrackBar trackBarBeta;
        private System.Windows.Forms.TrackBar trackBarResolution;
        private System.Windows.Forms.RadioButton radioButtonWireframe;
        private System.Windows.Forms.RadioButton radioButtonFill;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pictureBoxCanvas = new PictureBox();
            trackBarAlpha = new TrackBar();
            trackBarBeta = new TrackBar();
            trackBarResolution = new TrackBar();
            radioButtonWireframe = new RadioButton();
            radioButtonFill = new RadioButton();
            AlphaLabel = new Label();
            BetaLabel = new Label();
            ResolutionLabel = new Label();
            ControlCheckBox = new CheckBox();
            kdTrackbar = new TrackBar();
            ksTrackBar = new TrackBar();
            mTrackBar = new TrackBar();
            kdLabel = new Label();
            ksLabel = new Label();
            mLabel = new Label();
            ColorButton = new Button();
            TextureButton = new Button();
            StopLightButton = new CheckBox();
            ObjectColorButton = new Button();
            ZTrackBar = new TrackBar();
            label1 = new Label();
            NormalVectorCheckBox = new CheckBox();
            NormalMapButton = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBoxCanvas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarAlpha).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarBeta).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarResolution).BeginInit();
            ((System.ComponentModel.ISupportInitialize)kdTrackbar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ksTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)mTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ZTrackBar).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxCanvas
            // 
            pictureBoxCanvas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBoxCanvas.Location = new Point(12, 12);
            pictureBoxCanvas.Name = "pictureBoxCanvas";
            pictureBoxCanvas.Size = new Size(610, 600);
            pictureBoxCanvas.TabIndex = 0;
            pictureBoxCanvas.TabStop = false;
            pictureBoxCanvas.Paint += pictureBoxCanvas_Paint;
            // 
            // trackBarAlpha
            // 
            trackBarAlpha.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            trackBarAlpha.Location = new Point(628, 34);
            trackBarAlpha.Maximum = 45;
            trackBarAlpha.Minimum = -45;
            trackBarAlpha.Name = "trackBarAlpha";
            trackBarAlpha.Size = new Size(120, 56);
            trackBarAlpha.TabIndex = 1;
            trackBarAlpha.Scroll += trackBarAlpha_Scroll;
            // 
            // trackBarBeta
            // 
            trackBarBeta.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            trackBarBeta.Location = new Point(628, 107);
            trackBarBeta.Maximum = 90;
            trackBarBeta.Name = "trackBarBeta";
            trackBarBeta.Size = new Size(120, 56);
            trackBarBeta.TabIndex = 2;
            trackBarBeta.Scroll += trackBarBeta_Scroll;
            // 
            // trackBarResolution
            // 
            trackBarResolution.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            trackBarResolution.Location = new Point(628, 189);
            trackBarResolution.Maximum = 80;
            trackBarResolution.Minimum = 1;
            trackBarResolution.Name = "trackBarResolution";
            trackBarResolution.Size = new Size(120, 56);
            trackBarResolution.TabIndex = 3;
            trackBarResolution.Value = 12;
            trackBarResolution.Scroll += trackBarResolution_Scroll;
            // 
            // radioButtonWireframe
            // 
            radioButtonWireframe.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            radioButtonWireframe.AutoSize = true;
            radioButtonWireframe.Checked = true;
            radioButtonWireframe.Location = new Point(634, 425);
            radioButtonWireframe.Name = "radioButtonWireframe";
            radioButtonWireframe.Size = new Size(100, 24);
            radioButtonWireframe.TabIndex = 4;
            radioButtonWireframe.TabStop = true;
            radioButtonWireframe.Text = "Wireframe";
            radioButtonWireframe.CheckedChanged += radioButtonMode_CheckedChanged;
            // 
            // radioButtonFill
            // 
            radioButtonFill.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            radioButtonFill.AutoSize = true;
            radioButtonFill.Location = new Point(637, 455);
            radioButtonFill.Name = "radioButtonFill";
            radioButtonFill.Size = new Size(49, 24);
            radioButtonFill.TabIndex = 5;
            radioButtonFill.Text = "Fill";
            radioButtonFill.CheckedChanged += radioButtonMode_CheckedChanged;
            // 
            // AlphaLabel
            // 
            AlphaLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            AlphaLabel.AutoSize = true;
            AlphaLabel.Location = new Point(626, 11);
            AlphaLabel.Name = "AlphaLabel";
            AlphaLabel.Size = new Size(48, 20);
            AlphaLabel.TabIndex = 6;
            AlphaLabel.Text = "Alpha";
            // 
            // BetaLabel
            // 
            BetaLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BetaLabel.AutoSize = true;
            BetaLabel.Location = new Point(628, 84);
            BetaLabel.Name = "BetaLabel";
            BetaLabel.Size = new Size(39, 20);
            BetaLabel.TabIndex = 7;
            BetaLabel.Text = "Beta";
            // 
            // ResolutionLabel
            // 
            ResolutionLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ResolutionLabel.AutoSize = true;
            ResolutionLabel.Location = new Point(626, 166);
            ResolutionLabel.Name = "ResolutionLabel";
            ResolutionLabel.Size = new Size(79, 20);
            ResolutionLabel.TabIndex = 8;
            ResolutionLabel.Text = "Resolution";
            // 
            // ControlCheckBox
            // 
            ControlCheckBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ControlCheckBox.AutoSize = true;
            ControlCheckBox.Location = new Point(637, 233);
            ControlCheckBox.Name = "ControlCheckBox";
            ControlCheckBox.Size = new Size(123, 24);
            ControlCheckBox.TabIndex = 9;
            ControlCheckBox.Text = "Control Points";
            ControlCheckBox.UseVisualStyleBackColor = true;
            ControlCheckBox.CheckedChanged += ControlCheckBox_CheckedChanged;
            // 
            // kdTrackbar
            // 
            kdTrackbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            kdTrackbar.Location = new Point(775, 34);
            kdTrackbar.Maximum = 100;
            kdTrackbar.Name = "kdTrackbar";
            kdTrackbar.Size = new Size(130, 56);
            kdTrackbar.TabIndex = 10;
            kdTrackbar.Value = 50;
            kdTrackbar.Scroll += kdTrackbar_Scroll;
            // 
            // ksTrackBar
            // 
            ksTrackBar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ksTrackBar.Location = new Point(775, 107);
            ksTrackBar.Maximum = 100;
            ksTrackBar.Name = "ksTrackBar";
            ksTrackBar.Size = new Size(130, 56);
            ksTrackBar.TabIndex = 11;
            ksTrackBar.Value = 50;
            ksTrackBar.Scroll += ksTrackBar_Scroll;
            // 
            // mTrackBar
            // 
            mTrackBar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            mTrackBar.Location = new Point(775, 189);
            mTrackBar.Maximum = 100;
            mTrackBar.Minimum = 1;
            mTrackBar.Name = "mTrackBar";
            mTrackBar.Size = new Size(130, 56);
            mTrackBar.SmallChange = 5;
            mTrackBar.TabIndex = 12;
            mTrackBar.Value = 20;
            mTrackBar.Scroll += mTrackBar_Scroll;
            // 
            // kdLabel
            // 
            kdLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            kdLabel.AutoSize = true;
            kdLabel.Location = new Point(777, 11);
            kdLabel.Name = "kdLabel";
            kdLabel.Size = new Size(25, 20);
            kdLabel.TabIndex = 13;
            kdLabel.Text = "kd";
            // 
            // ksLabel
            // 
            ksLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ksLabel.AutoSize = true;
            ksLabel.Location = new Point(775, 84);
            ksLabel.Name = "ksLabel";
            ksLabel.Size = new Size(22, 20);
            ksLabel.TabIndex = 14;
            ksLabel.Text = "ks";
            // 
            // mLabel
            // 
            mLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            mLabel.AutoSize = true;
            mLabel.Location = new Point(777, 166);
            mLabel.Name = "mLabel";
            mLabel.Size = new Size(22, 20);
            mLabel.TabIndex = 15;
            mLabel.Text = "m";
            // 
            // ColorButton
            // 
            ColorButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ColorButton.Location = new Point(637, 485);
            ColorButton.Name = "ColorButton";
            ColorButton.Size = new Size(123, 29);
            ColorButton.TabIndex = 16;
            ColorButton.Text = "Light Color";
            ColorButton.UseVisualStyleBackColor = true;
            ColorButton.Click += ColorButton_Click;
            // 
            // TextureButton
            // 
            TextureButton.Location = new Point(634, 555);
            TextureButton.Name = "TextureButton";
            TextureButton.Size = new Size(126, 29);
            TextureButton.TabIndex = 17;
            TextureButton.Text = "Texture";
            TextureButton.UseVisualStyleBackColor = true;
            TextureButton.Click += TextureButton_Click;
            // 
            // StopLightButton
            // 
            StopLightButton.AutoSize = true;
            StopLightButton.Location = new Point(637, 264);
            StopLightButton.Name = "StopLightButton";
            StopLightButton.Size = new Size(99, 24);
            StopLightButton.TabIndex = 18;
            StopLightButton.Text = "Stop Light";
            StopLightButton.UseVisualStyleBackColor = true;
            StopLightButton.CheckedChanged += StopLightButton_CheckedChanged;
            // 
            // ObjectColorButton
            // 
            ObjectColorButton.Location = new Point(637, 520);
            ObjectColorButton.Name = "ObjectColorButton";
            ObjectColorButton.Size = new Size(123, 29);
            ObjectColorButton.TabIndex = 19;
            ObjectColorButton.Text = "Object Color";
            ObjectColorButton.UseVisualStyleBackColor = true;
            ObjectColorButton.Click += ObjectColorButton_Click;
            // 
            // ZTrackBar
            // 
            ZTrackBar.Location = new Point(626, 363);
            ZTrackBar.Maximum = 5;
            ZTrackBar.Minimum = -5;
            ZTrackBar.Name = "ZTrackBar";
            ZTrackBar.Size = new Size(250, 56);
            ZTrackBar.TabIndex = 20;
            ZTrackBar.Scroll += trackBar1_Scroll;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(634, 340);
            label1.Name = "label1";
            label1.Size = new Size(174, 20);
            label1.TabIndex = 21;
            label1.Text = "Lightsource Z coordinate";
            // 
            // NormalVectorCheckBox
            // 
            NormalVectorCheckBox.AutoSize = true;
            NormalVectorCheckBox.Location = new Point(637, 294);
            NormalVectorCheckBox.Name = "NormalVectorCheckBox";
            NormalVectorCheckBox.Size = new Size(184, 24);
            NormalVectorCheckBox.TabIndex = 22;
            NormalVectorCheckBox.Text = "Modify Normal Vectors";
            NormalVectorCheckBox.UseVisualStyleBackColor = true;
            NormalVectorCheckBox.CheckedChanged += NormalVectorCheckBox_CheckedChanged;
            // 
            // NormalMapButton
            // 
            NormalMapButton.Location = new Point(634, 590);
            NormalMapButton.Name = "NormalMapButton";
            NormalMapButton.Size = new Size(126, 29);
            NormalMapButton.TabIndex = 23;
            NormalMapButton.Text = "NormalMap";
            NormalMapButton.UseVisualStyleBackColor = true;
            NormalMapButton.Click += NormalMapButton_Click;
            // 
            // Form1
            // 
            ClientSize = new Size(906, 650);
            Controls.Add(NormalMapButton);
            Controls.Add(NormalVectorCheckBox);
            Controls.Add(label1);
            Controls.Add(ZTrackBar);
            Controls.Add(ObjectColorButton);
            Controls.Add(StopLightButton);
            Controls.Add(TextureButton);
            Controls.Add(ColorButton);
            Controls.Add(mLabel);
            Controls.Add(ksLabel);
            Controls.Add(kdLabel);
            Controls.Add(mTrackBar);
            Controls.Add(ksTrackBar);
            Controls.Add(kdTrackbar);
            Controls.Add(ControlCheckBox);
            Controls.Add(ResolutionLabel);
            Controls.Add(BetaLabel);
            Controls.Add(AlphaLabel);
            Controls.Add(pictureBoxCanvas);
            Controls.Add(trackBarAlpha);
            Controls.Add(trackBarBeta);
            Controls.Add(trackBarResolution);
            Controls.Add(radioButtonWireframe);
            Controls.Add(radioButtonFill);
            Name = "Form1";
            Text = "Bezier Surface Viewer";
            ((System.ComponentModel.ISupportInitialize)pictureBoxCanvas).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarAlpha).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarBeta).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarResolution).EndInit();
            ((System.ComponentModel.ISupportInitialize)kdTrackbar).EndInit();
            ((System.ComponentModel.ISupportInitialize)ksTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)mTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)ZTrackBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label AlphaLabel;
        private Label BetaLabel;
        private Label ResolutionLabel;
        private CheckBox ControlCheckBox;
        private TrackBar kdTrackbar;
        private TrackBar ksTrackBar;
        private TrackBar mTrackBar;
        private Label kdLabel;
        private Label ksLabel;
        private Label mLabel;
        private Button ColorButton;
        private Button TextureButton;
        private CheckBox StopLightButton;
        private Button ObjectColorButton;
        private TrackBar ZTrackBar;
        private Label label1;
        private CheckBox NormalVectorCheckBox;
        private Button NormalMapButton;
    }
}
