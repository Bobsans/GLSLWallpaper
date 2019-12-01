using System;
using System.IO;
using System.Text;

namespace GLSLWallpapers.Helpers {
    public static class Logger {
        public static string FilePath { get; set; }

        static void Log(LogLevel level, string value) {
            string direcory = Path.GetDirectoryName(FilePath);

            if (!Directory.Exists(direcory)) {
                Directory.CreateDirectory(direcory ?? throw new Exception("Invalid log file path."));
            }

            using (StreamWriter writer = new StreamWriter(FilePath, true, Encoding.UTF8)) {
                writer.WriteLine($"{DateTime.Now:G} [{level.ToString().PadRight(7)}] {value}");
            }
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
}