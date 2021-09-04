using System;
using System.Collections.Generic;

namespace MeAgendaAi.Domain.Utils
{
    public class GuidUtil
    {
        public static bool IsGuidValid(string guidString)
        {
            return Guid.TryParse(guidString, out var idGuid);
        }

        public static Guid? ReturnGuidIfValid(string stringGuid)
        {
            if (IsGuidValid(stringGuid)) return Guid.Parse(stringGuid);

            return null;
        }

        public static string GetGuidListSeparadaPorVirgula(List<Guid> guids)
        {
            var textoFormatado = string.Empty;

            for (var i = 0; i < guids.Count; i++)
            {
                textoFormatado += guids[i].ToString();

                if (i != guids.Count - 1) textoFormatado += ", ";
            }

            return textoFormatado;
        }
    }
}