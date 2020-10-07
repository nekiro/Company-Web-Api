# Company Web Api

Company Web Api is a simple Web Api Console App, which let you create new companies and manage them. Companies and their employees are saved in relational database.
Aditionally requests are protected by Base64 basic authentication.
Authentication header is following:
```
Basic <encodedBase64 admin:admin>
```
It was written as a test task for a job application.

Technologies:
  - .Net Core 3.1
  - Restful Api
  - C#
  - MSSQL
  - Entity Framework

## Supported requests

```
POST /company/create
```

```
{
    "Name": "<string>",
    "EstablishmentYear”: <integer>,
    "Employees": [{
        "FirstName": "<string>",
        "LastName": "<string>",
        "DateOfBirth": "<DateTime>",
        "JobTitle": "<string(enum)>"
    }, ... ]
}
```
Answer
```
{
    "Id”: <long>
}
```
&nbsp;
```
POST /company/search
```
```
{
    "Keyword": "<string>",
    "EmployeeDateOfBirthFrom": "<DateTime?>",
    "EmployeeDateOfBirthTo": "<DateTime?>",
    "EmployeeJobTitles": [“<string(enum)>”, ...]
}
```
Answer
```
[{
        "Name": "<string>",
        "EstablishmentYear”: <integer>,
        "Employees": [{
        "FirstName": "<string>",
        "LastName": "<string>",
        "DateOfBirth": "<DateTime>",
        "JobTitle": "<string(enum)>"
}, ...]
```
&nbsp;
```
PUT /company/update/<id>
```
```
{
    "Name": "<string>",
    "EstablishmentYear”: <integer>,
    "Employees": [{
        "FirstName": "<string>",
        "LastName": "<string>",
        "DateOfBirth": "<DateTime>",
        "JobTitle": "<string(enum)>"
    }, ...]
}
```
&nbsp;
```
PUT /company/delete/<id>
```

License
----

MIT
