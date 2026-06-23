import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../providers/izin_provider.dart';

class TalepScreen extends ConsumerStatefulWidget {
  const TalepScreen({super.key});

  @override
  ConsumerState<TalepScreen> createState() => _TalepScreenState();
}

class _TalepScreenState extends ConsumerState<TalepScreen> {
  DateTime? _baslangic;
  DateTime? _bitis;

  Future<void> _tarihSec(bool isBaslangic) async {
    final secilen = await showDatePicker(
      context: context,
      initialDate: DateTime.now().add(const Duration(days: 1)),
      firstDate: DateTime.now(),
      lastDate: DateTime.now().add(const Duration(days: 365)),
      locale: const Locale('tr', 'TR'),
    );

    if (secilen != null) {
      setState(() {
        if (isBaslangic) {
          _baslangic = secilen;
          if (_bitis != null && _bitis!.isBefore(secilen)) {
            _bitis = null;
          }
        } else {
          _bitis = secilen;
        }
      });
    }
  }

  Future<void> _talepGonder() async {
    if (_baslangic == null || _bitis == null) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Lütfen başlangıç ve bitiş tarihini seçin.')),
      );
      return;
    }

    final profil = await ref.read(profilProvider.future);

    // TODO: amirId personelin departmanındaki amirden DB'den otomatik çekilmeli
    await ref.read(izinTalepProvider.notifier).talepOlustur(
      amirId:    1,
      baslangic: _baslangic!,
      bitis:     _bitis!,
      kalanIzin: profil.kalanIzin,
    );
  }

  @override
  Widget build(BuildContext context) {
    final talepState = ref.watch(izinTalepProvider);
    final profil     = ref.watch(profilProvider);

    ref.listen(izinTalepProvider, (prev, next) {
      if (next.isBasarili) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text(next.mesaj ?? 'Talep oluşturuldu.'),
            backgroundColor: Colors.green,
          ),
        );
        ref.read(izinTalepProvider.notifier).sifirla();
        ref.invalidate(izinGecmisiProvider);
        ref.invalidate(profilProvider);
        Navigator.of(context).pop();
      }
      if (next.hata != null) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text(next.hata!),
            backgroundColor: Colors.red,
          ),
        );
        ref.read(izinTalepProvider.notifier).sifirla();
      }
    });

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
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(16),
        child: Column(
          children: [

            // Kalan izin bilgisi
            profil.when(
              loading: () => const SizedBox.shrink(),
              error:   (e, _) => const SizedBox.shrink(),
              data: (p) => Card(
                elevation: 2,
                shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(12)),
                child: Padding(
                  padding: const EdgeInsets.all(16),
                  child: Row(
                    mainAxisAlignment: MainAxisAlignment.spaceAround,
                    children: [
                      _OzetKutu(
                        label: 'Toplam Hak',
                        deger: '${p.toplamHak} gün',
                        renk: const Color(0xFF2D6CDF),
                      ),
                      _OzetKutu(
                        label: 'Kullanılan',
                        deger: '${p.kullanilanIzin} gün',
                        renk: Colors.orange,
                      ),
                      _OzetKutu(
                        label: 'Kalan',
                        deger: '${p.kalanIzin} gün',
                        renk: Colors.green,
                      ),
                    ],
                  ),
                ),
              ),
            ),

            const SizedBox(height: 12),

            // Tarih seçim kartı
            Card(
              elevation: 3,
              shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(16)),
              child: Padding(
                padding: const EdgeInsets.all(20),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    const Text(
                      'İzin Tarihleri',
                      style: TextStyle(
                          fontSize: 16, fontWeight: FontWeight.bold),
                    ),
                    const SizedBox(height: 16),

                    _TarihSecici(
                      label:     'Başlangıç Tarihi',
                      tarih:     _baslangic,
                      onPressed: () => _tarihSec(true),
                    ),
                    const SizedBox(height: 12),

                    _TarihSecici(
                      label:     'Bitiş Tarihi',
                      tarih:     _bitis,
                      onPressed: () => _tarihSec(false),
                    ),
                    const SizedBox(height: 24),

                    SizedBox(
                      width: double.infinity,
                      height: 48,
                      child: ElevatedButton.icon(
                        onPressed: talepState.isLoading ? null : _talepGonder,
                        icon: talepState.isLoading
                            ? const SizedBox(
                                width: 20,
                                height: 20,
                                child: CircularProgressIndicator(
                                    color: Colors.white, strokeWidth: 2),
                              )
                            : const Icon(Icons.send, color: Colors.white),
                        label: const Text(
                          'Talebi Gönder',
                          style: TextStyle(
                              fontSize: 16, color: Colors.white),
                        ),
                        style: ElevatedButton.styleFrom(
                          backgroundColor: const Color(0xFF2D6CDF),
                          shape: RoundedRectangleBorder(
                              borderRadius: BorderRadius.circular(8)),
                        ),
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}

class _OzetKutu extends StatelessWidget {
  final String label;
  final String deger;
  final Color renk;

  const _OzetKutu({
    required this.label,
    required this.deger,
    required this.renk,
  });

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Text(
          deger,
          style: TextStyle(
            fontSize: 18,
            fontWeight: FontWeight.bold,
            color: renk,
          ),
        ),
        Text(
          label,
          style: TextStyle(color: Colors.grey[600], fontSize: 12),
        ),
      ],
    );
  }
}

class _TarihSecici extends StatelessWidget {
  final String label;
  final DateTime? tarih;
  final VoidCallback onPressed;

  const _TarihSecici({
    required this.label,
    required this.tarih,
    required this.onPressed,
  });

  @override
  Widget build(BuildContext context) {
    return InkWell(
      onTap: onPressed,
      borderRadius: BorderRadius.circular(8),
      child: Container(
        padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 14),
        decoration: BoxDecoration(
          border: Border.all(color: Colors.grey.shade400),
          borderRadius: BorderRadius.circular(8),
        ),
        child: Row(
          children: [
            const Icon(Icons.calendar_today, color: Color(0xFF2D6CDF)),
            const SizedBox(width: 12),
            Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(label,
                    style: TextStyle(
                        color: Colors.grey[600], fontSize: 12)),
                Text(
                  tarih != null
                      ? '${tarih!.day.toString().padLeft(2, '0')}.${tarih!.month.toString().padLeft(2, '0')}.${tarih!.year}'
                      : 'Tarih seçin',
                  style: TextStyle(
                    fontSize: 15,
                    fontWeight: tarih != null
                        ? FontWeight.w500
                        : FontWeight.normal,
                    color: tarih != null ? Colors.black : Colors.grey,
                  ),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}