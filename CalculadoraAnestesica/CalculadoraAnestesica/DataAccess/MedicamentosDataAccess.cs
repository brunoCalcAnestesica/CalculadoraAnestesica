using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CalculadoraAnestesica.DataAccess.Base;
using CalculadoraAnestesica.DataAccess.Interfaces;
using CalculadoraAnestesica.DbContext.Tables;
using CalculadoraAnestesica.DependencyInjection.IoC;
using CalculadoraAnestesica.Model;
using CalculadoraAnestesica.Model.Interfaces;
using CalculadoraAnestesica.Shared;
using HtmlAgilityPack;

namespace CalculadoraAnestesica.DataAccess
{
    public class MedicamentosDataAccess : DataAccessBase<IMedicamento, Medicamento>, IMedicamentosDataAccess
    {
        private string CurrentGroup { get; set; }
        private string GroupName { get; set; }
        private string NomeMedicamento { get; set; }
        private string DescricaoMedicamento { get; set; }

        public void InsertMedicamento(
            string tableName,
            string value,
            string descricao,
            int id_grupo)
        {
            Connection.Execute($@"
                INSERT INTO {tableName}
                (nome_medicamento, descricao_medicamento, id_grupo)
                VALUES ('{value}', '{descricao}', '{id_grupo}');"
            );
        }

