﻿using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using BCGSA;
using InTheHand.Net.Sockets;

namespace TestGameAPP
{
    class ReceiverBluetoothService : IDisposable
    {
        private readonly Guid _serviceClassId;
        private Action<AccelerometerEntity> _responseAction;
        private BluetoothListener _listener;
        private CancellationTokenSource _cancelSource;
        private static readonly BinaryFormatter _formatter = new BinaryFormatter();
        /// <summary>  
        /// Initializes a new instance of the <see cref="ReceiverBluetoothService" /> class.  
        /// </summary>  
        public ReceiverBluetoothService()
        {
            _serviceClassId = new Guid("4d89187e-476a-11e9-b210-d663bd873d93");
        }

        /// <summary>  
        /// Gets or sets a value indicating whether was started.  
        /// </summary>  
        /// <value>  
        /// The was started.  
        /// </value>  
        public bool WasStarted;

        /// <summary>  
        /// Starts the listening from Senders.  
        /// </summary>  
        /// <param name="reportAction">  
        /// The report Action.  
        /// </param>  
        public void Start(Action<AccelerometerEntity> reportAction)
        {
            WasStarted = true;
            _responseAction = reportAction;
            if (_cancelSource != null && _listener != null)
            {
                Dispose(true);
            }
            _listener = new BluetoothListener(_serviceClassId)
            {
                ServiceName = "MyService"
            };
            _listener.Start();

            _cancelSource = new CancellationTokenSource();

            Task.Run(() => Listener(_cancelSource));
        }

        /// <summary>  
        /// Stops the listening from Senders.  
        /// </summary>  
        public void Stop()
        {
            WasStarted = false;
            _cancelSource.Cancel();
        }

        /// <summary>  
        /// Listeners the accept bluetooth client.  
        /// </summary>  
        /// <param name="token">  
        /// The token.  
        /// </param>  
        private void Listener(CancellationTokenSource token)
        {
            using (var client = _listener.AcceptBluetoothClient())
            {
                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }
                    try
                    {
                        var content = (AccelerometerEntity)_formatter.Deserialize(client.GetStream());
                        client.GetStream().Flush();
                        _responseAction(content);
                    }
                    catch (IOException)
                    {
                        client.Close();
                        break;
                    }
                }
            }
        }

        /// <summary>  
        /// The dispose.  
        /// </summary>  
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>  
        /// The dispose.  
        /// </summary>  
        /// <param name="disposing">  
        /// The disposing.  
        /// </param>  
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_cancelSource != null)
                {
                    _listener.Stop();
                    _listener = null;
                    _cancelSource.Dispose();
                    _cancelSource = null;
                }
            }
        }
    }
}
