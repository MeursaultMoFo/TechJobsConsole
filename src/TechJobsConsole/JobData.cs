using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace TechJobsConsole
{
    class JobData
    {
        static List<Dictionary<string, string>> AllJobs = new List<Dictionary<string, string>>();
        static bool IsDataLoaded = false;

        public static List<Dictionary<string, string>> FindAll()
        {
            LoadData();
            return AllJobs;
        }

        /*
         * Returns a list of all values contained in a given column,
         * without duplicates. 
         */
        public static List<string> FindAll(string column)
        {
            LoadData();

            List<string> values = new List<string>();

            foreach (Dictionary<string, string> job in AllJobs)
            {
                string aValue = job[column];

                if (!values.Contains(aValue))
                {
                    values.Add(aValue);
                }
            }
            return values;
        }

        public static List<Dictionary<string, string>> FindByColumnAndValue(string column, string value)
        {
            // load data, if not already loaded
            LoadData();
            // Placeholder for job results
            List<Dictionary<string, string>> jobs = new List<Dictionary<string, string>>();

            foreach (Dictionary<string, string> row in AllJobs) // Grabs one job at a time...
            {
                string aValue = row[column]; // grabbing the value associated with the key/column

                if (aValue.ToLower().Contains(value.ToLower())) // compares key's value to search term
                {
                    jobs.Add(row); // Adds job to Placeholder
                }
            }

            return jobs; // Return results
        }

        // part 2 create method FindByValue
        public static List<Dictionary<string, string>> FindByValue(string searchterm)
        {
            LoadData();
            List<Dictionary<string, string>> listings = new List<Dictionary<string, string>>();

            foreach (Dictionary<string, string> jobpost in AllJobs)
            {
                //loop by jobpost !!!but won't give results if only searching for one word in a section e.g. web or developer or kansas!!!
                foreach (KeyValuePair<string, string> row in jobpost)
                {
                    if (row.Value.ToLower().Contains(searchterm.ToLower()))
                    {
                        listings.Add(jobpost);
                    }
                }
                

                //loop by array column !!!there are duplicates!!!
                //foreach (string section in jobpost.Values)
                //{
                //    if (section.Contains(searchterm))
                //    {
                //        listings.Add(jobpost);
                //    }
                //}

                //loop by keyvaluepair !!!there are duplicates!!!
                //foreach (KeyValuePair<string, string> section in jobpost)
                //{ 
                //    if (section.Value.Contains(searchterm))
                //    {
                //        listings.Add(jobpost);
                //    }
                //}
            }
            return listings;
        }

        /*
         * Load and parse data from job_data.csv
         */
        private static void LoadData()
        {

            if (IsDataLoaded)
            {
                return;
            }

            List<string[]> rows = new List<string[]>();

            using (StreamReader reader = File.OpenText("job_data.csv"))
            {
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    //add toLower() after line?
                    string[] rowArrray = CSVRowToStringArray(line);
                    if (rowArrray.Length > 0)
                    {
                        rows.Add(rowArrray);
                    }
                }
            }

            string[] headers = rows[0];
            rows.Remove(headers);

            // Parse each row array into a more friendly Dictionary
            foreach (string[] row in rows)
            {
                Dictionary<string, string> rowDict = new Dictionary<string, string>();

                for (int i = 0; i < headers.Length; i++)
                {
                    rowDict.Add(headers[i], row[i]);
                }
                AllJobs.Add(rowDict);
            }

            IsDataLoaded = true;
        }

        /*
         * Parse a single line of a CSV file into a string array
         */
        private static string[] CSVRowToStringArray(string row, char fieldSeparator = ',', char stringSeparator = '\"')
        {
            // make jobs lowercase ?
            // string lowerrow = row.ToLower();
            bool isBetweenQuotes = false;
            StringBuilder valueBuilder = new StringBuilder();
            List<string> rowValues = new List<string>();

            // Loop through the row string one char at a time
            foreach (char c in row.ToCharArray())
            {
                if ((c == fieldSeparator && !isBetweenQuotes))
                {
                    rowValues.Add(valueBuilder.ToString());
                    valueBuilder.Clear();
                }
                else
                {
                    if (c == stringSeparator)
                    {
                        isBetweenQuotes = !isBetweenQuotes;
                    }
                    else
                    {
                        valueBuilder.Append(c);
                    }
                }
            }

            // Add the final value
            rowValues.Add(valueBuilder.ToString());
            valueBuilder.Clear();

            return rowValues.ToArray();
        }


    }
}