        public List<Medicamento> GetMedicamentos(string tableName)
        {
            return Connection.CreateCommand($@"
                    SELECT {tableName}.id AS Id,
                           {tableName}.nome_medicamento AS NomeMedicamento,
                           {tableName}.descricao_medicamento AS DescricaoMedicamento,
                           {tableName}.id_grupo AS IdGrupo
                      FROM {tableName}")
            .ExecuteQuery<Medicamento>();
        }

        public List<Medicamento> GetMedicamentosById(string tableName,
            int id_grupo)
        {
            return GetMedicamentoInternal(tableName);
        }

        public void CreateMedicamentosTables()
        {
            LoadMedicamentosHtml();
        }

        public List<Medicamento> GetMedicamento(string tableName)
        {
            return GetMedicamentoInternal(nameof(TablesSchema.ANALGESICOS));
        }

        private void LoadMedicamentosHtml()
        {
            CreateGroupsNameTable();

            var stream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream("CalculadoraAnestesica.EmbeddedResources.AnestCalc.html");

            var doc = new HtmlDocument();
            doc.Load(stream);

            ReadAllHtmlFile(doc);
        }

        private void ReadAllHtmlFile(HtmlDocument html)
        {
            if (html.DocumentNode != null)
            {
                HtmlNode bodyNode = html
                    .DocumentNode
                    .SelectSingleNode("//body");

                if (bodyNode != null)
                {
                    foreach (var child in bodyNode.ChildNodes)
                    {
                        if (!string.IsNullOrEmpty(child.InnerText) &&
                            child.InnerText.Replace(" ", "") != "\n")
                        {
                            CreateTables(child);
                        }
                    }
                }
            }
        }

        private void CreateTables(HtmlNode child)
        {
            if (child.Name == "h1")
            {
                switch (child.InnerText.TrimStart(' ').TrimEnd(' '))
                {
                    case TablesSchema.ANALGESICOS:
                        CreateTable(nameof(TablesSchema.ANALGESICOS), TablesSchema.ANALGESICOS);
                        break;
                    case TablesSchema.ANESTESICOS_LOCAIS:
                        CreateTable(nameof(TablesSchema.ANESTESICOS_LOCAIS), TablesSchema.ANESTESICOS_LOCAIS);
                        break;
                    case TablesSchema.ANTIBIOTICOS:
                        CreateTable(nameof(TablesSchema.ANTIBIOTICOS), TablesSchema.ANTIBIOTICOS);
                        break;
                    case TablesSchema.ANTIEMETICOS:
                        CreateTable(nameof(TablesSchema.ANTIEMETICOS), TablesSchema.ANTIEMETICOS);
                        break;
                    case TablesSchema.BLOQUEADORES_NEURO_MUSUCLAR:
                        CreateTable(nameof(TablesSchema.BLOQUEADORES_NEURO_MUSUCLAR), TablesSchema.BLOQUEADORES_NEURO_MUSUCLAR);
                        break;
                    case TablesSchema.BLOQUEIOS_LOCOREGIONA:
                        CreateTable(nameof(TablesSchema.BLOQUEIOS_LOCOREGIONA), TablesSchema.BLOQUEIOS_LOCOREGIONA);
                        break;
                    case TablesSchema.CARDIO:
                        CreateTable(nameof(TablesSchema.CARDIO), TablesSchema.CARDIO);
                        break;
                    case TablesSchema.DIVESOS:
                        CreateTable(nameof(TablesSchema.DIVESOS), TablesSchema.DIVESOS);
                        break;
                    case TablesSchema.DROGAS_DE_EMERGENCIA:
                        CreateTable(nameof(TablesSchema.DROGAS_DE_EMERGENCIA), TablesSchema.DROGAS_DE_EMERGENCIA);
                        break;
                    case TablesSchema.INALATORIOS:
                        CreateTable(nameof(TablesSchema.INALATORIOS), TablesSchema.INALATORIOS);
                        break;
                    case TablesSchema.INDUCAO:
                        CreateTable(nameof(TablesSchema.INDUCAO), TablesSchema.INDUCAO);
                        break;
                    case TablesSchema.INFUSOES:
                        CreateTable(nameof(TablesSchema.INFUSOES), TablesSchema.INFUSOES);
                        break;
                    case TablesSchema.OBSTETRICOS:
                        CreateTable(nameof(TablesSchema.OBSTETRICOS), TablesSchema.OBSTETRICOS);
                        break;
                    case TablesSchema.PRE_MEDICACAO:
                        CreateTable(nameof(TablesSchema.PRE_MEDICACAO), TablesSchema.PRE_MEDICACAO);
                        break;
                    case TablesSchema.PRE_MEDICACAO_IM_DART:
                        CreateTable(nameof(TablesSchema.PRE_MEDICACAO_IM_DART), TablesSchema.PRE_MEDICACAO_IM_DART);
                        break;
                    case TablesSchema.SEQUENCIAL_RAPIDO:
                        CreateTable(nameof(TablesSchema.SEQUENCIAL_RAPIDO), TablesSchema.SEQUENCIAL_RAPIDO);
                        break;
                    case TablesSchema.VASOPRESSORES_DE_ACAO_CURTA:
                        CreateTable(nameof(TablesSchema.VASOPRESSORES_DE_ACAO_CURTA), TablesSchema.VASOPRESSORES_DE_ACAO_CURTA);
                        break;
                }
            }

            if (child.Name == "#text")
            {
                NomeMedicamento = child.InnerText;
                DescricaoMedicamento = child.InnerText;

                var grupo = GetGrupoNome(GroupName);

                InsertMedicamento(
                    CurrentGroup,
                    NomeMedicamento.Replace("\n", "").TrimStart(' ').TrimEnd(' '),
                    DescricaoMedicamento.Replace("\n", "").TrimStart(' ').TrimEnd(' '),
                    grupo.Id
                );
            }

            if (child.Name == "p1")
            {
                DescricaoMedicamento = child.InnerText;

                var grupo = GetGrupoNome(GroupName);

                InsertMedicamento(
                    CurrentGroup,
                    NomeMedicamento.Replace("\n", "").TrimStart(' ').TrimEnd(' '),
                    DescricaoMedicamento.Replace("\n", "").TrimStart(' ').TrimEnd(' '),
                    grupo.Id
                );
            }
        }

        private void CreateTable(string tableName, string groupName)
        {
            try
            {
                Connection.Execute($@"
                CREATE TABLE IF NOT EXISTS {tableName}
                (
	                id INTEGER PRIMARY KEY,
	                nome_medicamento TEXT,
                    descricao_medicamento TEXT,
                    id_grupo INT
                );");

                InsertGrupo(groupName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                CurrentGroup = tableName;
                GroupName = groupName;
            }
        }

        private void CreateGroupsNameTable()
        {
            try
            {
                Connection.Execute($@"
                CREATE TABLE IF NOT EXISTS GrupoMedicamentos
                (
	                id INTEGER PRIMARY KEY,
	                nome_grupo TEXT
                );");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void InsertGrupo(string grupoNome)
        {
            Connection.Execute($@"
                INSERT INTO {TablesSchema.GRUPO_MEDICAMENTOS}
                (nome_grupo)
                VALUES ('{grupoNome}');"
            );
        }

        public List<GrupoNomesDTO> GetGrupoNomes()
        {
            string tableName = TablesSchema.GRUPO_MEDICAMENTOS;

            return Connection.CreateCommand($@"
                    SELECT {tableName}.nome_grupo AS NomeGrupo,
                           {tableName}.id as Id
                      FROM {tableName}")
           .ExecuteQuery<GrupoNomesDTO>();
        }

        public GrupoNomesDTO GetGrupoNome(string nome)
        {
            string tableName = TablesSchema.GRUPO_MEDICAMENTOS;

            return Connection.CreateCommand($@"
                    SELECT {tableName}.nome_grupo AS NomeGrupo,
                           {tableName}.id as Id
                      FROM {tableName}
                     WHERE {tableName}.nome_grupo = '{nome}'")
           .ExecuteQuery<GrupoNomesDTO>()?
           .FirstOrDefault();
        }

        private List<Medicamento> GetMedicamentoInternal(string tableName)
        {
            var med = GetMedicamentos(tableName);

            for (int i = 0; i < med.Count; i++)
            {
                var medicamento = med[i];
                string[] arr = medicamento.NomeMedicamento.Split(' ');

                medicamento.NomeMedicamento = arr[0].Trim();
                medicamento.DosagemMedicamento = $"{string.Join(" ", arr.Skip(1))}";
                medicamento.Resultado = "0 mg";
            }

            return med;
        }
    }
}

