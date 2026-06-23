-- Migration: Avans İzin Sistemi için LeaveType sütunu ekleme
-- Çalıştırmadan önce yedek aldığınızdan emin olun.

IF NOT EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'Leaves' AND COLUMN_NAME = 'LeaveType'
)
BEGIN
    ALTER TABLE Leaves
    ADD LeaveType NVARCHAR(20) NOT NULL DEFAULT 'Normal';

    PRINT 'LeaveType sütunu başarıyla eklendi.';
END
ELSE
BEGIN
    PRINT 'LeaveType sütunu zaten mevcut — migration atlandı.';
END
