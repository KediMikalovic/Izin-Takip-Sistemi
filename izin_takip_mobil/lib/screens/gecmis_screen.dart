import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../models/izin_model.dart';
import '../providers/auth_provider.dart';
import '../providers/izin_provider.dart';

class GecmisScreen extends ConsumerWidget {
  const GecmisScreen({super.key});

  Color _statusRenk(String status) {
    switch (status) {
      case 'Onaylandi':     return Colors.green;
      case 'Beklemede':     return Colors.orange;
      case 'Amir Onayinda': return Colors.blue;
      case 'Reddedildi':    return Colors.red;
      case 'Iptal Edildi':  return Colors.grey;
      default:              return Colors.grey;
    }
  }

  String _statusText(String status) {
    switch (status) {
      case 'Onaylandi':     return 'Onaylandı';
      case 'Beklemede':     return 'Beklemede';
      case 'Amir Onayinda': return 'Amir Onayında';
      case 'Reddedildi':    return 'Reddedildi';
      case 'Iptal Edildi':  return 'İptal Edildi';
      default:              return status;
    }
  }

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final gecmis = ref.watch(izinGecmisiProvider);

    return Scaffold(
      backgroundColor: const Color(0xFFF5F6FA),
      appBar: AppBar(
        title: Image.asset(
          'assets/images/bossa.png',
          height: 36,
          color: Colors.white,
          colorBlendMode: BlendMode.srcIn,
        ),
        backgroundColor: const Color(0xFF2D6CDF),
        foregroundColor: Colors.white,
      ),
      body: gecmis.when(
        loading: () => const Center(child: CircularProgressIndicator()),
        error: (e, _) => const Center(child: Text('Veriler yüklenemedi.')),
        data: (izinler) {
          if (izinler.isEmpty) {
            return const Center(
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Icon(Icons.inbox_outlined, size: 64, color: Colors.grey),
                  SizedBox(height: 12),
                  Text('Henüz izin kaydınız yok.',
                      style: TextStyle(color: Colors.grey)),
                ],
              ),
            );
          }

          return RefreshIndicator(
            onRefresh: () async => ref.invalidate(izinGecmisiProvider),
            child: ListView.builder(
              padding: const EdgeInsets.all(16),
              itemCount: izinler.length,
              itemBuilder: (context, index) {
                final izin = izinler[index];
                return _IzinKarti(
                  izin: izin,
                  statusRenk: _statusRenk(izin.status),
                  statusText: _statusText(izin.status),
                );
              },
            ),
          );
        },
      ),
    );
  }
}

class _IzinKarti extends StatelessWidget {
  final IzinModel izin;
  final Color statusRenk;
  final String statusText;

  const _IzinKarti({
    required this.izin,
    required this.statusRenk,
    required this.statusText,
  });

  @override
  Widget build(BuildContext context) {
    final baslangic = izin.startDate.substring(0, 10);
    final bitis     = izin.endDate.substring(0, 10);

    return Card(
      margin: const EdgeInsets.only(bottom: 12),
      elevation: 2,
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
      child: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // Üst satır: tarih + status
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Text(
                  '$baslangic → $bitis',
                  style: const TextStyle(
                    fontWeight: FontWeight.bold,
                    fontSize: 15,
                  ),
                ),
                Container(
                  padding: const EdgeInsets.symmetric(
                      horizontal: 10, vertical: 4),
                  decoration: BoxDecoration(
                    color: statusRenk.withOpacity(0.15),
                    borderRadius: BorderRadius.circular(8),
                  ),
                  child: Text(
                    statusText,
                    style: TextStyle(
                      color: statusRenk,
                      fontSize: 12,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                ),
              ],
            ),
            const SizedBox(height: 8),

            // Alt satır: gün sayısı + izin türü
            Row(
              children: [
                const Icon(Icons.calendar_today,
                    size: 14, color: Colors.grey),
                const SizedBox(width: 4),
                Text(
                  '${izin.dayCount} iş günü',
                  style: const TextStyle(color: Colors.grey),
                ),
                const SizedBox(width: 16),
                const Icon(Icons.label_outline,
                    size: 14, color: Colors.grey),
                const SizedBox(width: 4),
                Text(
                  izin.leaveType,
                  style: const TextStyle(color: Colors.grey),
                ),
              ],
            ),

            // Red notu varsa göster
            if (izin.managerNote != null &&
                izin.managerNote!.isNotEmpty) ...[
              const SizedBox(height: 8),
              Container(
                padding: const EdgeInsets.all(8),
                decoration: BoxDecoration(
                  color: Colors.red.withOpacity(0.08),
                  borderRadius: BorderRadius.circular(8),
                ),
                child: Row(
                  children: [
                    const Icon(Icons.info_outline,
                        size: 14, color: Colors.red),
                    const SizedBox(width: 6),
                    Expanded(
                      child: Text(
                        izin.managerNote!,
                        style: const TextStyle(
                            color: Colors.red, fontSize: 12),
                      ),
                    ),
                  ],
                ),
              ),
            ],
          ],
        ),
      ),
    );
  }
}