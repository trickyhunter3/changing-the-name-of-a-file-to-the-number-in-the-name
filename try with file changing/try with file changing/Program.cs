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
            Console.Write("Enter a Path: ");
            string path = Console.ReadLine();
            if (path[path.Length - 1] != '\\')
                path += '\\';
            DirectoryInfo d = new DirectoryInfo(path);
            FileInfo[] infos = d.GetFiles();
            foreach (FileInfo f in infos)
            {
                string[] splitedbydotes = f.FullName.Split('.');
                file_type = '.' + splitedbydotes[splitedbydotes.Length -1];
                string num = pg.GetNumberOutOfString(f.Name, file_type).ToString();
                if(f.FullName != path + num.ToString() + file_type)
                    File.Move(f.FullName, path + num + file_type);
            }
        }
        public int GetNumberOutOfString(string File_name, string file_type)
        {
            int i = 0;
            int[] converted = new int[2];
            int numbers_together = 0;
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
                        break;
                    default:
                        if(numbers_together < 4 && numbers_together != 0)
                        {
                           if(numbers != null)
                            {
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
                                        if(numbers + file_type == File_name)
                                        {
                                            converted[i] = Convert.ToInt32(numbers);
                                            goto pont;
                                        }
                                        converted[i] = 0;
                                        i--;
                                        goto pont;
                                }
                                converted[i] = Convert.ToInt32(numbers);
                                switch (converted[i])
                                {
                                    case 720:
                                    case 1080:
                                    case 640:
                                    case 2160:
                                        converted[i] = 0;
                                        i--;
                                        break;
                                }
                            pont:;
                                numbers = null;
                                i++;
                            } 
                        }
                        numbers_together = 0;
                        break;
                }
            }
            return converted[0] + converted[1];
        }
    }
}