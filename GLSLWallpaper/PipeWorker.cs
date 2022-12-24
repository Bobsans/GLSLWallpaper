using System;
using System.IO;
using System.IO.Pipes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace GLSLWallpaper;

public static class PipeWorker {
    public static void RunServer() {
        new Thread(() => {
            while (true) {
                try {
                    using NamedPipeServerStream pipe = new(Identity.NAME, PipeDirection.In);
                    pipe.WaitForConnection();
                    Message message = Read(pipe);
                    if (message.Action == WALLPAPER_CHANGED) {
                        Settings.CurrentShader = message.Value;
                    }
                } catch (Exception e) {
                    Logger.Error(e.Message);
                }
            }
        }) { IsBackground = true }.Start();
    }

    public static bool Send(Message message) {
        try {
            using NamedPipeClientStream pipe = new(".", Identity.NAME, PipeDirection.Out);
            pipe.Connect(TimeSpan.FromMilliseconds(500));
            Write(pipe, message);
            return true;
        } catch (TimeoutException) {
            return false;
        }
    }

    static Message Read(Stream stream) {
        using BinaryReader reader = new(stream);
        return new Message {
            Action = reader.ReadByte(),
            Value = reader.ReadString()
        };
    }

    static void Write(Stream stream, Message message) {
        using BinaryWriter writer = new(stream);
        writer.Write(message.Action);
        writer.Write(message.Value);
        stream.Flush();
    }

    public class Message {
        public byte Action { get; set; }
        public string Value { get; set; } = null!;
    }

    public const byte WALLPAPER_CHANGED = 0x0;
}
