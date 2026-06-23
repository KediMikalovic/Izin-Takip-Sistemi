-- ============================================================
-- IzinTakipSistemi - Dummy Data Script  (tek batch, GO yok)
-- ============================================================
-- Yönetici : Y001  /  bossa2024
-- Amirler   : A001-A005  /  amir123
-- Personel  : P001-P010  /  123456
-- ============================================================
-- SSMS'te veritabanınızı seçtikten sonra tümünü seçip F5 ile çalıştırın.
-- ============================================================

-- --------------------------------------------------------
-- 1. DEPARTMENTS
-- --------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM Departments WHERE DepartmentName = 'Bilgi Teknolojileri')
    INSERT INTO Departments (DepartmentName) VALUES ('Bilgi Teknolojileri');

IF NOT EXISTS (SELECT 1 FROM Departments WHERE DepartmentName = 'İnsan Kaynakları')
    INSERT INTO Departments (DepartmentName) VALUES ('İnsan Kaynakları');

IF NOT EXISTS (SELECT 1 FROM Departments WHERE DepartmentName = 'Muhasebe')
    INSERT INTO Departments (DepartmentName) VALUES ('Muhasebe');

IF NOT EXISTS (SELECT 1 FROM Departments WHERE DepartmentName = 'Üretim')
    INSERT INTO Departments (DepartmentName) VALUES ('Üretim');

IF NOT EXISTS (SELECT 1 FROM Departments WHERE DepartmentName = 'Satış')
    INSERT INTO Departments (DepartmentName) VALUES ('Satış');

-- --------------------------------------------------------
-- 2. DEĞIŞKENLER  (tek seferlik tanım)
-- --------------------------------------------------------
DECLARE @BT_ID   INT = (SELECT DepartmentID FROM Departments WHERE DepartmentName = 'Bilgi Teknolojileri');
DECLARE @IK_ID   INT = (SELECT DepartmentID FROM Departments WHERE DepartmentName = 'İnsan Kaynakları');
DECLARE @MUH_ID  INT = (SELECT DepartmentID FROM Departments WHERE DepartmentName = 'Muhasebe');
DECLARE @URE_ID  INT = (SELECT DepartmentID FROM Departments WHERE DepartmentName = 'Üretim');
DECLARE @SAT_ID  INT = (SELECT DepartmentID FROM Departments WHERE DepartmentName = 'Satış');

-- SHA-256 hash'leri (düz hex, eski uyumlu format)
DECLARE @PW_YON  NVARCHAR(200) = '9500cf9c984d75cc3437460f991aa24991be38c64e26d4713a343526c0933972'; -- bossa2024
DECLARE @PW_AMIR NVARCHAR(200) = '4d22be2786c0540b0389fa978a6117cbe9849c2e47e0abbf87ae88f0974c3f27'; -- amir123
DECLARE @PW_PER  NVARCHAR(200) = '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92'; -- 123456

-- --------------------------------------------------------
-- 3. EMPLOYEES
-- --------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM Employees WHERE SicilNo = 'Y001')
    INSERT INTO Employees (FullName, SicilNo, Password, DepartmentID, Role, StartDateOfWork, IsActive)
    VALUES ('Ahmet Kaya', 'Y001', @PW_YON, @IK_ID, 'Yönetici', '2018-01-15', 1);

IF NOT EXISTS (SELECT 1 FROM Employees WHERE SicilNo = 'A001')
    INSERT INTO Employees (FullName, SicilNo, Password, DepartmentID, Role, StartDateOfWork, IsActive)
    VALUES ('Mehmet Demir', 'A001', @PW_AMIR, @BT_ID, 'Amir', '2019-03-10', 1);

IF NOT EXISTS (SELECT 1 FROM Employees WHERE SicilNo = 'A002')
    INSERT INTO Employees (FullName, SicilNo, Password, DepartmentID, Role, StartDateOfWork, IsActive)
    VALUES ('Fatma Şahin', 'A002', @PW_AMIR, @IK_ID, 'Amir', '2017-06-01', 1);

