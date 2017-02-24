using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

namespace ComPortManager
{
    public class ComPortMonitor
    {
        public List<string> CurrentPortList { get; private set; }
        private string _message;
        private List<string> _newPortList;
        private List<string> _addedPorts;
        private List<string> _removedPorts;

        public event EventHandler<string> Change;


        public ComPortMonitor()
        {
            UpdateNewPortList();
            CurrentPortList = _newPortList;
        }

        public void Update()
        {
            UpdateNewPortList();
            UpdateAddedList();
            UpdateRemovedList();

            if (IsThereAnyChange())
            {
                CurrentPortList = _newPortList;
                OnChange();
            }
        }

       
        private bool IsThereAnyChange()
        {
            return _addedPorts.Count != 0 || _removedPorts.Count != 0;
        }

        private void ConstructChangedMessage()
        {
            _message = string.Empty;
            _message = _addedPorts.Aggregate(_message, (current, added) => current + "Added: " + added);
            _message = _removedPorts.Aggregate(_message, (current, removed) => current + "Removed: " + removed);
        }

        private void UpdateAddedList()
        {
            _addedPorts = _newPortList.Where(port => !CurrentPortList.Contains(port)).ToList();
        }

        private void UpdateRemovedList()
        {
            _removedPorts = CurrentPortList.Where(port => !_newPortList.Contains(port)).ToList();
        }

        private void UpdateNewPortList()
        {
            var ports = SystemManagement.GetFriendlyComPortNames();
            _newPortList = SerialPort.GetPortNames().Select(n => n + " - " + ports.FirstOrDefault(s => s.Contains(n))).ToList();
        }

        protected virtual void OnChange()
        {
            ConstructChangedMessage();
            Change?.Invoke(this, _message);
        }
    }
}