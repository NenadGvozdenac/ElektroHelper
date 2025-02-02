DROP TABLE IF EXISTS tokens;
DROP TABLE IF EXISTS notifications;
DROP TABLE IF EXISTS electricity_readings;
DROP TABLE IF EXISTS electricity_meters;
DROP TABLE IF EXISTS locations;
DROP TABLE IF EXISTS users;

CREATE TABLE IF NOT EXISTS users (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    surname VARCHAR(255) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    phone VARCHAR(15),
    password VARCHAR(255) NOT NULL,
    creation_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    role VARCHAR(50) NOT NULL
);

CREATE TABLE IF NOT EXISTS locations (
    id SERIAL PRIMARY KEY,
    street VARCHAR(255) NOT NULL,
    number VARCHAR(10) NOT NULL,
    city VARCHAR(255) NOT NULL,
    country VARCHAR(255) NOT NULL,
    postal_code VARCHAR(10) NOT NULL,
    user_id INT NOT NULL,
    CONSTRAINT fk_user FOREIGN KEY (user_id) REFERENCES users(id)
);

CREATE TABLE IF NOT EXISTS electricity_meters (
    id SERIAL PRIMARY KEY,
    location_id INT NOT NULL,
    meter_code VARCHAR(255) NOT NULL,
    date_of_registration TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_location FOREIGN KEY (location_id) REFERENCES locations(id)
);

CREATE TABLE IF NOT EXISTS electricity_readings (
    id SERIAL PRIMARY KEY,
    electricity_meter_id INT NOT NULL,
    reading_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    lower_reading VARCHAR(255) NOT NULL,
    upper_reading VARCHAR(255) NOT NULL,
    CONSTRAINT fk_electricity_meter FOREIGN KEY (electricity_meter_id) REFERENCES electricity_meters(id)
);

CREATE TABLE IF NOT EXISTS notifications (
    id SERIAL PRIMARY KEY,
    user_id INT NOT NULL,
    notification_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    subject VARCHAR(255) NOT NULL,
    message TEXT NOT NULL,
    CONSTRAINT fk_user FOREIGN KEY (user_id) REFERENCES users(id)
);

CREATE TABLE IF NOT EXISTS tokens (
    id SERIAL PRIMARY KEY,
    user_id INT NOT NULL,
    refresh_token TEXT NOT NULL UNIQUE,
    expires_at TIMESTAMP NOT NULL,
    issued_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    revoked BOOLEAN DEFAULT FALSE,
    device VARCHAR(255),
    ip_address VARCHAR(255),
    user_agent TEXT,
    CONSTRAINT fk_user FOREIGN KEY (user_id) REFERENCES users(id)
);
