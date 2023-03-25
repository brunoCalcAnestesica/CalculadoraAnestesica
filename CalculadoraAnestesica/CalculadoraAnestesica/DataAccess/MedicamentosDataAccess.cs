using System;
using System.Collections.Generic;
using System.Reflection;
using CalculadoraAnestesica.DataAccess.Base;
using CalculadoraAnestesica.DataAccess.Interfaces;
using CalculadoraAnestesica.DbContext.Tables;
using CalculadoraAnestesica.DependencyInjection.IoC;
using CalculadoraAnestesica.Model;
using CalculadoraAnestesica.Model.Interfaces;
using HtmlAgilityPack;

namespace CalculadoraAnestesica.DataAccess
{
    public class MedicamentosDataAccess : DataAccessBase<IMedicamento, Medicamento>, IMedicamentosDataAccess
    {
        private string CurrentGroup { get; set; }
        private string NomeMedicamento { get; set; }
        private string DescricaoMedicamento { get; set; }

        public void InsertMedicamento(
            string tableName,
            string value,
            string descricao)
        {
            Connection.Execute($@"
                INSERT INTO {tableName}
                (nome_medicamento, descricao_medicamento)
                VALUES ('{value}', '{descricao}');"
            );
        }

        public List<Medicamento> GetMedicamentos(string tableName)
        {
            return Connection.CreateCommand($@"
                    SELECT {tableName}.id AS Id,
                           {tableName}.nome_medicamento AS NomeMedicamento,
                           {tableName}.descricao_medicamento AS DescricaoMedicamento
                      FROM {tableName}")
            .ExecuteQuery<Medicamento>();
        }

        public void CreateMedicamentosTables()
        {
            LoadMedicamentosHtml();
        }

        public List<Medicamentos> GetAllMedicamentos()
        {
            var medicamentos = new List<Medicamentos>();
            medicamentos.Add(GetMedicamento(TablesSchema.ANALGESICOS, nameof(TablesSchema.ANALGESICOS)));
            medicamentos.Add(GetMedicamento(TablesSchema.ANESTESICOS_LOCAIS, nameof(TablesSchema.ANESTESICOS_LOCAIS)));
            medicamentos.Add(GetMedicamento(TablesSchema.ANTIBIOTICOS, nameof(TablesSchema.ANTIBIOTICOS)));
            medicamentos.Add(GetMedicamento(TablesSchema.ANTIEMETICOS, nameof(TablesSchema.ANTIEMETICOS)));
            medicamentos.Add(GetMedicamento(TablesSchema.BLOQUEADORES_NEURO_MUSUCLAR, nameof(TablesSchema.BLOQUEADORES_NEURO_MUSUCLAR)));
            medicamentos.Add(GetMedicamento(TablesSchema.BLOQUEIOS_LOCOREGIONA, nameof(TablesSchema.BLOQUEIOS_LOCOREGIONA)));
            medicamentos.Add(GetMedicamento(TablesSchema.CARDIO, nameof(TablesSchema.CARDIO)));
            medicamentos.Add(GetMedicamento(TablesSchema.DIVESOS, nameof(TablesSchema.DIVESOS)));
            medicamentos.Add(GetMedicamento(TablesSchema.DROGAS_DE_EMERGENCIA, nameof(TablesSchema.DROGAS_DE_EMERGENCIA)));
            medicamentos.Add(GetMedicamento(TablesSchema.INALATORIOS, nameof(TablesSchema.INALATORIOS)));
            medicamentos.Add(GetMedicamento(TablesSchema.INDUCAO, nameof(TablesSchema.INDUCAO)));
            medicamentos.Add(GetMedicamento(TablesSchema.INFUSOES, nameof(TablesSchema.INFUSOES)));
            medicamentos.Add(GetMedicamento(TablesSchema.OBSTETRICOS, nameof(TablesSchema.OBSTETRICOS)));
            medicamentos.Add(GetMedicamento(TablesSchema.PRE_MEDICACAO, nameof(TablesSchema.PRE_MEDICACAO)));
            medicamentos.Add(GetMedicamento(TablesSchema.PRE_MEDICACAO_IM_DART, nameof(TablesSchema.PRE_MEDICACAO_IM_DART)));
            medicamentos.Add(GetMedicamento(TablesSchema.SEQUENCIAL_RAPIDO, nameof(TablesSchema.SEQUENCIAL_RAPIDO)));
            medicamentos.Add(GetMedicamento(TablesSchema.VASOPRESSORES_DE_ACAO_CURTA, nameof(TablesSchema.VASOPRESSORES_DE_ACAO_CURTA)));
            return medicamentos;
        }

        private void LoadMedicamentosHtml()
        {
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
                        CreateTable(nameof(TablesSchema.ANALGESICOS));
                        break;
                    case TablesSchema.ANESTESICOS_LOCAIS:
                        CreateTable(nameof(TablesSchema.ANESTESICOS_LOCAIS));
                        break;
                    case TablesSchema.ANTIBIOTICOS:
                        CreateTable(nameof(TablesSchema.ANTIBIOTICOS));
                        break;
                    case TablesSchema.ANTIEMETICOS:
                        CreateTable(nameof(TablesSchema.ANTIEMETICOS));
                        break;
                    case TablesSchema.BLOQUEADORES_NEURO_MUSUCLAR:
                        CreateTable(nameof(TablesSchema.BLOQUEADORES_NEURO_MUSUCLAR));
                        break;
                    case TablesSchema.BLOQUEIOS_LOCOREGIONA:
                        CreateTable(nameof(TablesSchema.BLOQUEIOS_LOCOREGIONA));
                        break;
                    case TablesSchema.CARDIO:
                        CreateTable(nameof(TablesSchema.CARDIO));
                        break;
                    case TablesSchema.DIVESOS:
                        CreateTable(nameof(TablesSchema.DIVESOS));
                        break;
                    case TablesSchema.DROGAS_DE_EMERGENCIA:
                        CreateTable(nameof(TablesSchema.DROGAS_DE_EMERGENCIA));
                        break;
                    case TablesSchema.INALATORIOS:
                        CreateTable(nameof(TablesSchema.INALATORIOS));
                        break;
                    case TablesSchema.INDUCAO:
                        CreateTable(nameof(TablesSchema.INDUCAO));
                        break;
                    case TablesSchema.INFUSOES:
                        CreateTable(nameof(TablesSchema.INFUSOES));
                        break;
                    case TablesSchema.OBSTETRICOS:
                        CreateTable(nameof(TablesSchema.OBSTETRICOS));
                        break;
                    case TablesSchema.PRE_MEDICACAO:
                        CreateTable(nameof(TablesSchema.PRE_MEDICACAO));
                        break;
                    case TablesSchema.PRE_MEDICACAO_IM_DART:
                        CreateTable(nameof(TablesSchema.PRE_MEDICACAO_IM_DART));
                        break;
                    case TablesSchema.SEQUENCIAL_RAPIDO:
                        CreateTable(nameof(TablesSchema.SEQUENCIAL_RAPIDO));
                        break;
                    case TablesSchema.VASOPRESSORES_DE_ACAO_CURTA:
                        CreateTable(nameof(TablesSchema.VASOPRESSORES_DE_ACAO_CURTA));
                        break;
                }
            }

            if (child.Name == "#text")
                NomeMedicamento = child.InnerText;

            if (child.Name == "p1")
            {
                DescricaoMedicamento = child.InnerText;

                Resolver
                .Get<IMedicamentosDataAccess>()
                .InsertMedicamento(
                    CurrentGroup,
                    NomeMedicamento.Replace("\n", "").TrimStart(' ').TrimEnd(' '),
                    DescricaoMedicamento.Replace("\n", "").TrimStart(' ').TrimEnd(' ')
                );
            }
        }

        private void CreateTable(string tableName)
        {
            try
            {
                Connection.Execute($@"
                CREATE TABLE IF NOT EXISTS {tableName}
                (
	                id INTEGER PRIMARY KEY,
	                nome_medicamento TEXT,
                    descricao_medicamento TEXT
                );");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                CurrentGroup = tableName;
            }
        }

        private Medicamentos GetMedicamento(string groupHeader, string tableName)
        {
            return new Medicamentos(groupHeader, GetMedicamentos(tableName));
        }
    }
}

