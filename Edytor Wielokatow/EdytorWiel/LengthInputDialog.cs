namespace EdytorWiel
{
    public partial class LengthInputDialog : Form
    {
        public double EnteredLength { get; private set; }
        private double initialLength;

        public LengthInputDialog(double currentLength)
        {
            InitializeComponent();

            // Ustawienie początkowej wartości TrackBar w zależności od aktualnej długości krawędzi
            EnteredLength = currentLength;
            initialLength = currentLength;
            lengthTrackBar.Value = (int)currentLength;
            lengthLabel.Text = $"Długość: {currentLength:F2}";
        }

        // Obsługa zmiany wartości suwaka
        private void lengthTrackBar_Scroll(object sender, EventArgs e)
        {
            // Aktualizacja etykiety na podstawie pozycji suwaka
            EnteredLength = lengthTrackBar.Value;
            lengthLabel.Text = $"Długość: {EnteredLength:F2}";
        }

        // Obsługa przycisku OK
        private void okButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // Obsługa przycisku Anuluj
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
