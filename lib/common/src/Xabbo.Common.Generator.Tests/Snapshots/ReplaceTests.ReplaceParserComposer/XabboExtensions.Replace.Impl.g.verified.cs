﻿//HintName: XabboExtensions.Replace.Impl.g.cs
internal static partial class XabboExtensions
{
    private static partial void Replace<T>(in global::Xabbo.Messages.PacketWriter w, T value)
    {
        switch (value)
        {
            case bool v: w.ReplaceBool(v); break;
            case byte v: w.ReplaceByte(v); break;
            case short v: w.ReplaceShort(v); break;
            case int v: w.ReplaceInt(v); break;
            case float v: w.ReplaceFloat(v); break;
            case int v: w.Replaceint(v); break;
            case string v: w.ReplaceString(v); break;
            case global::Xabbo.Length v: w.ReplaceLength(v); break;
            case global::Xabbo.Id v: w.Replaceint(v); break;
            case global::Xabbo.Messages.B64 v: w.ReplaceB64(v); break;
            case global::Xabbo.Messages.VL64 v: w.ReplaceVL64(v); break;
            /* Generated 1 case */
            case global::ParserComposer v: w.ReplaceStruct<global::ParserComposer>(v); break;
            default: throw new global::System.NotSupportedException($"Cannot replace value of type '{typeof(T)}'.");
        }
    }
}
