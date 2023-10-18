using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ExcelDataReader.Log;
using UnityEditor;
using UnityEngine;

namespace BigDream
{
    public static class TableExportEditorUtility
    {
        /// <summary>
        /// 打开指定的文件夹
        /// </summary>
        /// <param name="directoryPath">需要打开的文件夹绝对路径</param>
        public static void OpenDirectory(string directoryPath)
        {
            if (System.IO.Directory.Exists(directoryPath))
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
                        Process.Start("Explorer.exe", directoryPath.Replace('/', '\\'));
                        break;

                    case RuntimePlatform.OSXEditor:
                        Process.Start("open", directoryPath);
                        break;

                    default:
                        throw new Exception(Txt.Format("Not support open folder on '{0}' platform.", Application.platform.ToString()));
                }
            }
            else
            {
                Log.Info("文件夹 {0} 不存在，无法打开。", directoryPath);
            }
        }

        /// <summary>
        /// 删除指定文件夹
        /// </summary>
        /// <param name="directoryPath">需要删除的文件夹绝对路径</param>
        public static void DeleteDirectory(string directoryPath)
        {
            if (System.IO.Directory.Exists(directoryPath))
            {
                System.IO.Directory.Delete(directoryPath, true);
                Log.Info("文件夹 {0} 删除完毕。", directoryPath);
            }
            else
            {
                Log.Info("文件夹 {0} 不存在，无需删除。", directoryPath);
            }
        }

        /// <summary>
        /// 打开指定的excel表格
        /// </summary>
        /// <param name="excelPath">要打开的Excel文件绝对路径</param>
        /// <exception cref="SolarException"></exception>
        public static void OpenExcel(string excelPath)
        {
            string strExtense = System.IO.Path.GetExtension(excelPath);
            if (strExtense == ".xlsm")
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
                        Process.Start(excelPath);
                        break;
                    case RuntimePlatform.OSXEditor:
                        Process.Start("open", excelPath);
                        break;
                    default:
                        throw new Exception(Txt.Format("Not support open file on '{0}' platform.", Application.platform.ToString()));
                }
            }
            else
            {
                Log.Error("指定的文件不是 xlsm文件");
            }
        }

        /// <summary>
        /// 判断输入路径是否位于lua文件夹下
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static bool IsDirectoryInLuaDirectory(string checkPath)
        {
            if (string.IsNullOrEmpty(checkPath))
            {
                return false;
            }
            checkPath = checkPath.Replace("\\", "/");
            if (checkPath.Contains("Assets/Framework/LuaScripts/"))
            {
                return true;
            }
            if (checkPath.Contains("Assets/Game/LuaScripts/"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取Excel表格内容，目前支持.xlsm和.csv文件
        /// </summary>
        /// <param name="ExcleAbsolutePath">Excel文件的绝对路径</param>
        /// <param name="CsvFile">是否是.csv文件</param>
        /// <returns></returns>
        public static DataSet GetExcelData(string ExcleAbsolutePath)
        {
            if (string.IsNullOrEmpty(ExcleAbsolutePath))
            {
                Log.Error("Excel, 打开路径为null");
                return null;    
            }

            string strExtension = System.IO.Path.GetExtension(ExcleAbsolutePath);
            bool csvFile = false;
            if (strExtension == ".xlsm")
            {
                csvFile = false;
            }
            else if (strExtension == ".csv")
            {
                csvFile = true;
            }
            else
            {
                Log.Error("Excel, 需要打开的文件类型不是xlsm或csv");
                return null;
            }

            FileStream stream = null;
            try
            {
                stream = File.Open(ExcleAbsolutePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
            catch (Exception)
            {
                Log.Error("表格" + ExcleAbsolutePath + " 打开失败，请检查表格是否存在");
                throw;
            }

            try
            {
                IExcelDataReader excelReader = null;
                if (csvFile)
                {
                    ExcelReaderConfiguration csvConfig = new ExcelReaderConfiguration();
                    csvConfig.FallbackEncoding = Encoding.GetEncoding("GB2312");
                    excelReader = ExcelReaderFactory.CreateCsvReader(stream, csvConfig);
                }
                else
                {
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                // 转换数据
                DataSet excelData = (excelReader != null) ? excelReader.AsDataSet() : null;
                stream.Close();
                excelReader.Close();
                return excelData;
            }
            catch (Exception)
            {
                Log.Error("Excel {0}存在无法解析的数据。", ExcleAbsolutePath);
                throw;
            }
        }

        /// <summary>
        /// 获取表格名称，文件名去掉Tables前缀，去掉文件标识符
        /// </summary>
        /// <param name="excelFileName">Excel文件名</param>
        /// <returns></returns>
        public static string GetExcelName(string excelFileName)
        {
            if (string.IsNullOrEmpty(excelFileName))
            {
                Log.Error("Excel 表格名称为null");
                return null;
            }
            // 去掉后缀
            string tableName = System.IO.Path.GetFileNameWithoutExtension(excelFileName);
            // 去掉Table
            if (tableName.StartsWith("Table"))
            {
                tableName = tableName.Substring(5);
            }
            return tableName;
        }

        /// <summary>
        /// 判断表格导出数据是否需要加密
        /// </summary>
        /// <param name="excelDescrip">表格中B1字段</param>
        /// <returns></returns>
        public static bool IsTableNeedEncrypt(string excelDescrip)
        {
            return string.IsNullOrEmpty(excelDescrip) ? false : excelDescrip.Contains("[encrypt]");
        }

        /// <summary>
        /// 获取lua表格定义
        /// </summary>
        /// <param name="excelFileName"></param>
        /// <param name="needChangeName"></param>
        /// <returns></returns>
        public static string GetLuaTableDefine(string excelFileName, bool needChangeName = false)
        {
            string tableName = needChangeName ? GetExcelName(excelFileName) : excelFileName;
            return $"Tables.{tableName}";
        }

        /// <summary>
        /// 获取Lua表格中每项类型定义
        /// </summary>
        /// <param name="excelFileName"></param>
        /// <param name="needChangeName"></param>
        /// <returns></returns>
        public static string GetLuaTableItemDefine(string excelFileName, bool needChangeName = false)
        { 
            string tableName = needChangeName ? GetExcelName(excelFileName) : excelFileName;
            return $"Tables.{tableName}_Item";
        }

        /// <summary>
        /// 获取Excel转换成的lua文件中详情信息
        /// </summary>
        /// <param name="excelFileName"></param>
        /// <param name="excelDescrip"></param>
        /// <returns></returns>
        public static string GetExcelToLuaDetailInfo(string excelFileName, string excelDescrip)
        {
            string writeTableName = GetExcelName(excelFileName);
            if (string.IsNullOrEmpty(excelFileName))
            {
                Log.Error("Excel 表格名称为null");
                return null;
            }
            if (excelDescrip == null)
            {
                excelDescrip = "";
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("--=====================================================================================================")
                         .AppendLine("-- (c) copyright 2021 - 2025, Solar.Game")
                         .AppendLine("-- All Rights Reserved.")
                         .AppendLine("-- ----------------------------------------------------------------------------------------------------")
                         .AppendLine(Txt.Format($"-- filename:  Table{writeTableName}"))
                         .AppendLine(Txt.Format($"-- descrip:   {excelDescrip}"))
                         .AppendLine("-- notices:   该文件自动生成，请不要手动修改！")
                         .AppendLine("--=====================================================================================================")
                         .AppendLine("")
                         .AppendLine($"---@class Tables.{writeTableName}_Item @{excelDescrip}条目");
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 添加加密表格中的元表功能
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tableItem"></param>
        /// <returns></returns>
        public static string GetEncryptTableMetaFunc(string tableName, string tableItem)
        {
            string TableEncryptName = $"table{tableName}Encrypt";
            string TableOriginalName = $"Tables.{tableName}";

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine($"---@type fun(inTable: table, key: any):{tableItem} @返回解密后的数据");
            stringBuilder.AppendLine("---@param luaTable table @加密后的表格");
            stringBuilder.AppendLine("---@param key any @需要解密的表格字段的关键字");
            stringBuilder.AppendLine($"---@return {tableItem} @返回解密后的数据");
            stringBuilder.AppendLine("local pairfunc = function(inTable, key)");
            stringBuilder.AppendLine($"    local nk, nv = next({TableEncryptName}, key)");
            stringBuilder.AppendLine("    if nk then");
            stringBuilder.AppendLine($"        nv = {TableOriginalName}[nk]");
            stringBuilder.AppendLine("    end");
            stringBuilder.AppendLine("    return nk, nv");
            stringBuilder.AppendLine("end");

            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("local mt = { }");

            stringBuilder.AppendLine("");
            stringBuilder.AppendLine($"---@type fun(inTable: table, key: any):{tableItem} @返回解密后的数据");
            stringBuilder.AppendLine("---@param luaTable table @加密后的表格");
            stringBuilder.AppendLine("---@param key any @需要解密的表格字段的关键字");
            stringBuilder.AppendLine($"---@return {tableItem} @返回解密后的数据");
            stringBuilder.AppendLine("mt.__index = function(inTable, key)");
            stringBuilder.AppendLine($"    if {TableEncryptName}[key] ~= nil then");
            stringBuilder.AppendLine($"        return AESDecodeFromBase64({TableEncryptName}[key], \"table\")");
            stringBuilder.AppendLine("    end");
            stringBuilder.AppendLine("end");

            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("---@type fun():number @返回关联表格真实长度");
            stringBuilder.AppendLine("---@return number @返回关联表格真实长度");
            stringBuilder.AppendLine("mt.__len = function()");
            stringBuilder.AppendLine($"    return #{TableEncryptName}");
            stringBuilder.AppendLine("end");
      
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine($"---@type fun(inTable: table, key: any):{tableItem} @重写，返回关联表格解密后的数据");
            stringBuilder.AppendLine("---@param inTable table @加密表格");
            stringBuilder.AppendLine("---@param key any @需要解密的表格字段的关键字");
            stringBuilder.AppendLine($"---@return {tableItem} @返回关联表格解密后的数据");
            stringBuilder.AppendLine("mt.__pairs = function(inTable, key)");
            stringBuilder.AppendLine("    return pairfunc, inTable, nil");
            stringBuilder.AppendLine("end");

            stringBuilder.AppendLine("");
            stringBuilder.AppendLine($"setmetatable({TableOriginalName}, mt)");
            stringBuilder.AppendLine("");
            //stringBuilder.AppendLine($"Tables.{tableName} = table{tableName}");

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 将表格中的字段转为lua类型
        /// </summary>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        public static string GetTypeFromExcel(string typeStr)
        {
            switch (typeStr)
            {
                case "color32":
                    return "UnityEngine.Color32";
                case "color":
                    return "UnityEngine.Color";
                case "datetime":
                    return "System.DateTime";
                case "quaternion":
                    return "UnityEngine.Quaternion";
                case "rect":
                    return "UnityEngine.Rect";
                case "vector2":
                    return "UnityEngine.Vector2";
                case "vector3":
                    return "UnityEngine.Vector3";
                case "vector4":
                    return "UnityEngine.Vector4";
                case "int":
                case "float":
                    return "number";
                default:
                    break;
            }

            return typeStr;
        }

        /// <summary>
        /// 删除注释文字中的换行符
        /// </summary>
        /// <param name="checkStr"></param>
        /// <returns></returns>
        public static string DelNewLineFlag(string checkStr)
        {
            if (!string.IsNullOrEmpty(checkStr))
            {
                if (checkStr.IndexOf("\n") >= 0)
                {
                    return checkStr.Replace("\n", " ");
                }
                else if (checkStr.IndexOf("\n\r") >= 0)
                {
                    return checkStr.Replace("\n\r", " ");
                }
                else if (checkStr.IndexOf("\r\n") >= 0)
                {
                    return checkStr.Replace("\r\n", " ");
                }
                return checkStr;
            }
            return " ";
        }

        /// <summary>
        /// Table中定义的数据类型和lua数据转换格式
        /// </summary>
        /// <param name="initialStr"> 带转换的字符串</param>
        /// <param name="typeStr">带转换的字符串在table中的类型</param>
        /// <returns></returns>
        public static string GetLuaTypeFromExcel(string initialStr, string typeStr)
        {
            if (string.IsNullOrEmpty(initialStr))
            {
                return "";
            }
            string changedStr = initialStr;
            switch (typeStr)
            {
                case "number":
                    break;
                case "boolean":
                    break;
                case "table":
                    break;
                case "string":
                    changedStr = "\"" + initialStr + "\"";
                    break;
                case "color32":
                    changedStr = "CS.UnityEngine.Color32(" + initialStr + ")";
                    break;
                case "color":
                    changedStr = "CS.UnityEngine.Color(" + initialStr + ")";
                    break;
                case "datetime":
                    changedStr = "CS.System.DateTime.Parse(\"" + initialStr + "\")";
                    break;
                case "quaternion":
                    changedStr = "CS.UnityEngine.Quaternion(" + initialStr + ")";
                    break;
                case "rect":
                    changedStr = "CS.UnityEngine.Rect(" + initialStr + ")";
                    break;
                case "vector2":
                    changedStr = "CS.UnityEngine.Vector2(" + initialStr + ")";
                    break;
                case "vector3":
                    changedStr = "CS.UnityEngine.Vector3(" + initialStr + ")";
                    break;
                case "vector4":
                    changedStr = "CS.UnityEngine.Vector4(" + initialStr + ")";
                    break;
                default:
                    break;
            }
            return changedStr;

        }

        public static bool CheckLuaTypeInExcel(string defType, string checkString)
        {
            if (string.IsNullOrEmpty(checkString))
            {
                return true;
            }
            switch (defType)
            {
                case "number":
                    {
                        return CheckStringIsNumber(checkString);
                    }
                case "boolean":
                    {
                        if (checkString == "false" || checkString == "true")
                        {
                            return true;
                        }
                        return false;
                    }
                case "table":
                    {
                        if (checkString.StartsWith("{") && checkString.EndsWith("}"))
                        {
                            return true;
                        }
                        return false;
                    }
                case "string":
                    {
                        return true;
                    }
                case "color32":
                    {
                        // 用,分割的4个int数字
                        string[] ints = checkString.Split(',');
                        if (ints.Length != 4)
                        {
                            return false;
                        }
                        Regex regex = new Regex("^[0-9]*[1-9][0-9]*$");
                        for (int i = 0; i < ints.Length; i++)
                        {
                            if (!regex.IsMatch(ints[i].Trim()))
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                case "color":
                    {
                        // 用,分割的4个float数字 [0,1]
                        string[] floats = checkString.Split(',');
                        if (floats.Length != 4)
                        {
                            return false;
                        }
                        Regex regex = new Regex("^(?:0|1|0.[0-9]+)$");
                        for (int i = 0; i < floats.Length; i++)
                        {
                            if (!regex.IsMatch(floats[i].Trim()))
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                case "quaternion":
                    {
                        // 用,分割的4个float数字
                        string[] floats = checkString.Split(',');
                        if (floats.Length != 4)
                        {
                            return false;
                        }
                        //Regex regex = new Regex("^(-?\\d+)(\\.\\d+)?$");
                        for (int i = 0; i < floats.Length; i++)
                        {
                            if (!CheckStringIsNumber(floats[i].Trim()))
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                case "datetime":
                    {
                        DateTime convertTime;
                        bool tt = DateTime.TryParse(checkString, out convertTime);
                        return DateTime.TryParse(checkString, out convertTime);
                    }
                case "rect":
                    {
                        // 用,分割的4个数字
                        string[] floats = checkString.Split(',');
                        if (floats.Length != 4)
                        {
                            return false;
                        }
                        //Regex regex = new Regex("^(-?\\d+)(\\.\\d+)?$");
                        for (int i = 0; i < floats.Length; i++)
                        {
                            if (!CheckStringIsNumber(floats[i].Trim()))
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                case "vector2":
                    {
                        return CheckStringIsVector(checkString, 2);
                    }
                case "vector3":
                    {
                        return CheckStringIsVector(checkString, 3);
                    }
                case "vector4":
                    {
                        return CheckStringIsVector(checkString, 4);
                    }
            }
            return true;

        }

        /// <summary>
        /// 检查字符串是否是numbe
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool CheckStringIsNumber(string strNumber)
        {
            Regex objNotNumberPattern = new Regex("[^0-9.-]");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
            String strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            String strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
            Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");

            return !objNotNumberPattern.IsMatch(strNumber) &&
                !objTwoDotPattern.IsMatch(strNumber) &&
                !objTwoMinusPattern.IsMatch(strNumber) &&
                objNumberPattern.IsMatch(strNumber);
        }

        /// <summary>
        /// 判断输入的字符串是否可以转为vector
        /// </summary>
        /// <param name="vectorString"></param>
        /// <param name="vectorPointCount"></param>
        /// <returns></returns>
        public static bool CheckStringIsVector(string vectorString, int vectorPointCount)
        {
            string[] floats = vectorString.Split(',');
            if (floats.Length != vectorPointCount)
            {
                return false;
            }
            for (int i = 0; i < vectorPointCount; i++)
            {
                if (!CheckStringIsNumber(floats[i].Trim()))
                {
                    return false;
                }
            }
            return true;
        }
        
        
        /// <summary>
        /// 导出表格到Json文件
        /// </summary>
        /// <param name="exclePath">需要导出数据的excel表格绝对路径</param>
        /// <param name="jsonPath">要导出的json文件的绝对路径</param>
        /// <returns></returns>
        public static bool ExportExcelToJson(string exclePath, string jsonPath, byte[] EncrytionKey = null)
        {
            if (string.IsNullOrEmpty(jsonPath))
            {
                Log.Error("Excel, 导出路径为空，导出json文件失败");
                return false;
            }
            bool exportToEncryteJson = (EncrytionKey != null) ? true : false;
            string jsonExtension = System.IO.Path.GetExtension(jsonPath);
            if (exportToEncryteJson)
            {
                if (jsonExtension != ".bytes")
                {
                    Log.Error("Excel, 导出加密json文件需要扩展名为.bytes");
                    return false;
                }
            }
            else
            {
                if (jsonExtension != ".json")
                {
                    Log.Error("Excel, 导出json文件需要扩展名为.json");
                    return false;
                }
            }

            // 要打开的excel根路径
            DataSet result = GetExcelData(exclePath);

            // 开始添加文件头
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("{");

            int columns = result.Tables[0].Columns.Count;
            int rows = result.Tables[0].Rows.Count;
            string[] cellKey = new string[columns];
            string[] typeList = new string[columns];
            for (int keyi = 0; keyi < columns; keyi++)
            {
                cellKey[keyi] = result.Tables[0].Rows[1][keyi].ToString();
                typeList[keyi] = result.Tables[0].Rows[2][keyi].ToString();
                if (cellKey[keyi] != string.Empty & typeList[keyi] == string.Empty)
                {
                    Log.Error(exclePath + "表格错误, " + cellKey[keyi] + " 没有类型");
                    return false;
                }
            }
            for (int i = 4; i < rows; i++)
            {
                string Key = result.Tables[0].Rows[i][1].ToString();
                string thisRow = "    \"" + Key + "\" : {";
                for (int j = 1; j < columns; j++)
                {
                    if (cellKey[j] != string.Empty)
                    {
                        string cellValue = result.Tables[0].Rows[i][j].ToString();
                        if (typeList[j] == "number")
                        {
                            thisRow = thisRow + "\"" + cellKey[j] + "\":" + cellValue;
                        }
                        else if (typeList[j] == "string" || typeList[j] == "table")
                        {
                            thisRow = thisRow + "\"" + cellKey[j] + "\":" + "\"" + cellValue + "\"";
                        }
                        else if (typeList[j] == "boolean")
                        {
                            thisRow = thisRow + "\"" + cellKey[j] + "\":" + cellValue;
                        }
                        if (j < columns - 1)
                        {
                            thisRow = thisRow + ",";
                        }
                    }
                }
                thisRow = thisRow + "}";
                if (i < rows - 1)
                {
                    thisRow = thisRow + ",";
                }
                stringBuilder.AppendLine(thisRow);
            }
            stringBuilder.AppendLine("}");
            // 开始写入数据
            if (File.Exists(jsonPath))
            {
                File.Delete(jsonPath);
            }
            if (exportToEncryteJson)
            {
                File.WriteAllBytes(jsonPath, Encryption.GetQuickXorBytes(Converter.GetBytesByString(stringBuilder.ToString()), EncrytionKey));
            }
            else
            {
                File.WriteAllText(jsonPath, stringBuilder.ToString(), new System.Text.UTF8Encoding(false));
            }
            Log.Info($"{exclePath} 导出 {jsonPath}完成");

            AssetDatabase.Refresh();

            return true;
        }

        /// <summary>
        /// 导出表格数据到json文件，格式为JArray
        /// </summary>
        /// <param name="exclePath">需要导出数据的excel表格绝对路径</param>
        /// <param name="jsonPath">要导出的json文件的绝对路径</param>
        /// <returns></returns>
        public static bool ExportExcelToJsonArrayList(string exclePath, string jsonPath, byte[] EncrytionKey = null)
        {
            if (string.IsNullOrEmpty(jsonPath))
            {
                Log.Error("Excel, 导出路径为空，导出json文件失败");
                return false;
            }
            bool exportToEncryteJson = (EncrytionKey != null) ? true : false;
            string jsonExtension = System.IO.Path.GetExtension(jsonPath);
            if (exportToEncryteJson)
            {
                if (jsonExtension != ".bytes")
                {
                    Log.Error("Excel, 导出加密json文件需要扩展名为.bytes");
                    return false;
                }
            }
            else
            {
                if (jsonExtension != ".json")
                {
                    Log.Error("Excel, 导出json文件需要扩展名为.json");
                    return false;
                }
            }

            // 要打开的excel根路径
            DataSet result = GetExcelData(exclePath);

            // 开始添加文件头
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("[");

            int columns = result.Tables[0].Columns.Count;
            int rows = result.Tables[0].Rows.Count;
            string[] cellKey = new string[columns];
            string[] typeList = new string[columns];
            for (int keyi = 0; keyi < columns; keyi++)
            {
                cellKey[keyi] = result.Tables[0].Rows[1][keyi].ToString();
                typeList[keyi] = result.Tables[0].Rows[2][keyi].ToString();
                if (cellKey[keyi] != string.Empty & typeList[keyi] == string.Empty)
                {
                    Log.Error(exclePath + "表格错误, " + cellKey[keyi] + " 没有类型");
                    return false;
                }
            }
            for (int i = 4; i < rows; i++)
            {
                string Key = result.Tables[0].Rows[i][1].ToString();
                string thisRow = "  {";
                for (int j = 1; j < columns; j++)
                {
                    if (cellKey[j] != string.Empty)
                    {
                        string cellValue = result.Tables[0].Rows[i][j].ToString();
                        if (typeList[j] == "number")
                        {
                            thisRow = thisRow + "\"" + cellKey[j] + "\":" + cellValue;
                        }
                        else if (typeList[j] == "string" || typeList[j] == "table")
                        {
                            thisRow = thisRow + "\"" + cellKey[j] + "\":" + "\"" + cellValue + "\"";
                        }
                        else if (typeList[j] == "boolean")
                        {
                            thisRow = thisRow + "\"" + cellKey[j] + "\":" + cellValue;
                        }
                        if (j < columns - 1)
                        {
                            thisRow = thisRow + ",";
                        }
                    }
                }
                thisRow = thisRow + "}";
                if (i < rows - 1)
                {
                    thisRow = thisRow + ",";
                }
                stringBuilder.AppendLine(thisRow);
            }
            stringBuilder.AppendLine("]");
            // 开始写入数据
            if (File.Exists(jsonPath))
            {
                File.Delete(jsonPath);
            }
            if (exportToEncryteJson)
            {
                File.WriteAllBytes(jsonPath, Encryption.GetQuickXorBytes(Converter.GetBytesByString(stringBuilder.ToString()), EncrytionKey));
            }
            else
            {
                File.WriteAllText(jsonPath, stringBuilder.ToString(), new System.Text.UTF8Encoding(false));
            }
            Log.Info($"{exclePath} 导出 {jsonPath}完成");

            AssetDatabase.Refresh();

            return true;
        }

        /// <summary>
        /// 使用table导出的string创建asset文件
        /// </summary>
        /// <param name="exportDirectory">创建asset文件需要存放的文件夹</param>
        /// <param name="mulToOne">是否将assetChars的内容导出到一个文件</param>
        /// <param name="exportOneFileName">是否将assetChars的内容导出到一个文件</param>
        /// <param name="assetChars">Dictionary<string, List<string>> 要导出的文件名/导出文件内容</param>
        public static bool CreateTableExportTextAsset(string exportDirectory, bool mulToOne, string exportOneFileName, Dictionary<string, List<string>> assetChars)
        {
            if (string.IsNullOrEmpty(exportDirectory))
            {
                Log.Error("CreateTextAssetFile 时传入的路径为空");
                return false;
            }
            // 删除当前文件夹下面原有的asset文件
            string assetFilePath = exportDirectory.Substring(exportDirectory.IndexOf("/Assets") + 1);
            string oneFilePath = $"{assetFilePath}/{exportOneFileName}.asset";
            //if (File.Exists(oneFilePath))
            //{
            //    Log.Info($"CreateTextAssetFile 删除旧的TMP字符集文件{oneFilePath}");
            //    File.Delete(oneFilePath);
            //}
            //foreach (var item in assetChars)
            //{
            //    string perFilePath = $"{assetFilePath}/{item.Key}.asset";
            //    if (File.Exists(perFilePath))
            //    {
            //        Log.Info($"CreateTextAssetFile 删除旧的TMP字符集文件{perFilePath}");
            //        File.Delete(perFilePath);
            //    }
            //}
            if (Directory.Exists(assetFilePath))
            {
                Directory.Delete(assetFilePath, true);
            }
            DirectoryInfo newDir =  Directory.CreateDirectory(assetFilePath);
            if (newDir != null)
            {
                Log.Info($"创建导出字符集文件夹{newDir}成功");
            }
            else
            {
                Log.Error($"创建导出字符集文件夹{newDir}失败，不再继续导出");
                return false;
            }

            if (mulToOne)
            {
                string tmpChars = "";
                foreach (var item in assetChars)
                {
                    // 对要制作字库的文字内容进行去重
                    List<string> assetStringArray = item.Value.Distinct().ToList();
                    tmpChars = $"{tmpChars}{string.Join("", assetStringArray)}";
                }
                TextAsset instance = new TextAsset(tmpChars);
                instance.name = "exportFileName";
                AssetDatabase.CreateAsset(instance, oneFilePath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            else
            {
                foreach (var item in assetChars)
                {
                    string perFilePath = $"{assetFilePath}/Localization_{item.Key}_FontTMP_Chars.asset";
                    List<string> assetStringArray = item.Value.Distinct().ToList();
                    TextAsset instance = new TextAsset(string.Join("", assetStringArray));
                    instance.name = item.Key;
                    AssetDatabase.CreateAsset(instance, perFilePath);
                    AssetDatabase.SaveAssets();
                }
                AssetDatabase.Refresh();
            }
            return true;
        }
    }
}

