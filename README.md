# İzin Takip Sistemi

Türk İş Kanunu'na uygun, çok katmanlı bir yıllık izin yönetim sistemidir. Windows Forms masaüstü uygulaması ve mobil istemciler için ASP.NET Core REST API olmak üzere iki farklı sunum katmanı, ortak bir iş mantığı kütüphanesi (`IzinTakip.Core`) üzerine inşa edilmiştir.

---

## Özellikler

- **Rol tabanlı yetkilendirme** — Personel, Amir, Yönetici rolleri; her rol kendi ekranını ve endpoint'ini görür
- **İki aşamalı onay akışı** — Personel talebi → Amir onayı → Yönetici onayı
- **Türk İş Kanunu izin hakkı** — Kıdeme göre otomatik hesaplama (1–4 yıl: 14 gün, 5–14 yıl: 20 gün, 15+ yıl: 26 gün)
- **Avans izin** — Bakiyesi tükenmiş personel için gelecek yıl hakkından avans kullanımı
- **İş günü hesabı** — Hafta sonları ve Türk resmi tatilleri (Hicri takvim dini bayramları dahil) otomatik hariç tutulur
- **Departman kotası** — Aynı anda aynı departmandan en fazla 2 kişi izinli olabilir
- **Audit log** — Tüm işlemler (ekleme, onaylama, reddetme, iptal) veritabanına kaydedilir
- **Excel raporu** — ClosedXML ile izin raporları dışa aktarılabilir
- **JWT kimlik doğrulama** — API katmanı HMAC-SHA256 imzalı JWT token kullanır
- **Güvenli parola saklama** — PBKDF2-SHA256 (100.000 iterasyon + salt)

---

## Proje Yapısı

```
IzinTakipSistemi/
├── IzinTakip.Core/          # Paylaşılan iş mantığı kütüphanesi
│   ├── Abstractions/        # IDbHelper, IAuthService, IIzinService, IRaporService
│   ├── DataAccess/          # SqlHelper (ADO.NET), tatil repository'leri
│   ├── Services/            # AuthService, IzinService, RaporService
│   ├── Helpers/             # İş günü hesabı, izin hakkı hesabı
│   ├── Security/            # PBKDF2 şifreleme
│   └── models/              # Entity ve DTO'lar
│
├── IzinTakipSistemi/        # Windows Forms masaüstü uygulaması (net8.0-windows)
│   └── UI Forms/            # LoginForm, AmirForm, YoneticiForm, RaporForm, LogForm
│
└── IzinTakip.Api/           # ASP.NET Core REST API (net8.0)
    ├── Controllers/         # AuthController, IzinController
    └── Helpers/             # JwtHelper
```

### Bağımlılık Grafiği

```
IzinTakipSistemi (WinForms)  ──┐
                               ├──► IzinTakip.Core ──► SQL Server (LocalDB)
IzinTakip.Api (REST API)     ──┘
```

---

## Teknolojiler

| Katman | Teknoloji |
|---|---|
| Masaüstü UI | .NET 8 Windows Forms |
| REST API | ASP.NET Core 8 |
| Veritabanı | SQL Server (LocalDB geliştirme, SQL Server üretim) |
| ORM / Veri Erişimi | ADO.NET (ham SQL) |
| Kimlik Doğrulama (API) | JWT Bearer — `Microsoft.AspNetCore.Authentication.JwtBearer` |
| Tatil Hesabı | `Nager.Date` + HijriCalendar |
| Excel Raporu | `ClosedXML` |
| API Dokümantasyon | Swagger / Swashbuckle |

---

## Kurulum

### Gereksinimler

- .NET 8 SDK
- SQL Server veya SQL Server LocalDB

### 1. Repoyu klonla

```bash
git clone https://github.com/KULLANICI_ADIN/IzinTakipSistemi.git
cd IzinTakipSistemi
```

### 2. Config dosyalarını oluştur

Her iki projede de `appsettings.Example.json` dosyasını kopyalayıp `appsettings.json` olarak yeniden adlandır, ardından kendi ortamına göre düzenle:

```bash
cp IzinTakipSistemi/appsettings.Example.json IzinTakipSistemi/appsettings.json
cp IzinTakip.Api/appsettings.Example.json IzinTakip.Api/appsettings.json
```

`IzinTakip.Api/appsettings.json` içindeki `SecretKey` değerini güçlü, rastgele bir string ile değiştir:

```json
"JwtSettings": {
  "SecretKey": "BURAYA-EN-AZ-32-KARAKTER-GIZLI-ANAHTAR-GIRINIZ"
}
```

### 3. Veritabanını hazırla

```sql
-- LocalDB'de IzinTakipDB adında bir veritabanı oluştur, ardından:
-- 1. Örnek veriyi yükle
-- DummyData.sql

-- 2. Performans indekslerini uygula
-- Indexes.sql
```

SQL Server Management Studio veya `sqlcmd` ile çalıştırabilirsin:

```bash
sqlcmd -S "(localdb)\MSSQLLocalDB" -d IzinTakipDB -i DummyData.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -d IzinTakipDB -i Indexes.sql
```

### 4. Uygulamayı çalıştır

**Masaüstü uygulaması:**
```bash
dotnet run --project IzinTakipSistemi/IzinTakipSistemi.csproj
```

**REST API:**
```bash
dotnet run --project IzinTakip.Api/IzinTakip.Api.csproj
```

API ayağa kalktıktan sonra Swagger arayüzüne `http://localhost:5000/swagger` adresinden erişebilirsin.

---

## Varsayılan Giriş Bilgileri

Seed verisi aşağıdaki test hesaplarını içerir:

| Rol | Sicil No | Şifre |
|---|---|---|
| Yönetici | `Y001` | `bossa2024` |
| Amir | `A001` – `A005` | `amir123` |
| Personel | `P001` – `P010` | `123456` |

> **Uyarı:** Bu bilgiler yalnızca geliştirme ortamı içindir. Üretimde mutlaka değiştirilmeli.

---

## İzin Akışı

```
Personel talebi oluşturur
        │
        ▼
  [Amir Onayında]
        │
   Amir inceler
   ┌────┴────┐
Onaylar    Reddeder
   │
   ▼
[Beklemede]
   │
Yönetici inceler
   ┌────┴────┐
Onaylar    Reddeder
   │
   ▼
[Onaylandı]
```

Yönetici veya Amir doğrudan izin girebilir; bu durumda talep "Beklemede" statüsüyle başlar.

---

## API Endpoint Özeti

| Method | Endpoint | Yetki | Açıklama |
|---|---|---|---|
| POST | `/api/auth/login` | Herkese açık | JWT token al |
| GET | `/api/izin/profil` | Tüm roller | Kendi profil ve izin özeti |
| GET | `/api/izin/gecmis` | Tüm roller | Kendi izin geçmişi |
| POST | `/api/izin/personel-talep` | Personel | Yeni izin talebi oluştur |
| GET | `/api/izin/amir-bekleyenler` | Amir, Yönetici | Amir onayındaki talepler |
| POST | `/api/izin/{id}/amir-onayla` | Amir | Talebi yöneticiye ilet |
| GET | `/api/izin/talepler` | Yönetici | Tüm talepleri filtreli getir |
| POST | `/api/izin/{id}/onayla` | Yönetici | İzni onayla |
| POST | `/api/izin/{id}/reddet` | Yönetici | İzni reddet |

---

## Lisans

MIT
