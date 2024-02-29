﻿
#### Путь логеру
 > [Logging](./Models/Logging/LoggerInit.cs)
 ***
#### Путь к конфигурации
 > [Config](App.config)
 ***
#### Короткое описание и сколько этапов готово
- [x] Все вычисления должны осуществляться в отдельном потоке.
##### Задание для сервера
- [x] При запуске сервер должен циклически воспроизводить сценарий событий из прошлого этапа с таймаутом между событиями, указанным в конфигурации
- [x] При попытке подключения клиента сервер должен разрешить/запретить подключение при проверке соответствия пользователя и пароля в БД
- [x] При успешном подключении клиента сервер должен отправлять данные о каждом действии
##### Серверная часть должна соответствовать следующим требованиям:
- [x] Выполнять все вычисления, управлять авторизацией, взаимодействовать с БД
- [x] Принимать подключения от нескольких клиентов
- [x] Допускается реализация серверной части без графического интерфейса
- [ ] Необходимо логировать действия при подключении или отключении клиента


***
#### Дополнительная информация
##### Команды
 > -auth login@password
 > -spam
 ***
База данных: PostgreSql
***
Таблица:

```
CREATE TABLE IF NOT EXISTS Users(
	id SERIAL PRIMARY KEY,
	name CHARACTER VARYING(128),
	password CHARACTER VARYING(64),
	login CHARACTER VARYING(64) UNIQUE
);
```

***