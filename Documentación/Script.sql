DROP TABLE Usuarios;
CREATE TABLE Usuarios (
    id_usuario INTEGER PRIMARY KEY AUTOINCREMENT,
    nombre_usuario TEXT NOT NULL UNIQUE,
    contrasenia TEXT NOT NULL,
    correo TEXT NOT NULL UNIQUE,
    fecha_registro DATETIME,
    es_admin BOOLEAN
);

DROP TABLE Progreso;
CREATE TABLE Progreso (
    id_progreso INTEGER PRIMARY KEY AUTOINCREMENT,
    id_usuario INTEGER NOT NULL,
    niveles_completados INTEGER DEFAULT 0,
    puntuacion INTEGER DEFAULT 0,
    FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario) ON DELETE CASCADE
);

DROP TABLE Tesoros;
CREATE TABLE Tesoros (
    id_tesoro INTEGER PRIMARY KEY AUTOINCREMENT,
    nombre TEXT NOT NULL,
    descripcion TEXT
);

DROP TABLE TesorosObtenidos;
CREATE TABLE TesorosObtenidos (
    id_tesoro_obtenido INTEGER PRIMARY KEY AUTOINCREMENT,
    id_usuario INTEGER NOT NULL,
    id_tesoro INTEGER NOT NULL,
    fecha_obtenido DATETIME,
    UNIQUE (id_usuario, id_tesoro),
    FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario) ON DELETE CASCADE,
    FOREIGN KEY (id_tesoro) REFERENCES Tesoros(id_tesoro) ON DELETE CASCADE
);


INSERT INTO Usuarios (nombre_usuario, contrasenia, correo, fecha_registro, es_admin) VALUES
('Jugador1', 'password123', 'jugador1@email.com', '2025-04-04 10:00:00', false),
('Jugador2', 'securepass', 'jugador2@email.com', '2025-04-04 10:00:00', false),
('Admin', 'a1', 'admin@email.com', '2025-04-04 10:00:00', true);

INSERT INTO Progreso (id_usuario, niveles_completados, puntuacion) VALUES
(1, 10, 100),
(2, 0, 0);


INSERT INTO Tesoros (nombre, descripcion) VALUES
('Lapislázuli', 'El Lapislázuli es...'),
('Ruby Rojo', 'El Rubí Rojo...'),
('Diamante Poderoso', 'El Diamante Poderoso...'),
('Brazalete del Poder', 'El Brazalete del Poder...'),
('Anillo de las Sombras', 'En Anillo de las Sombras...'),
('Anillo de la Valentía', 'El Anillo de la valentía...'),
('Anillo de la Destrucción', 'El Anillo de la Destrucción...'),
('Anillo de la Luz', 'El Anillo de la Luz...'),
('Cobre Valioso', 'El Cobre Valioso...'),
('Gema Mística', 'La Gema Mística...');



INSERT INTO TesorosObtenidos (id_usuario, id_tesoro, fecha_obtenido) VALUES
(1, 1, '2025-04-04 10:00:00'),
(1, 2, '2025-04-04 11:00:00'),
(1, 3, '2025-04-04 12:00:00'),
(1, 4, '2025-04-04 12:00:00'),
(1, 5, '2025-04-04 12:00:00'),
(1, 6, '2025-04-04 12:00:00'),
(1, 7, '2025-04-04 12:00:00'),
(1, 8, '2025-04-04 12:00:00'),
(1, 9, '2025-04-04 12:00:00'),
(1, 10, '2025-04-04 12:00:00');
