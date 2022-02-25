﻿using System;

public enum ActionType
{
    Unknown,
    SendText,
    ShowTrayIcon,
    HideTrayIcon
}

namespace llcats.Common
{
    [Serializable]
    public class PipeMessage
    {
        public PipeMessage()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public ActionType Action { get; set; }
        public string Text { get; set; }
    }
}