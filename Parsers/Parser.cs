using EfcToXamarinAndroid.Core;
using MyFinToControl.Configs.ManagerCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Tabula;
using Tabula.Extractors;
using UglyToad.PdfPig;

namespace MyFinToControl.Parsers
{
    internal class Parser
    {
        public List<BankConfiguration> BankConfigurations { get; }
        private List<DataItem> Data { get; set; }
        List<Sms> smslist = new List<Sms>();

        public Parser(List<Sms> data, List<BankConfiguration> bankConfigurations)
        {
            BankConfigurations = bankConfigurations;
            Data = new List<DataItem>();
            smslist = data;
           
        }
        public Parser(Sms data, List<BankConfiguration> bankConfigurations)
        {
            BankConfigurations = bankConfigurations;
            Data = new List<DataItem>();
            smslist.Add(data);

        }

        public async Task<List<DataItem>> GetData()
        {
           await SmsToDataItems();
            return Data;
        }
        private async Task SmsToDataItems()
        {
            await Task.Run(() =>
            {
                foreach (Sms sms in smslist)
                {
                   // Thread.Sleep(100);

                    var ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                    ci.NumberFormat.NumberDecimalSeparator = ".";

                    var msg = sms.getMsg();

                    var BelarusbankSmsRegex = BankConfigurations.Where(x => x.SmsNumber == sms.getAddress()).First().SmsParseRegex;

                    OperacionTyps operType;
                    var parselableOperTyp = new Regex(BelarusbankSmsRegex.OperacionTyp).Match(msg).Value.ToString();

                    var operTyptempls = BankConfigurations.Where(x => x.SmsNumber == sms.getAddress()).First();
                    if (BankConfigurations.Where(x => x.SmsNumber == sms.getAddress()).First().PaymentTemplates.Contains(parselableOperTyp))
                        operType = OperacionTyps.OPLATA;
                    else if (BankConfigurations.Where(x => x.SmsNumber == sms.getAddress()).First().DepositTemplates.Contains(parselableOperTyp))
                        operType = OperacionTyps.ZACHISLENIE;
                    else if (BankConfigurations.Where(x => x.SmsNumber == sms.getAddress()).First().СashTemplates.Contains(parselableOperTyp))
                        operType = OperacionTyps.NALICHNYE;
                    else
                        operType = OperacionTyps.UNREACHABLE;

                    string date = new Regex(BelarusbankSmsRegex.Date).Match(msg).Value;
                    if (date != "")
                    {
                        DateTime dateValue = DateTime.Parse(date);
                        DataItem dataItem = new DataItem(operType, (DateTime)dateValue);
                        dataItem.Sum = float.TryParse(new Regex(BelarusbankSmsRegex.Sum).Match(msg).Value, NumberStyles.Any, ci, out float tempSum) ? tempSum : default;
                        dataItem.Balance = float.TryParse(new Regex(BelarusbankSmsRegex.Balance).Match(msg).Value, NumberStyles.Any, ci, out float tempBalance) ? tempBalance : default;
                        dataItem.Karta = int.TryParse(new Regex(BelarusbankSmsRegex.Karta).Match(msg).Value, NumberStyles.Any, ci, out int tempKarta) ? tempKarta : default;
                        dataItem.MCC = int.TryParse(new Regex(BelarusbankSmsRegex.Mcc).Match(msg).Value, NumberStyles.Any, ci, out int tempMcc) ? tempMcc : default;
                        dataItem.Descripton = new Regex(BelarusbankSmsRegex.Descripton).Match(msg).Value.Trim(' ').ToUpper();
                        dataItem.SmsAdress = sms.getAddress();
                        if (operType == OperacionTyps.UNREACHABLE)
                            dataItem.UnreachableOperacionTyp = parselableOperTyp;
                        Data.Add(dataItem);

 //!!                   //makeAsoption
                        if(sms.getReadState()=="0")
                        sms.setReadState("1");
                    }
                }
            });
           
        }

