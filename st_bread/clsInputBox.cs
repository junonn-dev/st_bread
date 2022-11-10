using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace st_bread
{
    class clsInputBox
    {

        public static DialogResult InputBox(string title, string promptText, ref string value,string sKeyBoard)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            textBox.PasswordChar = '*';


            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;


            try
            {
                string touchKeyboardPath = sKeyBoard;
                Process.Start(touchKeyboardPath);
            }
            catch (Exception e)
            { }

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }
        
        public static DialogResult QABox(string title, string promptText)
        {
            Form form = new Form();
            Label label = new Label();
            //TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.FormBorderStyle = FormBorderStyle.None;
            form.BackgroundImage = Properties.Resources.bg_cash;
            form.BackgroundImageLayout = ImageLayout.Stretch;

            buttonOk.BackgroundImage = Properties.Resources.bg_btntype03;
            buttonCancel.BackgroundImage = Properties.Resources.bg_btntype03;
            buttonCancel.BackgroundImageLayout = ImageLayout.Stretch;
            buttonOk.BackgroundImageLayout = ImageLayout.Stretch;

            label.BackColor = Color.Transparent;
            label.Font = new Font("나눔고딕", 14, FontStyle.Regular);

            
            //form.Text = title;
            label.Text = promptText;
            
            buttonOk.Text = "OK";
            buttonCancel.Text = "NO";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            //textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(210, 72, 139, 50);
            buttonCancel.SetBounds(350, 72, 139, 50);

            label.AutoSize = true;
           // textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
           // textBox.PasswordChar = '*';

            form.ClientSize = new Size(550, 150);

            form.Controls.AddRange(new Control[] { label,  buttonOk, buttonCancel });
            //form.ClientSize = new Size(Math.Max(500, label.Right + 10), form.ClientSize.Height);
            //form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            //form.MinimizeBox = false;
            //form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
           // value = textBox.Text;
            return dialogResult;
        }




    }
}
