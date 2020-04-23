# :book: Wiki по API

**Оглавление**

- [Autho POST](#Autho-POST)
- [Autho PUT](#Autho-PUT)
- [Status GET](#Status-GET)
- [Status POST](#Status-POST)
- [Status PUT](#Status-PUT)
- [Plan GET](#Plan-Get)
- [Plan POST](#Plan-POST)
- [Task GET](#Task-GET)
- [Task POST](#Task-POST)
- [Task PUT](#Task-PUT)
- [Worker POST](#Worker-POST)

## Autho POST

Производит авторизацию пользователя по логину и паролю. 
*Пароль должен быть захеширован*

**Входные данные:**

```json
{
    "Login": "value",
    "Password": "value"
}
```

**Выходные данные:**

```json
{
    "Session": "value"
}
```

*Коды ответа*


## Autho PUT

Производит авторизацию по сессии, продлевает ее вермя жизни

**Входные данные:**

```json
{
    "Session": "value"
}
```

**Выходные данные отсутствуют**

*Коды ответа*

## Status GET

Возвращает список всех возможных статусов

**Входные данные отсутствуют**

**Выходные данные:**

```json
[
	{
		"StatusCode": "value",
		"Title": "value",
		"Description": "value"
	},

]
```

*Коды ответа*


## Status POST

Получает статус пользователя по сессии

**Входные данные:**

```json
{
    "Session": "value"
}
```

**Выходные данные:**

```json
{
	"StatusCode": "value",
    "Updated": "value"
}
```

*Коды ответа*


## Status PUT

Устанавливает новый статус пользователя по сессии

**Входные данные:**

```json
{
	"Session": "value",
	"StatusCode": "value"
}
```

**Выходные данные отсутствуют**


*Коды ответа*


## Plan GET

Получение списка всех видов графиков

**Входные данные отсутствуют**

**Выходные данные:**

```json
[
	{
		"PlanCode": "value",
		"Title": "value"
	},

]
```

*Коды ответа*


## Plan POST

Получение спсика графиков по сессии, начальному и конечному дню отбору и типов графика

**Входные данные:**

```json
{
	"Session": "value",
	"StartDate": "value",
	"EndDate": "value",
	"PlanCodes": ["value", ]
}
```

**Выходные данные:**

```json
[
	{
		"Date": "value",
		"DayStart": "value",
		"DayEnd": "value",
		"PlanCode": "value"
	},

]
```

*Коды ответа*


## Task GET

Получает список всех стадий задач

**Входные данные отсутствуют**

**Выходные данные:**

```json
[
	{
		"Stage": "value",
		"Title": "value"
	},

]
```

*Коды ответа*


## Task POST

Получает список заданий по сессии, стадии задач, дате создания - ?

**Входные данные:**

```json
{
	"Session": "value",
	"TaskStages": ["value", ],
	"DateCreation": "value"
}
```

*`DateCreation` может быть `null`, чтобы выбрать все задачи, не учитывая даты создания*

**Выходные данные:**

```json
[
	{
		"TaskId": "value",
		"Description": "value",
		"SetterWorkerName": "value",
		"Created": "value",
		"Finished": "value"
	},

]
```

*Коды ответа*


## Task PUT

Устанавливает задаче завершенное состояние

**Входные данные:**

```json
{
	"Session": "value",
	"TaskId": "value"
}
```

**Выходные данные отсутствуют**

*Коды ответа*


## Worker POST

Получает информацию о сотруднике по сессии

**Входные данные:**

```json
{
    "Session": "value"
}
```

**Выходные данные:**

```json
{
    "Name": "value",
    "Surname": "value",
    "Patonymic": "value",
    "Department": "value",
    "Posistion": "value"
}
```

*Коды ответа*
