namespace Trcont.IRS.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Trcont.Domain.RsiDto;
    using Trcont.IRS.Domain.Entity;

    public interface IScriptSqlBuilder
    {
        IEnumerable<string> Build(IEnumerable<ScriptStepDto> scripts, IEnumerable<ScriptParams> paramsScript, Dictionary<string, string> customScriptParams);

        IEnumerable<string> BuildDebug(IEnumerable<ScriptStepDto> scripts);
    }

    public class ScriptSqlBuilder : IScriptSqlBuilder
    {
        public ScriptSqlBuilder()
        {
        }

        public IEnumerable<string> Build(IEnumerable<ScriptStepDto> scripts, IEnumerable<ScriptParams> paramsScript, Dictionary<string, string> customScriptParams)
        {
            IList<string> list = new List<string>();

            foreach (var script in scripts)
            {
                list.Add(BuildForScript(script, paramsScript, customScriptParams));
            }

            return list;
        }

        private string BuildForScript(ScriptStepDto script, IEnumerable<ScriptParams> paramsScript, Dictionary<string, string> customScriptParams)
        {
            var sql = GetSql(script);

            foreach (var param in paramsScript)
            {
                string value = null;
                customScriptParams.TryGetValue(param.ParamName, out value);

                sql = sql.Replace($":{param.ParamName.ToLower()}", GetParamValue(param, value));
            }

            return sql;
        }

        public IEnumerable<string> BuildDebug(IEnumerable<ScriptStepDto> scripts)
        {
            IList<string> list = new List<string>();

            foreach (var script in scripts)
            {
                list.Add(GetSql(script));
            }

            return list;
        }

        private string GetParamValue(ScriptParams param, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            switch (param.ParamType)
            {
                case 1:
                    if (string.IsNullOrEmpty(param.DefValue1))
                    {
                        return string.Empty;
                    }

                    return $"'{param.DefValue1}'";

                case 2:
                    return param.ParamInt1.ToString();

                case 3:
                case 4:
                case 5:
                case 6:
                case 16:
                case 17:
                    return param.ParamInt1.ToString();
            }

            throw new Exception($"Not find ParamType - {param.ParamType}");
        }

        private static string GetSql(ScriptStepDto script)
        {
            return Encoding.GetEncoding("windows-1251")
                .GetString(script.Content)
                .Replace('%', ':')
                .ToLower();
        }
    }
}