        string _pdfRepportPatch;
        public Parser(string pdfRepportPatch, List<BankConfiguration> bankConfigurations)
        {
            Data = new List<DataItem>();
            BankConfigurations = bankConfigurations;
            _pdfRepportPatch = pdfRepportPatch;
            //BankConfiguration bankConfiguration = _bankConfigurations[0];
        }
        async Task ParseStart(string pdfRepportPatch)
        {
            string firstColmnName = BankConfigurations[0].PdfReportTemplate.FirstColumnName;
            int collCount = BankConfigurations[0].PdfReportTemplate.CollamnCount;

            List<string[]> resalt = new List<string[]>();

            await Task.Run(() =>
            {
                using (PdfDocument document = PdfDocument.Open(pdfRepportPatch, new ParsingOptions() { ClipPaths = true }))
                {
                    ObjectExtractor oe = new ObjectExtractor(document);
                    List<Cell> allRowsInTables = new List<Cell>();

                    for (int i = 1; i <= document.NumberOfPages; i++)
                    {
                        PageArea page = oe.Extract(i);
                        IExtractionAlgorithm ea = new SpreadsheetExtractionAlgorithm();
                        List<Table> tables = ea.Extract(page);
                        var _tables = tables.Where(x => x.ColumnCount == collCount).ToList<Table>().Where(x => x.Rows[0][0].ToString() == firstColmnName).ToList();
                        foreach (var tb in _tables)
                        {
                            for (int k = 1; k < tb.Cells.Count / collCount; k++)
                            {
                                var pr = tb.Cells.Skip(collCount * k).Take(collCount).Select(x => x.GetText()).ToArray();
                                resalt.Add(pr);
                            }
                        }
                    }
                }
            });

            PdfToDataItem(resalt);
        }
        void PdfToDataItem(List<string[]> resalt)
        {
            foreach (var str in resalt)
            {
                var bc = BankConfigurations[0].PdfReportTemplate;
                var ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                ci.NumberFormat.NumberDecimalSeparator = ".";

                OperacionTyps operType = OperacionTyps.OPLATA;
                //OperacionTyps operType = str[bc.NomrColumnOfOperacionTyp] == bc.PaymentsName ? OperacionTyps.OPLATA : OperacionTyps.ZACHISLENIE;
                if (str[bc.NomrColumnOfOperacionTyp] == bc.PaymentsName)
                {
                    if (bc.CashMCCs.Split(" ").Contains(str[bc.NomrColumnOfMCC]))
                        operType = OperacionTyps.NALICHNYE;
                    else
                        operType = OperacionTyps.OPLATA;
                }
                else
                    operType = OperacionTyps.ZACHISLENIE;
                DateTime dateValue = DateTime.Parse(str[bc.NomrColumnOfDate]);

                DataItem dataItem = new DataItem(operType, (DateTime)dateValue);

                dataItem.Sum = float.TryParse(str[bc.NomrColumnOfSum], NumberStyles.Any, ci, out float tempSum) ? tempSum : default;
                dataItem.Balance = float.TryParse(str[bc.NomrColumnOfBalance], NumberStyles.Any, ci, out float tempBalance) ? tempBalance : default;
                dataItem.Karta = int.TryParse(str[bc.NomrColumnOfKarta].Split(" ").Last(), NumberStyles.Any, ci, out int tempKarta) ? tempKarta : default;
                var deskr = str[bc.NomrColumnOfDescripton].Split("/")[0]?.ToUpper().Trim();
                if (deskr != "")
                    dataItem.Descripton = deskr;
                dataItem.MCC = int.TryParse(str[bc.NomrColumnOfMCC], NumberStyles.Any, ci, out int tempMCC) ? tempMCC : default;
                Data.Add(dataItem);
            }
        }
        public async Task<List<DataItem>> GetDataFromPdf()
        {
            await ParseStart(_pdfRepportPatch);
            return Data;
        }
    }
}