using System;
using CalculadoraAnestesica.DbContext.Tables;

namespace CalculadoraAnestesica.Helpers
{
	public static class MedicamentosHelper
	{
        public static bool IsGroupedMedication(string tableName)
        {
            return tableName.Equals(TablesSchema.ANESTESICOS_LOCAIS) ||
                   tableName.Equals(TablesSchema.ANTIBIOTICOS) ||
                   tableName.Equals(TablesSchema.INALATORIOS) ||
                   tableName.Equals(TablesSchema.INFUSOES);
        }
    }
}

