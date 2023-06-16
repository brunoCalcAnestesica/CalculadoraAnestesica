using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CalculadoraAnestesica.DataAccess.Base;
using CalculadoraAnestesica.DataAccess.Interfaces;
using CalculadoraAnestesica.DbContext.Tables;
using CalculadoraAnestesica.DependencyInjection.IoC;
using CalculadoraAnestesica.Helpers;
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
        private static Mutex Mutex;

        public MedicamentosDataAccess()
        {
            Mutex = new Mutex();
        }

        public List<Medicamento> GetMedicamentos(string tableName)
        {
            return Connection.CreateCommand($@"
                    SELECT {tableName}.id AS Id,
                           {tableName}.nome_medicamento AS NomeMedicamento,
                           {tableName}.descricao_medicamento AS DescricaoMedicamento,
                           {tableName}.id_grupo AS IdGrupo,
                           {tableName}.is_favorite AS IsFavorite
                      FROM {tableName}")
            .ExecuteQuery<Medicamento>();
        }

        public Medicamento GetMedicamentos(string tableName,
            int id)
        {
            return Connection.CreateCommand($@"
                    SELECT {tableName}.id AS Id,
                           {tableName}.nome_medicamento AS NomeMedicamento,
                           {tableName}.descricao_medicamento AS DescricaoMedicamento,
                           {tableName}.id_grupo AS IdGrupo
                      FROM {tableName}
                     WHERE {tableName}.id = {id}")
                .ExecuteQuery<Medicamento>()?
                .FirstOrDefault();
        }

        public List<Medicamento> GetMedicamentosById(string tableName,
            int id_grupo)
        {
            return GetMedicamentoInternal(tableName);
        }

        public List<Medicamento> GetMedicamento(string tableName)
        {
            return GetMedicamentoInternal(tableName);
        }

        #region HTML

        public void InsertMedicamento(
                string tableName,
                string value,
                string descricao,
                int id_grupo)
        {
            Connection.Execute($@"
                INSERT INTO {tableName}
                (nome_medicamento, descricao_medicamento, id_grupo, is_favorite)
                VALUES ('{value}', '{descricao}', '{id_grupo}', 0);"
            );
        }

        public void CreateMedicamentosTables()
        {
            LoadMedicamentosHtml();
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
                    id_grupo INT,
                    is_favorite BOOLEAN
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
        #endregion

        public List<GrupoNomesDTO> GetGrupoNomes()
        {
            Mutex.WaitOne();

            string tableName = TablesSchema.GRUPO_MEDICAMENTOS;

            var grupoNomes = Connection.CreateCommand($@"
                    SELECT {tableName}.nome_grupo AS NomeGrupo,
                           {tableName}.id as Id
                      FROM {tableName}")
            .ExecuteQuery<GrupoNomesDTO>();

            Mutex.ReleaseMutex();

            return grupoNomes;
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

        public void SetFavoriteMedication(string tableName, int id, bool isFavorite)
        {
            int value = isFavorite ? 1 : 0;

            int rows = Connection.CreateCommand($@"
                UPDATE {tableName}
                    SET is_favorite = {value}
                WHERE {tableName}.id = {id}")
                .ExecuteNonQuery();
        }

        public List<Medicamento> GetFavoriteMedications(string tableName)
        {
            var meds = GetFavoriteMedicationsInternal(tableName);

            for (int i = 0; i < meds.Count; i++)
            {
                var medicamento = BuldMedicamento(meds[i]);
            }

            return meds;
        }

        private List<Medicamento> GetFavoriteMedicationsInternal(string tableName)
        {
            return Connection.CreateCommand($@"
                    SELECT {tableName}.id AS Id,
                           {tableName}.nome_medicamento AS NomeMedicamento,
                           {tableName}.descricao_medicamento AS DescricaoMedicamento,
                           {tableName}.id_grupo AS IdGrupo,
                           {tableName}.is_favorite AS IsFavorite
                      FROM {tableName}
                     WHERE {tableName}.is_favorite = 1")
           .ExecuteQuery<Medicamento>();
        }

        private List<Medicamento> GetMedicamentoInternal(string tableName)
        {
            var med = GetMedicamentos(tableName);

            for (int i = 0; i < med.Count; i++)
            {
                var medicamento = BuldMedicamento(med[i]);
            }

            return med;
        }

        private Medicamento GetMedicamentoInternal(string tableName,
            int id)
        {
            var med = GetMedicamentos(tableName, id);

            if (med is null)
                return null;
            
            return BuldMedicamento(med);
        }

        private Medicamento BuldMedicamento(Medicamento medicamento)
        {
            string[] arr = medicamento.NomeMedicamento.Split(' ');
            medicamento.NomeMedicamento = arr[0].Trim();
            medicamento.DosagemMedicamento = $"{string.Join(" ", arr.Skip(1))}";
            medicamento.Resultado = "0 mg";
            return medicamento;
        }

        public string GetGrupoNomeById(int id)
        {
            string tableName = TablesSchema.GRUPO_MEDICAMENTOS;

            var result = Connection.CreateCommand($@"
                    SELECT {tableName}.nome_grupo AS NomeGrupo
                      FROM {tableName}
                     WHERE {tableName}.id = {id}")
                .ExecuteQuery<GrupoNomesDTO>()?
                .FirstOrDefault();

            return result?.NomeGrupo;
        }

        public Medicamento GetMedicamentosByMedicationId(string tableName,
            int id)
        {
            return GetMedicamentoInternal(tableName, id);
        }
    }
}

