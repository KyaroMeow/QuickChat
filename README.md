# QuickChat

# QuickChatBase - Серверная часть чат-приложения

![GitHub](https://img.shields.io/badge/.NET-6.0-blue)
![GitHub](https://img.shields.io/badge/PostgreSQL-15.0-blue)
![GitHub](https://img.shields.io/badge/EF_Core-7.0-blue)

Серверная часть чат-приложения, разработанная на ASP.NET Core с использованием PostgreSQL в качестве базы данных. Проект предоставляет RESTful API для управления пользователями, чатами и сообщениями.

## 📌 Оглавление

- [Особенности](#-особенности)
- [Технологии](#-технологии)
- [Установка и запуск](#-установка-и-запуск)
- [API Endpoints](#-api-endpoints)
- [Структура базы данных](#-структура-базы-данных)
- [Разработчики](#-разработчики)

## 🌟 Особенности

- Регистрация и аутентификация пользователей
- Создание личных и групповых чатов
- Отправка и получение сообщений
- Отметка сообщений как прочитанных
- Хранение истории сообщений
- Статусы онлайн/оффлайн пользователей

## 💻 Технологии

- **Backend:** ASP.NET Core 6.0
- **Database:** PostgreSQL 15
- **ORM:** Entity Framework Core 7.0
- **Authentication:** SHA-256 хеширование паролей
- **API Documentation:** Swagger/OpenAPI

## 🚀 Установка и запуск

1. **Клонируйте репозиторий:**
   ```bash
   git clone https://github.com/yourusername/QuickChatBase.git
   cd QuickChatBase/Server
   ```

2. **Настройте базу данных:**
   - Установите PostgreSQL 15+
   - Создайте базу данных с именем `QuickChatBase`
   - Обновите строку подключения в `appsettings.json`:
     ```json
     "ConnectionStrings": {
       "QuickChatBase": "Host=localhost;Port=5432;Database=QuickChatBase;Username=youruser;Password=yourpassword"
     }
     ```

3. **Примените миграции:**
   ```bash
   dotnet ef database update
   ```

4. **Запустите сервер:**
   ```bash
   dotnet run
   ```

5. **Документация API:**
   - После запуска откройте в браузере `https://localhost:5001/swagger`

## 📡 API Endpoints

### Аутентификация
- `POST /api/auth/register` - Регистрация нового пользователя
- `POST /api/auth/login` - Вход пользователя

### Пользователи
- `GET /api/users` - Получить список пользователей

### Чаты
- `GET /api/chats` - Получить чаты пользователя
- `POST /api/chats` - Создать новый чат

### Сообщения
- `GET /api/messages` - Получить список сообщений
- `GET /api/messages/chat/{chatId}` - Получить сообщения чата
- `POST /api/messages` - Отправить сообщение
- `PUT /api/messages/{id}` - Обновить сообщение
- `DELETE /api/messages/{id}` - Удалить сообщение

## 🗃️ Структура базы данных



Основные таблицы:
- `users` - информация о пользователях
- `chats` - данные чатов
- `messages` - история сообщений
- `userchats` - связь пользователей с чатами (многие-ко-многим)
