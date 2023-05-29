using System;
using System.Drawing;
using System.Windows.Forms;


namespace BypassSSLPinning
{
    public class HintTextBox : TextBox
    {
        private string hint;
        private Color hintColor = Color.Gray;
        private Color defaultColor;

        public string Hint
        {
            get { return hint; }
            set
            {
                hint = value;
                RefreshHint();
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            RefreshHint();
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            if (Text == hint)
            {
                Text = string.Empty;
                ForeColor = defaultColor;
            }
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            if (string.IsNullOrWhiteSpace(Text))
            {
                RefreshHint();
            }
        }

        private void RefreshHint()
        {
            if (!IsHandleCreated || !string.IsNullOrEmpty(Text))
                return;

            defaultColor = ForeColor;
            Text = hint;
            ForeColor = hintColor;
        }
    }
}
