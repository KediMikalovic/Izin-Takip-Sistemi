# İzin Takip — Mobil Uygulama

İzin Takip Sistemi'nin Flutter ile geliştirilmiş mobil istemcisi. Yalnızca **Personel** rolündeki kullanıcılar için tasarlanmıştır; giriş yapan personel izin geçmişini görüntüleyebilir ve yeni izin talebi oluşturabilir.

Backend olarak aynı repodaki [IzinTakip.Api](../IzinTakip.Api) projesine bağlanır.

---

## Ekranlar

| Ekran | Açıklama |
|---|---|
| **Giriş** | Sicil no + şifre ile JWT tabanlı kimlik doğrulama |
| **Ana Sayfa** | Profil özeti, izin bakiyesi (toplam / kullanılan / kalan), son izinler |
| **İzin Geçmişi** | Tüm izin kayıtları durum renk kodlamasıyla listelenir |
| **İzin Talebi** | Tarih seçici ile yeni talep oluşturma; bakiye kontrolü anlık yapılır |

---

## Teknolojiler

| Paket | Kullanım |
|---|---|
| `flutter_riverpod` | State management |
| `dio` | HTTP istemcisi, JWT interceptor |
| `flutter_secure_storage` | Token ve oturum bilgilerini güvenli saklama |
| `go_router` | Navigasyon |
| `flutter_localizations` | Türkçe tarih seçici ve lokalizasyon |

---

## Kurulum

### Gereksinimler

- Flutter SDK 3.x
- Android emülatör veya fiziksel cihaz (Android 6.0+)
- Çalışan bir [IzinTakip.Api](../IzinTakip.Api) örneği

### 1. Bağımlılıkları yükle

```bash
cd izin_takip_mobil
flutter pub get
```

### 2. API adresini ayarla

`lib/core/api_client.dart` dosyasındaki `baseUrl` değerini ortamına göre güncelle:

```dart
// Android emülatör için (localhost = 10.0.2.2)
static const String baseUrl = 'http://10.0.2.2:5048/api';

// Fiziksel cihaz için bilgisayarın yerel IP'sini kullan
static const String baseUrl = 'http://192.168.1.X:5048/api';

// Üretim
static const String baseUrl = 'https://api.sirket.com/api';
```

### 3. Uygulamayı çalıştır

```bash
flutter run
```

---

## Mimari

```
lib/
├── core/
│   ├── api_client.dart      # Dio istemcisi, JWT interceptor, tüm API çağrıları
│   └── auth_storage.dart    # flutter_secure_storage ile token yönetimi
├── models/
│   └── izin_model.dart      # İzin DTO'su
├── providers/
│   ├── auth_provider.dart   # Giriş / çıkış state'i (StateNotifier)
│   └── izin_provider.dart   # Profil ve izin verileri (FutureProvider)
└── screens/
    ├── login_screen.dart
    ├── home_screen.dart
    ├── gecmis_screen.dart
    └── talep_screen.dart
```

State management: Riverpod `StateNotifierProvider` (auth) + `FutureProvider` (veri çekme).

---

## Rol Kısıtlaması

Uygulama yalnızca `Personel` rolündeki hesapların girişine izin verir. Amir veya Yönetici hesabıyla giriş yapılmaya çalışıldığında hata mesajı gösterilir. Bu roller için masaüstü uygulaması kullanılmalıdır.

---

## Geliştirme Notları

- `AmirId` şu an sabit `1` olarak gönderilmektedir. İleride personelin departmanından API üzerinden otomatik çekilmesi planlanmaktadır (`talep_screen.dart` içindeki `TODO`).
- Üretim ortamında `baseUrl` HTTPS olmalı ve `appsettings.json` içindeki JWT `SecretKey` güçlü bir değerle değiştirilmelidir.
