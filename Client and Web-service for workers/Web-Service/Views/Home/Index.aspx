<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Web_Service.Views.Home.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Welcome</title>
    <style>
        
        * {
            font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            margin: 8px;
        }
        body{
            margin: 0px;
            display: block;
            background-color: #cecdcc;
        }
        #content{
            background: #ffffff;
            margin-left: auto;
            margin-right: auto;
            margin-bottom: 0px;
            width: 1000px;
            box-shadow: 0 14px 28px rgba(0,0,0,0.25), 0 10px 10px rgba(0,0,0,0.22);
            padding-bottom: 10px;
        }
        a {
            color: blue;
        }
        a :hover{
            text-decoration: underline;
        }
        pre {
            background-color: #F6F8FA; 
            padding: 5px;
        }
        code {
            background-color: #F6F8FA; 
            font-family: Courier New, Courier, monospace;
        }
        pre span {
            font-family: Courier New, Courier, monospace;
        }
        table {
            width: 980px;
            border-collapse: collapse;
            margin: 10px;
            margin-right: -30px;
        }
        td {
            border-style: solid;
            border: 1px solid #cecece;
            padding: 6px;
        }
        th{
            border-style: solid;
            border: 1px solid #cecece;
            padding: 6px;
        }
    </style>
</head>
<body>
    <div id ="content">
    <h1 id="-book-wiki-api" style="text-align: center">API веб-сервера</h1>
