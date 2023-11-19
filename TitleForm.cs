namespace Delete_Push_Pull
{
    internal class TitleForm : Form
    {
        private TextBox textBoxTitle;
        private Button buttonOK;
        private Button buttonCancel;

        public TitleForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Initialize UI elements
            textBoxTitle = new TextBox();
            buttonOK = new Button();
            buttonCancel = new Button();

            // Set properties for UI elements
            textBoxTitle.Location = new System.Drawing.Point(10, 10);
            textBoxTitle.Size = new System.Drawing.Size(200, 20);

            buttonOK.Location = new System.Drawing.Point(10, 40);
            buttonOK.Size = new System.Drawing.Size(75, 23);
            buttonOK.Text = "OK";
            buttonOK.DialogResult = DialogResult.OK;
            buttonOK.Click += ButtonOK_Click;

            buttonCancel.Location = new System.Drawing.Point(90, 40);
            buttonCancel.Size = new System.Drawing.Size(75, 23);
            buttonCancel.Text = "Cancel";
            buttonCancel.DialogResult = DialogResult.Cancel;

            // Set form properties
            this.Text = "Enter Title";
            this.ClientSize = new System.Drawing.Size(220, 80);
            this.Controls.Add(textBoxTitle);
            this.Controls.Add(buttonOK);
            this.Controls.Add(buttonCancel);
        }

        public string Title
        {
            get { return textBoxTitle.Text; }
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}