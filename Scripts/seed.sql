USE MedicalCare;
GO

-- Limpieza rápida (orden por FK)
/*
DELETE FROM Appointment;
DBCC CHECKIDENT ('Appointment', RESEED, 0);

DELETE FROM Doctor;
DBCC CHECKIDENT ('Doctor', RESEED, 0);

DELETE FROM Patient;
DBCC CHECKIDENT ('Patient', RESEED, 0);

DELETE FROM Speciality;
DBCC CHECKIDENT ('Speciality', RESEED, 0);
*/

-- Especialidades
INSERT INTO Speciality (Speciality_Name, Speciality_Description, Speciality_CreatedBy)
VALUES (N'Cardiología', N'Atención del corazón', N'seed'),
       (N'Dermatología', N'Piel y anexos', N'seed'),
       (N'Pediatría', N'Niños y adolescentes', N'seed');

-- Pacientes
INSERT INTO Patient (Patient_FirstName, Patient_LastName, Patient_RUT, Patient_DateOfBirth, Patient_Gender, Patient_Email, Patient_CreatedBy)
VALUES (N'Juan', N'Pérez', '12.345.678-9', '1990-04-15', 'M', N'juan@test.cl', N'seed'),
       (N'Ana', N'Rojas', '9.876.543-2', '1985-01-10', 'F', N'ana@test.cl', N'seed');

-- Doctores (licencia única)
INSERT INTO Doctor (Doctor_FirstName, Doctor_LastName, Doctor_Email, Doctor_Phone, Doctor_LicenseNumber, SpecialityId, Doctor_CreatedBy)
VALUES (N'Alejandro', N'González', N'alejandro@clinic.cl', N'+56 9 1111 2222', N'MED-1001', 1, N'seed'),
       (N'Beatriz',   N'Muñoz',    N'beatriz@clinic.cl',   N'+56 9 2222 3333', N'MED-2002', 2, N'seed');

-- Atenciones (no solapadas)
INSERT INTO Appointment (PatientId, DoctorId, Appointment_StartUtc, Appointment_EndUtc, Appointment_Diagnosis, Appointment_Room, Appointment_Status, Appointment_CreatedBy)
VALUES 
 (1, 1, '2025-08-08T14:00:00Z', '2025-08-08T14:30:00Z', N'Control rutina', N'101', N'Completed', N'seed'),
 (2, 1, '2025-08-08T15:00:00Z', '2025-08-08T15:20:00Z', N'Chequeo',        N'101', N'Scheduled', N'seed'),
 (1, 2, '2025-08-09T10:00:00Z', '2025-08-09T10:45:00Z', N'Revisión piel',  N'202', N'Completed', N'seed');
