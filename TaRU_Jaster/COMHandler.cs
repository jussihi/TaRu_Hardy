﻿using System;
using System.Threading.Tasks;
using System.IO.Ports;

/* TaRu Logger */
using static TaRU_Jaster.Logger;

namespace TaRU_Jaster
{
    public class COMHandler
    {
        public enum ComStatus
        {
            Disconnected,
            Connected,
            Sending,
            Receiving
        }

        private ComStatus _comStatus;
        public event System.EventHandler ComStatusChanged;

        public ComStatus pComStatus
        {
            get
            {
                return _comStatus;
            }

            set
            {
                bool changed = false;
                if (_comStatus != value)
                {
                    _comStatus = value;
                    changed = true;
                }
                if (ComStatusChanged != null && changed) ComStatusChanged(this, EventArgs.Empty);
            }
        }

        private SerialPort _serialPort;
        private int _serialTimeOut;

        public COMHandler()
        {
            _serialPort = new SerialPort();
            _serialPort.ErrorReceived += HandleSerialError;
            LOG("Hardy Commander Executor initialized!", DEBUG);

            _comStatus = ComStatus.Disconnected;
            _serialTimeOut = 500;
        }


        public string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }

        public bool ConnectSerial(string w_portName)
        {
            try
            {
                _serialPort.PortName = w_portName;
                _serialPort.BaudRate = 4800;
                _serialPort.Parity = Parity.None;
                _serialPort.DataBits = 8;
                _serialPort.StopBits = StopBits.One;
                _serialPort.Handshake = Handshake.None;
                _serialPort.Open();
                pComStatus = ComStatus.Connected;
                // clear serial port data
                _serialPort.DiscardInBuffer();

                // add listener for new data
                _serialPort.DataReceived += DataReceivedHandler;
                return true;
            }
            catch (Exception ex)
            {
                LOG("Failed opening port " + _serialPort.PortName + "! error message: " + ex.Message, ERR);
                return false;
            }
        }

        public bool DisonnectSerial()
        {
            try
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.DiscardInBuffer();
                    _serialPort.DiscardInBuffer();
                    _serialPort.Close();
                }
                pComStatus = ComStatus.Disconnected;
                return true;
            }
            catch (Exception ex)
            {
                LOG("Failed closing port " + _serialPort.PortName + "! error message: " + ex.Message, ERR);
                return false;
            }
        }

        public bool IsSerialOpen()
        {
            return _serialPort.IsOpen;
        }


        public async Task<bool> SendSerial(byte[] w_data, int w_timeout = 0)
        {
            // Sanity check
            if (w_data == null)
            {
                return false;
            }

            // Catch exceptions, show to user
            try
            {
                if (_serialPort.IsOpen)
                {
                    pComStatus = ComStatus.Sending;
                    if (w_timeout <= 0) w_timeout = _serialTimeOut;
                    var receiveTask = Task.Run(async () => {
                        await _serialPort.BaseStream.WriteAsync(w_data, 0, w_data.Length);
                    });
                    var isReceived = await Task.WhenAny(receiveTask, Task.Delay(w_timeout)) == receiveTask;
                    if (!isReceived) return false;
                    pComStatus = ComStatus.Connected;
                    return true;
                }
                else
                {
                    LOG("ERROR while sending on COM port! " + "The port is not open!", ERR);
                    pComStatus = ComStatus.Disconnected;
                    return false;
                }
            }
            catch (Exception ex)
            {
                LOG("Failed sending data on COM port " +
                    _serialPort.PortName + "! error message: " + ex.Message, ERR);
                pComStatus = ComStatus.Disconnected;
                return false;
            }
        }

        public async Task<byte[]> ReadSerial(int w_len, int w_timeout = 0)
        {
            // Sanity check
            if (w_len <= 0)
            {
                return null;
            }

            // Catch exceptions, show to user
            try
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.DiscardInBuffer();
                    pComStatus = ComStatus.Receiving;
                    byte[] data = new byte[w_len];
                    if (w_timeout <= 0) w_timeout = _serialTimeOut;
                    _serialPort.ReadTimeout = w_timeout;
                    var ReceiveCount = 0;
                    var receiveTask = Task.Run(async () => {
                        while (ReceiveCount < w_len)
                        {
                            ReceiveCount += await _serialPort.BaseStream.ReadAsync(data, ReceiveCount, w_len - ReceiveCount);
                            LOG("RECV: Currently received: " + ReceiveCount + "bytes");
                            await Task.Delay(20);
                            if (ReceiveCount < w_len)
                                LOG("RECV: Not satisfied, reading more ...");
                        }
                    });
                    var isReceived = await Task.WhenAny(receiveTask, Task.Delay(w_timeout)) == receiveTask;
                    LOG("RECV: Received total of: " + ReceiveCount + "bytes");
                    pComStatus = ComStatus.Connected;
                    if (!isReceived) return null;
                    return data;
                }
                else
                {
                    LOG("Failed reading on COM port! " +
                    "The port is not open!", ERR);
                    pComStatus = ComStatus.Disconnected;
                    return null;
                }
            }
            catch (Exception ex)
            {
                LOG("Failed reading on COM port " +
                    _serialPort.PortName + "! error message: " + ex.Message, ERR);
                pComStatus = ComStatus.Disconnected;
                return null;
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            LOG("DATA received! Currently " +
                    _serialPort.BytesToRead + " bytes of data in buffer.");
        }

        private void HandleSerialError(object sender, SerialErrorReceivedEventArgs e)
        {
            pComStatus = ComStatus.Disconnected;
            LOG("SERIAL FAILURE " + e.EventType.ToString() + "!", ERR);
        }

    }
}