IF NOT EXISTS (SELECT 1 FROM Employees WHERE SicilNo = 'A003')
    INSERT INTO Employees (FullName, SicilNo, Password, DepartmentID, Role, StartDateOfWork, IsActive)
    VALUES ('Hüseyin Yıldız', 'A003', @PW_AMIR, @MUH_ID, 'Amir', '2016-09-20', 1);

IF NOT EXISTS (SELECT 1 FROM Employees WHERE SicilNo = 'A004')
    INSERT INTO Employees (FullName, SicilNo, Password, DepartmentID, Role, StartDateOfWork, IsActive)
    VALUES ('Ayşe Çelik', 'A004', @PW_AMIR, @URE_ID, 'Amir', '2015-02-14', 1);

IF NOT EXISTS (SELECT 1 FROM Employees WHERE SicilNo = 'A005')
    INSERT INTO Employees (FullName, SicilNo, Password, DepartmentID, Role, StartDateOfWork, IsActive)
    VALUES ('Ömer Arslan', 'A005', @PW_AMIR, @SAT_ID, 'Amir', '2020-07-01', 1);

IF NOT EXISTS (SELECT 1 FROM Employees WHERE SicilNo = 'P001')
    INSERT INTO Employees (FullName, SicilNo, Password, DepartmentID, Role, StartDateOfWork, IsActive)
    VALUES ('Cem Özdemir', 'P001', @PW_PER, @BT_ID, 'Personel', '2021-01-10', 1);

IF NOT EXISTS (SELECT 1 FROM Employees WHERE SicilNo = 'P002')
    INSERT INTO Employees (FullName, SicilNo, Password, DepartmentID, Role, StartDateOfWork, IsActive)
    VALUES ('Selin Güneş', 'P002', @PW_PER, @BT_ID, 'Personel', '2022-04-15', 1);

IF NOT EXISTS (SELECT 1 FROM Employees WHERE SicilNo = 'P003')
    INSERT INTO Employees (FullName, SicilNo, Password, DepartmentID, Role, StartDateOfWork, IsActive)
    VALUES ('Merve Koç', 'P003', @PW_PER, @IK_ID, 'Personel', '2020-08-01', 1);

IF NOT EXISTS (SELECT 1 FROM Employees WHERE SicilNo = 'P004')
    INSERT INTO Employees (FullName, SicilNo, Password, DepartmentID, Role, StartDateOfWork, IsActive)
    VALUES ('Burak Aydın', 'P004', @PW_PER, @IK_ID, 'Personel', '2023-01-16', 1);

IF NOT EXISTS (SELECT 1 FROM Employees WHERE SicilNo = 'P005')
    INSERT INTO Employees (FullName, SicilNo, Password, DepartmentID, Role, StartDateOfWork, IsActive)
    VALUES ('Nilüfer Aktaş', 'P005', @PW_PER, @MUH_ID, 'Personel', '2018-11-05', 1);

IF NOT EXISTS (SELECT 1 FROM Employees WHERE SicilNo = 'P006')
    INSERT INTO Employees (FullName, SicilNo, Password, DepartmentID, Role, StartDateOfWork, IsActive)
    VALUES ('Kerem Bulut', 'P006', @PW_PER, @MUH_ID, 'Personel', '2019-05-20', 1);

IF NOT EXISTS (SELECT 1 FROM Employees WHERE SicilNo = 'P007')
    INSERT INTO Employees (FullName, SicilNo, Password, DepartmentID, Role, StartDateOfWork, IsActive)
    VALUES ('Serkan Yalçın', 'P007', @PW_PER, @URE_ID, 'Personel', '2017-03-01', 1);

IF NOT EXISTS (SELECT 1 FROM Employees WHERE SicilNo = 'P008')
    INSERT INTO Employees (FullName, SicilNo, Password, DepartmentID, Role, StartDateOfWork, IsActive)
    VALUES ('Gamze Kurt', 'P008', @PW_PER, @URE_ID, 'Personel', '2020-10-12', 1);

IF NOT EXISTS (SELECT 1 FROM Employees WHERE SicilNo = 'P009')
    INSERT INTO Employees (FullName, SicilNo, Password, DepartmentID, Role, StartDateOfWork, IsActive)
    VALUES ('Emre Şimşek', 'P009', @PW_PER, @SAT_ID, 'Personel', '2021-07-01', 1);

