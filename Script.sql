CREATE TABLE Usuarios (
    id_usuario INTEGER PRIMARY KEY AUTOINCREMENT,
    nombre_usuario TEXT NOT NULL UNIQUE,
    contrasenia TEXT NOT NULL,
    correo TEXT NOT NULL UNIQUE,
    fecha_registro DATETIME DEFAULT CURRENT_TIMESTAMP
);


CREATE TABLE Progreso (
    id_progreso INTEGER PRIMARY KEY AUTOINCREMENT,
    id_usuario INTEGER NOT NULL,
    niveles_completados INTEGER DEFAULT 0,
    puntuacion INTEGER DEFAULT 0,
    FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario) ON DELETE CASCADE
);


CREATE TABLE Tesoros (
    id_tesoro INTEGER PRIMARY KEY AUTOINCREMENT,
    nombre TEXT NOT NULL,
    descripcion TEXT,
    fecha_registro TEXT
);


CREATE TABLE Tesoros_Obtenidos (
    id_usuario INTEGER NOT NULL,
    id_tesoro INTEGER NOT NULL,
    fecha_obtenido DATETIME DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id_usuario, id_tesoro),
    FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario) ON DELETE CASCADE,
    FOREIGN KEY (id_tesoro) REFERENCES Tesoros(id_tesoro) ON DELETE CASCADE
);


INSERT INTO Usuarios (nombre_usuario, contrasenia, correo) VALUES
('Jugador1', 'password123', 'jugador1@email.com'),
('Jugador2', 'securepass', 'jugador2@email.com'),
('Jugador3', 'clave1234', 'jugador3@email.com');

INSERT INTO Progreso (id_usuario, niveles_completados, puntuacion) VALUES
(1, 5, 1500),
(2, 3, 900),
(3, 7, 2000);


INSERT INTO Tesoros (nombre, descripcion, fecha_registro) VALUES
('Tesoro Dorado', 'Un antiguo tesoro de oro', '2025-04-04'),
('Espada Legendaria', 'Una espada con poderes mágicos', '2025-04-04'),
('Gema Mística', 'Una gema con energía desconocida', '2025-04-04');


INSERT INTO Tesoros_Obtenidos (id_usuario, id_tesoro, fecha_obtenido) VALUES
(1, 1, '2025-04-04 10:00:00'),
(1, 2, '2025-04-04 11:00:00'),
(2, 3, '2025-04-04 12:00:00');
