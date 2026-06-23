using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using IzinTakip.Core.Abstractions;
using IzinTakip.Core.Models;

namespace IzinTakipSistemi
{
    public partial class LoginForm : Form
    {
        private readonly IAuthService _authService;
        private readonly IIzinService _izinService;
        private readonly IRaporService _raporService;

        /// <summary>
        /// LoginForm artık servisleri dışarıdan alır (DIP).
        /// Giriş sonrası açılacak formlara da aynı servisler enjekte edilir.
        /// </summary>
        public LoginForm(IAuthService authService, IIzinService izinService, IRaporService raporService)
        {
            _authService = authService;
            _izinService = izinService;
            _raporService = raporService;

            InitializeComponent();
            this.VisibleChanged += LoginForm_VisibleChanged;
            this.Load += LoginForm_Load;
        }

        private void LoginForm_Load(object? sender, EventArgs e)
        {
            try
            {
                string iconPath = Path.Combine(AppContext.BaseDirectory, "BossaIconFormat.ico");
                if (File.Exists(iconPath))
                    this.Icon = new System.Drawing.Icon(iconPath);
            }
            catch { /* İkon yüklenemezse varsayılan kullan */ }
        }

        // Tekrar gösterildiğinde alanları temizle (logout sonrası)
        private void LoginForm_VisibleChanged(object? sender, EventArgs e)
        {
            if (this.Visible)
            {
                txtSicilNo.Clear();
                txtPassword.Clear();
                txtSicilNo.Focus();
            }
        }

        private async void btnLogin_Click(object? sender, EventArgs e)
        {
            string sicilNo = txtSicilNo.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(sicilNo) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Lütfen Sicil No ve şifre giriniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataRow? row = await _authService.GirisYapAsync(sicilNo, password);

                if (row != null)
                {
                    Session.CurrentEmployeeID = Convert.ToInt32(row["EmployeeID"]);
                    Session.CurrentFullName = row["FullName"].ToString();
                    Session.CurrentDepartmentID = Convert.ToInt32(row["DepartmentID"]);
                    Session.CurrentDepartmentName = row["DepartmentName"].ToString();
                    Session.CurrentRole = row["Role"].ToString();
                    Session.CurrentSicilNo = row["SicilNo"].ToString();

                    this.Hide();

                    if (Session.CurrentRole == "Amir")
                    {
                        AmirForm amirForm = new AmirForm(_izinService, _raporService);
                        amirForm.Show();
                    }
                    else if (Session.CurrentRole == "Yönetici")
                    {
                        YoneticiForm yoneticiForm = new YoneticiForm(_izinService, _raporService);
                        yoneticiForm.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Hatalı Sicil No veya şifre!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı hatası: " + ex.Message, "Kritik Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoginForm_FormClosed(object? sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}