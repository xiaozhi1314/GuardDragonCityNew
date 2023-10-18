/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  Log.cs
 * author:    taoye
 * created:   2020/12/16
 * descrip:   日志功能对外暴露
 * 级别说明:   DEBUG < INFO < WARNING < ERROR < FATAL
 ***************************************************************/

using System;
using System.Diagnostics;

namespace BigDream
{
    public static class Log
    {
        /// <summary>
        /// 日志等级
        /// </summary>
        public enum LogLevel : byte
        {
            // 调试
            Debug = 0,

            // 信息
            Info,

            // 警告
            Warning,

            // 错误
            Error,

            // 严重错误
            Fatal
        }

        /// <summary>
        /// 打印调试级别日志
        /// 用于记录调试类日志信息
        /// 仅在带有 DEBUG 预编译选项且带有 ENABLE_LOG、ENABLE_DEBUG_LOG 或 ENABLE_DEBUG_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="message">日志内容</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_DEBUG_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        public static void Debug(object message)
        {
            InternalLog(LogLevel.Debug, message);
        }

        /// <summary>
        /// 打印调试级别日志
        /// 用于记录调试类日志信息
        /// 仅在带有 DEBUG 预编译选项且带有 ENABLE_LOG、ENABLE_DEBUG_LOG 或 ENABLE_DEBUG_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="message">日志内容</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_DEBUG_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        public static void Debug(string message)
        {
            InternalLog(LogLevel.Debug, message);
        }

        /// <summary>
        /// 打印调试级别日志
        /// 用于记录调试类日志信息
        /// 仅在带有 DEBUG 预编译选项且带有 ENABLE_LOG、ENABLE_DEBUG_LOG 或 ENABLE_DEBUG_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="format">日志格式</param>
        /// <param name="arg0">日志参数0</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_DEBUG_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        public static void Debug(string format, object arg0)
        {
            InternalLog(LogLevel.Debug, Txt.Format(format, arg0));
        }

        /// <summary>
        /// 打印调试级别日志
        /// 用于记录调试类日志信息
        /// 仅在带有 DEBUG 预编译选项且带有 ENABLE_LOG、ENABLE_DEBUG_LOG 或 ENABLE_DEBUG_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="format">日志格式</param>
        /// <param name="arg0">日志参数0</param>
        /// <param name="arg1">日志参数1</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_DEBUG_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        public static void Debug(string format, object arg0, object arg1)
        {
            InternalLog(LogLevel.Debug, Txt.Format(format, arg0, arg1));
        }

