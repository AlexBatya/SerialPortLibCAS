# SerialPortLibCAS

`SerialPortLibCAS` — это библиотека для работы с COM-портом, предоставляющая удобный интерфейс для отправки и получения данных. Она включает поддержку событий, асинхронного чтения и освобождения ресурсов через интерфейс `IDisposable`.

---

## Возможности
- Открытие и закрытие COM-порта.
- Отправка и получение данных через COM-порт.
- Асинхронное чтение данных с использованием потоков.
- Обработка событий для передачи полученных данных в реальном времени.
- Удобная настройка параметров порта: скорость передачи данных, биты данных, четность и другие.

---

## Установка

### Использование как NuGet-пакет
1. Создайте локальный NuGet-пакет из библиотеки:
   ```bash
   dotnet pack --output ./nupkgs
   ```

2. Добавьте локальный источник NuGet:
    ```bash
    dotnet nuget add source ./nupkgs --name LocalSource
    ```

3. Подключите библиотеку к проекту:
    ```bash
    dotnet add package SerialPortLibCAS --source LocalSource
    ```

### Подключение вручную

1. Скомпилируйте библиотеку и найдите файл ``SerialPortLibCAS.dll``.

2. Добавьте его в свой проект.
    ```bash
    dotnet add reference path/to/SerialPortLibCAS.dll
    ```
## Использование

### Пример использования

```csharp
using SerialPortLibCAS;

class Program {
  static void Main(string[] args) {
    var serialHandler = new SerialPortHandler("COM4");

    try {
      serialHandler.Open();
      Console.WriteLine("Порт открыт. Ожидание данных...");

      // Подписка на событие получения данных
      serialHandler.DataReceived += (data) =>
      {
        Console.WriteLine($"Полученные данные: {data}");
      };

      // Имитация работы
      Console.ReadLine();
    }
    finally {
      serialHandler.Close();
      Console.WriteLine("Порт закрыт.");
    }
  }
}
```
## API библиотеки

### Класс SerialPortHandler

**Конструктор** 

```csharp
SerialPortHandler(
    string portName,
    int baudRate = 9600,
    Parity parity = Parity.None,
    int dataBits = 8,
    StopBits stopBits = StopBits.One,
    Handshake handshake = Handshake.None
)
```

**Параметры:**

- ``portName``: имя COM-порта (например, ``"COM4"``).
- ``baudRate``: скорость передачи данных, по умолчанию 9600.
- ``parity``: четность (enum ``Parity``), по умолчанию ``None``.
- ``dataBits``:  количество бит данных, по умолчанию 8.
- ``stopBits``: стоп-биты (enum StopBits), по умолчанию ``One``.
- ``handshake``: аппаратное управление потоком (enum ``Handshake``), по умолчанию ``None``.

**Методы:**

1. ``void Open()``
Открывает COM-порт и запускает поток для асинхронного чтения данных.
Генерирует исключение, если порт не удаётся открыть.
2. ``void Close()``
Закрывает COM-порт и останавливает поток чтения.
3. ``void Dispose()``
Освобождает ресурсы, закрывает порт и завершает работу потоков.

**События:**

1. ``event Action<string> DataReceived``
Вызывается при получении данных из COM-порта. Передаёт полученные данные в виде строки.

## Особенности

- **Асинхронное чтение данных:** использование потоков позволяет обрабатывать данные в реальном времени.
- **Обработка ошибок:** библиотека перехватывает ошибки чтения и сообщает о них в консоль.
- **Совместимость:** библиотека работает с любой .NET-платформой, поддерживающей ``System.IO.Ports``.

## Советы по использованию

- **Закрывайте порт:** убедитесь, что метод ``Close ``вызывается всегда, чтобы освободить ресурсы.
- **Обработка событий:** используйте событие ``DataReceived`` для обработки данных без блокирования основного потока.
- **Настройка порта:** убедитесь, что параметры порта соответствуют настройкам подключенного устройства.
