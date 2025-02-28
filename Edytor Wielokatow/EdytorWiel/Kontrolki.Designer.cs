namespace EdytorWiel
{
    partial class Kontrolki
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Kontrolki));
            richTextBox1 = new RichTextBox();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            richTextBox1.Location = new Point(-3, -6);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(701, 291);
            richTextBox1.TabIndex = 1;
            richTextBox1.Text = "Dodawanie Wierzchołków: lewy przycisk myszy" + Environment.NewLine + Environment.NewLine +
                "Domknięcie Wielokąta: Kliknięcie na pierwszy wierzchołek" + Environment.NewLine + Environment.NewLine +
                "Ustawienia Wierzchołków/Krawędzi: Prawy Przycisk myszy" + Environment.NewLine + Environment.NewLine +
                "Przesuwanie Wierzchołka/Krawędzi/Figury: przytrzymanie lewego przycisku myszy na tym obiekcie" + Environment.NewLine + Environment.NewLine +
                "Dodanie Krzywej Beziera: Wybranie lewym przyciskiem myszy lokacji punktów kontrolnych" + Environment.NewLine +
                "(Bez zmiany ciągłości w wierzchołkach i tak się automatycznie poprawią na C1)";

            // 
            // Kontrolki
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(694, 280);
            Controls.Add(richTextBox1);
            Name = "Kontrolki";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Kontrolki";
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox richTextBox1;
    }
}