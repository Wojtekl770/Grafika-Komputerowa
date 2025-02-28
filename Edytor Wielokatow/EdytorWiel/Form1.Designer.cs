namespace EdytorWiel
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            panel1 = new BufferedPanel();
            ResetButton = new Button();
            DefaultButton = new Button();
            ControlButton = new Button();
            AlgorithmButton = new Button();
            BresenhamButton = new Button();
            LoadButton = new Button();
            SaveButton = new Button();
            label1 = new Label();
            DrawLineButton = new Button();
            WUButton = new Button();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Location = new Point(2, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(731, 549);
            panel1.TabIndex = 0;
            // 
            // ResetButton
            // 
            ResetButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ResetButton.Location = new Point(739, 12);
            ResetButton.Name = "ResetButton";
            ResetButton.Size = new Size(137, 50);
            ResetButton.TabIndex = 1;
            ResetButton.Text = "Reset";
            ResetButton.UseVisualStyleBackColor = true;
            ResetButton.Click += ResetButton_Click;
            // 
            // DefaultButton
            // 
            DefaultButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            DefaultButton.Location = new Point(739, 68);
            DefaultButton.Name = "DefaultButton";
            DefaultButton.Size = new Size(137, 50);
            DefaultButton.TabIndex = 2;
            DefaultButton.Text = "Add Default Shape";
            DefaultButton.UseVisualStyleBackColor = true;
            DefaultButton.Click += DefaultButton_Click;
            // 
            // ControlButton
            // 
            ControlButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ControlButton.Location = new Point(739, 124);
            ControlButton.Name = "ControlButton";
            ControlButton.Size = new Size(137, 50);
            ControlButton.TabIndex = 3;
            ControlButton.Text = "Controls Explanation";
            ControlButton.UseVisualStyleBackColor = true;
            ControlButton.Click += ControlButton_Click;
            // 
            // AlgorithmButton
            // 
            AlgorithmButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            AlgorithmButton.Location = new Point(739, 180);
            AlgorithmButton.Name = "AlgorithmButton";
            AlgorithmButton.Size = new Size(137, 50);
            AlgorithmButton.TabIndex = 4;
            AlgorithmButton.Text = "Algorithm Explanation";
            AlgorithmButton.UseVisualStyleBackColor = true;
            AlgorithmButton.Click += AlgorithmButton_Click;
            // 
            // BresenhamButton
            // 
            BresenhamButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BresenhamButton.Location = new Point(739, 436);
            BresenhamButton.Name = "BresenhamButton";
            BresenhamButton.Size = new Size(137, 50);
            BresenhamButton.TabIndex = 5;
            BresenhamButton.Text = "Bresenham lines";
            BresenhamButton.UseVisualStyleBackColor = true;
            BresenhamButton.Click += BresenhamButton_Click;
            // 
            // LoadButton
            // 
            LoadButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            LoadButton.Location = new Point(739, 292);
            LoadButton.Name = "LoadButton";
            LoadButton.Size = new Size(137, 50);
            LoadButton.TabIndex = 6;
            LoadButton.Text = "Load Shape";
            LoadButton.UseVisualStyleBackColor = true;
            LoadButton.Click += LoadButton_Click;
            // 
            // SaveButton
            // 
            SaveButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            SaveButton.Location = new Point(739, 236);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(137, 50);
            SaveButton.TabIndex = 7;
            SaveButton.Text = "Save Shape";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(755, 357);
            label1.Name = "label1";
            label1.Size = new Size(102, 20);
            label1.TabIndex = 8;
            label1.Text = "Drawing Lines";
            // 
            // DrawLineButton
            // 
            DrawLineButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            DrawLineButton.Location = new Point(739, 380);
            DrawLineButton.Name = "DrawLineButton";
            DrawLineButton.Size = new Size(137, 50);
            DrawLineButton.TabIndex = 9;
            DrawLineButton.Text = "DrawLine";
            DrawLineButton.UseVisualStyleBackColor = true;
            DrawLineButton.Click += DrawLineButton_Click;
            // 
            // WUButton
            // 
            WUButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            WUButton.Location = new Point(739, 491);
            WUButton.Name = "WUButton";
            WUButton.Size = new Size(137, 50);
            WUButton.TabIndex = 10;
            WUButton.Text = "WU lines";
            WUButton.UseVisualStyleBackColor = true;
            WUButton.Click += WUButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(882, 553);
            Controls.Add(WUButton);
            Controls.Add(DrawLineButton);
            Controls.Add(label1);
            Controls.Add(SaveButton);
            Controls.Add(LoadButton);
            Controls.Add(BresenhamButton);
            Controls.Add(AlgorithmButton);
            Controls.Add(ControlButton);
            Controls.Add(DefaultButton);
            Controls.Add(ResetButton);
            Controls.Add(panel1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }


        #endregion

        private BufferedPanel panel1; // The BufferedPanel where drawing occurs
        private Button ResetButton; // Button to reset the drawing
        private Button DefaultButton; // Button to add default shapes
        private Button ControlButton;
        private Button AlgorithmButton;
        private Button BresenhamButton;
        private Button LoadButton;
        private Button SaveButton;
        private Label label1;
        private Button DrawLineButton;
        private Button WUButton;
    }
}
