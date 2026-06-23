-- ============================================================
-- IzinTakipSistemi — Performans Index'leri
-- DBA tarafından production veritabanına uygulanacak.
-- Her index IF NOT EXISTS korumalıdır; tekrar çalıştırılabilir.
-- ============================================================

USE IzinTakipDB;   -- Veritabanı adınızı buraya yazın
GO

-- --------------------------------------------------------
-- EMPLOYEES tablosu
-- --------------------------------------------------------

-- Login sorgusunu hızlandırır: WHERE SicilNo = @Sicil AND IsActive = 1
-- INCLUDE ile Password, Role, DepartmentID doğrudan index'ten okunur (covering index)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Employees_Login' AND object_id = OBJECT_ID('Employees'))
    CREATE NONCLUSTERED INDEX IX_Employees_Login
        ON Employees (SicilNo, IsActive)
        INCLUDE (EmployeeID, FullName, DepartmentID, Role, Password, StartDateOfWork);

-- Amir paneli personel listesi: WHERE DepartmentID = @DepID AND Role = 'Personel' AND IsActive = 1
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Employees_Department_Role' AND object_id = OBJECT_ID('Employees'))
    CREATE NONCLUSTERED INDEX IX_Employees_Department_Role
        ON Employees (DepartmentID, Role, IsActive)
        INCLUDE (EmployeeID, FullName, SicilNo, StartDateOfWork);

-- --------------------------------------------------------
-- LEAVES tablosu
-- --------------------------------------------------------

-- En sık kullanılan filtre: WHERE EmployeeID = @EmpID (kişinin izin geçmişi)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Leaves_EmployeeID' AND object_id = OBJECT_ID('Leaves'))
    CREATE NONCLUSTERED INDEX IX_Leaves_EmployeeID
        ON Leaves (EmployeeID)
        INCLUDE (LeaveID, StartDate, EndDate, DayCount, Status, AmirID, CreatedDate);

-- Yönetici filtresi + LEFT JOIN toplam hesabı: WHERE Status = 'Onaylandi'
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Leaves_Status' AND object_id = OBJECT_ID('Leaves'))
    CREATE NONCLUSTERED INDEX IX_Leaves_Status
        ON Leaves (Status)
        INCLUDE (EmployeeID, DayCount, StartDate, EndDate);

-- Aralık sorgularında çakışma kontrolü ve tarih filtresi:
-- WHERE StartDate <= @Bitis AND EndDate >= @Baslangic AND Status IN (...)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Leaves_DateRange' AND object_id = OBJECT_ID('Leaves'))
    CREATE NONCLUSTERED INDEX IX_Leaves_DateRange
        ON Leaves (StartDate, EndDate, Status)
        INCLUDE (EmployeeID, DayCount);

-- EmployeeID + Status bileşik filtre (LEFT JOIN aggregate için ideal)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Leaves_EmployeeID_Status' AND object_id = OBJECT_ID('Leaves'))
    CREATE NONCLUSTERED INDEX IX_Leaves_EmployeeID_Status
        ON Leaves (EmployeeID, Status)
        INCLUDE (DayCount);

-- --------------------------------------------------------
-- AUDITLOGS tablosu
-- --------------------------------------------------------

-- Log sorgularında JOIN: AuditLogs.PerformedBy = Employees.EmployeeID
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_AuditLogs_PerformedBy' AND object_id = OBJECT_ID('AuditLogs'))
    CREATE NONCLUSTERED INDEX IX_AuditLogs_PerformedBy
        ON AuditLogs (PerformedBy)
        INCLUDE (LogID, OperationType, TargetTable, Details, LogDate);

-- LogForm: ORDER BY LogDate DESC — index scan yerine index seek olur
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_AuditLogs_LogDate' AND object_id = OBJECT_ID('AuditLogs'))
    CREATE NONCLUSTERED INDEX IX_AuditLogs_LogDate
        ON AuditLogs (LogDate DESC);

GO

-- --------------------------------------------------------
-- İndex kullanım durumunu kontrol etmek için (izleme sorgusu)
-- Production'da birkaç hafta sonra çalıştırılabilir.
-- --------------------------------------------------------
/*
SELECT
    OBJECT_NAME(i.object_id)    AS TableName,
    i.name                       AS IndexName,
    ius.user_seeks,
    ius.user_scans,
    ius.user_lookups,
    ius.user_updates,
    ius.last_user_seek,
    ius.last_user_scan
FROM sys.indexes i
JOIN sys.dm_db_index_usage_stats ius
    ON i.object_id = ius.object_id AND i.index_id = ius.index_id
WHERE OBJECT_NAME(i.object_id) IN ('Employees','Leaves','AuditLogs')
  AND ius.database_id = DB_ID()
ORDER BY TableName, IndexName;
*/

PRINT 'Index''ler başarıyla oluşturuldu.';
