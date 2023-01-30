using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace FormatConverter.Functions.FileTypes
{
    internal enum YamlValueType
    {
        Null,
        Int,
        Float,
        Inf,
        NaN,
        Bool,
        Str,
        Timestamp,
        Others
    }

    internal static class YamlExtensions
    {
        public static string GetString(this YamlNode yaml)
        {
            return yaml switch
            {
                YamlScalarNode scalar => scalar.Value,
                YamlMappingNode mapping => "{" + string.Join(", ", mapping.Select(i => $"{i.Key.GetString()}: {i.Value.GetString()}")) + "}",
                YamlSequenceNode sequence => "[" + string.Join(", ", sequence.Select(i => i.GetString())) + "]",
                _ => ""
            };
        }

        public static YamlValueType GetYamlValueType(this YamlScalarNode yaml)
        {
            var raw = yaml.Value;

            if (yaml.Tag != null)
            {
                return yaml.Tag switch
                {
                    "tag:yaml.org,2002:null" => YamlValueType.Null,
                    "tag:yaml.org,2002:int" => YamlValueType.Int,
                    "tag:yaml.org,2002:float" => Regex.Match(raw, @"^[-+]?\.(inf|Inf|INF)f$").Success ? YamlValueType.Inf :
                        Regex.Match(raw, @"^\.(nan|NaN|NAN)$").Success ? YamlValueType.NaN : YamlValueType.Float,
                    "tag:yaml.org,2002:bool" => YamlValueType.Bool,
                    "tag:yaml.org,2002:str" => YamlValueType.Str,
                    "tag:yaml.org,2002:timestamp" => YamlValueType.Timestamp,
                    _ => YamlValueType.Others
                };
            }

            if (yaml.Style == ScalarStyle.SingleQuoted) return YamlValueType.Str;
            if (yaml.Style == ScalarStyle.DoubleQuoted) return YamlValueType.Str;
            if (yaml.Style == ScalarStyle.Literal) return YamlValueType.Str;
            if (yaml.Style == ScalarStyle.Folded) return YamlValueType.Str;

            if (Regex.Match(raw, @"^(~|null|Null|NULL|)$").Success) return YamlValueType.Null;
            if (Regex.Match(raw, @"^[-+]?(0b[01_]+|0[0-7_]+|0|[1-9][0-9_]*|0x[0-9a-fA-F_]+|[1-9][0-9_]*(:[0-5]?[0-9])+)$").Success)
                return YamlValueType.Int;
            if (Regex.Match(raw.ToLower(), @"^[-+]?([0-9][0-9_]*)?(\.[0-9.]*([eE][-+][0-9]+)?|(:[0-5]?[0-9])+\.[0-9_]*)$").Success)
                return YamlValueType.Float;
            if (Regex.Match(raw, @"^[-+]?\.(inf|Inf|INF)$").Success) return YamlValueType.Inf;
            if (Regex.Match(raw, @"^\.(nan|NaN|NAN)$").Success) return YamlValueType.NaN;
            if (Regex.Match(raw, @"^(y|Y|yes|Yes|YES|n|N|no|No|NO|true|True|TRUE|false|False|FALSE|on|On|ON|off|Off|OFF)$").Success)
                return YamlValueType.Bool;
            if (Regex.Match(raw, @"^'.*'$|^"".*""$").Success) return YamlValueType.Str;
            if (Regex.Match(raw, @"^([0-9]{4}-[0-9]{2}-[0-9]{2}|[0-9]{4}-[0-9]{1,2}-[0-9]{1,2}([Tt]|[ \t]+)[0-9]{1,2}:[0-9]{2}:[0-9]{2}(\.[0-9]*)?(([ \t]*)Z|[-+][0-9]{1,2}(:[0-9]{2})?)?)$").Success)
                return YamlValueType.Timestamp;
            return YamlValueType.Str;
        }

        public static short GetInt16Value(this YamlScalarNode yaml)
        {
            string value = yaml.Value.Replace("_", "");
            if (value.StartsWith("0b")) return Convert.ToInt16(value[2..], 2);
            if (value.StartsWith("0x")) return Convert.ToInt16(value[2..], 16);
            if (value.StartsWith("0") && value != "0") return Convert.ToInt16(value[1..], 8);
            if (value.Contains(":")) return (short)new SexagesimalNumber(value.Split(':').Select(i => int.Parse(i)).ToArray());
            return short.Parse(value);
        }

        public static int GetInt32Value(this YamlScalarNode yaml)
        {
            string value = yaml.Value.Replace("_", "");
            if (value.StartsWith("0b")) return Convert.ToInt32(value[2..], 2);
            if (value.StartsWith("0x")) return Convert.ToInt32(value[2..], 16);
            if (value.StartsWith("0") && value != "0") return Convert.ToInt32(value[1..], 8);
            if (value.Contains(":")) return (int)new SexagesimalNumber(value.Split(':').Select(i => int.Parse(i)).ToArray());
            return int.Parse(value);
        }

        public static long GetInt64Value(this YamlScalarNode yaml)
        {
            string value = yaml.Value.Replace("_", "");
            if (value.StartsWith("0b")) return Convert.ToInt64(value[2..], 2);
            if (value.StartsWith("0x")) return Convert.ToInt64(value[2..], 16);
            if (value.StartsWith("0") && value != "0") return Convert.ToInt64(value[1..], 8);
            if (value.Contains(":")) return (long)new SexagesimalNumber(value.Split(':').Select(i => int.Parse(i)).ToArray());
            return long.Parse(value);
        }

        public static float GetSingleValue(this YamlScalarNode yaml)
        {
            string value = new Regex(@"(?<=\..*)\.").Replace(yaml.Value.Replace("_", ""), "");
            if (value.Contains(":"))
            {
                var parts = value.Split('.');
                var intPart = parts[0];
                var decimalPart = parts.Length > 1 ? parts[1] : "0";
                return (float)new SexagesimalNumber(decimal.Parse("0." + decimalPart), intPart.Split(':').Select(i => int.Parse(i)).ToArray());
            }
            if (value.ToLower().EndsWith("inf")) return value.StartsWith("-") ? float.NegativeInfinity : float.PositiveInfinity;
            if (value.ToLower().EndsWith("nan")) return float.NaN;
            return float.Parse(value);
        }

        public static double GetDoubleValue(this YamlScalarNode yaml)
        {
            string value = new Regex(@"(?<=\..*)\.").Replace(yaml.Value.Replace("_", ""), "");
            if (value.Contains(":"))
            {
                var parts = value.Split('.');
                var intPart = parts[0];
                var decimalPart = parts.Length > 1 ? parts[1] : "0";
                return (double)new SexagesimalNumber(decimal.Parse("0." + decimalPart), intPart.Split(':').Select(i => int.Parse(i)).ToArray());
            }
            if (value.ToLower().EndsWith("inf")) return value.StartsWith("-") ? double.NegativeInfinity : double.PositiveInfinity;
            if (value.ToLower().EndsWith("nan")) return double.NaN;
            return double.Parse(value);
        }

        public static decimal GetDecimalValue(this YamlScalarNode yaml)
        {
            string value = new Regex(@"(?<=\..*)\.").Replace(yaml.Value.Replace("_", ""), "");
            if (value.Contains(":"))
            { 
                var parts = value.Split('.');
                var intPart = parts[0];
                var decimalPart = parts.Length > 1 ? parts[1] : "0";
                return new SexagesimalNumber(decimal.Parse("0." + decimalPart), intPart.Split(':').Select(i => int.Parse(i)).ToArray());
            }
            return decimal.Parse(value, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint);
        }

        public static bool GetBooleanValue(this YamlScalarNode yaml)
        {
            return Regex.Match(yaml.Value, @"^(y|Y|yes|Yes|YES|true|True|TRUE|on|On|ON)$").Success;
        }


        public static string GetStringValue(this YamlScalarNode yaml)
        {
            return yaml.Value;
        }

        public static DateTime GetDateTimeValue(this YamlScalarNode yaml)
        {
            return DateTime.Parse(yaml.Value);
        }

        public static DateTimeOffset GetDateTimeOffsetValue(this YamlScalarNode yaml)
        {
            return DateTimeOffset.Parse(yaml.Value);
        }

        public static string GetDateTimeStringValue(this YamlScalarNode yaml)
        {
            var dt = yaml.GetDateTimeValue();
            if (dt.Kind != DateTimeKind.Unspecified) return yaml.GetDateTimeOffsetValue().ToString();
            else return dt.ToString();
        }
    }
}
