# Введение

Модуль CI/CD предназначен для простой интеграции и настройки CI/CD с помощью сервиса codemagic.io.

## Импорт

```JSON
"dependencies": {
    "com.littlebitgames.cicd": "https://github.com/LittleBitOrganization/evolution-engine-cicd.git"
}
```

## Настройка codemagic.io

Создаем приложение
![image](https://user-images.githubusercontent.com/42607380/189104279-a92d4afa-0d45-42bf-92c4-295e3ddfdcb0.png)

Настраиваем приложение, вставляем ссылку на репозиторий
![image](https://user-images.githubusercontent.com/42607380/189104303-29049d1d-13dd-4067-a5ac-ef03f31e6e6b.png)

Вставляем кейстор от проекта
![image](https://user-images.githubusercontent.com/42607380/189104335-73233e8e-198b-48a9-8b7b-2cca6b23886d.png)

## Настройка CI/CD в проекте

Создаем конфиг
![image](https://user-images.githubusercontent.com/42607380/189104875-c5cb14fc-60ca-4bcb-9155-ef32489ea156.png)

Переходим, вводим айдишник для кейстора, который создали выше и нажимаем Edit Yaml File.

![image](https://user-images.githubusercontent.com/42607380/189105189-d0e95b71-8a24-45aa-b6bb-b76fcf63ff20.png)
