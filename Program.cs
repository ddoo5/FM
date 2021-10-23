using System;
using System.IO;

namespace FM_solution
{
    class MainClass                             //заранее скажу недочет, не смог сделать вывод каталогов по страницам
    {                                           //при эксплуатации советую просто закрыть код и наслаждаться процессом
        public static void Main(string[] args)
        {
            dir_file open = new dir_file();     // тут вход классов, они описаны ниже, под методом while(true)
            Help helP = new Help();
            error eror = new error();
            delite del = new delite();
            getinfo info = new getinfo();
            copy cop = new copy();

            string userenter = "";

            while (true) 
            {
                try     // основной метод
                {
                    userenter = Console.ReadLine();

                    for (int i = 0; i < 1; i++)
                    {
                        switch (userenter[i])
                        {
                            case 'h':
                                helP.help(userenter);
                                break;
                            case 'c':
                                cop.Fcopy(userenter);
                                break;
                            case 'd':
                                del.delete(userenter);
                                break;
                            case 'i':
                                info.FD(userenter);
                                break;
                            case 'w':
                                open.DF(userenter);
                                info.FD(userenter);
                                break;
                            default:       // сохраняет ошибки пользователя в файл errors.txt
                                Console.WriteLine(eror.A(userenter));
                                break;
                        }
                    }
                }

                catch(Exception a)              // не дает системе упасть + сохраняет ошибки в файл system_errors.txt
                {
                    eror.B(Convert.ToString(a));
                }
            }
        }
    }
    class Help             // класс help, для вывода команды h 
    {
        public string help(string a)
        {
            Console.WriteLine("h » help");
            Console.WriteLine("i f » information about file(ex.'i f /Users/mac/test/test.xml')");
            Console.WriteLine("i dir » information about file(ex.'i dir /Users/mac/test')");
            Console.WriteLine("c f » copy file(ex.'c f /Users/mac/test.txt /Users/mac/test2.txt')");
            Console.WriteLine("c dir » copy directory(ex.'c dir /Users/mac/test /Users/mac/test1(test1 will be new directory)')");
            Console.WriteLine("d f » delete file(ex.'d f /Users/mac/test/test.txt')");
            Console.WriteLine("d dir » delete directory(ex.'d dir /Users/mac/test')");
            Console.WriteLine("w » use to see 'tree' of files(ex.'w /Users/mac')");
            return a;
        }
    }
    class UI              // класс, который рисует линию
    {
        public string DrawLine(string a)
        {
            for (int i = 0; i < 20; i++)
            {
                Console.Write(a);
            }
            Console.WriteLine("");
            return a;
        }
    }
    class info           // класс преобразования информации
    {
        UI line = new UI();

        public string InfoD(string a)
        {
            if (Directory.Exists(a))   // вывод информации о каталоге
            {
                DirectoryInfo info = new DirectoryInfo(a);
                line.DrawLine("-");
                Console.WriteLine($"name: {info.Name}");
                Console.WriteLine($"last access time: {info.LastAccessTime}");
                Console.WriteLine($"creation time: {info.CreationTime}");
                Console.WriteLine($"root: {info.Root}");
                Console.WriteLine($"attributes: {info.Attributes}");
                line.DrawLine("-");
            }
            return a;
        }
        public string InfoF(string a)
        {
            FileInfo file = new FileInfo(a);  
            if (file.Exists)       // вывод информации о файле
            {
                line.DrawLine("-");
                Console.WriteLine($"name: {file.Name}");
                Console.WriteLine($"directory: {file.DirectoryName}");
                Console.WriteLine($"creation time: {file.CreationTime}");
                Console.WriteLine($"last edit: {file.LastWriteTime}");
                Console.WriteLine($"size in bytes: {file.Length}");
                Console.WriteLine($"file exists: {file.Exists}");
                Console.WriteLine($"file format: {file.Extension}");
                Console.WriteLine($"last enter: {file.LastAccessTime}");
                line.DrawLine("-");
            }
            return a;
        }
    }
    class dir_file      // класс, который выводит каталоги с файлами
    {
        UI line = new UI();
        info GetInfo = new info();
        error eror = new error();

        char[] dels = { 'w', ' ' };
        string c = "";

        public string DF(string a)
        {
            Console.Clear();
            c = a.TrimStart(dels);

            if (Directory.Exists(c))     
            {
                string[] b = Directory.GetDirectories(c);
                string[] d = Directory.GetFiles(c);

                for (int i = 0; i < b.Length; i++)  
                {
                    Console.WriteLine($"║ {b[i]}");     // вывод каталога

                    for (int s = 0; s < d.Length; s++)
                    {
                        Console.WriteLine($" ╚{ d[s]}");  // вывод файла
                    }
                }

                GetInfo.InfoD(c);  // вывод информации о каталоге
            }

            else  // сохранение опечатки пользователя
            {
                Console.WriteLine(eror.D(c));
            }

            return a;
        }
    }
    class copy         // класс копирования файлов и директорий
    {
        error eror = new error();

        char[] f = {'c',' ', 'f', ' ' };
        char[] dir = {'c',' ', 'd', 'i', 'r', ' ' };
        string b = "";

