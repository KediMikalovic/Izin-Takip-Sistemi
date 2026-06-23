import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../core/api_client.dart';
import '../models/izin_model.dart';

// İzin geçmişi provider
final izinGecmisiProvider = FutureProvider<List<IzinModel>>((ref) async {
  final response = await ApiClient.izinGecmisi();
  final list = response.data as List;
  return list.map((e) => IzinModel.fromJson(e)).toList();
});

// İzin talebi durumu
class IzinTalepState {
  final bool isLoading;
  final bool isBasarili;
  final String? mesaj;
  final String? hata;

  const IzinTalepState({
    this.isLoading  = false,
    this.isBasarili = false,
    this.mesaj,
    this.hata,
  });

  IzinTalepState copyWith({
    bool? isLoading,
    bool? isBasarili,
    String? mesaj,
    String? hata,
  }) {
    return IzinTalepState(
      isLoading:  isLoading  ?? this.isLoading,
      isBasarili: isBasarili ?? this.isBasarili,
      mesaj:      mesaj,
      hata:       hata,
    );
  }
}

class IzinTalepNotifier extends StateNotifier<IzinTalepState> {
  IzinTalepNotifier() : super(const IzinTalepState());

  Future<void> talepOlustur({
    required int amirId,
    required DateTime baslangic,
    required DateTime bitis,
    required int kalanIzin,
  }) async {
    state = state.copyWith(isLoading: true);

    try {
      await ApiClient.izinTalebiOlustur(
        amirId:    amirId,
        baslangic: baslangic.toIso8601String(),
        bitis:     bitis.toIso8601String(),
        kalanIzin: kalanIzin,
      );

      state = state.copyWith(
        isLoading:  false,
        isBasarili: true,
        mesaj:      'İzin talebiniz başarıyla oluşturuldu.',
      );
    } catch (e) {
      state = state.copyWith(
        isLoading: false,
        hata:      'İzin talebi oluşturulamadı. Lütfen tekrar deneyin.',
      );
    }
  }

  void sifirla() {
    state = const IzinTalepState();
  }
}

final izinTalepProvider =
    StateNotifierProvider<IzinTalepNotifier, IzinTalepState>(
  (ref) => IzinTalepNotifier(),
);

// Profil modeli
class PersonelProfil {
  final int employeeId;
  final String fullName;
  final String sicilNo;
  final String departman;
  final int kidemYil;
  final int toplamHak;
  final int kullanilanIzin;
  final int kalanIzin;

  const PersonelProfil({
    required this.employeeId,
    required this.fullName,
    required this.sicilNo,
    required this.departman,
    required this.kidemYil,
    required this.toplamHak,
    required this.kullanilanIzin,
    required this.kalanIzin,
  });

  factory PersonelProfil.fromJson(Map<String, dynamic> json) {
    return PersonelProfil(
      employeeId:    json['employeeId'] ?? 0,
      fullName:      json['fullName'] ?? '',
      sicilNo:       json['sicilNo'] ?? '',
      departman:     json['departman'] ?? '',
      kidemYil:      json['kidemYil'] ?? 0,
      toplamHak:     json['toplamHak'] ?? 0,
      kullanilanIzin: json['kullanilanIzin'] ?? 0,
      kalanIzin:     json['kalanIzin'] ?? 0,
    );
  }
}

// Profil provider
final profilProvider = FutureProvider<PersonelProfil>((ref) async {
  final response = await ApiClient.profilGetir();
  return PersonelProfil.fromJson(response.data);
});