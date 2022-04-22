﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace RecruitmentExercise
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> logMessages = new List<string>();
            string content;

            var logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
            var filePath = logFile + "logFile.txt";
            File.Create(filePath).Dispose();

            Console.WriteLine("Enter the file path: ");
            string path = Console.ReadLine();

            string[] usernames = File.ReadAllLines(path);

            for (int i = 0; i < usernames.Length; i++)
            {
                try
                {
                    string target = "https://api.bitbucket.org/2.0/users/" + usernames[i];
                    WebRequest wrGETURL;
                    wrGETURL = WebRequest.Create(target);

                    Stream objStream;
                    objStream = wrGETURL.GetResponse().GetResponseStream();

                    StreamReader objReader = new StreamReader(objStream);

                    string sLine = "";
                    int j = 0;

                    while (sLine != null)
                    {
                        j++;
                        sLine = objReader.ReadLine();
                        if (sLine != null)
                            Console.WriteLine("{0}:{1}", j, sLine);
                    }
                    Console.ReadLine();

                }
                catch (WebException ex)
                {
                    using (Stream stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        Console.WriteLine("Username: " + usernames[i]);
                        Console.WriteLine(reader.ReadToEnd());

                        stream.Seek(0, SeekOrigin.Begin);
                        content = reader.ReadToEnd().ToString();
                        Console.WriteLine(content);
                        Console.WriteLine("URL: https://api.bitbucket.org/2.0/users/" + usernames[i] + "\n");
                    }
                }
                System.Threading.Thread.Sleep(5000);
            }
        }
    }
}