IF NOT EXISTS (SELECT 1 FROM Employees WHERE SicilNo = 'P010')
    INSERT INTO Employees (FullName, SicilNo, Password, DepartmentID, Role, StartDateOfWork, IsActive)
    VALUES ('Cansu Erdoğan', 'P010', @PW_PER, @SAT_ID, 'Personel', '2022-09-01', 1);

-- --------------------------------------------------------
-- 4. LEAVES  (employee ID'leri burada çekiliyor)
-- --------------------------------------------------------
DECLARE @A001_ID INT = (SELECT EmployeeID FROM Employees WHERE SicilNo = 'A001');
DECLARE @A002_ID INT = (SELECT EmployeeID FROM Employees WHERE SicilNo = 'A002');
DECLARE @A003_ID INT = (SELECT EmployeeID FROM Employees WHERE SicilNo = 'A003');
DECLARE @A004_ID INT = (SELECT EmployeeID FROM Employees WHERE SicilNo = 'A004');
DECLARE @A005_ID INT = (SELECT EmployeeID FROM Employees WHERE SicilNo = 'A005');
DECLARE @YON_ID  INT = (SELECT EmployeeID FROM Employees WHERE SicilNo = 'Y001');
DECLARE @P001_ID INT = (SELECT EmployeeID FROM Employees WHERE SicilNo = 'P001');
DECLARE @P002_ID INT = (SELECT EmployeeID FROM Employees WHERE SicilNo = 'P002');
DECLARE @P003_ID INT = (SELECT EmployeeID FROM Employees WHERE SicilNo = 'P003');
DECLARE @P004_ID INT = (SELECT EmployeeID FROM Employees WHERE SicilNo = 'P004');
DECLARE @P005_ID INT = (SELECT EmployeeID FROM Employees WHERE SicilNo = 'P005');
DECLARE @P006_ID INT = (SELECT EmployeeID FROM Employees WHERE SicilNo = 'P006');
DECLARE @P007_ID INT = (SELECT EmployeeID FROM Employees WHERE SicilNo = 'P007');
DECLARE @P008_ID INT = (SELECT EmployeeID FROM Employees WHERE SicilNo = 'P008');
DECLARE @P009_ID INT = (SELECT EmployeeID FROM Employees WHERE SicilNo = 'P009');
DECLARE @P010_ID INT = (SELECT EmployeeID FROM Employees WHERE SicilNo = 'P010');

-- P001 - Cem Özdemir (BT)
IF NOT EXISTS (SELECT 1 FROM Leaves WHERE EmployeeID = @P001_ID AND StartDate = '2025-07-10')
    INSERT INTO Leaves (EmployeeID, StartDate, EndDate, DayCount, Status, AmirID, CreatedDate)
    VALUES (@P001_ID, '2025-07-10', '2025-07-18', 7, 'Onaylandi', @A001_ID, '2025-07-01');

IF NOT EXISTS (SELECT 1 FROM Leaves WHERE EmployeeID = @P001_ID AND StartDate = '2025-11-17')
    INSERT INTO Leaves (EmployeeID, StartDate, EndDate, DayCount, Status, AmirID, CreatedDate)
    VALUES (@P001_ID, '2025-11-17', '2025-11-21', 5, 'Onaylandi', @A001_ID, '2025-11-05');

IF NOT EXISTS (SELECT 1 FROM Leaves WHERE EmployeeID = @P001_ID AND StartDate = '2026-04-07')
    INSERT INTO Leaves (EmployeeID, StartDate, EndDate, DayCount, Status, AmirID, CreatedDate)
    VALUES (@P001_ID, '2026-04-07', '2026-04-11', 5, 'Beklemede', @A001_ID, GETDATE());

-- P002 - Selin Güneş (BT)
IF NOT EXISTS (SELECT 1 FROM Leaves WHERE EmployeeID = @P002_ID AND StartDate = '2025-12-23')
    INSERT INTO Leaves (EmployeeID, StartDate, EndDate, DayCount, Status, AmirID, CreatedDate)
    VALUES (@P002_ID, '2025-12-23', '2025-12-31', 7, 'Onaylandi', @A001_ID, '2025-12-10');

IF NOT EXISTS (SELECT 1 FROM Leaves WHERE EmployeeID = @P002_ID AND StartDate = '2026-03-03')
    INSERT INTO Leaves (EmployeeID, StartDate, EndDate, DayCount, Status, AmirID, ManagerNote, CreatedDate)
    VALUES (@P002_ID, '2026-03-03', '2026-03-07', 5, 'Reddedildi', @A001_ID,
            'Kritik proje teslim tarihi çakışıyor.', '2026-02-20');

-- P003 - Merve Koç (İK)
IF NOT EXISTS (SELECT 1 FROM Leaves WHERE EmployeeID = @P003_ID AND StartDate = '2025-08-18')
    INSERT INTO Leaves (EmployeeID, StartDate, EndDate, DayCount, Status, AmirID, CreatedDate)
    VALUES (@P003_ID, '2025-08-18', '2025-08-29', 10, 'Onaylandi', @A002_ID, '2025-08-01');

IF NOT EXISTS (SELECT 1 FROM Leaves WHERE EmployeeID = @P003_ID AND StartDate = '2026-04-14')
    INSERT INTO Leaves (EmployeeID, StartDate, EndDate, DayCount, Status, AmirID, CreatedDate)
    VALUES (@P003_ID, '2026-04-14', '2026-04-18', 5, 'Beklemede', @A002_ID, GETDATE());

-- P004 - Burak Aydın (İK)
IF NOT EXISTS (SELECT 1 FROM Leaves WHERE EmployeeID = @P004_ID AND StartDate = '2026-01-02')
    INSERT INTO Leaves (EmployeeID, StartDate, EndDate, DayCount, Status, AmirID, CreatedDate)
    VALUES (@P004_ID, '2026-01-02', '2026-01-10', 7, 'Onaylandi', @A002_ID, '2025-12-20');

IF NOT EXISTS (SELECT 1 FROM Leaves WHERE EmployeeID = @P004_ID AND StartDate = '2026-04-28')
    INSERT INTO Leaves (EmployeeID, StartDate, EndDate, DayCount, Status, AmirID, CreatedDate)
    VALUES (@P004_ID, '2026-04-28', '2026-05-02', 5, 'Beklemede', @A002_ID, GETDATE());

-- P005 - Nilüfer Aktaş (Muhasebe)
IF NOT EXISTS (SELECT 1 FROM Leaves WHERE EmployeeID = @P005_ID AND StartDate = '2025-09-01')
    INSERT INTO Leaves (EmployeeID, StartDate, EndDate, DayCount, Status, AmirID, CreatedDate)
    VALUES (@P005_ID, '2025-09-01', '2025-09-12', 10, 'Onaylandi', @A003_ID, '2025-08-20');

IF NOT EXISTS (SELECT 1 FROM Leaves WHERE EmployeeID = @P005_ID AND StartDate = '2025-12-02')
    INSERT INTO Leaves (EmployeeID, StartDate, EndDate, DayCount, Status, AmirID, CreatedDate)
    VALUES (@P005_ID, '2025-12-02', '2025-12-06', 5, 'Onaylandi', @A003_ID, '2025-11-25');

IF NOT EXISTS (SELECT 1 FROM Leaves WHERE EmployeeID = @P005_ID AND StartDate = '2026-02-10')
    INSERT INTO Leaves (EmployeeID, StartDate, EndDate, DayCount, Status, AmirID, CreatedDate)
    VALUES (@P005_ID, '2026-02-10', '2026-02-14', 5, 'Iptal Edildi', @A003_ID, '2026-02-01');

-- P006 - Kerem Bulut (Muhasebe)
IF NOT EXISTS (SELECT 1 FROM Leaves WHERE EmployeeID = @P006_ID AND StartDate = '2025-10-20')
    INSERT INTO Leaves (EmployeeID, StartDate, EndDate, DayCount, Status, AmirID, CreatedDate)
    VALUES (@P006_ID, '2025-10-20', '2025-10-31', 10, 'Onaylandi', @A003_ID, '2025-10-05');

IF NOT EXISTS (SELECT 1 FROM Leaves WHERE EmployeeID = @P006_ID AND StartDate = '2026-04-21')
    INSERT INTO Leaves (EmployeeID, StartDate, EndDate, DayCount, Status, AmirID, CreatedDate)
    VALUES (@P006_ID, '2026-04-21', '2026-04-25', 5, 'Beklemede', @A003_ID, GETDATE());

-- P007 - Serkan Yalçın (Üretim)
IF NOT EXISTS (SELECT 1 FROM Leaves WHERE EmployeeID = @P007_ID AND StartDate = '2025-06-02')
    INSERT INTO Leaves (EmployeeID, StartDate, EndDate, DayCount, Status, AmirID, CreatedDate)
    VALUES (@P007_ID, '2025-06-02', '2025-06-06', 5, 'Onaylandi', @A004_ID, '2025-05-20');

IF NOT EXISTS (SELECT 1 FROM Leaves WHERE EmployeeID = @P007_ID AND StartDate = '2025-10-06')
    INSERT INTO Leaves (EmployeeID, StartDate, EndDate, DayCount, Status, AmirID, CreatedDate)
    VALUES (@P007_ID, '2025-10-06', '2025-10-17', 10, 'Onaylandi', @A004_ID, '2025-09-25');

IF NOT EXISTS (SELECT 1 FROM Leaves WHERE EmployeeID = @P007_ID AND StartDate = '2026-05-04')
    INSERT INTO Leaves (EmployeeID, StartDate, EndDate, DayCount, Status, AmirID, CreatedDate)
    VALUES (@P007_ID, '2026-05-04', '2026-05-08', 5, 'Beklemede', @A004_ID, GETDATE());

-- P008 - Gamze Kurt (Üretim)
IF NOT EXISTS (SELECT 1 FROM Leaves WHERE EmployeeID = @P008_ID AND StartDate = '2026-01-13')
    INSERT INTO Leaves (EmployeeID, StartDate, EndDate, DayCount, Status, AmirID, CreatedDate)
    VALUES (@P008_ID, '2026-01-13', '2026-01-17', 5, 'Onaylandi', @A004_ID, '2026-01-05');

IF NOT EXISTS (SELECT 1 FROM Leaves WHERE EmployeeID = @P008_ID AND StartDate = '2026-03-10')
    INSERT INTO Leaves (EmployeeID, StartDate, EndDate, DayCount, Status, AmirID, ManagerNote, CreatedDate)
    VALUES (@P008_ID, '2026-03-10', '2026-03-14', 5, 'Reddedildi', @A004_ID,
            'Üretim bandı yoğunluğu nedeniyle onaylanamadı.', '2026-03-01');

-- P009 - Emre Şimşek (Satış)
IF NOT EXISTS (SELECT 1 FROM Leaves WHERE EmployeeID = @P009_ID AND StartDate = '2025-11-03')
    INSERT INTO Leaves (EmployeeID, StartDate, EndDate, DayCount, Status, AmirID, CreatedDate)
    VALUES (@P009_ID, '2025-11-03', '2025-11-07', 5, 'Onaylandi', @A005_ID, '2025-10-25');

IF NOT EXISTS (SELECT 1 FROM Leaves WHERE EmployeeID = @P009_ID AND StartDate = '2026-04-28')
    INSERT INTO Leaves (EmployeeID, StartDate, EndDate, DayCount, Status, AmirID, CreatedDate)
    VALUES (@P009_ID, '2026-04-28', '2026-05-02', 5, 'Beklemede', @A005_ID, GETDATE());

-- P010 - Cansu Erdoğan (Satış)
IF NOT EXISTS (SELECT 1 FROM Leaves WHERE EmployeeID = @P010_ID AND StartDate = '2025-06-16')
    INSERT INTO Leaves (EmployeeID, StartDate, EndDate, DayCount, Status, AmirID, CreatedDate)
    VALUES (@P010_ID, '2025-06-16', '2025-06-27', 10, 'Onaylandi', @A005_ID, '2025-06-01');

IF NOT EXISTS (SELECT 1 FROM Leaves WHERE EmployeeID = @P010_ID AND StartDate = '2026-02-02')
    INSERT INTO Leaves (EmployeeID, StartDate, EndDate, DayCount, Status, AmirID, ManagerNote, CreatedDate)
    VALUES (@P010_ID, '2026-02-02', '2026-02-06', 5, 'Reddedildi', @A005_ID,
            'Yıl başı satış hedef sezonu, izin uygun değil.', '2026-01-25');

-- --------------------------------------------------------
-- 5. AUDIT LOGS
-- --------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM AuditLogs WHERE Details LIKE '%Cem Özdemir%7 günlük%')
    INSERT INTO AuditLogs (OperationType, PerformedBy, TargetTable, Details, LogDate)
    VALUES ('Insert', @A001_ID, 'Leaves',
            'Cem Özdemir için 7 günlük yeni izin talebi (Beklemede) oluşturuldu.',
            DATEADD(day, -60, GETDATE()));

