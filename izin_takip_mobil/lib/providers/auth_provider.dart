import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../core/api_client.dart';
import '../core/auth_storage.dart';
import 'izin_provider.dart';

// Kullanıcı durumu
class AuthState {
  final bool isLoading;
  final bool isLoggedIn;
  final String? error;
  final String? role;
  final String? fullName;

  const AuthState({
    this.isLoading = false,
    this.isLoggedIn = false,
    this.error,
    this.role,
    this.fullName,
  });

  AuthState copyWith({
    bool? isLoading,
    bool? isLoggedIn,
    String? error,
    String? role,
    String? fullName,
  }) {
    return AuthState(
      isLoading:  isLoading  ?? this.isLoading,
      isLoggedIn: isLoggedIn ?? this.isLoggedIn,
      error:      error,
      role:       role       ?? this.role,
      fullName:   fullName   ?? this.fullName,
    );
  }
}

class AuthNotifier extends StateNotifier<AuthState> {
  final Ref _ref;

  AuthNotifier(this._ref) : super(const AuthState());

  Future<void> girisYap(String sicilNo, String password) async {
    state = state.copyWith(isLoading: true);

    try {
      final response = await ApiClient.login(sicilNo, password);
      final data = response.data;

      // Sadece Personel girişine izin ver
      if (data['role'] != 'Personel') {
        state = state.copyWith(
          isLoading: false,
          error: 'Bu uygulama sadece personel girişi içindir.',
        );
        return;
      }

      await AuthStorage.kaydet(
        token:        data['token'],
        role:         data['role'],
        fullName:     data['fullName'],
        employeeId:   data['employeeId'],
        departmentId: data['departmentId'],
      );

      // Önceki kullanıcının cache'ini temizle
      _ref.invalidate(profilProvider);
      _ref.invalidate(izinGecmisiProvider);

      state = state.copyWith(
        isLoading:  false,
        isLoggedIn: true,
        role:       data['role'],
        fullName:   data['fullName'],
      );
    } catch (e) {
      state = state.copyWith(
        isLoading: false,
        error: 'Sicil no veya şifre hatalı.',
      );
    }
  }

  Future<void> cikisYap() async {
    await AuthStorage.temizle();
    _ref.invalidate(profilProvider);
    _ref.invalidate(izinGecmisiProvider);
    state = const AuthState();
  }
}

final authProvider = StateNotifierProvider<AuthNotifier, AuthState>(
  (ref) => AuthNotifier(ref),
);