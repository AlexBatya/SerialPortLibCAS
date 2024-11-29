using System;
using System.IO.Ports;
using System.Threading;

namespace SerialPortLibCAS {
  public class SerialPortHandler : IDisposable {
    private SerialPort _serialPort;
    private Thread _readThread;
    private bool _isRunning;

    // Событие для передачи полученных данных
    public event Action<string> DataReceived;

    // Конструктор с настройками
    public SerialPortHandler(string portName, int baudRate = 9600, Parity parity = Parity.None, 
                             int dataBits = 8, StopBits stopBits = StopBits.One, Handshake handshake = Handshake.None) {
      _serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits) {
        Handshake = handshake,
        DtrEnable = true,
        RtsEnable = true
      };
    }

    public void Open() {
      if (_serialPort.IsOpen) return;
      _serialPort.Open();
      _isRunning = true;
      _readThread = new Thread(ReadData);
      _readThread.Start();
    }

    public void Close() {
      if (!_serialPort.IsOpen) return;
      _isRunning = false;
      _readThread?.Join();
      _serialPort.Close();
    }

    private void ReadData() {
      while (_isRunning) {
        try {
          if (_serialPort.BytesToRead > 0) {
            string data = _serialPort.ReadExisting();
            DataReceived?.Invoke(data);
          }
        } catch (Exception ex) {
          Console.WriteLine($"Ошибка чтения: {ex.Message}");
        }
        Thread.Sleep(100);
      }
    }

    public void Dispose() {
      Close();
      _serialPort?.Dispose();
    }
  }
}