        public string Fcopy(string a)  //копирование файла
        {
            if (a[2] == 'f')
            {
                b = a.TrimStart(f);
                string[] file = b.Split(' ');
                if(File.Exists(file[0]) == File.Exists(file[1]))
                {
                    File.Copy(file[0], file[1], true);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("file copied");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(eror.A(a));
                }
            }
            else if (a[2] == 'd')
            {
                if (a[3] == 'i')
                {
                    if (a[4] == 'r')       // копирование каталога   
                    {
                        b = a.TrimStart(dir);
                        string[] dir1 = b.Split(' ');
                        DirectoryInfo dir2 = new DirectoryInfo(dir1[0]);
                        if (dir2.Exists && Directory.Exists(dir1[1]) == false)
                        {
                            dir2.MoveTo(dir1[1]);

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("directory copied");
                            Console.ResetColor();
                        }
                        else
                        {
                            eror.D(a);
                        }
                    }
                    else  //сохранение опечатки пользователя
                    {
                        eror.A(a);
                    }
                }
                else    //сохранение опечатки пользователя
                {
                    eror.A(a);
                }
            }
            else    //сохранение опечатки пользователя
            {
                eror.A(a);
            }


            return a;
        }
    }
    class delite      // класс удаления файлов и каталогов
    {
        error eror = new error();

        char[] d = { ' ', 'd', 'i', 'r', ' '};
        char[] f = { 'd', ' ', 'f', ' '};
        string check = "";

        public string delete(string a)
        {
            if (a[2] == 'f')       //удаление файла
            {
                check = a.TrimStart(f);
                FileInfo file = new FileInfo(check);

                if (file.Exists)
                {
                    file.Delete();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("file deleted");
                    Console.ResetColor();

                }

                else    //сохранение ошибки о не найденом файле
                {
                    eror.D(check);
                }

            }
            else if (a[2] == 'd')
            {
                if (a[3] == 'i')
                {
                    if (a[4] == 'r')          //удаление каталога
                    {
                        check = a.TrimStart(d);
                        if (Directory.Exists(check))
                        {
                            DirectoryInfo info = new DirectoryInfo(check);
                            info.Delete(true);

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("directory deleted");
                            Console.ResetColor();
                        }
                        else      //сохранение ошибки о не найденом каталоге
                        {
                            Console.WriteLine(eror.D(check));
                        }
                    }
                    else  //сохранение опечатки пользователя
                    {
                        Console.WriteLine(eror.A(a));
                    }
                }
                else    //сохранение опечатки пользователя
                {
                    Console.WriteLine(eror.A(a));
                }
            }
            else   //сохранение опечатки пользователя
            {
                Console.WriteLine(eror.A(a));
            }
            return a;
        } 
    }
    class error      // класс сохранения ошибок
    {
        string errorcollect = "errors.txt";
        string serrorcollect = "system_errors.txt";
        string time = Convert.ToString(DateTime.Now);
        string forcatch = "";
        string errorenter = "Incorrect format, use 'h' for help";
        string nfound = "not found";

        public string A(string a)     //сохранение опечаток пользователя
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errorenter);
            Console.ResetColor();

            forcatch = errorenter + " " + time;
            File.AppendAllText(errorcollect, forcatch);
            File.AppendAllText(errorcollect, Environment.NewLine);

            return a;
        }
        public string B(string a)    //сохранение ошибок системы
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {a}");
            Console.ResetColor();
            
            forcatch = a + time;
            File.AppendAllText(serrorcollect, forcatch);
            File.AppendAllText(serrorcollect, Environment.NewLine);

            return a;
        }
        public string D(string a)    //сохранение ошибок при поиске директорий/файлов
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{a} {nfound}");
            Console.ResetColor();

            forcatch = a + " " + nfound + " " + time;
            File.AppendAllText(errorcollect, forcatch);
            File.AppendAllText(errorcollect, Environment.NewLine);

            return a;
        }
    }
    class getinfo   //класс вывода информации о файле/каталогеc dir /Users/mac/test2 /Users/mac/test3
    {
        info GetInfo = new info();
        error eror = new error();

        char[] f = {'i',' ','f',' '};
        char[] dir = {'i',' ','d','i','r',' '};
        string b = "";

        public string FD(string a)
        {
            if (a[2] == 'f')
            {
                b = a.TrimStart(f);

                if (File.Exists(b))     //вывод информации о файле
                {
                    GetInfo.InfoF(b);
                }
                else     //если файл не найден, то ошибка сохраняется
                {
                    Console.WriteLine(eror.D(b));
                }
            }
            else if (a[2] == 'd')
            {
                if (a[3] == 'i')
                {
                    if (a[4] == 'r')          //вывод информации о каталоге
                    {
                        b = a.TrimStart(dir);

                        if (Directory.Exists(b))
                        {
                            GetInfo.InfoD(b);
                        }
                        else     //если каталог не найден, то ошибка сохраняется
                        {
                            Console.WriteLine(eror.D(b));
                        }
                    }
                    else     
                    {
                        Console.WriteLine(eror.A(b));
                    }
                }
                else
                {
                    Console.WriteLine(eror.A(b));
                }
            }
            else
            {
                Console.WriteLine(eror.A(b));
            }

            return a;
        }
    }
}
