# emailAPI
A simple ASP.NET Core API for sending contact form emails with built-in rate limiting via Redis.

## 🚀 Features

- ✅ Send emails using a service class (EmailService) with dependency injection.
- ✅ Redis-based IP rate limiter to prevent spam.
- ✅ Clean and configurable middleware architecture.
- ✅ Environment-variable-based configuration for security.

---

## 🧱 Tech Stack
| Layer        | Technology            |
|--------------|------------------------|
| Language     | C#                     |
| Framework    | ASP.NET Core Web API   |
| DB / Cache   | Redis (Cloud)       |

---

## 📁 Architecture Overview

```bash
Controllers/EmailController.cs # Receives requests and calls the service.
Services/EmailService.cs # Handles email sending logic.
Middleware/RateLimiterMiddleware.cs # Implements IP-based rate limiting with Redis.
Models/ContactForm.cs # Defines the data structure for contact form submissions.
Extensions/RedisExtension.cs # Configures Redis connection using dependency injection.
```

---

## 🛠️ Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/BrandonGatewood/emailAPI.git
cd emailAPI
```

### 2. Configure environment
Create a .env file in the root directory with the following:
```env
SMTP_SERVER=your_smtp_host
SMTP_PORT=587
EMAIL=your_email_username
PASSWORD=your_email_password
API_KEY=your_api_key
REDIS_HOST=your_redis_host
REDIS_PORT=your_redis_port
REDIS_PASSWORD=your_redis_password
RATE_LIMIT=2
WINDOW_SEC=60
```

### 3. Run the application
```bash
dotnet run
```

### 4. Send POST requests to `/email` with the contact form payload:
```JSON
  {
    "name": "John Doe",
    "email": "john@example.com",
    "message": "Hello!"
  }
  ```

---

## License
This project is licensed under the MIT License.
