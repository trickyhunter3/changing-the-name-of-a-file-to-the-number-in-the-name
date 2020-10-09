using System;
using System.IO;

namespace try_with_file_changing
{
    class Program
    {
        static void Main()
        {
            Program pg = new Program();
            string a = @"D:\Anime\Ajin\Season_1\";
            DirectoryInfo d = new DirectoryInfo(@"D:\Anime\Ajin\Season_1\");
            FileInfo[] infos = d.GetFiles();
            pg.GetNumberOutOfString(infos[0].FullName);
            foreach (FileInfo f in infos)
            {
                int num = pg.GetNumberOutOfString(f.FullName);
                File.Move(f.FullName, a + num.ToString() + ".MKV");
            }
        }
        public int GetNumberOutOfString(string name)
        {
            int i = 0;
            int[] converted = new int[3];
            int n = 0;
            string numbers = null;
            string names = name.Substring(23);
            for (int j = 0; j < names.Length; j++) 
            {
                switch (names[j])
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
                        n++;
                        if(n >= 4)
                        {
                            n = 0;
                            goto END;
                        }
                        numbers += names[j];
                    END:;
                        break;
                    default:
                        if(n < 4 && n != 0)
                        {
                           if(numbers != null)
                            {
                                converted[i] = Convert.ToInt32(numbers);
                                if(converted[i] ==  720 || converted[i] == 1920)
                                {
                                    converted[i] = 0;
                                    i--;
                                }
                                numbers = null;
                                i++;
                            } 
                        }
                        n = 0;
                        break;
                }
            }
            return converted[0] + converted[1] + converted[2];
        }
    }
}