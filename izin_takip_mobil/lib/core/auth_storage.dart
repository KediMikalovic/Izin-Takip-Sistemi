import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class AuthStorage {
  static const _storage = FlutterSecureStorage();
  static const _tokenKey = 'jwt_token';
  static const _roleKey = 'user_role';
  static const _nameKey = 'user_name';
  static const _employeeIdKey = 'employee_id';
  static const _departmentIdKey = 'department_id';

  static Future<void> kaydet({
    required String token,
    required String role,
    required String fullName,
    required int employeeId,
    required int departmentId,
  }) async {
    await _storage.write(key: _tokenKey, value: token);
    await _storage.write(key: _roleKey, value: role);
    await _storage.write(key: _nameKey, value: fullName);
    await _storage.write(key: _employeeIdKey, value: employeeId.toString());
    await _storage.write(key: _departmentIdKey, value: departmentId.toString());
  }

  static Future<String?> tokenGetir() => _storage.read(key: _tokenKey);
  static Future<String?> roleGetir() => _storage.read(key: _roleKey);
  static Future<String?> adGetir() => _storage.read(key: _nameKey);
  static Future<int?> employeeIdGetir() async {
    final val = await _storage.read(key: _employeeIdKey);
    return val != null ? int.tryParse(val) : null;
  }
  static Future<int?> departmentIdGetir() async {
    final val = await _storage.read(key: _departmentIdKey);
    return val != null ? int.tryParse(val) : null;
  }

  static Future<void> temizle() => _storage.deleteAll();
}