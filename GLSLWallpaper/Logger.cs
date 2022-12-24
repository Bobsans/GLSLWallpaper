using System;
using System.IO;
using System.Text;

namespace GLSLWallpaper; 

public static class Logger {
    static void Log(LogLevel level, string value) {
        using StreamWriter writer = new(Path.Join(Settings.ROOT, "latest.log"), true, Encoding.UTF8);
        writer.WriteLine($"{DateTime.Now:G} [{level.ToString(),-7}] {value}");
    }

    public static void Debug(string value) {
        Log(LogLevel.DEBUG, value);
    }

    public static void Info(string value) {
        Log(LogLevel.INFO, value);
    }

    public static void Warning(string value) {
        Log(LogLevel.WARNING, value);
    }

    public static void Error(string value) {
        Log(LogLevel.ERROR, value);
    }
}

enum LogLevel {
    DEBUG,
    INFO,
    WARNING,
    ERROR
}
