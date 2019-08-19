using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows;

namespace FrontV2.Utilities
{
    public class CompanyNameCleaner
    {
        public static List<string> extensions;
        public static Dictionary<string, string> exceptions;

        public CompanyNameCleaner()
        {

        }

        public void FillExtensions()
        {
            string sourceFilePath = @"\\mede1\partage\,FGA Front Office\02_Gestion_Actions\00_BASE\Base 2.0\NettoyageExtensions.csv";
            FileInfo myFile = new FileInfo(sourceFilePath);
            if (!myFile.Exists)
                return;

            extensions = new List<string>();
            try
            {
                TextFieldParser csvReader = new TextFieldParser(myFile.ToString());
                csvReader.SetDelimiters(new String[] { ";" });
                csvReader.HasFieldsEnclosedInQuotes = false;

                while (!csvReader.EndOfData)
                {
                    String[] line = csvReader.ReadFields();
                    extensions.Add(line[0].ToString());
                }
                csvReader.Close();
            }
            catch
            {
                MessageBox.Show("Impossible d'ouvrir le fichier:\n" + sourceFilePath);
            }
        }

        public void FillExceptions()
        {
            string sourceFilePath = @"\\mede1\partage\,FGA Front Office\02_Gestion_Actions\00_BASE\Base 2.0\NettoyageExceptions.csv";
            FileInfo myFile = new FileInfo(sourceFilePath);
            if (!myFile.Exists)
                return;

            exceptions = new Dictionary<string, string>();
            try
            {
                TextFieldParser csvReader = new TextFieldParser(myFile.ToString());
                csvReader.SetDelimiters(new String[] { ";" });
                csvReader.HasFieldsEnclosedInQuotes = false;

                while (!csvReader.EndOfData)
                {
                    String[] line = csvReader.ReadFields();
                    exceptions.Add(line[0].ToString(), line[1].ToString());
                }
                csvReader.Close();
            }
            catch
            {
                MessageBox.Show("Impossible d'ouvrir le fichier:\n" + sourceFilePath);
            }
        }

        public void PrintExtensions()
        {
            String message = "";
            foreach (var v in extensions)
                message += v.ToString() + "\n";

            MessageBox.Show("Voici les extensions a nettoyer:\n" + message);
        }

        public void PrintExceptions()
        {
            String message = "";
            foreach (var v in exceptions)
                message += v.Key.ToString() + " | " + v.Value.ToString() + "\n";

            MessageBox.Show("Voici les exceptions a nettoyer:\n" + message);
        }

        public DataTable CleanCompanyName(DataTable source, String colTicker = "TICKER", String colCompany = "Company_Name")
        {
            foreach (DataRow row in source.Rows)
            {
                String curTicker = row[colTicker].ToString();
                if (curTicker == "")
                    continue;
                row[colCompany] = row[colCompany].ToString().Replace(",", "");
                row[colCompany] = row[colCompany].ToString().Replace(".", "");

                if (exceptions.ContainsKey(curTicker))
                {
                    row[colCompany] = exceptions[curTicker];
                }
                else
                {
                    String company = row[colCompany].ToString();

                    foreach (String extension in extensions)
                    {
                        bool containsAll = true;
                        String[] extensionCut = extension.Split(' ');
                        List<String> extensionWords = new List<string>();
                        foreach (String s in extensionCut)
                            extensionWords.Add(s);

                        foreach (String token in extensionWords)
                        {
                            if (!company.Contains(token))
                            {
                                containsAll = false;
                                break;
                            }

                        }
                        if (containsAll)
                        {
                            row[colCompany] = "";
                            foreach (String c in company.Split(' '))
                            {
                                if (!extensionWords.Contains(c))
                                    row[colCompany] += c + " ";
                            }
                            row[colCompany] = row[colCompany].ToString().TrimEnd();
                            company = row[colCompany].ToString();
                        }
                    }
                    company = company.Replace(" AND ", " & ");
                }
            }
            return source;
        }

        public List<String> CleanCompanyName(List<String> companiesAndTickers, char separator = '|')
        {
            List<String> result = new List<string>();
            foreach (String line in companiesAndTickers)
            {
                if (line == " ")
                    continue;
                String ticker = line.Split(separator)[1];
                ticker = ticker.TrimStart();
                String company = line.Split(separator)[0];

               
              
                company = company.ToString().Replace(",", "");
                company = company.ToString().Replace(".", "");

                if (exceptions.ContainsKey(ticker))
                    company = exceptions[ticker];
                else
                {
                    foreach (String extension in extensions)
                    {
                        bool containsAll = true;
                        String[] extensionCut = extension.Split(' ');
                        List<String> extensionWords = new List<string>();
                        foreach (String s in extensionCut)
                            extensionWords.Add(s);

                        foreach (String token in extensionWords)
                        {
                            if (!company.Contains(token))
                            {
                                containsAll = false;
                                break;
                            }
                        }

                        if (containsAll)
                        {
                            String tmp = "";
                            foreach (String c in company.Split(' '))
                            {
                                if (!extensionWords.Contains(c))
                                    tmp += c + " ";
                            }
                            company = tmp;
                            company = company.ToString().TrimEnd();
                            company = company.ToString();
                        }
                    }
                    company = company.Replace(" AND ", " & ");
                }
                result.Add(company + " | " + ticker);
            }
            return result;
        }
    }
}