        /// <summary>
        /// 打印调试级别日志
        /// 用于记录调试类日志信息
        /// 仅在带有 DEBUG 预编译选项且带有 ENABLE_LOG、ENABLE_DEBUG_LOG 或 ENABLE_DEBUG_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="format">日志格式</param>
        /// <param name="arg0">日志参数0</param>
        /// <param name="arg1">日志参数1</param>
        /// <param name="arg2">日志参数2</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_DEBUG_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        public static void Debug(string format, object arg0, object arg1, object arg2)
        {
            InternalLog(LogLevel.Debug, Txt.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// 打印调试级别日志
        /// 用于记录调试类日志信息
        /// 仅在带有 DEBUG 预编译选项且带有 ENABLE_LOG、ENABLE_DEBUG_LOG 或 ENABLE_DEBUG_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="format">日志格式</param>
        /// <param name="args">日志参数数组</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_DEBUG_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        public static void Debug(string format, params object[] args)
        {
            InternalLog(LogLevel.Debug, Txt.Format(format, args));
        }

        /// <summary>
        /// 打印信息级别日志
        /// 用于记录程序正常运行日志信息
        /// 仅在带有 ENABLE_LOG、ENABLE_INFO_LOG、ENABLE_DEBUG_AND_ABOVE_LOG 或 ENABLE_INFO_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="message">日志内容</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_INFO_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        public static void Info(object message)
        {
            InternalLog(LogLevel.Info, message);
        }

        /// <summary>
        /// 打印信息级别日志
        /// 用于记录程序正常运行日志信息
        /// 仅在带有 ENABLE_LOG、ENABLE_INFO_LOG、ENABLE_DEBUG_AND_ABOVE_LOG 或 ENABLE_INFO_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="message">日志内容</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_INFO_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        public static void Info(string message)
        {
            InternalLog(LogLevel.Info, message);
        }

        /// <summary>
        /// 打印信息级别日志
        /// 用于记录程序正常运行日志信息
        /// 仅在带有 ENABLE_LOG、ENABLE_INFO_LOG、ENABLE_DEBUG_AND_ABOVE_LOG 或 ENABLE_INFO_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="format">日志格式</param>
        /// <param name="arg0">日志参数0</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_INFO_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        public static void Info(string format, object arg0)
        {
            InternalLog(LogLevel.Info, Txt.Format(format, arg0));
        }

        /// <summary>
        /// 打印信息级别日志
        /// 用于记录程序正常运行日志信息
        /// 仅在带有 ENABLE_LOG、ENABLE_INFO_LOG、ENABLE_DEBUG_AND_ABOVE_LOG 或 ENABLE_INFO_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="format">日志格式</param>
        /// <param name="arg0">日志参数0</param>
        /// <param name="arg1">日志参数1</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_INFO_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        public static void Info(string format, object arg0, object arg1)
        {
            InternalLog(LogLevel.Info, Txt.Format(format, arg0, arg1));
        }

        /// <summary>
        /// 打印信息级别日志
        /// 用于记录程序正常运行日志信息
        /// 仅在带有 ENABLE_LOG、ENABLE_INFO_LOG、ENABLE_DEBUG_AND_ABOVE_LOG 或 ENABLE_INFO_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="format">日志格式</param>
        /// <param name="arg0">日志参数0</param>
        /// <param name="arg1">日志参数1</param>
        /// <param name="arg2">日志参数2</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_INFO_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        public static void Info(string format, object arg0, object arg1, object arg2)
        {
            InternalLog(LogLevel.Info, Txt.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// 打印信息级别日志
        /// 用于记录程序正常运行日志信息
        /// 仅在带有 ENABLE_LOG、ENABLE_INFO_LOG、ENABLE_DEBUG_AND_ABOVE_LOG 或 ENABLE_INFO_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="format">日志格式</param>
        /// <param name="args">日志参数数组</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_INFO_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        public static void Info(string format, params object[] args)
        {
            InternalLog(LogLevel.Info, Txt.Format(format, args));
        }

        /// <summary>
        /// 打印警告级别日志
        /// 建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用
        /// 仅在带有 ENABLE_LOG、ENABLE_INFO_LOG、ENABLE_DEBUG_AND_ABOVE_LOG、ENABLE_INFO_AND_ABOVE_LOG 或 ENABLE_WARNING_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="message">日志内容</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_WARNING_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        public static void Warning(object message)
        {
            InternalLog(LogLevel.Warning, message);
        }

        /// <summary>
        /// 打印警告级别日志
        /// 建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用
        /// 仅在带有 ENABLE_LOG、ENABLE_INFO_LOG、ENABLE_DEBUG_AND_ABOVE_LOG、ENABLE_INFO_AND_ABOVE_LOG 或 ENABLE_WARNING_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="message">日志内容</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_WARNING_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        public static void Warning(string message)
        {
            InternalLog(LogLevel.Warning, message);
        }

        /// <summary>
        /// 打印警告级别日志
        /// 建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用
        /// 仅在带有 ENABLE_LOG、ENABLE_INFO_LOG、ENABLE_DEBUG_AND_ABOVE_LOG、ENABLE_INFO_AND_ABOVE_LOG 或 ENABLE_WARNING_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="format">日志格式</param>
        /// <param name="arg0">日志参数0</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_WARNING_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        public static void Warning(string format, object arg0)
        {
            InternalLog(LogLevel.Warning, Txt.Format(format, arg0));
        }

        /// <summary>
        /// 打印警告级别日志
        /// 建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用
        /// 仅在带有 ENABLE_LOG、ENABLE_INFO_LOG、ENABLE_DEBUG_AND_ABOVE_LOG、ENABLE_INFO_AND_ABOVE_LOG 或 ENABLE_WARNING_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="format">日志格式</param>
        /// <param name="arg0">日志参数0</param>
        /// <param name="arg1">日志参数1</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_WARNING_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        public static void Warning(string format, object arg0, object arg1)
        {
            InternalLog(LogLevel.Warning, Txt.Format(format, arg0, arg1));
        }

        /// <summary>
        /// 打印警告级别日志
        /// 建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用
        /// 仅在带有 ENABLE_LOG、ENABLE_INFO_LOG、ENABLE_DEBUG_AND_ABOVE_LOG、ENABLE_INFO_AND_ABOVE_LOG 或 ENABLE_WARNING_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="format">日志格式</param>
        /// <param name="arg0">日志参数0</param>
        /// <param name="arg1">日志参数1</param>
        /// <param name="arg2">日志参数2</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_WARNING_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        public static void Warning(string format, object arg0, object arg1, object arg2)
        {
            InternalLog(LogLevel.Warning, Txt.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// 打印警告级别日志
        /// 建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用
        /// 仅在带有 ENABLE_LOG、ENABLE_INFO_LOG、ENABLE_DEBUG_AND_ABOVE_LOG、ENABLE_INFO_AND_ABOVE_LOG 或 ENABLE_WARNING_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="format">日志格式</param>
        /// <param name="args">日志参数数组</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_WARNING_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        public static void Warning(string format, params object[] args)
        {
            InternalLog(LogLevel.Warning, Txt.Format(format, args));
        }

        /// <summary>
        /// 打印错误级别日志
        /// 建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用
        /// 仅在带有 ENABLE_LOG、ENABLE_INFO_LOG、ENABLE_DEBUG_AND_ABOVE_LOG、ENABLE_INFO_AND_ABOVE_LOG、ENABLE_WARNING_AND_ABOVE_LOG 或 ENABLE_ERROR_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="message">日志内容</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_ERROR_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        [Conditional("ENABLE_ERROR_AND_ABOVE_LOG")]
        public static void Error(object message)
        {
            InternalLog(LogLevel.Error, message);
        }

        /// <summary>
        /// 打印错误级别日志
        /// 建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用
        /// 仅在带有 ENABLE_LOG、ENABLE_INFO_LOG、ENABLE_DEBUG_AND_ABOVE_LOG、ENABLE_INFO_AND_ABOVE_LOG、ENABLE_WARNING_AND_ABOVE_LOG 或 ENABLE_ERROR_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="message">日志内容</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_ERROR_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        [Conditional("ENABLE_ERROR_AND_ABOVE_LOG")]
        public static void Error(string message)
        {
            InternalLog(LogLevel.Error, message);
        }

        /// <summary>
        /// 打印错误级别日志
        /// 建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用
        /// 仅在带有 ENABLE_LOG、ENABLE_INFO_LOG、ENABLE_DEBUG_AND_ABOVE_LOG、ENABLE_INFO_AND_ABOVE_LOG、ENABLE_WARNING_AND_ABOVE_LOG 或 ENABLE_ERROR_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="format">日志格式</param>
        /// <param name="arg0">日志参数0</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_ERROR_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        [Conditional("ENABLE_ERROR_AND_ABOVE_LOG")]
        public static void Error(string format, object arg0)
        {
            InternalLog(LogLevel.Error, Txt.Format(format, arg0));
        }

        /// <summary>
        /// 打印错误级别日志
        /// 建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用
        /// 仅在带有 ENABLE_LOG、ENABLE_INFO_LOG、ENABLE_DEBUG_AND_ABOVE_LOG、ENABLE_INFO_AND_ABOVE_LOG、ENABLE_WARNING_AND_ABOVE_LOG 或 ENABLE_ERROR_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="format">日志格式</param>
        /// <param name="arg0">日志参数0</param>
        /// <param name="arg1">日志参数1</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_ERROR_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        [Conditional("ENABLE_ERROR_AND_ABOVE_LOG")]
        public static void Error(string format, object arg0, object arg1)
        {
            InternalLog(LogLevel.Error, Txt.Format(format, arg0, arg1));
        }

        /// <summary>
        /// 打印错误级别日志
        /// 建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用
        /// 仅在带有 ENABLE_LOG、ENABLE_INFO_LOG、ENABLE_DEBUG_AND_ABOVE_LOG、ENABLE_INFO_AND_ABOVE_LOG、ENABLE_WARNING_AND_ABOVE_LOG 或 ENABLE_ERROR_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="format">日志格式</param>
        /// <param name="arg0">日志参数0</param>
        /// <param name="arg1">日志参数1</param>
        /// <param name="arg2">日志参数2</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_ERROR_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        [Conditional("ENABLE_ERROR_AND_ABOVE_LOG")]
        public static void Error(string format, object arg0, object arg1, object arg2)
        {
            InternalLog(LogLevel.Error, Txt.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// 打印错误级别日志
        /// 建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用
        /// 仅在带有 ENABLE_LOG、ENABLE_INFO_LOG、ENABLE_DEBUG_AND_ABOVE_LOG、ENABLE_INFO_AND_ABOVE_LOG、ENABLE_WARNING_AND_ABOVE_LOG 或 ENABLE_ERROR_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="format">日志格式</param>
        /// <param name="args">日志参数数组</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_ERROR_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        [Conditional("ENABLE_ERROR_AND_ABOVE_LOG")]
        public static void Error(string format, params object[] args)
        {
            InternalLog(LogLevel.Error, Txt.Format(format, args));
        }

        /// <summary>
        /// 打印严重错误级别日志
        /// 建议在发生严重错误，可能导致游戏崩溃或异常时使用，此时应尝试重启进程或重建游戏框架
        /// 仅在带有 ENABLE_LOG、ENABLE_INFO_LOG、ENABLE_DEBUG_AND_ABOVE_LOG、ENABLE_INFO_AND_ABOVE_LOG、ENABLE_WARNING_AND_ABOVE_LOG、ENABLE_ERROR_AND_ABOVE_LOG 或 ENABLE_FATAL_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="message">日志内容</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_FATAL_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        [Conditional("ENABLE_ERROR_AND_ABOVE_LOG")]
        [Conditional("ENABLE_FATAL_AND_ABOVE_LOG")]
        public static void Fatal(object message)
        {
            InternalLog(LogLevel.Fatal, message);
        }

        /// <summary>
        /// 打印严重错误级别日志
        /// 建议在发生严重错误，可能导致游戏崩溃或异常时使用，此时应尝试重启进程或重建游戏框架
        /// 仅在带有 ENABLE_LOG、ENABLE_INFO_LOG、ENABLE_DEBUG_AND_ABOVE_LOG、ENABLE_INFO_AND_ABOVE_LOG、ENABLE_WARNING_AND_ABOVE_LOG、ENABLE_ERROR_AND_ABOVE_LOG 或 ENABLE_FATAL_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="message">日志内容</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_FATAL_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        [Conditional("ENABLE_ERROR_AND_ABOVE_LOG")]
        [Conditional("ENABLE_FATAL_AND_ABOVE_LOG")]
        public static void Fatal(string message)
        {
            InternalLog(LogLevel.Fatal, message);
        }

        /// <summary>
        /// 打印严重错误级别日志
        /// 建议在发生严重错误，可能导致游戏崩溃或异常时使用，此时应尝试重启进程或重建游戏框架
        /// 仅在带有 ENABLE_LOG、ENABLE_INFO_LOG、ENABLE_DEBUG_AND_ABOVE_LOG、ENABLE_INFO_AND_ABOVE_LOG、ENABLE_WARNING_AND_ABOVE_LOG、ENABLE_ERROR_AND_ABOVE_LOG 或 ENABLE_FATAL_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="format">日志格式</param>
        /// <param name="arg0">日志参数0</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_FATAL_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        [Conditional("ENABLE_ERROR_AND_ABOVE_LOG")]
        [Conditional("ENABLE_FATAL_AND_ABOVE_LOG")]
        public static void Fatal(string format, object arg0)
        {
            InternalLog(LogLevel.Fatal, Txt.Format(format, arg0));
        }

        /// <summary>
        /// 打印严重错误级别日志
        /// 建议在发生严重错误，可能导致游戏崩溃或异常时使用，此时应尝试重启进程或重建游戏框架
        /// 仅在带有 ENABLE_LOG、ENABLE_INFO_LOG、ENABLE_DEBUG_AND_ABOVE_LOG、ENABLE_INFO_AND_ABOVE_LOG、ENABLE_WARNING_AND_ABOVE_LOG、ENABLE_ERROR_AND_ABOVE_LOG 或 ENABLE_FATAL_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="format">日志格式</param>
        /// <param name="arg0">日志参数0</param>
        /// <param name="arg1">日志参数1</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_FATAL_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        [Conditional("ENABLE_ERROR_AND_ABOVE_LOG")]
        [Conditional("ENABLE_FATAL_AND_ABOVE_LOG")]
        public static void Fatal(string format, object arg0, object arg1)
        {
            InternalLog(LogLevel.Fatal, Txt.Format(format, arg0, arg1));
        }

        /// <summary>
        /// 打印严重错误级别日志
        /// 建议在发生严重错误，可能导致游戏崩溃或异常时使用，此时应尝试重启进程或重建游戏框架
        /// 仅在带有 ENABLE_LOG、ENABLE_INFO_LOG、ENABLE_DEBUG_AND_ABOVE_LOG、ENABLE_INFO_AND_ABOVE_LOG、ENABLE_WARNING_AND_ABOVE_LOG、ENABLE_ERROR_AND_ABOVE_LOG 或 ENABLE_FATAL_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="format">日志格式</param>
        /// <param name="arg0">日志参数0</param>
        /// <param name="arg1">日志参数1</param>
        /// <param name="arg2">日志参数2</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_FATAL_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        [Conditional("ENABLE_ERROR_AND_ABOVE_LOG")]
        [Conditional("ENABLE_FATAL_AND_ABOVE_LOG")]
        public static void Fatal(string format, object arg0, object arg1, object arg2)
        {
            InternalLog(LogLevel.Fatal, Txt.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// 打印严重错误级别日志
        /// 建议在发生严重错误，可能导致游戏崩溃或异常时使用，此时应尝试重启进程或重建游戏框架
        /// 仅在带有 ENABLE_LOG、ENABLE_INFO_LOG、ENABLE_DEBUG_AND_ABOVE_LOG、ENABLE_INFO_AND_ABOVE_LOG、ENABLE_WARNING_AND_ABOVE_LOG、ENABLE_ERROR_AND_ABOVE_LOG 或 ENABLE_FATAL_AND_ABOVE_LOG 预编译选项时生效
        /// </summary>
        /// <param name="format">日志格式</param>
        /// <param name="args">日志参数数组</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_FATAL_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        [Conditional("ENABLE_ERROR_AND_ABOVE_LOG")]
        [Conditional("ENABLE_FATAL_AND_ABOVE_LOG")]
        public static void Fatal(string format, params object[] args)
        {
            InternalLog(LogLevel.Fatal, Txt.Format(format, args));
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="level">日志等级</param>
        /// <param name="message">日志内容</param>
        private static void InternalLog(LogLevel level, object message)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    UnityEngine.Debug.Log(Txt.Format("<color=#888888>{0}</color>", message.ToString()));  // 灰色
                    break;

                case LogLevel.Info:
                    UnityEngine.Debug.Log(message.ToString());
                    break;

                case LogLevel.Warning:
                    UnityEngine.Debug.LogWarning(message.ToString());
                    break;

                case LogLevel.Error:
                    UnityEngine.Debug.LogError(message.ToString());
                    break;

                default:
                    throw new Exception(message.ToString());
            }
        }

    }
}