<p><strong>Таблица интерфейсов</strong></p>
<table>
<thead>
<tr>
<th style="text-align:center">Контроллер</th>
<th style="text-align:center">Тип запроса</th>
<th>Краткое описание</th>
</tr>
</thead>
<tbody>
<tr>
<td style="text-align:center" rowspan="2">Autho</td>
<td style="text-align:center"><a href="#autho-post">POST</a></td>
<td>Авторизация по логину и паролю</td>
</tr>
<tr>
<td style="text-align:center"><a href="#autho-put">PUT</a></td>
<td>Авторизация по сессии, продление времени жизни сессии</td>
</tr>
<tr>
<td style="text-align:center" rowspan="3">Status</td>
<td style="text-align:center"><a href="#status-get">GET</a></td>
<td>Получение всех видов статуса</td>
</tr>
<tr>
<td style="text-align:center"><a href="#status-post">POST</a></td>
<td>Получение статуса работника</td>
</tr>
<tr>
<td style="text-align:center"><a href="#status-put">PUT</a></td>
<td>Установка нового статуса</td>
</tr>
<tr>
<td style="text-align:center" rowspan="2">Plan</td>
<td style="text-align:center"><a href="#plan-get">GET</a></td>
<td>Получение видов графиков</td>
</tr>
<tr>
<td style="text-align:center"><a href="#plan-post">POST</a></td>
<td>Получение всех графиков по параметрам</td>
</tr>
<tr>
<td style="text-align:center" rowspan="3">Task</td>
<td style="text-align:center"><a href="#task-get">GET</a></td>
<td>Получение стадий заданий</td>
</tr>
<tr>
<td style="text-align:center"><a href="#task-POST">POST</a></td>
<td>Получение заданий по параметрам</td>
</tr>
<tr>
<td style="text-align:center"><a href="#task-put">PUT</a></td>
<td>Установка новых значений</td>
</tr>
<tr>
<td style="text-align:center">Worker</td>
<td style="text-align:center"><a href="#worker-post">POST</a></td>
<td>Получение информации о работнике</td>
</tr>
</tbody>
</table>
<h2 id="autho-post">Autho POST</h2>
<p>Производит авторизацию пользователя по логину и паролю. 
<em>Пароль должен быть захеширован</em></p>
<p><strong>Входные данные:</strong></p>
<pre><code class="lang-json">{
    <span class="hljs-attr">"Login"</span>: <span class="hljs-string">"value"</span>,
    <span class="hljs-attr">"Password"</span>: <span class="hljs-string" aria-autocomplete="inline">"value"</span>
}</code></pre>
<p><strong>Выходные данные:</strong></p>
<pre><code class="lang-json">{
    <span class="hljs-attr">"Session"</span>: <span class="hljs-string">"value"</span>
}</code></pre>
<p><em>Коды ошибок</em></p>
<table>
<thead>
<tr>
<th style="text-align:center">Код</th>
<th>Сообщение</th>
<th>Причина</th>
</tr>
</thead>
<tbody>
<tr>
<td style="text-align:center">400, BadRequest</td>
<td>Ошибка сериализации запроса</td>
<td>Отправляемый запрос имел некорректный вид</td>
</tr>
<tr>
<td style="text-align:center">401, Unauthorized</td>
<td>Пользователь не найден</td>
<td>При авторизации не был найден пользователь с таким логином и паролем</td>
</tr>
<tr>
<td style="text-align:center">500, InternalServerError</td>
<td>Ошибка обработки сообщения</td>
<td>Во вермя обработки сообщения на сервере произошла ошибка</td>
</tr>
<tr>
<td style="text-align:center">500, InternalServerError</td>
<td>Сессия не была создана</td>
<td>Сессия не была создана по внутренней ошибке</td>
</tr>
</tbody>
</table>
<h2 id="autho-put">Autho PUT</h2>
<p>Производит авторизацию по сессии, продлевает ее вермя жизни</p>
<p><strong>Входные данные:</strong></p>
<pre><code class="lang-json">{
    <span class="hljs-attr">"Session"</span>: <span class="hljs-string">"value"</span>
}</code></pre>
<p><strong>Выходные данные отсутствуют</strong></p>
<p><em>Коды ошибок</em></p>
<table>
<thead>
<tr>
<th style="text-align:center">Код</th>
<th>Сообщение</th>
<th>Причина</th>
</tr>
</thead>
<tbody>
<tr>
<td style="text-align:center">400, BadRequest</td>
<td>Ошибка сериализации запроса</td>
<td>Отправляемый запрос имел некорректный вид</td>
</tr>
<tr>
<td style="text-align:center">401, Unauthorized</td>
<td>Сессия не была найдена</td>
<td>Авторизация не была пройдена; такая сессия не существует</td>
</tr>
<tr>
<td style="text-align:center">401, Unauthorized</td>
<td>Клиент не был найден</td>
<td>Авторизация не была пройдена; отправляемый клиент-отправитель не найден</td>
</tr>
<tr>
<td style="text-align:center">500, InternalServerError</td>
<td>Ошибка обработки сообщения</td>
<td>Во вермя обработки сообщения на сервере произошла ошибка</td>
</tr>
</tbody>
</table>
<h2 id="status-get">Status GET</h2>
<p>Возвращает список всех возможных статусов</p>
<p><strong>Входные данные отсутствуют</strong></p>
<p><strong>Выходные данные:</strong></p>
<pre><code class="lang-json">[
    {
        <span class="hljs-attr">"StatusCode"</span>: <span class="hljs-string">"value"</span>,
        <span class="hljs-attr">"Title"</span>: <span class="hljs-string">"value"</span>,
        <span class="hljs-attr">"Description"</span>: <span class="hljs-string">"value"</span>
    },

]</code></pre>
<p><em>Коды ошибок</em></p>
<table>
<thead>
<tr>
<th style="text-align:center">Код</th>
<th>Сообщение</th>
<th>Причина</th>
</tr>
</thead>
<tbody>
<tr>
<td style="text-align:center">500, InternalServerError</td>
<td>Ошибка обработки сообщения</td>
<td>Во вермя обработки сообщения на сервере произошла ошибка</td>
</tr>
</tbody>
</table>
<h2 id="status-post">Status POST</h2>
<p>Получает статус пользователя по сессии</p>
<p><strong>Входные данные:</strong></p>
<pre><code class="lang-json">{
    <span class="hljs-attr">"Session"</span>: <span class="hljs-string">"value"</span>
}</code></pre>
<p><strong>Выходные данные:</strong></p>
<pre><code class="lang-json">{
    <span class="hljs-attr">"StatusCode"</span>: <span class="hljs-string">"value"</span>,
    <span class="hljs-attr">"Updated"</span>: <span class="hljs-string">"value"</span>
}</code></pre>
<p><em>Коды ошибок</em></p>
<table>
<thead>
<tr>
<th style="text-align:center">Код</th>
<th>Сообщение</th>
<th>Причина</th>
</tr>
</thead>
<tbody>
<tr>
<td style="text-align:center">400, BadRequest</td>
<td>Ошибка сериализации запроса</td>
<td>Отправляемый запрос имел некорректный вид</td>
</tr>
<tr>
<td style="text-align:center">401, Unauthorized</td>
<td>Сессия не была найдена</td>
<td>Авторизация не была пройдена; такая сессия не существует</td>
</tr>
<tr>
<td style="text-align:center">401, Unauthorized</td>
<td>Клиент не был найден</td>
<td>Авторизация не была пройдена; отправляемый клиент-отправитель не найден</td>
</tr>
<tr>
<td style="text-align:center">500, InternalServerError</td>
<td>Ошибка обработки сообщения</td>
<td>Во вермя обработки сообщения на сервере произошла ошибка</td>
</tr>
</tbody>
</table>
<h2 id="status-put">Status PUT</h2>
<p>Устанавливает новый статус пользователя по сессии</p>
<p><strong>Входные данные:</strong></p>
<pre><code class="lang-json">{
    <span class="hljs-attr">"Session"</span>: <span class="hljs-string">"value"</span>,
    <span class="hljs-attr">"StatusCode"</span>: <span class="hljs-string">"value"</span>
}</code></pre>
<p><strong>Выходные данные отсутствуют</strong></p>
<p><em>Коды ошибок</em></p>
<table>
<thead>
<tr>
<th style="text-align:center">Код</th>
<th>Сообщение</th>
<th>Причина</th>
</tr>
</thead>
<tbody>
<tr>
<td style="text-align:center">400, BadRequest</td>
<td>Ошибка сериализации запроса</td>
<td>Отправляемый запрос имел некорректный вид</td>
</tr>
<tr>
<td style="text-align:center">400, BadRequest</td>
<td>Статус не может быть обновлён</td>
<td>Уже установленный статус не может быть обновлён</td>
</tr>
<tr>
<td style="text-align:center">400, BadRequest</td>
<td>Статус не может быть установлен</td>
<td>Такой статус не может быть установленым</td>
</tr>
<tr>
<td style="text-align:center">401, Unauthorized</td>
<td>Сессия не была найдена</td>
<td>Авторизация не была пройдена; такая сессия не существует</td>
</tr>
<tr>
<td style="text-align:center">401, Unauthorized</td>
<td>Клиент не был найден</td>
<td>Авторизация не была пройдена; отправляемый клиент-отправитель не найден</td>
</tr>
<tr>
<td style="text-align:center">500, InternalServerError</td>
<td>Ошибка обработки сообщения</td>
<td>Во вермя обработки сообщения на сервере произошла ошибка</td>
</tr>
</tbody>
</table>
<h2 id="plan-get">Plan GET</h2>
<p>Получение списка всех видов графиков</p>
<p><strong>Входные данные отсутствуют</strong></p>
<p><strong>Выходные данные:</strong></p>
<pre><code class="lang-json">[
    {
        <span class="hljs-attr">"PlanCode"</span>: <span class="hljs-string">"value"</span>,
        <span class="hljs-attr">"Title"</span>: <span class="hljs-string">"value"</span>
    },

]</code></pre>
<p><em>Коды ошибок</em></p>
<table>
<thead>
<tr>
<th style="text-align:center">Код</th>
<th>Сообщение</th>
<th>Причина</th>
</tr>
</thead>
<tbody>
<tr>
<td style="text-align:center">500, InternalServerError</td>
<td>Ошибка обработки сообщения</td>
<td>Во вермя обработки сообщения на сервере произошла ошибка</td>
</tr>
</tbody>
</table>
<h2 id="plan-post">Plan POST</h2>
<p>Получение спсика графиков по сессии, начальному и конечному дню отбору и типов графика</p>
<p><strong>Входные данные:</strong></p>
<pre><code class="lang-json">{
    <span class="hljs-attr">"Session"</span>: <span class="hljs-string">"value"</span>,
    <span class="hljs-attr">"StartDate"</span>: <span class="hljs-string">"value"</span>,
    <span class="hljs-attr">"EndDate"</span>: <span class="hljs-string">"value"</span>,
    <span class="hljs-attr">"PlanCodes"</span>: [<span class="hljs-string">"value"</span>, ]
}</code></pre>
<p><em><code>PlanCodes</code> может быть <code>null</code>, чтобы выбрать все планы без фильтрации по типам</em></p>
<p><strong>Выходные данные:</strong></p>
<pre><code class="lang-json">[
    {
        <span class="hljs-attr">"Date"</span>: <span class="hljs-string">"value"</span>,
        <span class="hljs-attr">"DayStart"</span>: <span class="hljs-string">"value"</span>,
        <span class="hljs-attr">"DayEnd"</span>: <span class="hljs-string">"value"</span>,
        <span class="hljs-attr">"PlanCode"</span>: <span class="hljs-string">"value"</span>
    },

]</code></pre>
<p><em>Коды ошибок</em></p>
<table>
<thead>
<tr>
<th style="text-align:center">Код</th>
<th>Сообщение</th>
<th>Причина</th>
</tr>
</thead>
<tbody>
<tr>
<td style="text-align:center">400, BadRequest</td>
<td>Ошибка сериализации запроса</td>
<td>Отправляемый запрос имел некорректный вид</td>
</tr>
<tr>
<td style="text-align:center">400, BadRequest</td>
<td>Неправильные значения даты</td>
<td>Начальная дата была больше конечной</td>
</tr>
<tr>
<td style="text-align:center">401, Unauthorized</td>
<td>Сессия не была найдена</td>
<td>Авторизация не была пройдена; такая сессия не существует</td>
</tr>
<tr>
<td style="text-align:center">401, Unauthorized</td>
<td>Клиент не был найден</td>
<td>Авторизация не была пройдена; отправляемый клиент-отправитель не найден</td>
</tr>
<tr>
<td style="text-align:center">500, InternalServerError</td>
<td>Ошибка обработки сообщения</td>
<td>Во вермя обработки сообщения на сервере произошла ошибка</td>
</tr>
</tbody>
</table>
<h2 id="task-get">Task GET</h2>
<p>Получает список всех стадий задач</p>
<p><strong>Входные данные отсутствуют</strong></p>
<p><strong>Выходные данные:</strong></p>
<pre><code class="lang-json">[
    {
        <span class="hljs-attr">"Stage"</span>: <span class="hljs-string">"value"</span>,
        <span class="hljs-attr">"Title"</span>: <span class="hljs-string">"value"</span>
    },

]</code></pre>
<p><em>Коды ошибок</em></p>
<table>
<thead>
<tr>
<th style="text-align:center">Код</th>
<th>Сообщение</th>
<th>Причина</th>
</tr>
</thead>
<tbody>
<tr>
<td style="text-align:center">500, InternalServerError</td>
<td>Ошибка обработки сообщения</td>
<td>Во вермя обработки сообщения на сервере произошла ошибка</td>
</tr>
</tbody>
</table>
<h2 id="task-post">Task POST</h2>
<p>Получает список заданий по сессии, стадии задач, дате создания</p>
<p><strong>Входные данные:</strong></p>
<pre><code class="lang-json">{
    <span class="hljs-attr">"Session"</span>: <span class="hljs-string">"value"</span>,
    <span class="hljs-attr">"TaskStages"</span>: [<span class="hljs-string">"value"</span>, ],
    <span class="hljs-attr">"DateCreation"</span>: <span class="hljs-string">"value"</span>
}</code></pre>
<p><em><code>DateCreation</code> может быть <code>null</code>, чтобы выбрать все задачи, не учитывая даты создания</em></p>
<p><em><code>TaskStages</code> может быть <code>null</code>, чтобы выбрать все задачи без фильтрации по стадиям</em></p>
<p><strong>Выходные данные:</strong></p>
<pre><code class="lang-json">[
    {
        <span class="hljs-attr">"TaskId"</span>: <span class="hljs-string">"value"</span>,
        <span class="hljs-attr">"Description"</span>: <span class="hljs-string">"value"</span>,
        <span class="hljs-attr">"SetterWorkerName"</span>: <span class="hljs-string">"value"</span>,
        <span class="hljs-attr">"Created"</span>: <span class="hljs-string">"value"</span>,
        <span class="hljs-attr">"Finished"</span>: <span class="hljs-string">"value"</span>,
        <span class="hljs-attr">"Stage"</span>: <span class="hljs-string">"value"</span>
    },

]</code></pre>
<p><em>Коды ошибок</em></p>
<table>
<thead>
<tr>
<th style="text-align:center">Код</th>
<th>Сообщение</th>
<th>Причина</th>
</tr>
</thead>
<tbody>
<tr>
<td style="text-align:center">400, BadRequest</td>
<td>Ошибка сериализации запроса</td>
<td>Отправляемый запрос имел некорректный вид</td>
</tr>
<tr>
<td style="text-align:center">400, BadRequest</td>
<td>Некорректный спсиок стадий</td>
<td>Полученный список стадий имеет некорректные значения</td>
</tr>
<tr>
<td style="text-align:center">401, Unauthorized</td>
<td>Сессия не была найдена</td>
<td>Авторизация не была пройдена; такая сессия не существует</td>
</tr>
<tr>
<td style="text-align:center">401, Unauthorized</td>
<td>Клиент не был найден</td>
<td>Авторизация не была пройдена; отправляемый клиент-отправитель не найден</td>
</tr>
<tr>
<td style="text-align:center">500, InternalServerError</td>
<td>Ошибка обработки сообщения</td>
<td>Во вермя обработки сообщения на сервере произошла ошибка</td>
</tr>
</tbody>
</table>
<h2 id="task-put">Task PUT</h2>
<p>Устанавливает задаче выбранное состояние (<code>приянтое к исполнению</code> или <code>завершенное</code>)</p>
<p><em>Изменение задачи может происходить только тогда, когда работник находится на работе</em></p>
<p><strong>Входные данные:</strong></p>
<pre><code class="lang-json">{
    <span class="hljs-attr">"Session"</span>: <span class="hljs-string">"value"</span>,
    <span class="hljs-attr">"TaskId"</span>: <span class="hljs-string">"value"</span>,
    <span class="hljs-attr">"Stage"</span>: <span class="hljs-string">"value"</span>
}</code></pre>
<p><strong>Выходные данные отсутствуют</strong></p>
<p><em>Коды ошибок</em></p>
<table>
<thead>
<tr>
<th style="text-align:center">Код</th>
<th>Сообщение</th>
<th>Причина</th>
</tr>
</thead>
<tbody>
<tr>
<td style="text-align:center">400, BadRequest</td>
<td>Ошибка сериализации запроса</td>
<td>Отправляемый запрос имел некорректный вид</td>
</tr>
<tr>
<td style="text-align:center">400, BadRequest</td>
<td>Такая стадия не может быть установлена</td>
<td>Полученный статус не может быть установлен</td>
</tr>
<tr>
<td style="text-align:center">400, BadRequest</td>
<td>Работник не на рабочем месте</td>
<td>Для установки стадии необходимо быть на рабочем месте</td>
</tr>
<tr>
<td style="text-align:center">401, Unauthorized</td>
<td>Сессия не была найдена</td>
<td>Авторизация не была пройдена; такая сессия не существует</td>
</tr>
<tr>
<td style="text-align:center">401, Unauthorized</td>
<td>Клиент не был найден</td>
<td>Авторизация не была пройдена; отправляемый клиент-отправитель не найден</td>
</tr>
<tr>
<td style="text-align:center">500, InternalServerError</td>
<td>Ошибка обработки сообщения</td>
<td>Во вермя обработки сообщения на сервере произошла ошибка</td>
</tr>
</tbody>
</table>
<h2 id="worker-post">Worker POST</h2>
<p>Получает информацию о сотруднике по сессии</p>
<p><strong>Входные данные:</strong></p>
<pre><code class="lang-json">{
    <span class="hljs-attr">"Session"</span>: <span class="hljs-string">"value"</span>
}</code></pre>
<p><strong>Выходные данные:</strong></p>
<pre><code class="lang-json">{
    <span class="hljs-attr">"Name"</span>: <span class="hljs-string">"value"</span>,
    <span class="hljs-attr">"Surname"</span>: <span class="hljs-string">"value"</span>,
    <span class="hljs-attr">"Patronymic"</span>: <span class="hljs-string">"value"</span>,
    <span class="hljs-attr">"Department"</span>: <span class="hljs-string">"value"</span>,
    <span class="hljs-attr">"Position"</span>: <span class="hljs-string">"value"</span>
}</code></pre>
<p><em>Коды ошибок</em></p>
<table>
<thead>
<tr>
<th style="text-align:center">Код</th>
<th>Сообщение</th>
<th>Причина</th>
</tr>
</thead>
<tbody>
<tr>
<td style="text-align:center">400, BadRequest</td>
<td>Ошибка сериализации запроса</td>
<td>Отправляемый запрос имел некорректный вид</td>
</tr>
<tr>
<td style="text-align:center">401, Unauthorized</td>
<td>Сессия не была найдена</td>
<td>Авторизация не была пройдена; такая сессия не существует</td>
</tr>
<tr>
<td style="text-align:center">401, Unauthorized</td>
<td>Клиент не был найден</td>
<td>Авторизация не была пройдена; отправляемый клиент-отправитель не найден</td>
</tr>
<tr>
<td style="text-align:center">500, InternalServerError</td>
<td>Ошибка обработки сообщения</td>
<td>Во вермя обработки сообщения на сервере произошла ошибка</td>
</tr>
</tbody>
</table>
</div>
</body>
</html>
