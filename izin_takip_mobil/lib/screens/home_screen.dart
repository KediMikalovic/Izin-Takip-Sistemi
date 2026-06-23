import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../providers/auth_provider.dart';
import '../providers/izin_provider.dart';

class HomeScreen extends ConsumerWidget {
  const HomeScreen({super.key});

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

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final profil = ref.watch(profilProvider);
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
        actions: [
          IconButton(
            icon: const Icon(Icons.logout),
            onPressed: () async {
              await ref.read(authProvider.notifier).cikisYap();
              if (context.mounted) {
                Navigator.of(context).pushReplacementNamed('/login');
              }
            },
          ),
        ],
      ),
      body: RefreshIndicator(
        onRefresh: () async {
          ref.invalidate(profilProvider);
          ref.invalidate(izinGecmisiProvider);
        },
        child: ListView(
          padding: const EdgeInsets.all(16),
          children: [

            // Hoşgeldin + profil kartı
            profil.when(
              loading: () => const Card(
                child: Padding(
                  padding: EdgeInsets.all(16),
                  child: Center(child: CircularProgressIndicator()),
                ),
              ),
              error: (e, _) => Card(
                child: Padding(
                  padding: const EdgeInsets.all(16),
                  child: Text('Profil yüklenemedi: $e'),
                ),
              ),
              data: (p) => Card(
                elevation: 3,
                shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(12)),
                child: Padding(
                  padding: const EdgeInsets.all(16),
                  child: Column(
                    children: [
                      // Üst satır: avatar + isim
                      Row(
                        children: [
                          const CircleAvatar(
                            backgroundColor: Color(0xFF2D6CDF),
                            radius: 24,
                            child: Icon(Icons.person,
                                color: Colors.white, size: 28),
                          ),
                          const SizedBox(width: 12),
                          Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Text(
                                'Hoşgeldin,',
                                style: TextStyle(color: Colors.grey[600]),
                              ),
                              Text(
                                p.fullName,
                                style: const TextStyle(
                                  fontSize: 16,
                                  fontWeight: FontWeight.bold,
                                ),
                              ),
                              Text(
                                p.departman,
                                style: TextStyle(
                                    color: Colors.grey[600], fontSize: 12),
                              ),
                            ],
                          ),
                        ],
                      ),
                      const SizedBox(height: 16),
                      const Divider(),
                      const SizedBox(height: 8),

                      // İzin özeti
                      Row(
                        mainAxisAlignment: MainAxisAlignment.spaceAround,
                        children: [
                          _IzinOzetKutu(
                            label: 'Toplam Hak',
                            deger: '${p.toplamHak} gün',
                            renk: const Color(0xFF2D6CDF),
                          ),
                          _IzinOzetKutu(
                            label: 'Kullanılan',
                            deger: '${p.kullanilanIzin} gün',
                            renk: Colors.orange,
                          ),
                          _IzinOzetKutu(
                            label: 'Kalan',
                            deger: '${p.kalanIzin} gün',
                            renk: Colors.green,
                          ),
                        ],
                      ),
                      const SizedBox(height: 8),
                      Text(
                        'Kıdem: ${p.kidemYil} yıl',
                        style: TextStyle(
                            color: Colors.grey[600], fontSize: 12),
                      ),
                    ],
                  ),
                ),
              ),
            ),

            const SizedBox(height: 16),

            // Hızlı erişim butonları
            Row(
              children: [
                Expanded(
                  child: _HizliButon(
                    icon: Icons.history,
                    label: 'İzin Geçmişim',
                    onTap: () =>
                        Navigator.of(context).pushNamed('/gecmis'),
                  ),
                ),
                const SizedBox(width: 12),
                Expanded(
                  child: _HizliButon(
                    icon: Icons.add_circle_outline,
                    label: 'İzin Talebi',
                    onTap: () =>
                        Navigator.of(context).pushNamed('/talep'),
                  ),
                ),
              ],
            ),

            const SizedBox(height: 24),

            // Son izinler
            const Text(
              'Son İzinlerim',
              style: TextStyle(
                  fontSize: 16, fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 8),

            gecmis.when(
              loading: () =>
                  const Center(child: CircularProgressIndicator()),
              error: (e, _) =>
                  const Text('Veriler yüklenemedi.'),
              data: (izinler) {
                if (izinler.isEmpty) {
                  return const Center(
                      child: Text('Henüz izin kaydınız yok.'));
                }
                final sonUc = izinler.take(3).toList();
                return Column(
                  children: sonUc.map((izin) {
                    return Card(
                      margin: const EdgeInsets.only(bottom: 8),
                      child: ListTile(
                        leading: CircleAvatar(
                          backgroundColor:
                              _statusRenk(izin.status).withOpacity(0.15),
                          child: Icon(Icons.beach_access,
                              color: _statusRenk(izin.status)),
                        ),
                        title: Text(
                          '${izin.startDate.substring(0, 10)} → ${izin.endDate.substring(0, 10)}',
                          style: const TextStyle(fontSize: 13),
                        ),
                        subtitle: Text('${izin.dayCount} iş günü'),
                        trailing: Container(
                          padding: const EdgeInsets.symmetric(
                              horizontal: 8, vertical: 4),
                          decoration: BoxDecoration(
                            color: _statusRenk(izin.status)
                                .withOpacity(0.15),
                            borderRadius: BorderRadius.circular(8),
                          ),
                          child: Text(
                            izin.status,
                            style: TextStyle(
                              color: _statusRenk(izin.status),
                              fontSize: 11,
                              fontWeight: FontWeight.bold,
                            ),
                          ),
                        ),
                      ),
                    );
                  }).toList(),
                );
              },
            ),
          ],
        ),
      ),
    );
  }
}

class _IzinOzetKutu extends StatelessWidget {
  final String label;
  final String deger;
  final Color renk;

  const _IzinOzetKutu({
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

class _HizliButon extends StatelessWidget {
  final IconData icon;
  final String label;
  final VoidCallback onTap;

  const _HizliButon({
    required this.icon,
    required this.label,
    required this.onTap,
  });

  @override
  Widget build(BuildContext context) {
    return Card(
      elevation: 2,
      shape:
          RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
      child: InkWell(
        onTap: onTap,
        borderRadius: BorderRadius.circular(12),
        child: Padding(
          padding: const EdgeInsets.symmetric(vertical: 20),
          child: Column(
            children: [
              Icon(icon, size: 32, color: const Color(0xFF2D6CDF)),
              const SizedBox(height: 8),
              Text(label,
                  style: const TextStyle(fontWeight: FontWeight.w500)),
            ],
          ),
        ),
      ),
    );
  }
}