// import 'package:dio/dio.dart';
// import 'auth_storage.dart';

// class ApiClient {
//   // TODO: Üretim ortamında bu URL sunucu adresine güncellenmeli
//   static const String baseUrl = 'https://localhost:7297/api';

//   static Dio _dio() {
//     final dio = Dio(BaseOptions(
//       baseUrl: baseUrl,
//       connectTimeout: const Duration(seconds: 10),
//       receiveTimeout: const Duration(seconds: 10),
//       headers: {'Content-Type': 'application/json'},
//     ));

//     // Her istekte token otomatik eklenir
//     dio.interceptors.add(InterceptorsWrapper(
//       onRequest: (options, handler) async {
//         final token = await AuthStorage.tokenGetir();
//         if (token != null) {
//           options.headers['Authorization'] = 'Bearer $token';
//         }
//         handler.next(options);
//       },
//     ));

//     return dio;
//   }

//   // ── Auth ─────────────────────────────────────────────────

//   static Future<Response> login(String sicilNo, String password) {
//     return _dio().post('/Auth/login', data: {
//       'sicilNo': sicilNo,
//       'password': password,
//     });
//   }

//   // ── İzin ─────────────────────────────────────────────────

//   static Future<Response> izinGecmisi() {
//     return _dio().get('/Izin/gecmis');
//   }

//   static Future<Response> izinTalebiOlustur({
//     required int amirId,
//     required String baslangic,
//     required String bitis,
//     required int kalanIzin,
//   }) {
//     return _dio().post('/Izin/personel-talep', data: {
//       'amirId': amirId,
//       'baslangic': baslangic,
//       'bitis': bitis,
//       'kalanIzin': kalanIzin,
//     });
//   }
// }


import 'dart:io';
import 'package:dio/dio.dart';
import 'auth_storage.dart';

class ApiClient {
  // TODO: Üretim ortamında bu URL sunucu adresine güncellenmeli
  // Emülatörde 10.0.2.2 = bilgisayarın localhost'u
  static const String baseUrl = 'http://10.0.2.2:5048/api';

  static Dio _dio() {
    final dio = Dio(BaseOptions(
      baseUrl: baseUrl,
      connectTimeout: const Duration(seconds: 10),
      receiveTimeout: const Duration(seconds: 10),
      headers: {'Content-Type': 'application/json'},
    ));

    // // TODO: Üretimde kaldırılmalı — self-signed sertifika için
    // (dio.httpClientAdapter as dynamic).onHttpClientCreate =
    //     (HttpClient client) {
    //   client.badCertificateCallback =
    //       (X509Certificate cert, String host, int port) => true;
    //   return client;
    // };

    // Her istekte token otomatik eklenir
    dio.interceptors.add(InterceptorsWrapper(
      onRequest: (options, handler) async {
        final token = await AuthStorage.tokenGetir();
        if (token != null) {
          options.headers['Authorization'] = 'Bearer $token';
        }
        handler.next(options);
      },
    ));

    return dio;
  }

  // ── Auth ─────────────────────────────────────────────────

  static Future<Response> login(String sicilNo, String password) {
    return _dio().post('/Auth/login', data: {
      'sicilNo': sicilNo,
      'password': password,
    });
  }

  // ── İzin ─────────────────────────────────────────────────

  static Future<Response> izinGecmisi() {
    return _dio().get('/Izin/gecmis');
  }

  static Future<Response> izinTalebiOlustur({
    required int amirId,
    required String baslangic,
    required String bitis,
    required int kalanIzin,
  }) {
    return _dio().post('/Izin/personel-talep', data: {
      'amirId':    amirId,
      'baslangic': baslangic,
      'bitis':     bitis,
      'kalanIzin': kalanIzin,
    });
  }
  static Future<Response> profilGetir() {
    return _dio().get('/Izin/profil');
  }
}