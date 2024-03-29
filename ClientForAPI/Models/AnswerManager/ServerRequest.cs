﻿using System;

namespace ClientForAPI.Models.AnswerManager
{
    internal class ServerRequest : IServerMessage
    {
        #region Fields

        private string _message;

        public string Message
        { get { return _message; } }

        private int _id;

        public int Id
        { get { return _id; } }

        private string _command;

        public string Command
        { get { return _command; } }

        #endregion Fields

        #region Methods

        public ServerRequest(string command, string message)
        {
            _message = message;
            _command = command;
            _id = new Random().Next(100000000, 999999999);
        }

        public ServerRequest(string command)
        {
            _message = string.Empty;

            _command = command;
            _id = new Random().Next(100000000, 999999999);
        }

        public override string ToString()
        {
            return $"{Id}@{Command}{(!System.String.IsNullOrEmpty(_message) ? "@" : "")}{(!System.String.IsNullOrEmpty(_message) ? Message : "")}";
        }

        #endregion Methods
    }
}