﻿using System;
using System.IO;

namespace ChangeNameV2
{
    class Program
    {
        static void Main()
        {
            try
            {
                string file_type;
                Program pg = new Program();
                Console.WriteLine("640p 720p 1080p 2160p , if the file has different resolution on the name\n" +
                    "then change a bit the source code so it will filter it\n");
            START:;
                Console.Write("Enter a Path: ");
                string path = Console.ReadLine();
                if (path == "")
                {
                    goto START;
                }
                if (path[^1] != '\\')
                    path += '\\';
                Console.Clear();
                DirectoryInfo d = new DirectoryInfo(path);
                FileInfo[] infos = d.GetFiles();
                //get all the file info from the folder
                foreach (FileInfo f in infos)
                {
                    string SeasonName;
                    string SeasonNum;

                    int num;
                    string[] splitedbydotes = f.FullName.Split('.');

                    string[] splitedbyslash = f.Directory.FullName.Split('\\');
                    string[] splitedbyspace = splitedbyslash[^1].Split(' ');

                    //if the parent folder is season then there is no need for
                    switch (splitedbyspace[0].ToLower())
                    {
                        case "season":
                            file_type = '.' + splitedbydotes[^1];
                            num = pg.GetNumberOutOfString(f.Name, file_type);
                            SeasonNum = splitedbyspace[1];
                            if (num / 10 < 1)
                            {
                                if (splitedbyspace[1] == "00" || splitedbyspace[1].ToLower() == "specials")
                                {
                                    SeasonName = "S" + SeasonNum + "E0" + num.ToString();
                                }
                                else if (Convert.ToInt32(splitedbyspace[1]) < 9)
                                {
                                    SeasonName = "S0" + SeasonNum + "E0" + num.ToString();
                                }
                                else
                                {
                                    SeasonName = "S" + SeasonNum + "E0" + num.ToString();
                                }
                            }
                            else
                            {
                                if (splitedbyspace[1] == "00" || splitedbyspace[1].ToLower() == "specials")
                                {
                                    SeasonName = "S" + SeasonNum + "E" + num.ToString();
                                }
                                else if (Convert.ToInt32(splitedbyspace[1]) < 9)
                                    SeasonName = "S0" + SeasonNum + "E" + num.ToString();
                                else
                                    SeasonName = "S" + SeasonNum + "E" + num.ToString();
                            }
                            File.Move(f.FullName, path + SeasonName + file_type);
                            Console.WriteLine(num.ToString() + " Complete");
                            break;
                        default:
                            file_type = '.' + splitedbydotes[^1];
                            num = pg.GetNumberOutOfString(f.Name, file_type);
                            SeasonNum = (num / 50 + 1).ToString();
                            if (num / 10 < 1)
                            {
                                if (Convert.ToInt32(SeasonNum) < 9)
                                    SeasonName = "S0" + SeasonNum + "E0" + num.ToString();
                                else
                                    SeasonName = "S" + SeasonNum + "E0" + num.ToString();
                            }
                            else
                            {
                                if (Convert.ToInt32(SeasonNum) < 9)
                                    SeasonName = "S0" + SeasonNum + "E" + num.ToString();
                                else
                                    SeasonName = "S" + SeasonNum + "E" + num.ToString();
                            }
                            string NewPath = path + "Season " + SeasonNum + '\\';
                            if (!Directory.Exists(NewPath))
                            {
                                Directory.CreateDirectory(NewPath);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Season " + SeasonNum + " folder Created");
                                Console.ResetColor();
                            }
                            File.Move(f.FullName, NewPath + SeasonName + file_type);
                            Console.WriteLine(num.ToString() + " Complete");
                            Console.WriteLine();
                            break;
                    }
                }

                Console.ReadLine();
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Directory not found");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }
        }
        public int GetNumberOutOfString(string File_name, string file_type)
        {
            // j is current index of the file_name
            int converted = 0;
            //if we find a number that is episode then i++ happen so we save the episode number and 
            //on the next run when it find a season number or resoulution number it will go to 0 on the next int not on the
            //episode number itself
            int numbers_together = 0;
            //when he find number he start to count so that it won't check if statment IF he is not at least 1 number
            int number_holder = -1;
            //hold a number if it's the only number then 
            string numbers = null;
            for (int j = 0; j < File_name.Length; j++)
            {
                switch (File_name[j])
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        numbers_together++;
                        numbers += File_name[j];
                        //start recording the numbers if they are found
                        break;
                    default:
                        if (numbers_together != 0)
                        {
                            if (numbers + file_type == File_name)
                            {
                                converted = Convert.ToInt32(numbers);
                                number_holder = 0;
                                goto END;
                                //if file is just a number then returns that number
                            }
                            if (numbers == "0")
                                number_holder = 0;
                            switch (numbers)
                            {
                                case "1":
                                case "2":
                                case "3":
                                case "4":
                                case "5":
                                case "6":
                                case "7":
                                case "8":
                                case "9":
                                case "640":
                                case "720":
                                case "1080":
                                case "2160":
                                    if (number_holder == 0)
                                    {
                                        goto END;
                                    }
                                    number_holder = Convert.ToInt32(numbers);
                                    goto END;
                            }
                            converted = Convert.ToInt32(numbers);
                        END:;
                            numbers = null;
                        }
                        numbers_together = 0;
                        break;
                }
            }
            if (converted + number_holder == number_holder)
                return number_holder;
            //converted + num = num that means that the season or resolution filter worked but was not necessery
            return converted;
        }
    }
}