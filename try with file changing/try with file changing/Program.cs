using System;
using System.Drawing;
using System.IO;
using System.Net.NetworkInformation;

namespace try_with_file_changing
{
    class Program
    {
        static void Main()
        {
            string file_type;
            Program pg = new Program();
            Console.WriteLine("640p 720p 1080p 2160p if the file has different resolution on the name\nthen change a bit the source code so it will filter it\n");
            Console.Write("Enter a Path: ");
            string path = Console.ReadLine();
            if (path[path.Length - 1] != '\\')
                path += '\\';
            DirectoryInfo d = new DirectoryInfo(path);
            FileInfo[] infos = d.GetFiles();
            //get all the file info from the folder
            foreach (FileInfo f in infos)
            {
                string[] splitedbydotes = f.FullName.Split('.');
                file_type = '.' + splitedbydotes[splitedbydotes.Length -1];
                string num = pg.GetNumberOutOfString(f.Name, file_type).ToString();
                if (f.FullName != path + num.ToString() + file_type)
                    File.Move(f.FullName, path + num + file_type);
            }
        }
        public int GetNumberOutOfString(string File_name, string file_type)
        {
            // j is current index of the file_name
            int i = 0;
            int[] converted = new int[2];
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
                        if(numbers_together != 0)
                        {
                            if (numbers + file_type == File_name)
                            {
                                converted[i] = Convert.ToInt32(numbers);
                                goto END;
                                //if file is just a number then it just returns that number in the end
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
                                    case "2160":
                                        number_holder = Convert.ToInt32(numbers);
                                        converted[i] = 0;
                                        i--;
                                        goto END;
                                }
                            converted[i] = Convert.ToInt32(numbers);
                        END:;
                                numbers = null;
                                i++;
                        }
                        numbers_together = 0;
                        break;
                }
            }
            if (converted[0] + converted[1] + number_holder == number_holder)
                converted[0] = number_holder;
            return converted[0] + converted[1];
        }
    }
}