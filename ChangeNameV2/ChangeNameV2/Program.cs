using System;
using System.IO;

namespace ChangeNameV2
{
    class Program
    {
        int[] numFilter;
        bool needFilter = false;
        static void Main()
        {
            try
            {
                Program program = new Program();

                // numbers that will be filtered
                string filterNumbers = null;

                string fileType;

                Console.WriteLine("Read the \"ReadME\" file to know which numbers this filters");

            START:;

                Console.Write("Enter a Path: ");
                string usersPath = Console.ReadLine();

                // 
                if (usersPath == "")
                    goto START;
                if (usersPath[^1] != '\\')
                    usersPath += '\\';
                if (usersPath == @"C:\" || usersPath == @"C:\Program Files (x86)\" || usersPath == @"C:\Program Files\" || usersPath == @"C:\Windows\")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Important folder\nDon't touch");
                    Console.ResetColor();
                    goto START;
                }

                //get the directory info now to  check if there is a path
                DirectoryInfo directoryInfo = new DirectoryInfo(usersPath);
                FileInfo[] infos = directoryInfo.GetFiles();

                Console.WriteLine("The episode number is first number or the Season number (press - first - f ,anything else)");

                string numberSide = Console.ReadLine();

                // The User's choice to answer
                string filterAnswer;

                Console.WriteLine("Do You Want To Add numbers to filter? (Y,N)");
                filterAnswer =Console.ReadLine();
                if (filterAnswer.ToLower() != "n")
                {
                    filterNumbers = program.FilterInterface();   
                    
                    if (filterNumbers == null)
                    {
                        goto AFTERFILTER;
                    }
                    string[] stringToFilter = filterNumbers.Split(" ");
                    int[] numbersToFilter = new int[stringToFilter.Length - 1];
                    for (int i = 0; i < stringToFilter.Length - 1; i++)
                        numbersToFilter[i] = Convert.ToInt32(stringToFilter[i]);

                    program.needFilter = true;
                    program.numFilter = numbersToFilter;
                }
            AFTERFILTER:;

                Console.Clear();

                //get all the files info from the folder
                foreach (FileInfo fileInfo in infos)
                {
                    if (!program.IsFileLocked(fileInfo))
                    {
                        //filter this files, no need to rename them
                        if (fileInfo.Name == "desktop.ini" || fileInfo.Name == "icon.ico")
                            goto END;
                        string seriesName = fileInfo.Directory.Parent.Name;
                        string NewPath;
                        string finalName;
                        string SeasonNum;

                        int numberFromTheString;
                        fileType = '.' + fileInfo.Name.Split('.')[^1];

                        string seasonWithNumber = fileInfo.Directory.Name;
                        string[] seasonAndNumberSplited = seasonWithNumber.Split(' ');

                        //if the parent folder is season then there is no need for
                        switch (seasonAndNumberSplited[0].ToLower())
                        {
                            case "season":
                                try
                                {
                                    numberFromTheString = program.GetNumberOutOfString(fileInfo.Name, fileType, numberSide);
                                    SeasonNum = seasonAndNumberSplited[1];

                                    if (numberFromTheString / 10 < 1)
                                    {
                                        if (seasonAndNumberSplited[1] == "00" || seasonAndNumberSplited[1].ToLower() == "specials")
                                            finalName = "S" + SeasonNum + "E0" + numberFromTheString.ToString();
                                        else if (Convert.ToInt32(seasonAndNumberSplited[1]) < 9)
                                            finalName = "S0" + SeasonNum + "E0" + numberFromTheString.ToString();
                                        else
                                            finalName = "S" + SeasonNum + "E0" + numberFromTheString.ToString();
                                    }
                                    else
                                    {
                                        if (seasonAndNumberSplited[1] == "00" || seasonAndNumberSplited[1].ToLower() == "specials")
                                            finalName = "S" + SeasonNum + "E" + numberFromTheString.ToString();
                                        else if (Convert.ToInt32(seasonAndNumberSplited[1]) < 9)
                                            finalName = "S0" + SeasonNum + "E" + numberFromTheString.ToString();
                                        else
                                            finalName = "S" + SeasonNum + "E" + numberFromTheString.ToString();
                                    }

                                    finalName = seriesName + " - " + finalName;
                                    File.Move(fileInfo.FullName, usersPath + finalName + fileType);

                                    Console.WriteLine("{0} Complete \\\\ {1}", numberFromTheString.ToString(), fileInfo.Name);
                                    break;
                                }
                                catch (IOException)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    NewPath = fileInfo.DirectoryName + '\\' + "-[NameAlreadyExist]-" + '\\';

                                    if (!Directory.Exists(NewPath))
                                    {
                                        Directory.CreateDirectory(NewPath);
                                        Console.WriteLine("Execption Folder Created");
                                    }

                                    File.Move(fileInfo.FullName, NewPath + fileInfo.Name);
                                    Console.WriteLine(fileInfo.Name + " Already exist");
                                    Console.ResetColor();
                                    break;
                                }
                            default:
                                try
                                {
                                    numberFromTheString = program.GetNumberOutOfString(fileInfo.Name, fileType, numberSide);

                                    //12 episodes a season setted by me
                                    SeasonNum = ((numberFromTheString / 13) + 1).ToString();

                                    if (numberFromTheString / 10 < 1)
                                    {
                                        if (Convert.ToInt32(SeasonNum) < 9)
                                            finalName = "S0" + SeasonNum + "E0" + numberFromTheString.ToString();
                                        else
                                            finalName = "S" + SeasonNum + "E0" + numberFromTheString.ToString();
                                    }
                                    else
                                    {
                                        if (Convert.ToInt32(SeasonNum) < 9)
                                            finalName = "S0" + SeasonNum + "E" + numberFromTheString.ToString();
                                        else
                                            finalName = "S" + SeasonNum + "E" + numberFromTheString.ToString();
                                    }

                                    NewPath = usersPath + "Season " + SeasonNum + '\\';
                                    finalName = seriesName + " - " + finalName;

                                    if (!Directory.Exists(NewPath))
                                    {
                                        Directory.CreateDirectory(NewPath);
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine("Season " + SeasonNum + " folder Created");
                                        Console.ResetColor();
                                    }

                                    File.Move(fileInfo.FullName, NewPath + finalName + fileType);
                                    Console.WriteLine("{0} Complete - {1}", numberFromTheString.ToString(), fileInfo.Name);
                                    Console.WriteLine();
                                    break;
                                }
                                catch (IOException)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    NewPath = fileInfo.DirectoryName + '\\' + "-[NameAlreadyExist]-" + '\\';

                                    if (!Directory.Exists(NewPath))
                                    {
                                        Directory.CreateDirectory(NewPath);
                                        Console.WriteLine("Execption Folder Created");
                                    }

                                    File.Move(fileInfo.FullName, NewPath + fileInfo.Name);
                                    Console.WriteLine(fileInfo.Name + " Already exist");
                                    Console.ResetColor();
                                    break;
                                }
                        }
                    }
                    else
                    {
                        Console.WriteLine("{0} is being used", fileInfo.Name);
                    }
                END:;
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