IF NOT EXISTS (SELECT 1 FROM AuditLogs WHERE Details LIKE '%Cem Özdemir%onaylandı%')
    INSERT INTO AuditLogs (OperationType, PerformedBy, TargetTable, Details, LogDate)
    VALUES ('Update', @YON_ID, 'Leaves',
            'Cem Özdemir izin talebi onaylandı (7 gün, 2025-07-10 / 2025-07-18).',
            DATEADD(day, -58, GETDATE()));

IF NOT EXISTS (SELECT 1 FROM AuditLogs WHERE Details LIKE '%Selin Güneş%onaylandı%')
    INSERT INTO AuditLogs (OperationType, PerformedBy, TargetTable, Details, LogDate)
    VALUES ('Update', @YON_ID, 'Leaves',
            'Selin Güneş izin talebi onaylandı (7 gün, 2025-12-23 / 2025-12-31).',
            DATEADD(day, -100, GETDATE()));

IF NOT EXISTS (SELECT 1 FROM AuditLogs WHERE Details LIKE '%Selin Güneş%reddedildi%')
    INSERT INTO AuditLogs (OperationType, PerformedBy, TargetTable, Details, LogDate)
    VALUES ('Update', @YON_ID, 'Leaves',
            'Selin Güneş izin talebi reddedildi: Kritik proje teslim tarihi çakışıyor.',
            DATEADD(day, -30, GETDATE()));

