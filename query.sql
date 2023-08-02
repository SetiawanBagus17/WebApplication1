-- Create the database
CREATE DATABASE coding_id_backend;

-- Use the database
USE coding_id_backend;

-- Create the Users table
CREATE TABLE Users (
    pk_users_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(25)
);

-- Create the Tasks table
CREATE TABLE Tasks (
    pk_tasks_id INT AUTO_INCREMENT PRIMARY KEY,
    task_detail TEXT,
    fk_user_id INT
);