        public int GetNumberOutOfString(string File_name, string file_type, string Side)
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
                            if (Side == "f")
                                return Convert.ToInt32(numbers);
                            if (numbers + file_type == File_name)
                            {
                                converted = Convert.ToInt32(numbers);
                                number_holder = 0;
                                goto END;
                                //if file is just a number then returns that number
                            }
                            if (numbers == "0")
                                number_holder = 0;
                            if (needFilter)
                            {
                                for (int i = 0; i < numFilter.Length; i++)
                                {
                                    if (numFilter[i] == Convert.ToInt32(numbers))
                                    {
                                        if (number_holder == 0)
                                        {
                                            goto END;
                                        }
                                        number_holder = Convert.ToInt32(numbers);
                                        goto END;
                                    }
                                }
                            }
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
                                case "1920":
                                case "2160":
                                case "2010":
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

        private string FilterInterface()
        {
            string filterNumbers = null;

            while (true)
            {
                Console.WriteLine("What number to filter? Leave blank or type n to close");
                string filterNumbersHelper = Console.ReadLine();
                if (filterNumbersHelper == "" || filterNumbersHelper.ToLower() == "n")
                    return filterNumbers;

                filterNumbers += filterNumbersHelper + ' ';
            }
        }

        protected virtual bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }
    }
}