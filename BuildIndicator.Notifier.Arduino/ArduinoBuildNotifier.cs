using System;
using System.Globalization;
using System.IO.Ports;
using BuildIndicator.Core;

namespace BuildIndicator.Notifier.Arduino
{
    public class ArduinoBuildNotifier : IBuildNotifier
    {
        private readonly string _serialPortName;

        public ArduinoBuildNotifier(string serialPortName = "COM4")
        {
            _serialPortName = serialPortName;
        }

        public void Notify(BuildNotification notification)
        {            
            SendMessage(((int) notification.Status).ToString(CultureInfo.CurrentCulture));            
        }
        
        private void SendMessage(string message)
        {
            Console.WriteLine("Sending Message: {0}", message);
            try
            {
                using (var outputPort = new SerialPort(_serialPortName, 9600))
                {
                    outputPort.Open();
                    outputPort.Write(message);
                    outputPort.Close();
                }
            }
            catch
            {
            }
        }
    }
}