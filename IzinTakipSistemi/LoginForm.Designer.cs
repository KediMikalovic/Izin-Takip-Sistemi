namespace IzinTakipSistemi
{
    partial class LoginForm
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
            label1 = new Label();
            txtSicilNo = new TextBox();
            btnLogin = new Button();
            label2 = new Label();
            txtPassword = new TextBox();
            logoPicture = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)logoPicture).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(330, 277);
            label1.Name = "label1";
            label1.Size = new Size(63, 20);
            label1.TabIndex = 0;
            label1.Text = "Sicil No:";
            // 
            // txtSicilNo
            // 
            txtSicilNo.Location = new Point(327, 300);
            txtSicilNo.Name = "txtSicilNo";
            txtSicilNo.Size = new Size(125, 27);
            txtSicilNo.TabIndex = 1;
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(340, 398);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(94, 29);
            btnLogin.TabIndex = 2;
            btnLogin.Text = "Giriş Yap";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(327, 342);
            label2.Name = "label2";
            label2.Size = new Size(42, 20);
            label2.TabIndex = 3;
            label2.Text = "Şifre:";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(327, 365);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(125, 27);
            txtPassword.TabIndex = 4;
            // 
            // logoPicture
            // 
            logoPicture.Image = Properties.Resources.görsel1;
            logoPicture.Location = new Point(63, 2);
            logoPicture.Name = "logoPicture";
            logoPicture.Size = new Size(705, 272);
            logoPicture.TabIndex = 5;
            logoPicture.TabStop = false;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(logoPicture);
            Controls.Add(txtPassword);
            Controls.Add(label2);
            Controls.Add(btnLogin);
            Controls.Add(txtSicilNo);
            Controls.Add(label1);
            Name = "LoginForm";
            Text = "LoginForm";
            ((System.ComponentModel.ISupportInitialize)logoPicture).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtSicilNo;
        private Button btnLogin;
        private Label label2;
        private TextBox txtPassword;
        private PictureBox logoPicture;
    }
}