using System;
using CalculadoraAnestesica.DbContext.Tables;

namespace CalculadoraAnestesica.Helpers
{
	public static class Utils
	{
        public static string ConvertToTableSchema(string name)
        {
            switch (name)
            {
                case TablesSchema.ANALGESICOS:
                    return nameof(TablesSchema.ANALGESICOS);

                case TablesSchema.ANESTESICOS_LOCAIS:
                    return nameof(TablesSchema.ANESTESICOS_LOCAIS);

                case TablesSchema.ANTIBIOTICOS:
                    return nameof(TablesSchema.ANTIBIOTICOS);

                case TablesSchema.ANTIEMETICOS:
                    return nameof(TablesSchema.ANTIEMETICOS);

                case TablesSchema.BLOQUEADORES_NEURO_MUSUCLAR:
                    return nameof(TablesSchema.BLOQUEADORES_NEURO_MUSUCLAR);

                case TablesSchema.BLOQUEIOS_LOCOREGIONA:
                    return nameof(TablesSchema.BLOQUEIOS_LOCOREGIONA);

                case TablesSchema.CARDIO:
                    return nameof(TablesSchema.CARDIO);

                case TablesSchema.DIVESOS:
                    return nameof(TablesSchema.DIVESOS);

                case TablesSchema.DROGAS_DE_EMERGENCIA:
                    return nameof(TablesSchema.DROGAS_DE_EMERGENCIA);

                case TablesSchema.INALATORIOS:
                    return nameof(TablesSchema.INALATORIOS);

                case TablesSchema.INDUCAO:
                    return nameof(TablesSchema.INDUCAO);

                case TablesSchema.INFUSOES:
                    return nameof(TablesSchema.INFUSOES);

                case TablesSchema.OBSTETRICOS:
                    return nameof(TablesSchema.OBSTETRICOS);

                case TablesSchema.PRE_MEDICACAO:
                    return nameof(TablesSchema.PRE_MEDICACAO);

                case TablesSchema.PRE_MEDICACAO_IM_DART:
                    return nameof(TablesSchema.PRE_MEDICACAO_IM_DART);

                case TablesSchema.SEQUENCIAL_RAPIDO:
                    return nameof(TablesSchema.SEQUENCIAL_RAPIDO);

                case TablesSchema.VASOPRESSORES_DE_ACAO_CURTA:
                    return nameof(TablesSchema.VASOPRESSORES_DE_ACAO_CURTA);
            }

            return "";
        }
    }
}

