﻿using Xabbo;
using Xabbo.Core;
using Xabbo.Extension;
using Xabbo.Messages.Nitro;


namespace Xabbo.Components;

public class DisconnectionReasonComponent(IExtension extension) : Component(extension)
{
    //[InterceptIn(nameof(In.Hotel_Maintenance))]
    //protected void HandleDisconnectionReason(Intercept e)
    //{
    //    e.Block();

    //    DisconnectReason reason = (DisconnectReason)e.Packet.Read<int>();

    //    string reasonText = Enum.IsDefined(reason) ? reason.ToString() : $"unknown ({(int)reason})";
    //    string message = $"[xabbo] You were disconnected by the server.\n\nReason: {reasonText}";

    //    Ext.Send(In.Generic_Alert, message);
    //}
}