IF NOT EXISTS (SELECT 1 FROM AuditLogs WHERE Details LIKE '%Merve Koç%onaylandı%')
    INSERT INTO AuditLogs (OperationType, PerformedBy, TargetTable, Details, LogDate)
    VALUES ('Update', @YON_ID, 'Leaves',
            'Merve Koç izin talebi onaylandı (10 gün, 2025-08-18 / 2025-08-29).',
            DATEADD(day, -220, GETDATE()));

IF NOT EXISTS (SELECT 1 FROM AuditLogs WHERE Details LIKE '%Serkan Yalçın%onaylandı%')
    INSERT INTO AuditLogs (OperationType, PerformedBy, TargetTable, Details, LogDate)
    VALUES ('Update', @YON_ID, 'Leaves',
            'Serkan Yalçın izin talebi onaylandı (10 gün, 2025-10-06 / 2025-10-17).',
            DATEADD(day, -175, GETDATE()));

IF NOT EXISTS (SELECT 1 FROM AuditLogs WHERE Details LIKE '%Nilüfer Aktaş%iptal%')
    INSERT INTO AuditLogs (OperationType, PerformedBy, TargetTable, Details, LogDate)
    VALUES ('Delete', @A003_ID, 'Leaves',
            'Nilüfer Aktaş izin talebi iptal edildi (5 gün, 2026-02-10 / 2026-02-14).',
            DATEADD(day, -50, GETDATE()));

IF NOT EXISTS (SELECT 1 FROM AuditLogs WHERE Details LIKE '%Gamze Kurt%reddedildi%')
    INSERT INTO AuditLogs (OperationType, PerformedBy, TargetTable, Details, LogDate)
    VALUES ('Update', @YON_ID, 'Leaves',
            'Gamze Kurt izin talebi reddedildi: Üretim bandı yoğunluğu.',
            DATEADD(day, -20, GETDATE()));

PRINT 'Dummy data basariyla eklendi!';
PRINT 'Departmanlar: 5  |  Calisanlar: 16  |  Izinler: 23  |  Loglar: 8';
